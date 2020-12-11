using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using StockPriceTools.Model;

namespace StockPriceTools.Utils
{
    public class SQLiteDBHelper
    {
        static SQLiteDBHelper()
        {
            
        }
        private SQLiteDBHelper()
        {

        }

        private static readonly SQLiteDBHelper instance = new SQLiteDBHelper();
        private static readonly string dbName = "../../SQliteDB/StockDB.db";
        private static readonly string strConn = $"Data Source={dbName}";
        public static SQLiteDBHelper Instance
        {
            get
            {
                return instance;
            }
        }

        public void InitSQLiteDB()
        {
            CreateDB(dbName);

            #region 买卖策略
            var columns = new List<SQLiteColumn>();
            columns.Add(new SQLiteColumn("Name", "nvarchar", 50));
            columns.Add(new SQLiteColumn("IncreasePer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("IncreaseAmount", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("MaxPositionPer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("IncreaseMorePer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("IncreaseMoreAmountPer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("IncreaseMaxPer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("IncreaseMaxAmountPer", "decimal", 18, 2));
            CreateTable("ExchangeStrategy", columns.ToArray());
            #endregion

            #region 股票基本面
            columns = new List<SQLiteColumn>();
            columns.Add(new SQLiteColumn("Code", "nvarchar", 50));
            columns.Add(new SQLiteColumn("Name", "nvarchar", 50));
            columns.Add(new SQLiteColumn("TotalCapital", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TotalPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("Price", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("PE", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TTM", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("PB", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("PEG", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("ROE", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("ROIC", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("EPS", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("BVPS", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TotalRevenue", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("NetProfit", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("RevenueGrewPer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("ProfitGrewPer", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("GrossRate", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("NetRate", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("DebtRage", "decimal", 18, 2));
            CreateTable("Stock", columns.ToArray());
            #endregion

            #region 股票收盘价

            columns = new List<SQLiteColumn>();
            columns.Add(new SQLiteColumn("Code", "nvarchar", 50));
            columns.Add(new SQLiteColumn("Price", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TodayStartPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TodayEndPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TodayMaxPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TodayMinPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("YesterdayEndPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("DealQty", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("DealPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TTM", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TotalCapital", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("TotalPrice", "decimal", 18, 2));
            columns.Add(new SQLiteColumn("DealDate", "datetime"));
            CreateTable("StockPrice", columns.ToArray());

            #endregion
        }

        void CreateDB(string dbName)
        {
            SQLiteConnection.CreateFile($"{dbName}");
        }

        /// <summary>
        /// uid varchar(50), name varchar(50), avatar varchar(250), desc varchar(250),profile varchar(250),statuses int,followers int,verified varchar(50)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        void CreateTable(string tableName, SQLiteColumn[] columns)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Append($"create table IF NOT EXISTS {tableName} (ID INTEGER PRIMARY KEY AUTOINCREMENT");
                foreach(var column in columns)
                {
                    if (column.Length != 0)
                    {
                        if (column.DecLength > 0)
                        {
                            sb.Append($",{column.ColumnName} {column.DataType}({column.Length},{column.DecLength})");
                        }
                        else
                        {
                            sb.Append($",{column.ColumnName} {column.DataType}({column.Length})");
                        }

                    }
                    else
                    {
                        sb.Append($",{column.ColumnName} {column.DataType}");
                    }
                }
                sb.Append($",LastDate datetime");
                sb.Append(")");
                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        public bool CreateEntity(BaseEntity entity, string table)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                StringBuilder sbCol = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();

                var type = entity.GetType();
                var fields = type.GetProperties();
                foreach (var field in fields)
                {
                    if (field.Name == "id") continue;

                    sbCol.Append($",{field.Name}");

                    var value = field.GetValue(entity);
                    if (value != null)
                    {
                        string val = null;
                        if(field.PropertyType == typeof(DateTime))
                        {
                            val = DateTime.Parse(value.ToString()).ToString("s");
                        }
                        else if(field.PropertyType == typeof(string))
                        {
                            val = value.ToString().Replace("'", "''");
                        }
                        else
                        {
                            val = value.ToString();
                        }
                        sbVal.Append($",'{val}'");
                    }
                    else
                    {
                        sbVal.Append(",NULL");
                    }
                }
                cmd.CommandText = $"insert into {table} (ID {sbCol.ToString()}) values (NULL {sbVal.ToString()})";
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool UpdateEntity(BaseEntity entity, string table, string col, string val, string[] columns)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                StringBuilder sb = new StringBuilder();

                var type = entity.GetType();
                var fields = type.GetProperties();
                foreach (var field in fields)
                {
                    if (field.Name == "id") continue;
                    if (!columns.Contains(field.Name)) continue;

                    var value = field.GetValue(entity);
                    if (value != null)
                    {
                        sb.Append($",{field.Name}='{value}'");
                    }
                }
                cmd.CommandText = $"update {table} set lastdate='{DateTime.Now.ToString("s")}' {sb.ToString()} where {col}='{val}'";
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool DeleteEntity(string table, string col, string val)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                StringBuilder sb = new StringBuilder();
                cmd.CommandText = $"delete from {table} where {col}='{val}'";
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool ExistsEntity(string table, string column, string value)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = $"select count(*) from {table} where {column}='{value}'";
                try
                {
                    var obj = cmd.ExecuteScalar();
                    if(obj != DBNull.Value)
                    {
                        int count = 0;
                        int.TryParse(obj.ToString(), out count);
                        return count > 0 ? true : false;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool ExistsEntity(string table, string where)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = $"select count(*) from {table} where {where}";
                try
                {
                    var obj = cmd.ExecuteScalar();
                    if (obj != DBNull.Value)
                    {
                        int count = 0;
                        int.TryParse(obj.ToString(), out count);
                        return count > 0 ? true : false;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public TEntity GetEntity<TEntity>(string table, string where) where TEntity : BaseEntity, new ()
        {
            return GetEntitys<TEntity>(table, where).FirstOrDefault();
        }

        public List<TEntity> GetEntitys<TEntity>(string table, string where)  where TEntity : BaseEntity, new ()
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = $"select * from {table} where {where}";
                    var ada = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable(table);
                    ada.Fill(dt);

                    var lst = new List<TEntity>();
                    for(var i=0; i<dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        var item = new TEntity();

                        var type = item.GetType();
                        var fields = type.GetProperties();
                        foreach (var field in fields)
                        {
                            if (dt.Columns.Contains(field.Name))
                            {
                                object obj = null;
                                if(field.PropertyType == typeof(Int32))
                                {
                                    obj = Int32.Parse(dr[field.Name].ToString());
                                }
                                else if(field.PropertyType == typeof(DateTime))
                                {
                                    obj = DateTime.Parse(dr[field.Name].ToString());
                                }
                                else
                                {
                                    obj = dr[field.Name].ToString();
                                }
                                field.SetValue(item, obj);
                            }
                        }

                        lst.Add(item);
                    }
                    return lst;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return null;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public List<string> GetGroupStrings(string table, string groupColumn, string where)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = $"select {groupColumn} from {table} where {where} group by {groupColumn}";
                    var ada = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable(table);
                    ada.Fill(dt);

                    var lst = new List<string>();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        lst.Add(dr[0].ToString());
                    }
                    return lst;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex);
                    //log4net.LogManager.GetLogger("logAppender").Error(ex);
                    return null;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
