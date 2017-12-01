using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.battle
{
    /// <summary>
    /// 招兵买马
    /// </summary>
    public class WorldExpansionInfo : XmlObject
    {
        public string areaids;//正在招募的城市,逗号隔开
        public int needchange;//需要升级兵营
        public int needtroops;//需要获得兵力
        public int havetroops;//已经获得兵力
        public int maxtimes;//今日最大招募次数
        public int todaytimes;//今日招募次数
        public int maxplunder;//今日最大掠夺兵力
        public int todayplunder;//今日掠夺兵力
        public int status;//状态,0:空闲中;1:募兵中;2:领取兵力
        public int currenttimes;//当前招募次数
        public int canrecvnum;//可领取兵力次数
        public int remaintime;//剩余时间

        public WorldExpansionInfo()
        {
            areaids = "";
            needchange = 0;
            needtroops = 0;
            havetroops = 0;
            maxtimes = 0;
            todaytimes = 0;
            maxplunder = 0;
            todayplunder = 0;
            status = 0;
            currenttimes = 0;
            canrecvnum = 0;
            remaintime = 0;
        }

        public override void Parse(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "areaids") areaids = item.InnerText;
                else if (item.Name == "needtroops") needtroops = int.Parse(item.InnerText);
                else if (item.Name == "havetroops") havetroops = int.Parse(item.InnerText);
                else if (item.Name == "maxtimes") maxtimes = int.Parse(item.InnerText);
                else if (item.Name == "todaytimes") todaytimes = int.Parse(item.InnerText);
                else if (item.Name == "maxplunder") maxplunder = int.Parse(item.InnerText);
                else if (item.Name == "todayplunder") todayplunder = int.Parse(item.InnerText);
                else if (item.Name == "status") status = int.Parse(item.InnerText);
                else if (item.Name == "currenttimes") currenttimes = int.Parse(item.InnerText);
                else if (item.Name == "canrecvnum") canrecvnum = int.Parse(item.InnerText);
                else if (item.Name == "remaintime") remaintime = int.Parse(item.InnerText);
                else if (item.Name == "needchange") needchange = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return true;
        }

        public override string ToString()
        {
            if (needchange == 1) return string.Format("请先升级兵营");
            else return string.Format("累计获得兵力：{0}/{1}, 掠夺兵力：{2}/{3}, 今天招募次数：{4}/{5}", havetroops, needtroops, todayplunder, maxplunder, todaytimes, maxtimes);
        }
    }
}
