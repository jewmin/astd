using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.misc
{
    public class TradeInfo : XmlObject
    {
        public int id;
        public string name;
        public string cost;
        public int active;
        public string costtype;
        public int costnum;

        public TradeInfo()
        {
            id = 0;
            name = "";
            cost = "";
            active = 0;
            costtype = "";
            costnum = 0;
        }

        public override void Parse(System.Xml.XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "id") id = int.Parse(item.InnerText);
                else if (item.Name == "name") name = item.InnerText;
                else if (item.Name == "cost") cost = item.InnerText;
                else if (item.Name == "active") active = int.Parse(item.InnerText);
            }

            if (cost != "")
            {
                string[] split = cost.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2)
                {
                    costtype = split[0];
                    costnum = Convert.ToInt32(split[1]);
                }
            }
        }

        public override bool CanAdd()
        {
            if (costtype == "copper") return true;
            return false;
        }
    }
}
