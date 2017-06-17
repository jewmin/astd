using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;
using System.Xml;
using com.lover.astd.common.model.misc;

namespace com.lover.astd.common.activity
{
    public class ArrestEvent : MgrBase
    {
        /// <summary>
        /// 可领取免费抓捕令
        /// </summary>
        private int cangettoken_;
        /// <summary>
        /// 免费抓捕令
        /// </summary>
        private int cangetarrestnum_;
        /// <summary>
        /// 当前拥有抓捕令
        /// </summary>
        private int arresttokennum_;
        /// <summary>
        /// 购买抓捕令花费金币
        /// </summary>
        private int arresttokencostgold_;
        /// <summary>
        /// 待审问俘虏
        /// </summary>
        private int slaves_;
        /// <summary>
        /// 已审问俘虏
        /// </summary>
        private int shenslaves_;
        /// <summary>
        /// 天下神捕需要的俘虏
        /// </summary>
        private int buffneedshenslaves_;
        /// <summary>
        /// 免费鞭子
        /// </summary>
        private int freehighshen_;
        /// <summary>
        /// 使用鞭子花费金币
        /// </summary>
        private int hishengold_;
        /// <summary>
        /// 可享用端午密粽
        /// </summary>
        private int ricedumpling_;
        /// <summary>
        /// 享用端午密粽花费金币
        /// </summary>
        private int ricedumplingcostgold_;
        /// <summary>
        /// 天下神捕buff抓捕劳工宝石加成
        /// </summary>
        private int buffadd_;
        /// <summary>
        /// 天下神捕buff状态
        /// </summary>
        private int buffstate_;
        /// <summary>
        /// 当前活动天数
        /// </summary>
        private int day_;

        public ArrestEvent(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Chocolate;
            this._tmrMgr = tmrMgr;
            this._factory = factory;

            cangettoken_ = cangetarrestnum_ = arresttokennum_ = 0;
            arresttokencostgold_ = hishengold_ = ricedumplingcostgold_ = 10000;
            slaves_ = shenslaves_ = buffneedshenslaves_ = freehighshen_ = 0;
            ricedumpling_ = buffadd_ = buffstate_ = day_ = 0;
        }

        public long execute(ProtocolMgr protocol, ILogger logger, User user, int goldavailable, int hishengold, int ricedumplingcostgold, int arresttokencostgold)
        {
            bool result = getArrestEventInfo(protocol, logger);
            if (!result) return next_hour();

            logInfo(logger, string.Format("端午活动, 天下神捕{0}/{1}", shenslaves_, buffneedshenslaves_));

            if (cangettoken_ == 1)
            {
                result = recvArrestToken(protocol, logger);
                if (!result) return next_hour();
            }

            if (slaves_ > 0)
            {
                int highLv = 0;
                if ((freehighshen_ > 0) || (hishengold_ <= hishengold && hishengold_ <= goldavailable)) highLv = 1;
                result = shenSlaves(protocol, logger, user, highLv);
                if (!result) return next_hour();
                if (highLv == 1) goldavailable -= hishengold_;
            }

            if (ricedumpling_ > 0)
            {
                result = eatRiceDumpling(protocol, logger);
                if (!result) return next_hour();
            }

            bool buy = false;

            if (ricedumpling_ <= 0 && ricedumplingcostgold_ <= ricedumplingcostgold && ricedumplingcostgold_ <= goldavailable)
            {
                result = buyRiceDumpling(protocol, logger, user);
                if (!result) return next_hour();
                goldavailable -= ricedumplingcostgold_;
                buy = true;
            }

            if (arresttokennum_ <= 0 && arresttokencostgold_ <= arresttokencostgold && arresttokencostgold_ <= goldavailable)
            {
                result = buyArrestToken(protocol, logger, user);
                if (!result) return next_hour();
                goldavailable -= arresttokencostgold_;
                buy = true;
            }

            if (buy || ricedumpling_ > 0 || slaves_ > 0) return immediate();

            return next_hour();
        }

        public bool getArrestEventInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!getArrestEventInfo.action";
            ServerResult xml = protocol.getXml(url, "获取端午活动信息");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            if (node == null || !node.HasChildNodes) return false;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "cangettoken") cangettoken_ = int.Parse(node1.InnerText);
                else if (node1.Name == "cangetarrestnum") cangetarrestnum_ = int.Parse(node1.InnerText);
                else if (node1.Name == "arresttokennum") arresttokennum_ = int.Parse(node1.InnerText);
                else if (node1.Name == "slaves") slaves_ = int.Parse(node1.InnerText);
                else if (node1.Name == "freehighshen") freehighshen_ = int.Parse(node1.InnerText);
                else if (node1.Name == "hishengold") hishengold_ = int.Parse(node1.InnerText);
                else if (node1.Name == "shenslaves") shenslaves_ = int.Parse(node1.InnerText);
                else if (node1.Name == "buffneedshenslaves") buffneedshenslaves_ = int.Parse(node1.InnerText);
                else if (node1.Name == "ricedumpling") ricedumpling_ = int.Parse(node1.InnerText);
                else if (node1.Name == "ricedumplingcostgold") ricedumplingcostgold_ = int.Parse(node1.InnerText);
                else if (node1.Name == "arresttokencostgold") arresttokencostgold_ = int.Parse(node1.InnerText);
                else if (node1.Name == "buffadd") buffadd_ = int.Parse(node1.InnerText);
                else if (node1.Name == "buffstate") buffstate_ = int.Parse(node1.InnerText);
                else if (node1.Name == "day") day_ = int.Parse(node1.InnerText);
            }

            return true;
        }

        public bool recvArrestToken(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!recvArrestToken.action";
            ServerResult xml = protocol.getXml(url, "领取免费抓捕令");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            if (node == null || !node.HasChildNodes) return false;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "cangettoken") cangettoken_ = int.Parse(node1.InnerText);
                else if (node1.Name == "arresttokennum") arresttokennum_ = int.Parse(node1.InnerText);
            }

            logInfo(logger, string.Format("领取免费抓捕令, 获得抓捕令+{0}", arresttokennum_));

            return true;
        }

        public bool shenSlaves(ProtocolMgr protocol, ILogger logger, User user, int highLv)
        {
            string url = "/root/event!shenSlaves.action";
            string data = string.Format("highLv={0}", highLv);
            ServerResult xml = protocol.postXml(url, data, "审问俘虏");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            if (node == null || !node.HasChildNodes) return false;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "slaves") slaves_ = int.Parse(node1.InnerText);
                else if (node1.Name == "shenslaves") shenslaves_ = int.Parse(node1.InnerText);
                else if (node1.Name == "freehighshen") freehighshen_ = int.Parse(node1.InnerText);
                else if (node1.Name == "hishengold") hishengold_ = int.Parse(node1.InnerText);
                if (node1.Name == "ricedumpling") ricedumpling_ = int.Parse(node1.InnerText);
            }

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));

            logInfo(logger, string.Format("{0}审问俘虏, 待审问俘虏+{1}, 已审问俘虏+{2}, 需要审问俘虏+{3}, 获得{4}", (highLv == 1 ? "使用鞭子" : ""), slaves_, shenslaves_, buffneedshenslaves_, reward.ToString()));
            logInfo(logger, string.Format("当前系统金币+{0}, 充值金币+{1}", user._sysgold, user._usergold), Color.Goldenrod);

            return true;
        }

        public bool eatRiceDumpling(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!eatRiceDumpling.action";
            ServerResult xml = protocol.getXml(url, "享用端午密粽");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            if (node == null || !node.HasChildNodes) return false;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "ricedumpling") ricedumpling_ = int.Parse(node1.InnerText);
            }

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));

            logInfo(logger, string.Format("享用端午密粽, 剩余端午密粽+{0}, 获得{1}", ricedumpling_, reward.ToString()));

            return true;
        }

        public bool buyRiceDumpling(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/event!buyRiceDumpling.action";
            ServerResult xml = protocol.getXml(url, "购买端午密粽");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            if (node == null || !node.HasChildNodes) return false;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "ricedumpling") ricedumpling_ = int.Parse(node1.InnerText);
                else if (node1.Name == "ricedumplingcostgold") ricedumplingcostgold_ = int.Parse(node1.InnerText);
            }

            logInfo(logger, string.Format("购买端午密粽, 剩余端午密粽+{0}, 下次购买需要花费金币+{1}", ricedumpling_, ricedumplingcostgold_));
            logInfo(logger, string.Format("当前系统金币+{0}, 充值金币+{1}", user._sysgold, user._usergold), Color.Goldenrod);

            return true;
        }

        public bool buyArrestToken(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/event!buyArrestToken.action";
            ServerResult xml = protocol.getXml(url, "购买抓捕令");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            if (node == null || !node.HasChildNodes) return false;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name == "arresttokencostgold") arresttokencostgold_ = int.Parse(node1.InnerText);
            }

            logInfo(logger, string.Format("购买抓捕令, 下次购买需要花费金币+{0}", arresttokencostgold_));
            logInfo(logger, string.Format("当前系统金币+{0}, 充值金币+{1}", user._sysgold, user._usergold), Color.Goldenrod);

            return true;
        }
    }
}
