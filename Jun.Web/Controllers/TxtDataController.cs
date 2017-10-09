using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class TxtDataController : Controller
    {
        ///// <summary>
        ///// 获取服务器信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult Retrieve(Txt param)
        //{
        //    var list = TxtDAL.GetInstance().Retrieve(param);
        //    return Json(list);
        //}
        [HttpPost]
        public JsonResult RetrievePaged(ParamTxt param)
        {
            Paged<Txt> paged = TxtDAL.GetInstance().RetrievePaged(param);
            return Json(paged);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult edit(Txt model)
        {
            string id = TxtDAL.GetInstance().Edit(model);
            model.id = id;
            int flagTag = TxtDAL.GetInstance().EditTag(model);
            return Json(new { Issuccess = true, status = 1 });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            int flag = TxtDAL.GetInstance().Delete(id);
            return Json(new { Issuccess = true, status = flag });
        }
        [HttpPost]
        public JsonResult RetrieveType()
        {
            List<TxtType> list = TxtDAL.GetInstance().RetrieveType();
            return Json(list);
        }
        [HttpPost]
        public JsonResult RetrieveTag()
        {
            List<Tag> list = TxtDAL.GetInstance().RetrieveTag();
            return Json(list);
        }
        [HttpPost]
        public JsonResult ReCount()
        {
            int flag = TxtDAL.GetInstance().ReCount();
            return Json(flag);
        }
        [HttpPost]
        public JsonResult AddType(string type)
        {
            int flag = TxtDAL.GetInstance().AddType(type);
            return Json(flag);
        }
        [HttpPost]
        public JsonResult AddTag(string tag)
        {
            int flag = TxtDAL.GetInstance().AddTag(tag);
            return Json(flag);
        }
        [HttpPost]
        public JsonResult DeleteType(int id)
        {
            int flag = TxtDAL.GetInstance().DeleteType(id);
            return Json(flag);
        }
        [HttpPost]
        public JsonResult DeleteTag(int id)
        {
            int flag = TxtDAL.GetInstance().DeleteTag(id);
            return Json(flag);
        }
        [HttpPost]
        public JsonResult ReloadTag()
        {
            int flag = TxtDAL.GetInstance().ReloadTag();
            return Json(flag);
        }
    }
}
