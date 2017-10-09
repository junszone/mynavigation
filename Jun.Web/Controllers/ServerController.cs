using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class ServerController : Controller
    {
        //
        // GET: /Server/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            Server model = ServerDAL.GetInstance().RetrieveByID(id);
            return View(model);
        }

    }
}
