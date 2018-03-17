using ITResourceLibrary.Helps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Business
{
    public class Currency : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            string a = AdminId;
            if (!attrs.Any(p => p.GetType() == typeof(AllowAnonymousAttribute)) && string.IsNullOrEmpty(AdminId))
            {
                filterContext.HttpContext.Response.Redirect(Url.Action("Index", "login") + "?ReturnUrl=" + GetUrl(filterContext.HttpContext.Request.Url.PathAndQuery), true);
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
}