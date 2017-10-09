using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Jun.DAL
{
    public class NavigationTypeDAL
    {
        private static NavigationTypeDAL instance;
        public static NavigationTypeDAL GetInstance() { return instance ?? (instance = new NavigationTypeDAL()); }
        private NavigationTypeDAL() { }
        public List<NavigationType> Retrieve()
        {
            StringBuilder sb = new StringBuilder();// sql = "select * from `NavigationType` ";
            sb.Append("select * from `NavigationType` where 1=1  order by `order` desc ");
            List<NavigationType> list = new List<NavigationType>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sb.ToString()))
            {
                while (dr.Read())
                {
                    NavigationType model = new NavigationType();
                    model.id = dr["id"].ToString();
                    model.name = dr["name"].ToString();
                    model.description = dr["description"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }

        public NavigationType RetrieveByID(int id)
        {
            string sql = "select * from `NavigationType` where id=" + id + "";
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                NavigationType model = new NavigationType();
                while (dr.Read())
                {

                    model.id = dr["id"].ToString();
                    model.name = dr["name"].ToString();
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
        public int Edit(NavigationType model)
        {
            if (string.IsNullOrWhiteSpace(model.id))
            {//新增
                string sql = "INSERT INTO `jun`.`NavigationType` (`type`, `name`, `url`, `order`, `createid`, `description`) VALUES (?type, ?name, ?url, ?order, ?createid, ?description);";
                MySqlParameter[] paramters = new MySqlParameter[]
                          {  
                             new MySqlParameter("?name",model.name),
                            new MySqlParameter("?order",model.order),
                            new MySqlParameter("?createid",model.createid),
                            new MySqlParameter("?description",model.description),

                          };
              return  MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, paramters);
            }
            else
            {//编辑
                string sql = "UPDATE `NavigationType`  set (`type`=?type, `name`=?type, `url`=?url, `order`=?order, `createid`=?createid, `description`=?description)  where id=`?id`";
                MySqlParameter[] paramters = new MySqlParameter[]
                          {  
                                new MySqlParameter("?id",model.id),
                             new MySqlParameter("?name",model.name),
                            new MySqlParameter("?order",model.order),
                            new MySqlParameter("?createid",model.createid),
                            new MySqlParameter("?description",model.description)
                          };
                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, paramters);
            }
        }
    }
}
