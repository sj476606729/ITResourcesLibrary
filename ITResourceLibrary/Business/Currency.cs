using ITResourceLibrary.Helps;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Business
{
    /// <summary>
    /// 通用控制器
    /// </summary>
    public class Currency : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            if (!attrs.Any(p => p.GetType() == typeof(AllowAnonymousAttribute)) && string.IsNullOrEmpty(AdminId))
            {
                try
                {
                    filterContext.HttpContext.Response.Redirect(Url.Action("Index", "login") + "?ReturnUrl=" + GetUrl(filterContext.HttpContext.Request.Url.PathAndQuery), true);
                }
                catch (Exception e) { }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        protected string AdminId
        {
            get
            {
                return SessionHelp.Get<string>("UserName");
            }
        }

        protected string GetUrl(string path)
        {
            return HttpUtility.UrlEncode(path);
        }

        //转换成通用result结果json
        public string ToJson(string value)
        {
            return JsonConvert.SerializeObject(new { result = value });
        }
    }

    public class Function
    {
        /// <summary>
        /// 是否一般账户
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool PublicPermission(string UserName)
        {
            if (UserName != "沙俊" && UserName != "沙杰") return true;
            return false;
        }
    }
}