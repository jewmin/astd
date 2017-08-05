using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class BigHero : AsObject, IComparable
    {
        private int level_;
        private int big_;
        private int big_level_;
        private int tufei_;
        private int exp_;
        private int max_exp_;
        private int pos_;
        private int change_;
        private int index_;
        private int generaltype_;
        /// <summary>
        /// 武将等级
        /// </summary>
        public int Level
        {
            get { return level_; }
            set { level_ = value; }
        }
        /// <summary>
        /// 是否转成大将
        /// </summary>
        public int Big
        {
            get { return big_; }
            set { big_ = value; }
        }
        /// <summary>
        /// 大将等级
        /// </summary>
        public int BigLevel
        {
            get { return big_level_; }
            set { big_level_ = value; }
        }
        /// <summary>
        /// 大将突飞令数量
        /// </summary>
        public int TuFei
        {
            get { return tufei_; }
            set { tufei_ = value; }
        }
        /// <summary>
        /// 大将当前经验
        /// </summary>
        public int Exp
        {
            get { return exp_; }
            set { exp_ = value; }
        }
        /// <summary>
        /// 大将下级经验
        /// </summary>
        public int MaxExp
        {
            get { return max_exp_; }
            set { max_exp_ = value; }
        }
        /// <summary>
        /// 大将训练位
        /// </summary>
        public int TrainPos
        {
            get { return pos_; }
            set { pos_ = value; }
        }
        /// <summary>
        /// 大将晋升
        /// </summary>
        public int Change
        {
            get { return change_; }
            set { change_ = value; }
        }
        /// <summary>
        /// 大将类型 0:战法 1:策略 2:机械
        /// </summary>
        public int GeneralType
        {
            get { return generaltype_; }
            set { generaltype_ = value; }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Index
        {
            get { return index_; }
            set { index_ = value; }
        }
        /// <summary>
        /// 大将描述
        /// </summary>
        public string Desc
        {
            get { return string.Format("{0} {1}级", Name, BigLevel); }
        }
        /// <summary>
        /// 处理xml
        /// </summary>
        /// <param name="nodes"></param>
        public void fillValues(XmlNodeList nodes)
        {
            index_ = 9999;
            foreach (XmlNode xmlNode in nodes)
            {
                if (xmlNode.Name == "generalid")
                {
                    Id = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "name")
                {
                    Name = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "generallv")
                {
                    Level = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "big")
                {
                    Big = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "biglv")
                {
                    BigLevel = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "num")
                {
                    TuFei = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "exp")
                {
                    Exp = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "maxexp")
                {
                    MaxExp = int.Parse(xmlNode.InnerText);
                }
            }
        }
        /// <summary>
        /// 比较大小，小于-1，等于0，大于1
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            BigHero hero = obj as BigHero;
            if (this.index_ < hero.index_) return -1;
            else if (this.index_ > hero.index_) return 1;
            else return 0;
        }
    }

    public class BigHeroExpBook : XmlObject
    {
        /// <summary>
        /// 经验书类型 0:战法 1:策略 2:机械
        /// </summary>
        public int type;
        /// <summary>
        /// 数量
        /// </summary>
        public int num;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                if (type == 0) return "战法经验书";
                else if (type == 1) return "策略经验书";
                else return "机械经验书";
            }
        }

        public override void Parse(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "type") type = int.Parse(item.InnerText);
                else if (item.Name == "num") num = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return num > 0;
        }
    }
}
