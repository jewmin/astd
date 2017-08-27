using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class GeneralAwakeInfo : XmlObject
    {
        public int isfull;
        public int maxlevel;
        public string awakentips;
        public int liquornum;
        public int freeliquornum;
        public int needliquornum;
        public int needbaoshinum;
        public int isawaken;
        public override void Parse(System.Xml.XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "isfull") isfull = int.Parse(item.InnerText);
                else if (item.Name == "maxlevel") maxlevel = int.Parse(item.InnerText);
                else if (item.Name == "awakentips") awakentips = item.InnerText;
                else if (item.Name == "liquornum") liquornum = int.Parse(item.InnerText);
                else if (item.Name == "freeliquornum") freeliquornum = int.Parse(item.InnerText);
                else if (item.Name == "needliquornum") needliquornum = int.Parse(item.InnerText);
                else if (item.Name == "needbaoshinum") needbaoshinum = int.Parse(item.InnerText);
                else if (item.Name == "isawaken") isawaken = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return true;
        }
    }
}
