using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;
using System.Collections.Specialized;
using LuaInterface;
using com.lover.astd.common.model.misc;
using System.Xml;

namespace com.lover.astd.common.activity
{
    public class streamarrows : XmlObject, IComparable
    {
        /// <summary>
        /// 0 下游 1 中游 2 上游
        /// </summary>
        public int id;
        /// <summary>
        /// 箭矢数量
        /// </summary>
        public int arrows;
        /// <summary>
        /// 排序 箭矢数量从多到少
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            streamarrows arrows = obj as streamarrows;
            if (this.arrows > arrows.arrows) return -1;
            else if (this.arrows < arrows.arrows) return 1;
            else return 0;
        }

        public override void Parse(System.Xml.XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "id") id = int.Parse(item.InnerText);
                else if (item.Name == "arrows") arrows = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return true;
        }
    }

    public class exchangereward : XmlObject, IComparable
    {
        private string[] rewardname = { "镔铁", "点卷", "宝物", "宝石" };
        private int[] priority = { 4, 3, 2, 1 };
        /// <summary>
        /// 邀功类型
        /// </summary>
        public int rewardtype;
        /// <summary>
        /// 邀功次数
        /// </summary>
        public int buynum;
        /// <summary>
        /// 消耗军功
        /// </summary>
        public int cost;
        /// <summary>
        /// 邀功奖励名称
        /// </summary>
        public string RewardName
        {
            get { return rewardname[rewardtype]; }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return priority[rewardtype]; }
        }
        /// <summary>
        /// 排序 越小越优先
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            exchangereward reward = obj as exchangereward;
            if (this.Priority < reward.Priority) return -1;
            else if (this.Priority > reward.Priority) return 1;
            else return 0;
        }

        public override void Parse(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "rewardtype") rewardtype = int.Parse(item.InnerText);
                else if (item.Name == "buynum") buynum = int.Parse(item.InnerText);
                else if (item.Name == "cost") cost = int.Parse(item.InnerText);
            }
        }

        public override bool CanAdd()
        {
            return true;
        }
    }

    public class BorrowingArrowsEvent : MgrBase
    {
        /// <summary>
        /// 船只承重
        /// </summary>
        private int arrowsboat = 0;
        /// <summary>
        /// 剩余军功
        /// </summary>
        private int arrowsleft = 0;
        /// <summary>
        /// 总军功
        /// </summary>
        private int arrowstotal = 0;
        /// <summary>
        /// 返回军功
        /// </summary>
        private int arrowsdeliver = 0;
        /// <summary>
        /// 增加的箭矢数量
        /// </summary>
        private int arrowsstream = 0;
        /// <summary>
        /// 船只承重上限
        /// </summary>
        private int boatcapacity = 0;
        /// <summary>
        /// 发船免费次数
        /// </summary>
        private int boatnum = 0;
        /// <summary>
        /// 发船消耗金币
        /// </summary>
        private int buyboatcost = 0;
        /// <summary>
        /// 神机妙算 0 没有 1 有
        /// </summary>
        private int calculatestreamstate = 0;
        /// <summary>
        /// 神机妙算消耗金币
        /// </summary>
        private int calculatestreamcost = 0;
        /// <summary>
        /// 当前状态 -2 未发船 -1 已发船 0 选择下游 1 选择中游 2 选择上游
        /// </summary>
        private int currentstream = 0;
        /// <summary>
        /// 上、中、下游
        /// </summary>
        private List<streamarrows> streamarrows_list = new List<streamarrows>();
        /// <summary>
        /// 剩余时间
        /// </summary>
        private int eventresttime;
        /// <summary>
        /// 上交邀功
        /// </summary>
        private List<exchangereward> exchangereward_list = new List<exchangereward>();
        /// <summary>
        /// 钥匙数量
        /// </summary>
        private int unlocknum;
        /// <summary>
        /// 钥匙领取状态 1 已领取 0 可领取 -1 未领取
        /// </summary>
        private List<int> stagestatus_list = new List<int>();

        public BorrowingArrowsEvent(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Chocolate;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <borrowingarrowseventinfo>
         *      <arrowsboat>0</arrowsboat>
         *      <arrowsleft>25000</arrowsleft>
         *      <arrowstotal>1025000</arrowstotal>
         *      <stagenum>200000,1000000,3000000,10000000</stagenum>
         *      <boatcapacity>200000</boatcapacity>
         *      <boatnum>2</boatnum>
         *      <buyboatcost>5</buyboatcost>
         *      <calculatestreamstate>0</calculatestreamstate>
         *      <calculatestreamcost>20</calculatestreamcost>
         *      <currentstream>-2</currentstream>
         *      <streamarrows>
         *          <id>0</id>
         *          <arrows>0</arrows>
         *      </streamarrows>
         *      <streamarrows>
         *          <id>1</id>
         *          <arrows>0</arrows>
         *      </streamarrows>
         *      <streamarrows>
         *          <id>2</id>
         *          <arrows>0</arrows>
         *      </streamarrows>
         *      <eventresttime>240642596</eventresttime>
         *      <exchangereward>
         *          <rewardtype>0</rewardtype>
         *          <buynum>-1</buynum>
         *          <cost>50000</cost>
         *      </exchangereward>
         *      <exchangereward>
         *          <rewardtype>1</rewardtype>
         *          <buynum>-1</buynum>
         *          <cost>50000</cost>
         *      </exchangereward>
         *      <exchangereward>
         *          <rewardtype>2</rewardtype>
         *          <buynum>-1</buynum>
         *          <cost>50000</cost>
         *      </exchangereward>
         *      <exchangereward>
         *          <rewardtype>3</rewardtype>
         *          <buynum>-1</buynum>
         *          <cost>50000</cost>
         *      </exchangereward>
         *      <unlocknum>0</unlocknum>
         *      <stagestatus>-1,-1,-1,-1</stagestatus>
         *  </borrowingarrowseventinfo>
         * </results>
         */
        public int getPlayerBorrowingArrowsEventInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/borrowingArrowsEvent!getPlayerBorrowingArrowsEventInfo.action";
            ServerResult result = protocol.getXml(url, "草船借箭");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //this.arrowsboat = lua.GetIntValue("results.borrowingarrowseventinfo.arrowsboat");
            //this.arrowsleft = lua.GetIntValue("results.borrowingarrowseventinfo.arrowsleft");
            //this.arrowstotal = lua.GetIntValue("results.borrowingarrowseventinfo.arrowstotal");
            //this.boatcapacity = lua.GetIntValue("results.borrowingarrowseventinfo.boatcapacity");
            //this.boatnum = lua.GetIntValue("results.borrowingarrowseventinfo.boatnum");
            //this.buyboatcost = lua.GetIntValue("results.borrowingarrowseventinfo.buyboatcost");
            //this.calculatestreamstate = lua.GetIntValue("results.borrowingarrowseventinfo.calculatestreamstate");
            //this.calculatestreamcost = lua.GetIntValue("results.borrowingarrowseventinfo.calculatestreamcost");
            //this.currentstream = lua.GetIntValue("results.borrowingarrowseventinfo.currentstream");
            //this.eventresttime = lua.GetIntValue("results.borrowingarrowseventinfo.eventresttime");
            //this.unlocknum = lua.GetIntValue("results.borrowingarrowseventinfo.unlocknum");
            arrowsboat = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/arrowsboat"));
            arrowsleft = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/arrowsleft"));
            arrowstotal = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/arrowstotal"));
            boatcapacity = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/boatcapacity"));
            boatnum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/boatnum"));
            buyboatcost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/buyboatcost"));
            calculatestreamstate = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/calculatestreamstate"));
            calculatestreamcost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/calculatestreamcost"));
            currentstream = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/currentstream"));
            eventresttime = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/eventresttime"));
            unlocknum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/unlocknum"));
            //ListDictionary streamarrows_table = lua.GetListValue("results.borrowingarrowseventinfo.streamarrows");
            //this.streamarrows_list.Clear();
            //foreach (LuaTable table in streamarrows_table.Values)
            //{
            //    streamarrows item = new streamarrows();
            //    item.id = AstdLuaObject.GetIntValue(table, "id");
            //    item.arrows = AstdLuaObject.GetIntValue(table, "arrows");
            //    this.streamarrows_list.Add(item);
            //}
            streamarrows_list = XmlHelper.GetClassList<streamarrows>(result.CmdResult.SelectNodes("/results/borrowingarrowseventinfo/streamarrows"));
            //ListDictionary exchangereward_table = lua.GetListValue("results.borrowingarrowseventinfo.exchangereward");
            //this.exchangereward_list.Clear();
            //foreach (LuaTable table in exchangereward_table.Values)
            //{
            //    exchangereward item = new exchangereward();
            //    item.rewardtype = AstdLuaObject.GetIntValue(table, "rewardtype");
            //    item.buynum = AstdLuaObject.GetIntValue(table, "buynum");
            //    item.cost = AstdLuaObject.GetIntValue(table, "cost");
            //    this.exchangereward_list.Add(item);
            //}
            exchangereward_list = XmlHelper.GetClassList<exchangereward>(result.CmdResult.SelectNodes("/results/borrowingarrowseventinfo/exchangereward"));
            //string stagestatus = lua.GetStringValue("results.borrowingarrowseventinfo.stagestatus");
            string stagestatus = XmlHelper.GetString(result.CmdResult.SelectSingleNode("/results/borrowingarrowseventinfo/stagestatus"));
            string[] tmp = stagestatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.stagestatus_list.Clear();
            foreach (string item in tmp)
            {
                this.stagestatus_list.Add(int.Parse(item));
            }
            return 0;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <buyboat>
         *      <currentstream>-1</currentstream>
         *  </buyboat>
         * </results>
         */
        public int setSail(ProtocolMgr protocol, ILogger logger, User user, ref int goldavailable)
        {
            string url = "/root/borrowingArrowsEvent!setSail.action";
            ServerResult result = protocol.getXml(url, "发船");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //this.currentstream = lua.GetIntValue("results.buyboat.currentstream");
            currentstream = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/buyboat/currentstream"));
            if (this.boatnum > 0)
            {
                logInfo(logger, string.Format("免费发船"));
            }
            else
            {
                logInfo(logger, string.Format("消耗{0}金币发船", this.buyboatcost));
                goldavailable -= this.buyboatcost;
            }
            return 0;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <borrowingarrows>
         *      <borrowingresult>1</borrowingresult>    <borrowingresult>0</borrowingresult>
         *                                              <boatnum>1</boatnum>
         *                                              <boatcapacity>100000</boatcapacity>
         *                                              <arrowsleft>105000</arrowsleft>
         *                                              <arrowstotal>105000</arrowstotal>
         *                                              <arrowsdeliver>105000</arrowsdeliver>
         *                                              <stagestatus>-1,-1,-1,-1,</stagestatus>
         *      <arrowsboat>50000</arrowsboat>          <arrowsboat>0</arrowsboat>
         *      <arrowsstream>50000</arrowsstream>      <arrowsstream>20000</arrowsstream>
         *      <currentstream>0</currentstream>        <currentstream>-2</currentstream>
         *      <streamarrows>
         *          <id>0</id>
         *          <arrows>50000</arrows>
         *      </streamarrows>
         *      <streamarrows>
         *          <id>1</id>
         *          <arrows>10000</arrows>
         *      </streamarrows>
         *      <streamarrows>
         *          <id>2</id>
         *          <arrows>20000</arrows>
         *      </streamarrows>
         *  </borrowingarrows>
         * </results>
         */
        public int choiceStream(ProtocolMgr protocol, ILogger logger, User user, int streamId)
        {
            string[] choice = { "下", "中", "上" };
            string url = "/root/borrowingArrowsEvent!choiceStream.action";
            string data = string.Format("streamId={0}", streamId);
            string desc = string.Format("选择{0}游", choice[streamId]);
            ServerResult result = protocol.postXml(url, data, desc);
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //int borrowingresult = lua.GetIntValue("results.borrowingarrows.borrowingresult");
            int borrowingresult = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/borrowingresult"));
            if (borrowingresult == 0)
            {
                //this.boatnum = lua.GetIntValue("results.borrowingarrows.boatnum");
                //this.boatcapacity = lua.GetIntValue("results.borrowingarrows.boatcapacity");
                //this.arrowsleft = lua.GetIntValue("results.borrowingarrows.arrowsleft");
                //this.arrowstotal = lua.GetIntValue("results.borrowingarrows.arrowstotal");
                //this.arrowsdeliver = lua.GetIntValue("results.borrowingarrows.arrowsdeliver");
                //string stagestatus = lua.GetStringValue("results.borrowingarrows.stagestatus");
                boatnum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/boatnum"));
                boatcapacity = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/boatcapacity"));
                arrowsleft = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/arrowsleft"));
                arrowstotal = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/arrowstotal"));
                arrowsdeliver = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/arrowsdeliver"));
                string stagestatus = XmlHelper.GetString(result.CmdResult.SelectSingleNode("/results/borrowingarrows/stagestatus"));
                string[] tmp = stagestatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                this.stagestatus_list.Clear();
                foreach (string item in tmp)
                {
                    this.stagestatus_list.Add(int.Parse(item));
                }
            }
            //this.arrowsboat = lua.GetIntValue("results.borrowingarrows.arrowsboat");
            //this.arrowsstream = lua.GetIntValue("results.borrowingarrows.arrowsstream");
            //this.currentstream = lua.GetIntValue("results.borrowingarrows.currentstream");
            arrowsboat = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/arrowsboat"));
            arrowsstream = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/arrowsstream"));
            currentstream = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/borrowingarrows/currentstream"));
            //ListDictionary streamarrows_table = lua.GetListValue("results.borrowingarrows.streamarrows");
            //this.streamarrows_list.Clear();
            //foreach (LuaTable table in streamarrows_table.Values)
            //{
            //    streamarrows item = new streamarrows();
            //    item.id = AstdLuaObject.GetIntValue(table, "id");
            //    item.arrows = AstdLuaObject.GetIntValue(table, "arrows");
            //    this.streamarrows_list.Add(item);
            //}
            streamarrows_list = XmlHelper.GetClassList<streamarrows>(result.CmdResult.SelectNodes("/results/borrowingarrows/streamarrows"));
            if (borrowingresult == 1)
            {
                logInfo(logger, string.Format("{0}，箭矢增加{1}，当前承重{2}/{3}", desc, this.arrowsstream, this.arrowsboat, this.boatcapacity));
                return 0;
            }
            else
            {
                logInfo(logger, string.Format("{0}，超重，承重变为{1}，返回军功{2}", desc, this.boatcapacity, this.arrowsdeliver));
                return -1;
            }
        }

        /*
         * <results>
         *  <state>1</state>
         *  <deliverarrows>
         *      <arrowsleft>250000</arrowsleft>
         *      <arrowstotal>250000</arrowstotal>
         *      <arrowsdeliver>90000</arrowsdeliver>
         *      <boatnum>0</boatnum>
         *      <arrowsboat>0</arrowsboat>
         *      <boatcapacity>150000</boatcapacity>
         *      <currentstream>-2</currentstream>
         *      <stagestatus>0,-1,-1,-1,</stagestatus>
         *  </deliverarrows>
         * </results>
         */
        public int deliverArrows(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/borrowingArrowsEvent!deliverArrows.action";
            ServerResult result = protocol.getXml(url, "返航");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //this.arrowsleft = lua.GetIntValue("results.deliverarrows.arrowsleft");
            //this.arrowstotal = lua.GetIntValue("results.deliverarrows.arrowstotal");
            //this.arrowsdeliver = lua.GetIntValue("results.deliverarrows.arrowsdeliver");
            //this.boatnum = lua.GetIntValue("results.deliverarrows.boatnum");
            //this.arrowsboat = lua.GetIntValue("results.deliverarrows.arrowsboat");
            //this.boatcapacity = lua.GetIntValue("results.deliverarrows.boatcapacity");
            //this.currentstream = lua.GetIntValue("results.deliverarrows.currentstream");
            //string stagestatus = lua.GetStringValue("results.deliverarrows.stagestatus");
            arrowsleft = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/arrowsleft"));
            arrowstotal = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/arrowstotal"));
            arrowsdeliver = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/arrowsdeliver"));
            boatnum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/boatnum"));
            arrowsboat = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/arrowsboat"));
            boatcapacity = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/boatcapacity"));
            currentstream = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/deliverarrows/currentstream"));
            string stagestatus = XmlHelper.GetString(result.CmdResult.SelectSingleNode("/results/deliverarrows/stagestatus"));
            string[] tmp = stagestatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.stagestatus_list.Clear();
            foreach (string item in tmp)
            {
                this.stagestatus_list.Add(int.Parse(item));
            }
            logInfo(logger, string.Format("返航，承重变为{0}，返回军功{1}", this.boatcapacity, this.arrowsdeliver));
            return 0;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <getkey>
         *      <stagestatus>1,-1,-1,-1,</stagestatus>
         *      <unlocknum>1</unlocknum>
         *  </getkey>
         * </results>
         */
        public int getKey(ProtocolMgr protocol, ILogger logger, User user, int keyId)
        {
            string url = "/root/borrowingArrowsEvent!getKey.action";
            string data = string.Format("keyId={0}", keyId);
            ServerResult result = protocol.postXml(url, data, "领取钥匙");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //this.unlocknum = lua.GetIntValue("results.getkey.unlocknum");
            //string stagestatus = lua.GetStringValue("results.getkey.stagestatus");
            unlocknum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/getkey/unlocknum"));
            string stagestatus = XmlHelper.GetString(result.CmdResult.SelectSingleNode("/results/getkey/stagestatus"));
            string[] tmp = stagestatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.stagestatus_list.Clear();
            foreach (string item in tmp)
            {
                this.stagestatus_list.Add(int.Parse(item));
            }
            logInfo(logger, string.Format("领取钥匙，钥匙数量={0}", this.unlocknum));
            return 0;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <unlockreward>
         *      <unlocknum>0</unlocknum>
         *      <exchangereward>
         *          <rewardtype>3</rewardtype>
         *          <buynum>0</buynum>
         *          <cost>50000</cost>
         *      </exchangereward>
         *  </unlockreward>
         * </results>
         */
        public int unlockReward(ProtocolMgr protocol, ILogger logger, User user, int rewardType)
        {
            string url = "/root/borrowingArrowsEvent!unlockReward.action";
            string data = string.Format("rewardType={0}", rewardType);
            ServerResult result = protocol.postXml(url, data, "打开宝箱");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //this.unlocknum = lua.GetIntValue("results.unlockreward.unlocknum");
            //rewardType = lua.GetIntValue("results.unlockreward.exchangereward.rewardtype");
            unlocknum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/unlockreward/unlocknum"));
            rewardType = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/unlockreward/exchangereward/rewardtype"));
            foreach (exchangereward item in this.exchangereward_list)
            {
                if (item.rewardtype == rewardType)
                {
                    //item.buynum = lua.GetIntValue("results.unlockreward.exchangereward.buynum");
                    //item.cost = lua.GetIntValue("results.unlockreward.exchangereward.cost");
                    item.buynum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/unlockreward/exchangereward/buynum"));
                    item.cost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/unlockreward/exchangereward/cost"));
                    logInfo(logger, string.Format("打开[{0}]宝箱", item.RewardName));
                    break;
                }
            }
            return 0;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <rewardinfo>
         *      <reward>
         *          <type>5</type>
         *          <lv>1</lv>
         *          <num>200000</num>
         *      </reward>
         *  </rewardinfo>
         *  <exchangereward>
         *      <rewardtype>3</rewardtype>
         *      <buynum>1</buynum>
         *      <cost>50000</cost>
         *  </exchangereward>
         *  <arrowsleft>200000</arrowsleft>
         * </results>
         */
        public int exchangeReward(ProtocolMgr protocol, ILogger logger, User user, int rewardType)
        {
            string url = "/root/borrowingArrowsEvent!exchangeReward.action";
            string data = string.Format("rewardType={0}", rewardType);
            ServerResult result = protocol.postXml(url, data, "邀功");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //this.arrowsleft = lua.GetIntValue("results.arrowsleft");
            arrowsleft = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/arrowsleft"));
            int cost = 0;
            //rewardType = lua.GetIntValue("results.exchangereward.rewardtype");
            rewardType = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/exchangereward/rewardtype"));
            foreach (exchangereward item in this.exchangereward_list)
            {
                if (item.rewardtype == rewardType)
                {
                    cost = item.cost;
                    //item.buynum = lua.GetIntValue("results.exchangereward.buynum");
                    //item.cost = lua.GetIntValue("results.exchangereward.cost");
                    item.buynum = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/exchangereward/buynum"));
                    item.cost = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/exchangereward/cost"));
                    break;
                }
            }
            RewardInfo rewardinfo = new RewardInfo();
            rewardinfo.handleXmlNode(result.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("邀功，消耗{0}军功，获得{1}", cost, rewardinfo.ToString()));
            return 0;
        }

        /*
         * <results>
         *  <state>1</state>
         *  <calculatestream>
         *      <streamarrows>
         *          <id>0</id>
         *          <arrows>10000</arrows>
         *      </streamarrows>
         *      <streamarrows>
         *          <id>1</id>
         *          <arrows>20000</arrows>
         *      </streamarrows>
         *      <streamarrows>
         *          <id>2</id>
         *          <arrows>50000</arrows>
         *      </streamarrows>
         *  </calculatestream>
         * </results>
         */
        public int calculateStream(ProtocolMgr protocol, ILogger logger, User user, ref int goldavailable)
        {
            string url = "/root/borrowingArrowsEvent!calculateStream.action";
            ServerResult result = protocol.getXml(url, "神机妙算");
            if (result == null || !result.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(result.CmdResult.SelectSingleNode("/results"));
            //ListDictionary streamarrows_table = lua.GetListValue("results.calculatestream.streamarrows");
            //this.streamarrows_list.Clear();
            //foreach (LuaTable table in streamarrows_table.Values)
            //{
            //    streamarrows item = new streamarrows();
            //    item.id = AstdLuaObject.GetIntValue(table, "id");
            //    item.arrows = AstdLuaObject.GetIntValue(table, "arrows");
            //    this.streamarrows_list.Add(item);
            //}
            streamarrows_list = XmlHelper.GetClassList<streamarrows>(result.CmdResult.SelectNodes("/results/calculatestream/streamarrows"));
            goldavailable -= this.calculatestreamcost;
            return 0;
        }

        public long execute(ProtocolMgr protocol, ILogger logger, User user, int goldavailable, int buyboatcostlimit, bool calculatestream, int calculatestreamcostlimit, int costlimit, float percent)
        {
            //获取草船借箭活动信息
            int result = getPlayerBorrowingArrowsEventInfo(protocol, logger, user);
            if (result != 0) return next_halfhour();

            logInfo(logger, string.Format("草船借箭 剩余军功：{0} 总军功：{1}", this.arrowsleft, this.arrowstotal));

            //领取钥匙
            for (int i = 0; i < this.stagestatus_list.Count; i++)
            {
                if (this.stagestatus_list[i] == 0)
                {
                    result = getKey(protocol, logger, user, i);
                    if (result != 0) return next_halfhour();
                }
            }

            //开启宝箱
            while (this.unlocknum > 0)
            {
                this.exchangereward_list.Sort();
                foreach (exchangereward item in this.exchangereward_list)
                {
                    if (item.buynum == -1)
                    {
                        result = unlockReward(protocol, logger, user, item.rewardtype);
                        if (result != 0) return next_halfhour();
                        break;
                    }
                }
            }

            //邀功
            this.exchangereward_list.Sort();
            foreach (exchangereward item in this.exchangereward_list)
            {
                if (item.buynum != -1)
                {
                    while (item.cost <= costlimit && item.cost <= this.arrowsleft)
                    {
                        result = exchangeReward(protocol, logger, user, item.rewardtype);
                        if (result != 0) return next_halfhour();
                    }
                }
            }

            //发船
            if (this.currentstream == -2)
            {
                if (this.boatnum > 0 || (this.buyboatcost <= buyboatcostlimit && this.buyboatcost <= goldavailable))
                {
                    result = setSail(protocol, logger, user, ref goldavailable);
                    if (result != 0) return next_halfhour();
                }
                else
                {
                    return next_day();
                }
            }

            //借箭
            while (true)
            {
                float arrow_percent = (float)this.arrowsboat / (float)this.boatcapacity;
                if (arrow_percent >= percent)
                {
                    result = deliverArrows(protocol, logger, user);
                    if (result != 0) return next_halfhour();
                    break;
                }

                //神机妙算
                if (calculatestream)
                {
                    if (this.calculatestreamstate == 0 && this.calculatestreamcost <= calculatestreamcostlimit && this.calculatestreamcost <= goldavailable)
                    {
                        result = calculateStream(protocol, logger, user, ref goldavailable);
                        if (result != 0) return next_halfhour();
                    }
                }

                //选择区域
                this.streamarrows_list.Sort();
                int streamId = 0;
                if (calculatestream)
                {
                    foreach (streamarrows item in this.streamarrows_list)
                    {
                        if (this.arrowsboat + item.arrows <= this.boatcapacity)
                        {
                            streamId = item.id;
                            break;
                        }
                    }
                }
                else
                {
                    streamId = this.streamarrows_list[1].id;
                }
                result = choiceStream(protocol, logger, user, streamId);
                if (result > 0) return next_halfhour();
                else if (result == -1) break;
            }

            return immediate();
        }
    }
}
