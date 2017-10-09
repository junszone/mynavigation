using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class ServerDataController : Controller
    {
        ///// <summary>
        ///// 获取服务器信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult Retrieve(Server param)
        //{
        //    var list = ServerDAL.GetInstance().Retrieve(param);
        //    return Json(list);
        //}
        [HttpPost]
        public JsonResult RetrievePaged(ParamServer param)
        {
            Paged<Server> paged = ServerDAL.GetInstance().RetrievePaged(param);
            return Json(paged);
        }
        [HttpPost]
        public JsonResult edit(Server model)
        {
            int flag = ServerDAL.GetInstance().Edit(model);
            return Json(new { Issuccess = true, status = flag });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            int flag = ServerDAL.GetInstance().Delete(id);
            return Json(new { Issuccess = true, status = flag });
        }
    }
}
