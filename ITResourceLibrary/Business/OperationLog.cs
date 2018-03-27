using Bmob_space;
using cn.bmob.api;
using cn.bmob.io;
using cn.bmob.json;
using ITResourceLibrary.Business.Models;
using ITResourceLibrary.HandlerData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ITResourceLibrary.Business
{
    public class OperationLog
    {
        private BmobWindows Bmob = null; //必须的

        private string tablename = "OperationNews_tb";

        //单例
        private OperationLog()
        {
            Bmob = Bmob_Initial.Initial().Bmob;
        }

        public static OperationLog getOperationInstance()
        {
            return CreatOperation.op;
        }

        private static class CreatOperation
        {
            public static OperationLog op = new OperationLog();
        }

        /// <summary>
        /// 添加记录数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public void addLog(string user, string title, string content, string kind)
        {
            BmobOperationModel data = new BmobOperationModel(tablename);
            data.Title = title;
            data.User = user;
            data.Operate = content;
            data.Kind = kind;
            //保存数据
            var future = Bmob.CreateTaskAsync(data);
            //异步显示返回的数据
            // return HttpUtility.HtmlEncode(future.Result.ToString() + "success1") ;
        }

        /// <summary>
        /// 得到全部更改信息
        /// </summary>
        /// <returns></returns>
        public List<OperationModel> getLog(string user)
        {
            //查找表中的全部数据（默认最多返回10条数据）
            var query = new BmobQuery();
            query.WhereEqualTo("User", user);
            var future = Bmob.FindTaskAsync<BmobOperationModel>(tablename, query);
            //var future = Bmob.GetTaskAsync<BmobOperationModel>(tablename, "f5add0c42c");
            string ss = JsonAdapter.JSON.ToDebugJsonString(future.Result.results);
            List<OperationModel> logs = JsonConvert.DeserializeObject<List<OperationModel>>(ss);
            logs.Sort(
                 delegate (OperationModel p1, OperationModel p2)
                 {
                     return p2.createdAt.CompareTo(p1.createdAt);//降序
                 }
                 );
            return logs;
        }

        public string getParent(string title)
        {
            List<List<string>> titles = new List<List<string>>();
            titles.AddRange(Operation.titles);
            int type = -1;
            for (int i = 0; i < titles.Count; i++)
            {
                if (titles[i].Contains(title))
                {
                    type = i;
                    break;
                }
            }
            string typetitle = Operation.titleids[type];

            return typetitle;
        }
    }
}