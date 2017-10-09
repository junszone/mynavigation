using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Jun.DAL
{
    public class NavigationDAL
    {
        private static NavigationDAL instance;
        public static NavigationDAL GetInstance() { return instance ?? (instance = new NavigationDAL()); }
        private NavigationDAL() { }
        public List<Navigation> Retrieve(Navigation param)
        {
            StringBuilder sb = new StringBuilder();// sql = "select * from `Navigation` ";
            sb.Append("select * from `Navigation` where 1=1 ");
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
                if (!String.IsNullOrWhiteSpace(param.description))
                {
                    sb.AppendFormat(" and description like '%{0}%' ", param.description);
                }
            }
            sb.Append(" order by `order` desc");
            List<Navigation> list = new List<Navigation>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sb.ToString()))
            {
                while (dr.Read())
                {
                    Navigation model = new Navigation();
                    model.id = dr["id"].ToString();
                    model.type = dr["type"].ToString();
                    model.name = dr["name"].ToString();
                    model.url = dr["url"].ToString();
                    model.icon = dr["icon"].ToString();
                    model.description = dr["description"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }

        public Navigation RetrieveByID(int id)
        {
            string sql = "select * from `Navigation` where id=" + id + "";
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                Navigation model = new Navigation();
                while (dr.Read())
                {

                    model.id = dr["id"].ToString();
                    model.type = dr["type"].ToString();
                    model.name = dr["name"].ToString();
                    model.url = dr["url"].ToString();
                    model.icon = dr["icon"].ToString();
                    model.order = decimal.Parse(dr["order"].ToString());
                    model.description = dr["description"].ToString();
                }
                return model;
            }
        }
        /// <summary>
        /// 新增或编辑导航
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Edit(Navigation model)
        {
            if (string.IsNullOrWhiteSpace(model.id))
            {//新增
                string sql = "INSERT INTO `navigation` (`type`, `name`, `url`, `order`, `createid`, `description`) VALUES (?type, ?name, ?url, ?order, ?createid, ?description);";
                MySqlParameter[] paramters = new MySqlParameter[]
                          {  
                             new MySqlParameter("?type",model.type),
                             new MySqlParameter("?name",model.name),
                             new MySqlParameter("?url",model.url),
                            new MySqlParameter("?order",model.order),
                            new MySqlParameter("?icon",model.icon),
                            new MySqlParameter("?createid",model.createid),
                            new MySqlParameter("?description",model.description),

                          };
                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, paramters);
            }
            else
            {//编辑
                string sql = "UPDATE `navigation`  set  `type`=?type, `name`=?name, `url`=?url, `order`=?order,`icon`=?icon, `description`=?description  where id=?id ";
                MySqlParameter[] paramters = new MySqlParameter[]
                          {  
                                new MySqlParameter("?id",model.id),
                             new MySqlParameter("?type",model.type),
                             new MySqlParameter("?name",model.name),
                             new MySqlParameter("?url",model.url),
                            new MySqlParameter("?order",model.order),
                               new MySqlParameter("?icon",model.icon),
                            new MySqlParameter("?createid",model.createid),
                            new MySqlParameter("?description",model.description)
                          };
                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, paramters);
            }
        }

        public int Delete(string id)
        {
            string sql = "delete from navigation where id=?id";
            MySqlParameter[] paramters = new MySqlParameter[] { 
             new MySqlParameter("?id",id)
            };
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, paramters);
        }
    }
}
