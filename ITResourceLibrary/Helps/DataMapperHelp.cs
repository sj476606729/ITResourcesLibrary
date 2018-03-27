using Nelibur.ObjectMapper;
using System.Collections.Generic;

namespace ITResourceLibrary.Helps
{
    public class DataMapperHelper
    {
        public static T Map<T>(object source)
        {
            return TinyMapper.Map<T>(source);
        }

        public static IEnumerable<T> MapList<T>(IEnumerable<object> source)
        {
            var result = new List<T>();
            foreach (var item in source)
            {
                result.Add(TinyMapper.Map<T>(item));
            }
            return result;
        }

        public static List<T> MapList<T>(List<object> source)
        {
            var result = new List<T>();
            foreach (var item in source)
            {
                result.Add(TinyMapper.Map<T>(item));
            }
            return result;
        }
    }
}