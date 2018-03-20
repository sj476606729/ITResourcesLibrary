using ITResourceLibrary.Business;
using ITResourceLibrary.HandlerData;
using Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class SearchController : Currency
    {
        Operation operate = new Operation();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        //搜索标题
        public string SearchTitle(string Title,string TypeSelect)
        {
            string data = new SearchOperate().SearchTitle(Title,TypeSelect);
            return data;
        }
        //显示内容
        public string ShowData(string Title)
        {
            string data = operate.GetCode(Title);
            return data;
        }
        [HttpPost]
        public string ModifyCode(string Title, string Code)
        {
            return ToJson(operate.ModifyCode(Title, Code));
        }
    }
}