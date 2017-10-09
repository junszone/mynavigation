using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
//using  System.Runtime.Serialization.Json;

namespace Career.Utility
{
    /// <summary>
    /// 公共的值转换方法
    /// </summary>
    public static partial class ValueConvert
    {
        #region 日历类型和Long类型互相转化

        #endregion
        public static long ConvertDataTimeToLong(this DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
            return timeStamp;
        }


        // long --> DateTime
        public static DateTime ConvertLongToDateTime(this long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
        /// <summary>
        /// 拼接CTS下载附件的路径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>create at 2016年11月3日11:07:22 by jun</remarks>
        public static string GetDownLoadPath(this object obj)
        {
            string attach = obj.GetString();
            var _basepath = ConfigurationManager.AppSettings["AttachmentPath"].GetString();
            string filePath = "";
            if (attach.Contains('|'))
            {//Daxtra解析文件夹和邮件
                filePath = _basepath + "//daxtrafilepath//" + attach.Split('|')[0];
            }
            else
            {//候选人上传的
                filePath = _basepath + "//uploadfilepath//" + attach;
            }
            return filePath;
        }
        #region 起始工作年限和工作年数转化
        /// <summary>
        /// 根据起始工作年限 换算出工作年数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetTotalYearByBeginDate(this object obj)
        {
            DateTime workStartTime = obj.GetDateTime(DateTime.Now);
            TimeSpan ts = workStartTime.Subtract(DateTime.Now).Duration();
            int days = ts.Days;
            int year = days / 365;
            return year;
        }
        /// <summary>
        /// 根据工作年数 换算出起始工作年限
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetBeginDateByTotalYear(this object obj)
        {
            int year = obj.GetInt(-1);
            DateTime workStartTime = DateTime.Now.AddYears(-year);
            return workStartTime;
        }
        #endregion
        #region 从简历xml中获取简历渠道来源
        public static string GetSourceChannel(this object obj)
        {
            var str = obj.GetTrim();
            string strResult = "";
            if (str.Contains("src=51job_CV"))
            {
                strResult = "51job";
            }
            else if (str.Contains("src=LIEPIN_CV"))
            {
                strResult = "猎聘";
            }
            else if (str.Contains("智联") || str.Contains("zhilian") || str.Contains("src=ZHAOPIN_CV"))
            {
                strResult = "智联";
            }
            else
            {
                strResult = "";
            }
            return strResult;
        }
        #endregion

        #region 从简历全文中匹配候选人编号
        /// <summary>
        /// 过滤重复的简历编号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string FilterRepeatExternalID(this object obj)
        {
            string str = obj.GetTrim();
            string strResult = string.Empty;
            if (!String.IsNullOrWhiteSpace(str))
            {
                string[] arr = str.Split(' ');
                foreach (var item in arr)
                {
                    if (!strResult.Contains(item))
                    {
                        strResult = strResult + " " + item;
                    }
                }
            }
            return strResult;
        }
      
        #endregion
        #region 加密解密
        /// <summary>
        /// 当前程序加密所使用的密钥

        /// </summary>
        public static readonly string myKey = "q0m3sd8l";

        #region 加密方法
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">需要加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(this string pToEncrypt)
        {
            try
            {
                string sKey = myKey;
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                //把字符串放到byte数组中


                //原来使用的UTF8编码，我改成Unicode编码了，不行
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

                //建立加密对象的密钥和偏移量


                //使得输入密码必须输入英文文本
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("写入配置信息失败，详细信息：", ex);
            }

            return "";
        }
        #endregion

        #region 解密方法
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密匙</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(this string pToDecrypt)
        {
            try
            {
                string sKey = myKey;
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                //建立加密对象的密钥和偏移量，此值重要，不能修改
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error("读取配置信息失败，详细信息：", ex);
            }
            return "";
        }
        #endregion
        #endregion

        #region 薪资显示
        public static string GetTotalYearShow(this object obj)
        {
            int total = obj.GetInt(-1);
            if (total == -1)
            {
                return "";
            }
            else
            {
                if (total > 30)
                {
                    return "30年以上";
                }
                else
                {
                    if (total == 0)
                    {
                        return "";//应届生(有很多数据是0的不准,所以不能显示实习生)
                    }
                    else
                    {
                        return total.GetString() + "年";
                    }
                }
            }
        }
        /// <summary>
        /// 薪资显示
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetSalaryRangeShow(this object obj)
        {
            string str = obj.GetTrim().Replace("W", "");

            if (str == "-1" || str == "")
            {
                return "";
            }
            else if (str == "0-0")
            {
                return "面议";
            }
            else if (str == "200-99999")
            {
                return ">200" + "W";
            }
            else
            {
                return str + "W";
            }
        }
        public static string GetSalaryRangeCnShow(this object obj)
        {
            string str = obj.GetTrim().Replace("W", "");

            if (str == "-1" || str == "")
            {
                return "";
            }
            else if (str == "0-0")
            {
                return "面议";
            }
            else if (str == "200-99999")
            {
                return ">200" + "万";
            }
            else
            {
                return str + "万";
            }
        }
        #endregion
        /// <summary>
        /// 是否是电话号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsPhone(this object obj)
        {
            var regexTelephone = new Regex(@"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$");
            var regexMobilePhone = new Regex(@"^[1][358]\d{9}$");
            while (true)
            {
                var inputString = obj.GetString();
                if (string.IsNullOrWhiteSpace(inputString)) continue;
                if (regexTelephone.IsMatch(inputString) || regexMobilePhone.IsMatch(inputString))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 奔驰固定电话格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>(8621)-61602715</remarks>
        public static string GetPhoneForBenz(this object obj)
        {
            string str = obj.GetString();
            string strResult = string.Empty;
            if (str.Contains("-"))
            {
                var arr = str.Split('-');
                string areaCode = arr[0];
                if (areaCode.Contains("86"))
                {
                    areaCode = "(" + areaCode + ")";
                }
                else
                {
                    int areaCodeInt = areaCode.GetInt();
                    areaCode = "(86" + areaCodeInt.GetString() + ")";
                }
                strResult = areaCode + "-" + arr[1];
            }
            return strResult;
        }
        /// <summary>
        ///  奔驰固定手机格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks> 86-15900575181</remarks>
        public static string GetMobileForBenz(this object obj)
        {
            string str = obj.GetString();
            string strResult = string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (str.Length >= 2)
                {
                    if (str.Substring(0, 2) != "86")
                    {
                        strResult = "86-" + str;
                    }
                }
            }
            return strResult;
        }
        /// <summary>
        /// 隐藏手机号中间的几位数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetMobileHide(this object obj, int begin = 3, int end = 4)
        {
            string str = obj.GetString();
            if (str.Length >= (begin + end))
            {
                string beginStr = str.Substring(0, begin);
                string endStr = str.Remove(0, str.Length - end);
                return beginStr + "xxxx" + endStr;
            }
            else
            {
                return str;
            }
        }
        /// <summary>
        /// 隐藏邮箱中间的部分
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="begin"></param>
        /// <returns></returns>
        public static string GetEmailHide(this object obj, int begin = 2)
        {
            string str = obj.GetString();
            string strResult = string.Empty;
            if (str.Contains('@'))
            {
                var arr = str.Split('@');
                if (arr[0].Length > begin)
                {
                    string beginStr = arr[0].Substring(0, begin);
                    strResult = beginStr + "xxxxx" + "@" + arr[1];
                }
                else
                {
                    strResult = "xxxxx" + "@" + arr[1];
                }
            }
            else
            {
                strResult = str;
            }
            return strResult;
        }
        /// <summary>
        /// 从顾问的名字的提取英文名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>英文名</returns>
        /// <remarks>Jack Yang(杨利军)为参数，返回Jack Yang</remarks>
        public static string GetEnglishByEmpName(this object obj)
        {
            string str = obj.GetTrim();
            string strResult = string.Empty;
            if (str.Contains("("))
            {
                strResult = str.Split('(')[0].GetString();
            }
            else
            {
                strResult = str;
            }
            return strResult;
        }
        /// <summary>
        /// 转化日期时间(主要是处理解析出来的present和/1)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDateTimeStandard(this object obj)
        {
            string str = obj.GetTrim();
            string strResult = string.Empty;
            if (str == "present" || str == "至今")
            {
                strResult = "2099-01-01";
            }
            else if (str == "/1")
            {
                strResult = "";
            }
            else if (str.GetDateTime().Year == 1)
            {
                strResult = "1900-01-01";
            }
            else
            {
                strResult = str;
            }
            return strResult;
        }
        /// <summary>
        /// 日期显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDateTimeShow(this object str)
        {
            string strShow = str.GetTrim();
            string returnFlag = strShow;
            if (strShow == "-1" || strShow == "/1")
            {
                returnFlag = "";
            }
            else if (strShow.Contains("2099") || strShow.Contains("2999"))
            {
                returnFlag = "Present";
            }
            else
            {
                returnFlag = strShow;
            }
            return returnFlag;
        }
        public static string EmptyShow(this object str)
        {
            string _str = str.GetString("");
            string _defaultValue = "";
            if (string.IsNullOrWhiteSpace(_str))
            {
                return _defaultValue;
            }
            else
            {
                return _str;
            }
        }
        /// <summary>
        /// 设置主键ID
        /// </summary>
        /// <returns></returns>
        public static string SetRandomId(this DateTime dt)
        {
            Random r = new Random();
            return dt.ToString("yyMMddHHmmssffff") + r.Next(100, 999);
        }
        /// <summary>
        /// 是否是汉字
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static bool IsChinese(this object CString)
        {
            return Regex.IsMatch(CString.GetString(""), @"^[\u4e00-\u9fa5]+$");
        }
        /// <summary>
        /// Event Contact Type
        /// </summary>
        /// <param name="contacType"></param>
        /// <returns></returns>
        public static string GetContactType(this object contacType)
        {
            string result = "";
            int type_ = contacType.GetInt();
            switch (type_)
            {
                case 0:
                    result = "Telephone Call";
                    break;
                case 1:
                    result = "CV Sent";
                    break;
                case 2:
                    result = "Candidate Interview";
                    break;
                case 3:
                    result = "1st Interview";
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// 根据年龄计算生日
        /// </summary>
        /// <param name="Age"></param>
        /// <returns></returns>
        public static string GetBirthday(this object Age)
        {
            int _age = -Age.GetInt(0);
            return DateTime.Now.AddYears(_age) > DateTime.Now ? "" : DateTime.Now.AddYears(_age).ToString("yyyy-01-01");//如果生日大于当前日期就显示空
        }
        /// <summary>
        /// 根据生日计算年龄
        /// </summary>
        /// <param name="Birthday"></param>
        /// <returns></returns>
        public static int GetAge(this object Birthday)
        {
            try
            {
                string _birthday = Birthday.GetString();
                if (!string.IsNullOrWhiteSpace(_birthday))
                {
                    DateTime bri = Birthday.GetDateTime();
                    TimeSpan ts = DateTime.Now - bri;
                    int year = (int)(ts.TotalDays / 365);
                    return year > 60 ? 0 : year;//如果大于60就为零(错误数据)
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Log.Error("GetAge." + Birthday.GetString(), e);
                return 0;
            }

        }
        public static string GetAgeStr(this object Birthday)
        {
            string _birthday = Birthday.GetString();
            if (!string.IsNullOrWhiteSpace(_birthday))
            {
                DateTime bri = Birthday.GetDateTime();
                TimeSpan ts = DateTime.Now - bri;
                int year = (int)(ts.TotalDays / 365);
                return year > 60 ? "" : year.ToString();////如果大于60就为空
            }
            else
            {
                return "";
            }
        }

        public static string ud(this string str)
        {
            return System.Web.HttpUtility.UrlDecode(str).Trim();
        }
        /// <summary>
        /// 截取字符串长度
        /// </summary>
        /// <param name="strObj"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutStr(this object strObj, int len)
        {
            if (strObj == null)
            {
                return string.Empty;
            }
            else
            {
                string str = strObj.ToString();
                int l = str.Length;
                if (l == 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (len < l)
                    {
                        return str.Substring(0, len);
                    }
                    else
                    {
                        return str;
                    }
                }

            }

        }
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {

            // 半角转全角：
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 32)
                {
                    array[i] = (char)12288;
                    continue;
                }
                if (array[i] < 127)
                {
                    array[i] = (char)(array[i] + 65248);
                }
            }
            return new string(array);
        }



        /// <summary>
        /// 转半角的函数(DBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }

        public static string ShowEmpty(this string str)
        {
            return (string.IsNullOrWhiteSpace(str) || str.Trim() == "0") ? "" : str.Trim();
        }
        //public static string ToUnicodeString(this string str)
        //{
        //    StringBuilder strResult = new StringBuilder();
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        for (int i = 0; i < str.Length; i++)
        //        {
        //            strResult.Append("\\u");
        //            strResult.Append(((int)str[i]).ToString("x"));
        //        }
        //    }
        //    return strResult.ToString();
        //}

        //public static string FromUnicodeString(this string str)
        //{
        //    //最直接的方法Regex.Unescape(str);
        //    StringBuilder strResult = new StringBuilder();
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        //string[] strlist = str.Replace("\\", "").Split('u');
        //        string[] strlist = str.Replace("&", "").Replace("#", "").TrimEnd(';').Split(';');
        //        try
        //        {
        //            for (int i = 1; i < strlist.Length; i++)
        //            {
        //                long charCode = Convert.ToInt64(strlist[i], 16);
        //                strResult.Append((char)charCode);
        //            }
        //        }
        //        catch (FormatException ex)
        //        {
        //            return Regex.Unescape(str);
        //        }
        //    }
        //    return strResult.ToString();
        //}

        ///  <summary>
        ///  使用MD5加密字符串
        ///  </summary>
        ///  <param  name="str">待加密的字符</param>
        ///  <returns></returns>
        public static string MD5(this  string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] arr = UTF8Encoding.Default.GetBytes(str);
            byte[] bytes = md5.ComputeHash(arr);
            str = BitConverter.ToString(bytes);
            str = str.Replace("-", "");
            return str;
        }
        ///  <summary>
        ///  将最后一个字符串的路径path替换
        ///  </summary>
        ///  <param  name="str"></param>
        ///  <param  name="path"></param>
        ///  <returns></returns>
        public static string Path(this  string str, string path)
        {
            int index = str.LastIndexOf('\\');
            int indexDian = str.LastIndexOf('.');
            return str.Substring(0, index + 1) + path + str.Substring(indexDian);
        }
        public static List<string> ToList(this  string ids)
        {
            List<string> listId = new List<string>();
            if (!string.IsNullOrEmpty(ids))
            {
                var sort = new SortedSet<string>(ids.Split(','));
                foreach (var item in sort)
                {
                    listId.Add(item);

                }
            }
            return listId;
        }
        ///  <summary>
        ///  从^分割的字符串中获取多个Id,先是用  ^  分割，再使用  &  分割
        ///  </summary>
        ///  <param  name="ids">先是用  ^  分割，再使用  &  分割</param>
        ///  <returns></returns>
        public static List<string> GetIdSort(this  string ids)
        {
            List<string> listId = new List<string>();
            if (!string.IsNullOrEmpty(ids))
            {
                var sort = new SortedSet<string>(ids.Split('^')
                        .Where(w => !string.IsNullOrWhiteSpace(w) && w.Contains('&'))
                        .Select(s => s.Substring(0, s.IndexOf('&'))));
                foreach (var item in sort)
                {
                    listId.Add(item);
                }
            }
            return listId;
        }
        ///  <summary>
        ///  从，分割的字符串中获取单个Id
        ///  </summary>
        ///  <param  name="ids"></param>
        ///  <returns></returns>
        public static string GetId(this  string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var sort = new SortedSet<string>(ids.Split('^')
                        .Where(w => !string.IsNullOrWhiteSpace(w) && w.Contains('&'))
                        .Select(s => s.Substring(0, s.IndexOf('&'))));
                foreach (var item in sort)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
        ///  <summary>
        ///  从，分割的字符串中获取单个Id
        ///  </summary>
        ///  <param  name="ids"></param>
        ///  <returns></returns>
        public static string GetShortDateTime(this  DateTime? time)
        {
            return Convert.ToDateTime(time).ToString("yyyy-MM-dd");
        }

        /////  <summary>
        /////  将String转换为Dictionary类型，过滤掉为空的值，首先  6  分割，再  7  分割
        /////  </summary>
        /////  <param  name="value"></param>
        /////  <returns></returns>
        //public static Dictionary<string, string> StringToDictionary(string value)
        //{
        //    Dictionary<string, string> queryDictionary = new Dictionary<string, string>();
        //    if (value != "")
        //    {
        //        var obj = JSONHelper.FromJson<DataOrder>(value).rules;


        //        string[] s = value.Split('^');
        //        for (int i = 0; i < s.Length; i++)
        //        {
        //            if (!string.IsNullOrWhiteSpace(s[i]) && !s[i].Contains("undefined"))
        //            {
        //                var ss = s[i].Split('&');
        //                if ((!string.IsNullOrEmpty(ss[0])) && (!string.IsNullOrEmpty(ss[1])))
        //                {
        //                    queryDictionary.Add(ss[0], ss[1]);
        //                }
        //            }

        //        }
        //        if (obj.Count > 0)
        //        {
        //            foreach (var item in obj)
        //            {
        //                queryDictionary.Add(item.field.Trim(), item.value.Trim());
        //            }
        //            return queryDictionary;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
        ///  <summary>
        ///  得到对象的  Int  类型的值，默认值0
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值0</returns>
        public static int GetInt(this  object Value)
        {
            return GetInt(Value, 0);
        }
        ///  <summary>
        ///  得到对象的  Int  类型的值，默认值0
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <param  name="defaultValue">如果转换失败，返回的默认值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值0</returns>
        public static int GetInt(this  object Value, int defaultValue)
        {

            if (Value == null) return defaultValue;
            if (Value is string && Value.GetString().HasValue() == false) return defaultValue;

            if (Value is DBNull) return defaultValue;

            if ((Value is string) == false && (Value is IConvertible) == true)
            {
                return (Value as IConvertible).ToInt32(CultureInfo.CurrentCulture);
            }

            int retVal = defaultValue;
            if (int.TryParse(Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out  retVal))
            {
                return retVal;
            }
            else
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 获得婚姻状态显示
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetMarriage(this object Value)
        {
            string result = "未知";
            string str = Value.GetString().Trim().ToLower();
            //Single/Married/Divorced/Confidential

            if (str.Contains("single") || str.Contains("单身"))
            {
                result = "单身";
            }
            else if (str.Contains("married") || str.Contains("已婚"))
            {
                result = "已婚";
            }
            else if (str.Contains("divorced") || str.Contains("离婚") || str.Contains("离异"))
            {
                result = "离异";
            }
            else if (str.Contains("confidential") || str.Contains("保密"))
            {
                result = "保密";
            }
            return result;
        }
        /// <summary>
        /// 性别index
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int GetSexValue(this object Value)
        {
            int result = 0;
            string str = Value.GetString();
            if (str.Trim().ToLower().Contains("female") || str.Contains("0") || str.Contains("女"))
            {
                result = 0;
            }
            else
            {
                result = 1;
            }
            return result;
        }
        /// <summary>
        /// 性别显示
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetSex(this object Value)
        {
            string result = "";
            string str = Value.GetString();
            if (str.Trim().ToLower().Contains("female") || str.Contains("0") || str.Contains("女") || str.ToLower().Contains("woman") || str.ToLower().Contains("women") || str.ToLower().Contains("nv"))
            {
                result = "女";
            }
            else if (str.Trim().ToLower().Contains("male") || str.Contains("1") || str.Contains("男") || str.ToLower().Contains("man") || str.ToLower().Contains("men") || str.ToLower().Contains("nan"))
            {
                result = "男";
            }
            else
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// 性别显示(英文Mr/Ms)
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetSexShowEn(this object Value)
        {
            string result = "";
            string str = Value.GetTrim().ToLower();
            if (str.Contains("female") || str.Contains("0") || str.Contains("女") || str.Contains("woman") || str.Contains("women") || str.Contains("nv") || str.Contains("Ms"))
            {
                result = "Ms";
            }
            else if (str.Contains("male") || str.Contains("1") || str.Contains("男") || str.Contains("man") || str.Contains("men") || str.Contains("nan") || str.Contains("Mr"))
            {
                result = "Mr";
            }
            else
            {
                result = "";
            }
            return result;
        }
        /// <summary>
        /// 薪资（1-5W转化为10-50K）
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetRange_W2K(this object Value)
        {
            string val = Value.GetString();
            if (val.Contains("-"))
            {
                var arr = val.Split('-');
                string start = (arr[0].GetInt() * 10).GetString();
                string end = (arr[1].GetInt() * 10).GetString();
                return start + "-" + end;
            }
            else
            {
                return "";
            }
        }
        ///  <summary>
        ///  得到对象的  Int  类型的值，默认值0
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值0</returns>
        public static decimal GetDecimal(this  object Value)
        {
            return GetDecimal(Value, 0);
        }
        ///  <summary>
        ///  得到对象的  Int  类型的值，默认值0
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <param  name="defaultValue">如果转换失败，返回的默认值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值0</returns>
        public static decimal GetDecimal(this  object Value, decimal defaultValue)
        {

            if (Value == null) return defaultValue;
            if (Value is string && Value.GetString().HasValue() == false) return defaultValue;

            if (Value is DBNull) return defaultValue;

            if ((Value is string) == false && (Value is IConvertible) == true)
            {
                return (Value as IConvertible).ToDecimal(CultureInfo.CurrentCulture);
            }

            decimal retVal = defaultValue;
            if (decimal.TryParse(Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out  retVal))
            {
                return retVal;
            }
            else
            {
                return defaultValue;
            }
        }
        ///  <summary>
        ///  得到对象的  String  类型的值，默认值string.Empty
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值string.Empty</returns>
        public static string GetString(this  object Value)
        {
            return GetString(Value, string.Empty);
        }
        public static string GetTrim(this object Value)
        {
            return Value.GetString().Trim();
        }
        ///  <summary>
        ///  得到对象的  String  类型的值，默认值string.Empty
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <param  name="defaultValue">如果转换失败，返回的默认值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值  。</returns>
        public static string GetString(this  object Value, string defaultValue)
        {
            if (Value == null) return defaultValue;
            string retVal = defaultValue;
            try
            {
                var strValue = Value as string;
                if (strValue != null)
                {
                    return strValue;
                }

                char[] chrs = Value as char[];
                if (chrs != null)
                {
                    return new string(chrs);
                }

                retVal = Value.ToString();
            }
            catch
            {
                return defaultValue;
            }
            return retVal;
        }
        ///  <summary>
        ///  得到对象的  String  类型的值，默认值string.Empty
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值string.Empty</returns>
        public static string GetXmlEscapeChar(this  object Value)
        {
            return GetXmlEscapeChar(Value, string.Empty);
        }
        ///  <summary>
        ///  C#在引用XML文件时时只需要将读出的字符串进行替换就行了
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <param  name="defaultValue">如果转换失败，返回的默认值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值  。</returns>
        public static string GetXmlEscapeChar(this  object Value, string defaultValue)
        {
            if (Value == null) return defaultValue;
            string retVal = defaultValue;
            try
            {
                var strValue = Value as string;
                if (strValue != null)
                {
                    return strValue;
                }

                char[] chrs = Value as char[];
                if (chrs != null)
                {
                    return new string(chrs);
                }

                retVal = replaceSpecialChar(Value);
            }
            catch
            {
                return defaultValue;
            }
            return retVal;
        }
        /// <summary>
        ///  C#在引用XML文件时时只需要将读出的字符串进行替换就行了
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string replaceSpecialChar(object strSource)
        {
            var strTemp = strSource.GetString();
            strTemp = strTemp.Replace("&amp;", "&");
            strTemp = strTemp.Replace("&lt;", "<");
            strTemp = strTemp.Replace("&gt;", ">");
            strTemp = strTemp.Replace("&apos;", "\'");
            strTemp = strTemp.Replace("&quot;", "\"");
            return strTemp;
        }
        /// <summary>
        /// 转义字符串中所有正则特殊字符
        /// </summary>
        /// <param name="input">传入字符串</param>
        /// <returns></returns>
        public static string FilterString(this string input)
        {
            input = input.Replace("\\", "\\\\");//先替换“\”，不然后面会因为替换出现其他的“\”

            Regex r = new Regex("[\\*\\.\\?\\+\\$\\^\\[\\]\\(\\)\\{\\}\\|\\/]");
            MatchCollection ms = r.Matches(input);
            List<string> list = new List<string>();
            foreach (Match item in ms)
            {
                if (list.Contains(item.Value))
                    continue;
                input = input.Replace(item.Value, "\\" + item.Value);
                list.Add(item.Value);
            }
            return input;
        }


        ///  <summary>
        ///  得到对象的  DateTime  类型的值，默认值为DateTime.MinValue
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回的默认值为DateTime.MinValue  </returns>
        public static DateTime GetDateTime(this  object Value)
        {
            if (Value.GetDateTime(Convert.ToDateTime("1900-01-01")).Year == 1)
            {
                return Convert.ToDateTime("1900-01-01");
            }
            else
            {
                return GetDateTime(Value, Convert.ToDateTime("1900-01-01"));
            }
        }

        ///  <summary>
        ///  得到对象的  DateTime  类型的值，默认值为DateTime.MinValue
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <param  name="defaultValue">如果转换失败，返回默认值为DateTime.MinValue</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回的默认值为DateTime.MinValue</returns>
        public static DateTime GetDateTime(this  object Value, DateTime defaultValue)
        {
            if (Value == null) return defaultValue;

            if (Value is DBNull) return defaultValue;

            string strValue = Value as string;
            if (strValue == null && (Value is IConvertible))
            {
                return (Value as IConvertible).ToDateTime(CultureInfo.CurrentCulture);
            }
            if (strValue != null)
            {
                strValue = strValue
                        .Replace("年", "-")
                        .Replace("月", "-")
                        .Replace("日", "-")
                        .Replace("点", ":")
                        .Replace("时", ":")
                        .Replace("分", ":")
                        .Replace("秒", ":")
                            ;
            }
            DateTime dt = defaultValue;
            if (DateTime.TryParse(Value.GetString(), out  dt))
            {
                return dt;
            }

            return defaultValue;
        }
        ///  <summary>
        ///  得到对象的布尔类型的值，默认值false
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值false</returns>
        public static bool GetBool(this  object Value)
        {
            return GetBool(Value, false);
        }

        ///  <summary>
        ///  得到对象的  Bool  类型的值，默认值false
        ///  </summary>
        ///  <param  name="Value">要转换的值</param>
        ///  <param  name="defaultValue">如果转换失败，返回的默认值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值false</returns>
        public static bool GetBool(this  object Value, bool defaultValue)
        {
            if (Value == null) return defaultValue;
            if (Value is string && Value.GetString().HasValue() == false) return defaultValue;

            if ((Value is string) == false && (Value is IConvertible) == true)
            {
                if (Value is DBNull) return defaultValue;

                try
                {
                    return (Value as IConvertible).ToBoolean(CultureInfo.CurrentCulture);
                }
                catch { }
            }

            if (Value is string)
            {
                if (Value.GetString() == "0") return false;
                if (Value.GetString() == "1") return true;
                if (Value.GetString().ToLower() == "yes") return true;
                if (Value.GetString().ToLower() == "no") return false;
            }
            ///    if  (Value.GetInt(0)  !=  0)  return  true;
            bool retVal = defaultValue;
            if (bool.TryParse(Value.GetString(), out  retVal))
            {
                return retVal;
            }
            else return defaultValue;
        }
        ///  <summary>
        ///  检测  GuidValue  是否包含有效的值，默认值Guid.Empty
        ///  </summary>
        ///  <param  name="GuidValue">要转换的值</param>
        ///  <returns>如果对象的值可正确返回，  返回对象转换的值  ，否则，  返回默认值Guid.Empty</returns>
        public static Guid GetGuid(string GuidValue)
        {
            try
            {
                return new Guid(GuidValue);
            }
            catch { return Guid.Empty; }
        }
        ///  <summary>
        ///  检测  Value  是否包含有效的值，默认值false
        ///  </summary>
        ///  <param  name="Value">  传入的值</param>
        ///  <returns>  包含，返回true，不包含，返回默认值false</returns>
        public static bool HasValue(this  string Value)
        {
            if (Value != null)
            {
                return !string.IsNullOrEmpty(Value.ToString());
            }
            else return false;
        }
        #region 获取随机数
        /// <summary>
        /// 取得随机字符串
        /// </summary>
        /// <param name="pwdchars"></param>
        /// <param name="pwdlen"></param>
        /// <returns></returns>
        public static string GetRandomString(this string pwdchars, int pwdlen)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                int num = random.Next(pwdchars.Length);
                builder.Append(pwdchars[num]);
            }
            return builder.ToString();
        }
        #endregion
        #region 根据文件路径获取文件的Hash值,用于对比文件是否完全一致

        /// <summary>
        /// 获取文件的hash值
        /// </summary>
        /// <param name="path1">文件路径</param>
        /// <returns></returns>
        public static string GetFileHashString(this string path1)
        {
            //计算文件的哈希值
            var hash = System.Security.Cryptography.HashAlgorithm.Create();
            var stream_1 = new System.IO.FileStream(path1, System.IO.FileMode.Open);
            byte[] hashByte_1 = hash.ComputeHash(stream_1);
            stream_1.Close();
            string bit1 = BitConverter.ToString(hashByte_1);
            return bit1;
        }
        /// <summary>
        /// 获取文件的hash值
        /// </summary>
        /// <param name="path1">文件路径</param>
        /// <returns></returns>
        public static string GetFileHashString(this FileStream stream_1)
        {
            //计算文件的哈希值
            var hash = System.Security.Cryptography.HashAlgorithm.Create();
            //var stream_1 = new System.IO.FileStream(path1, System.IO.FileMode.Open);
            byte[] hashByte_1 = hash.ComputeHash(stream_1);
            stream_1.Close();
            string bit1 = BitConverter.ToString(hashByte_1);
            return bit1;
        }
        #endregion
    }
}
