using Bmob_space;
using cn.bmob.api;
using cn.bmob.io;
using ITResourceLibrary.Business;
using ITResourceLibrary.Business.Models;
using ITResourceLibrary.Helps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ITResourceLibrary.HandlerData
{
    public class KindCodeOperation : Function
    {
        BaseOperation<KindModel, BmobKindModel> kind = new BaseOperation<KindModel, BmobKindModel>();
        BaseOperation<CodeModel, BmobCodeModel> code = new BaseOperation<CodeModel, BmobCodeModel>();
        BaseOperation<OperationModel, BmobOperationModel> operate = new BaseOperation<OperationModel, BmobOperationModel>();
        public static List<CodeModel> CodeData = new List<CodeModel>();
        public static List<KindModel> KindData = new List<KindModel>();
        /// <summary>
        /// Json序列化所有分类
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToKindJson(IEnumerable<KindModel> KindTable, IEnumerable<CodeModel> CodeTable)
        {
            List<TreeModel> list = new List<TreeModel>();
            foreach(KindModel row in KindTable)
            {
                string Color = "blue";
                string Icon = "glyphicon glyphicon-folder-open";
                bool pri = false;//判断是否为私有账户，true为私有账户，跳过添加
                if (row.ParentId == "无")
                {
                    Color = "brown"; Icon = "glyphicon glyphicon-folder-open";
                }
                else
                {
                    var data = CodeData.Where(x => x.KindObjectId == row.objectId).ToList();
                    if (data.Count == 1)
                    {
                        if (data.First().Visible == "Invisible" && PublicPermission((string)SessionHelp.Get("UserName")))
                        { pri = true; }
                        Color = "black"; Icon = "glyphicon glyphicon-pencil";
                    }
                    else
                    {
                        
                        foreach (KindModel row_ in KindTable)
                        {
                            if (row_.ParentId.ToString() == row.objectId) { Color = "#4B0082"; Icon = "glyphicon glyphicon-folder-open"; break; }
                        }
                    }
                    if (pri)
                    {
                        continue;
                    }


                }
 
                list.Add(new TreeModel()
                {
                    Id = row.objectId,
                    ParentId = row.ParentId,
                    text = row.Name,
                    color = Color,
                    icon = Icon
                });
                
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KindModel> GetAllKind()
        {
            if (KindData.Count==0)
            {
                KindData= kind.GetAll("Kind_tb").ToList();
                return KindData;
            }
            else
            {
                return KindData;
            }
        }

        /// <summary>
        /// 获取所有代码数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CodeModel> GetAllCode()
        {
            if (CodeData.Count==0)
            {
                CodeData = code.GetAll("Code_tb").ToList();
                return CodeData;
            }
            else
            {
                return CodeData;
            }
        }

     

        /// <summary>
        /// 修改代码数据
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Code"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string ModifyCode(string Title, string Code, string Id, string OldTitle, string Visible)
        {
            if (Title != OldTitle)
            {
                var test = CodeData.Where(x => x.Title == Title).ToList();
                if (test.Count > 0) return ToJson("出错,已存在改标题");
                KindModel kindmodel = new KindModel("Kind_tb");
                kindmodel.objectId = Id; kindmodel.Name = Title;
                if (kind.ModifyData(kindmodel))
                {
                    var data_ = KindData.Where(x => x.objectId == Id).Single();
                    data_.Name = Title;
                }
                else return ToJson("出错,修改分类失败");
            }
            var data = CodeData.Where(x => x.Title == OldTitle).Single();
            CodeModel model = new CodeModel("Code_tb");
            model.objectId = data.objectId;
            model.Title = Title;
            model.Code = Code;
            model.Visible = Visible;
            if (code.ModifyData(model))
            {
                data.Title = Title; data.Code = Code; data.Visible = Visible;
            }
            //添加修改记录--------------------
            if (Visible == "Visible")
            {
                OperationModel operationModel = new OperationModel("OperationNews_tb");
                operationModel.User = (string)SessionHelp.Get("UserName");
                operationModel.Operate = "修改了";
                operationModel.Title = Title;
                List<string> PathKind = new List<string>();
                string id;
                id = KindData.Where(x => x.objectId == Id).Single().ParentId;
                KindPath(id, ref PathKind);
                operationModel.Kind = PathKind[PathKind.Count - 1];
                operate.Create(operationModel);
            }
            //添加修改记录-------------------
            return ToJson("修改成功");
        }
        /// <summary>
        /// 搜索页面修改代码数据
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Code"></param>
        /// <param name="Visible"></param>
        /// <returns></returns>
        public string ModifyCode(string Title, string Code, string Visible)
        {
            var data = CodeData.Where(x => x.Title == Title).Single();
            if(code.ModifyData(new CodeModel{fTable = "Code_tb", Visible = Visible, Title = Title, Code = Code,objectId=data.objectId }))
            {
                data.Code = Code; data.Visible = Visible; data.Title = Title;
                //添加修改记录--------------------
                if (Visible == "Visible")
                {
                    OperationModel operationModel = new OperationModel("OperationNews_tb");
                    operationModel.User = (string)SessionHelp.Get("UserName");
                    operationModel.Operate = "修改了";
                    operationModel.Title = Title;
                    List<string> PathKind = new List<string>();
                    string  id;
                    id = KindData.Where(x => x.objectId == data.KindObjectId).Single().ParentId;
                    KindPath(id, ref PathKind);
                    operationModel.Kind = PathKind[PathKind.Count-1];
                    operate.Create(operationModel);
                }
                //添加修改记录-------------------
                return "修改成功";
            }
            
            return "修改失败";
        }


        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Kind"></param>
        /// <param name="Code"></param>
        /// <param name="Author"></param>
        /// <returns></returns>
        public string AddData(CodeModel model)
        {
            try
            {
                string codeId, ParentId = model.objectId;//将model里的分类id提取出来作为父项分类id，之前暂存进去的
                string kindId;
                var data = CodeData.Where(x => x.Title == model.Title).ToList();
                if (data.Count > 0) { return "出错,此标题已存在"; }
                KindModel kindmodel = new KindModel("Kind_tb");
                kindmodel.ParentId = ParentId;
                kindmodel.Name = model.Title;
                kindId = kind.Create(kindmodel);
                if (kindId != null)
                {
                    kindmodel.objectId = kindId;
                    KindData.Add(kindmodel);
                    model.KindObjectId = kindId;
                    codeId = code.Create(model); ;
                    if (codeId != null)
                    {
                        model.objectId = codeId;
                        CodeData.Add(model);
                        if (model.Visible == "Visible")
                        {
                            OperationModel operationModel = new OperationModel("OperationNews_tb");
                            operationModel.User = (string)SessionHelp.Get("UserName");
                            List<string> PathKind = new List<string>();
                            KindPath(ParentId, ref PathKind);
                            operationModel.Kind = PathKind[PathKind.Count - 1];
                            operationModel.Operate = "添加了";
                            operationModel.Title = model.Title;
                            operate.Create(operationModel);
                        }
                        
                        return kindId;
                    }
                    else return "出错,添加代码成功,添加标题失败";
                }
                else return "出错,添加代码失败";
            }
            catch (Exception e)
            {
                return "出错," + e.Message;
            }
        }

        /// <summary>
        /// 获取标题路径
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public static string Path_Kind(string Title)
        {
            string id = CodeData.Where(x => x.Title == Title).Single().KindObjectId;
            id = KindData.Where(x => x.objectId == id).Single().ParentId;
            List<string> PathKind =new List<string>();
            KindPath(id,ref PathKind);
            string path = "";
            foreach(string i in PathKind)
            {
                path = i+ "=>" + path;
            }
            return path;
        }
        /// <summary>
        /// 递归找出标题分类路径
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public static void KindPath (string ObjectId,ref List<string> PathKind)
        {
            
            var data = KindData.Where(x => x.objectId == ObjectId).Single();
            PathKind.Add(data.Name);
            if (data.ParentId != "无"){
                KindPath(data.ParentId,ref PathKind);
            }
        }
 
    }
}