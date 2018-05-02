using ITResourceLibrary.Helps;
using ITResourceLibrary.Models;
using System.Web.Mvc;
using ITResourceLibrary.Business;

namespace ITResourceLibrary.Controllers
{
    public class loginController : Controller
    {
        AccountOperation Account = new AccountOperation();
        public ActionResult Index(string ReturnUrl)
        {
            SessionHelp.Remove("UserName");
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Index(AccountViewModel account)
        {
            string name = Account.AllowAccount(account);
            if (name!=null)
            {
                SessionHelp.Set("UserName", name);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.error = "账号密码有误";
            return View();
        }
        
    }
}