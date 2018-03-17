using Bmob_space;
using cn.bmob.api;
using cn.bmob.io;
using ITResourceLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.HandlerData
{
    public class Operation
    {
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
            List<KindModel> list = new List<KindModel>();
            foreach (DataRow row in table.Rows)
            {
                string Color = "blue";
                string Icon = "glyphicon glyphicon-folder-open";
                if (row["ParentId"].ToString() == "无")
                {
                    Color = "brown"; Icon = "glyphicon glyphicon-folder-open";
                }
                else
                {
                    foreach (DataRow row_ in tablecode.Rows)
                    {
                        if (row_["Title"].ToString() == row["Name"].ToString()) { Color = "black"; Icon = "glyphicon glyphicon-pencil"; break; }
                    }
                    foreach (DataRow row_ in table.Rows)
                    {
                        if (row_["ParentId"].ToString() == row["ID"].ToString()) { Color = "blue"; Icon = "glyphicon glyphicon-folder-open"; break; }
                    }
                }

                list.Add(new KindModel()
                {
                    Id = row["ID"].ToString(),
                    ParentId = row["ParentId"].ToString(),
                    text = row["Name"].ToString(),
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
                        table.Rows.Add(row); i++;
                    }
                }
                Operation.Code_Data = table;
                DataSynchronous("Code_tb");
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
                        Author = future.Result.results[0].Author
                    });
                    return JsonData;
                }
                else { return null; }
            }
            else
            {
                var query = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == Name select r;
                string JsonData = JsonConvert.SerializeObject(new CodeModel()
                {
                    Title = query.First().Field<string>("Title"),
                    Code = query.First().Field<string>("Code"),
                    Author = query.First().Field<string>("Author")
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
        public string ModifyCode(string Title, string Code, string Id, string OldTitle)
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
                var future1 = Bmob.UpdateTaskAsync<BmobCodeModel>(codeModel);
                if (future1.Result is IBmobWritable)
                {
                    linq = from r in Operation.Code_Data.AsEnumerable() where r.Field<string>("Title") == OldTitle select r;
                    foreach (var data in linq)
                    {
                        data.SetField<string>("Title", Title);
                        data.SetField<string>("Code", Code);
                    }
                    DataSynchronous("Code_tb");
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
        /// <summary>
        /// 数据同步到服务器
        /// </summary>
        /// <param name="select"></param>
        private void DataSynchronous(string select)
        {
            HttpContext.Current.Application.Lock();
            if (select == "Kind_tb") { HttpContext.Current.Application["Kind_tb"] = Operation.Kind_Data; } else { HttpContext.Current.Application["Code_tb"] = Operation.Code_Data; }
            HttpContext.Current.Application.UnLock();
        }
    }
 
 

}