using Career.Utility;
using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class TxtController : Controller
    {
        //
        // GET: /Txt/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            Txt model = TxtDAL.GetInstance().RetrieveByID(id);
            model.html = String.IsNullOrWhiteSpace(model.html) ? model.content : model.html;
            model.tag = TxtDAL.GetInstance().RetrieveTagByTxtID(id).TrimStart(',').TrimEnd(',');
            model.txttype = String.IsNullOrWhiteSpace(model.txttype) ? "1" : model.txttype;
            return View(model);
        }
        [HttpGet]
        public ActionResult test()
        {
            DirectoryInfo TheFolder = new DirectoryInfo("F:/txt");
            ////遍历文件夹
            //foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            //    this.listBox1.Items.Add(NextFolder.Name);
            StringBuilder sb = new StringBuilder();
            var dal = TxtDAL.GetInstance();
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                try
                {
                    
                    var log = Log.ReadLogs("F:/txt/" + NextFile.Name);
                    Txt model = new Txt()
                    {
                        title = NextFile.Name,
                        content = log,
                        fileName=NextFile.Name,
                        CreateTime = DateTime.Now
                    };
                    dal.Edit(model);
                    //sb.Append(NextFile.Name);
                    Log.Write("成功文件名字", NextFile.Name);
                }
                catch (Exception ex)
                {
                    Log.Error("异常", ex);
                    Log.Write("异常文件名字",NextFile.Name);
                }
            }

            ViewBag.filename = sb.ToString();
            return View();
            //this.listBox2.Items.Add(NextFile.Name);
            //

        }
        [HttpGet]
        public ActionResult deletetag()
        {
            List<Tag> list = TxtDAL.GetInstance().RetrieveTag();
            return View(list);
        }
        [HttpGet]
        public ActionResult deletetype()
        {
            List<TxtType> list = TxtDAL.GetInstance().RetrieveType();
            return View(list);
        }
    }
}
