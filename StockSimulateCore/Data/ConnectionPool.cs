using MySqlConnector;
using StockSimulateCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Data
{
    public class ConnectionPool
    {
        private static ConnectionPool cpool = null;//池管理对象
        private static Object objlock = typeof(ConnectionPool);//池管理对象实例
        private int maxPoolSize = 200;//最大连接数
        private int defaultPoolSize = 5;//默认链接数
        private int useCount = 0;//已经使用的连接数
        private Queue<MySqlConnection> pool = null;//连接保存的集合
        private string ConnectionStr = "";//连接字符串

        public ConnectionPool(string connStr)
        {
            //数据库连接字符串
            ConnectionStr = connStr;
            //创建可用连接的集合
            pool = new Queue<MySqlConnection>();
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

        public void InitPool()
        {
            for (var i = 0; i < defaultPoolSize; i++)
            {
                MySqlConnection tmp = null;
                tmp = CreateConnection(tmp);

                CloseConnection(tmp);
            }
        }

        #endregion

        #region 获取池中的连接
        public MySqlConnection GetConnection()
        {
            lock (pool)
            {
                MySqlConnection tmp = null;
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
                        if (tmp.State == System.Data.ConnectionState.Open) tmp.Close();
                        tmp = GetConnection();
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
                            tmp = CreateConnection(tmp);
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
        private MySqlConnection CreateConnection(MySqlConnection tmp)
        {
            //创建连接
            MySqlConnection conn = new MySqlConnection(ConnectionStr);
            conn.Open();
            //可用的连接数加上一个
            useCount++;
            tmp = conn;
            //LogUtil.Debug($"!!! Create Connection, Pool={pool.Count},UseConn={useCount},Thread={tmp.ServerThread}");
            return tmp;
        }
        #endregion

        #region 关闭连接,加连接回到池中
        public void CloseConnection(MySqlConnection con)
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
        private bool isUserful(MySqlConnection con)
        {
            //主要用于不同用户
            bool result = true;
            if (con != null)
            {
                string sql = "select 1";//随便执行对数据库操作
                MySqlCommand cmd = new MySqlCommand(sql, con);
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