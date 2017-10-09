using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using Career.Utility;

namespace Jun.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 权限拦截
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var path = filterContext.HttpContext.Request.Path.ToLower();
            //权限拦截是否忽略
        //    bool IsIgnored = false;
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            ////接下来进行权限拦截与验证
            //object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ViewPageAttribute), true);
           // var isViewPage = attrs.Length == 1;//当前Action请求是否为具体的功能页
            if (this.AuthorizeCore(filterContext) == false)//根据验证判断进行处理
            {
                if (filterContext.HttpContext.Request.HttpMethod == "POST")
                {
                    // filterContext.RequestContext.HttpContext.Response.Redirect("~/Account/login");
                    throw new ArgumentNullException("httpContext");
                }
            }
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }




        #region 权限判断业务逻辑

        /// <summary>
        /// //权限判断业务逻辑
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="isViewPage">是否是页面</param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext)
        {
            #region 记录日志
            bool flag = true;
         
            var paramss = filterContext.ActionParameters;
            StringBuilder sb = new StringBuilder();
            if (paramss.Any()) //Any是判断这个集合是否包含任何元素，如果包含元素返回true，否则返回false
            {
                foreach (var key in paramss.Keys) //遍历它的键；(因为我们要获取的是参数的名称s,所以遍历键)
                {
                    sb.Append(key + "=" + paramss[key]).Append("&");
                }
            }
          
            var area = filterContext.RouteData.DataTokens;
         
            var rd = filterContext.RouteData;
         
            #endregion
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            //验证当前Action是否是匿名访问Action
            if (IsLogin(filterContext))
            {//已经登录
                //如果Session不存在就从数据库查询
                if (filterContext.HttpContext.Session["LoginUser"] == null)
                {
                    ReturnLogin(filterContext);
                    flag = false;
                }
                else
                {
                }
            }
            else
            {//没有登录
                ReturnLogin(filterContext);
                flag = false;
            }
            return flag;
        }

        #endregion

        #region 跳转到登录页面
        /// <summary>
        /// 跳到登录
        /// </summary>
        /// <param name="filterContext"></param>
        private static void ReturnLogin(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Session["LoginUser"] = null;
            filterContext.HttpContext.Session.Remove("LoginUser");
            filterContext.HttpContext.Request.Cookies.Clear();
            filterContext.HttpContext.Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            string requestUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
            string returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
            string redirectUrl = string.Format("?ReturnUrl={0}", returnUrl);
            string loginUrl = "/Account/Login" + redirectUrl; //FormsAuthentication.LoginUrl + redirectUrl;
            filterContext.HttpContext.Response.Redirect(loginUrl, true);
        }
        #endregion

        #region 判断是否登录状态
        /// <summary>
        /// 通过此法判断登录
        /// </summary>
        /// <returns>已登录返回true</returns>
        public static bool IsLogin(ActionExecutingContext filterContext)
        {
            //return HttpContext.Current.User.Identity.IsAuthenticated;//IE11在win10下面有bug
            if (filterContext.HttpContext.Session["LoginUser"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 异常捕获
        public void OnException(ExceptionContext filterContext)
        {
            //Log.ControllerError(filterContext);
        }
        #endregion
    }
}