using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Career.Utility
{
    /// <summary>
    /// 记录日志
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 获取调用者的命名空间.类名.方法名
        /// </summary>
        /// <returns></returns>
        public static string GetMethodInfo(string userID = "")
        {
            string str = "";
            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            str += "Namespace:" + mb.DeclaringType.Namespace + "\n";
            str += "ClassName:" + mb.DeclaringType.Name + "\n";
            str += "MethodName:" + mb.Name + "\n";
            if (userID != "-1")
            {
                str += "UserID:" + userID;
            }
            return str;
        }
        ///// <summary>
        ///// 获取mysql的参数
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public static string MySqlParam(MySql.Data.MySqlClient.MySqlParameter[] param)
        //{
        //    string result = "";
        //    try
        //    {
        //        StringBuilder sbParam = new StringBuilder();
        //        sbParam.Append("------------------参数列表---------------------------" + "\r\n");
        //        foreach (var item in param)
        //        {
        //            sbParam.Append(item.ParameterName + ":");
        //            sbParam.Append(item.Value);
        //            sbParam.Append("(" + item.DbType.ToString() + ")" + "\r\n");
        //        }
        //        result = sbParam.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        //string err = ex.Message;
        //        //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
        //    }
        //    return result;
        //}

        /// <summary>
        /// 记录日志到指定的文件里
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void Write(string fileName, string content)
        {
            try
            {
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + fileName + ".log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("======" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + "======");
                objSW.WriteLine(content);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {

                //throw;
            }
        }
        /// <summary>
        /// 获取sqlserver的参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string SqlParam(SqlParameter[] param)
        {
            string result = "";
            try
            {
                StringBuilder sbParam = new StringBuilder();
                sbParam.Append("------------------参数列表---------------------------" + "\r\n");
                foreach (var item in param)
                {
                    sbParam.Append(item.ParameterName + ":");
                    sbParam.Append(item.Value);
                    sbParam.Append("(" + item.DbType.ToString() + ")" + "\r\n");
                }
                result = sbParam.ToString();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
            return result;
        }
        /// <summary>
        /// 错误记录
        /// </summary>
        /// <param name="eException">错误文本</param>
        public static void Error(string title, Exception eException)
        {
            try
            {
                string sExp = "Title:" + title;
                sExp += "\r\n Message:" + eException.Message;
                sExp += "\r\n Source:" + eException.Source;
                sExp += "\r\n StackTrace:" + eException.StackTrace;
                sExp += "\r\n ";
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\error.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write(sExp);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 错误记录
        /// </summary>
        /// <param name="eException">错误文本</param>
        public static void FilterError(string title, Exception eException)
        {
            try
            {
                string sExp = "Title:" + title;
                sExp += "\r\n Message:" + eException.Message;
                sExp += "\r\n Source:" + eException.Source;
                sExp += "\r\n StackTrace:" + eException.StackTrace;
                sExp += "\r\n ";
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\FilterError.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write(sExp);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 打印sql语句
        /// </summary>
        /// <param name="title"></param>
        /// <param name="strmessage"></param>
        /// <param name="param"></param>
        public static void Sql(string title, string strmessage, SqlParameter[] param)
        {
            {
                try
                {

                    string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(strFilePathName))
                    {
                        Directory.CreateDirectory(strFilePathName);
                    }
                    string strFileName = strFilePathName + @"\sql.log";
                    FileStream objFS;
                    if (File.Exists(strFileName))
                    {
                        FileInfo objFileInfo = new FileInfo(strFileName);
                        objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                        objFS.Seek(0, SeekOrigin.End);
                    }
                    else
                    {
                        objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                        objFS.Seek(0, SeekOrigin.End);
                    }
                    StreamWriter objSW = new StreamWriter(objFS);
                    objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                    System.DateTime.Now.Millisecond.ToString() + "============================");
                    objSW.Write("Title:" + title + "\r\n Message:" + strmessage);
                    objSW.Write("\r\n Parameter");
                    objSW.Write(SqlParam(param));//参数列表
                    objSW.WriteLine();
                    objSW.Close();
                    objFS.Close();
                }
                catch (Exception)
                {
                    //string err = ex.Message;
                    //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 测试用的
        /// </summary>
        /// <param name="strmessage"></param>
        public static void Print(string title)
        {
            try
            {

                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("HH");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\Print.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.Write(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + "-----" + title);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strmessage"></param>
        public static void Info(string title, string strmessage)
        {
            try
            {

                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\info.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write("Title:" + title + "\r\n Message:" + strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 记录上传错误
        /// </summary>
        /// <param name="strmessage"></param>
        public static void Upload(string title, string strmessage)
        {
            try
            {

                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\Upload.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write("Title:" + title + "\r\n Message:" + strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strmessage"></param>
        public static void Timer(string title, string strmessage)
        {
            try
            {

                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\timer.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write("Title:" + title + "\r\n Message:" + strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="strmessage"></param>
        public static void Debug(string title, string strmessage)
        {
            try
            {
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\debug.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write("Title:" + title + "\r\n Message:" + strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 记录登陆日志
        /// </summary>
        /// <param name="strmessage"></param>
        public static void Login(string title, string strmessage)
        {
            try
            {

                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log" + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\login.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine("=================================" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString() + "============================");
                objSW.Write("Title:" + title + "\r\n Message:" + strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        public static void WriteLogs(string strmessage, string pathname)
        {
            try
            {
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log";
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\" + pathname + ".log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString());
                objSW.Write(strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 读文件操作
        /// </summary>
        /// <returns></returns>
        public static string ReadLogs(string FilePath)
        {
            StreamReader SR;
            string TempStr, RtnStr = "";
            if (File.Exists(FilePath))
            {
                SR = File.OpenText(FilePath);
                TempStr = SR.ReadLine();
                while (TempStr != null)
                {
                    RtnStr += TempStr + "\r\n";
                    TempStr = SR.ReadLine();

                }
                SR.Close();
            }
            return RtnStr;
        }
    }
}