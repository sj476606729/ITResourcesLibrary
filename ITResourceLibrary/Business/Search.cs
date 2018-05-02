using ITResourceLibrary.Business;
using System.Linq;
using ITResourceLibrary.HandlerData;
using ITResourceLibrary.Helps;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Search
{
    public class SearchOperate : Function
    {
        List<string> selectTitles = new List<string>();
        List<ReturnTitle> list = new List<ReturnTitle>();
        ///搜索
        public string SearchTitle(string Title, string Select)
        {

            List<ListData> result = new List<ListData>();//搜索完返回结果
            if (KindCodeOperation.CodeData == null) { return "还未初始化，请等待"; }
            
            if (Select == "所有")//判断是否为所有
            {
            Titles("无");
            }
            else
            {
            string id = (from a in KindCodeOperation.KindData where a.ParentId == "无" && a.Name == Select select a).Single().objectId;
            Titles(id);
            }
            if (PublicPermission((string)SessionHelp.Get("UserName")))//判断是否为一半账户，分两组数据类分别存共有和私有
            {
                int i = 0;
                do
                {
                    string data = (string)selectTitles[i];
                    if (KindCodeOperation.CodeData.Where(x => x.Title == data).Single().Visible == "Invisible")
                    {
                        selectTitles.Remove(data);i--;
                    }
                    i++;
                } while (i < selectTitles.Count);
            }
            result= SearchEngines.Search(selectTitles, Title);//开始搜索并返回结果
            foreach(var a in result)//使得前端高亮匹配显示
            {
                List<int> count = new List<int>();
                for(int i=0;i<a.Word.Length -1;i++)
                {
                    int str = a.Search.ToLower().IndexOf(a.Word[i]);
                    for(int j = str; j < str + a.Word[i].Length; j++)
                    {
                        count.Add(j);
                    }
                }
                string Result = "<li onclick=\"ShowData(&quot;" + a.Search+ "&quot;,this)\">";
                for (int i = 0; i < a.Search.Length; i++)
                {
                    
                    if (count.Contains(i))
                    {
                        Result += "<div style=\"color:red\">" + a.Search.Substring(i, 1) + "</div>";
                    }
                    else { Result += "<div>" + a.Search.Substring(i, 1) + "</div>"; }
                }
                List<string> PathKind = new List<string>();//找出此标题的父分类
                var data = KindCodeOperation.CodeData.Where(x => x.Title == a.Search).Single();
                string id = KindCodeOperation.KindData.Where(x => x.objectId == data.KindObjectId ).Single().ParentId;
                KindCodeOperation.KindPath(id, ref PathKind);
                Result += "<div class='seach-move'><div style='float:none'>分类:" + PathKind[PathKind.Count-1]+"</div><div style='float:none'>作者:"+data.Author+"</div></div>";
                Result += "</li>";
                list.Add(new ReturnTitle() { title = Result });
            }
            string jsonData = JsonConvert.SerializeObject(list);
            return jsonData;
        }
        /// <summary>
        /// 根据选择分类获取搜索相关标题
        /// </summary>
        /// <param name="selectid"></param>
        public void Titles(string selectid)
        {
            string title = "";
            bool IsTitle = false;
            foreach(var data in KindCodeOperation.KindData)
            {
                if (title.Length == 0)
                {
                    if (data.objectId == selectid) { title = data.Name; }
                }
                if(data.ParentId==selectid)
                {
                    IsTitle = true;
                    Titles(data.objectId);
                }
            }
            if (IsTitle == false)
            {
                var data = KindCodeOperation.CodeData.Where(x => x.Title == title).ToList();
                if (data.Count > 0)
                {
                    selectTitles.Add(title);
                }
                
            }
            
        }
    }




public class ReturnTitle
    {
        public string title { get; set; }
    }
    /// <summary>
    /// 搜索引擎
    /// </summary>

    public class SearchEngines
    {
        public static List<ListData> Search(List<string> Source, string KeyWord)
        {
            string[] key = new string[Source.Count];
            string[] Word = new string[Source.Count];
            KeyWord = KeyWord.ToLower();
            Regex mRegex = new Regex("[a-zA-Z]+");
            MatchCollection mMactchCol;
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = ""; Word[i] = "";
                mMactchCol = mRegex.Matches(Source[i].ToLower());
                foreach (Match mMatch in mMactchCol)
                {
                    Word[i] += mMatch.ToString() + ",";
                }
            }
            for (int i = KeyWord.Length; i > 1; i--)
            {
                for (int j = 0; j <= KeyWord.Length - i; j++)
                {
                    string search = KeyWord.Substring(j, i);
                    Regex regChina = new Regex("^[\u4e00-\u9fbb]+$");
                    Regex regEnglish = new Regex("^[a-zA-Z]+$");
                    if (!regEnglish.IsMatch(search) && !regChina.IsMatch(search))
                        continue;
                    for (int k = 0; k < Source.Count; k++)
                    {
                        string[] split = key[k].Split(',');//检查是否已经包含关键词
                        bool Continue = false;
                        foreach (string a in split)
                        {
                            if (a.Contains(search)) { Continue = true; break; }
                        }
                        if (Continue) continue;
                        if (regEnglish.IsMatch(search))//判断关键词是不是英文
                        {
                            split = Word[k].Split(',');//检查字母是否符合搜索条件
                            float length = 10;
                            foreach (string a in split)
                            {
                                if (a.Contains(search))
                                {
                                    if (a.Length < length) length = a.Length;
                                }
                            }
                            double b = search.Length / length;
                            if ((search.Length / length) < 0.6)//用来判断搜索的字母长度和单词长度比例，小于一定比例不符合搜索条件
                                continue;
                        }

                        if (Source[k].ToLower().Contains(search)) { key[k] += (search + ","); }
                    }
                }
            }
            List<ListData> resultList = new List<ListData>();
            for (int i = 0; i < key.Count(); i++)
            {
                resultList.Add(new ListData { Search = key[i] , Key=i });
            }
            resultList.Sort(
                delegate (ListData x, ListData y)
                {
                    if (x.Search.Length > y.Search.Length) return -1; else return 1;
                }
                );
            List<ListData> result = new List<ListData>();
            foreach (ListData a in resultList)
            {
                if (a.Search.Length == 0) break;
                result.Add(new ListData() { Search = Source[a.Key], Word = a.Search.Split(',') });
            }
            return result;
        }
    }
    public class ListData
    {
        public string Search { get; set; }
        public int Key { get; set; }
        public string[] Word { get; set; }
    }
}
