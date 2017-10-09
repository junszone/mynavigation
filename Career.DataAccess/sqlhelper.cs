using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Career.DataAccess
{
    /// <summary>
    /// 操作帮助类
    /// </summary>
    public class SqlHelper
    {
        private static SqlHelper instance;
        public static SqlHelper GetInstance() { return instance ?? (instance = new SqlHelper()); }
        private SqlHelper() { }

        private SqlConnection con;
        private SqlCommand cmd;
        /// <summary>
        /// Open
        /// </summary>
        /// <returns></returns>
        public SqlConnection OpenConnection()
        {
            if (con == null)
            {
                string conStr = ConfigurationManager.ConnectionStrings["conStr"].ToString();
                con = new SqlConnection(conStr);
                con.Open();
            }
            return con;
        }
        /// <summary>
        /// Close
        /// </summary>
        public void CloseConnection()
        {
            if (con != null && con.State != ConnectionState.Closed)
            {
                con.Close();
                con = null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string sql, SqlParameter[] cmdParms)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                cmd = new SqlCommand(sql, con);
                cmd.Connection = OpenConnection();
            }
            //if (para != null && para.Length > 0)
            //{
            //    cmd.Parameters.Add(para);
            //}
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                cmd = new SqlCommand(sql, con);
                cmd.Connection = OpenConnection();
            }

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// 查询第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecueteScalar(string sql,SqlConnection conn, SqlTransaction trans=null)
        {
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    if (trans!=null)
                    {
                        cmd.Transaction = trans;
                    }
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                CloseConnection();
            }

        }
        /// <summary>
        /// 查询第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecueteScalar(string sql, SqlParameter[] cmdParms,SqlConnection conn, SqlTransaction trans=null)
        {
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parameter in cmdParms)
                        {
                            if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    if (trans!=null)
                    {
                        cmd.Transaction = trans;
                    }
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                CloseConnection();
            }

        }
        /// <summary>
        /// 非查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public int UpdateDataBySql(string sql, SqlParameter[] cmdParms)
        {
            int flag = 0;
            if (!string.IsNullOrEmpty(sql))
            {
                cmd = new SqlCommand(sql, con);
                cmd.Connection = OpenConnection();
            }
            //if (para != null && para.Length > 0)
            //{
            //    cmd.Parameters.AddRange(para);
            //}
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            try
            {
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return flag;
        }
        /// <summary>
        /// 非查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public int UpdateDataBySql(string sql)
        {
            int flag = 0;
            if (!string.IsNullOrEmpty(sql))
            {
                cmd = new SqlCommand(sql, con);
                cmd.Connection = OpenConnection();
            }
            try
            {
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return flag;
        }
        /// <summary>
        /// 执行非查询操作
        /// </summary>
        /// <param name="spName">sql语句或存储过程名(但这个方法主要为了执行存储过程，因为执行sql语句上面更合适的方法)</param>
        /// <param name="para">参数数组</param>
        /// <param name="type">类型（Text或是StoredProcedure）</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery(string spName, SqlParameter[] para, CommandType type)
        {
            int flag = 0;
            if (!string.IsNullOrEmpty(spName))
            {
                cmd = new SqlCommand(spName, con);
                cmd.CommandType = type;
                cmd.CommandText = spName;
                cmd.Connection = OpenConnection();
            }
            if (para != null && para.Length > 0)
            {
                cmd.Parameters.AddRange(para);
            }
            try
            {
                flag = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con = null;
                }
            }
            return flag;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            return val;
        }
        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            //cmd.CommandTimeout = 600;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);

            }
        }
    }
}