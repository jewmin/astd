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
    public class RingEvent : MgrBase
    {
        class RingEventInfo : BaseObject
        {
            public int randomtimes { get; set; }
            public int randomcost { get; set; }
            public int firsttimes { get; set; }
            public int firstcost { get; set; }
            public int secondtimes { get; set; }
            public int secondcost { get; set; }
            public int thirdtimes { get; set; }
            public int thirdcost { get; set; }
            public int progress { get; set; }
            public int maxprogress { get; set; }
            public int reelstatus { get; set; }
            public string words { get; set; }
            public int reelnum { get; set; }
            public RingEventInfo()
            {
                randomtimes = 0;
                randomcost = 0;
                firsttimes = 0;
                firstcost = 0;
                secondtimes = 0;
                secondcost = 0;
                thirdtimes = 0;
                thirdcost = 0;
                progress = 0;
                maxprogress = 0;
                reelstatus = 0;
                words = "";
                reelnum = 0;
            }
        }

        class RingState : BaseObject
        {
            public int id { get; set; }
            public int state { get; set; }
            public int need { get; set; }
            public RingState()
            {
                id = 0;
                state = 0;
                need = 0;
            }
        }

        readonly string[] choose_name = { "随机", "福", "禄", "寿" };

        public RingEvent(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Chocolate;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }

        /// <summary>
        /// 新年敲钟活动信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="random_ring_cost">随机敲钟花费金币上限</param>
        /// <param name="other_ring_cost">固定敲钟花费金币上限</param>
        /// <param name="progress_choose">进度奖励选项</param>
        /// <returns>10:错误 0:成功 2:已完成</returns>
        public int getRingEventInfo(ProtocolMgr protocol, ILogger logger, User user, int random_ring_cost, int other_ring_cost, int progress_choose)
        {
            string url = "/root/ringEvent!getRingEventInfo.action";
            ServerResult xml = protocol.getXml(url, "新年敲钟 - 活动面板");
            if (xml == null || !xml.CmdSucceed) return 10;

            List<RingState> state_list = XmlHelper.GetClassList<RingState>(xml.CmdResult.SelectNodes("/results/ringstate"));
            foreach (RingState state in state_list)
            {
                if (state.state == 1)
                {
                    getProgressReward(protocol, logger, user, state.id, progress_choose);
                }
            }

            RingEventInfo info = XmlHelper.GetClass<RingEventInfo>(xml.CmdResult.SelectSingleNode("/results"));
            if (info.reelstatus == 1)
            {
                openReel(protocol, logger, user);
            }
            else if (info.reelstatus == 2)
            {
                try
                {
                    List<int> need_bellids = XmlHelper.GetStringSplit(xml.CmdResult.SelectSingleNode("/results/need"));
                    List<int> ring_costs = new List<int>();
                    ring_costs.Add(random_ring_cost);
                    ring_costs.Add(info.firstcost);
                    ring_costs.Add(info.secondcost);
                    ring_costs.Add(info.thirdcost);
                    int bellid = need_bellids[info.reelnum];
                    int cost = ring_costs[bellid];
                    if (cost <= other_ring_cost)
                    {
                        if (ring(protocol, logger, user, bellid, cost) == 0) return 0;
                        return 10;
                    }
                    else if (info.reelnum > 0 && info.randomcost <= random_ring_cost)
                    {
                        if (ring(protocol, logger, user, 0, info.randomcost) == 0) return 0;
                        return 10;
                    }
                    else
                    {
                        giveUpReel(protocol, logger, user);
                    }
                }
                catch (Exception ex)
                {
                    logInfo(logger, string.Format("新年敲钟 - 活动面板: {0}", ex.Message));
                }
            }
            else
            {
                if (info.randomcost <= random_ring_cost)
                {
                    if (ring(protocol, logger, user, 0, info.randomcost) == 0) return 0;
                    return 10;
                }
                else
                {
                    Dictionary<int, int> ring_costs = new Dictionary<int, int>();
                    ring_costs.Add(1, info.firstcost);
                    ring_costs.Add(2, info.secondcost);
                    ring_costs.Add(3, info.thirdcost);
                    XmlHelper.DictionarySort(ring_costs);
                    foreach (KeyValuePair<int, int> kvp in ring_costs)
                    {
                        if (kvp.Value <= other_ring_cost)
                        {
                            if (ring(protocol, logger, user, kvp.Key, kvp.Value) == 0) return 0;
                            return 10;
                        }
                    }
                }

                return 2;
            }

            return 0;
        }

        /// <summary>
        /// 敲钟
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="bellId">0随机 1福 2禄 3寿</param>
        /// <param name="cost">花费金币</param>
        public int ring(ProtocolMgr protocol, ILogger logger, User user, int bellId, int cost)
        {
            if (cost > user.Gold) return 10;

            string url = "/root/ringEvent!ring.action";
            string data = string.Format("bellId={0}", bellId);
            ServerResult xml = protocol.postXml(url, data, "新年敲钟 - 敲钟");
            if (xml == null || !xml.CmdSucceed) return 10;

            int choose = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/choose"), 0);
            int ringappear = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/ringappear"), 0);
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            RewardInfo bigreward = new RewardInfo();
            bigreward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/bigreward/rewardinfo"));
            
            StringBuilder sb = new StringBuilder("新年敲钟 - ");
            
            if (cost == 0) sb.Append("免费");
            else sb.AppendFormat("花费{0}金币", cost);

            sb.AppendFormat("敲钟【{0}】", choose_name[choose]);

            sb.AppendFormat("获得{0}", reward.ToString());

            if (ringappear > 0) sb.Append(", 出现对联");

            if (bigreward.getRewardCount() > 0) sb.AppendFormat(", 大奖获得{0}", bigreward.ToString());

            logInfo(logger, sb.ToString());

            return 0;
        }

        /// <summary>
        /// 打开对联
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public void openReel(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/ringEvent!openReel.action";
            ServerResult xml = protocol.getXml(url, "新年敲钟 - 打开对联");
            if (xml == null || !xml.CmdSucceed) return;

            try
            {
                string words = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/words"));
                List<int> need_bellids = XmlHelper.GetStringSplit(xml.CmdResult.SelectSingleNode("/results/need"));
                int reelnum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/reelnum"), 0);
                StringBuilder sb = new StringBuilder();
                foreach (int bellid in need_bellids)
                {
                    sb.AppendFormat(", {0}", choose_name[bellid]);
                }
                logInfo(logger, string.Format("新年敲钟 - 打开对联, {0}, {1}", words, sb.ToString(2, sb.Length - 2)));
            }
            catch (Exception ex)
            {
                logInfo(logger, string.Format("新年敲钟 - 打开对联: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 领取进度奖励
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="rewardId"></param>
        /// <param name="type">1:大将令 2:镔铁</param>
        public void getProgressReward(ProtocolMgr protocol, ILogger logger, User user, int rewardId, int type)
        {
            string url = "/root/ringEvent!getProgressReward.action";
            string data = string.Format("rewardId={0}&type={1}", rewardId, type);
            ServerResult xml = protocol.postXml(url, data, "新年敲钟 - 领取进度奖励");
            if (xml == null || !xml.CmdSucceed) return;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("新年敲钟 - 领取进度奖励, 获得{0}", reward.ToString()));
        }

        /// <summary>
        /// 放弃对联
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public void giveUpReel(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/ringEvent!giveUpReel.action";
            ServerResult xml = protocol.getXml(url, "新年敲钟 - 放弃对联");
            if (xml == null || !xml.CmdSucceed) return;

            logInfo(logger, "新年敲钟 - 放弃对联");
        }
    }
}
