using System.ComponentModel;
using System.Web;

namespace ITResourceLibrary.Helps
{
    public class SessionHelp
    {
        public static void Set(string key, object value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        //获取session并转换成类型T
        public static T Get<T>(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            {
                var result = HttpContext.Current.Session[key];
                var type = typeof(T);//解读参考网址
                var converter = TypeDescriptor.GetConverter(type);//得到此类型的类型转换器
                if (converter != null)
                {
                    return (T)result;
                }
            }
            return default(T);//意思参照参考网址
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        //获取session并转换成object
        public static object Get(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            {
                return HttpContext.Current.Session[key];
            }
            return null;
        }
    }
}