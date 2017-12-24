using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;
using com.lover.astd.common.model.misc;
using System.Collections.Specialized;
using LuaInterface;
using com.lover.astd.common.model.activity;

namespace com.lover.astd.common.activity
{
    public class ActivitySnowTrading : MgrBase
    {
        public ActivitySnowTrading(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.HotPink;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }
        /// <summary>
        /// 雪地通商信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="buyroundcostlimit">购买次数费用上限</param>
        /// <param name="goldavailable">可用金币</param>
        /// <param name="isreinforce">加固雪橇?</param>
        /// <param name="reinforcecostlimit">加固雪橇费用上限</param>
        /// <returns>0:成功 1:null 2:出错 3:完成 10:失败</returns>
        public int getSnowTradingInfo(ProtocolMgr protocol, ILogger logger, User user, int buyroundcostlimit, int goldavailable, bool isreinforce, int reinforcecostlimit, int choose)
        {
            string url = "/root/snowTrading!getSnowTradingInfo.action";
            ServerResult result = protocol.getXml(url, "雪地通商信息");
            if (result == null)
            {
                return 1;
            }
            else if (!result.CmdSucceed)
            {
                return 10;
            }

            int buyroundcost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/buyroundcost"));//购买次数费用
            int reinforcecost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/reinforcecost"));//加固雪橇费用
            int hastime = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/hastime"));//剩余次数
            int buyroundnum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/buyroundnum"));//购买次数
            int castnum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/castnum"));//运送宝箱
            int reinforce = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/reinforce"));//加固雪橇 0:未加固 1:已加固
            int casttype = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/casttype"));//宝箱 1:木质 2:白银 3:黄金
            List<CaseState> casestate = XmlHelper.GetClassList<CaseState>(result.CmdResult.SelectNodes("/results/casestate"));//运送宝箱状态
            foreach (CaseState item in casestate)
            {
                if (!getCaseNumReward(protocol, logger, user, item.id))
                {
                    return 2;
                }
            }
            if (hastime <= 0)
            {
                if (buyroundcost <= buyroundcostlimit && buyroundcost <= goldavailable)
                {
                    if (!buyRound(protocol, logger, user, buyroundcost))
                    {
                        return 2;
                    }
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                if (isreinforce && reinforcecost <= reinforcecostlimit && reinforcecost <= goldavailable)
                {
                    if (!reinforceSled(protocol, logger, user, reinforcecost))
                    {
                        return 2;
                    }
                }
                if (!transport(protocol, logger, user, choose))
                {
                    return 2;
                }
            }

            return 0;
        }
        /// <summary>
        /// 加固雪橇
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="reinforcecost">加固雪橇费用</param>
        /// <returns></returns>
        public bool reinforceSled(ProtocolMgr protocol, ILogger logger, User user, int reinforcecost)
        {
            string url = "/root/snowTrading!reinforceSled.action";
            ServerResult result = protocol.getXml(url, "加固雪橇");
            if (result == null)
            {
                return false;
            }
            else if (!result.CmdSucceed)
            {
                return false;
            }
            logInfo(logger, string.Format("花费{0}金币，加固雪橇", reinforcecost));
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="buyroundcost">购买次数费用</param>
        /// <returns></returns>
        public bool buyRound(ProtocolMgr protocol, ILogger logger, User user, int buyroundcost)
        {
            string url = "/root/snowTrading!buyRound.action";
            ServerResult result = protocol.getXml(url, "购买次数");
            if (result == null)
            {
                return false;
            }
            else if (!result.CmdSucceed)
            {
                return false;
            }
            logInfo(logger, string.Format("花费{0}金币，购买次数", buyroundcost));
            return true;
        }
        /// <summary>
        /// 雪地通商
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="choose">1:镔铁 2:点券</param>
        /// <returns></returns>
        public bool transport(ProtocolMgr protocol, ILogger logger, User user, int choose)
        {
            string url = "/root/snowTrading!transport.action";
            string data = string.Format("choose={0}", choose);
            ServerResult result = protocol.postXml(url, data, "雪地通商");
            if (result == null)
            {
                return false;
            }
            else if (!result.CmdSucceed)
            {
                return false;
            }

            int storneloss = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/stonestate/storneloss"));//宝箱掉落个数
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(result.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("雪地通商，丢失{0}宝箱，获得{1}", storneloss, reward.ToString()));
            return true;
        }
        /// <summary>
        /// 雪地通商奖励
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="cases">宝箱索引</param>
        /// <returns></returns>
        public bool getCaseNumReward(ProtocolMgr protocol, ILogger logger, User user, int cases)
        {
            string url = "/root/snowTrading!getCaseNumReward.action";
            string data = string.Format("cases={0}", cases);
            ServerResult result = protocol.postXml(url, data, "雪地通商奖励");
            if (result == null)
            {
                return false;
            }
            else if (!result.CmdSucceed)
            {
                return false;
            }

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(result.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("雪地通商奖励，获得{0}", reward.ToString()));
            return true;
        }
    }
}
