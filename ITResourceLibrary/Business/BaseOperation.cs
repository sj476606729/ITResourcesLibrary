using Bmob_space;
using cn.bmob.api;
using cn.bmob.io;
using ITResourceLibrary.Business.Models;
using ITResourceLibrary.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ITResourceLibrary.Business
{
    public class BaseOperation<TModel,TBmobModel>where TBmobModel:BmobTable
    {
        private BmobWindows Bmob = new BmobWindows();
        private Bmob_Initial initial = Bmob_Initial.Initial();
        //获取所有数据
        public IEnumerable<TModel> GetAll(string BmobTable)
        {
            var query = new BmobQuery();
            query.Limit(500);
            var future = Bmob.FindTaskAsync<TBmobModel>(BmobTable, query);
            var model = DataMapperHelper.MapList<TModel>( future.Result.results);
            return model;
        }

        //根据id获取单个数据
        public TModel GetById(string BmobTable,string ObjectId)
        {
            var future = Bmob.GetTaskAsync<TBmobModel>(BmobTable, ObjectId);

            return DataMapperHelper.Map<TModel>(future.Result);
        }
        //根据字段获取数据
        public IEnumerable<TModel> GetByNoId(string BmobTable,string Field,object Value)
        {
            var query = new BmobQuery();
            query.WhereEqualTo(Field,Value);
            var future = Bmob.FindTaskAsync<TBmobModel>(BmobTable, query);
            List<TModel> model = new List<TModel>();
            foreach (var data in future.Result.results)
            {
                model.Add(DataMapperHelper.Map<TModel>(data));
            }
            return model;
        }

        //修改字段
        public bool ModifyData(TModel model)
        {
            try
            {
                TBmobModel Bmodel = DataMapperHelper.Map<TBmobModel>(model);
                var future = Bmob.UpdateTaskAsync<TBmobModel>(Bmodel);
                return true;
            }
            catch(Exception e)
            {
                string message = e.Message;
                return false;
            }
            
        }

        //删除数据
        public bool Delete(string BmobTable,string ObjectId)
        {
            try
            {
                var future = Bmob.DeleteTaskAsync(BmobTable, ObjectId);
                return true;
            }catch(Exception e)
            {
                return false;
            }
            
        }
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Create(TModel model)
        {
            try
            {
                TBmobModel Bmodel = DataMapperHelper.Map<TBmobModel>(model);
                var future = Bmob.CreateTaskAsync(Bmodel);
                return future.Result.objectId;
            }catch(Exception e)
            {
                return null;
            }
            
        }

    }
}