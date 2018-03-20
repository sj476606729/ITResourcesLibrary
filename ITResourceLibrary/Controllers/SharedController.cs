using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITResourceLibrary.Business;
using ITResourceLibrary.HandlerData;
using ITResourceLibrary.Helps;
using Newtonsoft.Json;

namespace ITResources.Controllers
{
    
    public class SharedController : Controller
    {
        // GET: Shared
        Operation operation = new Operation();
        //获取单条数据
        
        //获取当前登陆用户
        public string GetUserName()
        {
            string username = (string)SessionHelp.Get("UserName");
            if (username == "shajun")
            {
                username = "沙俊";
            }else if (username == "shajie") { username = "沙杰"; }
            else if (username == "chenyu") { username = "陈煜"; }
            return username;
        }
    }
}