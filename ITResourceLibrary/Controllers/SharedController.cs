using ITResourceLibrary.HandlerData;
using ITResourceLibrary.Helps;
using System.Web.Mvc;

namespace ITResources.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        private Operation operation = new Operation();

        //获取单条数据

        //获取当前登陆用户
        public string GetUserName()
        {
            string username = (string)SessionHelp.Get("UserName");
            if (username == "shajun")
            {
                username = "沙俊";
            }
            else if (username == "shajie") { username = "沙杰"; }
            else if (username == "chenyu") { username = "陈煜"; }
            return username;
        }
    }
}