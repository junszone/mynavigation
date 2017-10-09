using Jun.DAL;
using Jun.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    [PermissionFilter]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("/txt/index");
        }
    }
}
