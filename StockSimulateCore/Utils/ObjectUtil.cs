﻿using ServiceStack.Text;
using StockSimulateCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
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
                //else
                //{
                //    dt.Columns.Add(prep.Name);
                //}
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
    }
}
