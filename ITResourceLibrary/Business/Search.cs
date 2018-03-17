﻿using cn.bmob.api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bmob_space;
using cn.bmob.io;
using System.Data;
using ITResourceLibrary.HandlerData;

namespace Search
{
    public class SearchOperate
    {
        BmobWindows Bmob = new BmobWindows();
        Bmob_Initial initial = Bmob_Initial.Initial();
        //搜索排序
        public string SearchTitle(string Title)
        {
            Util util = Util.Instance;
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList result = new ArrayList();
            if (Operation.Code_Data == null) { return "还未初始化，请等待"; }
            foreach(DataRow row in Operation.Code_Data.Rows)
            {
                list.Add(row["Title"]);
                

                list2.Add(row["ObjectId"]);
            }
            CallBack callback = new CallBack();
            util.paixu(list, list2, Title, callback);
            return callback.result;


        }
    }
    public class CallBack : Back
    {

        public string result { get; set; }
        public void success(ArrayList listtitle, ArrayList listtitleid)
        {

            if (listtitle.Count > 0)
            {
                string json = "[";
                string json2 = "[";
                foreach (object data in listtitle)
                {
                    json += "{\"title\":\"" + data.ToString() + "\"},";
                }
                json = json.Substring(0, json.Length - 1) + "]";
                foreach (object data in listtitleid)
                {
                    json2 += "{\"objectid\":\"" + data.ToString() + "\"},";
                }
                json2 = json2.Substring(0, json2.Length - 1) + "]";

                result = "{\"result\":" + json + ",\"objectids\":" + json2 + "}";
             
            }
            else result = "未搜到信息";
        }
    }
    public class Util
    {
        static Util util = null;
        /**
         * equls 和 plus根据自己需要调整大小
         */
        //匹配值达到equls 则在listview中显示出来（匹配值为最终匹配值包含了plus）
        private const float equls = 0.3f;
        //全包含情况下匹配值再加一个plus
        private const float plus = 0.25f;
        private Util() { }

        public static Util Instance
        {
            get
            {
                if (util == null)
                {
                    util = new Util();
                }
                return util;
            }

        }
        /// <summary>
        /// 入口方法返回降序的ArrayList类型
        /// </summary>
        /// <param name="数据搜索出来的数据集"></param>
        /// <param name="搜索的关键字"></param>
        /// <returns></returns>
        public void paixu(ArrayList array, ArrayList arrayid, String title, Back iback)
        {
            if (title.Length == 0)
            {
                iback.success(array, arrayid);
            }
            title = title.ToLower();
            string[] array_s = (string[])array.ToArray(typeof(string));
            double[] num = new double[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                num[i] = (similarCalc(array[i].ToString().ToLower(), title));
                num[i] += (similarCalc2(array[i].ToString().ToLower(), title));
                if (array[i].ToString().ToLower().Contains(title))
                {
                    num[i] += plus;
                }
            }


            double b;
            string s, c;
            for (int i = 0; i < array.Count - 1; i++)
            {

                for (int j = i + 1; j < array.Count; j++)
                {
                    if (num[i] < num[j])
                    {
                        s = array_s[j];
                        array_s[j] = array_s[i];
                        array_s[i] = s;


                        b = num[j];
                        num[j] = num[i];
                        num[i] = b;

                        c = arrayid[j].ToString();
                        arrayid[j] = arrayid[i];
                        arrayid[i] = c;
                    }
                }
            }

            array = new ArrayList(array_s);
            ArrayList array2 = new ArrayList();
            ArrayList array3 = new ArrayList();
            //排除
            for (int i = 0; i < array.Count; i++)
            {
                if (num[i] > equls)
                {

                    array2.Add(array[i]);
                    array3.Add(arrayid[i]);
                }
            }

            // MessageBox.Show("" + array2.Count);
            iback.success(array2, array3);
        }
        /**
        *此处直至最后为字符串匹配算法
*/
        public static double similarCalc(String s1, String s2)
        {

            int value = matrix(s1, s2);

            return 1 - (double)value / Math.Max(s1.Length, s2.Length);

        }
        public static int matrix(String s1, String s2)
        {

            int[,] matrix;

            int n = s1.Length;

            int m = s2.Length;

            int i;

            int j;

            char c1;

            char c2;

            int temp;

            if (n == 0) return m;

            if (m == 0) return n;



            matrix = new int[n + 1, m + 1];

            for (i = 0; i <= n; i++)
            {

                matrix[i, 0] = i;

            }



            for (j = 0; j <= m; j++)
            {

                matrix[0, j] = j;

            }

            for (i = 1; i <= n; i++)
            {

                c1 = s1.ToCharArray()[i - 1];



                for (j = 1; j <= m; j++)
                {

                    c2 = s2.ToCharArray()[j - 1];

                    if (c1 == c2) temp = 0;

                    else temp = 1;



                    matrix[i, j] = min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1, matrix[i - 1, j - 1] + temp);

                }

            }



            return matrix[n, m];

        }

        private static int min(int one, int two, int three)
        {

            int min = one;

            if (two < min) min = two;

            if (three < min) min = three;

            return min;

        }

        /*字符倒序*/

        public static double similarCalc2(String s1, String s2)
        {

            int value = matrix(swapWords(s1), s2);

            return 1 - (double)value / Math.Max(s1.Length, s2.Length);

        }

        /**将字符倒过来*/

        public static String swapWords(String str)
        {

            char[] arr = str.ToCharArray();

            swap(arr, 0, arr.Length - 1);

            int begin = 0;

            for (int i = 1; i < arr.Length; i++)
            {

                if (arr[i] == ' ')
                {

                    swap(arr, begin, i - 1);

                    begin = i + 1;

                }

            }



            return new String(arr);

        }

        private static void swap(char[] arr, int begin, int end)
        {

            while (begin < end)
            {

                char temp = arr[begin];

                arr[begin] = arr[end];

                arr[end] = temp;

                begin++;

                end--;

            }

        }




    }
}

