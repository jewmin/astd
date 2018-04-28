using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.misc
{
    public class BuPi : XmlObject
    {
        public int id;
        public int modulus;

        public BuPi()
        {
            id = 0;
            modulus = 0;
        }

        public override void Parse(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "id") id = int.Parse(item.InnerText);
                else if (item.Name == "modulus") modulus = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return true;
        }
    }
}
