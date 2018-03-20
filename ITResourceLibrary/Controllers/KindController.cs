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
        [HttpPost]
        //修改分类
        public string ModifyKind(string Name, string Key)
        {
            if (operation.ModifyKind(Name, Key)) return ToJson("修改成功");
            return ToJson("修改失败");
        }
        [HttpPost]
        //修改代码数据
        public string ModifyCode(string Title, string Code, string Key, string OldTitle,string Visible)
        {
            return ToJson(operation.ModifyCode(Title, Code, Key, OldTitle,Visible));
        }

        //获取所有数据条目总数
        public int CodeCount()
        {
            return operation.GetAllCode().Rows.Count;
        }
        [HttpPost]
        //添加代码数据
        public string AddCode(string Title, string Code, string Key, string User,string Visible)
        {
            string data=ToJson( operation.AddData(Title, Key, Code, User,Visible));
            return data;
        }
        [HttpPost]
        //添加分类数据
        public string AddKind(string ParentId,string Name)
        {
           return ToJson( operation.AddKind(ParentId, Name));
        }
        [HttpPost]
        //删除分类
        public string DeleteKind(string Key)
        {
          return ToJson(operation.DeleteKind(Key));
        }
        //移动分类
        [HttpPost]
        public string MoveKind(string node,string newnode)
        {
            string result = operation.MoveKind(node, newnode);
           return ToJson(result);
        }
        [HttpPost]
        //删除代码数据
        public string DeleteCode(string Key,string Title)
        {
            return ToJson(operation.DeleteCode(Key,Title));
        }

        public string GetSelect(string Kind)
        {
            string data= operation.Totitles(Kind);
            return data;
        }
    }
}