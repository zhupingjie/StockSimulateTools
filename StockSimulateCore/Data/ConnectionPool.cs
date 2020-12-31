using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace StockSimulateCore.Data
{
    public class ConnectionPool
    {
        private static ConnectionPool cpool = null;//池管理对象
        private static Object objlock = typeof(ConnectionPool);//池管理对象实例
        private int maxPoolSize = 200;//最大连接数
        private int useCount = 0;//已经使用的连接数
        private Queue<IDbConnection> pool = null;//连接保存的集合
        private string ConnectionStr = "";//连接字符串

        public ConnectionPool(string connStr)
        {
            //数据库连接字符串
            ConnectionStr = connStr;
            //创建可用连接的集合
            pool = new Queue<IDbConnection>();
        }

        #region 创建获取连接池对象
        public static ConnectionPool GetPool(string connStr)
        {
            if (cpool == null)
            {
                lock (objlock)
                {
                    if (cpool == null)
                    {
                        cpool = new ConnectionPool(connStr);
                    }
                }
            }
            return cpool;
        }
        #endregion

        #region 初始化连接池

        public void InitPool(int defaultPoolSize = 10)
        {
            for (var i = 0; i < defaultPoolSize; i++)
            {
                var conn = CreateConnection(); 

                Free(conn);
            }
        }

        #endregion

        #region 获取池中的连接
        /// <summary>
        /// 从连接池中获取一个链接
        /// </summary>
        /// <returns></returns>
        public IDbConnection Rent()
        {
            lock (pool)
            {
                IDbConnection tmp = null;
                //可用连接数量大于0
                if (pool.Count > 0)
                {
                    //取第一个可用连接
                    //tmp = (MySqlConnection)pool[0];
                    //在可用连接中移除此链接
                    tmp = pool.Dequeue();
                    //LogUtil.Debug($"!!! Get Connection, Pool={pool.Count},UserConn={useCount},Thread={tmp.ServerThread}");
                    //不成功
                    if (!isUserful(tmp))
                    {
                        //LogUtil.Debug($"!!! Bad Connection, Pool={pool.Count},UserConn={useCount},Thread={tmp.ServerThread}");
                        //可用的连接数据已去掉一个
                        useCount--;
                        if (tmp.State == ConnectionState.Open) tmp.Close();
                        tmp = Rent();
                    }
                }
                else
                {
                    //可使用的连接小于连接数量
                    if (useCount<= maxPoolSize)
                    {
                        try
                        {
                            //创建连接
                            tmp = CreateConnection();
                        }
                        catch (Exception e)
                        {
                            LogUtil.Error(e);
                        }
                    }
                    else
                    {
                        throw new Exception($"当前连接数已达到最大连接池数量:{maxPoolSize}");
                    }
                }
                return tmp;
            }
        }
        #endregion

        #region 创建连接
        private IDbConnection CreateConnection()
        {
            //创建连接
            var conn = SqlDBFactory.GetDBConnection(DatabaseTypeEnum.MySQL, ConnectionStr);
            conn.Open();
            //可用的连接数加上一个
            useCount++;
            //LogUtil.Debug($"!!! Create Connection, Pool={pool.Count},UseConn={useCount},Thread={tmp.ServerThread}");
            return conn;
        }
        #endregion

        #region 关闭连接,加连接回到池中
        /// <summary>
        /// 释放链接(放回连接池)
        /// </summary>
        /// <param name="con"></param>
        public void Free(IDbConnection con)
        {
            lock (pool)
            {
                if (con != null)
                {
                    //将连接添加在连接池中
                    pool.Enqueue(con);

                    //LogUtil.Debug($"!!! Back Connection, Pool={pool.Count},UserConn={useCount},Thread={con.ServerThread}");
                }
            }
        }
        #endregion

        #region 目的保证所创连接成功,测试池中连接
        private bool isUserful(IDbConnection con)
        {
            //主要用于不同用户
            bool result = true;
            if (con != null)
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = "select 1";
                try
                {
                    cmd.ExecuteScalar();
                }
                catch
                {
                    result = false;
                }

            }
            return result;
        }
        #endregion
    }
}