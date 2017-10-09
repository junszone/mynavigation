using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jun.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult LoginData(string user, string pwd)
        {
            if (user == "jun" && pwd == "meiyoumima")
            {
                Session["LoginUser"] = user;
                CookieHelper.SetCookie("userinfo", user);
                return Json(new { IsSuccess = true });
            }
            else
            {
                return Json(new { IsSuccess=false});
            }
        }
    }
}
