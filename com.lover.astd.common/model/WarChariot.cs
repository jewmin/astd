using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    /// <summary>
    /// 铁锤
    /// </summary>
    public class Hammer
    {
        /// <summary>
        /// 暴击倍数
        /// </summary>
        private int _cri = 1;

        public int Cri
        {
            get { return _cri; }
            set { _cri = value; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        private int _num = 0;

        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }

        public void handleXmlNode(XmlNode xml)
        {
            if (xml != null)
            {
                string[] pairs = xml.InnerText.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length >= 3)
                {
                    if (pairs[0] == "hammer_10")
                    {
                        _cri = 10;
                    }
                    else if (pairs[0] == "hammer_4")
                    {
                        _cri = 4;
                    }
                    int.TryParse(pairs[2], out _num);
                }
            }
        }
    }
    /// <summary>
    /// 战车
    /// </summary>
    public class WarChariot
    {
        /// <summary>
        /// 战车id
        /// </summary>
        private int _id = 0;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        private string _name = "";

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 原石
        /// </summary>
        private int _bowlder = 0;

        public int Bowlder
        {
            get { return _bowlder; }
            set { _bowlder = value; }
        }
        /// <summary>
        /// 兵器碎片
        /// </summary>
        private int _equipitemnum = 0;

        public int Equipitemnum
        {
            get { return _equipitemnum; }
            set { _equipitemnum = value; }
        }
        /// <summary>
        /// 可提升等级
        /// </summary>
        private int _needtofull = 0;

        public int Needtofull
        {
            get { return _needtofull; }
            set { _needtofull = value; }
        }
        /// <summary>
        /// 当前进度
        /// </summary>
        private int _upgradeeffectnum = 0;

        public int Upgradeeffectnum
        {
            get { return _upgradeeffectnum; }
            set { _upgradeeffectnum = value; }
        }
        /// <summary>
        /// 提升进度
        /// </summary>
        private int _upgradenum = 0;

        public int Upgradenum
        {
            get { return _upgradenum; }
            set { _upgradenum = value; }
        }
        /// <summary>
        /// 总进度
        /// </summary>
        private int _total = 0;

        public int Total
        {
            get { return _total; }
            set { _total = value; }
        }
        /// <summary>
        /// 可升级
        /// </summary>
        private bool _islaststrengthenflag = false;

        public bool Islaststrengthenflag
        {
            get { return _islaststrengthenflag; }
            set { _islaststrengthenflag = value; }
        }
        /// <summary>
        /// 升级
        /// </summary>
        private bool _islaststrengthen = false;

        public bool Islaststrengthen
        {
            get { return _islaststrengthen; }
            set { _islaststrengthen = value; }
        }
        /// <summary>
        /// 等级
        /// </summary>
        private int _equiplevel = 0;

        public int Equiplevel
        {
            get { return _equiplevel; }
            set { _equiplevel = value; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        private int _type = 0;

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 消耗玉石
        /// </summary>
        private int _needbowlder = 0;

        public int Needbowlder
        {
            get { return _needbowlder; }
            set { _needbowlder = value; }
        }
        /// <summary>
        /// 消耗兵器碎片
        /// </summary>
        private int _needequipitem = 0;

        public int Needequipitem
        {
            get { return _needequipitem; }
            set { _needequipitem = value; }
        }
        /// <summary>
        /// 墨子铁锤
        /// </summary>
        private int _mozichuizi = 0;

        public int Mozichuizi
        {
            get { return _mozichuizi; }
            set { _mozichuizi = value; }
        }
        /// <summary>
        /// 其他铁锤
        /// </summary>
        private List<Hammer> _hammerList = new List<Hammer>();

        public List<Hammer> HammerList
        {
            get { return _hammerList; }
            set { _hammerList = value; }
        }

        public int Chuizi
        {
            get
            {
                if (_mozichuizi > 0)
                {
                    return 1;
                }
                else
                {
                    foreach (Hammer hammer in _hammerList)
                    {
                        if (hammer.Num > 0)
                        {
                            return hammer.Cri;
                        }
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 暴击倍数
        /// </summary>
        private int _isbaoji = 1;

        public int Isbaoji
        {
            get { return _isbaoji; }
            set { _isbaoji = value; }
        }
        /// <summary>
        /// 余料
        /// </summary>
        private int _surplus = 0;

        public int Surplus
        {
            get { return _surplus; }
            set { _surplus = value; }
        }

        /// <summary>
        /// 进度比例
        /// </summary>
        public int Percent
        {
            get { return _upgradeeffectnum * 100 / _total; }
        }
        /// <summary>
        /// 增加普攻
        /// </summary>
        private double _att = 0.0;
        /// <summary>
        /// 增加普防
        /// </summary>
        private double _def = 0.0;
        /// <summary>
        /// 增加战攻
        /// </summary>
        private double _satt = 0.0;
        /// <summary>
        /// 增加战防
        /// </summary>
        private double _sdef = 0.0;
        /// <summary>
        /// 增加策攻
        /// </summary>
        private double _stgatt = 0.0;
        /// <summary>
        /// 增加策防
        /// </summary>
        private double _stgdef = 0.0;
        /// <summary>
        /// 增加兵数
        /// </summary>
        private double _hp = 0.0;

        public string toString()
        {
            return string.Format("当前进度({0}%),普攻+{1},普防+{2},战攻+{3},战防+{4},策攻+{5},策防+{6},可带兵数+{7}", Percent, _att, _def, _satt, _sdef, _stgatt, _stgdef, _hp);
        }

        public int GetHammer(int limit_level)
        {
            foreach (Hammer hammer in _hammerList)
            {
                if (hammer.Cri <= limit_level && hammer.Num > 0)
                {
                    return hammer.Cri;
                }
            }

            return 0;
        }

        public void fillValues(XmlNodeList nodes)
        {
            foreach (XmlNode xmlNode in nodes)
            {
                if (xmlNode.Name == "warchariotid")
                {
                    int.TryParse(xmlNode.InnerText, out _id);
                }
                else if (xmlNode.Name == "wutaoname")
                {
                    Name = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "bowlder")
                {
                    int.TryParse(xmlNode.InnerText.Substring(0, xmlNode.InnerText.Length - 3), out _bowlder);
                }
                else if (xmlNode.Name == "equipitemnum")
                {
                    int.TryParse(xmlNode.InnerText, out _equipitemnum);
                }
                else if (xmlNode.Name == "needtofull")
                {
                    int.TryParse(xmlNode.InnerText, out _needtofull);
                }
                else if (xmlNode.Name == "upgradeeffectnum")
                {
                    int.TryParse(xmlNode.InnerText, out _upgradeeffectnum);
                }
                else if (xmlNode.Name == "upgradenum")
                {
                    int.TryParse(xmlNode.InnerText, out _upgradenum);
                }
                else if (xmlNode.Name == "total")
                {
                    int.TryParse(xmlNode.InnerText, out _total);
                }
                else if (xmlNode.Name == "islaststrengthenflag")
                {
                    _islaststrengthenflag = xmlNode.InnerText == "1";
                }
                else if (xmlNode.Name == "equiplevel")
                {
                    int.TryParse(xmlNode.InnerText, out _equiplevel);
                }
                else if (xmlNode.Name == "type")
                {
                    int.TryParse(xmlNode.InnerText, out _type);
                }
                else if (xmlNode.Name == "needbowlder")
                {
                    int.TryParse(xmlNode.InnerText, out _needbowlder);
                }
                else if (xmlNode.Name == "needequipitem")
                {
                    int.TryParse(xmlNode.InnerText, out _needequipitem);
                }
                else if (xmlNode.Name == "mozichuizi")
                {
                    int.TryParse(xmlNode.InnerText, out _mozichuizi);
                }
                else if (xmlNode.Name == "hammer")
                {
                    if (xmlNode.HasChildNodes)
                    {
                        Hammer hammer = new Hammer();
                        foreach (XmlNode childNode in xmlNode.ChildNodes)
                        {
                            if (childNode.Name == "cri")
                            {
                                hammer.Cri = int.Parse(childNode.InnerText);
                            }
                            else if (childNode.Name == "num")
                            {
                                hammer.Num = int.Parse(childNode.InnerText);
                            }
                        }
                        _hammerList.Add(hammer);
                    }
                }
                else if (xmlNode.Name == "addatt")
                {
                    double.TryParse(xmlNode.InnerText, out _att);
                }
                else if (xmlNode.Name == "adddef")
                {
                    double.TryParse(xmlNode.InnerText, out _def);
                }
                else if (xmlNode.Name == "addsatt")
                {
                    double.TryParse(xmlNode.InnerText, out _satt);
                }
                else if (xmlNode.Name == "addsdef")
                {
                    double.TryParse(xmlNode.InnerText, out _sdef);
                }
                else if (xmlNode.Name == "addstgatt")
                {
                    double.TryParse(xmlNode.InnerText, out _stgatt);
                }
                else if (xmlNode.Name == "addstgdef")
                {
                    double.TryParse(xmlNode.InnerText, out _stgdef);
                }
                else if (xmlNode.Name == "addhp")
                {
                    double.TryParse(xmlNode.InnerText, out _hp);
                }
                else if (xmlNode.Name == "isbaoji")
                {
                    int.TryParse(xmlNode.InnerText, out _isbaoji);
                }
                else if (xmlNode.Name == "surplus")
                {
                    int.TryParse(xmlNode.InnerText, out _surplus);
                }
            }
        }
    }
}
