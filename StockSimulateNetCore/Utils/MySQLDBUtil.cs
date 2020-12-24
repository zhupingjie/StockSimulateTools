﻿using MySqlConnector;
using ServiceStack;
using StockSimulateNetCore.Data;
using StockSimulateNetCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace StockSimulateNetCore.Utils
{
    public class MySQLDBUtil
    {
        static MySQLDBUtil()
        {

        }
        private MySQLDBUtil()
        {

        }

        private static readonly MySQLDBUtil instance = new MySQLDBUtil();
        private static readonly string strConn = $"Server=47.99.184.6;Database=stock;User Id=stock;Password=sa!123456;Pooling=true;Max Pool Size={100};Connect Timeout=20;";

        public static MySQLDBUtil Instance
        {
            get
            {
                return instance;
            }
        }


        public void InitDataBase()
        {
            var entityTypes = FindEntityTypes();
            CreateTable(entityTypes);
        }

        #region 初始化数据库
        void CreateTable(Type[] types)
        {
            foreach (var type in types)
            {
                var sqlCols = new List<DBColumn>();
                var preps = ObjectUtil.GetPropertyInfos(type);

                foreach (var prep in preps)
                {
                    var col = GetDBColumnType(prep.Name, prep.PropertyType);
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
            path = "~/".MapHostAbsolutePath().CombineWith("bin", "Debug", "netcoreapp3.1");
#endif

            var entityTypes = new List<Type>();
            var assmblies = AssemblyUtil.LoadAssemblies(path, x => x.Name.Contains("StockSimulate"));
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

        DBColumn GetDBColumnType(string name, Type type)
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
            else if (ObjectUtil.IsNullableType(type) && ObjectUtil.GetNullableType(type) == typeof(DateTime))
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
                length = 1;
            }

            return new DBColumn(name, colType, length, decLength);
        }

        /// <summary>
        /// uid varchar(50), name varchar(50), avatar varchar(250), desc varchar(250),profile varchar(250),statuses int,followers int,verified varchar(50)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        void CreateTable(string tableName, DBColumn[] columns)
        {
            using (MySqlConnection con = new MySqlConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();
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

        public bool Delete<TEntity>(TEntity[] entitys) where TEntity : BaseEntity
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

        string GetEntityTypeName<TEntity>() where TEntity : BaseEntity
        {
            return typeof(TEntity).Name.Replace("Entity", "");
        }

        bool CreateEntity(BaseEntity[] entitys, string tableName)
        {
            using (MySqlConnection con = new MySqlConnection(strConn))
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
            using (MySqlConnection con = new MySqlConnection(strConn))
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
            using (MySqlConnection con = new MySqlConnection(strConn))
            {
                var lst = new List<TEntity>();
                var sql = $"select * from {table}";
                if (columns != null && columns.Length > 0) sql = $"select {string.Join(",", columns)} from {table}";
                if (!string.IsNullOrWhiteSpace(where)) sql = $"{sql} where {where}";
                if (!string.IsNullOrEmpty(orderBy)) sql = $"{sql} order by {orderBy}";
                if (takeSize > 0) sql = $"{sql} limit {takeSize}";

                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = sql;
                    var ada = new MySqlDataAdapter(cmd);
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
                                else if (field.PropertyType == typeof(Decimal))
                                {
                                    obj = Decimal.Parse(dr[field.Name].ToString());
                                }
                                else if (field.PropertyType == typeof(Boolean))
                                {
                                    obj = Boolean.Parse(dr[field.Name].ToString());
                                }
                                else if (ObjectUtil.IsNullableType(field.PropertyType) && ObjectUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
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


        bool DeleteEntity(BaseEntity[] entitys, string table)
        {
            using (MySqlConnection con = new MySqlConnection(strConn))
            {
                con.Open();
                var cmd = con.CreateCommand();

                StringBuilder sb = new StringBuilder();
                foreach (var entity in entitys)
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
            using (MySqlConnection con = new MySqlConnection(strConn))
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
        #endregion
    }
}
