using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITResourceLibrary.Business;
using ITResourceLibrary.HandlerData;
using ITResourceLibrary.Helps;
using Newtonsoft.Json;

namespace ITResources.Controllers
{
    
    public class SharedController : Currency
    {
        // GET: Shared
        Operation operation = new Operation();
        public string Kind(string id)
        {
            //获取初始目录数据json序列化
            string data= operation.ToKindJson(operation.GetAllKind(), operation.GetAllCode());
            return data;
        }
        //获取单条数据
        public string GetCode(string Title)
        {
            string data= operation.GetCode(Title);
            return data;
        }
        //修改分类
        public string ModifyKind(string Name,string Key)
        {
            if (operation.ModifyKind(Name, Key))return ToJson("修改成功");
             return ToJson("修改失败");
        }
        //修改代码数据
         public string ModifyCode(string Title,string Code,string Key,string OldTitle)
        {
            return ToJson(operation.ModifyCode(Title, Code, Key, OldTitle));
        }
        //获取所有数据条目总数
        public int CodeCount()
        {
            return operation.GetAllCode().Rows.Count;
        }
        //添加代码数据
        public string AddCode(string Title,string Code,string Key,string User)
        {
            //operation.AddData(HttpContext.Current.Request.Form["Title"], HttpContext.Current.Request.Form["Key"].ToString(), HttpContext.Current.Request.Form["Code"], HttpContext.Current.Request.Form["User"]);
            return "";
        }
        //获取当前登陆用户
        public string GetUserName()
        {
            string username = (string)SessionHelp.Get("UserName");
            if (username == "shajun")
            {
                username = "沙俊";
            }else if (username == "shajie") { username = "沙杰"; }
            else if (username == "chenyu") { username = "陈煜"; }
            return username;
        }
    }
}