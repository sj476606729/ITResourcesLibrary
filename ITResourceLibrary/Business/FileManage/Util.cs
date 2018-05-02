using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.Business.FileManage
{
    public class Util
    {
        private static Util util=null;

        static BucketManager bucketManager=null;
        private Util() {
           
        }
        public static Util getInstance()
        {
            if (util == null)
            {
                util = new Util();
            }
            return util;
        }
        public BucketManager getBucketManager()
        {
            if (bucketManager == null)
            {
                Mac mac = new Mac("pKP4DoyhjVvvW_UirFDc-nhzQSX9nNmuQRPzGqH8", "oMLxDz-zF4WpAahcY6OiecjfWJXvMDAjJurCJxk6");
                Config config = new Config();
                // 空间对应的机房
                config.Zone = Zone.ZONE_CN_East;
                // 是否使用https域名
                config.UseHttps = true;
                // 上传是否使用cdn加速
                config.UseCdnDomains = true;
                // 是否设置强制覆盖
                Boolean force = true;
                bucketManager = new BucketManager(mac, config);
            }
            return bucketManager;
        }
    }
}