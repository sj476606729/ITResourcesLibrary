using ITResourceLibrary.HandlerData;
using ITResourceLibrary.Helps;
using System.Web.Mvc;

namespace ITResources.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        private KindCodeOperation operation = new KindCodeOperation();

        //获取单条数据

        //获取当前登陆用户
        public string GetUserName()
        {
            string username = (string)SessionHelp.Get("UserName");
           
            return username;
        }
        public int CodeCount()
        {
            return KindCodeOperation.CodeData.Count;
        }
    }
}