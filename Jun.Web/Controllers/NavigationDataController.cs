using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jun.DAL;

namespace Jun.Web.Controllers
{
    public class NavigationDataController : Controller
    {
        //
        // GET: /NavigationData/
        [HttpPost]
        public JsonResult edit(Navigation model)
        {
            int flag = NavigationDAL.GetInstance().Edit(model);
            return Json(new { IsSuccess = true, status = flag });
        }
        [HttpPost]
        public JsonResult Retrieve()
        {
            Navigation param = new Navigation();
            List<NavigationType> listType = NavigationTypeDAL.GetInstance().Retrieve();
            List<Navigation> list = NavigationDAL.GetInstance().Retrieve(param);
            return Json(new { type = listType, list = list });
        }
        [HttpPost]
        public JsonResult delete(string id)
        {
            int flag=NavigationDAL.GetInstance().Delete(id);
            return Json(new { IsSuccess = true, status = flag });
        }
    }
}
