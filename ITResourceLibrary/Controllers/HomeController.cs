using ITResourceLibrary.Business;
using ITResourceLibrary.HandlerData;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Linq;
using ITResourceLibrary.Helps;
using ITResourceLibrary.Models;
using ITResourceLibrary.Business.Models;
using Bmob_space;
using System.Collections.Generic;
using Search;
using System;

namespace ITResourceLibrary.Controllers
{
    public class HomeController : Currency
    {
        //代码分类操作123
        KindCodeOperation operation = new KindCodeOperation();
        BaseOperation<KindModel, BmobKindModel> kind = new BaseOperation<KindModel, BmobKindModel>();
        BaseOperation<CodeModel, BmobCodeModel> code = new BaseOperation<CodeModel, BmobCodeModel>();
        BaseOperation<OperationModel, BmobOperationModel> operate = new BaseOperation<OperationModel, BmobOperationModel>();
        public static string AllKind;
        public ActionResult Index()
        {
            AllKind = operation.ToKindJson(operation.GetAllKind(), operation.GetAllCode());
            List<Select> select = new List<Select>();
            var data = KindCodeOperation.KindData.Where(x => x.ParentId == "无").ToList();
            select.Add(new Select { Value = "所有", Name = "所有" });
            foreach(var i in data)
            {
                select.Add(new Select { Value = i.Name, Name = i.Name });
            }
            ViewBag.select = select;
            return View();
        }

        
        public string GetKind()
        {
            //获取初始目录数据json序列化
            string data = operation.ToKindJson(operation.GetAllKind(), operation.GetAllCode());
            return data;
        }

        public string GetCode(string Title)
        {
            var data = KindCodeOperation.CodeData.Where(x => x.Title == Title).First();
            var result = DataMapperHelper.Map<CodeViewModel>(data);
            result.Kind = KindCodeOperation.Path_Kind(Title);
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        //修改分类
        public string ModifyKind(string Name, string Key)
        {
            KindModel model = new KindModel("Kind_tb");
            model.objectId = Key;
            model.Name = Name;
            if (kind.ModifyData(model))
            {
                var data = KindCodeOperation.KindData.Where(x => x.objectId == Key).Single();
                data.Name = Name;
                return ToJson("修改成功");
            }

            return ToJson("修改失败");
        }

        [HttpPost]
        //修改代码数据
        public string ModifyCode(string Title, string Code, string Key, string OldTitle, string Visible)
        {
            return operation.ModifyCode(Title, Code, Key, OldTitle, Visible);
        }
        [HttpPost]
        public string ModifySearch (string Title,string Code,string Visible)
        {
            string result= ToJson(operation.ModifyCode(Title, Code, Visible));
            return result;
        }

        [HttpPost]
        //删除分类
        public string DeleteKind(string Key)
        {
            var data = KindCodeOperation.KindData.Where(x => x.objectId == Key).Single();
            var delete = KindCodeOperation.KindData.Where(x => x.ParentId == Key).ToList();
            if (delete.Count > 0) return ToJson("出错，该分类存在子项，删除失败");
            if (kind.Delete("Kind_tb", Key))
            {
                KindCodeOperation.KindData.Remove(data);
                return ToJson("删除成功");
            }
            return ToJson("出错，删除失败");
        }

        //移动分类
        [HttpPost]
        public string MoveKind(string node, string newnode)
        {
            KindModel model = new KindModel("Kind_tb");
            model.objectId = node;
            model.ParentId = newnode;
            if (kind.ModifyData(model))
            {
                var data = KindCodeOperation.KindData.Where(x => x.objectId == node).Single();
                data.ParentId = newnode;
            }
            else { return ToJson("移动失败"); }
            return ToJson("移动成功");
        }

        [HttpPost]
        //删除代码数据
        public string DeleteCode(string Key, string Title)
        {
            var data = KindCodeOperation.CodeData.Where(x => x.Title == Title).Single();
            if (code.Delete("Code_tb", data.objectId))
            {
                KindCodeOperation.CodeData.Remove(data);
                var data_ = KindCodeOperation.KindData.Where(x => x.objectId == Key).Single();
                if (kind.Delete("Kind_tb", Key))
                {
                    KindCodeOperation.KindData.Remove(data_);
                    return ToJson("删除成功");
                }
            }
            return ToJson("出错,删除失败");
        }

        [HttpPost]
        //添加代码数据
        public string AddCode(string Title, string Code, string Key, string User, string Visible)
        {
            CodeModel codemodel = new CodeModel
            {
                Title = Title,
                Code = Code,
                objectId = Key,//此id为选中分类id，暂且存到CodeModel里
                Author = User,
                Visible = Visible,
                fTable = "Code_tb"
            };
            string data = ToJson(operation.AddData(codemodel));
            return data;
        }

        [HttpPost]
        //添加分类数据
        public string AddKind(string ParentId, string Name)
        {
            int count = (from a in KindCodeOperation.KindData where a.ParentId == ParentId && a.Name == Name select a).ToList().Count;
            if (count > 0) { return "已存在该分类"; }
            KindModel model = new KindModel
            {
                ParentId = ParentId,
                Name = Name,
                fTable = "Kind_tb"
            };
            string returnId = kind.Create(model);

            if (returnId != null) { model.objectId = returnId; KindCodeOperation.KindData.Add(model); } else return ToJson("添加分类失败");
            return ToJson(returnId);
        }


        //搜索下拉框临时类
        private class Select
        {
            public string Value { get; set; }
            public string Name { get; set; }
        }


        //搜索标题
        public string SearchTitle(string Title, string TypeSelect)
        {   if (Title.Length == 0) return "";
            string data = new SearchOperate().SearchTitle(Title,TypeSelect);
            return data;
        }



        /// <summary>
        /// 获取操作记录
        /// </summary>
        /// <returns></returns>
        /// 
     
       public string GetAllOperate()
        {
            DateTime date = DateTime.Now.AddDays(-5);
            var data = (from a in operate.GetAll("OperationNews_tb") where Convert.ToDateTime(a.createdAt) > date && a.User!=(string)SessionHelp.Get("UserName") orderby a.createdAt descending select a).ToList();
            return JsonConvert.SerializeObject(data);
        }

      
    }
}