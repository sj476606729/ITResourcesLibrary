using ITResourceLibrary.Helps;
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
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username,string password,string ReturnUrl)
        {

            if (username == "shajun")
            {
                if (password == "sj76606729") {
                    SessionHelp.Set("UserName", "沙俊");
                    return Redirect(ReturnUrl); }
            }
            else if (username == "shajie") { if (password == "sj76606729") { SessionHelp.Set("UserName", "沙俊"); return RedirectToAction(ReturnUrl); } }
            else if (username == "chenyu")
            { if (password == "CHENYU") { SessionHelp.Set("UserName", "陈煜"); return RedirectToAction(ReturnUrl); } }

            return View();
        }
    }
}