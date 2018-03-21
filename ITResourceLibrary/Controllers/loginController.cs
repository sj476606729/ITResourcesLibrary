using ITResourceLibrary.Helps;
using ITResourceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult Index(string ReturnUrl)
        {
            SessionHelp.Remove("UserName");
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Index(AccountViewModel account)
        {
            
            if (account.User == "shajun")
            {
                if (account.Password == "sj76606729") {
                    SessionHelp.Set("UserName", "沙俊");
                    return RedirectToAction("Index","Home"); 
                }
            }
            else if (account.User == "shajie") { if (account.Password == "190750991jie") { SessionHelp.Set("UserName", "沙杰"); return RedirectToAction("Index", "Home"); } }
            else if (account.User == "chenyu")
            { if (account.Password == "CHENYU") { SessionHelp.Set("UserName", "陈煜"); return RedirectToAction("Index", "Home"); } }
            else if (account.User == "xichaoqun")
            { if (account.Password == "XICHAOQUN") { SessionHelp.Set("UserName", "奚超群"); return RedirectToAction("Index", "Home"); } }
            else if (account.User == "123321")
            { if (account.Password == "123321") { SessionHelp.Set("UserName", "通用账户");
                    return RedirectToAction("Index", "Home"); } }
            ViewBag.error = "账号密码有误";
            return View();
        }
    }
}