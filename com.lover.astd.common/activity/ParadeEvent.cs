using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using System.Xml;
using com.lover.astd.common.model.misc;
using com.lover.astd.common.manager;
using System.Drawing;

namespace com.lover.astd.common.activity
{
    public class ParadeEvent : MgrBase
    {
        public class needformation : XmlObject
        {
            public int place;
            public int id;
            public int state;

            public needformation()
            {
                place = 0;
                id = 0;
                state = 0;
            }

            public override void Parse(System.Xml.XmlNode node)
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "place") place = int.Parse(item.InnerText);
                    else if (item.Name == "id") id = int.Parse(item.InnerText);
                    else if (item.Name == "state") state = int.Parse(item.InnerText);
                }
            }

            public override bool CanAdd()
            {
                return true;
            }
        }

        public class paradestate : XmlObject
        {
            public int id;
            public int state;
            public int neednum;

            public paradestate()
            {
                id = 0;
                state = 0;
                neednum = 0;
            }

            public override void Parse(XmlNode node)
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "id") id = int.Parse(item.InnerText);
                    else if (item.Name == "state") state = int.Parse(item.InnerText);
                    else if (item.Name == "neednum") neednum = int.Parse(item.InnerText);
                }
            }

            public override bool CanAdd()
            {
                return true;
            }
        }

        public int freetimes;
        public int freeroundtimes;
        public int cost;
        public int roundcost;
        public int totalmorale;
        public List<paradestate> paradestate_list;
        public List<needformation> needformation_list;
        public bool finish_formation;

        public ParadeEvent(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Chocolate;
            this._tmrMgr = tmrMgr;
            this._factory = factory;

            freetimes = 0;
            freeroundtimes = 0;
            cost = 100;
            roundcost = 100;
            totalmorale = 0;
            paradestate_list = new List<paradestate>();
            needformation_list = new List<needformation>();
            finish_formation = false;
        }

        public long execute(ProtocolMgr protocol, ILogger logger, User user, int gold_available, int limit_cost, int limit_roundcost)
        {
            int result = getParadeEventInfo(protocol, logger, user, gold_available, limit_cost, limit_roundcost);
            if (result == 0) return immediate();
            else if (result == 2) return next_day();
            else return next_halfhour();
        }

        public int getParadeEventInfo(ProtocolMgr protocol, ILogger logger, User user, int gold_available, int limit_cost, int limit_roundcost)
        {
            string url = "/root/paradeEvent!getParadeEventInfo.action";
            ServerResult result = protocol.getXml(url, "新国庆阅兵");
            if (result == null || !result.CmdSucceed) return 10;

            freetimes = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/freetimes"));
            freeroundtimes = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/freeroundtimes"));
            cost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/cost"), 100);
            roundcost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/roundcost"), 100);
            totalmorale = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/totalmorale"));
            paradestate_list = XmlHelper.GetClassList<paradestate>(result.CmdResult.SelectNodes("/results/paradestate"));
            needformation_list = XmlHelper.GetClassList<needformation>(result.CmdResult.SelectNodes("/results/needformation"));
            logInfo(logger, string.Format("新国庆阅兵: 免费阅兵+{0}, 轮次+{1}, 阅兵费用+{2}, 轮次购买费用+{3}, 士气+{4}", freetimes, freeroundtimes, cost, roundcost, totalmorale));

            //检查奖励
            foreach (paradestate item in paradestate_list)
            {
                if (item.state == 1)
                {
                    if (!getParadeReward(protocol, logger, item)) return 10;
                }
            }

            //检查轮次
            if (freeroundtimes <= 0)
            {
                if (roundcost > limit_roundcost || roundcost > gold_available) return 2;
                if (!addRoundTimes(protocol, logger, roundcost)) return 10;
                return 0;
            }

            //检查阵型
            //checkFormation();
            //if (finish_formation)
            //{
            //    if (!getNextGeneral(protocol, logger)) return 10;
            //    return 0;
            //}

            //免费阅兵
            while (freetimes > 0)
            {
                if (!paradeArmy(protocol, logger, freetimes, cost)) return 10;
                //if (finish_formation)
                //{
                //    if (!getNextGeneral(protocol, logger)) return 10;
                //    return 0;
                //}
            }

            //金币阅兵
            while (cost <= limit_cost && cost <= gold_available)
            {
                gold_available -= cost;
                if (!paradeArmy(protocol, logger, freetimes, cost)) return 10;
                if (finish_formation)
                {
                    if (!getNextGeneral(protocol, logger)) return 10;
                    return 0;
                }
            }

            //下一位武将
            if (!getNextGeneral(protocol, logger)) return 10;
            return 0;
        }

        public bool paradeArmy(ProtocolMgr protocol, ILogger logger, int use_freetimes, int use_cost)
        {
            string url = "/root/paradeEvent!paradeArmy.action";
            ServerResult result = protocol.getXml(url, "新国庆阅兵 - 开始阅兵");
            if (result == null || !result.CmdSucceed) return false;

            int army = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/army"));
            int change = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/change"));
            freetimes = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/freetimes"));
            cost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/cost"), 100);
            totalmorale = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/totalmorale"));
            paradestate_list = XmlHelper.GetClassList<paradestate>(result.CmdResult.SelectNodes("/results/paradestate"));
            if (change == 1)
            {
                updateFormation(army);
                checkFormation();
            }

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(result.CmdResult.SelectSingleNode("/results/rewardinfo"));
            RewardInfo bigreward = new RewardInfo();
            bigreward.handleXmlNode(result.CmdResult.SelectSingleNode("/results/bigreward/rewardinfo"));
            StringBuilder sb = new StringBuilder("新国庆阅兵: ");
            if (use_freetimes > 0) sb.Append("免费阅兵");
            else sb.AppendFormat("花费{0}金币阅兵", use_cost);
            if (change == 1) sb.Append(", 激活要求阵型");
            sb.AppendFormat(", 获得{0}{1}", reward.ToString(), bigreward.ToString());
            logInfo(logger, sb.ToString());
            return true;
        }

        public void updateFormation(int army)
        {
            foreach (needformation item in needformation_list)
            {
                if (item.id == army) item.state = 1;
            }
        }

        public void checkFormation()
        {
            finish_formation = true;
            foreach (needformation item in needformation_list)
            {
                if (item.state == 0)
                {
                    finish_formation = false;
                    break;
                }
            }
        }

        public bool getNextGeneral(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/paradeEvent!getNextGeneral.action";
            ServerResult result = protocol.getXml(url, "新国庆阅兵 - 下一位武将");
            if (result == null || !result.CmdSucceed) return false;

            freetimes = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/freetimes"));
            freeroundtimes = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/freeroundtimes"));
            cost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/cost"), 100);
            roundcost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/roundcost"), 100);
            totalmorale = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/totalmorale"));
            paradestate_list = XmlHelper.GetClassList<paradestate>(result.CmdResult.SelectNodes("/results/paradestate"));
            needformation_list = XmlHelper.GetClassList<needformation>(result.CmdResult.SelectNodes("/results/needformation"));
            logInfo(logger, string.Format("新国庆阅兵: 免费阅兵+{0}, 轮次+{1}, 阅兵费用+{2}, 轮次购买费用+{3}, 士气+{4}", freetimes, freeroundtimes, cost, roundcost, totalmorale));
            return true;
        }

        public bool addRoundTimes(ProtocolMgr protocol, ILogger logger, int use_roundcost)
        {
            string url = "/root/paradeEvent!addRoundTimes.action";
            ServerResult result = protocol.getXml(url, "新国庆阅兵 - 购买轮次");
            if (result == null || !result.CmdSucceed) return false;

            freeroundtimes = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/freeroundtimes"));
            roundcost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/roundcost"), 100);
            logInfo(logger, string.Format("新国庆阅兵: 花费{0}金币, 购买轮次", use_roundcost));
            return true;
        }

        public bool getParadeReward(ProtocolMgr protocol, ILogger logger, paradestate parade)
        {
            string url = "/root/paradeEvent!getParadeReward.action";
            string data = string.Format("rewardId={0}", parade.id);
            ServerResult result = protocol.postXml(url, data, "新国庆阅兵 - 领取奖励");
            if (result == null || !result.CmdSucceed) return false;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(result.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("新国庆阅兵: 士气达到{0}, 获得{1}", parade.neednum, reward.ToString()));
            return true;
        }
    }
}
