using MySqlConnector;
using StockSimulateCore.Config;
using StockSimulateCore.Utils;
using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace StockSimulateCore.Data
{
    public class Repository
    {
        public Repository()
        {
            this.Pool = ConnectionPool.GetPool(RunningConfig.Instance.DBConnectionString);
            this.Pool.InitPool();
        }
        private static Object objlock = typeof(Repository);

        private static Repository _instance = null;
        public static Repository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (objlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Repository();
                        }
                    }
                }
                return _instance;
            }
        }

        #region 链接 & 事务

        /// <summary>
        /// 连接池
        /// </summary>
        protected ConnectionPool Pool { get; private set; }

        #endregion

        #region  CRUD

        public TEntity[] QueryAll<TEntity>(string where = "", string orderBy = "ID desc", int takeSize = 0, string[] columns = null, bool withNoLock = false) where TEntity : BaseEntity, new()
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();

            return GetEntitys<TEntity>(tableName, where, orderBy, takeSize, columns, withNoLock);
        }

        public TEntity QueryFirst<TEntity>(string where = "", string orderBy = "ID desc", bool withNoLock = false) where TEntity : BaseEntity, new()
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();

            return GetEntitys<TEntity>(tableName, where, orderBy, withNoLock:withNoLock).FirstOrDefault();
        }

        public bool Insert<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            return CreateEntity(new TEntity[] { entity }, tableName);
        }
        public bool Insert<TEntity>(TEntity[] entitys) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return CreateEntity(entitys, tableName);
        }
        public bool Update<TEntity>(TEntity entity, string[] columns = null) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            return UpdateEntity(new TEntity[] { entity }, tableName);
        }

        public bool Update<TEntity>(TEntity[] entitys, string[] columns = null) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return UpdateEntity(entitys, tableName, columns);
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            return DeleteEntity(new TEntity[] { entity }, tableName);
        }

        public bool Delete<TEntity>(TEntity[] entitys) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return DeleteEntity(entitys, tableName);
        }

        public bool Delete<TEntity>(string where) where TEntity : BaseEntity
        {
            var tableName = ObjectUtil.GetEntityTypeName<TEntity>();
            return DeleteEntity(tableName, where);
        }

        TEntity[] GetEntitys<TEntity>(string table, string where, string orderBy = "", int takeSize = 0, string[] columns = null, bool withNoLock = false) where TEntity : BaseEntity, new()
        {
            var lst = new List<TEntity>();
            var sql = "";
            if (withNoLock) sql = $"{sql} SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;";
            if (columns != null && columns.Length > 0) sql = $"{sql} select {string.Join(",", columns)} from {table}";
            else sql = $"{sql} select * from {table}";
            if (!string.IsNullOrWhiteSpace(where)) sql = $"{sql} where {where}";
            if (!string.IsNullOrEmpty(orderBy)) sql = $"{sql} order by {orderBy}";
            if (takeSize > 0) sql = $"{sql} limit {takeSize}";
            if (withNoLock) sql = $"{sql}; SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;";

            var conn = Pool.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var item = new TEntity();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var name = reader.GetName(i);

                        object value = null;
                        var dbValue = reader.GetValue(i);
                        if (dbValue != DBNull.Value) value = dbValue;
                        ObjectUtil.SetPropertyValue(item, name, value);
                    }
                    if (item.ID > 0) lst.Add(item);
                }
                if(!reader.IsClosed) reader.Close();

                return lst.ToArray();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"{ex.Message},SQL:{sql}");
                return lst.ToArray();
            }
            finally
            {
                Pool.CloseConnection(conn);
            }
        }


        bool CreateEntity(BaseEntity[] entitys, string tableName)
        {
            var sql = string.Empty;
            var type = entitys.FirstOrDefault().GetType();
            var fields = type.GetProperties();
            foreach (var entity in entitys)
            {
                StringBuilder sbCol = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();

                foreach (var field in fields)
                {
                    if (field.Name == "ID") continue;
                    if (field.GetCustomAttributes(typeof(DBNotMappedAttribute), true).Length > 0) continue;

                    sbCol.Append($",`{field.Name}`");

                    var value = field.GetValue(entity);
                    if (value != null)
                    {
                        //string val = null;
                        //if (field.PropertyType == typeof(DateTime))
                        //{
                        //    val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        //}
                        //else if (TypeUtil.IsNullableType(field.PropertyType) && TypeUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                        //{
                        //    val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        //}
                        //else if (field.PropertyType == typeof(string))
                        //{
                        //    val = value.ToString().Replace("'", "''");
                        //}
                        //else if (field.PropertyType == typeof(bool))
                        //{
                        //    val = ($"{value}" == "True" ? "1" : "0");
                        //}
                        //else
                        //{
                        //    val = value.ToString();
                        //}
                        sbVal.Append($",'{value}'");
                    }
                    else
                    {
                        sbVal.Append(",NULL");
                    }
                }

                sql += $"insert into `{tableName}` (`ID` {sbCol.ToString()}) values (NULL {sbVal.ToString()});";
            }

            var conn = Pool.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Error($"{ex.Message},SQL:{sql}");
                return false;
            }
            finally
            {
                Pool.CloseConnection(conn);
            }
        }

        bool UpdateEntity(BaseEntity[] entitys, string table, string[] columns = null)
        {
            var sql = string.Empty;
            var type = entitys.FirstOrDefault().GetType();
            var fields = type.GetProperties();
            foreach (var entity in entitys)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var field in fields)
                {
                    if (field.Name == "ID" || field.Name == "LastDate") continue;
                    if (columns != null && !columns.Contains(field.Name)) continue;
                    if (field.GetCustomAttributes(typeof(DBNotMappedAttribute), true).Length > 0) continue;

                    var value = field.GetValue(entity);
                    if (value != null)
                    {
                        //if (field.PropertyType == typeof(DateTime))
                        //{
                        //    val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        //}
                        //else if (TypeUtil.IsNullableType(field.PropertyType) && TypeUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                        //{
                        //    val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        //}
                        //else if (field.PropertyType == typeof(string))
                        //{
                        //    val = value.ToString().Replace("'", "''");
                        //}
                        //else if (field.PropertyType == typeof(bool))
                        //{
                        //    val = $"{(value.ToString().ToUpper() == "TRUE" ? 1 : 0)}";
                        //}
                        //else
                        //{
                        //    val = value.ToString();
                        //}
                        sb.Append($",`{field.Name}`='{value}'");
                    }
                }
                sql += $"update {table} set `lastdate`='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' {sb.ToString()} where `ID`={entity.ID};";
            }

            var conn = Pool.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Error($"{ex.Message},SQL:{sql}");
                return false;
            }
            finally
            {
                Pool.CloseConnection(conn);
            }
        }


        bool DeleteEntity(BaseEntity[] entitys, string table)
        {
            var sql = "";
            StringBuilder sb = new StringBuilder();
            foreach (var entity in entitys)
            {
                sb.Append($"delete from {table} where `ID`='{entity.ID}';");
            }
            var conn = Pool.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql = sb.ToString();
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Error($"{ex.Message},SQL:{sql}");
                return false;
            }
            finally
            {
                Pool.CloseConnection(conn);
            }
        }

        bool DeleteEntity(string table, string where)
        {
            var sql = $"delete from {table} where {where}";

            var conn = Pool.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Error($"{ex.Message},SQL:{sql}");
                return false;
            }
            finally
            {
                Pool.CloseConnection(conn);
            }
        }

        void CreateTable(string tableName, DBColumn[] columns)
        {
            LogUtil.Debug($"检测并创建表[{tableName}]...");

            var sql = "";
            StringBuilder sb = new StringBuilder();
            sb.Append($"create table IF NOT EXISTS {tableName} (`ID` int(11) NOT NULL AUTO_INCREMENT");
            foreach (var column in columns)
            {
                if (column.Length != 0)
                {
                    if (column.DecLength > 0)
                    {
                        sb.Append($",`{column.ColumnName}` {column.DataType}({column.Length},{column.DecLength}) DEFAULT '0'");
                    }
                    else
                    {
                        sb.Append($",`{column.ColumnName}` {column.DataType}({column.Length}) DEFAULT NULL");
                    }

                }
                else
                {
                    sb.Append($",`{column.ColumnName}` {column.DataType} DEFAULT NULL");
                }
            }
            sb.Append(",PRIMARY KEY (`ID`)");
            sb.Append(") ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;");

            var conn = Pool.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql = sb.ToString();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                LogUtil.Error($"{ex.Message},SQL:{sql}");
            }
            finally
            {
                Pool.CloseConnection(conn);
            }
        }


        public void InitDataBase()
        {
            var entityTypes = ObjectUtil.FindEntityTypes();
            foreach (var type in entityTypes)
            {
                var sqlCols = new List<DBColumn>();
                var preps = type.GetProperties();

                foreach (var prep in preps)
                {
                    if (prep.GetCustomAttributes(typeof(DBNotMappedAttribute), true).Length > 0) continue;

                    var col = ObjectUtil.GetDBColumnType(prep.Name, prep.PropertyType);
                    if (col.ColumnName.Equals("ID", StringComparison.CurrentCultureIgnoreCase)) continue;

                    sqlCols.Add(col);
                }
                if (sqlCols.Count > 0)
                {
                    var tableName = ObjectUtil.GetEntityTypeName(type);
                    CreateTable(tableName, sqlCols.ToArray());
                }
            }
        }
        #endregion
    }
}
