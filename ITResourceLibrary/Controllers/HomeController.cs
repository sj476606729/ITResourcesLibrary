using ITResourceLibrary.Business;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class HomeController : Currency
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}