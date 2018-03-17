using ITResourceLibrary.Business;
using ITResourceLibrary.HandlerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public class KindController : Currency
    {
        Operation operation = new Operation();
        // GET: Kind
        public ActionResult Index()
        {
            return View();
        }
        //获得所有分类
        
        public string GetKind(string id)
        {
            //获取初始目录数据json序列化
            string data = operation.ToKindJson(operation.GetAllKind(), operation.GetAllCode());
            return data;
        }
        public string GetCode(string Title)
        {
            string data = operation.GetCode(Title);
            return data;
        }
        //修改分类
        public string ModifyKind(string Name, string Key)
        {
            if (operation.ModifyKind(Name, Key)) return ToJson("修改成功");
            return ToJson("修改失败");
        }
        //修改代码数据
        public string ModifyCode(string Title, string Code, string Key, string OldTitle)
        {
            return ToJson(operation.ModifyCode(Title, Code, Key, OldTitle));
        }

        //获取所有数据条目总数
        public int CodeCount()
        {
            return operation.GetAllCode().Rows.Count;
        }

        //添加代码数据
        public string AddCode(string Title, string Code, string Key, string User)
        {
            //operation.AddData(HttpContext.Current.Request.Form["Title"], HttpContext.Current.Request.Form["Key"].ToString(), HttpContext.Current.Request.Form["Code"], HttpContext.Current.Request.Form["User"]);
            return "";
        }
    }
}