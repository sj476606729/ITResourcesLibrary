using Bmob_space;
using cn.bmob.api;
using cn.bmob.io;
using ITResourceLibrary.Helps;
using ITResourceLibrary.Business.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ITResourceLibrary.Business;
using Search;
using System.Collections;

namespace ITResourceLibrary.HandlerData
{
    public class Operation:Function
    {
        
        #region 搜索标题部分
        public static List<TreeModel> listTitles;
        public static List<List<String>> listTitles2_public=new List<List<string>>();
        public static List<List<String>> listTitleids2_public = new List<List<string>>();
        public static List<List<String>> listTitles2_private = new List<List<string>>();
        public static List<List<String>> listTitleids2_private = new List<List<string>>();//定义静态目的是因为其他人登陆账户时时最全数据，可以覆盖原来数据(原来数据可能已操作过)
        public static List<string> titleids = new List<string>();
        public static List<string> title, id;
        public static List<List<string>> titles = new List<List<string>>();
        public static List<List<string>> ids = new List<List<string>>();
        #endregion

        public static DataTable Code_Data;
        public static DataTable Kind_Data;
    
        BmobWindows Bmob = new BmobWindows();
        Bmob_Initial initial = Bmob_Initial.Initial();
        /// <summary>
        /// Json序列化所有分类
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToKindJson(DataTable table, DataTable tablecode)
        {
            List<TreeModel> list = new List<TreeModel>();
            foreach (DataRow row in table.Rows)
            {
                string Color = "blue";
                string Icon = "glyphicon glyphicon-folder-open";
                bool pri = false;
                if (row["ParentId"].ToString() == "无")
                {
                    Color = "brown"; Icon = "glyphicon glyphicon-folder-open";
                }
                else
                {
                    foreach (DataRow row_ in tablecode.Rows)
                    {
                        
                        if (row_["Title"].ToString() == row["Name"].ToString()) {
                            if (row_["Visible"].ToString() == "Invisible" && PublicPermission((string)SessionHelp.Get("UserName")))
                                { pri = true; }
                            Color = "black"; Icon = "glyphicon glyphicon-pencil"; break;
                        }
                    }
                    if (pri) {
                        continue;
                    }
                    foreach (DataRow row_ in table.Rows)
                    {
                        if (row_["ParentId"].ToString() == row["ID"].ToString()) { Color = "blue"; Icon = "glyphicon glyphicon-folder-open"; break; }
                    }
                }

                list.Add(new TreeModel()
                {
                    Id = row["ID"].ToString(),
                    ParentId = row["ParentId"].ToString(),
                    text = row["Name"].ToString(),
                    color = Color,
                    icon = Icon
                });
                listTitles = list;
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllKind()
        {
            Bmob_Initial initial = Bmob_Initial.Initial();
            if (Kind_Data == null)
            {
                var query = new BmobQuery();
                query.Limit(500);
                var future = Bmob.FindTaskAsync<BmobKindModel>("Kind_tb", query);
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn("ID", typeof(string)));
                table.Columns.Add(new DataColumn("ParentId", typeof(string)));
                table.Columns.Add(new DataColumn("Name", typeof(string)));
                if (future.Result is IBmobWritable)
                {
                    int i = 0;
                    foreach (object data in future.Result.results)
                    {
                        DataRow row = table.NewRow();
                        row["ID"] = future.Result.results[i].objectId;
                        row["ParentId"] = future.Result.results[i].ParentId;
                        row["Name"] = future.Result.results[i].Name;
                        table.Rows.Add(row); i++;
                    }
                }
                Operation.Kind_Data = table;
                return table;
            }
            else {
                //Operation.Kind_Data = (DataTable)HttpContext.Current.Application["Kind_tb"]; 
                return Operation.Kind_Data; }
        }
        /// <summary>
        /// 获取所有代码数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCode()
        {
            if (Code_Data == null)
            {
                var query = new BmobQuery();
                query.Limit(500);
                var future = Bmob.FindTaskAsync<BmobCodeModel>("Code_tb", query);
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn("ObjectId", typeof(string)));
                table.Columns.Add(new DataColumn("Title", typeof(string)));
                table.Columns.Add(new DataColumn("Code", typeof(string)));
                table.Columns.Add(new DataColumn("Author", typeof(string)));
                table.Columns.Add(new DataColumn("Visible", typeof(string)));
                if (future.Result is IBmobWritable)
                {
                    int i = 0;
                    foreach (object data in future.Result.results)
                    {
                        DataRow row = table.NewRow();
                        row["ObjectId"] = future.Result.results[i].objectId;
                        row["Title"] = future.Result.results[i].Title;
                        row["Code"] = future.Result.results[i].Code;
                        row["Author"] = future.Result.results[i].Author;
                        row["Visible"] = future.Result.results[i].Visible;
                        table.Rows.Add(row); i++;
                    }
                }
                Operation.Code_Data = table;

                return table;
            }
            else {
                //Operation.Code_Data = (DataTable)HttpContext.Current.Application["Code_tb"]; 
                return Operation.Code_Data; }


        }
        /// <summary>
        /// 获得代码数据
        /// </summary>
        /// <param name="Name"></param>
        public string GetCode(string Name)
        {
            if (Operation.Code_Data == null)
            {
                BmobQuery query = new BmobQuery();
                query.WhereEqualTo("Title", Name);
                var future = Bmob.FindTaskAsync<BmobCodeModel>("Code_tb", query);

                if (future.Result is IBmobWritable)
                {
                    string JsonData = JsonConvert.SerializeObject(new CodeModel()
                    {
                        Title = future.Result.results[0].Title,
                        Code = future.Result.results[0].Code,
                        Author = future.Result.results[0].Author,
                        Visible=future.Result.results[0].Visible
                    });
                    return JsonData;
                }
                else { return null; }
            }
            else
            {
                var query = (from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == Name select r).Single();
                string JsonData = JsonConvert.SerializeObject(new CodeModel()
                {
                    Title = query.Field<string>("Title"),
                    Code = query.Field<string>("Code"),
                    Author = query.Field<string>("Author"),
                    Visible=query.Field<string>("Visible")
                });
                return JsonData;
            }


        }
        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool ModifyKind(string Name, string Id)
        {
            var future1 = Bmob.GetTaskAsync<BmobKindModel>("Kind_tb", Id);
            string ParentId = "";
            if (future1.Result is IBmobWritable)
            {
                ParentId = future1.Result.ParentId;
            }
            var linq = from r in Operation.Kind_Data.AsEnumerable() where r.Field<string>("ParentId") == ParentId && r.Field<string>("Name") == Name select r;
            if (linq.Count<DataRow>() > 0) { return false; }
            BmobKindModel kindModel = new BmobKindModel("Kind_tb");
            kindModel.objectId = Id;
            kindModel.Name = Name;
            var future = Bmob.UpdateTaskAsync<BmobKindModel>(kindModel);
            if (future.Result is IBmobWritable)
            {
                linq = from r in Operation.Kind_Data.AsEnumerable() where r.Field<string>("Id") == Id select r;
                foreach (var data in linq)
                {
                    data.SetField<string>("Name", Name);
                }
                //DataSynchronous("Kind_tb");
                return true;
            }
            else return false;

        }
        /// <summary>
        /// 修改代码数据
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Code"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string ModifyCode(string Title, string Code, string Id, string OldTitle,string Visible)
        {
            try
            {
                if (Title != OldTitle)
                {
                    var linq_ = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == Title select r;
                    if (linq_.Count<DataRow>() > 0) { return "出错,此标题已添加"; }
                }
                var linq = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == OldTitle select r;
                string Objectid = linq.First().Field<string>("ObjectId");
                BmobCodeModel codeModel = new BmobCodeModel("Code_tb");
                codeModel.objectId = Objectid;
                codeModel.Title = Title;
                codeModel.Code = Code;
                codeModel.Visible = Visible;
                var future1 = Bmob.UpdateTaskAsync<BmobCodeModel>(codeModel);
                if (future1.Result is IBmobWritable)
                {
                    linq = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == OldTitle select r;
                    foreach (var data in linq)
                    {
                        data.SetField<string>("Title", Title);
                        data.SetField<string>("Code", Code);
                        data.SetField<string>("Visible", Visible);
                    }
                    if (Title != OldTitle)
                    {
                        BmobKindModel kindModel = new BmobKindModel("Kind_tb");
                        kindModel.objectId = Id;
                        kindModel.Name = Title;
                        future1 = Bmob.UpdateTaskAsync<BmobKindModel>(kindModel);
                        if (future1.Result is IBmobWritable)
                        {
                            linq = from r in Operation.Kind_Data.AsEnumerable() where r.Field<string>("Id") == Id select r;
                            foreach (var data in linq)
                            {
                                data.SetField<string>("Name", Title);
                            }
                            //DataSynchronous("Kind_tb");
                            return "成功";
                        }
                        else { return "出错,修改代码数据成功，修改标题失败"; }
                    }
                    else
                    {
                        return "成功";
                    }
                }
                else return "出错,修改代码数据失败";

            }
            catch (Exception e)
            {
                return "出错," + e.Message;
            }
        }
        public string ModifyCode(string Title,string Code,string Visible)
        {
            var linq = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == Title select r;
            string Objectid = linq.First().Field<string>("ObjectId");
            BmobCodeModel codeModel = new BmobCodeModel("Code_tb");
            codeModel.objectId = Objectid;
            codeModel.Code = Code;
            codeModel.Visible = Visible;
            var future1 = Bmob.UpdateTaskAsync<BmobCodeModel>(codeModel);
            if(future1.Result is IBmobWritable) {
                linq = from r in Code_Data.AsEnumerable() where r.Field<string>("Title") == Title select r;
                foreach (var data in linq)
                {
                    data.SetField<string>("Title", Title);
                    data.SetField<string>("Code", Code);
                    data.SetField<string>("Visible", Visible);
                }

                return "成功";
            }
            return "修改失败";
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteKind(string Id)
        {
            var linq = from r in Operation.Kind_Data.AsEnumerable() where r.Field<string>("ParentId") == Id select r;
            if (linq.Count<DataRow>() > 0)
            {
                return "出错,该分类存在子分类";
            }
            var future1 = Bmob.DeleteTaskAsync("Kind_tb", Id);
            if (future1.Result is IBmobWritable)
            {
                foreach (DataRow row in Operation.Kind_Data.Rows)
                {
                    if (row["Id"].ToString() == Id)
                    {
                        Operation.Kind_Data.Rows.Remove(row); break;
                    }
                }
                //DataSynchronous("Kind_tb");
                return "删除成功";
            }
            else return "出错,删除分类失败";

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteCode(string Id, string Title)
        {
            var linq = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == Title select r;
            string Objectid = linq.First().Field<string>("ObjectId");
            var future1 = Bmob.DeleteTaskAsync("Code_tb", Objectid);
            if (future1.Result is IBmobWritable)
            {
                foreach (DataRow row in Operation.Code_Data.Rows)
                {
                    if (row["Title"].ToString() == Title)
                    {
                        Operation.Code_Data.Rows.Remove(row); break;
                    }
                }
                //DataSynchronous("Code_tb");
                future1 = Bmob.DeleteTaskAsync("Kind_tb", Id);
                if (future1.Result is IBmobWritable)
                {
                    foreach (DataRow row in Operation.Kind_Data.Rows)
                    {
                        if (row["Id"].ToString() == Id)
                        {
                            Operation.Kind_Data.Rows.Remove(row); break;
                        }
                    }
                    //DataSynchronous("Kind_tb");
                    return "删除成功";
                }
                else return "出错,删除代码数据成功，删除标题失败";
            }
            else return "出错,删除代码数据失败";
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Kind"></param>
        /// <param name="Code"></param>
        /// <param name="Author"></param>
        /// <returns></returns>
        public string AddData(string Title, string Kind, string Code, string Author,string Visible)
        {
            try
            {
                var query = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == Title select r;
                if (query.Count<DataRow>() > 0) { return "出错,已存在该标题"; }
                BmobCodeModel codeModel = new BmobCodeModel("Code_tb");
                codeModel.Title = Title;
                codeModel.Code = Code;
                codeModel.Author = Author;
                codeModel.Visible = Visible;
                var future = Bmob.CreateTaskAsync(codeModel);
                if (future.Result.objectId.Length > 0)
                {
                    DataRow row = Operation.Code_Data.NewRow();
                    row["Title"] = Title; row["Code"] = Code; row["Author"] = Author; row["ObjectId"] = future.Result.objectId;
                    row["Visible"] = Visible;
                    Operation.Code_Data.Rows.Add(row);
                    //DataSynchronous("Code_tb");
                    BmobKindModel kindModel = new BmobKindModel("Kind_tb");
                    kindModel.ParentId = Kind;
                    kindModel.Name = Title;
                    var future1 = Bmob.CreateTaskAsync(kindModel);
                    if (future1.Result.objectId.Length > 0)
                    {
                        DataRow row_ = Operation.Kind_Data.NewRow();
                        row_["ID"] = future1.Result.objectId; row_["ParentId"] = Kind; row_["Name"] = Title;

                        Operation.Kind_Data.Rows.Add(row_);
                        //DataSynchronous("Kind_tb");
                        return future1.Result.objectId;
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
        /// 添加分类
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string AddKind(string ParentId, string Name)
        {
            var query = new BmobQuery();
            query.WhereContainedIn<string>("ParentId", ParentId);
            query.WhereContainedIn<string>("Name", Name);
            var future = Bmob.FindTaskAsync<BmobKindModel>("Kind_tb", query);
            if (future.Result.results.Count == 0)
            {

                BmobKindModel kindModel = new BmobKindModel("Kind_tb");
                kindModel.ParentId = ParentId;
                kindModel.Name = Name;
                var future1 = Bmob.CreateTaskAsync(kindModel);
                if (future1.Result.objectId.Length > 0)
                {
                    DataRow row = Operation.Kind_Data.NewRow();
                    row["ID"] = future1.Result.objectId; row["ParentId"] = ParentId; row["Name"] = Name;
                    Operation.Kind_Data.Rows.Add(row);
                    //DataSynchronous("Kind_tb");
                    return future1.Result.objectId;
                }
                else return "添加失败";
            }
            else return "已存在该分类";
        }
        /// <summary>
        /// 移动分类
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="NewNode"></param>
        /// <returns></returns>
        public string MoveKind(string Node, string NewNode)
        {
            try
            {
                BmobKindModel kindModel = new BmobKindModel("Kind_tb");
                kindModel.objectId = Node;
                kindModel.ParentId = NewNode;
                var future = Bmob.UpdateTaskAsync<BmobKindModel>(kindModel);
                if (future.Result is IBmobWritable)
                {
                    var linq = from r in Operation.Kind_Data.AsEnumerable() where r.Field<string>("Id") == Node select r;
                    foreach (var data in linq)
                    {
                        data.SetField<string>("ParentId", NewNode);
                    }

                }
               // DataSynchronous("Kind_tb");
                return "移动成功";
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }
        /// <summary>
        /// 导入搜索分类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Totitles(string data)
        {
            //JObject,JArray
            titleids.Clear();
            titles.Clear();
            ids.Clear();
            listTitles2_public.Clear(); listTitleids2_public.Clear(); listTitles2_private.Clear(); listTitleids2_private.Clear();

             var mJObj = JArray.Parse(data);
            IList<JToken> delList = new List<JToken>(); //存储需要删除的项

            foreach (var ss in mJObj)  //查找某个字段与值
            {
                JObject _o = (JObject)ss;
                title = new List<string>();
                id = new List<string>();
                if(_o["text"].ToString()== "数据库操作超级工具类")
                {
                    string a="";
                }
                titleids.Add(_o["text"].ToString());
                xunhuan((JArray)_o["nodes"]);
                titles.Add(title);
                ids.Add(id);

            }
            if (PublicPermission((string)SessionHelp.Get("UserName")))
             {
                listTitles2_public.AddRange(titles);
                listTitleids2_public.AddRange(ids);
            }
            else
            {
                listTitles2_private.AddRange(titles);
                listTitleids2_private.AddRange(ids);
            }
            
            return JToken.FromObject(titleids).ToString();


        }
        /// <summary>
        /// 递归遍历搜索子节点
        /// </summary>
        /// <param name="mJObj"></param>
        private void xunhuan(JArray mJObj)
        {
            foreach (var _s in mJObj)
            {
                JObject _o = (JObject)_s;
                if (_o.Property("nodes") != null)
                {
                    xunhuan((JArray)_o["nodes"]);

                }
                else
                {
                    title.Add(_o["text"].ToString());
                    id.Add(_o["Id"].ToString());
                }

            }

        }

        
    }
 
 

}