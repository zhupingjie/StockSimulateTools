using Microsoft.Win32;
using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Utils
{
    public class ObjectUtil
    {
        public static EntityPropertyInfo[] GetGridDataColumns<TEntity>(bool withId = true) where TEntity : BaseEntity
        {
            var columns = new List<EntityPropertyInfo>();
            var preps = typeof(TEntity).GetProperties();
            if (withId)
            {
                columns.Add(new EntityPropertyInfo()
                {
                    Name = "ID",
                    Description = "序"
                });
            }
            foreach (var prep in preps)
            {
                if (prep.Name == "ID") continue;
                if (prep.GetCustomAttributes(typeof(GridColumnIgnoreAttribute), true).Length > 0) continue;

                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null)
                {
                    columns.Add(new EntityPropertyInfo()
                    {
                        Name = prep.Name,
                        Description = attr.Description,
                        Type = prep.PropertyType
                    });
                }
            }
            return columns.ToArray();
        }
        public static DataTable ConvertTable<TEntity>(TEntity[] entitys, bool withId = false) where TEntity : BaseEntity
        {
            var dt = new DataTable();
            var preps = typeof(TEntity).GetProperties();
            if (withId)
            {
                dt.Columns.Add("序");
            }
            foreach(var prep in preps)
            {
                if (prep.Name == "ID") continue;
                if (prep.GetCustomAttributes(typeof(GridColumnIgnoreAttribute), true).Length > 0) continue;

                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null)
                {
                    dt.Columns.Add(attr.Description);
                }
            }
            foreach(var entity in entitys)
            {
                var dr = dt.NewRow();
                if (withId)
                {
                    dr["序"] = GetPropertyValue(entity, "ID");
                }
                foreach (var prep in preps)
                {
                    if (prep.Name == "ID") continue;
                    if (prep.GetCustomAttributes(typeof(GridColumnIgnoreAttribute), true).Length > 0) continue;

                    var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                    if(attr != null)
                    {
                        dr[attr.Description] = GetPropertyValue(entity, prep.Name);
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static string GetEntityTypeName<TEntity>() where TEntity : BaseEntity
        {
            return GetEntityTypeName(typeof(TEntity));
        }
        public static string GetEntityTypeName(Type type)
        {
            return type.Name.Replace("Entity", "");
        }
        public static Type[] FindEntityTypes()
        {
            var entityTypes = new List<Type>();

            var assmblies = LoadAssemblies(AppDomain.CurrentDomain.BaseDirectory, x => x.Name.Contains("StockSimulate"));
            foreach (var assbm in assmblies)
            {
                foreach (var type in assbm.GetTypes())
                {
                    if (typeof(BaseEntity).IsAssignableFrom(type) && !type.Equals(typeof(BaseEntity)))
                    {
                        entityTypes.Add(type);
                    }
                }
            }
            return entityTypes.ToArray();
        }

        public static IList<Assembly> LoadAssemblies(string folderName, Func<FileInfo, bool> filter = null)
        {
            if (filter == null) filter = f => f.Length > 0;

            var pathInfo = new DirectoryInfo(folderName);
            return pathInfo.GetFiles("*.dll", SearchOption.AllDirectories)
                           .Where(filter)
                           .Select(f => Assembly.LoadFrom(f.FullName))
                           .ToList();
        }

        public static DBColumn GetDBColumnType(string name, Type type)
        {
            var colType = "nvarchar";
            var length = 0;
            var decLength = 0;
            if (type.Name.Contains("String"))
            {
                colType = "nvarchar";
                length = 200;
            }
            else if (type.Name.Contains("Decimal"))
            {
                colType = "decimal";
                length = 18;
                decLength = 2;
            }
            else if (type.Name.Contains("DateTime"))
            {
                colType = "datetime";
            }
            else if (TypeUtil.IsNullableType(type) && TypeUtil.GetNullableType(type) == typeof(DateTime))
            {
                colType = "datetime";
            }
            if (type.Name.Contains("Int"))
            {
                colType = "int";
                length = 11;
            }
            if (type.Name.Contains("Bool"))
            {
                colType = "bool";
            }

            return new DBColumn(name, colType, length, decLength);
        }

        public static DateTime StampToDateTime(long timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddSeconds(timeStamp).AddHours(8);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) return null;
            var propertyInfos = obj.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.ToLower() == propertyName.ToLower())
                {
                    return propertyInfo.GetValue(obj);
                }
            }
            return null;
        }

        public static T GetPropertyValue<T>(object obj, string field)
        {
            if (obj == null) return default(T);
            var propertyInfos = obj.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.ToLower() == field.ToLower())
                {
                    var val = propertyInfo.GetValue(obj);
                    if (val == null) return default(T);
                    return ToValue<T>(val);
                }
            }
            return default(T);
        }

        public static string GetPropertyDesc(Type type, string field)
        {
            var prep = type.GetProperties().FirstOrDefault(c => c.Name == field);
            if (prep == null) return field;

            var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (attr == null) return field;

            return (attr as DescriptionAttribute).Description;
        }

        public static Dictionary<string, object> GetPropertyValues(object obj, bool setKeyWithDesc = false)
        {
            var result = new Dictionary<string, object>();
            var propertyInfos = obj.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetMethod.IsStatic) continue;

                var key = propertyInfo.Name;
                if (setKeyWithDesc)
                {
                    var descAttrs = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (descAttrs.Count() > 0)
                    {
                        key = (descAttrs.FirstOrDefault() as DescriptionAttribute).Description;
                    }
                }

                if (!result.ContainsKey(key))
                {
                    var value = propertyInfo.GetValue(obj);
                    if (propertyInfo.PropertyType.IsEnum || (IsNullableType(propertyInfo.PropertyType) && GetNullableType(propertyInfo.PropertyType).IsEnum))
                    {
                        result.Add(key, (int)value);
                    }
                    else
                    {
                        result.Add(key, value);
                    }
                }
            }
            return result;
        }

        public static PropertyInfo[] GetPropertyInfos(Type objType)
        {
            var pInfos = new List<PropertyInfo>();
            var types = objType.GetProperties();
            foreach(var type in types)
            {
                if (type.GetCustomAttributes(typeof(DBNotMappedAttribute), true).Length > 0) continue;

                pInfos.Add(type);
            }
            return pInfos.ToArray();
        }

        public static PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            if (obj == null) return null;
            return obj.GetType().GetProperties().FirstOrDefault(c => c.Name.ToUpper() == propertyName.ToUpper());
        }

        public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, propertyValue);
            }
        }

        public static bool IsNullableType(Type theType)
        {
            return Nullable.GetUnderlyingType(theType) != null;
        }

        public static Type GetNullableType(Type theType)
        {
            return Nullable.GetUnderlyingType(theType);
        }

        public static T ToValue<T>(object value, T defaultValue = default(T))
        {
            var obj = ToValue(value, typeof(T));
            if (obj == null) return defaultValue;

            return (T)obj;
        }

        public static object ToValue(object value, Type type)
        {
            try
            {
                if (value == null) return null;
                var val = $"{value}".Trim();

                if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
                if (value == null) return null;
                if (type == value.GetType()) return value;
                if (type.IsEnum)
                {
                    if (value is string)
                        return Enum.Parse(type, value as string);
                    else
                        return Enum.ToObject(type, value);
                }
                if (!type.IsInterface && type.IsGenericType)
                {
                    Type innerType = type.GetGenericArguments()[0];
                    object innerValue = ToValue(value, innerType);
                    return Activator.CreateInstance(type, new object[] { innerValue });
                }
                if (value is string && type == typeof(Guid)) return new Guid(value as string);
                if (value is string && type == typeof(Version)) return new Version(value as string);
                if (!(value is IConvertible)) return value;
                return Convert.ChangeType(value, type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool EffectStockDealTime(int offsetMinute = 5)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday) return false;

            var from = DateTime.Now.AddMinutes(offsetMinute).ToString("HH:mm");
            var to = DateTime.Now.AddMinutes(-1 * offsetMinute).ToString("HH:mm");
            if (from.CompareTo("09:15") >= 0 && to.CompareTo("11:30") < 0) return true;
            if (from.CompareTo("13:00") >= 0 && to.CompareTo("15:00") < 0) return true;
            return false;
        }

        public static bool EffectStockMacdTime(int offsetMinute = 0)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday) return false;

            var from = DateTime.Now.AddMinutes(offsetMinute).ToString("HH:mm");
            var to = DateTime.Now.AddMinutes(-1 * offsetMinute).ToString("HH:mm");
            if (from.CompareTo("09:45") >= 0 && to.CompareTo("10:00") < 0) return true;
            if (from.CompareTo("11:15") >= 0 && to.CompareTo("11:20") < 0) return true;
            if (from.CompareTo("13:45") >= 0 && to.CompareTo("13:50") < 0) return true;
            if (from.CompareTo("14:45") >= 0 && to.CompareTo("14:50") < 0) return true;
            return false;
        }

        public static string[] GetSplitArray(object text, string split)
        {
            if (text == null) return new string[] { };
            return $"{text}".Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static int GetGridColumnLength(string name = "")
        {
            var length = name.Length;
            if (name.Contains("(%)")) length = length - 2;
            else if (name.Contains("(PE)")) length = length - 4;
            else if (name.Contains("(TTM)")) length = length - 4;
            else if (name.Contains("(")) length = length - 1;
            return length <= 1 ? 30 : length <= 2 ? 50 : length <= 3 ? 55 : length <= 4 ? 70 : length <= 6 ? 90 : length <= 8 ? 100 : 120;
        }

        public static string FormatMoney(decimal value)
        {
            var mil = value / 100000000m;
            if (Math.Abs(mil) >= 1) return $"{Math.Round(mil, 2)}亿";

            var ten = value / 10000m;
            if (Math.Abs(ten) >= 1) return $"{Math.Round(ten, 2)}万";

            return $"{Math.Round(value, 2)}元";
        }

        public static void OpenBrowserUrl(string url)
        {
            try
            {
                var result = Process.Start("chrome.exe", url);
                if (result == null)
                {
                    OpenIe(url);
                }
            }
            catch
            {
                //出错调用IE
                OpenIe(url);
            }
        }

        /// <summary>
        /// 用IE打开浏览器
        /// </summary>
        /// <param name="url"></param>
        public static void OpenIe(string url)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "iexplore.exe";   //IE浏览器，可以更换
            process.StartInfo.Arguments = url;
            process.Start();
        }

        public static bool ColudGatherFinanceReport(string reportDate)
        {
            if (string.IsNullOrEmpty(reportDate)) return true;

            var date = DateTime.Now.Date;
            if (!DateTime.TryParse(reportDate, out date)) return false;

            //等待一季报
            if(date.Month == 12)
            {
                if (DateTime.Now.Date.Month != 4) return false;
            }
            else if(date.Month == 3)
            {
                if (DateTime.Now.Date.Month != 7 && DateTime.Now.Date.Month != 8) return false;
            }
            else if (date.Month == 6)
            {
                if (DateTime.Now.Date.Month != 10) return false;
            }
            else if (date.Month == 9)
            {
                if (DateTime.Now.Date.Month > 4) return false;
            }
            return true;
        }

        public static bool ColudGatherFundStock(string reportDate)
        {
            if (string.IsNullOrEmpty(reportDate)) return true;

            var date = DateTime.Now.Date;
            if (!DateTime.TryParse(reportDate, out date)) return false;

            if (date.Month == 12)
            {
                if (DateTime.Now.Date.Month < 3) return false;
            }
            else if (date.Month == 9)
            {
                if (DateTime.Now.Date.Month >= 9) return false;
            }
            else if (date.Month == 6)
            {
                if (DateTime.Now.Date.Month < 9) return false;
            }
            else if (date.Month == 3)
            {
                if (DateTime.Now.Date.Month < 6) return false;
            }
            return true;
        }

        public static string GetStockMarket(string code)
        {
            if (code.EndsWith("HK")) return "";
            if (code.StartsWith("5") || code.StartsWith("6")) return "SH";
            if (code.StartsWith("0") || code.StartsWith("1") || code.StartsWith("3")) return "SZ";
            return "";
        }

        public static int GetGatherQuarterNum(DateTime date)
        {
            if (date.Month >= 10 && date.Month <= 12) return 3;
            else if (date.Month >= 1 && date.Month <= 3) return 4;
            else if (date.Month >= 4 && date.Month <= 6) return 1;
            else return 2;
        }

        public static string GetGatherQuarterStr(DateTime date, int quarter)
        {
            var month = 0;
            if (quarter == 1) month = 3;
            if (quarter == 2) month = 6;
            if (quarter == 3) month = 9;
            if (quarter == 4) month = 12;
            return $"{DateTime.Now.Year}-{month.ToString().PadLeft(2, '0')}-30";
        }

        public static void CheckOrCreateDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static void DeleteDirectoryFiles(string path, Action<string> action)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                File.Delete(file);
                if (action != null)
                {
                    action(file);
                }
            }
        }

        public static void MoveDirectoryFiles(string sourcePath, string destPath, Action<FileInfo, string> action)
        {
            var files = Directory.GetFiles(sourcePath).Select(c => new FileInfo(c)).ToArray();
            foreach (var file in files)
            {
                var destFile = Path.Combine(destPath, file.Name);
                file.CopyTo(destFile, true);
                file.Delete();
                if (action != null)
                {
                    action(file, destFile);
                }
            }
        }

        public static bool CheckTrendReverse(string trend , bool up)
        {
            if (string.IsNullOrEmpty(trend)) return false;
            if (trend.Length < 2) return false;

            var first = "";
            var next = (up ? "上" : "下");
            for (int i = 0; i < trend.Length; i++)
            {
                var t = trend.Substring(i, 1);
                if (string.IsNullOrEmpty(first))
                {
                    first = t;
                    if (first != next) return false;
                }
                else
                {
                    if (t == "平") continue;
                    else if (t == next) return false;
                    else return true;
                }
            }
            return false;
        }
    }
}
