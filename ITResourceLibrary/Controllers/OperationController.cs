using ITResourceLibrary.Business;
using ITResourceLibrary.Business.Models;
using ITResourceLibrary.Helps;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class OperationController : Controller
    {
        // GET: Operation
        private OperationLog _op = OperationLog.getOperationInstance();

        [HttpPost]
        public void AddLog(string user, string title, string content, string kind)
        {
            _op.addLog(user, title, content, kind);
        }

        public void GetLog(string user, string title)
        {
            // return _op.addLog(user, title);
        }

        public ActionResult Index()
        {
            List<OperationModel> logs = _op.getLog(SessionHelp.Get("UserName").ToString());
            ViewBag.logs = logs;
            return View();
        }
    }
}