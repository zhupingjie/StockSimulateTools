﻿using MySqlConnector;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StockSimulateCore.Utils
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
        //private static readonly string strConn = $"Server=47.99.184.6;Database=stock;User Id=stock;Password=sa!123456;Pooling=true;Max Pool Size={100};Connect Timeout=20;";
        private static string strConn = "";

        public static MySQLDBUtil Instance
        {
            get
            {
                return instance;
            }
        }

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
        public bool Update<TEntity>(TEntity entity, string[] columns = null) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            return UpdateEntity(new TEntity[] { entity }, tableName);
        }

        public bool Update<TEntity>(TEntity[] entitys, string[] columns = null) where TEntity : BaseEntity
        {
            var tableName = GetEntityTypeName<TEntity>();
            if (entitys.Length == 0) return false;

            return UpdateEntity(entitys, tableName, columns);
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
                var fields = type.GetProperties();
                foreach (var entity in entitys)
                {
                    StringBuilder sbCol = new StringBuilder();
                    StringBuilder sbVal = new StringBuilder();

                    foreach (var field in fields)
                    {
                        if (field.Name == "ID") continue;
                        if (field.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0) continue;

                        sbCol.Append($",`{field.Name}`");

                        var value = field.GetValue(entity);
                        if (value != null)
                        {
                            string val = null;
                            if (field.PropertyType == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (TypeUtil.IsNullableType(field.PropertyType) && TypeUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (field.PropertyType == typeof(string))
                            {
                                val = value.ToString().Replace("'", "''");
                            }
                            else if(field.PropertyType == typeof(bool))
                            {
                                val = ($"{value}" == "True" ? "1" : "0");
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

                    sql += $"insert into {tableName} (`ID` {sbCol.ToString()}) values (NULL {sbVal.ToString()});";
                }
                cmd.CommandText = sql;
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex, $"{ex.Message},SQL:{sql}");
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
                var fields = type.GetProperties();
                foreach (var entity in entitys)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var field in fields)
                    {
                        if (field.Name == "ID" || field.Name == "LastDate") continue;
                        if (columns != null && !columns.Contains(field.Name)) continue;
                        if (field.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0) continue;

                        var value = field.GetValue(entity);
                        if (value != null)
                        {
                            string val = null;
                            if (field.PropertyType == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (TypeUtil.IsNullableType(field.PropertyType) && TypeUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
                            {
                                val = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (field.PropertyType == typeof(string))
                            {
                                val = value.ToString().Replace("'", "''");
                            }
                            else if(field.PropertyType == typeof(bool))
                            {
                                val = $"{(value.ToString().ToUpper() == "TRUE" ? 1 : 0)}";
                            }
                            else
                            {
                                val = value.ToString();
                            }
                            sb.Append($",`{field.Name}`='{val}'");
                        }
                    }
                    sql += $"update {table} set `lastdate`='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' {sb.ToString()} where `ID`={entity.ID};";
                }

                cmd.CommandText = sql;
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex, $"{ex.Message},SQL:{sql}");
                    return false;
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

                var sql = "";
                StringBuilder sb = new StringBuilder();
                foreach (var entity in entitys)
                {
                    sb.Append($"delete from {table} where `ID`='{entity.ID}';");
                }
                cmd.CommandText = sql = sb.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex, $"{ex.Message},SQL:{sql}");
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

                var sql = $"delete from {table} where {where}";
                cmd.CommandText = sql;
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex, $"{ex.Message},SQL:{sql}");
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
                                else if (TypeUtil.IsNullableType(field.PropertyType) && TypeUtil.GetNullableType(field.PropertyType) == typeof(DateTime))
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
                    LogUtil.Error(ex, $"{ex.Message},SQL:{sql}");
                    return lst.ToArray();
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