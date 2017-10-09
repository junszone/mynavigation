using System.Data.Common;
using System.Data;
using System;
using System.Collections.Generic;

public abstract class DataAccess
{
    #region SQLDataAccess
    private string _connectionString = "";
    protected string ConnectionString
    {
        get { return _connectionString; }
        set { _connectionString = value; }
    }

    protected int ExecuteNonQuery(DbCommand cmd)
    {
        int ret = -1;

        try
        {
            ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Dispose();
        }
        catch (Exception ew)
        {
            throw new Exception(ew.Message);
        }

        return ret;
    }

    protected IDataReader ExecuteReader(DbCommand cmd)
    {
        return ExecuteReader(cmd, CommandBehavior.Default);
    }

    protected IDataReader ExecuteReader(DbCommand cmd, CommandBehavior behavior)
    {
        try
        {
            return cmd.ExecuteReader(behavior);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    protected object ExecuteScalar(DbCommand cmd)
    {
        return cmd.ExecuteScalar();
    }

     static readonly Dictionary<Type, DbType> typeMap;
    static DataAccess()
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


    protected int ExecuteNonQuery(string SQl,Object obj,IDbConnection con)
    {

        using (IDbCommand cmd = con.CreateCommand())
        {



            foreach (var p in obj.GetType().GetProperties())
            {
                var Parameter = cmd.CreateParameter();
                Parameter.DbType = typeMap[p.PropertyType];
                Parameter.Value = p.GetValue(obj,null);
                Parameter.ParameterName = "@"+p.Name;
                cmd.Parameters.Add(Parameter);
            }

            int ret = -1;
            try
            {
                ret = cmd.ExecuteNonQuery();
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
    #endregion
}


