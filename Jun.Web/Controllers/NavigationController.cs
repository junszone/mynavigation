using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jun.Entity;
using Jun.DAL;

namespace Jun.Web.Controllers
{
    public class NavigationController : Controller
    {
        //
        // GET: /Navigation/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult edit(int id, string type)
        {
            Navigation model = NavigationDAL.GetInstance().RetrieveByID(id);
            model.type = type;
            return View(model);
        }
    }
}
