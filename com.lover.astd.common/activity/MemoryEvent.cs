using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;
using com.lover.astd.common.model.misc;

namespace com.lover.astd.common.activity
{
    public class MemoryEvent : MgrBase
    {
        public class hongbao : BaseObject
        {
            public int id { get; set; }
            public int num { get; set; }
            /// <summary>
            /// 0 不可打开 1可打开
            /// </summary>
            public int canopen { get; set; }
            public int cost { get; set; }
            public hongbao()
            {
                id = 0;
                num = 0;
                canopen = 0;
                cost = 0;
            }
        }

        public class general : BaseObject
        {
            public int id { get; set; }
            public string name { get; set; }
            public int state { get; set; }
            public general()
            {
                id = 0;
                name = "";
                state = 0;
            }
        }

        public class picreward : BaseObject
        {
            public int id { get; set; }
            public int state { get; set; }
            public picreward()
            {
                id = 0;
                state = 0;
            }
        }

        public MemoryEvent(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Aquamarine;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }

        public int getMemoryEventInfo(ProtocolMgr protocol, ILogger logger, User user, int wishcost_limit, int hongbaocost_limit)
        {
            string url = "/root/memoryEvent!getMemoryEventInfo.action";
            ServerResult xml = protocol.getXml(url, "新春拜年 - 活动界面");
            if (xml == null || !xml.CmdSucceed) return 10;

            int freetimes = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/freetimes"));
            int year = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/year"));
            int finishtask = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/finishtask"));
            int finish = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/finish"));
            int wishcost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/wishcost"));
            List<hongbao> hongbaolist = XmlHelper.GetClassList<hongbao>(xml.CmdResult.SelectNodes("/results/hongbao"));
            List<picreward> picrewardlist = XmlHelper.GetClassList<picreward>(xml.CmdResult.SelectNodes("/results/picreward"));
            List<general> generallist = XmlHelper.GetClassList<general>(xml.CmdResult.SelectNodes("/results/general"));
            
            //红包
            foreach (hongbao hb in hongbaolist)
            {
                if (hb.canopen == 1)
                {
                    if (hb.num > 0 || hb.cost <= hongbaocost_limit)
                    {
                        openHongbao(protocol, logger, user, hb);
                        return 0;
                    }
                }
            }

            //回忆图
            foreach (picreward pr in picrewardlist)
            {
                if (pr.state == 1)
                {
                    openPicReward(protocol, logger, user, pr);
                }
            }

            if (freetimes > 0 || wishcost <= wishcost_limit)
            {
                newYearVisit(protocol, logger, user);
                return 0;
            }

            return 2;
        }

        public void newYearVisit(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/memoryEvent!newYearVisit.action";
            ServerResult xml = protocol.getXml(url, "新春拜年 - 拜年");
            if (xml == null || !xml.CmdSucceed) return;

            string name = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/name"));
            int light = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/light"));
            
            RewardInfo rewardinfo = new RewardInfo();
            rewardinfo.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("新春拜年, 和武将[{0}]拜年{1}, 获得{2}", name, light > 0 ? ", 点亮武将" : "", rewardinfo.ToString()));
        }

        public void openPicReward(ProtocolMgr protocol, ILogger logger, User user, picreward pr)
        {
            string url = "/root/memoryEvent!openPicReward.action";
            string data = string.Format("rewardId={0}", pr.id);
            ServerResult xml = protocol.postXml(url, data, "新春拜年 - 领取回忆图奖励");
            if (xml == null || !xml.CmdSucceed) return;
            
            RewardInfo rewardinfo = new RewardInfo();
            rewardinfo.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("新春拜年, 领取回忆图奖励, 获得{0}", rewardinfo.ToString()));
        }

        public void openHongbao(ProtocolMgr protocol, ILogger logger, User user, hongbao hb)
        {
            string url = "/root/memoryEvent!openHongbao.action";
            string data = string.Format("type={0}", hb.id);
            ServerResult xml = protocol.postXml(url, data, "新春拜年 - 领取红包");
            if (xml == null || !xml.CmdSucceed) return;

            RewardInfo rewardinfo = new RewardInfo();
            rewardinfo.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("新春拜年, 领取红包, 获得{0}", rewardinfo.ToString()));
        }
    }
}
