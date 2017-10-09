
using Jun.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace Jun.DAL
{
    public class PeopleDAL
    {
        private static PeopleDAL instance;
        public static PeopleDAL GetInstance() { return instance ?? (instance = new PeopleDAL()); }
        private PeopleDAL() { }
        public List<People> Retrieve(People param)
        {
            StringBuilder sb = new StringBuilder();// sql = "select * from `server` ";
            sb.Append("select * from `people` where 1=1 ");
            if (!String.IsNullOrWhiteSpace(param.keyword))
            {
                var arr = param.keyword.Split(' ').ToList();
                sb.Append(" and  ");
                foreach (string key in arr)
                {
                    var index = arr.IndexOf(key);//
                    if (index > 0)
                    {
                        sb.AppendFormat(" {0} ", param.logic);
                    }
                    sb.AppendFormat("  (RelationShip like '%{0}%' or RelationshipCompany like '%{0}%' or ChineseName like '%{0}%'  or  EnglishName like '%{0}%' or  NickName like '%{0}%' or Address_Registered like '%{0}%' or Address_post like '%{0}%' or Remarks like '%{0}%' or Phone like '%{0}%'  or Email like '%{0}%'  or QQ like '%{0}%'  or IDNumber like '%{0}%'  or Gender like '%{0}%'   or Keyworks like '%{0}%' )", key);
                }
            }
            else
            {
                //if (!String.IsNullOrWhiteSpace(param.level))
                //{
                //    sb.AppendFormat(" and level like '%{0}%' ", param.level);
                //}
                //if (!String.IsNullOrWhiteSpace(param.type))
                //{
                //    sb.AppendFormat(" and type like '%{0}%' ", param.type);
                //}
                //if (!String.IsNullOrWhiteSpace(param.name))
                //{
                //    sb.AppendFormat(" and name like '%{0}%' ", param.name);
                //}
                //if (!String.IsNullOrWhiteSpace(param.url))
                //{
                //    sb.AppendFormat(" and url like '%{0}%' ", param.url);
                //}
                //if (!String.IsNullOrWhiteSpace(param.ip))
                //{
                //    sb.AppendFormat(" and ip like '%{0}%' ", param.ip);
                //}
                //if (!String.IsNullOrWhiteSpace(param.user))
                //{
                //    sb.AppendFormat(" and user like '%{0}%' ", param.user);
                //}
                //if (!String.IsNullOrWhiteSpace(param.content))
                //{
                //    sb.AppendFormat(" and content like '%{0}%' ", param.content);
                //}
                //if (!String.IsNullOrWhiteSpace(param.description))
                //{
                //    sb.AppendFormat(" and description like '%{0}%' ", param.description);
                //}
            }
            sb.Append("  order by `order` desc ");
            List<People> list = new List<People>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sb.ToString()))
            {
                while (dr.Read())
                {
                    People model = new People();
                    model.ID = dr["ID"].ToString();
                    model.Relationship = dr["Relationship"].ToString();
                    model.RelationshipCompany = dr["RelationshipCompany"].ToString();
                    model.ChineseName = dr["ChineseName"].ToString();
                    model.EnglishName = dr["EnglishName"].ToString();
                    model.NickName = dr["NickName"].ToString();
                    model.MarriageTime = dr["MarriageTime"].ToString();
                    model.Address_Registered = dr["Address_Registered"].ToString();
                    model.Address_Post = dr["Address_Post"].ToString();
                    model.Remarks = dr["Remarks"].ToString();
                    model.Phone = dr["Phone"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.QQ = dr["QQ"].ToString();
                    model.IDNumber = dr["IDNumber"].ToString();
                    model.Gender = dr["Gender"].ToString();
                    model.Birthday = dr["Birthday"].ToString();
                    model.Hobby = dr["Hobby"].ToString();
                    model.CurrentCity = dr["CurrentCity"].ToString();
                    model.PostalCode = dr["PostalCode"].ToString();
                    model.MobilePhone = dr["MobilePhone"].ToString();
                    model.Homepage = dr["Homepage"].ToString();
                    model.CreateTime = dr["CreateTime"].ToString();
                    model.Keyworks = dr["Keyworks"].ToString();
                    model.UpdateTime = dr["UpdateTime"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }
        public MySqlParameter[] GetParam(People model)
        {
            MySqlParameter[] paramters = new MySqlParameter[]
                          {  
                                new MySqlParameter("?ID",model.ID),
                             new MySqlParameter("?Relationship",model.Relationship),
                             new MySqlParameter("?RelationshipCompany",model.RelationshipCompany),
                             new MySqlParameter("?ChineseName",model.ChineseName),
                            new MySqlParameter("?EnglishName",model.EnglishName),
                            new MySqlParameter("?NickName",model.NickName),
                            new MySqlParameter("?MarriageTime",model.MarriageTime),
                            new MySqlParameter("?Address_Registered",model.Address_Registered),
                            new MySqlParameter("?Address_Post",model.Address_Post),
                            new MySqlParameter("?Remarks",model.Remarks),
                            new MySqlParameter("?Phone",model.Phone),
                            new MySqlParameter("?Email",model.Email),
                            new MySqlParameter("?QQ",model.QQ),
                            new MySqlParameter("?IDNumber",model.IDNumber),
                            new MySqlParameter("?Gender",model.Gender),
                            new MySqlParameter("?Birthday",model.Birthday),
                            new MySqlParameter("?Hobby",model.Hobby),
                            new MySqlParameter("?CurrentCity",model.CurrentCity),
                            new MySqlParameter("?PostalCode",model.PostalCode),
                            new MySqlParameter("?MobilePhone",model.MobilePhone),
                            new MySqlParameter("?Homepage",model.Homepage),
                            new MySqlParameter("?CreateTime",model.CreateTime),
                            new MySqlParameter("?Keyworks",model.Keyworks),
                            new MySqlParameter("?UpdateTime",model.UpdateTime)
                          };
            return paramters;
        }
        public People RetrieveByID(int id)
        {
            string sql = "select * from People where id=" + id;
            People model = new People();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, sql))
            {
                while (dr.Read())
                {
                    model.ID = dr["ID"].ToString();
                    model.Relationship = dr["Relationship"].ToString();
                    model.RelationshipCompany = dr["RelationshipCompany"].ToString();
                    model.ChineseName = dr["ChineseName"].ToString();
                    model.EnglishName = dr["EnglishName"].ToString();
                    model.NickName = dr["NickName"].ToString();
                    model.MarriageTime = dr["MarriageTime"].ToString();
                    model.Address_Registered = dr["Address_Registered"].ToString();
                    model.Address_Post = dr["Address_Post"].ToString();
                    model.Remarks = dr["Remarks"].ToString();
                    model.Phone = dr["Phone"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.QQ = dr["QQ"].ToString();
                    model.IDNumber = dr["IDNumber"].ToString();
                    model.Gender = dr["Gender"].ToString();
                    model.Birthday = dr["Birthday"].ToString();
                    model.Hobby = dr["Hobby"].ToString();
                    model.CurrentCity = dr["CurrentCity"].ToString();
                    model.PostalCode = dr["PostalCode"].ToString();
                    model.MobilePhone = dr["MobilePhone"].ToString();
                    model.Homepage = dr["Homepage"].ToString();
                    model.CreateTime = dr["CreateTime"].ToString();
                    model.Keyworks = dr["Keyworks"].ToString();
                    model.UpdateTime = dr["UpdateTime"].ToString();
                }
                return model;
            }
        }

        public int Edit(People model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {//新增
                string sql = @"INSERT INTO `People` (
	`Relationship`,
	`RelationshipCompany`,
	`ChineseName`,
	`EnglishName`,
	`NickName`,
	`MarriageTime`,
	`Address_Registered`,
	`Address_Post`,
	`Remarks`,
	`Phone`,
	`Email`,
	`QQ`,
	`IDNumber`,
	`Gender`,
	`Birthday`,
	`Hobby`,
	`CurrentCity`,
	`PostalCode`,
	`MobilePhone`,
	`Homepage`,
	`CreateTime`,
	`Keyworks`,
	`UpdateTime`
)
VALUES
	(
	  ?Relationship,                  
	  ?RelationshipCompany,     
	  ?ChineseName,                
	  ?EnglishName ,                
	  ?NickName ,                    
	  ?MarriageTime,            
	  ?Address_Registered,    
	  ?Address_Post,               
	  ?Remarks,                        
	  ?Phone,                            
	  ?Email,                             
	  ?QQ,                               
	  ?IDNumber,                    
	  ?Gender,                          
	  ?Birthday ,                      
	  ?Hobby,                           
	  ?CurrentCity,                   
	  ?PostalCode,                    
	  ?MobilePhone,                 
	  ?Homepage,                     
	  ?CreateTime,                   
	  ?Keyworks ,                     
	  ?UpdateTime                  
	);
";
                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model));
            }
            else
            {//编辑
                string sql = @"
UPDATE `people`
SET 
 `Relationship`=?Relationship,
 `RelationshipCompany`=?RelationshipCompany,
 `ChineseName`=?ChineseName,
 `EnglishName`=?EnglishName,
 `NickName`=?NickName,
 `MarriageTime`=?MarriageTime,
 `Address_Registered`=?Address_Registered,
 `Address_Post`=?Address_Post,
 `Remarks`=?Remarks,
 `Phone`=?Phone,
 `Email`=?Email,
 `QQ`=?QQ,
 `IDNumber`=?IDNumber,
 `Gender`=?Gender,
 `Birthday`=?Birthday,
 `Hobby`=?Hobby,
 `CurrentCity`=?CurrentCity,
 `PostalCode`=?PostalCode,
 `MobilePhone`=?MobilePhone,
 `Homepage`=?Homepage,
 `CreateTime`=?CreateTime,
 `Keyworks`=?Keyworks,
 `UpdateTime`=?UpdateTime
WHERE
	(`ID`=?ID);
";
                return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, GetParam(model));
            }
        }

        public int Delete(int id)
        {
            string sql = "delete from People where id=" + id;
            return MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql, null);
        }
    }
}
