using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;


namespace Career.Utility
{
    public class Post
    {
        /// <summary>
        /// 以Post 形式提交数据及file内容到 uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="files"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static byte[] PostFile(Uri uri, IEnumerable<UploadFile1> files, NameValueCollection values)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            MemoryStream stream = new MemoryStream();


            byte[] line = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");



            if (values != null)
            {
                string format = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
                foreach (string key in values.Keys)
                {
                    string s = string.Format(format, key, values[key]);
                    byte[] data = Encoding.UTF8.GetBytes(s);
                    stream.Write(data, 0, data.Length);
                }
                stream.Write(line, 0, line.Length);
            }


            if (files != null)
            {
                string fformat = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";
                foreach (UploadFile1 file in files)
                {
                    string s = string.Format(fformat, file.Name, file.Filename);
                    byte[] data = Encoding.UTF8.GetBytes(s);
                    stream.Write(data, 0, data.Length);
                    stream.Write(file.Data, 0, file.Data.Length);
                    stream.Write(line, 0, line.Length);
                }
            }

            request.ContentLength = stream.Length;
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            Stream requestStream = request.GetRequestStream();

            stream.Position = 0L;
            stream.CopyTo(requestStream);
            stream.Close();
            requestStream.Close();

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var mstream = new MemoryStream())
            {
                responseStream.CopyTo(mstream);
                return mstream.ToArray();
            }
        }

        /// <summary>
        /// 以Post 形式提交数据及file内容到 uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="files"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string PostData(Uri uri)
        {
            string url = uri.AbsoluteUri;
            string param = "{\"email\":\"lynzhang@careerintlinc.com\",\"password\":\"61eb6807c830407ce1bba1ab2bcbaa6c\"}";
            //string callback = PostMoths(url, param);

            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;

        }

        public static string PostData(Uri uri, string param)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            string url = uri.AbsoluteUri;
            //string param = "{\"email\":\"lynzhang@careerintlinc.com\",\"password\":\"61eb6807c830407ce1bba1ab2bcbaa6c\"}";
            //string callback = PostMoths(url, param);

            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.KeepAlive = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json;charset=UTF-8";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }

            if (request != null)
            {
                request.Abort();
            }
            return strValue;

        }


        /// <summary>
        /// 上传文件
        /// </summary>
        public class UploadFile1
        {
            public UploadFile1()
            {
                ContentType = "application/octet-stream";
            }
            public string Name { get; set; }
            public string Filename { get; set; }
            public string ContentType { get; set; }
            public byte[] Data { get; set; }
        }


    }
}
