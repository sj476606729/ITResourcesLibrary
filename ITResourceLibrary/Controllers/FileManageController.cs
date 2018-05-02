using ITResourceLibrary.Business.FileManage;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ITResourceLibrary.Controllers
{
    public enum mode
    {
        PICTURE=0,
        FILE=1,
    };
    

    public class FileManageController : Controller
    {
        string url = "http://p0mrev3g3.bkt.clouddn.com/";
        string urll = "http://p0y9ixilz.bkt.clouddn.com/";//files空间
        // GET: FileManage
        public ActionResult Index()
        {
            return View();
        }
      
        public string getToken(int type)
        {
            Mac mac = new Mac("pKP4DoyhjVvvW_UirFDc-nhzQSX9nNmuQRPzGqH8", "oMLxDz-zF4WpAahcY6OiecjfWJXvMDAjJurCJxk6");
            PutPolicy putPolicy = new PutPolicy();
            switch (type)
            {
                case (int)mode.PICTURE:
                    putPolicy.Scope = "picture-file";
                    break;
                case (int)mode.FILE:
                    putPolicy.Scope = "files";
                    break;
            }
            
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            return token;
        }
        [ActionName("getTokenkey")]
        public string getToken(int type,string key)
        {
            Mac mac = new Mac("pKP4DoyhjVvvW_UirFDc-nhzQSX9nNmuQRPzGqH8", "oMLxDz-zF4WpAahcY6OiecjfWJXvMDAjJurCJxk6");
            PutPolicy putPolicy = new PutPolicy();
            switch (type)
            {
                case (int)mode.PICTURE:
                    putPolicy.Scope = "picture-file:"+key;
                    break;
                case (int)mode.FILE:
                    putPolicy.Scope = "files:"+key;
                    break;
            }

            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            return token;
        }
        public string reName(string oldname,string newname)
        {
            Regex regChina = new Regex("\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$");
            string bucket="";
            bucket=regChina.IsMatch(newname) ?"picture-file":"files";

            string oldBucket = bucket;
            // 原始文件名
            string Key = oldname;
            // 目标空间
            string newBucket = bucket;
            // 目标文件名
            string newKey = newname;
            // 是否设置强制覆盖
            Boolean force = true;
            BucketManager bucketManager = Util.getInstance().getBucketManager();
            HttpResult copyRet = bucketManager.Move(oldBucket, Key, newBucket, newKey, force);

            if (copyRet.Code != (int)HttpCode.OK)
            {
                return "rename error:" + copyRet.ToString();
            }
            return "success";
        }
        public string delFile(string filename)

        {
            Regex regChina = new Regex("\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$");
            string bucket = "";
            bucket = regChina.IsMatch(filename) ? "picture-file" : "files";
            // 空间名
            string Bucket = bucket;
            // 文件名
            string Key = filename;
            HttpResult deleteRet = Util.getInstance().getBucketManager().Delete(Bucket, Key);
            if (deleteRet.Code != (int)HttpCode.OK)
            {
                Console.WriteLine("delete error: " + deleteRet.ToString());
            }
            return "success";
        }
    }
}