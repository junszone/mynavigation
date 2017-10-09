using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class InformationDataController : Controller
    {
        ///// <summary>
        ///// 获取服务器信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult Retrieve(Information param)
        //{
        //    var list = InformationDAL.GetInstance().Retrieve(param);
        //    return Json(list);
        //}
        [HttpPost]
        public JsonResult RetrievePaged(ParamInformation param)
        {
            Paged<Information> paged = InformationDAL.GetInstance().RetrievePaged(param);
            return Json(paged);
        }
        [HttpPost]
        public JsonResult edit(Information model)
        {
            int flag = InformationDAL.GetInstance().Edit(model);
            return Json(new { Issuccess = true, status = flag });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            int flag = InformationDAL.GetInstance().Delete(id);
            return Json(new { Issuccess = true, status = flag });
        }
    }
}
