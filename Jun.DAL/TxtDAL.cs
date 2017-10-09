using Jun.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Career.Utility;


namespace Jun.DAL
{
    public class TxtDAL
    {
        private static TxtDAL instance;
        public static TxtDAL GetInstance() { return instance ?? (instance = new TxtDAL()); }
        private TxtDAL() { }


        public Txt RetrieveByID(int id)
        {
            string sql = "select * from `Txt` where id=" + id + "";
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                Txt model = new Txt();
                while (dr.Read())
                {

                    model.id = dr["id"].ToString();
                    model.title = dr["title"].ToString();
                    model.content = dr["content"].ToString();
                    model.html = dr["html"].ToString();
                    model.txttype = dr["type"].ToString();
                }
                return model;
            }
        }
        public string RetrieveTagByTxtID(int id)
        {
            string sql = "select tagid from `txttag` where txtid=" + id + "";
            string ids = "";
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                while (dr.Read())
                {
                    ids += "," + dr["tagid"].GetInt();
                }
            }
            return ids;
        }
        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MySqlParameter[] GetParam(Txt model)
        {
            MySqlParameter[] paramters = new MySqlParameter[]
            {  
                new MySqlParameter("?id",model.id),
                new MySqlParameter("?title",model.title.GetTrim()),
                new MySqlParameter("?CreateTime",DateTime.Now),
                new MySqlParameter("?UpdateTime",DateTime.Now),
                new MySqlParameter("?content",model.content.GetTrim()),
                new MySqlParameter("?html",model.html.GetTrim()),
                new MySqlParameter("?type",model.txttype),
                new MySqlParameter("?fileName",model.fileName)
             };
            return paramters;
        }
        public string Edit(Txt model)
        {
            string id = string.Empty;
            if (string.IsNullOrWhiteSpace(model.id))
            {//新增
                string sql = @"INSERT INTO `Txt` (
    `title`,
	`content`,
	`html`,
    `CreateTime`,
   `UpdateTime`,
   `fileName`,
    `type`
)
VALUES
	(	
    ?title,
	?content,
    ?html,
    ?CreateTime,
    ?UpdateTime,
    ?fileName,
    ?type
	);
select last_insert_id();
";
                id = MySqlHelper.ExecuteScalar(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model)).GetString();
                //return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model));
            }
            else
            {//编辑
                model.UpdateTime = DateTime.Now;
                string sql = @"UPDATE `Txt`
                                    SET 
                                         `title`=?title,
                                         `content` =?content,
                                          `html` =?html,
                                        `type`=?type,
                                        `UpdateTime`=?UpdateTime
                                    WHERE
	                                    (`id` = ?id);
                                    ";
                int flag = MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model));
                id = flag > 0 ? model.id : "0";
            }
            //string sqlcount = "update txttype  set txtCount=(select count(*) from txt where type=?type) where id=?type";
            //MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sqlcount, GetParam(model));
            return id;
        }

        public int Delete(int id)
        {
            //string sql = "delete from Txt where id=" + id;
            string sql = string.Format("update Txt set deleted=1,UpdateTime='{1}' where id={0}", id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, null);
        }

        public Paged<Txt> RetrievePaged(ParamTxt param)
        {
            StringBuilder sb = new StringBuilder();// sql = "select * from `Txt` ";
            if (!String.IsNullOrWhiteSpace(param.keyword))
            {
                var arr = param.keyword.Split(' ').ToList();
                sb.Append(" and ");
                foreach (string keyStr in arr)
                {
                    string key = keyStr.GetTrim();
                    var index = arr.IndexOf(key);//
                    if (index > 0)
                    {
                        sb.AppendFormat(" {0} ", param.logic);
                    }
                    if (param.logiccontent == "content")
                    {
                        sb.AppendFormat("  (  (title like '%{0}%' ) or (content like '%{0}%') )  ", key);
                    }
                    else
                    {
                        sb.AppendFormat("  ( title like '%{0}%')  ", key);
                    }
                }

            }
            if (!String.IsNullOrWhiteSpace(param.tag))
            {
                sb.Append(" and id in ( select txtid from txttag where tagid in (" + param.tag + ") ) ");
            }
            if (!String.IsNullOrWhiteSpace(param.txttype))
            {
                if (param.txttype != "-1")
                {
                    sb.Append(" and  type= " + param.txttype + " ");
                }
            }
            //else
            //{
            //    sb.Append(" and  type=0 ");//默认显示全部
            //}
            string orderStr = "  order by updatetime desc ";
            string limitStr = string.Format("limit {0},{1}", param.offset, param.limit);
            string whereStr = sb.ToString();
            string sqlPaged = "select * from `Txt` where  deleted=0  " + whereStr + orderStr + limitStr;
            string sqlCount = "select count(*) from `Txt` where deleted=0 " + whereStr;
            List<Txt> list = new List<Txt>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sqlPaged))
            {
                while (dr.Read())
                {
                    Txt model = new Txt();
                    model.id = dr["id"].ToString();
                    model.title = dr["title"].ToString();
                    //model.content = dr["content"].ToString();
                    model.CreateTime = dr["CreateTime"].GetDateTime();
                    model.UpdateTime = dr["UpdateTime"].GetDateTime();
                    list.Add(model);
                }
            }
            int count = MySqlHelper.ExecuteScalar(MySqlHelper.Conn, CommandType.Text, sqlCount).GetInt();
            Paged<Txt> paged = new Paged<Txt>()
            {
                rows = list,
                total = count
            };
            return paged;
        }

        public List<TxtType> RetrieveType()
        {
            string sql = "select * from txtType where deleted=0    order by updatetime desc";
            List<TxtType> list = new List<TxtType>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                while (dr.Read())
                {
                    TxtType model = new TxtType();
                    model.id = dr["id"].GetInt();
                    model.title = dr["title"].ToString();
                    model.txtCount = dr["txtCount"].GetInt();
                    list.Add(model);
                }
            }
            return list;
        }

        public List<Tag> RetrieveTag()
        {
            string sql = "select * from Tag where deleted=0  order by updatetime desc";
            List<Tag> list = new List<Tag>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                while (dr.Read())
                {
                    Tag model = new Tag();
                    model.id = dr["id"].GetInt();
                    model.title = dr["title"].ToString();
                    model.txtCount = dr["txtCount"].GetInt();
                    list.Add(model);
                }
            }
            return list;
        }


        public int EditTag(Txt model)
        {
            if (!String.IsNullOrWhiteSpace(model.tag))
            {
                MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, "delete from txttag where txtid='" + model.id + "'");
                string[] arr = model.tag.Split(',');
                foreach (var item in arr)
                {
                    string sql = @"INSERT INTO `jun`.`txttag` (`txtid`,`tagid`,`CreateTime`) VALUES (?txtid,?tagid,?CreateTime);";
                    MySqlParameter[] paramTxtTag = new MySqlParameter[]
                    {  
                        new MySqlParameter("?txtid",model.id),
                        new MySqlParameter("?tagid",item),
                        new MySqlParameter("?CreateTime",DateTime.Now)
                     };
                    MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, paramTxtTag);
                    string sqlUpdateCount = @"update tag set txtCount=(select count(*) from txttag  where tagid='1') where id=1";
                    MySqlParameter[] paramTag = new MySqlParameter[] { 
                    new MySqlParameter("?tagid",item)
                    };
                    MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sqlUpdateCount, paramTag);
                }
            }
            return 1;
        }

        public int ReCount()
        {
            string sql = "update txttype  as a set txtCount=(select count(*) from txt where type=a.id) ";
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, null);
        }

        public int AddType(string type)
        {
            string sql = @"INSERT INTO `txttype` (`title`, `content`, `CreateTime`) VALUES (?title,?content,?CreateTime);";
            MySqlParameter[] param = new MySqlParameter[]
                    {  
                        new MySqlParameter("?title",type),
                        new MySqlParameter("?content",type),
                        new MySqlParameter("?CreateTime",DateTime.Now)
                     };
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, param);
        }

        public int AddTag(string tag)
        {
            string sql = @"INSERT INTO `tag` (`title`, `CreateTime`) VALUES (?title,?CreateTime);";
            MySqlParameter[] param = new MySqlParameter[]
                    {  
                        new MySqlParameter("?title",tag),
                        new MySqlParameter("?CreateTime",DateTime.Now)
                     };
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, param);
        }

        public int DeleteType(int id)
        {
            string sql = "update txttype set deleted=1 where id=?id";
            MySqlParameter[] param = new MySqlParameter[]
                    {  
                        new MySqlParameter("?id",id)
                     };
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, param);
        }

        public int DeleteTag(int id)
        {
            string sql = "update tag set deleted=1 where id=?id";
            MySqlParameter[] param = new MySqlParameter[]
                    {  
                        new MySqlParameter("?id",id)
                     };
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, param);
        }

        public int ReloadTag()
        {
            string sql = "update tag set txtCount=(select count(*) from txttag where tag.id=txttag.tagid)";
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, null);
        }
    }
}
