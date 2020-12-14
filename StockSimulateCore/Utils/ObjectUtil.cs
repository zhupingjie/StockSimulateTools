using ServiceStack.Text;
using StockSimulateCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Utils
{
    public class ObjectUtil
    {
        public static DataTable ConvertTable<TEntity>(TEntity[] entitys) where TEntity : BaseEntity
        {
            var dt = new DataTable();
            var preps = typeof(TEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.Name == "ID") continue;

                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null)
                {
                    dt.Columns.Add(attr.Description);
                }
            }
            foreach(var entity in entitys)
            {
                var dr = dt.NewRow();
                foreach (var prep in preps)
                {
                    if (prep.Name == "ID") continue;

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
                    return TypeSerializer.DeserializeFromString<T>($"{val}");
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

        public static T ToValue<T>(object value, T defaultValue)
        {
            try
            {
                if (value == null) return defaultValue;
                var val = $"{value}".Trim();
                if (String.IsNullOrEmpty(val)) return defaultValue;

                return TypeSerializer.DeserializeFromString<T>(val);
            }
            catch (Exception ex)
            {
                throw new Exception($"数据[{value}]转换为类型[{typeof(T).Name}]错误:{ex.Message}");
            }
        }

        public static object ToValue(object value, Type type)
        {
            try
            {
                if (value == null) return null;
                var val = $"{value}".Trim();

                return TypeSerializer.DeserializeFromString(val, type);
            }
            catch (Exception ex)
            {
                throw new Exception($"数据[{value}]转换为类型[{type.Name}]错误:{ex.Message}");
            }
        }
    }
}
