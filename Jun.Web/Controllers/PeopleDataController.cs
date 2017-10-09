using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class PeopleDataController : Controller
    {
        [HttpPost]
        public JsonResult Retrieve(People param)
        {
            var list = PeopleDAL.GetInstance().Retrieve(param);
            return Json(list);
        }
        [HttpPost]
        public JsonResult edit(People param)
        {
            param.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            param.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int flag = PeopleDAL.GetInstance().Edit(param);
            return Json(new { Issuccess = true, status = flag });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            int flag = PeopleDAL.GetInstance().Delete(id);
            return Json(new { Issuccess = true, status = flag });
        }
    }
}
