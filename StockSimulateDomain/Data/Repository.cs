using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace StockSimulateDomain.Data
{
    public class Repository
    {
        DbConnection _conn;

        /// <summary>
        /// 当前原生Db连接的事务
        /// </summary>
        protected virtual DbTransaction Tran { get; private set; }

        /// <summary>
        /// 当前原生Db连接
        /// </summary>
        protected virtual DbConnection Conn
        {
            get
            {
                try
                {
                    if (this._conn == null)
                    {
                        this._conn = new MySqlConnection("");
                    }
                    if (this._conn.State == ConnectionState.Closed)
                    {
                        this._conn.Open();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"数据库配置错误,请检查配置项:{ex.Message}");
                }
                return this._conn;
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public virtual void BeginTran()
        {
            if (Tran != null) return;
            Tran = Conn.BeginTransaction();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="isolationLevel">事务隔离级别</param>

        public virtual void BeginTran(IsolationLevel isolationLevel)
        {
            if (Tran != null) return;
            Tran = Conn.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void Commit()
        {
            if (Tran != null && Tran.Connection != null)
                Tran.Commit();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void Rollback(bool closeConn = false)
        {
            if (Tran != null && Tran.Connection != null)
                Tran.Rollback();

            if (closeConn && this._conn != null)
            {
                if (this._conn.State == ConnectionState.Open)
                {
                    this._conn.Close();
                }
                if (Tran != null)
                {
                    Tran.Dispose();
                    Tran = null;
                }
            }
            if (Tran != null)
            {
                Tran.Dispose();
                Tran = null;
            }
        }

    }
}
