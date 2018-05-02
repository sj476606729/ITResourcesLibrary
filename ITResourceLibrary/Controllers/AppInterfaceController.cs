using Bmob_space;
using ITResourceLibrary.Business;
using ITResourceLibrary.Business.Models;
using ITResourceLibrary.HandlerData;
using ITResourceLibrary.Helps;
using ITResourceLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class AppInterfaceController : Controller
    {
        // GET: AppInterface
        KindCodeOperation operation = new KindCodeOperation();
        BaseOperation<OperationModel, BmobOperationModel> operate = new BaseOperation<OperationModel, BmobOperationModel>();
        AccountOperation Account = new AccountOperation();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string GetAllOperate(string User,string Password)
        {
            try
            {
                string name = Account.AllowAccount(new AccountViewModel { User = User, Password = Password });
                if (name != null)
                {
                    DateTime date = DateTime.Now.AddDays(-5);
                    var data = (from a in operate.GetAll("OperationNews_tb") where Convert.ToDateTime(a.createdAt) > date && a.User != name orderby a.createdAt descending select a).ToList();
                    return JsonConvert.SerializeObject(data);
                }
                else return null;
            }
            catch(Exception e)
            {
                return e.Message;
            }
            
            
        }
        /// <summary>
        /// 登陆账户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public string LoginAccount(AccountViewModel account)
        {
            return Account.AllowAccount(account);
        }
        /// <summary>
        /// 获取代码给app
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetCode(string title)
        {
            try
            {
                var data = KindCodeOperation.CodeData.Where(x => x.Title == title).First();
                data.Code = HttpUtility.UrlDecode(data.Code);
                data.Code = data.Code.Replace(" ", "\\b");
                var result = DataMapperHelper.Map<CodeViewModel>(data);
                result.Kind = KindCodeOperation.Path_Kind(title);
                return JsonConvert.SerializeObject(result);
            }
            catch(Exception e)
            {
                return "您提交的数据标题为：" + title + "\n" + e.Message;
            }
            
        }
        /// <summary>
        /// 获取树状分类数据
        /// </summary>
        /// <returns></returns>
        public string GetTree()
        {
            string data = operation.ToKindJson(operation.GetAllKind(), operation.GetAllCode());
            return data;
        }
    }
}