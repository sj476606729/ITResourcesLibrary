using cn.bmob.api;
using cn.bmob.io;
using System;

/// <summary>
/// Bmob_Operate 的摘要说明
/// </summary>
namespace Bmob_space
{
    /// <summary>
    /// bmob初始化单利
    /// </summary>
    public class Bmob_Initial
    {
        private static Bmob_Initial initial = null;
        private BmobWindows bmob;

        public static Bmob_Initial Initial()
        {
            if (initial == null)
            {
                initial = new Bmob_Initial();
            }
            return initial;
        }

        private Bmob_Initial() : base()
        {
            bmob = new BmobWindows();
            //初始化，这个ApplicationId/RestKey需要更改为你自己的ApplicationId/RestKey（ http://www.bmob.cn 上注册登录之后，创建应用可获取到ApplicationId/RestKey）
            Bmob.initialize("bcab92606ca5634cc0a65811ee7940f7", "f23363d3111faa6d14ba23d510a521f7");
        }

        public BmobWindows Bmob
        {
            get { return bmob; }
        }
    }

    public class Bmob_Operate
    {
        public Bmob_Operate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
    }

    /// <summary>
    /// 比目代码数据模型
    /// </summary>
    public class BmobCodeModel : BmobTable
    {
        public string fTable { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Author { get; set; }
        public string Visible { get; set; }
        public string KindObjectId { get; set; }

        //构造函数
        public BmobCodeModel() { }

        //构造函数
        public BmobCodeModel(String tableName)
        {
            this.fTable = tableName;
        }

        public override string table
        {
            get
            {
                if (fTable != null)
                {
                    return fTable;
                }
                return base.table;
            }
        }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.Title = input.getString("Title");
            this.Code = input.getString("Code");
            this.Author = input.getString("Author");
            this.Visible = input.getString("Visible");
            this.KindObjectId = input.getString("KindObjectId");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("Title", this.Title);
            output.Put("Code", this.Code);
            output.Put("Author", this.Author);
            output.Put("Visible", this.Visible);
            output.Put("KindObjectId", this.KindObjectId);
        }
    }

    //分类数据模型
    public class BmobKindModel : BmobTable
    {
        public string fTable { get;set; }
        public string ParentId { get; set; }
        public string Name { get; set; }

        //构造函数
        public BmobKindModel() { }

        //构造函数
        public BmobKindModel(String tableName)
        {
            this.fTable = tableName;
        }

        public override string table
        {
            get
            {
                if (fTable != null)
                {
                    return fTable;
                }
                return base.table;
            }
        }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.ParentId = input.getString("ParentId");
            this.Name = input.getString("Name");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("ParentId", this.ParentId);
            output.Put("Name", this.Name);
        }
    }

    /// <summary>
    /// 用户操作记录模型
    /// </summary>
    public class BmobOperationModel : BmobTable
    {
        public string fTable { get; set; }
        public string User { get; set; }
        public string Operate { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }

        //构造函数
        public BmobOperationModel() { }

        //构造函数
        public BmobOperationModel(String tableName)
        {
            this.fTable = tableName;
        }

        public override string table
        {
            get
            {
                if (fTable != null)
                {
                    return fTable;
                }
                return base.table;
            }
        }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.User = input.getString("User");
            this.Operate = input.getString("Operate");
            this.Title = input.getString("Title");
            this.Kind = input.getString("Kind");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("User", this.User);
            output.Put("Operate", this.Operate);
            output.Put("Title", this.Title);
            output.Put("Kind", this.Kind);
        }
    }

    //用户数据模型
    public class BmobUserModel : BmobTable
    {
        public string fTable { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        //构造函数
        public BmobUserModel() { }

        //构造函数
        public BmobUserModel(String tableName)
        {
            this.fTable = tableName;
        }

        public override string table
        {
            get
            {
                if (fTable != null)
                {
                    return fTable;
                }
                return base.table;
            }
        }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.username = input.getString("username");
            this.Name = input.getString("Name");
            this.Password = input.getString("Password");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("username", this.username);
            output.Put("Name", this.Name);
            output.Put("Password", this.Password);
        }
    }
}