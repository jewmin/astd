using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.misc
{
    public class Reward
    {
        public int Type;
        public string ItemName;
        public int Quality;
        public int Lv;
        public int Num;

        public Reward()
        {
            Type = 0;
            ItemName = "";
            Quality = 1;
            Lv = 1;
            Num = 0;
        }

        public override string ToString()
        {
            string reward;
            switch (Type)
            {
                case 1:
                    reward = "银币";
                    break;
                case 2:
                    reward = "玉石";
                    break;
                case 5:
                    reward = string.Format("{0}级宝石", Lv);
                    break;
                case 6:
                    reward = string.Format("兵器[{0}]", ItemName);
                    break;
                case 7:
                    reward = string.Format("兵器碎片[{0}]", ItemName);
                    break;
                case 8:
                    reward = "征收次数";
                    break;
                case 9:
                    reward = "纺织次数";
                    break;
                case 10:
                    reward = "通商次数";
                    break;
                case 11:
                    reward = "炼化次数";
                    break;
                case 12:
                    reward = "兵力减少";
                    break;
                case 13:
                    reward = "副本重置卡";
                    break;
                case 14:
                    reward = "战役双倍卡";
                    break;
                case 15:
                    reward = "强化暴击卡";
                    break;
                case 16:
                    reward = "强化打折卡";
                    break;
                case 17:
                    reward = "兵器提升卡";
                    break;
                case 18:
                    reward = "兵器暴击卡";
                    break;
                case 27:
                    reward = "进货令";
                    break;
                case 28:
                    reward = "军令";
                    break;
                case 29:
                    reward = "政绩翻倍卡";
                    break;
                case 30:
                    reward = "征收翻倍卡";
                    break;
                case 31:
                    reward = "商人召唤卡";
                    break;
                case 32:
                    reward = "纺织翻倍卡";
                    break;
                case 34:
                    reward = "行动力";
                    break;
                case 35:
                    reward = "摇钱树";
                    break;
                case 36:
                    reward = "超级门票";
                    break;
                case 38:
                    reward = string.Format("宝物[{0}+{1}]", ItemName, Lv);
                    break;
                case 39:
                    reward = "金币";
                    break;
                case 42:
                    reward = "点券";
                    break;
                case 43:
                    reward = "神秘宝箱";
                    break;
                case 44:
                    reward = "家传玉佩";
                    break;
                case 48:
                    reward = string.Format("{0}倍暴击铁锤", Lv);
                    break;
                case 49:
                    reward = string.Format("大将令[{0}]", ItemName);
                    break;
                case 50:
                    reward = "镔铁";
                    break;
                case 52:
                case 53:
                case 54:
                    reward = ItemName;
                    break;
                default:
                    reward = string.Format("[type={0},itemname={1},quality={2},lv={3},num={4}]", Type, ItemName, Quality, Lv, Num);
                    break;
            }
            return string.Format("{0}{1}{2}", reward, Num > 0 ? "+" : "", Num);
        }
    }

    public class RewardInfo
    {
        private List<Reward> _reward_list = new List<Reward>();

        public Reward getReward(int idx)
        {
            if (idx >= 0 && idx < _reward_list.Count)
            {
                return _reward_list[idx];
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Reward reward in _reward_list)
            {
                sb.AppendFormat("{0} ", reward.ToString());
            }
            return sb.ToString();
        }

        public void handleXmlNode(XmlNode xml)
        {
            _reward_list.Clear();
            if (xml == null)
            {
                return;
            }
            XmlNodeList list = xml.SelectNodes("reward");
            if (list == null)
            {
                return;
            }
            foreach (XmlNode node in list)
            {
                Reward reward = new Reward();
                XmlNode attr = node.SelectSingleNode("type");
                if (attr != null)
                {
                    reward.Type = int.Parse(attr.InnerText);
                }
                attr = node.SelectSingleNode("lv");
                if (attr != null)
                {
                    reward.Lv = int.Parse(attr.InnerText);
                }
                attr = node.SelectSingleNode("num");
                if (attr != null)
                {
                    reward.Num = int.Parse(attr.InnerText);
                }
                attr = node.SelectSingleNode("itemname");
                if (attr != null)
                {
                    reward.ItemName = attr.InnerText;
                }
                attr = node.SelectSingleNode("quality");
                if (attr != null)
                {
                    reward.Quality = int.Parse(attr.InnerText);
                }
                _reward_list.Add(reward);
            }
        }
    }
}
