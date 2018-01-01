using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using com.lover.astd.common.model;

namespace com.lover.astd.common
{
    public class XmlHelper
    {
        /// <summary>
        /// 获取数值类型的值, 如int, float, double
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetValue<T>(XmlNode node, T def = default(T)) where T : struct
        {
            T t = def;
            if (node != null) t = (T)Convert.ChangeType(node.InnerText, typeof(T));
            return t;
        }
        /// <summary>
        /// 获取字符串类型的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string GetString(XmlNode node, string def = "")
        {
            string t = def;
            if (node != null) t = node.InnerText;
            return t;
        }
        /// <summary>
        /// 返回类类型的值, 类必须继承XmlObject类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetClass<T>(XmlNode node, T def = default(T)) where T : new()
        {
            T t = def;
            if (node != null) { t = new T(); (t as XmlObject).Parse(node); }
            return t;
        }
        /// <summary>
        /// 返回类类型列表，类必须继承XmlObject类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> GetClassList<T>(XmlNodeList nodelist) where T : new()
        {
            List<T> list = new List<T>();
            if (nodelist != null)
            {
                foreach (XmlNode node in nodelist)
                {
                    T t = new T();
                    (t as XmlObject).Parse(node);
                    if ((t as XmlObject).CanAdd()) list.Add(t);
                }
            }
            return list;
        }

        /// <summary>
        /// 分隔字符串，返回整型列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<int> GetStringSplit(XmlNode node, char separator = ',')
        {
            List<int> list = new List<int>();

            if (node != null)
            {
                string[] split_strings = node.InnerText.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in split_strings)
                {
                    list.Add(Convert.ToInt32(str));
                }
            }

            return list;
        }

        /// <summary>
        /// 字典顺序排序
        /// </summary>
        /// <param name="dic"></param>
        public static void DictionarySort(Dictionary<int, int> dic)
        {
            if (dic.Count > 0)
            {
                List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>(dic);
                list.Sort(delegate(KeyValuePair<int, int> s1, KeyValuePair<int, int> s2)
                {
                    return s1.Value.CompareTo(s2.Value);
                });
                dic.Clear();
                foreach (KeyValuePair<int, int> kvp in list)
                {
                    dic.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
