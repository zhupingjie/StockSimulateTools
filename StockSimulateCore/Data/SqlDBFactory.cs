using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace StockSimulateCore.Data
{
    public class SqlDBFactory
    {
        public static IDbConnection GetDBConnection(DatabaseTypeEnum databaseType, string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
