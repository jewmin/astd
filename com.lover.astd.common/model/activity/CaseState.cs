using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.activity
{
    public class CaseState : XmlObject
    {
        public int id;
        public int state;

        public override void Parse(System.Xml.XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "id") id = int.Parse(item.InnerText);
                else if (item.Name == "state") state = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return state == 1;
        }
    }
}
