﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Reflection;
using StockSimulateCore.Entity;
using ServiceStack;
using System.IO;

namespace StockSimulateCore.Utils
{
    public class SQLiteDBUtil
    {
        static SQLiteDBUtil()
        {
            
        }
        private SQLiteDBUtil()
        {

        }

        private static readonly SQLiteDBUtil instance = new SQLiteDBUtil();
        private static readonly string dbName = "../../../StockSimulateDB/StockDB.db";
        private static readonly string strConn = $"Data Source={dbName}";
        public static SQLiteDBUtil Instance
        {
            get
            {
                return instance;
            }
        }

        public void InitSQLiteDB()
        {
            CreateDB(dbName);

            var entityTypes = FindEntityTypes();
            CreateTable(entityTypes);
        }

        void CreateDB(string dbName)
        {
            try
            {
                if (!File.Exists(dbName))
                {
                    SQLiteConnection.CreateFile($"{dbName}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"初始化数据库错误:{ex.Message}");
            }
        }

        #region 初始化数据库
        void CreateTable(Type[] types)
        {
            foreach(var type in types)
            {
                var sqlCols = new List<SQLiteColumn>();
                var preps = ObjectUtil.GetPropertyInfos(type);

                foreach(var prep in preps)
                {
                    var col = GetSQLiteColumnType(prep.Name, prep.PropertyType);
                    if (col.ColumnName.Equals("ID", StringComparison.CurrentCultureIgnoreCase)) continue;

                    sqlCols.Add(col);
                }
                CreateTable(type.Name.Replace("Entity", ""), sqlCols.ToArray());
            }
        }

        Type[] FindEntityTypes()
        {
            var path = "~/".MapAbsolutePath();
#if DEBUG
            path = "~/".MapHostAbsolutePath().CombineWith("Debug");
#endif

            var entityTypes = new List<Type>();
            var assmblies = AssemblyUtil.LoadAssemblies(path, x=>x.Name.Contains("StockSimulate"));
            foreach(var assbm in assmblies)
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

        SQLiteColumn GetSQLiteColumnType(string name, Type type)
        {
            var colType = "nvarchar";
            var length = 50;
            var decLength = 0;
            if (type.Name.Contains("String")) colType = "nvarchar";
            if (type.Name.Contains("Decimal"))
            {
                colType = "decimal";
                length = 50;
                decLength = 2;
            }
            if (type.Name.Contains("DateTime")) colType = "datetime";
            if (type.Name.Contains("Int")) colType = "int";
            if (type.Name.Contains("Bool")) colType = "bool";

            return new SQLiteColumn(name, colType, length, decLength);
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
                sb.Append(")");
                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        #endregion

        #region CRUD

        public TEntity[] QueryAll<TEntity>(string where = "", string orderBy = "ID desc", int takeSize = 0, string[] columns = null) where TEntity : BaseEntity, new()
        {
            var tableName = GetEntityTypeName<TEntity>();

            return GetEntitys<TEntity>(tableName, where, orderBy, takeSize, columns);
        }


        public TEntity QueryFirst<TEntity>(string where = "") where TEntity : BaseEntity, new()
        {
            var tableName = GetEntityTypeName<TEntity>();

            return GetEntitys<TEntity>(tableName, where).FirstOrDefault();
        }

        public bool Insert<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            return CreateEntity(new TEntity[] { entity }, tableName);
        }
        public bool Insert<TEntity>(TEntity[] entitys) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return CreateEntity(entitys, tableName); ;
        }
        public bool Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            return UpdateEntity(new TEntity[] { entity }, tableName);
        }

        public bool Update<TEntity>(TEntity[] entitys) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return UpdateEntity(entitys, tableName);
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            return DeleteEntity(new TEntity[] { entity }, tableName);
        }

        public bool Delete<TEntity>(TEntity[] entitys) where TEntity:BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return DeleteEntity(entitys, tableName);
        }

        public bool Delete<TEntity>(string where) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            return DeleteEntity(tableName, where);
        }

        string GetEntityTypeName<TEntity>() where TEntity:BaseEntity
        {
            return typeof(TEntity).Name.Replace("Entity", "");
        }

        bool CreateEntity(BaseEntity[] entitys, string tableName)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();
                
                var sql = string.Empty;
                var type = entitys.FirstOrDefault().GetType();
                var fields = ObjectUtil.GetPropertyInfos(type);
                foreach (var entity in entitys)
                {
                    StringBuilder sbCol = new StringBuilder();
                    StringBuilder sbVal = new StringBuilder();

                    foreach (var field in fields)
                    {
                        if (field.Name == "ID") continue;

                        sbCol.Append($",{field.Name}");

                        var value = field.GetValue(entity);
                        if (value != null)
                        {
                            string val = null;
                            if (field.PropertyType == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (ObjectUtil.IsNullableType(field.PropertyType) && ObjectUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (field.PropertyType == typeof(string))
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

                    sql += $"insert into {tableName} (ID {sbCol.ToString()}) values (NULL {sbVal.ToString()});";
                }
                cmd.CommandText = sql;
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error($"SQL:{sql}", ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        bool UpdateEntity(BaseEntity[] entitys, string table, string[] columns = null)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                var sql = string.Empty;
                var type = entitys.FirstOrDefault().GetType();
                var fields = ObjectUtil.GetPropertyInfos(type);

                foreach (var entity in entitys)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var field in fields)
                    {
                        if (field.Name == "ID" || field.Name == "LastDate") continue;
                        if (columns != null && !columns.Contains(field.Name)) continue;

                        var value = field.GetValue(entity);
                        if (value != null)
                        {
                            string val = null;
                            if (field.PropertyType == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (ObjectUtil.IsNullableType(field.PropertyType) && ObjectUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (field.PropertyType == typeof(string))
                            {
                                val = value.ToString().Replace("'", "''");
                            }
                            else
                            {
                                val = value.ToString();
                            }
                            sb.Append($",{field.Name}='{val}'");
                        }
                    }
                    sql += $"update {table} set lastdate='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' {sb.ToString()} where ID={entity.ID};";
                }
                
                cmd.CommandText = sql;
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error($"SQL:{sql}", ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        TEntity[] GetEntitys<TEntity>(string table, string where, string orderBy = "", int takeSize = 0, string[] columns = null) where TEntity : BaseEntity, new()
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                var lst = new List<TEntity>();
                var sql = $"select * from {table}";
                if (columns != null && columns.Length > 0) sql = $"select {string.Join(",", columns)} from {table}";
                if (!string.IsNullOrWhiteSpace(where)) sql = $"{sql} where {where}";
                if (!string.IsNullOrEmpty(orderBy)) sql = $"{sql} order by {orderBy}";
                if(takeSize > 0) sql = $"{sql} limit {takeSize}";

                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = sql;
                    var ada = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable(table);
                    ada.Fill(dt);

                    for (var i = 0; i < dt.Rows.Count; i++)
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
                                if (field.PropertyType == typeof(Int32))
                                {
                                    obj = Int32.Parse(dr[field.Name].ToString());
                                }
                                else if(field.PropertyType == typeof(Decimal))
                                {
                                    obj = Decimal.Parse(dr[field.Name].ToString());
                                }
                                else if(field.PropertyType == typeof(Boolean))
                                {
                                    obj = Boolean.Parse(dr[field.Name].ToString());
                                }
                                else if(ObjectUtil.IsNullableType (field.PropertyType) && ObjectUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                                {
                                    if (dr[field.Name] == DBNull.Value)
                                    {
                                        obj = null;
                                    }
                                    else
                                    {
                                        obj = DateTime.Parse(dr[field.Name].ToString());
                                    }
                                }
                                else if (field.PropertyType == typeof(DateTime))
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
                    return lst.ToArray();
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error($"SQL:{sql}", ex);
                    return lst.ToArray();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        bool DeleteEntity(BaseEntity[] entitys, string table)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                StringBuilder sb = new StringBuilder();
                foreach(var entity in entitys)
                {
                    sb.Append($"delete from {table} where ID='{entity.ID}';");
                }
                cmd.CommandText = sb.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error($"SQL:{sb.ToString()}", ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        bool DeleteEntity(string table, string where)
        {
            using (SQLiteConnection con = new SQLiteConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                StringBuilder sb = new StringBuilder();
                cmd.CommandText = $"delete from {table} where {where}";
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }


    public class SQLiteColumn
    {
        public SQLiteColumn(string name, string type)
        {
            this.ColumnName = name;
            this.DataType = type;
            this.Length = 0;
            this.DecLength = 0;
        }

        public SQLiteColumn(string name, string type, int length, int decLength = 0)
        {
            this.ColumnName = name;
            this.DataType = type;
            this.Length = length;
            this.DecLength = decLength;
        }

        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public int Length { get; set; }

        public int DecLength { get; set; }
    }
}