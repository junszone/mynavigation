﻿using Jun.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Career.Utility;


namespace Jun.DAL
{
    public class ServerDAL
    {
        private static ServerDAL instance;
        public static ServerDAL GetInstance() { return instance ?? (instance = new ServerDAL()); }
        private ServerDAL() { }
        //public List<Server> Retrieve(Server param)
        //{
        //    StringBuilder sb = new StringBuilder();// sql = "select * from `server` ";
        //    sb.Append("select * from `server` where 1=1 ");
        //    if (!String.IsNullOrWhiteSpace(param.keyword))
        //    {
        //        var arr = param.keyword.Split(' ').ToList();
        //        sb.Append(" and ");
        //        foreach (string key in arr)
        //        {
        //            var index = arr.IndexOf(key);//
        //            if (index > 0)
        //            {
        //                sb.AppendFormat(" {0} ", param.logic);
        //            }
        //            sb.AppendFormat("  (level like '%{0}%' or type like '%{0}%' or url like '%{0}%'  or  ip like '%{0}%' or  name like '%{0}%' or user like '%{0}%' or description like '%{0}%' or content like '%{0}%')  ", key);
        //        }
        //    }
        //    else
        //    {
        //        if (!String.IsNullOrWhiteSpace(param.level))
        //        {
        //            sb.AppendFormat(" and level like '%{0}%' ", param.level);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.type))
        //        {
        //            sb.AppendFormat(" and type like '%{0}%' ", param.type);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.name))
        //        {
        //            sb.AppendFormat(" and name like '%{0}%' ", param.name);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.url))
        //        {
        //            sb.AppendFormat(" and url like '%{0}%' ", param.url);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.ip))
        //        {
        //            sb.AppendFormat(" and ip like '%{0}%' ", param.ip);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.user))
        //        {
        //            sb.AppendFormat(" and user like '%{0}%' ", param.user);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.content))
        //        {
        //            sb.AppendFormat(" and content like '%{0}%' ", param.content);
        //        }
        //        if (!String.IsNullOrWhiteSpace(param.description))
        //        {
        //            sb.AppendFormat(" and description like '%{0}%' ", param.description);
        //        }
        //    }
        //    List<Server> list = new List<Server>();
        //    using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sb.ToString()))
        //    {
        //        while (dr.Read())
        //        {
        //            Server model = new Server();
        //            model.id = dr["id"].ToString();
        //            model.level = dr["level"].ToString();
        //            model.type = dr["type"].ToString();
        //            model.name = dr["name"].ToString();
        //            model.url = dr["url"].ToString();
        //            model.ip = dr["ip"].ToString();
        //            model.port = dr["port"].ToString();
        //            model.user = dr["user"].ToString();
        //            model.pwd = dr["pwd"].ToString();
        //            model.content = dr["content"].ToString();
        //            model.description = dr["description"].ToString();
        //            list.Add(model);
        //        }
        //    }
        //    return list;
        //}

        public Server RetrieveByID(int id)
        {
            string sql = "select * from `server` where id=" + id + "";
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                Server model = new Server();
                while (dr.Read())
                {

                    model.id = dr["id"].ToString();
                    model.level = dr["level"].ToString();
                    model.type = dr["type"].ToString();
                    model.name = dr["name"].ToString();
                    model.url = dr["url"].ToString();
                    model.ip = dr["ip"].ToString();
                    model.port = dr["port"].ToString();
                    model.user = dr["user"].ToString();
                    model.pwd = dr["pwd"].ToString();
                    model.content = dr["content"].ToString();
                    model.description = dr["description"].ToString();
                }
                return model;
            }
        }
        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MySqlParameter[] GetParam(Server model)
        {
            MySqlParameter[] paramters = new MySqlParameter[]
            {  
                             new MySqlParameter("?id",model.id),
                             new MySqlParameter("?level",model.level),
                             new MySqlParameter("?type",model.type),
                             new MySqlParameter("?name",model.name),
                             new MySqlParameter("?url",model.url),
                            new MySqlParameter("?ip",model.ip),
                            new MySqlParameter("?port",model.port),
                            new MySqlParameter("?user",model.user),
                            new MySqlParameter("?pwd",model.pwd),
                            new MySqlParameter("?content",model.content),
                            new MySqlParameter("?description",model.description)
                          };
            return paramters;
        }
        public int Edit(Server model)
        {
            if (string.IsNullOrWhiteSpace(model.id))
            {//新增
                string sql = @"INSERT INTO `server` (
	`level`,
	`type`,
	`name`,
	`url`,
	`ip`,
	`port`,
	`user`,
	`pwd`,
	`content`,
	`description`
)
VALUES
	(
	?level,
	?type,
	?name,
	?url,
	?ip,
	?port,
	?user,
	?pwd,
	?content,
	?description
	);
";

                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model));
            }
            else
            {//编辑
                string sql = @"UPDATE `server`
SET 
 `level` =?level,
 `type` =?type,
 `name` =?name,
 `url` =?url,
 `ip` =?ip,
 `port` =?port,
 `user` =?user,
 `pwd` =?pwd,
 `content` =?content,
 `description` =?description
WHERE
	(`id` = ?id);
";
                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model));
            }
        }

        public int Delete(int id)
        {
            //string sql = "delete from Server where id=" + id;
            string sql = "update Server set deleted=1 where id="+id;
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, null);
        }

        public Paged<Server> RetrievePaged(ParamServer param)
        {
            StringBuilder sb = new StringBuilder();// sql = "select * from `server` ";
            if (!String.IsNullOrWhiteSpace(param.keyword))
            {
                var arr = param.keyword.Split(' ').ToList();
                sb.Append(" and ");
                foreach (string key in arr)
                {
                    var index = arr.IndexOf(key);//
                    if (index > 0)
                    {
                        sb.AppendFormat(" {0} ", param.logic);
                    }
                    sb.AppendFormat("  (level like '%{0}%' or type like '%{0}%' or url like '%{0}%'  or  ip like '%{0}%' or  name like '%{0}%' or user like '%{0}%' or description like '%{0}%' or content like '%{0}%')  ", key);
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(param.level))
                {
                    sb.AppendFormat(" and level like '%{0}%' ", param.level);
                }
                if (!String.IsNullOrWhiteSpace(param.type))
                {
                    sb.AppendFormat(" and type like '%{0}%' ", param.type);
                }
                if (!String.IsNullOrWhiteSpace(param.name))
                {
                    sb.AppendFormat(" and name like '%{0}%' ", param.name);
                }
                if (!String.IsNullOrWhiteSpace(param.url))
                {
                    sb.AppendFormat(" and url like '%{0}%' ", param.url);
                }
                if (!String.IsNullOrWhiteSpace(param.ip))
                {
                    sb.AppendFormat(" and ip like '%{0}%' ", param.ip);
                }
                if (!String.IsNullOrWhiteSpace(param.user))
                {
                    sb.AppendFormat(" and user like '%{0}%' ", param.user);
                }
                if (!String.IsNullOrWhiteSpace(param.content))
                {
                    sb.AppendFormat(" and content like '%{0}%' ", param.content);
                }
                if (!String.IsNullOrWhiteSpace(param.description))
                {
                    sb.AppendFormat(" and description like '%{0}%' ", param.description);
                }
            }
            string sqlPaged = "select * from `server` where deleted=0  " + sb.ToString() + string.Format("limit {0},{1}", param.offset, param.limit + param.offset);
            string sqlCount = "select count(*) from `server` where deleted=0  " + sb.ToString();
            List<Server> list = new List<Server>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sqlPaged))
            {
                while (dr.Read())
                {
                    Server model = new Server();
                    model.id = dr["id"].ToString();
                    model.level = dr["level"].ToString();
                    model.type = dr["type"].ToString();
                    model.name = dr["name"].ToString();
                    model.url = dr["url"].ToString();
                    model.ip = dr["ip"].ToString();
                    model.port = dr["port"].ToString();
                    model.user = dr["user"].ToString();
                    model.pwd = dr["pwd"].ToString();
                    model.content = dr["content"].ToString();
                    model.description = dr["description"].ToString();
                    list.Add(model);
                }
            }
            int count = MySqlHelper.ExecuteScalar(MySqlHelper.Conn, CommandType.Text, sqlCount).GetInt();
            Paged<Server> paged = new Paged<Server>()
            {
                rows = list,
                total = count
            };
            return paged;
        }
    }
}
