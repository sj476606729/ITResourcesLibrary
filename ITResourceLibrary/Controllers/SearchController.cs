using ITResourceLibrary.HandlerData;
using Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class SearchController : Controller
    {
        Operation operate = new Operation();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        //搜索标题
        public string SearchTitle(string Title)
        {
            string data = new SearchOperate().SearchTitle(Title);
            return data;
        }
        public string ShowData(string Title)
        {
            string data = operate.GetCode(Title);
            return data;
        }
    }
}