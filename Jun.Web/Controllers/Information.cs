using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class InformationController : Controller
    {
        //
        // GET: /Information/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            Information model = InformationDAL.GetInstance().RetrieveByID(id);
            return View(model);
        }

    }
}
