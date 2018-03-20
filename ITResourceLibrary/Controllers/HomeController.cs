using ITResourceLibrary.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class HomeController :Currency
    {
        public ActionResult Index()
        {
            return View();
        }


    }
}