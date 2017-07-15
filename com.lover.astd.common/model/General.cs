using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class General : XmlObject
    {
        public int generalid;
        public int zhugeid;
        public int zhugetimes;

        public General()
        {
            generalid = 0;
            zhugeid = 0;
            zhugetimes = 0;
        }

        public override void Parse(System.Xml.XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "generalid") generalid = int.Parse(item.InnerText);
                else if (item.Name == "zhugeid") zhugeid = int.Parse(item.InnerText);
                else if (item.Name == "zhugetimes") zhugetimes = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return (zhugeid > 0 && zhugetimes > 0);
        }
    }
}
