using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data.SqlClient
{
    public static class DBHelp
    {
        static readonly Dictionary<Type, DbType> typeMap;
        static DBHelp()
        {

            typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(TimeSpan)] = DbType.Time;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            typeMap[typeof(TimeSpan?)] = DbType.Time;
            typeMap[typeof(Object)] = DbType.Object;
        }

        #region MyRegion
        private static T ObjectFormatByDataReader<T>(IDataReader reader) where T : class, new()
        {


            T cObject = new T();
            foreach (var p in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    p.SetValue(cObject, reader.GetValue(reader.GetOrdinal(p.Name)), null);
                }
                catch
                {

                }
            }
            return cObject;
        }
        #endregion

        #region
        private static int ExecuteNonQuery(this SqlConnection con, string SQl, Object obj, CommandType c, IDbTransaction transaction,
            IEnumerable<OutParameter> Parameters = null)
        {
            using (IDbCommand cmd = con.CreateCommand())
            {
                foreach (var p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var Parameter = cmd.CreateParameter();
                    Parameter.DbType = typeMap[p.PropertyType];
                    Parameter.Value = p.GetValue(obj, null);
                    Parameter.ParameterName = "@" + p.Name;
                    Parameter.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(Parameter);
                }

                if (Parameters != null)
                {
                    foreach (var p in Parameters)
                    {
                        var Parameter = cmd.CreateParameter();
                        Parameter.DbType = typeMap[p.Value.GetType()];
                        Parameter.ParameterName = "@" + p.Name;
                        Parameter.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(Parameter);
                    }
                }

                int ret = -1;
                try
                {
                    cmd.CommandText = SQl;
                    cmd.CommandType = c;
                    ret = cmd.ExecuteNonQuery();
                    if (Parameters != null)
                    {
                        foreach (var p in Parameters)
                        {
                            p.Value = cmd.Parameters["@" + p.Name];
                        }
                    }
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
                catch (Exception ew)
                {
                    throw new Exception(ew.Message);
                }
                return ret;
            }

        }
        private static IDataReader ExecuteReader(this SqlConnection con, string SQl, Object obj, CommandType c, IDbTransaction transaction,
            IEnumerable<OutParameter> Parameters = null)
        {
            using (SqlClient.SqlCommand cmd = con.CreateCommand())
            {
                foreach (var p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    SqlParameter Parameter = new SqlParameter();
                    Parameter.DbType = typeMap[p.PropertyType];
                    Parameter.Value = p.GetValue(obj, null);
                    Parameter.ParameterName = "@" + p.Name;
                    Parameter.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(Parameter);
                }

                if (Parameters != null)
                {
                    foreach (var p in Parameters)
                    {
                        SqlParameter Parameter = new SqlParameter("@" + p.Name, typeMap[p.Value.GetType()]);
                        Parameter.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(Parameter);
                    }
                }

                try
                {
                    cmd.CommandText = SQl;
                    cmd.CommandType = c;
                    var ret = cmd.ExecuteReader();

                    //cmd.Connection.Close();
                    //cmd.Dispose();
                    return ret;
                }
                catch (Exception ew)
                {
                    throw new Exception(ew.Message);
                }

            }

        }
        private static object ExecuteScalar(this SqlConnection con, string SQl, Object obj, CommandType c, IDbTransaction transaction,
            IEnumerable<OutParameter> Parameters = null)
        {
            using (IDbCommand cmd = con.CreateCommand())
            {

                foreach (var p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var Parameter = cmd.CreateParameter();
                    Parameter.DbType = typeMap[p.PropertyType];
                    Parameter.Value = p.GetValue(obj, null);
                    Parameter.ParameterName = "@" + p.Name;
                    cmd.Parameters.Add(Parameter);
                }

                if (Parameters != null)
                {
                    foreach (var p in Parameters)
                    {
                        var Parameter = cmd.CreateParameter();
                        Parameter.DbType = typeMap[p.Value.GetType()];
                        Parameter.ParameterName = "@" + p.Name;
                        Parameter.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(Parameter);
                    }
                }

                try
                {
                    cmd.CommandText = SQl;
                    cmd.CommandType = c;
                    var ret = cmd.ExecuteScalar();



                    //cmd.Connection.Close();
                    //cmd.Dispose();
                    return ret;
                }
                catch (Exception ew)
                {
                    throw new Exception(ew.Message);
                }

            }

        }

        #endregion

        #region Text
        public static T SingleOrDefaultBySQL<T>(this SqlConnection con, string SQl, Object obj = null,
            IDbTransaction transaction = null, IEnumerable<OutParameter> Parameters = null) where T : class, new()
        {

            using (IDataReader reader = ExecuteReader(con, SQl, obj, CommandType.Text, transaction, Parameters))
            {
                T info = new T();
                if (reader != null)
                {
                    if (reader.Read())
                        info = ObjectFormatByDataReader<T>(reader);
                }
                con.Close();
                return info;
            }
        }

        public static IEnumerable<T> GetEnumerableBySQL<T>(this SqlConnection con, string SQl, Object obj = null,
            IDbTransaction transaction = null, IEnumerable<OutParameter> Parameters = null) where T : class, new()
        {
            using (IDataReader reader = ExecuteReader(con, SQl, obj, CommandType.Text, transaction, Parameters))
            {
                IList<T> info = new List<T>();
                if (reader != null)
                {
                    while (reader.Read())
                        info.Add(ObjectFormatByDataReader<T>(reader));
                }
                con.Close();
                return info;
            }
        }
        #endregion

        #region StoredProcedure
        public static T SingleOrDefaultByStoredProcedure<T>(this SqlConnection con, string SQl, Object obj = null,
            IDbTransaction transaction = null, IEnumerable<OutParameter> Parameters = null) where T : class, new()
        {

            using (IDataReader reader = ExecuteReader(con, SQl, obj, CommandType.StoredProcedure, transaction, Parameters))
            {
                T info = new T();
                if (reader != null)
                {
                    if (reader.Read())
                        info = ObjectFormatByDataReader<T>(reader);
                }
                con.Close();
                return info;
            }
        }

        public static IEnumerable<T> GetEnumerableByStoredProcedure<T>(this SqlConnection con, string SQl, Object obj = null,
            IDbTransaction transaction = null, IEnumerable<OutParameter> Parameters = null) where T : class, new()
        {
            using (IDataReader reader = ExecuteReader(con, SQl, obj, CommandType.StoredProcedure, transaction, Parameters))
            {
                IList<T> info = new List<T>();
                if (reader != null)
                {
                    while (reader.Read())
                        info.Add(ObjectFormatByDataReader<T>(reader));
                    if (Parameters != null)
                    {
                        if (reader.NextResult())
                        {
                            if (reader.Read())
                            {
                                foreach (var p in Parameters)
                                {
                                    p.Value = reader[p.Name];
                                }
                            }
                        }
                    }

                }
                con.Close();
                return info;
            }
        }
        #endregion


    }
    public class OutParameter
    {
        string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        object _Value;

        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
