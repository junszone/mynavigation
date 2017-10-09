using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Career.DataAccess
{
    public static class ConnectionManager
    {
        public static string ConnectionString = null;

        static ConnectionManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        }
        /// <summary>
        /// 如果传入连接串的值就会连接到指定的数据库,默认为空,连接的是CareerCMS这个配置的连接串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDbConnection GetDbConnection(string name=null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new SqlConnection(ConnectionString);
            }
            else
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
            }
        }

        public static void ReleaseConnection(IDbConnection connection)
        {
            if (connection == null)
            {
                return;
            }
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
            connection.Dispose();
            connection = null;
        }
    }
}
