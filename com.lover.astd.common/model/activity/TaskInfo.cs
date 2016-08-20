using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.activity
{
    public class TaskInfo
    {
        public string name_;
        public string intro_;
        public int curnum_;
        public int num_;
        public int reward_;
        public int state_;
        public int cd_;

        public TaskInfo()
        {
            name_ = "";
            intro_ = "";
            curnum_ = 0;
            num_ = 0;
            reward_ = 0;
            state_ = 0;
            cd_ = 0;
        }

        public void handleXmlNode(XmlNode xml)
        {
            XmlNode attr = xml.SelectSingleNode("name");
            if (attr != null)
            {
                name_ = attr.InnerText;
            }
            attr = xml.SelectSingleNode("intro");
            if (attr != null)
            {
                intro_ = attr.InnerText;
            }
            attr = xml.SelectSingleNode("curnum");
            if (attr != null)
            {
                curnum_ = int.Parse(attr.InnerText);
            }
            attr = xml.SelectSingleNode("num");
            if (attr != null)
            {
                num_ = int.Parse(attr.InnerText);
            }
            attr = xml.SelectSingleNode("reward");
            if (attr != null)
            {
                reward_ = int.Parse(attr.InnerText);
            }
            attr = xml.SelectSingleNode("state");
            if (attr != null)
            {
                state_ = int.Parse(attr.InnerText);
            }
            attr = xml.SelectSingleNode("cd");
            if (attr != null)
            {
                cd_ = int.Parse(attr.InnerText);
            }
        }

        /// <summary>
        /// 连胜传奇
        /// </summary>
        /// <returns></returns>
        public bool isLianShengChuanQi()
        {
            return name_.Equals("连胜传奇");
        }
    }
}
