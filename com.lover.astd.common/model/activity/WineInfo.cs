using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.activity
{
    public class WineInfo
    {
        public int id_;
        public string name_;
        public int quality_;
        public int winenum_;
        public void fillValues(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "id")
                {
                    id_ = int.Parse(node.InnerText);
                }
                else if (node.Name == "name")
                {
                    name_ = node.InnerText;
                }
                else if (node.Name == "quality")
                {
                    quality_ = int.Parse(node.InnerText);
                }
                else if (node.Name == "winenum")
                {
                    winenum_ = int.Parse(node.InnerText);
                }
            }
        }
    }
}
