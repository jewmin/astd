using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.attack
{
    /// <summary>
    /// 封地可生产资源
    /// </summary>
    public class ProduceInfo
    {
        public int resid;
        public int state;
        public string bigname;
        public ProduceInfo()
        {
            resid = 0;
            state = 0;
            bigname = "";
        }
        public override string ToString()
        {
            switch (resid)
            {
                case 1:
                    return "宝石";
                case 2:
                    return "镔铁";
                case 3:
                    return "兵器";
                case 4:
                    return string.Format("大将令【{0}】", bigname);
                case 5:
                    return "觉醒酒";
                default:
                    return "无效资源";
            }
        }
        public void fillXmlNode(XmlNode xmlNode)
        {
            if (xmlNode == null) return;
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            foreach (XmlNode childNode in xmlNodeList)
            {
                if (childNode.Name == "resid")
                {
                    resid = int.Parse(childNode.InnerText);
                }
                else if (childNode.Name == "state")
                {
                    state = int.Parse(childNode.InnerText);
                }
                else if (childNode.Name == "bigname")
                {
                    bigname = childNode.InnerText;
                }
            }
        }
        public bool isValiable()
        {
            if (state == 0)
            {
                return false;
            }
            else if (resid != 4)
            {
                return true;
            }
            else if (bigname == "夏桀" || bigname == "王翦" || bigname == "后羿" || bigname == "黄帝" || bigname == "黄帝" || bigname == "李白")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
