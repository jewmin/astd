using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.building;
using com.lover.astd.common.model.enumer;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using com.lover.astd.common.model.misc;

namespace com.lover.astd.common.logic
{
    public class MiscMgr : MgrBase
    {
        private class ImposeEvent
        {
            public string name;

            public string desc1;

            public string desc2;

            public int loyalty_selection;

            public int other_selection;

            public ImposeEvent(string name, string desc1, string desc2, int loyalty_sel, int other_sel)
            {
                this.name = name;
                this.desc1 = desc1;
                this.desc2 = desc2;
                this.loyalty_selection = loyalty_sel;
                this.other_selection = other_sel;
            }
        }

        private class MarketConfig
        {
            public string item_type;

            public int min_quality;

            public bool use_gold;
        }

        private class MarketItem
        {
            public int id;

            public string restype;

            public int resnum;

            public int quality;

            public int type;

            public int num;

            public string itemname;

            public int gem_num;

            public int discount_num = 3;

            public string restype_name
            {
                get
                {
                    bool flag = this.restype == "copper";
                    string result;
                    if (flag)
                    {
                        result = "银币";
                    }
                    else
                    {
                        result = "金币";
                    }
                    return result;
                }
            }
        }

        private class StockItem
        {
            public int id;

            public string name;

            public int orig_price;

            public int now_price;

            public int num_per_buy;

            public int storage;

            public int cd;

            public int percent
            {
                get
                {
                    bool flag = this.orig_price > 0;
                    int result;
                    if (flag)
                    {
                        result = 100 * this.now_price / this.orig_price;
                    }
                    else
                    {
                        result = 0;
                    }
                    return result;
                }
            }
        }

        private class StoneQueue
        {
            public int id;

            public int now_stone;

            public int limit_stone;

            public bool can_gather;

            public int percent
            {
                get
                {
                    if (this.limit_stone > 0)
                    {
                        return 100 * this.now_stone / this.limit_stone;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public class RawStone
        {
            public int id;

            public string name;

            public int quality;

            public int state;

            public int price;

            public int sysprice;

            public int cutprice;

            public int num;

            public bool cut_use_gold
            {
                get
                {
                    return this.quality >= 6;
                }
            }
        }

        private class TradeMerchant
        {
            public int id;

            public string name;

            public bool discovered;
        }

        private class ClothInfo
        {
            public int id;

            public int type;

            public string name;

            public int cost;

            public float success;

            public float critical;

            public int sellprice;

            public int level;
        }

        private class WeaveAssist
        {
            public int id;

            public string name;

            public int level;
        }

        private class WeaveWorker
        {
            public int id;

            public string skill;

            public static string getSkillName(int skillId)
            {
                string[] array = new string[]
				{
					"",
					"级别+1(满)",
					"暴击+3",
					"成功+3",
					"暴击+5",
					"成功+5",
					"暴击+8",
					"暴击+10(满)",
					"成功+8",
					"成功+10(满)"
				};
                bool flag = skillId > array.Length - 1;
                string result;
                if (flag)
                {
                    result = "";
                }
                else
                {
                    result = array[skillId];
                }
                return result;
            }

            public bool haveLowSkill(out int lowSkillPos, out int lowSkillId)
            {
                lowSkillPos = 0;
                lowSkillId = 0;
                bool flag = this.skill == null || this.skill.Equals("null");
                bool result;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    string[] array = this.skill.Split(new char[]
					{
						','
					});
                    bool flag2 = array.Length < 2;
                    if (flag2)
                    {
                        result = false;
                    }
                    else
                    {
                        int num = 0;
                        int num2 = 0;
                        int.TryParse(array[0], out num);
                        int.TryParse(array[1], out num2);
                        bool flag3 = num > 0 && num != 1 && num != 7 && num != 9;
                        if (flag3)
                        {
                            lowSkillPos = 1;
                            lowSkillId = num;
                            result = true;
                        }
                        else
                        {
                            bool flag4 = num2 > 0 && num2 != 1 && num2 != 7 && num2 != 9;
                            if (flag4)
                            {
                                lowSkillPos = 2;
                                lowSkillId = num2;
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                    }
                }
                return result;
            }
        }

        private class Refiner : AsObject
        {
            private string _color;
            private int _order;

            public string Color
            {
                get
                {
                    return this._color;
                }
                set
                {
                    this._color = value;
                }
            }

            public int getColorInt()
            {
                int result = 0;
                string[] array = new string[]
				{
					"白",
					"蓝",
					"绿",
					"黄",
					"红",
					"紫"
				};
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Equals(this._color))
                    {
                        result = i;
                        break;
                    }
                }
                return result;
            }

            public int Order
            {
                get
                {
                    return _order;
                }
                set
                {
                    _order = value;
                }
            }
        }

        private class TaskItem
        {
            public int id;

            public int type;

            public int quality;

            public string name = "";

            public int reward;
        }

        private class GiftInfo
        {
            public string id;

            public string intime;

            public string content;

            public string name;

            public string status;

            public string getContentDesc()
            {
                string text = "";
                string[] array = this.content.Split(new char[]
				{
					','
				});
                string[] array2 = array;
                int num;
                for (int i = 0; i < array2.Length; i = num + 1)
                {
                    string text2 = array2[i];
                    bool flag = text2 != null && text2.Length != 0;
                    if (flag)
                    {
                        string[] array3 = text2.Split(new char[]
						{
							':'
						});
                        bool flag2 = array3.Length < 2;
                        if (flag2)
                        {
                            text += text2;
                        }
                        else
                        {
                            string text3 = array3[0];
                            string arg = array3[1];
                            string arg2 = text3;
                            bool flag3 = text3.IndexOf("gold") >= 0;
                            if (flag3)
                            {
                                arg2 = "金币";
                            }
                            else
                            {
                                bool flag4 = text3.IndexOf("silver") >= 0;
                                if (flag4)
                                {
                                    arg2 = "银币";
                                }
                                else
                                {
                                    bool flag5 = text3.IndexOf("ticket") >= 0;
                                    if (flag5)
                                    {
                                        arg2 = "点券";
                                    }
                                    else
                                    {
                                        bool flag6 = text3.IndexOf("baoshi") >= 0;
                                        if (flag6)
                                        {
                                            arg2 = "宝石";
                                        }
                                    }
                                }
                            }
                            text += string.Format("{0}+{1} ", arg2, arg);
                        }
                    }
                    num = i;
                }
                return text;
            }
        }

        private string[] gambling_stone_name_ = new string[] { "", "常见璞玉", "中原璞玉", "关东璞玉", "祁连璞玉", "缅甸璞玉", "和田璞玉" };

        public MiscMgr(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.DarkBlue;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }

        public bool getServerTime(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/server!getServerTime.action";
            ServerResult xml = protocol.getXml(url, "获取系统时间");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/time");
                long num = long.Parse(xmlNode.InnerText);
                this._tmrMgr.TimeStamp = num;
                logger.logDebug(string.Format("got timestamp = {0}", num));
                result = true;
            }
            return result;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2000];
            int count;
            while ((count = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, count);
            }
            output.Flush();
        }

        public Image getValidateCode(string url, ref List<Cookie> cookies)
        {
            Image result;
            try
            {
                string referer = "http://cdn05.aoshitang.com/astd_9-5-2/Main.swf?version=09.26.17.11.48_wuTao";
                HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, referer, null);
                bool flag = httpResult == null;
                if (flag)
                {
                    result = null;
                }
                else
                {
                    Stream decodedStream = httpResult.getDecodedStream();
                    Image image = Image.FromStream(decodedStream);
                    decodedStream.Close();
                    result = image;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public bool postValidateCode(ProtocolMgr protocol, ILogger logger, string code)
        {
            bool result;
            try
            {
                string url = "/root/validateCode!code.action";
                string data = "code=" + Uri.EscapeDataString(code);
                ServerResult serverResult = protocol.postXml(url, data, "提交验证码");
                bool flag = serverResult == null || !serverResult.CmdSucceed;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    base.logInfo(logger, string.Format("提交验证码成功", new object[0]));
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool chooseRole(ProtocolMgr protocol, ILogger logger, string playerId, string code)
        {
            string url = "/root/server!chooseRole.action";
            string data = string.Format("playerId={0}&code={1}", playerId, code);
            ServerResult serverResult = protocol.postXml(url, data, "选择玩家");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, "选择角色成功");
                result = true;
            }
            return result;
        }

        public int getPlayerInfo(ProtocolMgr protocol, ILogger logger, string roleName, User user)
        {
            string url = "/root/server!getPlayerInfoByUserId.action";
            ServerResult xml = protocol.getXml(url, "获取玩家信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/op");
                    bool flag3 = xmlNode != null && xmlNode.InnerText.Equals("xzjs");
                    if (flag3)
                    {
                        string code = "";
                        XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/code");
                        bool flag4 = xmlNode2 != null;
                        if (flag4)
                        {
                            code = xmlNode2.InnerText;
                        }
                        XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/player");
                        bool flag5 = false;
                        string playerId = "";
                        string text = "";
                        foreach (XmlNode xmlNode3 in xmlNodeList)
                        {
                            bool flag6 = xmlNode3 != null && xmlNode3.HasChildNodes;
                            if (flag6)
                            {
                                XmlNodeList childNodes = xmlNode3.ChildNodes;
                                foreach (XmlNode xmlNode4 in childNodes)
                                {
                                    bool flag7 = xmlNode4.Name == "playerid";
                                    if (flag7)
                                    {
                                        playerId = xmlNode4.InnerText;
                                    }
                                    else
                                    {
                                        bool flag8 = xmlNode4.Name == "playername";
                                        if (flag8)
                                        {
                                            text = xmlNode4.InnerText;
                                        }
                                    }
                                }
                                bool flag9 = text.Equals(roleName);
                                if (flag9)
                                {
                                    flag5 = true;
                                    break;
                                }
                            }
                        }
                        bool flag10 = !flag5;
                        if (flag10)
                        {
                            result = 2;
                            return result;
                        }
                        bool flag11 = !this.chooseRole(protocol, logger, playerId, code);
                        if (flag11)
                        {
                            result = 3;
                            return result;
                        }
                        xml = protocol.getXml(url, "获取玩家信息");
                        bool flag12 = xml == null;
                        if (flag12)
                        {
                            result = 1;
                            return result;
                        }
                        bool flag13 = !xml.CmdSucceed;
                        if (flag13)
                        {
                            result = 10;
                            return result;
                        }
                        cmdResult = xml.CmdResult;
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/blockreason");
                    bool flag14 = xmlNode5 != null;
                    if (flag14)
                    {
                        logger.logError("角色被封号, 原因是:" + xmlNode5.InnerText);
                        result = 4;
                    }
                    else
                    {
                        XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/player");
                        bool flag15 = xmlNode6 == null;
                        if (flag15)
                        {
                            xmlNode6 = cmdResult.SelectSingleNode("/results/message/player");
                        }
                        user.GotLoginReward = true;
                        user.clearActivities();
                        user.refreshPlayerInfo(xmlNode6);
                        XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/playerupdateinfo");
                        bool flag16 = xmlNode7 == null;
                        if (flag16)
                        {
                            xmlNode7 = cmdResult.SelectSingleNode("/results/message/playerupdateinfo");
                        }
                        user.refreshPlayerInfo(xmlNode7);
                        xmlNode6 = cmdResult.SelectSingleNode("/results/limitvalue");
                        bool flag17 = xmlNode6 == null;
                        if (flag17)
                        {
                            xmlNode6 = cmdResult.SelectSingleNode("/results/message/limitvalue");
                        }
                        user.updateLimits(xmlNode6);
                        base.logInfo(logger, string.Format("{0}({1}级, {2}), {3}年{4}, {5}金币, {6}银币", new object[]
						{
							user.Username,
							user.Level,
							user.Nation,
							user.Year,
							user.SeasonName,
							user.Gold,
							user.Silver
						}));
                        bool version_gift = user._version_gift;
                        if (version_gift)
                        {
                            this.getVersionGift(protocol, logger, user);
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        private void getVersionGift(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/mainCity!getUpdateReward.action";
            ServerResult xml = protocol.getXml(url, "领取版本更新奖励");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                user._version_gift = false;
                base.logInfo(logger, string.Format("领取版本更新奖励", new object[0]));
            }
        }

        public void getPerDayReward(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/mainCity!getPerDayReward.action";
            ServerResult xml = protocol.getXml(url, "领取每日奖励");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNodeList childNodes = cmdResult.SelectSingleNode("/results").ChildNodes;
            int num = 0;
            int num2 = 0;
            foreach (XmlNode xmlNode in childNodes)
            {
                if (xmlNode.Name == "gold")
                {
                    num = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "token")
                {
                    num2 = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "goldxs")
                {
                    int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "tokenxs")
                {
                    int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "playerupdateinfo")
                {
                    XmlNodeList childNodes2 = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes2)
                    {
                        if (xmlNode2.Name == "sys_gold")
                        {
                            user.Gold = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "token")
                        {
                            user.Token = int.Parse(xmlNode2.InnerText);
                        }
                    }
                }
            }
            base.logInfo(logger, string.Format("领取每日登录奖励, {0}金币, {1}军令", num, num2));
        }

        public void getLoginReward(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/mainCity!getLoginRewardInfo.action";
            ServerResult xml = protocol.getXml(url, "签到送礼");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int lastmonth_state15 = 0;
            int lastmonth_statefull = 0;
            int curmonth_state15 = 0;
            int curmonth_statefull = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/lastmonth/state15");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out lastmonth_state15);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/lastmonth/statefull");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out lastmonth_statefull);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/curmonth/state15");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out curmonth_state15);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/curmonth/statefull");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out curmonth_statefull);
            }
            StringBuilder sb = new StringBuilder();
            if (lastmonth_state15 == 1)
            {
                getReward(protocol, logger, user, 1, 1);
            }
            if (lastmonth_statefull == 1)
            {
                getReward(protocol, logger, user, 1, 2);
            }
            if (curmonth_state15 == 1)
            {
                getReward(protocol, logger, user, 2, 1);
            }
            if (curmonth_statefull == 1)
            {
                getReward(protocol, logger, user, 2, 2);
            }
        }

        public void getReward(ProtocolMgr protocol, ILogger logger, User user, int cId, int opt)
        {
            string url = "/root/mainCity!getReward.action";
            string data = string.Format("cId={0}&opt={1}", cId, opt);
            ServerResult xml = protocol.postXml(url, data, "领取签到送礼奖励");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("领取签到奖励 {0}", reward.ToString()));
        }

        public void getNewGiftReward(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/newGift!getNewGiftList.action";
            string data = "type=1";
            ServerResult serverResult = protocol.postXml(url, data, "登录礼物列表");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/weekendgift/id");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    string innerText = xmlNode.InnerText;
                    url = "/root/newGift!getNewGiftReward.action";
                    data = "giftId=" + innerText;
                    ServerResult serverResult2 = protocol.postXml(url, data, "领取登录礼物");
                    bool flag3 = serverResult2 == null || !serverResult2.CmdSucceed;
                    if (!flag3)
                    {
                        XmlDocument cmdResult2 = serverResult.CmdResult;
                        XmlNode xmlNode2 = cmdResult2.SelectSingleNode("/results/weekendgift/content");
                        bool flag4 = xmlNode2 != null;
                        if (flag4)
                        {
                            string innerText2 = xmlNode2.InnerText;
                            base.logInfo(logger, string.Format("领取登录礼物：{0}", innerText2));
                        }
                    }
                }
            }
        }

        public void getNewPerDayTask(ProtocolMgr protocol, ILogger logger, User user)
        {
            if (user.Level < 220) return;

            user._impose_task_num = 0;
            user._impose_force_task_num = 0;
            user._weave_task_num = 0;
            user._refine_task_num = 0;

            string url = "/root/task!getNewPerdayTask.action";
            ServerResult xml = protocol.getXml(url, "新每日任务信息");
            if (xml == null || !xml.CmdSucceed) return;

            XmlDocument cmdResult = xml.CmdResult;
            int finalreward = 0, inreward = 0, outreward = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/finalreward");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out finalreward);
            }
            outreward = finalreward / 10;
            inreward = finalreward % 10;
            if (finalreward == 22) return;

            xmlNode = cmdResult.SelectSingleNode("/results/dayboxstate");
            if (xmlNode != null)
            {
                string[] states = xmlNode.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < states.Length; i++)
                {
                    if (states[i] == "0")
                        openDayBox(protocol, logger, i + 1);
                }
            }

            xmlNode = cmdResult.SelectSingleNode("/results/redpacketinfo/redpacket");
            if (xmlNode != null)
            {
                if (xmlNode.InnerText.Equals("0"))
                    openWeekRedPacket(protocol, logger);
            }

            if (inreward == 1)
            {
                url = "/root/task!getNewPerdayTaskFinalReward.action";
                ServerResult xml2 = protocol.postXml(url, "rewardId=1", "新每日任务全部内政完成奖励");
                if (xml2 == null || !xml2.CmdSucceed)
                {
                    base.logInfo(logger, "新每日任务全部内政完成领取奖励失败");
                }
                else
                {
                    base.logInfo(logger, "新每日任务全部内政完成领取奖励成功");
                }
            }
            else
            {
                check_task(protocol, logger, user, cmdResult.SelectNodes("/results/task"));
            }

            if (outreward == 1)
            {
                url = "/root/task!getNewPerdayTaskFinalReward.action";
                ServerResult xml2 = protocol.postXml(url, "rewardId=2", "新每日任务全部外事完成奖励");
                if (xml2 == null || !xml2.CmdSucceed)
                {
                    base.logInfo(logger, "新每日任务全部外事完成领取奖励失败");
                }
                else
                {
                    base.logInfo(logger, "新每日任务全部外事完成领取奖励成功");
                }
            }
            else
            {
                check_task(protocol, logger, user, cmdResult.SelectNodes("/results/outtask"));
            }
        }

        private void openDayBox(ProtocolMgr protocol, ILogger logger, int rewardId)
        {
            string url = "/root/task!openDayBox.action";
            string data = string.Format("rewardId={0}", rewardId);
            ServerResult xml = protocol.postXml(url, data, "每日活跃-宝箱");
            if (xml == null || !xml.CmdSucceed) return;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("领取每日活跃宝箱, 获得{0}", reward.ToString()));
        }

        private void openWeekRedPacket(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/task!openWeekRedPacket.action";
            ServerResult xml = protocol.getXml(url, "本周红包");
            if (xml == null || !xml.CmdSucceed) return;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("领取本周红包, 获得{0}", reward.ToString()));
        }

        private void check_task(ProtocolMgr protocol, ILogger logger, User user, XmlNodeList xmlNodeList)
        {
            string url = "";
            int taskid = 0;
            int taskstate = 0;
            string taskname = "";
            string taskcontext = "";
            int finishnum = 0;
            int finishline = 0;
            foreach (XmlNode xmlNode2 in xmlNodeList)
            {
                if (xmlNode2 != null)
                {
                    XmlNode xmlNode3 = xmlNode2.SelectSingleNode("taskid");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out taskid);
                    }
                    xmlNode3 = xmlNode2.SelectSingleNode("taskstate");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out taskstate);
                    }
                    xmlNode3 = xmlNode2.SelectSingleNode("taskname");
                    if (xmlNode3 != null)
                    {
                        taskname = xmlNode3.InnerText;
                    }
                    xmlNode3 = xmlNode2.SelectSingleNode("content");
                    if (xmlNode3 != null)
                    {
                        taskcontext = xmlNode3.InnerText;
                    }
                    xmlNode3 = xmlNode2.SelectSingleNode("finishnum");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out finishnum);
                    }
                    xmlNode3 = xmlNode2.SelectSingleNode("finishline");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out finishline);
                    }
                    if (taskstate == 0)
                    {
                        url = "/root/task!newReceive.action";
                        string data = "taskId=" + taskid;
                        ServerResult serverResult = protocol.postXml(url, data, "接受新每日任务");
                        if (serverResult == null || !serverResult.CmdSucceed)
                        {
                            base.logInfo(logger, string.Format("接受新每日任务：{0}失败", taskname));
                        }
                        else
                        {
                            base.logInfo(logger, string.Format("接受新每日任务：{0}成功", taskname));
                        }
                    }
                    else if (taskstate == 1)
                    {
                        if (taskcontext.Contains("征收"))
                        {
                            user._impose_task_num = finishline - finishnum;
                            base.logInfo(logger, string.Format("新每日任务，需要征收{0}次", user._impose_task_num));
                        }
                        else if (taskcontext.Contains("强征"))
                        {
                            user._impose_force_task_num = finishline - finishnum;
                            base.logInfo(logger, string.Format("新每日任务，需要强征{0}次", user._impose_force_task_num));
                        }
                        else if (taskcontext.Contains("纺织"))
                        {
                            user._weave_task_num = finishline - finishnum;
                            base.logInfo(logger, string.Format("新每日任务，需要纺织{0}次", user._weave_task_num));
                        }
                        else if (taskcontext.Contains("炼制"))
                        {
                            user._refine_task_num = finishline - finishnum;
                            base.logInfo(logger, string.Format("新每日任务，需要炼制{0}次", user._refine_task_num));
                        }
                    }
                    else if (taskstate == 3)
                    {
                        url = "/root/task!getNewPerdayTaskReward.action";
                        string data = "rewardId=" + taskid;
                        ServerResult serverResult2 = protocol.postXml(url, data, "新每日任务领奖");
                        if (serverResult2 == null || !serverResult2.CmdSucceed)
                        {
                            base.logInfo(logger, string.Format("领取新每日任务：{0}奖励失败", taskname));
                        }
                        else
                        {
                            int type = 0;
                            int num = 0;
                            XmlDocument cmdResult2 = serverResult2.CmdResult;
                            XmlNode xmlNode4 = cmdResult2.SelectSingleNode("/results/rewardinfo/reward/type");
                            if (xmlNode4 != null)
                            {
                                int.TryParse(xmlNode4.InnerText, out type);
                            }
                            xmlNode4 = cmdResult2.SelectSingleNode("/results/rewardinfo/reward/num");
                            if (xmlNode4 != null)
                            {
                                int.TryParse(xmlNode4.InnerText, out num);
                            }
                            base.logInfo(logger, string.Format("领取新每日任务：{0}奖励成功,获得【{1}】【{2}】", taskname, this.convert_reward(type), num));
                        }
                    }
                }
            }
        }

        private string convert_reward(int type)
        {
            string result;
            if (type == 1)
            {
                result = "银币";
            }
            else if (type == 2)
            {
                result = "玉石";
            }
            else if (type == 5)
            {
                result = "宝石";
            }
            else if (type == 28)
            {
                result = "军令";
            }
            else if (type == 42)
            {
                result = "点券";
            }
            else
            {
                result = type.ToString();
            }
            return result;
        }

        /// <summary>
        /// 采集璞玉矿
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public int gamblingStone(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/gamblingStone!getNewPick.action";
            ServerResult xml = protocol.getXml(url, "采集信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return 4;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int remainhigh = 0;
            int cost1 = 5;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/active/remainhigh");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out remainhigh);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/active/cost1");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out cost1);
            }
            while (remainhigh > 0 && user.CurMovable >= cost1)
            {
                remainhigh--;
                url = "/root/gamblingStone!newPick.action";
                xml = protocol.getXml(url, "采集");
                if (xml == null || !xml.CmdSucceed)
                {
                    return 4;
                }
                cmdResult = xml.CmdResult;
                xmlNode = cmdResult.SelectSingleNode("/results/reward");
                if (xmlNode != null)//2:2,3:2
                {
                    string text = "璞玉矿采集一次";
                    string[] pairs = xmlNode.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in pairs)
                    {
                        string[] reward = item.Split(':');
                        int idx = int.Parse(reward[0]);
                        int num = int.Parse(reward[1]);
                        text += string.Format(", {0}+{1}", gambling_stone_name_[idx], num);
                    }
                    base.logInfo(logger, text);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/playerupdateinfo/curactive");
                if (xmlNode != null)
                {
                    int curactive = 0;
                    int.TryParse(xmlNode.InnerText, out curactive);
                    user.CurMovable = curactive;
                }
            }
            //切割璞玉
            if (user.Silver > 30000000)
            {
                url = "/root/gamblingStone!getNewPick.action";
                xml = protocol.getXml(url, "采集信息");
                if (xml == null || !xml.CmdSucceed)
                {
                    return 4;
                }
                cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/rawmaterial");
                if (xmlNodeList == null || xmlNodeList.Count == 0)
                {
                    return 4;
                }
                List<MiscMgr.RawStone> list = new List<MiscMgr.RawStone>();
                foreach (XmlNode xmlNode2 in xmlNodeList)
                {
                    MiscMgr.RawStone rawStone = new MiscMgr.RawStone();
                    foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
                    {
                        if (xmlNode3.Name == "id")
                        {
                            rawStone.id = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "name")
                        {
                            rawStone.name = xmlNode3.InnerText;
                        }
                        else if (xmlNode3.Name == "num")
                        {
                            rawStone.num = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "state")
                        {
                            rawStone.state = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "quality")
                        {
                            rawStone.quality = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "price")
                        {
                            rawStone.price = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "sysprice")
                        {
                            rawStone.sysprice = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "cutprice")
                        {
                            rawStone.cutprice = int.Parse(xmlNode3.InnerText);
                        }
                    }
                    if (!rawStone.cut_use_gold)
                    {
                        list.Add(rawStone);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    RawStone current = list[i];
                    int stone_got = 0;
                    while (user.Silver > 30000000 && current.num > 0)
                    {
                        current.num--;
                        user.Silver -= current.cutprice;
                        if (cutStone(protocol, logger, current.id, current.name, current.cutprice, out stone_got) != 0)
                        {
                            return 4;
                        }
                    }
                }
            }
            return 2;
        }

        public void autoBuyCredit(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/tickets!getTicketsReward.action";
            string data = "rewardId=1&num=1";
            ServerResult serverResult = protocol.postXml(url, data, "点券换军功");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/reward/item/name");
                string innerText = xmlNode.InnerText;
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/reward/item/num");
                string innerText2 = xmlNode2.InnerText;
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/reward/cost");
                string innerText3 = xmlNode3.InnerText;
                base.logInfo(logger, string.Format("花费{0}点券, 得到{1}{2}", innerText3, innerText, innerText2));
            }
        }

        public int newQiFu(ProtocolMgr protocol, ILogger logger, User user, int gold_available, int cointoqifu, int allopendq)
        {
            string url = "/root/yuandanqifu!getQifuEventInfo.action";
            ServerResult xml = protocol.getXml(url, "获取祈福活动信息");
            if (xml == null)
            {
                base.logInfo(logger, "获取祈福活动信息返回空值");
                return 4;
            }
            else if (!xml.CmdSucceed)
            {
                base.logInfo(logger, "祈福活动: " + xml.CmdError);
                if (xml.CmdError.Contains("不在活动时间"))
                {
                    return 0;
                }
                else
                {
                    return 4;
                }
            }
            XmlDocument cmdResult = xml.CmdResult;
            int leftfreetimes = 0;
            int qifuneedcoin = 0;
            int qifustate = 0;
            int xs = 1;
            int nextxs = 1;
            int maxfuqi = 100;
            int fuqi = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/leftfreetimes");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out leftfreetimes);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/qifuneedcoin");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out qifuneedcoin);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/qifustate");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out qifustate);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/xs");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out xs);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/nextxs");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out nextxs);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/maxfuqi");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out maxfuqi);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/fuqi");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out fuqi);
            }
            if (qifustate == 1)//选择祈福
            {
                if (fuqi >= maxfuqi)
                {
                    url = "/root/yuandanqifu!qifuActive.action";
                    xml = protocol.getXml(url, "双倍祈福");
                    if (xml != null && xml.CmdSucceed)
                    {
                        base.logInfo(logger, string.Format("下一轮祈福翻{0}倍", nextxs), Color.Purple);
                    }
                    else
                    {
                        return 4;
                    }
                }
                url = "/root/yuandanqifu!qifuChoose.action";
                string data = "indexId=2";
                xml = protocol.postXml(url, data, "选择祈福");
                if (xml != null && xml.CmdSucceed)
                {
                    cmdResult = xml.CmdResult;
                    xmlNode = cmdResult.SelectSingleNode("/results/fuqi");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out fuqi);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/xs");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out xs);
                    }
                    logInfo(logger, "本次选择情况:");
                    int reward_type = 0;
                    int reward_tickets = 0;
                    int reward_fuqi = 0;
                    int reward_get = 0;
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/card");
                    foreach (XmlNode xmlNode2 in xmlNodeList)
                    {
                        if (xmlNode2 != null)
                        {
                            XmlNode xmlNode3 = xmlNode2.SelectSingleNode("type");
                            if (xmlNode3 != null)
                            {
                                int.TryParse(xmlNode3.InnerText, out reward_type);
                            }
                            xmlNode3 = xmlNode2.SelectSingleNode("tickets");
                            if (xmlNode3 != null)
                            {
                                int.TryParse(xmlNode3.InnerText, out reward_tickets);
                            }
                            xmlNode3 = xmlNode2.SelectSingleNode("fuqi");
                            if (xmlNode3 != null)
                            {
                                int.TryParse(xmlNode3.InnerText, out reward_fuqi);
                            }
                            xmlNode3 = xmlNode2.SelectSingleNode("get");
                            if (xmlNode3 != null)
                            {
                                int.TryParse(xmlNode3.InnerText, out reward_get);
                            }
                            if (reward_get == 1)
                            {
                                logInfo(logger, string.Format("第{0}个-点卷+{1} 福气+{2}-选择", reward_type, reward_tickets * xs, reward_fuqi * xs), Color.Red);
                            }
                            else
                            {
                                logInfo(logger, string.Format("第{0}个-点卷+{1} 福气+{2}-未选", reward_type, reward_tickets * xs, reward_fuqi * xs));
                            }
                        }
                    }
                    string text = string.Format("当前福气{0}, 倍数{1}, 免费祈福还剩次数{2}次", fuqi, xs, leftfreetimes);
                    if (qifuneedcoin > 0)
                    {
                        string.Format("当前福气{0}, 倍数{1}", fuqi, xs);
                    }
                    base.logInfo(logger, text);
                    return 2;
                }
                return 4;
            }
            else if (qifustate == 2)//全开or下一轮祈福
            {
                url = "/root/yuandanqifu!nextQifu.action";
                xml = protocol.getXml(url, "下一轮");
                if (xml != null && xml.CmdSucceed)
                {
                    base.logInfo(logger, "下一轮祈福");
                    return 2;
                }
                return 4;
            }
            else if (qifustate == 3)//双倍祈福
            {
                url = "/root/yuandanqifu!qifuActive.action";
                xml = protocol.getXml(url, "双倍祈福");
                if (xml != null && xml.CmdSucceed)
                {
                    base.logInfo(logger, string.Format("下一轮祈福翻{0}倍", nextxs), Color.Purple);
                    return 2;
                }
                return 4;
            }
            else if (leftfreetimes > 0 || cointoqifu >= qifuneedcoin)
            {
                url = "/root/yuandanqifu!startQifu.action";
                xml = protocol.getXml(url, "开始祈福");
                if (xml != null && xml.CmdSucceed)
                {
                    string text = "免费祈福1次";
                    if (qifuneedcoin > 0)
                    {
                        text = string.Format("花费{0}金币祈福1次", qifuneedcoin);
                    }
                    base.logInfo(logger, text);
                    return 2;
                }
                return 4;
            }
            return 0;
        }

        public void yuandanQifu(ProtocolMgr protocol, ILogger logger, User user, int gold_available, int cointoqifu, int allopendq)
        {
            string url = "/root/yuandanqifu!getQifuInfo.action";
            ServerResult xml = protocol.getXml(url, "获取祈福活动信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int leftfreetimes = 0;
            int qifuneedcoin = 0;
            int num3 = 1;
            string text = "";
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/leftfreetimes");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out leftfreetimes);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/qifuneedcoin");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out qifuneedcoin);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/qifulevel");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out num3);
            }
            while (leftfreetimes > 0 || cointoqifu >= qifuneedcoin)
            {
                url = "/root/yuandanqifu!qifu.action";
                ServerResult serverResult = protocol.getXml(url, "开始祈福");
                if (serverResult == null || !serverResult.CmdSucceed)
                {
                    if (!serverResult.CmdError.Contains("先选礼包"))
                    {
                        break;
                    }
                }
                XmlDocument cmdResult2 = serverResult.CmdResult;
                bool flag7 = qifuneedcoin > 0;
                if (flag7)
                {
                    base.logInfo(logger, string.Format("花费{0}金币祈福1次", qifuneedcoin));
                }
                url = "/root/yuandanqifu!chooseOne.action";
                string data = "indexId=1";
                serverResult = protocol.postXml(url, data, "选择祈福");
                bool flag8 = serverResult == null || !serverResult.CmdSucceed;
                if (flag8)
                {
                    break;
                }
                cmdResult2 = serverResult.CmdResult;
                xmlNode = cmdResult2.SelectSingleNode("/results/qifuvalue");
                bool flag9 = xmlNode != null;
                if (flag9)
                {
                    int.TryParse(xmlNode.InnerText, out num4);
                }
                bool flag10 = qifuneedcoin > 0;
                if (flag10)
                {
                    text = string.Format("当前福气{0},倍数{1}", num4, num3);
                }
                else
                {
                    text = string.Format("当前福气{0},倍数{1},免费祈福还剩次数{2}次", num4, num3, leftfreetimes - 1);
                }
                base.logInfo(logger, text);
                XmlNodeList xmlNodeList = cmdResult2.SelectNodes("/results/card");
                int num9 = 0;
                text = "本次选择情况";
                foreach (XmlNode xmlNode2 in xmlNodeList)
                {
                    bool flag11 = xmlNode2 != null;
                    if (flag11)
                    {
                        XmlNode xmlNode3 = xmlNode2.SelectSingleNode("type");
                        bool flag12 = xmlNode3 != null;
                        if (flag12)
                        {
                            int.TryParse(xmlNode3.InnerText, out num5);
                        }
                        xmlNode3 = xmlNode2.SelectSingleNode("rewardtype");
                        bool flag13 = xmlNode3 != null;
                        if (flag13)
                        {
                            int.TryParse(xmlNode3.InnerText, out num6);
                        }
                        xmlNode3 = xmlNode2.SelectSingleNode("num");
                        bool flag14 = xmlNode3 != null;
                        if (flag14)
                        {
                            int.TryParse(xmlNode3.InnerText, out num7);
                        }
                        xmlNode3 = xmlNode2.SelectSingleNode("get");
                        bool flag15 = xmlNode3 != null;
                        if (flag15)
                        {
                            int.TryParse(xmlNode3.InnerText, out num8);
                        }
                        bool flag16 = num8 == 0 && num6 == 1;
                        if (flag16)
                        {
                            num9 += num7 * num3;
                        }
                        bool flag17 = num6 == 1;
                        if (flag17)
                        {
                            text += string.Format(",第{0}个-点券{1}", num5, num7 * num3);
                        }
                        else
                        {
                            text += string.Format(",第{0}个-福气{1}", num5, num7 * num3);
                        }
                        bool flag18 = num8 == 1;
                        if (flag18)
                        {
                            text += "-选择";
                        }
                        else
                        {
                            text += "-未选";
                        }
                    }
                }
                base.logInfo(logger, text);
                bool flag19 = num9 >= allopendq && gold_available > 20;
                if (flag19)
                {
                    url = "/root/yuandanqifu!fulingEnze.action";
                    serverResult = protocol.getXml(url, "金币全开");
                    bool flag20 = serverResult == null || !serverResult.CmdSucceed;
                    if (flag20)
                    {
                        break;
                    }
                    base.logInfo(logger, "选择金币全开");
                    cmdResult2 = serverResult.CmdResult;
                }
                xmlNode = cmdResult2.SelectSingleNode("/results/leftfreetimes");
                bool flag21 = xmlNode != null;
                if (flag21)
                {
                    int.TryParse(xmlNode.InnerText, out leftfreetimes);
                }
                xmlNode = cmdResult2.SelectSingleNode("/results/qifuneedcoin");
                bool flag22 = xmlNode != null;
                if (flag22)
                {
                    int.TryParse(xmlNode.InnerText, out qifuneedcoin);
                }
                xmlNode = cmdResult2.SelectSingleNode("/results/qifulevel");
                bool flag23 = xmlNode != null;
                if (flag23)
                {
                    int.TryParse(xmlNode.InnerText, out num3);
                }
            }
        }

        public void baiShen(ProtocolMgr protocol, ILogger logger, User user, int gold_available, int advance_baoshi, int advance_dianquan, int advance_bintie, bool b_usefree, int cishu)
        {
            while (true)
            {
                string url = "/root/baishen!getInformation.action";
                ServerResult xml = protocol.getXml(url, "取拜神信息");
                if (xml == null || !xml.CmdSucceed) break;
                
                int cailiaolefttime = 0;
                int leftbaishentime = 0;
                int buycishu = 0;
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cailiaolefttime");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out cailiaolefttime);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/leftbaishentime");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out leftbaishentime);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/buycishu");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out buycishu);
                }

                if (leftbaishentime == 0)
                {
                    if (buycishu >= cishu || gold_available < 5) break;
                    this.buyCishu(protocol, logger, user);
                }

                xmlNode = cmdResult.SelectSingleNode("/results/thisreward");
                string innerText = xmlNode.InnerText;
                string[] array = innerText.Split(new char[] { ':' });
                int count = int.Parse(array[1]);
                int type = 1;
                string text;
                if (array[0].Contains("baoshi"))
                {
                    text = "本次祭祀基础奖励为宝石：" + count.ToString();
                    if (count >= advance_baoshi) type = 3;
                }
                else if (array[0].Contains("bintie"))
                {
                    text = "本次祭祀基础奖励为镔铁：" + count.ToString();
                    if (count >= advance_bintie) type = 3;
                }
                else
                {
                    text = "本次祭祀基础奖励为点券：" + count.ToString();
                    if (count >= advance_dianquan) type = 3;
                }

                if (type == 3)
                {
                    if (cailiaolefttime < 10)
                    {
                        if (b_usefree) type = 1;
                    }
                }

                if (type == 1)
                {
                    if (cailiaolefttime > 0) text += ", 准备花费1个免费贡品, 初级祭祀";
                    else text += ", 准备花费1金币, 初级祭祀";
                }
                else if (type == 3)
                {
                    if (cailiaolefttime >= 10) text += ", 准备花费10个免费贡品, 高级祭祀";
                    else text += ", 准备花费50金币，高级祭祀";
                }
                else
                {
                    text += ", 准备祭祀";
                }
                base.logInfo(logger, text);
                this.startBaiShen(protocol, logger, user, type);
            }
        }

        private void buyCishu(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/baishen!buyCishu.action";
            ServerResult xml = protocol.getXml(url, "拜神:购买一次显灵");
            if (xml == null || !xml.CmdSucceed)
            {
                base.logInfo(logger, "购买拜神次数失败" + xml.CmdError);
            }
            else
            {
                base.logInfo(logger, "没拜神次数了，花费5金币买一次");
            }
        }

        private void startBaiShen(ProtocolMgr protocol, ILogger logger, User user, int type)
        {
            string url = "/root/baishen!baishen.action";
            string data = string.Format("type={0}", type);
            ServerResult serverResult = protocol.postXml(url, data, "开始拜神");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                base.logInfo(logger, "拜神祭祀失败" + serverResult.CmdError);
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, "拜神祭祀成功");
            }
        }

        public void ShenHuo(ProtocolMgr protocol, ILogger logger, User user, int gold_available)
        {
            string url = "/root/shenhuo!getInfo.action";
            ServerResult xml = protocol.getXml(url, "取百炼精铁信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                url = "/root/shenhuo!start.action";
                xml = protocol.getXml(url, "开始百炼精铁");
                bool flag2 = xml == null || !xml.CmdSucceed;
                if (flag2)
                {
                    base.logInfo(logger, "百炼精铁：" + xml.CmdError);
                }
                else
                {
                    base.logInfo(logger, "百炼精铁成功开启");
                }
            }
        }

        public void getExtraInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/server!getExtraInfo.action";
            ServerResult xml = protocol.getXml(url, "获取额外信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                user.removeActivity(ActivityType.RepayEvent);
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/player/isinrepayevent");
                bool flag2 = xmlNode != null && xmlNode.InnerText.ToLower().Equals("true");
                if (flag2)
                {
                    user.addActivity(ActivityType.RepayEvent);
                }
                user._can_get_nation_task_reward = false;
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/player/nationtaskreward");
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    user._can_get_nation_task_reward = (xmlNode2.InnerText == "1");
                }
            }
        }

        public void getExtraInfo2(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/server!getPlayerExtraInfo2.action";
            ServerResult xml = protocol.getXml(url, "获取额外信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/player/cantech");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    user._jail_can_tech = (xmlNode.InnerText == "1");
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/player/curactive");
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    int num = -1;
                    int.TryParse(xmlNode2.InnerText, out num);
                    bool flag4 = num >= 0;
                    if (flag4)
                    {
                        user.CurMovable = num;
                    }
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/player/arreststate");
                bool flag5 = xmlNode3 != null;
                if (flag5)
                {
                    int arrest_state = 0;
                    int.TryParse(xmlNode3.InnerText, out arrest_state);
                    user._arrest_state = arrest_state;
                }
                else
                {
                    user._arrest_state = 0;
                }
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/player/maxactive");
                bool flag6 = xmlNode4 != null;
                if (flag6)
                {
                    int num2 = -1;
                    int.TryParse(xmlNode4.InnerText, out num2);
                    bool flag7 = num2 >= 0;
                    if (flag7)
                    {
                        user._max_active = num2;
                    }
                }
                XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/player/tokencd");
                bool flag8 = xmlNode5 != null;
                if (flag8)
                {
                    int num3 = 0;
                    int.TryParse(xmlNode5.InnerText, out num3);
                    user.TokenCdTime = (long)num3;
                    bool flag9 = num3 == 0;
                    if (flag9)
                    {
                        user.TokenCdFlag = false;
                    }
                }
                XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/player/scoretokencd");
                bool flag10 = xmlNode6 != null;
                if (flag10)
                {
                    int attack_scoreTokencd = 0;
                    int.TryParse(xmlNode6.InnerText, out attack_scoreTokencd);
                    user._attack_scoreTokencd = attack_scoreTokencd;
                }
                user._is_new_trade = false;
                XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/player/trade200");
                bool flag11 = xmlNode7 != null;
                if (flag11)
                {
                    user._is_new_trade = (xmlNode7.InnerText == "1");
                }
                XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/player/remainbuytimes");
                bool flag12 = xmlNode8 != null;
                if (flag12)
                {
                    int.TryParse(xmlNode8.InnerText, out user._can_buy_active_count);
                }
                else
                {
                    user._can_buy_active_count = 0;
                }
                user._is_refine_bintie = false;
                XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/player/refinebintie");
                if (xmlNode9 != null)
                {
                    user._is_refine_bintie = (xmlNode9.InnerText == "1");
                }
            }
        }

        public void getSessionId(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/server!getSessionId.action";
            ServerResult xml = protocol.getXml(url, "获取Session信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
            }
        }

        public void getDinnerInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/dinner!getAllDinner.action";
            ServerResult xml = protocol.getXml(url, "获取宴会信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }

            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/indinnertime");
            if (xmlNode != null)
            {
                user._dinner_in_time = int.Parse(xmlNode.InnerText);
            }
            else
            {
                user._dinner_in_time = -1;
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/teamstate");
            user._dinner_joined = (xmlNode2.InnerText == "1");
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/normaldinner");
            if (xmlNode3 == null)
            {
                return;
            }

            user._dinner_count = 0;
            foreach (XmlNode xmlNode4 in xmlNode3)
            {
                if (xmlNode4.Name == "num")
                {
                    int.TryParse(xmlNode4.InnerText, out user._dinner_count);
                }
            }
            if (user._dinner_count == 0)
            {
                return;
            }

            XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results");
            if (xmlNode5 == null || !xmlNode5.HasChildNodes)
            {
                return;
            }

            user._dinner_team_id = "";
            user._dinner_team_creator = "";
            XmlNodeList childNodes = xmlNode5.ChildNodes;
            string nation = user.Nation;
            foreach (XmlNode xmlNode6 in childNodes)
            {
                if (xmlNode6.Name == "team")
                {
                    XmlNodeList childNodes2 = xmlNode6.ChildNodes;
                    string dinner_team_id = "";
                    string dinner_team_creator = "";
                    string text = "";
                    int num = 0;
                    int maxnum = 0;
                    foreach (XmlNode xmlNode7 in childNodes2)
                    {
                        if (xmlNode7.Name == "teamid")
                        {
                            dinner_team_id = xmlNode7.InnerText;
                        }
                        else if (xmlNode7.Name == "creator")
                        {
                            dinner_team_creator = xmlNode7.InnerText;
                        }
                        else if (xmlNode7.Name == "nation")
                        {
                            text = xmlNode7.InnerText;
                        }
                        else if (xmlNode7.Name == "num")
                        {
                            num = int.Parse(xmlNode7.InnerText);
                        }
                        else if (xmlNode7.Name == "maxnum")
                        {
                            maxnum = int.Parse(xmlNode7.InnerText);
                        }
                    }
                    if (/*text.IndexOf(nation) >= 0 && */maxnum != num)
                    {
                        user._dinner_team_id = dinner_team_id;
                        user._dinner_team_creator = dinner_team_creator;
                        break;
                    }
                }
            }
        }

        public void takeDinner(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/dinner!preTakeDinner.action";
            string data = "dinnerType=0";
            ServerResult serverResult = protocol.postXml(url, data, "获取预备宴会信息");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                string url2 = "/root/dinner!takeDinner.action";
                ServerResult serverResult2 = protocol.postXml(url2, data, "获取举办宴会信息");
            }
        }

        public void dismissDinner(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/dinner!dismissDinner.action";
            ServerResult xml = protocol.getXml(url, "退出宴会");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                base.logInfo(logger, "退出宴会成功");
            }
        }

        public void joinDinner(ProtocolMgr protocol, ILogger logger, User user)
        {
            bool flag = user._dinner_team_id == "";
            if (!flag)
            {
                string url = "/root/dinner!joinDinner.action";
                string data = "teamId=" + user._dinner_team_id;
                string text = string.Format("加入[{0}]的宴会", user._dinner_team_creator);
                ServerResult serverResult = protocol.postXml(url, data, text);
                bool flag2 = serverResult == null || !serverResult.CmdSucceed;
                if (!flag2)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    base.logInfo(logger, text);
                }
            }
        }

        public int getFeteEventInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/fete!getFeteEventInfo.action";
            ServerResult xml = protocol.getXml(url, "获取祭祀活动信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                if (xml.CmdError.Contains("不在活动期间"))
                {
                    return 3;
                }
                return 2;
            }
            else
            {
                user._is_fete_activity = true;
                XmlDocument cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/god");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("state");
                    if (xmlNode2.InnerText == "1")
                    {
                        XmlNode xmlNode3 = xmlNode.SelectSingleNode("godticket/id");
                        if (xmlNode3 != null)
                        {
                            XmlNode xmlNode4 = xmlNode.SelectSingleNode("godticket/ticket");
                            if (xmlNode4 != null)
                            {
                                bool feteEventAward = this.getFeteEventAward(protocol, logger, xmlNode3.InnerText);
                                if (feteEventAward)
                                {
                                    base.logInfo(logger, string.Format("领取祭祀活动奖励成功, 点券+{0}", xmlNode4.InnerText));
                                }
                            }
                        }
                    }
                }
                return 0;
            }
        }

        public bool getFeteEventAward(ProtocolMgr protocol, ILogger logger, string feteId)
        {
            string url = "/root/fete!recvFeteTicket.action";
            string data = "feteId=" + feteId;
            ServerResult serverResult = protocol.postXml(url, data, "获取祭祀活动奖励信息");
            if (serverResult == null)
            {
                return false;
            }
            if (!serverResult.CmdSucceed)
            {
                return false;
            }
            return true;
        }

        public bool getFeteInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/fete.action";
            ServerResult xml = protocol.getXml(url, "获取玩家祭祀信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode list = cmdResult.SelectSingleNode("/results/fetelist");
            this.updateFeteInfo(list, user);
            return true;
        }

        public void doFete(ProtocolMgr protocol, ILogger logger, User user, int feteId, bool free = false)
        {
            string url = "/root/fete!dofete.action";
            ServerResult serverResult = protocol.postXml(url, "feteId=" + feteId, "祭祀");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            string text = string.Format("祭祀{0}, 花费{1}金币, 获得", user._fete_names[feteId - 1], free ? 0 : user._fete_now_gold[feteId - 1]);
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gains");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            foreach (XmlNode xmlNode2 in childNodes)
            {
                if (xmlNode2.Name == "gain")
                {
                    int pro = 1;
                    int addvalue = 0;
                    string addtype = "";
                    XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                    foreach (XmlNode xmlNode3 in childNodes2)
                    {
                        if (xmlNode3.Name == "pro")
                        {
                            pro = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "addvalue")
                        {
                            addvalue = int.Parse(xmlNode3.InnerText);
                        }
                        else if (xmlNode3.Name == "addtype")
                        {
                            addtype = xmlNode3.InnerText;
                        }
                    }
                    if (pro > 1)
                    {
                        text += string.Format("[{0}倍暴击, {1}{2}] ", pro, addvalue, addtype);
                    }
                    else
                    {
                        text += string.Format("[{0}{1}]", addvalue, addtype);
                    }
                }
            }
            base.logInfo(logger, text);
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/fetelist");
            if (xmlNode4 != null)
            {
                this.updateFeteInfo(xmlNode4, user);
            }
        }

        private void updateFeteInfo(XmlNode list, User user)
        {
            XmlNodeList childNodes = list.ChildNodes;
            foreach (XmlNode xmlNode in childNodes)
            {
                if (xmlNode.Name == "fete")
                {
                    XmlNodeList childNodes2 = xmlNode.ChildNodes;
                    int id = 0;
                    int gold = 0;
                    int freetimes = 0;
                    string name = "";
                    int lv = 0;
                    foreach (XmlNode xmlNode2 in childNodes2)
                    {
                        if (xmlNode2.Name == "id")
                        {
                            id = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "gold")
                        {
                            gold = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "freetimes")
                        {
                            freetimes = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "name")
                        {
                            name = xmlNode2.InnerText;
                        }
                        else if (xmlNode2.Name == "lv")
                        {
                            lv = int.Parse(xmlNode2.InnerText);
                        }
                    }
                    user._fete_now_gold[id - 1] = gold;
                    user._fete_now_free_times[id - 1] = freetimes;
                    user._fete_min_levels[id - 1] = lv;
                    user._fete_names[id - 1] = name;
                }
                else if (xmlNode.Name == "freeallfete")
                {
                    int freeallfete = 0;
                    int.TryParse(xmlNode.InnerText, out freeallfete);
                    user._fete_now_free_times[5] = freeallfete;
                }
            }
            user._fete_names[5] = "大祭祀";
            user._fete_min_levels[5] = 50;
            //if (user._fete_names[1] == "昊天")
            //{
            //    user._is_fete_activity = true;
            //}
        }

        public int getRepayEventInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/gift!getRepayEventGiftInfo.action";
            ServerResult xml = protocol.getXml(url, "获取军资回馈信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/eventnum");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out user._impose_repayEvent_numNow);
                    }
                    else
                    {
                        user._impose_repayEvent_numNow = 0;
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/rewardnum");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        user._impose_repayEvent_rewardStatus = xmlNode2.InnerText;
                    }
                    else
                    {
                        user._impose_repayEvent_rewardStatus = "";
                    }
                    result = 0;
                }
            }
            return result;
        }

        public void getRepayEventReward(ProtocolMgr protocol, ILogger logger, int id)
        {
            string url = "/root/gift!receiveRepayEventReward.action";
            string data = "id=" + id;
            ServerResult serverResult = protocol.postXml(url, data, "获取军资回馈活动奖励");
            bool flag = serverResult == null;
            if (!flag)
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (!flag2)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message");
                    int num = 0;
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    base.logInfo(logger, string.Format("领取军资回馈活动奖励, 银币+{0}", num));
                }
            }
        }

        public int getImposeEventInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/gift!getEventGiftInfo.action";
            ServerResult xml = protocol.getXml(url, "获取征收活动信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/eventnum");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out user._impose_imposeEvent_numNow);
                    }
                    else
                    {
                        user._impose_imposeEvent_numNow = 0;
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/rewardnum");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        user._impose_imposeEvent_rewardStatus = xmlNode2.InnerText;
                    }
                    else
                    {
                        user._impose_imposeEvent_rewardStatus = "";
                    }
                    result = 0;
                }
            }
            return result;
        }

        public void getImposeEventReward(ProtocolMgr protocol, ILogger logger, int id)
        {
            string url = "/root/gift!receiveEventReward.action";
            string data = "id=" + id;
            ServerResult serverResult = protocol.postXml(url, data, "获取征收活动奖励");
            bool flag = serverResult == null;
            if (!flag)
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (!flag2)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message");
                    int num = 0;
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    base.logInfo(logger, string.Format("领取征收活动奖励, 银币+{0}", num));
                }
            }
        }

        public bool getImposeInfo(ProtocolMgr protocol, ILogger logger, User user, int min_loyalty)
        {
            string url = "/root/mainCity!perImpose.action";
            ServerResult xml = protocol.getXml(url, "获取玩家征收信息");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                base.tryUseCard(protocol, logger, cmdResult);
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "imposedto";
                    if (flag2)
                    {
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        IEnumerator enumerator2 = childNodes2.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                XmlNode xmlNode3 = (XmlNode)enumerator2.Current;
                                bool flag3 = xmlNode3.Name == "loyalty";
                                if (flag3)
                                {
                                    user._impose_now_loyalty = int.Parse(xmlNode3.InnerText);
                                }
                                else
                                {
                                    bool flag4 = xmlNode3.Name == "forceimposecost";
                                    if (flag4)
                                    {
                                        user._impose_force_gold = int.Parse(xmlNode3.InnerText);
                                    }
                                    else
                                    {
                                        bool flag5 = xmlNode3.Name == "imposenum";
                                        if (flag5)
                                        {
                                            user._impose_count = int.Parse(xmlNode3.InnerText);
                                        }
                                        else
                                        {
                                            bool flag6 = xmlNode3.Name == "lastimposetime";
                                            if (flag6)
                                            {
                                                user._impose_cdtime = long.Parse(xmlNode3.InnerText);
                                            }
                                            else
                                            {
                                                bool flag7 = xmlNode3.Name == "cdflag";
                                                if (flag7)
                                                {
                                                    bool flag8 = xmlNode3.InnerText == "0";
                                                    if (flag8)
                                                    {
                                                        user._impose_hasCd = false;
                                                    }
                                                    else
                                                    {
                                                        user._impose_hasCd = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            continue;
                        }
                        finally
                        {
                            IDisposable disposable = enumerator2 as IDisposable;
                            bool flag9 = disposable != null;
                            if (flag9)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    bool flag10 = xmlNode2.Name == "larrydto";
                    if (flag10)
                    {
                        string text = "";
                        XmlNodeList childNodes3 = xmlNode2.ChildNodes;
                        foreach (XmlNode xmlNode4 in childNodes3)
                        {
                            bool flag11 = xmlNode4.Name == "name";
                            if (flag11)
                            {
                                text = xmlNode4.InnerText;
                            }
                        }
                        bool flag12 = text != "";
                        if (flag12)
                        {
                            string text2 = "";
                            text2 += string.Format("获得征收事件[{0}] ", text);
                            int imposeEvents = this.getImposeEvents(text, min_loyalty, user._impose_now_loyalty);
                            if (imposeEvents > 0)
                            {
                                text2 += this.answerImposeEvent(protocol, logger, imposeEvents);
                            }
                            base.logInfo(logger, text2);
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public bool impose(ProtocolMgr protocol, ILogger logger, User user, bool force, int min_loyalty)
        {
            string url = "/root/mainCity!impose.action";
            if (force)
            {
                url = "/root/mainCity!forceImpose.action";
            }
            string text = force ? "强制征收" : "征收";
            ServerResult xml = protocol.getXml(url, text);
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                string text2 = text + "成功 获得";
                string text3 = "";
                bool flag2 = false;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag3 = xmlNode2.Name == "larrydto";
                    if (flag3)
                    {
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        IEnumerator enumerator2 = childNodes2.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                XmlNode xmlNode3 = (XmlNode)enumerator2.Current;
                                bool flag4 = xmlNode3.Name == "name";
                                if (flag4)
                                {
                                    text3 = xmlNode3.InnerText;
                                }
                            }
                            continue;
                        }
                        finally
                        {
                            IDisposable disposable = enumerator2 as IDisposable;
                            bool flag5 = disposable != null;
                            if (flag5)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    bool flag6 = xmlNode2.Name == "imposenum";
                    if (flag6)
                    {
                        user._impose_count = int.Parse(xmlNode2.InnerText);
                    }
                    else
                    {
                        bool flag7 = xmlNode2.Name == "imposecd";
                        if (flag7)
                        {
                            user._impose_cdtime = long.Parse(xmlNode2.InnerText);
                        }
                        else
                        {
                            bool flag8 = xmlNode2.Name == "loyalty";
                            if (flag8)
                            {
                                user._impose_now_loyalty = int.Parse(xmlNode2.InnerText);
                            }
                            else
                            {
                                bool flag9 = xmlNode2.Name == "copperdis";
                                if (flag9)
                                {
                                    text2 = text2 + xmlNode2.InnerText + "银币 ";
                                }
                                else
                                {
                                    bool flag10 = xmlNode2.Name == "golddis" && xmlNode2.InnerText != "0";
                                    if (flag10)
                                    {
                                        text2 = text2 + xmlNode2.InnerText + "金币 ";
                                        flag2 = true;
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag11 = text3 != "";
                if (flag11)
                {
                    text2 += string.Format("获得征收事件[{0}] ", text3);
                    int imposeEvents = this.getImposeEvents(text3, min_loyalty, user._impose_now_loyalty);
                    bool flag12 = imposeEvents > 0;
                    if (flag12)
                    {
                        text2 += this.answerImposeEvent(protocol, logger, imposeEvents);
                    }
                }
                text2 += string.Format(" 征收CD[{0}] ", base.formatTime(user._impose_cdtime));
                base.logInfo(logger, text2);
                bool flag13 = flag2;
                if (flag13)
                {
                    logger.logSurprise(text2);
                }
                result = true;
            }
            return result;
        }

        public string answerImposeEvent(ProtocolMgr protocol, ILogger logger, int optionId)
        {
            string url = "/root/mainCity!selectLE.action";
            ServerResult serverResult = protocol.postXml(url, "opt=" + optionId, "回答征收问题");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                string text = string.Format("回答征收问题, 选择选项[{0}], ", optionId);
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "ledto";
                    if (flag2)
                    {
                        int num = 0;
                        int num2 = 0;
                        int num3 = 0;
                        int num4 = 0;
                        int num5 = 0;
                        int num6 = 0;
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        foreach (XmlNode xmlNode3 in childNodes2)
                        {
                            bool flag3 = xmlNode3.Name == "l";
                            if (flag3)
                            {
                                num = int.Parse(xmlNode3.InnerText);
                            }
                            else
                            {
                                bool flag4 = xmlNode3.Name == "f";
                                if (flag4)
                                {
                                    num3 = int.Parse(xmlNode3.InnerText);
                                }
                                else
                                {
                                    bool flag5 = xmlNode3.Name == "s";
                                    if (flag5)
                                    {
                                        num4 = int.Parse(xmlNode3.InnerText);
                                    }
                                    else
                                    {
                                        bool flag6 = xmlNode3.Name == "g";
                                        if (flag6)
                                        {
                                            num2 = int.Parse(xmlNode3.InnerText);
                                        }
                                        else
                                        {
                                            bool flag7 = xmlNode3.Name == "c";
                                            if (flag7)
                                            {
                                                num5 = int.Parse(xmlNode3.InnerText);
                                            }
                                            else
                                            {
                                                bool flag8 = xmlNode3.Name == "t";
                                                if (flag8)
                                                {
                                                    num6 = int.Parse(xmlNode3.InnerText);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool flag9 = num > 0;
                        if (flag9)
                        {
                            text += string.Format("民忠 +{0} ", num);
                        }
                        bool flag10 = num2 > 0;
                        if (flag10)
                        {
                            text += string.Format("金币 +{0} ", num2);
                        }
                        bool flag11 = num5 > 0;
                        if (flag11)
                        {
                            text += string.Format("银币 +{0} ", num5);
                        }
                        bool flag12 = num4 > 0;
                        if (flag12)
                        {
                            text += string.Format("威望 +{0} ", num4);
                        }
                        bool flag13 = num3 > 0;
                        if (flag13)
                        {
                            text += string.Format("征收 +{0} ", num3);
                        }
                        bool flag14 = num6 > 0;
                        if (flag14)
                        {
                            text += string.Format("CD -{0} ", num6);
                        }
                    }
                }
                result = text;
            }
            return result;
        }

        private int getImposeEvents(string name, int min_loyalty, int now_loyalty)
        {
            Dictionary<string, MiscMgr.ImposeEvent> allEvents = this.getAllEvents();
            MiscMgr.ImposeEvent imposeEvent = null;
            if (allEvents.ContainsKey(name))
            {
                imposeEvent = allEvents[name];
            }
            if (imposeEvent == null)
            {
                return 0;
            }
            else if (now_loyalty > min_loyalty)
            {
                return imposeEvent.other_selection;
            }
            else
            {
                return imposeEvent.loyalty_selection;
            }
        }

        private Dictionary<string, MiscMgr.ImposeEvent> getAllEvents()
        {
            return new Dictionary<string, MiscMgr.ImposeEvent>
			{
				{
					"八风暴袭",
					new MiscMgr.ImposeEvent("八风暴袭", "以民为本，赈灾安民[民忠 +7]", "国威为重，押解充军[威望 +Lv*10，CD -10]", 1, 1)
				},
				{
					"棒打鸳鸯",
					new MiscMgr.ImposeEvent("棒打鸳鸯", "秉公执法，差人劝阻[民忠 +8]", "大局为重，私下调停[银币 +Lv*30]", 1, 2)
				},
				{
					"不肖子孙",
					new MiscMgr.ImposeEvent("不肖子孙", "姿心所欲，当街暴打[民忠 +4]", "秉公执法，戒律学堂[民忠 +5]", 2, 2)
				},
				{
					"村南一霸",
					new MiscMgr.ImposeEvent("村南一霸", "秉公执法，差人捉拿[民忠 +5]", "事必躬亲，带队出征[民忠 +7]", 2, 2)
				},
				{
					"敌国灾民",
					new MiscMgr.ImposeEvent("敌国灾民", "以民为本，安抚收容[民忠 +4]", "国威为重，差人掠夺[银币 +Lv*25]", 1, 2)
				},
				{
					"敌国逃兵",
					new MiscMgr.ImposeEvent("敌国逃兵", "国威为重，就地正法[威望 +Lv*4]", "大局为重，招安收编[银币 +Lv*25]", 2, 2)
				},
				{
					"敌国乱民",
					new MiscMgr.ImposeEvent("敌国乱民", "国威为重，临阵诛杀[银币 +Lv*30]", "以民为本，招安收编[民忠 +8]", 2, 1)
				},
				{
					"敌国伤兵",
					new MiscMgr.ImposeEvent("敌国伤兵", "以民为本，安顿救治[民忠 +7]", "国威为重，就地正法[威望 +Lv*4]", 1, 1)
				},
				{
					"地牛翻身",
					new MiscMgr.ImposeEvent("地牛翻身", "以民为本，赈灾安民[民忠 +8]", "大局为重，远道求援[银币 +Lv*30]", 1, 2)
				},
				{
					"独眼石人",
					new MiscMgr.ImposeEvent("独眼石人", "国威为重，差人夺取[威望 +Lv*3]", "以民为本，堂宣明理[民忠 +7]", 2, 2)
				},
				{
					"恶霸事件",
					new MiscMgr.ImposeEvent("恶霸事件", "升堂传唤[]", "私下调停[]", 1, 1)
				},
				{
					"风雨飘摇",
					new MiscMgr.ImposeEvent("风雨飘摇", "以民为本，征民修葺[民忠 +5]", "事必躬亲，拆到重建[民忠 +8，银币 -Lv*20]", 2, 2)
				},
				{
					"风雨祠堂",
					new MiscMgr.ImposeEvent("风雨祠堂", "事必躬亲，修堂祭祖[民忠 +7，金币 +7]", "公务缠身，征民修葺[威望 +Lv*7]", 1, 1)
				},
				{
					"飞蝗泛滥",
					new MiscMgr.ImposeEvent("飞蝗泛滥", "事必躬亲，食蝗励民[民忠 +5，威望 +Lv,银币 +Lv*10]", "以民为本，设坛祭天[民忠 +4]", 1, 1)
				},
				{
					"丰收之年",
					new MiscMgr.ImposeEvent("丰收之年", "正常税负[]", "加收税负[]", 1, 1)
				},
				{
					"官民同庆",
					new MiscMgr.ImposeEvent("官民同庆", "以民为本，开倉放粮[民忠 +8]", "恣心所欲，开怀畅饮[民忠 +5 CD -10]", 1, 1)
				},
				{
					"孤儿寡母",
					new MiscMgr.ImposeEvent("孤儿寡母", "事必躬亲，躬身询问[民忠 +7]", "公务缠身，差人安抚[威望 +Lv*3]", 1, 1)
				},
				{
					"荒山沃土",
					new MiscMgr.ImposeEvent("荒山沃土", "事必躬亲，迁寨耕田[民忠 +5，征收 +1]", "公务缠身，征民开荒[征收 +1]", 1, 1)
				},
				{
					"黄巾军事件",
					new MiscMgr.ImposeEvent("黄巾军事件", "身先士卒[]", "公务缠身[]", 2, 2)
				},
				{
					"黄巾来犯",
					new MiscMgr.ImposeEvent("黄巾来犯", "事必躬亲，身先士卒[民忠 +7]", "以民为本，招安收编[民忠 +5，银币 +Lv*25]", 1, 2)
				},
				{
					"黄巾逃兵",
					new MiscMgr.ImposeEvent("黄巾逃兵", "国威为重，就地正法[威望 +Lv*4]", "以民为本，招安收编[民忠 +5]", 2, 2)
				},
				{
					"黑衣刺客",
					new MiscMgr.ImposeEvent("黑衣刺客", "国威为重，诛其九族[威望 +Lv*7，银币 +Lv*30]", "以民为本，减免税赋[民忠 +8]", 2, 1)
				},
				{
					"河堤决口",
					new MiscMgr.ImposeEvent("河堤决口", "以民为本，赈灾安民[民忠 +5]", "国威为重，押解充军[银币 +Lv*35]", 1, 2)
				},
				{
					"虎啸山林",
					new MiscMgr.ImposeEvent("虎啸山林", "事必躬亲，身先士卒[民忠 +5]", "公务缠身[威望 +Lv*3]", 1, 1)
				},
				{
					"江洋大盗",
					new MiscMgr.ImposeEvent("江洋大盗", "公务缠身，差人捕捉[威望 +Lv*5]", "事必躬亲，身先士卒[民忠 +5]", 2, 2)
				},
				{
					"井水犯浑",
					new MiscMgr.ImposeEvent("井水犯浑", "以民为本，召集民众[民忠 +5，金币 +5]", "姿心所欲，提水澄清[民忠 +8]", 1, 1)
				},
				{
					"金枝醉打",
					new MiscMgr.ImposeEvent("金枝醉打", "秉公执法，上前劝阻[民忠 +5]", "大局为重，私下调停[金币 +10]", 2, 2)
				},
				{
					"路遇逃兵",
					new MiscMgr.ImposeEvent("路遇逃兵", "以民为本，押回大营[民忠 +8]", "国威为重，就地正法[银币 +Lv*15]", 1, 2)
				},
				{
					"路遇饥民",
					new MiscMgr.ImposeEvent("路遇饥民", "以民为本，赈灾放粮[民忠 +4]", "大局为重，驱往他方[银币 +Lv*10]", 1, 2)
				},
				{
					"路遇马均",
					new MiscMgr.ImposeEvent("路遇马均", "恣心所欲，盛情款待[民忠 +4，银币 -Lv*10，征收+1]", "礼贤下士，挽留录用[威望 +Lv*2，金币 +10]", 2, 2)
				},
				{
					"路遇凤姐",
					new MiscMgr.ImposeEvent("路遇凤姐", "礼贤下士，貌相清奇[威望 +Lv]", "姿心所欲，实话实说[征收 +1]", 2, 2)
				},
				{
					"路遇左慈",
					new MiscMgr.ImposeEvent("路遇左慈", "礼贤下士，下马问安[威望 +Lv*7，银币 +Lv*30，征收+1]", "国威为重，挽留录用[金币 +5，CD -10，征收 +1]", 2, 2)
				},
				{
					"六月飞雪",
					new MiscMgr.ImposeEvent("六月飞雪", "以民为本，微服私访[民忠 +8]", "国威为重，设坛祭天[威望 +Lv*3]", 1, 1)
				},
				{
					"蒙面人",
					new MiscMgr.ImposeEvent("蒙面人", "秉公执法，公堂审问[民忠 +7]", "恣心所欲，暗地跟踪[金币 +5]", 2, 2)
				},
				{
					"魅影妖姬",
					new MiscMgr.ImposeEvent("魅影妖姬", "秉公执法，严刑审问[民忠 +7]", "恣心所欲，法外开恩[金币 +5]", 2, 2)
				},
				{
					"泥泞古道",
					new MiscMgr.ImposeEvent("泥泞古道", "以民为本，艰难跋涉[民忠 +8]", "国威为重，率民休切[威望 +Lv]", 1, 1)
				},
				{
					"女飞贼",
					new MiscMgr.ImposeEvent("女飞贼", "严刑审问[]", "法外开恩[]", 1, 1)
				},
				{
					"清官服",
					new MiscMgr.ImposeEvent("清官服", "礼贤下士，下马接受[威望 +Lv*3]", "恣心所欲，当众穿戴[民忠 +7]", 2, 2)
				},
				{
					"强抢民女",
					new MiscMgr.ImposeEvent("强抢民女", "秉公执法，押上公堂[民忠 +4]", "大局为重，差人劝阻[银币 +Lv*20]", 1, 2)
				},
				{
					"欺行霸市",
					new MiscMgr.ImposeEvent("欺行霸市", "秉公执法，升堂传呼[民忠 +4]", "大局为重，私下调停[银币 +Lv*25]", 1, 2)
				},
				{
					"山贼来袭",
					new MiscMgr.ImposeEvent("山贼来袭", "事必躬亲，身先士卒[民忠 +7]", "公务缠身，派兵镇压[威望 +Lv*3]", 1, 1)
				},
				{
					"山崩地裂",
					new MiscMgr.ImposeEvent("山崩地裂", "国威为重，率民迁安[威望 +Lv*3]", "以民为本，开倉放粮[民忠 +7，银币 -Lv*20]", 2, 2)
				},
				{
					"硕鼠硕鼠",
					new MiscMgr.ImposeEvent("硕鼠硕鼠", "公务缠身，差人灭鼠[威望 +Lv*4]", "事必躬亲，抓猫治鼠[民忠 +5，银币 +Lv*5]", 2, 2)
				},
				{
					"使臣进献",
					new MiscMgr.ImposeEvent("使臣进献", "礼贤下士，宅内宴请[威望 +Lv*2]", "大局为重，公堂静候[银币 +Lv*35]", 2, 2)
				},
				{
					"铁口仙翁",
					new MiscMgr.ImposeEvent("铁口仙翁", "大局为重，暗中处理[金币 +3]", "秉公执法，公堂审问[民忠 +5，银币 +Lv*10]", 1, 1)
				},
				{
					"逃兵事件",
					new MiscMgr.ImposeEvent("逃兵事件", "押回营中[]", "就地正法[]", 1, 1)
				},
				{
					"天雷地火",
					new MiscMgr.ImposeEvent("天雷地火", "事必躬亲，率人救火[民忠 +7]", "公务缠身，呼民自救[银币 +Lv*10]", 1, 2)
				},
				{
					"台风暴袭",
					new MiscMgr.ImposeEvent("台风暴袭", "赈灾安民[]", "押解充军[]", 1, 1)
				},
				{
					"瘟疫泛滥",
					new MiscMgr.ImposeEvent("瘟疫泛滥", "以民为本，求医问药[民忠 +6]", "大局为重，隔离疫民[银币 +Lv*30]", 1, 2)
				},
				{
					"吾国伤兵",
					new MiscMgr.ImposeEvent("吾国伤兵", "以民为本，安顿救治[民忠 +5]", "大局为重，遣散回家[银币 +Lv*15]", 1, 2)
				},
				{
					"乌线追风",
					new MiscMgr.ImposeEvent("乌线追风", "次心所欲，捉来泡酒[银币 +Lv*30]", "事必躬亲，挥刀斩蛇[民忠 +7]", 2, 1)
				},
				{
					"五谷丰登",
					new MiscMgr.ImposeEvent("五谷丰登", "以民为本，正常税负[民忠 +4，威望 +Lv*5，征收 +1]", "恣心所欲，加收税负[银币 +Lv*35]", 1, 1)
				},
				{
					"为民祈雨",
					new MiscMgr.ImposeEvent("为民祈雨", "大局为重，言说明理[民忠 +4，威望 +Lv*2，银币 +Lv*15]", "事必躬亲，设坛祈雨[民忠 +7]", 2, 1)
				},
				{
					"西域番僧",
					new MiscMgr.ImposeEvent("西域番僧", "恣心所欲，驱散民众[CD -10]", "礼贤下士，旁列静听[金币 +3]", 2, 2)
				},
				{
					"西域来使",
					new MiscMgr.ImposeEvent("西域来使", "礼贤下士，宅内宴请[民忠 +5]", "国威为重，公堂静候[威望 +Lv*3]", 1, 1)
				},
				{
					"抓壮丁",
					new MiscMgr.ImposeEvent("抓壮丁", "国威为重，放任不管[威望 +Lv*5，CD -30]", "以民为本，呵诉阻止[民忠 +7]", 2, 2)
				},
				{
					"珍禽异兽",
					new MiscMgr.ImposeEvent("珍禽异兽", "国威为重，上山围捕[威望 +Lv*7，银币 +Lv*40]", "以民为本，军民勿扰[金币 +10，民忠 +4，征收 +1]", 2, 2)
				},
				{
					"越狱事件",
					new MiscMgr.ImposeEvent("越狱事件", "同流合污[]", "[]", 1, 1)
				}
			};
        }

        public bool handleOfficerInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/officer.action";
            ServerResult xml = protocol.getXml(url, "获取玩家官职信息");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "savesalary_cd" && xmlNode2.InnerText == "0";
                    if (flag2)
                    {
                        this.saveSalary(protocol, logger);
                    }
                }
                result = true;
            }
            return result;
        }

        private bool saveSalary(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/officer!saveSalary.action";
            ServerResult xml = protocol.getXml(url, "领取俸禄");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                int num = 0;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "gain";
                    if (flag2)
                    {
                        int.TryParse(xmlNode2.InnerText, out num);
                    }
                }
                base.logInfo(logger, string.Format("领取俸禄 [ {0}银币 ]", num));
                result = true;
            }
            return result;
        }

        public void buySpecialItem(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/market!buySupperMarketSpecialGoods.action";
            string data = "commodityId=1";
            ServerResult serverResult = protocol.postXml(url, data, "购买集市特供商品");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                base.logInfo(logger, "购买集市特供商品, 花费3000万银币购买999宝石");
            }
        }

        public void getDailyExtraItems(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/market!supplementSupperMarket.action";
            ServerResult xml = protocol.getXml(url, "获取集市信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                base.logInfo(logger, "使用集市进货令, 增加集市货物");
            }
        }

        public int handleMarketInfo(ProtocolMgr protocol, ILogger logger, string configstr, bool _notbuy_if_fail, bool drop_notbuy, bool buy_super, int silver_available, int gold_available, bool use_token, bool use_token_after_five)
        {
            List<MiscMgr.MarketConfig> list = new List<MiscMgr.MarketConfig>();
            string[] array = configstr.Split(new char[] { ',' });
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                string[] array3 = text.Split(new char[] { ':' });
                if (array3.Length >= 3)
                {
                    MiscMgr.MarketConfig marketConfig = new MiscMgr.MarketConfig();
                    marketConfig.item_type = array3[0];
                    int min_quality = -1;
                    if (!int.TryParse(array3[1], out min_quality))
                    {
                        min_quality = 5;
                    }
                    marketConfig.min_quality = min_quality;
                    marketConfig.use_gold = array3[2].ToLower().Equals("true");
                    list.Add(marketConfig);
                }
            }
            string url = "/root/market!getPlayerSupperMarket.action";
            ServerResult xml = protocol.getXml(url, "获取集市信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            List<MiscMgr.MarketItem> list2 = new List<MiscMgr.MarketItem>();
            List<MiscMgr.MarketItem> list3 = new List<MiscMgr.MarketItem>();
            bool state = false;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/special/state");
            if (xmlNode != null)
            {
                state = (xmlNode.InnerText == "1");
            }
            if ((state & buy_super) && silver_available >= 30000000)
            {
                this.buySpecialItem(protocol, logger);
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/suppermarketdto");
            foreach (XmlNode xmlNode2 in xmlNodeList)
            {
                XmlNodeList childNodes = xmlNode2.ChildNodes;
                MiscMgr.MarketItem marketItem = new MiscMgr.MarketItem();
                foreach (XmlNode xmlNode3 in childNodes)
                {
                    if (xmlNode3.Name == "id")
                    {
                        marketItem.id = int.Parse(xmlNode3.InnerText);
                    }
                    else if (xmlNode3.Name == "price")
                    {
                        string[] array4 = xmlNode3.InnerText.Split(new char[] { ':' });
                        marketItem.restype = array4[1];
                        marketItem.resnum = int.Parse(array4[2]);
                    }
                    else if (xmlNode3.Name == "name")
                    {
                        marketItem.itemname = xmlNode3.InnerText;
                    }
                    else if (xmlNode3.Name == "num")
                    {
                        marketItem.num = int.Parse(xmlNode3.InnerText);
                    }
                    else if (xmlNode3.Name == "quality")
                    {
                        marketItem.quality = int.Parse(xmlNode3.InnerText);
                    }
                    else if (xmlNode3.Name == "type")
                    {
                        marketItem.type = int.Parse(xmlNode3.InnerText);
                    }
                    else if (xmlNode3.Name == "baoshinum")
                    {
                        marketItem.gem_num = int.Parse(xmlNode3.InnerText);
                    }
                    else if (xmlNode3.Name == "discountnum")
                    {
                        marketItem.discount_num = int.Parse(xmlNode3.InnerText);
                    }
                }
                int num2 = this.ifBuyItem(marketItem, list, silver_available, gold_available);
                if (num2 == 0)
                {
                    list2.Add(marketItem);
                }
                else if (num2 == 1)
                {
                    list3.Add(marketItem);
                }
            }
            int count = xmlNodeList.Count;
            bool supplementnum = false;
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/supplementnum");
            if (xmlNode4 != null)
            {
                supplementnum = (xmlNode4.InnerText != "0");
            }
            if ((use_token & supplementnum) && count < 20)
            {
                DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
                if (!use_token_after_five || dateTimeNow.Hour > 5)
                {
                    this.getDailyExtraItems(protocol, logger);
                }
            }
            XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/giftdto");
            if (xmlNode5 != null)
            {
                XmlNodeList childNodes2 = xmlNode5.ChildNodes;
                MiscMgr.MarketItem marketItem2 = new MiscMgr.MarketItem();
                foreach (XmlNode xmlNode6 in childNodes2)
                {
                    if (xmlNode6.Name == "id")
                    {
                        marketItem2.id = int.Parse(xmlNode6.InnerText);
                    }
                    else if (xmlNode6.Name == "name")
                    {
                        marketItem2.itemname = xmlNode6.InnerText;
                    }
                    else if (xmlNode6.Name == "num")
                    {
                        marketItem2.num = int.Parse(xmlNode6.InnerText);
                    }
                    else if (xmlNode6.Name == "type")
                    {
                        marketItem2.type = int.Parse(xmlNode6.InnerText);
                    }
                    else if (xmlNode6.Name == "baoshinum")
                    {
                        marketItem2.gem_num = int.Parse(xmlNode6.InnerText);
                    }
                }
                this.getGiftItem(protocol, logger, marketItem2);
            }
            if (drop_notbuy)
            {
                for (int j = 0; j < list3.Count; j++)
                {
                    this.dropItem(protocol, logger, list3[j]);
                }
            }
            if (list2.Count == 0)
            {
                return 2;
            }
            else if (list2.Count > 0)
            {
                MiscMgr.MarketItem marketItem3 = list2[0];
                int num3 = 0;
                for (int i = 0; i < 3 && marketItem3.discount_num <= 3 && marketItem3.discount_num >= 0; i++)
                {
                    num3 = this.discountItem(protocol, logger, marketItem3);
                    if (num3 != 0)
                    {
                        break;
                    }
                }
                if (_notbuy_if_fail)
                {
                    if (num3 == 0)
                    {
                        int num5 = this.buyItem(protocol, logger, marketItem3);
                        if (num5 > 0)
                        {
                            return num5;
                        }
                    }
                }
                else
                {
                    int num6 = this.buyItem(protocol, logger, marketItem3);
                    if (num6 > 0)
                    {
                        return num6;
                    }
                }
            }
            return 0;
        }

        private int ifBuyItem(MiscMgr.MarketItem item, List<MiscMgr.MarketConfig> itemConfs, int silver_available, int gold_available)
        {
            string[] array = new string[]
			{
				"彤云角",
				"红衣将军炮",
				"化骨丛棘",
				"弑履岩",
				"玉麟笙麾",
				"铜人计里车",
				"化骨悬灯"
			};
            string[] array2 = new string[]
			{
				"玄霆角",
				"无敌将军炮",
				"五毒问心钉",
				"七戮锋",
				"蟠龙华盖",
				"轩辕指南车",
				"落魂冥灯"
			};
            string restype = item.restype;
            string text = "";
            bool flag = item.type != 0;
            if (flag)
            {
                int i = 0;
                int num = array2.Length;
                while (i < num)
                {
                    bool flag2 = array2[i] == item.itemname;
                    if (flag2)
                    {
                        text = "purpleweapon";
                        break;
                    }
                    int num2 = i;
                    i = num2 + 1;
                }
                bool flag3 = text == "";
                if (flag3)
                {
                    int j = 0;
                    int num3 = array.Length;
                    while (j < num3)
                    {
                        bool flag4 = array[j] == item.itemname;
                        if (flag4)
                        {
                            text = "redweapon";
                            break;
                        }
                        int num2 = j;
                        j = num2 + 1;
                    }
                }
            }
            else
            {
                text = item.itemname;
            }
            foreach (MiscMgr.MarketConfig current in itemConfs)
            {
                if (current.item_type.Equals(text) && current.min_quality <= item.quality)
                {
                    if (restype.Equals("copper"))
                    {
                        if (silver_available >= item.resnum)
                        {
                            return 0;
                        }
                    }
                    else if (restype.Equals("gold"))
                    {
                        if (current.use_gold)
                        {
                            if (gold_available >= item.resnum)
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    return 2;
                }
            }
            return 1;
        }

        private int buyItem(ProtocolMgr protocol, ILogger logger, MiscMgr.MarketItem item)
        {
            string url = "/root/market!buySupperMarketCommodity.action";
            string data = "commodityId=" + item.id;
            string text = string.Format("使用[{0}{1}]购买[{2}{3}]", new object[]
			{
				CommonUtils.getShortReadable((long)item.resnum),
				item.restype_name,
				(item.gem_num > 0) ? item.gem_num : item.num,
				item.itemname
			});
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    base.logInfo(logger, text);
                    result = 0;
                }
            }
            return result;
        }

        private int getGiftItem(ProtocolMgr protocol, ILogger logger, MiscMgr.MarketItem item)
        {
            string url;
            string text;
            if (item.id == 22)
            {
                url = "/root/market!abandonSupperMarketGift.action";
                text = string.Format("放弃赠送的商品[{0}{1}]", (item.gem_num > 0) ? item.gem_num : item.num, item.itemname);
            }
            else
            {
                url = "/root/market!recvSupperMarketGift.action";
                text = string.Format("获取赠送的商品[{0}{1}]", (item.gem_num > 0) ? item.gem_num : item.num, item.itemname);
            }
            ServerResult xml = protocol.getXml(url, text);
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    base.logInfo(logger, text);
                    result = 0;
                }
            }
            return result;
        }

        private int dropItem(ProtocolMgr protocol, ILogger logger, MiscMgr.MarketItem item)
        {
            string url = "/root/market!offSupperMarketCommodity.action";
            string data = "commodityId=" + item.id;
            string text = string.Format("下架商品[{0}{1}]-->[{2}{3}]", new object[]
			{
				CommonUtils.getShortReadable((long)item.resnum),
				item.restype_name,
				(item.gem_num > 0) ? item.gem_num : item.num,
				item.itemname
			});
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    base.logInfo(logger, text);
                    result = 0;
                }
            }
            return result;
        }

        private int discountItem(ProtocolMgr protocol, ILogger logger, MiscMgr.MarketItem item)
        {
            bool flag = item == null;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                bool flag2 = item.discount_num == 3;
                if (flag2)
                {
                    result = 0;
                }
                else
                {
                    string url = "/root/market!bargainSupperMarketCommodity.action";
                    string data = "commodityId=" + item.id;
                    string text = string.Format("对[{0}{1}]=>[{2}{3}]砍价", new object[]
					{
						CommonUtils.getShortReadable((long)item.resnum),
						item.restype_name,
						(item.gem_num > 0) ? item.gem_num : item.num,
						item.itemname
					});
                    ServerResult serverResult = protocol.postXml(url, data, text);
                    bool flag3 = serverResult == null;
                    if (flag3)
                    {
                        result = 1;
                    }
                    else
                    {
                        bool flag4 = !serverResult.CmdSucceed;
                        if (flag4)
                        {
                            result = -1;
                        }
                        else
                        {
                            XmlDocument cmdResult = serverResult.CmdResult;
                            int discount_num = 3;
                            int num = 0;
                            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/discount");
                            int.TryParse(xmlNode.InnerText, out discount_num);
                            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/nowprice");
                            int.TryParse(xmlNode2.InnerText, out num);
                            item.discount_num = discount_num;
                            bool flag5 = item.resnum > num;
                            item.resnum = num;
                            text = string.Format("{0}, {1}, 价格变为{2}", text, flag5 ? "成功" : "失败", CommonUtils.getShortReadable((long)num));
                            base.logInfo(logger, text);
                            bool flag6 = flag5;
                            if (flag6)
                            {
                                result = 0;
                            }
                            else
                            {
                                result = -1;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int handleStockInfo(ProtocolMgr protocol, ILogger logger, string configstr, User user)
        {
            int num = 70;
            int num2 = 130;
            List<int> list = new List<int>();
            List<MiscMgr.StockItem> list2 = new List<MiscMgr.StockItem>();
            string[] array = configstr.Split(new char[]
			{
				','
			});
            string[] array2 = array;
            int num3;
            for (int i = 0; i < array2.Length; i = num3 + 1)
            {
                string text = array2[i];
                bool flag = !(text == "");
                if (flag)
                {
                    int item = int.Parse(text);
                    bool flag2 = !list.Contains(item);
                    if (flag2)
                    {
                        list.Add(item);
                    }
                }
                num3 = i;
            }
            int num4 = 0;
            int num5 = 0;
            int num6 = -1;
            int num7 = -1;
            string url = "/root/stock.action";
            ServerResult xml = protocol.getXml(url, "获取贸易信息");
            bool flag3 = xml == null;
            int result;
            if (flag3)
            {
                result = 1;
            }
            else
            {
                bool flag4 = !xml.CmdSucceed;
                if (flag4)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        bool flag5 = xmlNode2.Name == "currentpositon";
                        if (flag5)
                        {
                            num4 = int.Parse(xmlNode2.InnerText);
                        }
                        else
                        {
                            bool flag6 = xmlNode2.Name == "postion";
                            if (flag6)
                            {
                                num5 = int.Parse(xmlNode2.InnerText);
                            }
                            else
                            {
                                bool flag7 = xmlNode2.Name == "cd";
                                if (flag7)
                                {
                                    int.TryParse(xmlNode2.InnerText, out num6);
                                }
                                else
                                {
                                    bool flag8 = xmlNode2.Name == "cantrade";
                                    if (flag8)
                                    {
                                        int.TryParse(xmlNode2.InnerText, out num7);
                                    }
                                    else
                                    {
                                        bool flag9 = xmlNode2.Name == "g1" || xmlNode2.Name == "g2" || xmlNode2.Name == "g3" || xmlNode2.Name == "g4" || xmlNode2.Name == "g5" || xmlNode2.Name == "g6" || xmlNode2.Name == "g7";
                                        if (flag9)
                                        {
                                            XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                                            MiscMgr.StockItem stockItem = new MiscMgr.StockItem();
                                            foreach (XmlNode xmlNode3 in childNodes2)
                                            {
                                                bool flag10 = xmlNode3.Name == "goodid";
                                                if (flag10)
                                                {
                                                    stockItem.id = int.Parse(xmlNode3.InnerText);
                                                }
                                                else
                                                {
                                                    bool flag11 = xmlNode3.Name == "name";
                                                    if (flag11)
                                                    {
                                                        stockItem.name = xmlNode3.InnerText;
                                                    }
                                                    else
                                                    {
                                                        bool flag12 = xmlNode3.Name == "price";
                                                        if (flag12)
                                                        {
                                                            stockItem.now_price = int.Parse(xmlNode3.InnerText);
                                                        }
                                                        else
                                                        {
                                                            bool flag13 = xmlNode3.Name == "originalprice";
                                                            if (flag13)
                                                            {
                                                                stockItem.orig_price = int.Parse(xmlNode3.InnerText);
                                                            }
                                                            else
                                                            {
                                                                bool flag14 = xmlNode3.Name == "storage";
                                                                if (flag14)
                                                                {
                                                                    stockItem.storage = int.Parse(xmlNode3.InnerText);
                                                                }
                                                                else
                                                                {
                                                                    bool flag15 = xmlNode3.Name == "num";
                                                                    if (flag15)
                                                                    {
                                                                        stockItem.num_per_buy = int.Parse(xmlNode3.InnerText);
                                                                    }
                                                                    else
                                                                    {
                                                                        bool flag16 = xmlNode3.Name == "cd";
                                                                        if (flag16)
                                                                        {
                                                                            stockItem.cd = int.Parse(xmlNode3.InnerText);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            list2.Add(stockItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    user._stock_cd = num6;
                    bool flag17 = num6 > 0;
                    if (flag17)
                    {
                        result = 3;
                    }
                    else
                    {
                        bool flag18 = num7 == 0;
                        if (flag18)
                        {
                            bool flag19 = num6 > 0;
                            if (flag19)
                            {
                                result = 3;
                            }
                            else
                            {
                                result = 4;
                            }
                        }
                        else
                        {
                            bool flag20 = false;
                            foreach (MiscMgr.StockItem current in list2)
                            {
                                bool flag21 = list.Contains(current.id);
                                if (flag21)
                                {
                                    bool flag22 = current.percent >= num2;
                                    if (flag22)
                                    {
                                        bool flag23 = current.storage > 0;
                                        if (flag23)
                                        {
                                            this.sell_buy_item(protocol, logger, true, current.id, current.name, 1, current.num_per_buy, current.now_price * current.num_per_buy);
                                            flag20 = true;
                                            user._stock_cd = current.cd * 60 * 1000 / 10;
                                        }
                                    }
                                    else
                                    {
                                        bool flag24 = current.percent <= num && num4 + current.num_per_buy <= num5;
                                        if (flag24)
                                        {
                                            this.sell_buy_item(protocol, logger, false, current.id, current.name, 1, current.num_per_buy, current.now_price * current.num_per_buy);
                                            flag20 = true;
                                            user._stock_cd = current.cd * 60 * 1000;
                                        }
                                    }
                                    bool flag25 = flag20;
                                    if (flag25)
                                    {
                                        break;
                                    }
                                }
                            }
                            bool flag26 = flag20;
                            if (flag26)
                            {
                                result = 3;
                            }
                            else
                            {
                                result = 0;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void sell_buy_item(ProtocolMgr protocol, ILogger logger, bool sell, int goodsId, string name, int count, int num_per_sellbuy, int silver_total)
        {
            string url = "/root/stock!tradeStock.action";
            string text = "";
            if (sell)
            {
                text += "tradeType=1&";
            }
            else
            {
                text += "tradeType=0&";
            }
            object obj = text;
            text = string.Concat(new object[]
			{
				obj,
				"goodId=",
				goodsId,
				"&num=",
				count
			});
            string text2 = string.Format("{0}[{1}] * {2}, 银币{3}{4}", new object[]
			{
				sell ? "卖出" : "买入",
				name,
				num_per_sellbuy,
				sell ? "+" : "-",
				silver_total
			});
            ServerResult serverResult = protocol.postXml(url, text, text2);
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, text2);
            }
        }

        public int handleBaoshiStoneInfo(ProtocolMgr protocol, ILogger logger, int gather_percent)
        {
            List<MiscMgr.StoneQueue> list = new List<MiscMgr.StoneQueue>();
            string url = "/root/outCity!getPickSpace.action";
            ServerResult xml = protocol.getXml(url, "获取宝石采集队列信息");
            int result;
            if (xml == null)
            {
                result = 1;
            }
            else if (!xml.CmdSucceed)
            {
                result = 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    if (xmlNode2.Name == "playerpickdto")
                    {
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        MiscMgr.StoneQueue stoneQueue = new MiscMgr.StoneQueue();
                        foreach (XmlNode xmlNode3 in childNodes2)
                        {
                            if (xmlNode3.Name == "id")
                            {
                                stoneQueue.id = int.Parse(xmlNode3.InnerText);
                            }
                            else if (xmlNode3.Name == "output")
                            {
                                stoneQueue.now_stone = int.Parse(xmlNode3.InnerText);
                            }
                            else if (xmlNode3.Name == "limit")
                            {
                                stoneQueue.limit_stone = int.Parse(xmlNode3.InnerText);
                            }
                            else if (xmlNode3.Name == "canendpick")
                            {
                                stoneQueue.can_gather = (xmlNode3.InnerText == "2");
                            }
                        }
                        list.Add(stoneQueue);
                    }
                }
                foreach (MiscMgr.StoneQueue current in list)
                {
                    if (current.percent >= gather_percent)
                    {
                        this.gatherBaoshiStone(protocol, logger, current.id);
                    }
                }
                result = 0;
            }
            return result;
        }

        private void gatherBaoshiStone(ProtocolMgr protocol, ILogger logger, int queueId)
        {
            string url = "/root/outCity!endBaoshiPick.action";
            string data = "pickSpaceId=" + queueId;
            string text = string.Format("采集队列 [{0}] 的宝石, ", queueId);
            ServerResult serverResult = protocol.postXml(url, data, text);
            if (serverResult != null && serverResult.CmdSucceed)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                int baoshinum = 0;
                int baoji = 0;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    if (xmlNode2.Name == "baoshi")
                    {
                        baoshinum = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "baoji")
                    {
                        baoji = int.Parse(xmlNode2.InnerText);
                    }
                }
                if (baoji != 0)
                {
                    text += "采集暴击, ";
                }
                text += string.Format("获得宝石 {0} ", baoshinum);
                base.logInfo(logger, text);
            }
        }

        public int handleStoneInfo(ProtocolMgr protocol, ILogger logger, int gather_percent)
        {
            List<MiscMgr.StoneQueue> list = new List<MiscMgr.StoneQueue>();
            string url = "/root/outCity!getPickSpace.action";
            ServerResult xml = protocol.getXml(url, "获取玉石采集队列信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        bool flag3 = xmlNode2.Name == "playerpickdto";
                        if (flag3)
                        {
                            XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                            MiscMgr.StoneQueue stoneQueue = new MiscMgr.StoneQueue();
                            foreach (XmlNode xmlNode3 in childNodes2)
                            {
                                bool flag4 = xmlNode3.Name == "id";
                                if (flag4)
                                {
                                    stoneQueue.id = int.Parse(xmlNode3.InnerText);
                                }
                                else
                                {
                                    bool flag5 = xmlNode3.Name == "output";
                                    if (flag5)
                                    {
                                        stoneQueue.now_stone = int.Parse(xmlNode3.InnerText);
                                    }
                                    else
                                    {
                                        bool flag6 = xmlNode3.Name == "limit";
                                        if (flag6)
                                        {
                                            stoneQueue.limit_stone = int.Parse(xmlNode3.InnerText);
                                        }
                                        else
                                        {
                                            bool flag7 = xmlNode3.Name == "canendpick";
                                            if (flag7)
                                            {
                                                stoneQueue.can_gather = (xmlNode3.InnerText == "1");
                                            }
                                        }
                                    }
                                }
                            }
                            list.Add(stoneQueue);
                        }
                    }
                    foreach (MiscMgr.StoneQueue current in list)
                    {
                        bool flag8 = current.percent >= gather_percent;
                        if (flag8)
                        {
                            this.gatherStone(protocol, logger, current.id);
                        }
                    }
                    result = 0;
                }
            }
            return result;
        }

        private void gatherStone(ProtocolMgr protocol, ILogger logger, int queueId)
        {
            string url = "/root/outCity!endPick.action";
            string data = "pickSpaceId=" + queueId;
            string text = string.Format("采集队列 [{0}] 的玉石, ", queueId);
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "addbowlder";
                    if (flag2)
                    {
                        num = int.Parse(xmlNode2.InnerText);
                    }
                    else
                    {
                        bool flag3 = xmlNode2.Name == "baoshinum";
                        if (flag3)
                        {
                            num2 = int.Parse(xmlNode2.InnerText);
                        }
                        else
                        {
                            bool flag4 = xmlNode2.Name == "surplus";
                            if (flag4)
                            {
                                num3 = int.Parse(xmlNode2.InnerText);
                            }
                            else
                            {
                                bool flag5 = xmlNode2.Name == "baoji";
                                if (flag5)
                                {
                                    num4 = int.Parse(xmlNode2.InnerText);
                                }
                            }
                        }
                    }
                }
                bool flag6 = num4 != 0;
                if (flag6)
                {
                    text += "采集暴击, ";
                }
                text += string.Format("获得原石 {0} ", num);
                bool flag7 = num3 > 0;
                if (flag7)
                {
                    text += string.Format("余料 {0} ", num3);
                }
                bool flag8 = num2 > 0;
                if (flag8)
                {
                    text += string.Format("宝石 {0}", num2);
                }
                base.logInfo(logger, text);
            }
        }

        public void getStoneMineInfo(ProtocolMgr protocol, ILogger logger, int scopeId)
        {
            string url = "/root/bowlderRes!getBowlderResInfo.action";
            string data = "scopeId=" + scopeId;
            ServerResult serverResult = protocol.postXml(url, data, "获取玉石矿信息");
            bool flag = serverResult != null;
            if (flag)
            {
                bool cmdSucceed = serverResult.CmdSucceed;
            }
        }

        private void attackStoneMine(ProtocolMgr protocol, ILogger logger, int scopeId, int mineId)
        {
            string url = "/root/bowlderRes!attackBowlderRes.action";
            string data = string.Format("resId={1}&scopeId={0}", scopeId, mineId);
            ServerResult serverResult = protocol.postXml(url, data, "攻打玉石矿");
            bool flag = serverResult != null;
            if (flag)
            {
                bool cmdSucceed = serverResult.CmdSucceed;
            }
        }

        private void rushHarvestStoneMine(ProtocolMgr protocol, ILogger logger, int scopeId, int mineId)
        {
            string url = "/root/bowlderRes!rushHarvestBowlderRes.action";
            string data = string.Format("resId={1}&scopeId={0}", scopeId, mineId);
            ServerResult serverResult = protocol.postXml(url, data, "抢收玉石矿");
            bool flag = serverResult != null;
            if (flag)
            {
                bool cmdSucceed = serverResult.CmdSucceed;
            }
        }

        private void cancelStoneMine(ProtocolMgr protocol, ILogger logger, int scopeId, int mineId)
        {
            string url = "/root/bowlderRes!cancelHarvestBowlderRes.action";
            string data = string.Format("resId={1}&scopeId={0}", scopeId, mineId);
            ServerResult serverResult = protocol.postXml(url, data, "放弃玉石矿");
            bool flag = serverResult != null;
            if (flag)
            {
                bool cmdSucceed = serverResult.CmdSucceed;
            }
        }

        public int handleRawStoneInfo(ProtocolMgr protocol, ILogger logger, int silver_available, bool sell_to_sys, int target_stone, out int stone_gains)
        {
            stone_gains = 0;
            string url = "/root/gamblingStone!getNewPick.action";
            ServerResult xml = protocol.getXml(url, "获取璞玉矿信息");
            int result;
            if (xml == null)
            {
                result = 1;
            }
            else if (!xml.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/rawmaterial");
                if (xmlNodeList == null || xmlNodeList.Count == 0)
                {
                    result = 3;
                }
                else
                {
                    List<MiscMgr.RawStone> list = new List<MiscMgr.RawStone>();
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        MiscMgr.RawStone rawStone = new MiscMgr.RawStone();
                        foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                        {
                            if (xmlNode2.Name == "id")
                            {
                                rawStone.id = int.Parse(xmlNode2.InnerText);
                            }
                            else if (xmlNode2.Name == "name")
                            {
                                rawStone.name = xmlNode2.InnerText;
                            }
                            else if (xmlNode2.Name == "num")
                            {
                                rawStone.num = int.Parse(xmlNode2.InnerText);
                            }
                            else if (xmlNode2.Name == "state")
                            {
                                rawStone.state = int.Parse(xmlNode2.InnerText);
                            }
                            else if (xmlNode2.Name == "quality")
                            {
                                rawStone.quality = int.Parse(xmlNode2.InnerText);
                            }
                            else if (xmlNode2.Name == "price")
                            {
                                rawStone.price = int.Parse(xmlNode2.InnerText);
                            }
                            else if (xmlNode2.Name == "sysprice")
                            {
                                rawStone.sysprice = int.Parse(xmlNode2.InnerText);
                            }
                            else if (xmlNode2.Name == "cutprice")
                            {
                                rawStone.cutprice = int.Parse(xmlNode2.InnerText);
                            }
                        }
                        list.Add(rawStone);
                    }
                    foreach (MiscMgr.RawStone current in list)
                    {
                        while (current.num > 0)
                        {
                            int num3 = 0;
                            if (current.cut_use_gold)
                            {
                                if (sell_to_sys)
                                {
                                    int num4 = this.sell2SysStone(protocol, logger, current.id, current.name, out num3);
                                    if (num4 > 0)
                                    {
                                        return num4;
                                    }
                                    current.num--;
                                }
                            }
                            else
                            {
                                if (current.cutprice > silver_available)
                                {
                                    return 2;
                                }
                                int num7 = this.cutStone(protocol, logger, current.id, current.name, current.cutprice, out num3);
                                if (num7 > 0)
                                {
                                    return num7;
                                }
                                silver_available -= current.cutprice;
                                current.num--;
                            }
                            stone_gains += num3;
                            if (stone_gains >= target_stone)
                            {
                                return 3;
                            }
                        }
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int cutStone(ProtocolMgr protocol, ILogger logger, int id, string name, int price, out int stone_got)
        {
            stone_got = 0;
            string url = "/root/gamblingStone!newCut.action";
            string data = "id=" + id;
            ServerResult serverResult = protocol.postXml(url, data, "切割璞玉");
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                this.handleCutResult(logger, true, string.Format("花费{0}银币, 切割璞玉[{1}]", price, name), cmdResult, out stone_got);
                return 0;
            }
        }

        public int sellStone(ProtocolMgr protocol, ILogger logger, int id, string name, out int stone_got)
        {
            stone_got = 0;
            string url = "/root/gamblingStone!sell.action";
            string data = "id=" + id;
            ServerResult serverResult = protocol.postXml(url, data, "挂牌石头");
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    string text = string.Format("挂牌石头[{0}]", name);
                    base.logInfo(logger, text);
                    result = 0;
                }
            }
            return result;
        }

        public int sell2SysStone(ProtocolMgr protocol, ILogger logger, int id, string name, out int stone_got)
        {
            stone_got = 0;
            string url = "/root/gamblingStone!sellToSys.action";
            string data = "id=" + id;
            ServerResult serverResult = protocol.postXml(url, data, "抛售璞玉");
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                this.handleCutResult(logger, false, string.Format("抛售璞玉[{0}]", name), cmdResult, out stone_got);
                return 0;
            }
        }

        private void handleCutResult(ILogger logger, bool isCut, string prefix, XmlDocument xml, out int stone_got)
        {
            stone_got = 0;
            int price = 0;
            XmlNode xmlNode = xml.SelectSingleNode("/results/price");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out price);
            }
            bool success = false;
            xmlNode = xml.SelectSingleNode("/results/success");
            if (xmlNode != null)
            {
                success = xmlNode.InnerText == "1";
            }
            int product = 0;
            xmlNode = xml.SelectSingleNode("/results/product");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out product);
            }
            string text;
            if (isCut)
            {
                text = string.Format("{0} {1}, 玉石+{2}", prefix, success ? "成功" : "失败", product);
                stone_got = product;
            }
            else
            {
                text = string.Format("{0}, 玉石+{1}", prefix, price);
                stone_got = price;
            }
            base.logInfo(logger, text);
        }

        /// <summary>
        /// 使用商盟之友 1:空值,10:错误,0:成功,2:金币商人全通,3:没有商盟之友
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int handleTradeFriend(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/caravan!getNewTrade.action";
            ServerResult xml = protocol.getXml(url, "获取新通商信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int tradefreind = 0;
                XmlNode tradefreindXmlNode = cmdResult.SelectSingleNode("/results/tradefreind");
                if (tradefreindXmlNode != null)
                {
                    int.TryParse(tradefreindXmlNode.InnerText, out tradefreind);
                }
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/newtrade");
                int[] visitStateArray = new int[12];
                int[] visitFailArray = new int[12];
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    int id = 0;
                    int visitfailnum = 0;
                    int visitstate = 0;
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        if (xmlNode2.Name == "id")
                        {
                            id = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "visitstate")
                        {
                            visitstate = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "visitfailnum")
                        {
                            visitfailnum = int.Parse(xmlNode2.InnerText);
                        }
                    }
                    visitStateArray[id - 1] = visitstate;
                    visitFailArray[id - 1] = visitfailnum;
                }
                if (tradefreind == 1)
                {
                    int[] gold_merchant = new int[3] { 11, 10, 8 };
                    for (int i = 0; i < gold_merchant.Length; i++)
                    {
                        if (visitStateArray[gold_merchant[i] - 1] == 0)
                        {
                            base.logInfo(logger, "使用商盟之友");
                            this.visitNewMerchant(protocol, logger, user, gold_merchant[i]);
                            return 0;
                        }
                    }
                    return 2;
                }
                return 3;
            }
        }

        public void openNewTradeBox(ProtocolMgr protocol, ILogger logger, out int boxnum)
        {
            boxnum = 0;
            string url = "/root/caravan!openNewTradeBox.action";
            ServerResult xml = protocol.getXml(url, "打开通商宝藏宝箱");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/boxreward/baoshi");
                if (xmlNode != null)
                {
                    logInfo(logger, string.Format("打开通商宝藏宝箱，宝石+{0}", xmlNode.InnerText));
                }
                xmlNode = cmdResult.SelectSingleNode("/results/boxnum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out boxnum);
                }
            }
        }

        public int handleNewTradeInfo(ProtocolMgr protocol, ILogger logger, User user, string visit_merchants, int max_fail_count, int silver_available, int gold_available, int limit_active, out int map_count, out int boxnum)
        {
            if (user.Silver < 10000000)
            {
                ticketExchangeMoney(protocol, logger, 3);
            }

            map_count = 0;
            boxnum = 0;

            if (user.Level >= 400)
            {
                return getWesternTradeInfo(protocol, logger, user, limit_active);
            }

            string url = "/root/caravan!getNewTrade.action";
            ServerResult xml = protocol.getXml(url, "获取新通商信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            else
            {
                string[] merchants = visit_merchants.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> visitMerchantsArray = new List<int>();
                foreach (string merchant in merchants)
                {
                    visitMerchantsArray.Add(int.Parse(merchant));
                }
                XmlDocument cmdResult = xml.CmdResult;
                int tradefreind = 0;
                XmlNode tradefreindXmlNode = cmdResult.SelectSingleNode("/results/tradefreind");
                if (tradefreindXmlNode != null)
                {
                    int.TryParse(tradefreindXmlNode.InnerText, out tradefreind);
                }
                XmlNode boxnumXmlNode = cmdResult.SelectSingleNode("/results/boxinfo/boxnum");
                if (boxnumXmlNode != null)
                {
                    int.TryParse(boxnumXmlNode.InnerText, out boxnum);
                }
                int canabroud = 0;
                XmlNode canabroudXmlNode = cmdResult.SelectSingleNode("/results/canabroud");
                if (canabroudXmlNode != null)
                {
                    int.TryParse(canabroudXmlNode.InnerText, out canabroud);
                }
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/newtrade");
                int[] visitStateArray = new int[12];
                int[] visitFailArray = new int[12];
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    int id = 0;
                    int visitfailnum = 0;
                    int visitstate = 0;
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        if (xmlNode2.Name == "id")
                        {
                            id = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "visitstate")
                        {
                            visitstate = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "visitfailnum")
                        {
                            visitfailnum = int.Parse(xmlNode2.InnerText);
                        }
                    }
                    visitStateArray[id - 1] = visitstate;
                    visitFailArray[id - 1] = visitfailnum;
                }
                if (canabroud == 1)
                {
                    if (user.CurMovable < 60)
                    {
                        return 2;
                    }
                    else if (abroudTrade(protocol, logger, user))
                    {
                        return 0;
                    }
                    else
                    {
                        return 10;
                    }
                }
                if (tradefreind == 1)
                {
                    int[] gold_merchant = new int[3] { 11, 10, 8 };
                    for (int i = 0; i < gold_merchant.Length; i++)
                    {
                        if (visitStateArray[gold_merchant[i] - 1] == 0)
                        {
                            base.logInfo(logger, "使用商盟之友");
                            this.visitNewMerchant(protocol, logger, user, gold_merchant[i]);
                            break;
                        }
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    //if (visit_merchants.Contains((i + 1).ToString()))
                    if (visitMerchantsArray.Contains(i + 1))
                    {
                        int run_time = 30;
                        while (visitStateArray[i] == 0 && visitFailArray[i] < max_fail_count && user.CurMovable >= 10 && run_time > 0)
                        {
                            run_time--;
                            int ret = this.visitNewMerchant(protocol, logger, user, i + 1);
                            if (ret >= 1)
                            {
                                return 10;
                            }
                            else if (ret == -1)
                            {
                                visitFailArray[i]++;
                            }
                            else if (ret == 0)
                            {
                                visitStateArray[i] = 1;
                                break;
                            }
                        }
                    }
                }
                bool canTrade = true;
                for (int j = 0; j < 12; j++)
                {
                    //if (visit_merchants.Contains((j + 1).ToString()) && visitStateArray[j] == 0 && visitFailArray[j] < max_fail_count)
                    if (visitMerchantsArray.Contains(j + 1) && visitStateArray[j] == 0 && visitFailArray[j] < max_fail_count)
                    {
                        canTrade = false;
                        break;
                    }
                }
                if (!canTrade)
                {
                    return 2;
                }
                else if (user.CurMovable < 60)
                {
                    return 2;
                }
                else if (this.doNewTrade(protocol, logger, user, out map_count) > 0)
                {
                    return 10;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool abroudTrade(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/caravan!abroudTrade.action";
            ServerResult xml = protocol.getXml(url, "海外通商");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("海外通商，获得{0}", reward.ToString()));
            if (!openAbroudTradeBox(protocol, logger, user, 0))
            {
                return false;
            }

            return true;
        }

        public bool openAbroudTradeBox(ProtocolMgr protocol, ILogger logger, User user, int abroud)
        {
            string url = "/root/caravan!openAbroudTradeBox.action";
            string data = string.Format("abroud={0}", abroud);
            ServerResult xml = protocol.postXml(url, data, "海外通商 - 再开一次");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            return true;
        }

        public int doNewTrade(ProtocolMgr protocol, ILogger logger, User user, out int map_count)
        {
            map_count = 0;
            string url = "/root/caravan!doNewTrade.action";
            ServerResult xml = protocol.getXml(url, "新通商");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int baoshi = 0;
                int cri = 1;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out baoshi);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cri");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode.InnerText, out cri);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/map");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out map_count);
                }
                int quality = 0;
                int getbox = 0;
                int boxnum = 0;
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/specialreward");
                if (xmlNode4 != null)
                {
                    XmlNodeList childNodes = xmlNode4.ChildNodes;
                    foreach (XmlNode xmlNode5 in childNodes)
                    {
                        if (xmlNode5.Name == "quality")
                        {
                            quality = int.Parse(xmlNode5.InnerText);
                        }
                        else if (xmlNode5.Name == "getbox")
                        {
                            getbox = int.Parse(xmlNode5.InnerText);
                        }
                        else if (xmlNode5.Name == "boxnum")
                        {
                            boxnum = int.Parse(xmlNode5.InnerText);
                        }
                    }
                }
                string text = string.Format("新版通商成功, 获得宝石+{0}{1}{2}", baoshi, (map_count > 0) ? (", 考古藏宝图+" + map_count) : "", (getbox > 0) ? (", 宝藏+" + getbox) : "");
                base.logInfo(logger, text);
                return 0;
            }
        }

        public int visitNewMerchant(ProtocolMgr protocol, ILogger logger, User user, int merchantId)
        {
            string[] array = new string[]
			{
				"楼兰商盟",
				"西域商盟",
				"巴蜀商盟",
				"大理商盟",
				"闽南商盟",
				"辽东商盟",
				"关东商盟",
				"淮南商盟",
				"荆楚商盟",
				"南越商盟",
				"浔阳商盟",
				"岭南商盟",
			};
            string url = "/root/caravan!newTradeVisit.action";
            string data = "caravanId=" + merchantId;
            ServerResult serverResult = protocol.postXml(url, data, "拜访商盟");
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/succ");
                if (xmlNode == null)
                {
                    return -1;
                }
                else if (xmlNode.InnerText == "1")
                {
                    base.logInfo(logger, string.Format("拜访[{0}]成功了, 天灵灵地灵灵~~", array[merchantId - 1]));
                    return 0;
                }
                else
                {
                    base.logInfo(logger, string.Format("拜访[{0}]失败了?? 您比习老板还忙啊?!!", array[merchantId - 1]));
                    return -1;
                }
            }
        }

        public int handleTradeInfo(ProtocolMgr protocol, ILogger logger, User user, out int state, bool do_tired_trade = false, bool only_free = true)
        {
            if (user.Silver < 10000000)
            {
                ticketExchangeMoney(protocol, logger, 3);
            }

            state = 0;
            bool flag = user.Level < 40;
            int result;
            if (flag)
            {
                result = 2;
            }
            else
            {
                Building building = this._factory.getBuildingManager().getBuilding(user._buildings, "驿站");
                bool flag2 = building != null && building.Level >= 200;
                if (flag2)
                {
                    user._is_new_trade = true;
                    result = 2;
                }
                else
                {
                    List<string> merchants = this.getMerchants();
                    List<MiscMgr.TradeMerchant> list = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list2 = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list3 = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list4 = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list5 = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list6 = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list7 = new List<MiscMgr.TradeMerchant>();
                    List<MiscMgr.TradeMerchant> list8 = new List<MiscMgr.TradeMerchant>();
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    string url = "/root/caravan!getTradeInfo.action";
                    ServerResult xml = protocol.getXml(url, "获取通商信息");
                    bool flag3 = xml == null;
                    if (flag3)
                    {
                        result = 1;
                    }
                    else
                    {
                        bool flag4 = !xml.CmdSucceed;
                        if (flag4)
                        {
                            result = 10;
                        }
                        else
                        {
                            XmlDocument cmdResult = xml.CmdResult;
                            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost1");
                            bool flag5 = xmlNode != null;
                            if (flag5)
                            {
                                int.TryParse(xmlNode.InnerText, out num2);
                            }
                            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cost2");
                            bool flag6 = xmlNode2 != null;
                            if (flag6)
                            {
                                int.TryParse(xmlNode2.InnerText, out num3);
                            }
                            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/status");
                            bool flag7 = xmlNode3 != null;
                            if (flag7)
                            {
                                int.TryParse(xmlNode3.InnerText, out num4);
                            }
                            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/remainhigh");
                            bool flag8 = xmlNode4 != null;
                            if (flag8)
                            {
                                int.TryParse(xmlNode4.InnerText, out num5);
                            }
                            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/card");
                            bool flag9 = xmlNodeList != null;
                            IEnumerator enumerator;
                            if (flag9)
                            {
                                enumerator = xmlNodeList.GetEnumerator();
                                try
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        XmlNode xmlNode5 = (XmlNode)enumerator.Current;
                                        bool flag10 = xmlNode5 != null && xmlNode5.HasChildNodes;
                                        if (flag10)
                                        {
                                            XmlNode xmlNode6 = xmlNode5.SelectSingleNode("cardnum");
                                            bool flag11 = xmlNode6 != null;
                                            if (flag11)
                                            {
                                                int.TryParse(xmlNode6.InnerText, out num6);
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    IDisposable disposable = enumerator as IDisposable;
                                    bool flag12 = disposable != null;
                                    if (flag12)
                                    {
                                        disposable.Dispose();
                                    }
                                }
                            }
                            XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results");
                            XmlNodeList childNodes = xmlNode7.ChildNodes;
                            enumerator = childNodes.GetEnumerator();
                            try
                            {
                                while (enumerator.MoveNext())
                                {
                                    XmlNode xmlNode8 = (XmlNode)enumerator.Current;
                                    bool flag13 = xmlNode8.Name == "tradetimes";
                                    if (flag13)
                                    {
                                        num = int.Parse(xmlNode8.InnerText);
                                    }
                                    else
                                    {
                                        bool flag14 = xmlNode8.Name == "tradecombodto";
                                        if (flag14)
                                        {
                                            XmlNodeList childNodes2 = xmlNode8.ChildNodes;
                                            int num7 = 0;
                                            string text = "";
                                            foreach (XmlNode xmlNode9 in childNodes2)
                                            {
                                                bool flag15 = xmlNode9.Name == "type";
                                                if (flag15)
                                                {
                                                    num7 = int.Parse(xmlNode9.InnerText);
                                                }
                                                else
                                                {
                                                    bool flag16 = xmlNode9.Name == "info";
                                                    if (flag16)
                                                    {
                                                        text = xmlNode9.InnerText;
                                                    }
                                                }
                                            }
                                            text = text.Substring(0, text.IndexOf(":"));
                                            string[] array = text.Split(new char[]
											{
												','
											});
                                            bool flag17 = num7 == 1;
                                            if (flag17)
                                            {
                                                bool flag18 = array.Length != 0 && array[0] != "";
                                                if (flag18)
                                                {
                                                    string[] array2 = array[0].Split(new char[]
													{
														'+'
													});
                                                    string[] array3 = array2;
                                                    int num8;
                                                    for (int i = 0; i < array3.Length; i = num8 + 1)
                                                    {
                                                        string text2 = array3[i];
                                                        list.Add(new MiscMgr.TradeMerchant
                                                        {
                                                            name = text2,
                                                            id = merchants.IndexOf(text2) + 1
                                                        });
                                                        num8 = i;
                                                    }
                                                }
                                                bool flag19 = array.Length > 1 && array[1] != "";
                                                if (flag19)
                                                {
                                                    string[] array4 = array[1].Split(new char[]
													{
														'+'
													});
                                                    string[] array5 = array4;
                                                    int num8;
                                                    for (int j = 0; j < array5.Length; j = num8 + 1)
                                                    {
                                                        string text3 = array5[j];
                                                        list2.Add(new MiscMgr.TradeMerchant
                                                        {
                                                            name = text3,
                                                            id = merchants.IndexOf(text3) + 1
                                                        });
                                                        num8 = j;
                                                    }
                                                }
                                                bool flag20 = array.Length > 2 && array[2] != "";
                                                if (flag20)
                                                {
                                                    string[] array6 = array[2].Split(new char[]
													{
														'+'
													});
                                                    string[] array7 = array6;
                                                    int num8;
                                                    for (int k = 0; k < array7.Length; k = num8 + 1)
                                                    {
                                                        string text4 = array7[k];
                                                        list3.Add(new MiscMgr.TradeMerchant
                                                        {
                                                            name = text4,
                                                            id = merchants.IndexOf(text4) + 1
                                                        });
                                                        num8 = k;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bool flag21 = num7 == 2;
                                                if (flag21)
                                                {
                                                    bool flag22 = array.Length != 0 && array[0] != "";
                                                    if (flag22)
                                                    {
                                                        string[] array8 = array[0].Split(new char[]
														{
															'+'
														});
                                                        string[] array9 = array8;
                                                        int num8;
                                                        for (int l = 0; l < array9.Length; l = num8 + 1)
                                                        {
                                                            string text5 = array9[l];
                                                            list4.Add(new MiscMgr.TradeMerchant
                                                            {
                                                                name = text5,
                                                                id = merchants.IndexOf(text5) + 1
                                                            });
                                                            num8 = l;
                                                        }
                                                    }
                                                    bool flag23 = array.Length > 1 && array[1] != "";
                                                    if (flag23)
                                                    {
                                                        string[] array10 = array[1].Split(new char[]
														{
															'+'
														});
                                                        string[] array11 = array10;
                                                        int num8;
                                                        for (int m = 0; m < array11.Length; m = num8 + 1)
                                                        {
                                                            string text6 = array11[m];
                                                            list5.Add(new MiscMgr.TradeMerchant
                                                            {
                                                                name = text6,
                                                                id = merchants.IndexOf(text6) + 1
                                                            });
                                                            num8 = m;
                                                        }
                                                    }
                                                    bool flag24 = array.Length > 2 && array[2] != "";
                                                    if (flag24)
                                                    {
                                                        string[] array12 = array[2].Split(new char[]
														{
															'+'
														});
                                                        string[] array13 = array12;
                                                        int num8;
                                                        for (int n = 0; n < array13.Length; n = num8 + 1)
                                                        {
                                                            string text7 = array13[n];
                                                            list6.Add(new MiscMgr.TradeMerchant
                                                            {
                                                                name = text7,
                                                                id = merchants.IndexOf(text7) + 1
                                                            });
                                                            num8 = n;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            bool flag25 = xmlNode8.Name == "refresh";
                                            if (flag25)
                                            {
                                                XmlNodeList childNodes3 = xmlNode8.ChildNodes;
                                                MiscMgr.TradeMerchant tradeMerchant = new MiscMgr.TradeMerchant();
                                                foreach (XmlNode xmlNode10 in childNodes3)
                                                {
                                                    bool flag26 = xmlNode10.Name == "textilemerchantid";
                                                    if (flag26)
                                                    {
                                                        tradeMerchant.id = int.Parse(xmlNode10.InnerText);
                                                    }
                                                    else
                                                    {
                                                        bool flag27 = xmlNode10.Name == "name";
                                                        if (flag27)
                                                        {
                                                            tradeMerchant.name = xmlNode10.InnerText;
                                                        }
                                                        else
                                                        {
                                                            bool flag28 = xmlNode10.Name == "state";
                                                            if (flag28)
                                                            {
                                                                tradeMerchant.discovered = (xmlNode10.InnerText == "2");
                                                            }
                                                        }
                                                    }
                                                }
                                                list7.Add(tradeMerchant);
                                            }
                                            else
                                            {
                                                bool flag29 = xmlNode8.Name == "recent";
                                                if (flag29)
                                                {
                                                    XmlNodeList childNodes4 = xmlNode8.ChildNodes;
                                                    MiscMgr.TradeMerchant tradeMerchant2 = new MiscMgr.TradeMerchant();
                                                    foreach (XmlNode xmlNode11 in childNodes4)
                                                    {
                                                        bool flag30 = xmlNode11.Name == "textilemerchantid";
                                                        if (flag30)
                                                        {
                                                            tradeMerchant2.id = int.Parse(xmlNode11.InnerText);
                                                        }
                                                        else
                                                        {
                                                            bool flag31 = xmlNode11.Name == "name";
                                                            if (flag31)
                                                            {
                                                                tradeMerchant2.name = xmlNode11.InnerText;
                                                            }
                                                        }
                                                    }
                                                    list8.Add(tradeMerchant2);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable3 = enumerator as IDisposable;
                                bool flag32 = disposable3 != null;
                                if (flag32)
                                {
                                    disposable3.Dispose();
                                }
                            }
                            state = num4;
                            bool flag33 = num4 == 3;
                            if (flag33)
                            {
                                result = 2;
                            }
                            else
                            {
                                bool usecard = num6 > 0;
                                bool useCard = false;
                                bool flag34 = num > 0;
                                if (flag34)
                                {
                                    base.logInfo(logger, "发现免费通商, 开启通商");
                                }
                                bool flag35 = num == 0;
                                if (flag35)
                                {
                                    if (only_free)
                                    {
                                        result = 0;
                                        return result;
                                    }
                                    bool flag36 = !do_tired_trade && num4 != 1;
                                    if (flag36)
                                    {
                                        result = 2;
                                        return result;
                                    }
                                    bool flag37 = num4 == 1 && user.CurMovable < num2;
                                    if (flag37)
                                    {
                                        result = 3;
                                        return result;
                                    }
                                    bool flag38 = num4 == 2 && user.CurMovable < num3;
                                    if (flag38)
                                    {
                                        result = 3;
                                        return result;
                                    }
                                }
                                int merchantToTrade = this.getMerchantToTrade(usecard, out useCard, list, list2, list3, list4, list5, list6, list7, list8);
                                bool flag39 = merchantToTrade == 0;
                                if (flag39)
                                {
                                    result = 10;
                                }
                                else
                                {
                                    this.trade(protocol, logger, merchantToTrade, merchants[merchantToTrade - 1], useCard);
                                    result = 0;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void trade(ProtocolMgr protocol, ILogger logger, int merchantId, string name, bool useCard)
        {
            string url = "/root/caravan!trade.action";
            string text = "caravanId=" + merchantId;
            if (useCard)
            {
                text += "&usecard=1";
            }
            string text2 = string.Format("{0}通商[{1}]", useCard ? "使用[商人召唤卡]," : "", name);
            ServerResult serverResult = protocol.postXml(url, text, text2);
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gold");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    text2 += string.Format(", 银币+{0}", xmlNode.InnerText);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/goldcombo");
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    text2 += string.Format(", 完成银币事件, 获得{0}银币", xmlNode2.InnerText);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/jewelcombo");
                bool flag4 = xmlNode3 != null;
                if (flag4)
                {
                    text2 += string.Format(", 完成宝石事件, 获得宝石{0}个", xmlNode3.InnerText);
                }
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/bowldercombo");
                bool flag5 = xmlNode4 != null;
                if (flag5)
                {
                    text2 += string.Format(", 完成玉石事件, 获得玉石{0}个", xmlNode4.InnerText);
                }
                base.logInfo(logger, text2);
            }
        }

        private int getMerchantToTrade(bool usecard, out bool card_used, List<MiscMgr.TradeMerchant> _event_1_silver, List<MiscMgr.TradeMerchant> _event_1_stone, List<MiscMgr.TradeMerchant> _event_1_gem, List<MiscMgr.TradeMerchant> _event_2_silver, List<MiscMgr.TradeMerchant> _event_2_stone, List<MiscMgr.TradeMerchant> _event_2_gem, List<MiscMgr.TradeMerchant> _refresh_merchants, List<MiscMgr.TradeMerchant> _recent_merchants)
        {
            card_used = false;
            bool flag = _event_2_gem.Count > 0 && _recent_merchants.Count > 0;
            int result;
            if (flag)
            {
                bool flag2 = _recent_merchants.Count == 1;
                if (flag2)
                {
                    bool flag3 = _event_2_gem[0].id == _recent_merchants[0].id;
                    if (flag3)
                    {
                        int num = 0;
                        foreach (MiscMgr.TradeMerchant current in _refresh_merchants)
                        {
                            bool flag4 = current.id == _event_2_gem[1].id;
                            if (flag4)
                            {
                                num = current.id;
                                break;
                            }
                        }
                        bool flag5 = num > 0;
                        if (flag5)
                        {
                            result = num;
                            return result;
                        }
                    }
                }
                else
                {
                    bool flag6 = _recent_merchants.Count >= 2;
                    if (flag6)
                    {
                        bool flag7 = _event_2_gem[0].id == _recent_merchants[_recent_merchants.Count - 2].id && _event_2_gem[1].id == _recent_merchants[_recent_merchants.Count - 1].id;
                        if (flag7)
                        {
                            int num2 = 0;
                            foreach (MiscMgr.TradeMerchant current2 in _refresh_merchants)
                            {
                                bool flag8 = current2.id == _event_2_gem[2].id;
                                if (flag8)
                                {
                                    num2 = current2.id;
                                    break;
                                }
                            }
                            bool flag9 = num2 > 0;
                            if (flag9)
                            {
                                result = num2;
                                return result;
                            }
                            if (usecard)
                            {
                                card_used = true;
                                result = _event_2_gem[2].id;
                                return result;
                            }
                        }
                        else
                        {
                            bool flag10 = _event_2_gem[0].id == _recent_merchants[_recent_merchants.Count - 1].id;
                            if (flag10)
                            {
                                int num3 = 0;
                                foreach (MiscMgr.TradeMerchant current3 in _refresh_merchants)
                                {
                                    bool flag11 = current3.id == _event_2_gem[1].id;
                                    if (flag11)
                                    {
                                        num3 = current3.id;
                                        break;
                                    }
                                }
                                bool flag12 = num3 > 0;
                                if (flag12)
                                {
                                    result = num3;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            bool flag13 = _event_1_gem.Count > 0 && _recent_merchants.Count > 0 && _event_1_gem[0].id == _recent_merchants[_recent_merchants.Count - 1].id;
            if (flag13)
            {
                int num4 = 0;
                foreach (MiscMgr.TradeMerchant current4 in _refresh_merchants)
                {
                    bool flag14 = current4.id == _event_1_gem[1].id;
                    if (flag14)
                    {
                        num4 = current4.id;
                        break;
                    }
                }
                bool flag15 = num4 > 0;
                if (flag15)
                {
                    result = num4;
                    return result;
                }
                if (usecard)
                {
                    card_used = true;
                    result = _event_1_gem[1].id;
                    return result;
                }
            }
            bool flag16 = _event_2_silver.Count > 0 && _recent_merchants.Count > 0;
            if (flag16)
            {
                bool flag17 = _recent_merchants.Count == 1;
                if (flag17)
                {
                    bool flag18 = _event_2_silver[0].id == _recent_merchants[0].id;
                    if (flag18)
                    {
                        int num5 = 0;
                        foreach (MiscMgr.TradeMerchant current5 in _refresh_merchants)
                        {
                            bool flag19 = current5.id == _event_2_silver[1].id;
                            if (flag19)
                            {
                                num5 = current5.id;
                                break;
                            }
                        }
                        bool flag20 = num5 > 0;
                        if (flag20)
                        {
                            result = num5;
                            return result;
                        }
                    }
                }
                else
                {
                    bool flag21 = _recent_merchants.Count >= 2;
                    if (flag21)
                    {
                        bool flag22 = _event_2_silver[0].id == _recent_merchants[_recent_merchants.Count - 2].id && _event_2_silver[1].id == _recent_merchants[_recent_merchants.Count - 1].id;
                        if (flag22)
                        {
                            int num6 = 0;
                            foreach (MiscMgr.TradeMerchant current6 in _refresh_merchants)
                            {
                                bool flag23 = current6.id == _event_2_silver[2].id;
                                if (flag23)
                                {
                                    num6 = current6.id;
                                    break;
                                }
                            }
                            bool flag24 = num6 > 0;
                            if (flag24)
                            {
                                result = num6;
                                return result;
                            }
                            if (usecard)
                            {
                                card_used = true;
                                result = _event_2_silver[2].id;
                                return result;
                            }
                        }
                        else
                        {
                            bool flag25 = _event_2_silver[0].id == _recent_merchants[_recent_merchants.Count - 1].id;
                            if (flag25)
                            {
                                int num7 = 0;
                                foreach (MiscMgr.TradeMerchant current7 in _refresh_merchants)
                                {
                                    bool flag26 = current7.id == _event_2_silver[1].id;
                                    if (flag26)
                                    {
                                        num7 = current7.id;
                                        break;
                                    }
                                }
                                bool flag27 = num7 > 0;
                                if (flag27)
                                {
                                    result = num7;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            bool flag28 = _event_1_silver.Count > 0 && _recent_merchants.Count > 0 && _event_1_silver[0].id == _recent_merchants[_recent_merchants.Count - 1].id;
            if (flag28)
            {
                int num8 = 0;
                foreach (MiscMgr.TradeMerchant current8 in _refresh_merchants)
                {
                    bool flag29 = current8.id == _event_1_silver[1].id;
                    if (flag29)
                    {
                        num8 = current8.id;
                        break;
                    }
                }
                bool flag30 = num8 > 0;
                if (flag30)
                {
                    result = num8;
                    return result;
                }
                if (usecard)
                {
                    card_used = true;
                    result = _event_1_silver[1].id;
                    return result;
                }
            }
            bool flag31 = _event_2_stone.Count > 0 && _recent_merchants.Count > 0;
            if (flag31)
            {
                bool flag32 = _recent_merchants.Count == 1;
                if (flag32)
                {
                    bool flag33 = _event_2_stone[0].id == _recent_merchants[0].id;
                    if (flag33)
                    {
                        int num9 = 0;
                        foreach (MiscMgr.TradeMerchant current9 in _refresh_merchants)
                        {
                            bool flag34 = current9.id == _event_2_stone[1].id;
                            if (flag34)
                            {
                                num9 = current9.id;
                                break;
                            }
                        }
                        bool flag35 = num9 > 0;
                        if (flag35)
                        {
                            result = num9;
                            return result;
                        }
                    }
                }
                else
                {
                    bool flag36 = _recent_merchants.Count >= 2;
                    if (flag36)
                    {
                        bool flag37 = _event_2_stone[0].id == _recent_merchants[_recent_merchants.Count - 2].id && _event_2_stone[1].id == _recent_merchants[_recent_merchants.Count - 1].id;
                        if (flag37)
                        {
                            int num10 = 0;
                            foreach (MiscMgr.TradeMerchant current10 in _refresh_merchants)
                            {
                                bool flag38 = current10.id == _event_2_stone[2].id;
                                if (flag38)
                                {
                                    num10 = current10.id;
                                    break;
                                }
                            }
                            bool flag39 = num10 > 0;
                            if (flag39)
                            {
                                result = num10;
                                return result;
                            }
                            if (usecard)
                            {
                                card_used = true;
                                result = _event_2_stone[2].id;
                                return result;
                            }
                        }
                        else
                        {
                            bool flag40 = _event_2_stone[0].id == _recent_merchants[_recent_merchants.Count - 1].id;
                            if (flag40)
                            {
                                int num11 = 0;
                                foreach (MiscMgr.TradeMerchant current11 in _refresh_merchants)
                                {
                                    bool flag41 = current11.id == _event_2_stone[1].id;
                                    if (flag41)
                                    {
                                        num11 = current11.id;
                                        break;
                                    }
                                }
                                bool flag42 = num11 > 0;
                                if (flag42)
                                {
                                    result = num11;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            bool flag43 = _event_1_stone.Count > 0 && _recent_merchants.Count > 0 && _event_1_stone[0].id == _recent_merchants[_recent_merchants.Count - 1].id;
            if (flag43)
            {
                int num12 = 0;
                foreach (MiscMgr.TradeMerchant current12 in _refresh_merchants)
                {
                    bool flag44 = current12.id == _event_1_stone[1].id;
                    if (flag44)
                    {
                        num12 = current12.id;
                        break;
                    }
                }
                bool flag45 = num12 > 0;
                if (flag45)
                {
                    result = num12;
                    return result;
                }
                if (usecard)
                {
                    card_used = true;
                    result = _event_1_stone[1].id;
                    return result;
                }
            }
            int num13 = 0;
            foreach (MiscMgr.TradeMerchant current13 in _refresh_merchants)
            {
                bool flag46 = !current13.discovered;
                if (flag46)
                {
                    num13 = current13.id;
                    break;
                }
            }
            bool flag47 = num13 > 0;
            if (flag47)
            {
                result = num13;
            }
            else
            {
                int num14 = 0;
                bool flag48 = _event_2_gem.Count > 0;
                if (flag48)
                {
                    foreach (MiscMgr.TradeMerchant current14 in _refresh_merchants)
                    {
                        bool flag49 = current14.id == _event_2_gem[0].id;
                        if (flag49)
                        {
                            num14 = current14.id;
                            break;
                        }
                    }
                }
                bool flag50 = num14 > 0;
                if (flag50)
                {
                    result = num14;
                }
                else
                {
                    bool flag51 = _event_1_gem.Count > 0;
                    if (flag51)
                    {
                        foreach (MiscMgr.TradeMerchant current15 in _refresh_merchants)
                        {
                            bool flag52 = current15.id == _event_1_gem[0].id;
                            if (flag52)
                            {
                                num14 = current15.id;
                                break;
                            }
                        }
                    }
                    bool flag53 = num14 > 0;
                    if (flag53)
                    {
                        result = num14;
                    }
                    else
                    {
                        bool flag54 = _event_2_silver.Count > 0;
                        if (flag54)
                        {
                            foreach (MiscMgr.TradeMerchant current16 in _refresh_merchants)
                            {
                                bool flag55 = current16.id == _event_2_silver[0].id;
                                if (flag55)
                                {
                                    num14 = current16.id;
                                    break;
                                }
                            }
                        }
                        bool flag56 = num14 > 0;
                        if (flag56)
                        {
                            result = num14;
                        }
                        else
                        {
                            bool flag57 = _event_1_silver.Count > 0;
                            if (flag57)
                            {
                                foreach (MiscMgr.TradeMerchant current17 in _refresh_merchants)
                                {
                                    bool flag58 = current17.id == _event_1_silver[0].id;
                                    if (flag58)
                                    {
                                        num14 = current17.id;
                                        break;
                                    }
                                }
                            }
                            bool flag59 = num14 > 0;
                            if (flag59)
                            {
                                result = num14;
                            }
                            else
                            {
                                bool flag60 = _event_2_stone.Count > 0;
                                if (flag60)
                                {
                                    foreach (MiscMgr.TradeMerchant current18 in _refresh_merchants)
                                    {
                                        bool flag61 = current18.id == _event_2_stone[0].id;
                                        if (flag61)
                                        {
                                            num14 = current18.id;
                                            break;
                                        }
                                    }
                                }
                                bool flag62 = num14 > 0;
                                if (flag62)
                                {
                                    result = num14;
                                }
                                else
                                {
                                    bool flag63 = _event_1_stone.Count > 0;
                                    if (flag63)
                                    {
                                        foreach (MiscMgr.TradeMerchant current19 in _refresh_merchants)
                                        {
                                            bool flag64 = current19.id == _event_1_stone[0].id;
                                            if (flag64)
                                            {
                                                num14 = current19.id;
                                                break;
                                            }
                                        }
                                    }
                                    bool flag65 = num14 > 0;
                                    if (flag65)
                                    {
                                        result = num14;
                                    }
                                    else
                                    {
                                        result = _refresh_merchants[0].id;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<string> getMerchants()
        {
            return new List<string>
			{
				"楼兰商人",
				"西域胡商",
				"巴蜀商人",
				"大理商人",
				"闽南商人",
				"辽东商人",
				"关东商人",
				"淮南商人",
				"荆楚商人",
				"南越商人",
				"浔阳商人",
				"岭南商人"
			};
        }
        
        private void refreshWorker(ProtocolMgr protocol, ILogger logger, int workerId, int pos, int formerSkillId)
        {
            string url = "/root/make!refreshWeaverAttribute.action";
            string data = string.Format("pos={1}&playerMakerId={0}", workerId, pos);
            string text = string.Format("刷新纺织工技能[{0}] ", MiscMgr.WeaveWorker.getSkillName(formerSkillId));
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/newskill");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    int num = 0;
                    int.TryParse(xmlNode.InnerText, out num);
                    bool flag3 = num == 0;
                    if (flag3)
                    {
                        text += ", 刷新失败, 技能不变";
                    }
                    else
                    {
                        text += string.Format(", 刷新成功, 技能变为[{0}]", MiscMgr.WeaveWorker.getSkillName(num));
                    }
                }
                base.logInfo(logger, text);
            }
        }

        private List<string> getClothes()
        {
            return new List<string>
			{
				"孔羽缭绫",
				"虎纹亚麻",
				"流云壮锦"
			};
        }

        //0:孔羽 1:虎纹 2:流云
        public bool makeRoyaltyWeave(ProtocolMgr protocol, ILogger logger, string bussinessname, int like, out int islike)
        {
            islike = 0;
            string url = "/root/make!royaltyWeave.action";
            string data = string.Format("like={0}", like);
            ServerResult xml = protocol.postXml(url, data, "纺织");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }

            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //islike = lua.GetIntValue("results.like");//0:不喜欢,1:喜欢
            //int weavestate = lua.GetIntValue("results.weavestate");//纺织状态,0:失败
            //int hanganadd = lua.GetIntValue("results.hanganadd");//好感度增加
            //int bussinessdone = lua.GetIntValue("results.bussinessdone");//已通商人数
            //int bussinessdonemax = lua.GetIntValue("results.bussinessdonemax");//最大通商人数
            //int heishi = lua.GetIntValue("results.heishi");//黑市状态,0:没有,1:出现
            //int buyheishicost = lua.GetIntValue("results.buyheishicost");//黑市购买宝箱花费金币数
            //int heishimianfei = lua.GetIntValue("results.heishimianfei");//黑市免费开宝箱次数
            //string heishistate = lua.GetStringValue("results.heishistate");//黑市宝箱开启状态 0,1,0,0,0, 0未开启 1已开启
            islike = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/like"));
            int weavestate = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/weavestate"));
            int hanganadd = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/hanganadd"));
            int bussinessdone = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/bussinessdone"));
            int bussinessdonemax = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/bussinessdonemax"));
            int heishi = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/heishi"));
            int buyheishicost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/buyheishicost"));
            int heishimianfei = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/heishimianfei"));
            string heishistate = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/heishistate"));
            if (weavestate > 1)
            {
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
                logInfo(logger, string.Format("纺织成功，[{0}({1}/{2})]商人好感度增加{3}，获得{4}", bussinessname, bussinessdone, bussinessdonemax, hanganadd, reward.ToString()));
            }
            else
            {
                string likeString = "不喜欢";
                if (islike == 1)
                {
                    likeString = "喜欢";
                }
                logInfo(logger, string.Format("纺织失败，[{0}({1}/{2})]商人{3}{4}", bussinessname, bussinessdone, bussinessdonemax, likeString, getClothes()[like]));
            }
            if (heishi == 1)
            {
                logInfo(logger, "黑市出现");
                while (heishimianfei > 0)
                {
                    if (!makeHeishi(protocol, logger, ref heishimianfei, ref heishistate))
                    {
                        break;
                    }
                }
            }
            return true;
        }

        public bool makeHeishi(ProtocolMgr protocol, ILogger logger, ref int heishimianfei, ref string heishistate)
        {
            int state = 0;
            string[] heishilist = heishistate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < heishilist.Length; i++)
            {
                if (heishilist[i].Equals("0"))
                {
                    state = i;
                    break;
                }
            }
            string url = "/root/make!heishi.action";
            string data = string.Format("heishi={0}", state);
            ServerResult xml = protocol.postXml(url, data, "黑市");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }

            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int heishi = lua.GetIntValue("results.heishi");//黑市状态,0:没有,1:出现
            //int buyheishicost = lua.GetIntValue("results.buyheishicost");//黑市购买宝箱花费金币数
            //heishimianfei = lua.GetIntValue("results.heishimianfei");//黑市免费开宝箱次数
            //heishistate = lua.GetStringValue("results.heishistate");//黑市宝箱开启状态 0,1,0,0,0, 0未开启 1已开启
            int heishi = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/heishi"));
            int buyheishicost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/buyheishicost"));
            heishimianfei = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/heishimianfei"));
            heishistate = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/heishistate"));
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("免费开启黑市宝箱, 获得{0}", reward.ToString()));
            return true;
        }

        //0:success, 1:null, 2:finish, 3:movable, 4:price, 10:error
        public int handleRoyaltyWeaveInfo(ProtocolMgr protocol, ILogger logger, User user, int weave_count, string convert_condition, ref int like, bool do_tired_weave = false)
        {
            string url = "/root/make!royaltyWeaveInfo.action";
            ServerResult xml = protocol.getXml(url, "获取皇家织造厂信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }

            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int activestatus = lua.GetIntValue("results.activestatus");//纺织状态,1:高效,2:疲劳
            //int needactive = lua.GetIntValue("results.needactive");//纺织所需行动力
            //int remainhigh = lua.GetIntValue("results.remainhigh");//高效次数
            //int limit = lua.GetIntValue("results.limit");//极限次数
            //int remainlimit = lua.GetIntValue("results.remainlimit");//剩余极限次数
            //int cost1 = lua.GetIntValue("results.cost1");//高效消耗行动力
            //int cost2 = lua.GetIntValue("results.cost2");//疲劳消耗行动力
            //string bussinessname = lua.GetStringValue("results.bussinessname");//当前商人
            //int bussinessdone = lua.GetIntValue("results.bussinessdone");//已通商人数
            //int bussinessdonemax = lua.GetIntValue("results.bussinessdonemax");//最大通商人数
            //int haogandu = lua.GetIntValue("results.haogandu");//当前商人好感度
            //int haogandumax = lua.GetIntValue("results.haogandumax");//商人最大好感度
            //int heishi = lua.GetIntValue("results.heishi");//黑市状态,0:没有,1:出现
            //int buyheishicost = lua.GetIntValue("results.buyheishicost");//黑市购买宝箱花费金币数
            //int heishimianfei = lua.GetIntValue("results.heishimianfei");//黑市免费开宝箱次数
            //string heishistate = lua.GetStringValue("results.heishistate");//黑市宝箱开启状态 0,1,0,0,0, 0未开启 1已开启
            int activestatus = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/activestatus"));
            int needactive = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/needactive"));
            int remainhigh = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/remainhigh"));
            int limit = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/limit"));
            int remainlimit = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/remainlimit"));
            int cost1 = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/cost1"));
            int cost2 = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/cost2"));
            string bussinessname = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/bussinessname"));
            int bussinessdone = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/bussinessdone"));
            int bussinessdonemax = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/bussinessdonemax"));
            int haogandu = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/haogandu"));
            int haogandumax = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/haogandumax"));
            int heishi = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/heishi"));
            int buyheishicost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/buyheishicost"));
            int heishimianfei = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/heishimianfei"));
            string heishistate = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/heishistate"));

            //换购专属装备
            int oncenum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/oncenum"));
            int maxnum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/maxnum"));
            int weavenum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/weavenum"));
            int needweavenum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/needweavenum"));
            string tradername = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/tradername"));
            RewardInfo rewards = new RewardInfo();
            rewards.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            string[] conditions = convert_condition.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string condition in conditions)
            {
                string[] conds = condition.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (conds.Length == 4)
                {
                    int convert_type = int.Parse(conds[0]);
                    int convert_quality = int.Parse(conds[1]);
                    int convert_lv = int.Parse(conds[2]);
                    int convert_limit = int.Parse(conds[3]);
                    List<Reward> convert_list = rewards.getRewardList(convert_type);
                    foreach (Reward item in convert_list)
                    {
                        if (item.Quality == convert_quality && item.Lv == convert_lv && weavenum >= needweavenum && needweavenum <= convert_limit)
                        {
                            convertRoyaltyWeaveNew(protocol, logger);
                            break;
                        }
                    }
                }
            }

            int use = limit - remainlimit;
            if (remainlimit <= 0)
            {
                return 2;
            }
            else if (activestatus == 2 && !do_tired_weave)
            {
                return 2;
            }
            else if (user.CurMovable < needactive)
            {
                return 3;
            }
            else if (use >= weave_count)
            {
                return 2;
            }

            int islike;
            if (this.makeRoyaltyWeave(protocol, logger, bussinessname, like, out islike))
            {
                user._weave_task_num--;
                if (islike == 0)
                {
                    like = (like + 1) % 3;
                }
                return 0;
            }
            return 10;
        }

        public bool convertRoyaltyWeaveNew(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/make!convertRoyaltyWeaveNew.action";
            ServerResult xml = protocol.getXml(url, "换购");
            if (xml == null || !xml.CmdSucceed) return false;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("换购, 获得{0}", reward.ToString()));
            return true;
        }

        public int handleWeaveInfo(ProtocolMgr protocol, ILogger logger, User user, int weave_price, int weave_count, string convert_condition, out int weave_state, ref int like, bool do_tired_weave = false, bool only_free = true, bool only_task = true)
        {
            weave_state = 0;
            if (user.Level < 82)
            {
                return 2;
            }

            bool isRoyaltyWeave = false;
            foreach (Building building in user._buildings)
            {
                if (building.BuildingId == 24)
                {
                    if (building.State == 1)
                    {
                        isRoyaltyWeave = true;
                    }
                    break;
                }
            }
            if (isRoyaltyWeave)
            {
                return handleRoyaltyWeaveInfo(protocol, logger, user, weave_count, convert_condition, ref like, do_tired_weave);
            }

            string url = "/root/make.action";
            ServerResult xml = protocol.getXml(url, "获取纺织信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }

            List<MiscMgr.WeaveWorker> worker_list = new List<MiscMgr.WeaveWorker>();
            List<MiscMgr.ClothInfo> cloth_list = new List<MiscMgr.ClothInfo>();
            int price = 0;
            bool firstrefresh = false;
            int cardnum = 0;
            int status = -1;
            int cost1 = 10;
            int cost2 = 30;
            int remainhigh = 0;
            int makenum = 0;
            int use = 0;
            int maxdaytimes = 0;
            int alreadytimes = 0;
            int remainlimit = 0;
            int limit = 0;
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost1");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out cost1);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/alreadytimes");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out alreadytimes);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/maxdaytimes");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out maxdaytimes);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/cost2");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out cost2);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/status");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out status);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/remainhigh");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out remainhigh);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/remainlimit");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out remainlimit);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/limit");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out limit);
            }
            use = limit - remainlimit;
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/card");
            if (xmlNodeList != null)
            {
                foreach (XmlNode xmlNode6 in xmlNodeList)
                {
                    if (xmlNode6 != null && xmlNode6.HasChildNodes)
                    {
                        XmlNode xmlNode7 = xmlNode6.SelectSingleNode("cardnum");
                        if (xmlNode7 != null)
                        {
                            int.TryParse(xmlNode7.InnerText, out cardnum);
                        }
                    }
                }
            }
            XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/firstrefresh");
            firstrefresh = (xmlNode8 != null && xmlNode8.InnerText == "1");
            XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode9.ChildNodes;
            int up = 0;
            bool teacher = false;
            foreach (XmlNode xmlNode10 in childNodes)
            {
                if (xmlNode10.Name == "makenum")
                {
                    makenum = int.Parse(xmlNode10.InnerText);
                }
                else if (xmlNode10.Name == "makemax")
                {
                    int.Parse(xmlNode10.InnerText);
                }
                else if (xmlNode10.Name == "price")
                {
                    price = int.Parse(xmlNode10.InnerText);
                }
                else if (xmlNode10.Name == "tech")
                {
                    int.Parse(xmlNode10.InnerText);
                }
                else if (xmlNode10.Name == "up")
                {
                    up = int.Parse(xmlNode10.InnerText);
                }
                else if (xmlNode10.Name == "teacher")
                {
                    teacher = xmlNode10.InnerText.Equals("1");
                }
                else if (xmlNode10.Name == "make")
                {
                    XmlNodeList childNodes2 = xmlNode10.ChildNodes;
                    MiscMgr.ClothInfo clothInfo = new MiscMgr.ClothInfo();
                    foreach (XmlNode xmlNode11 in childNodes2)
                    {
                        if (xmlNode11.Name == "id")
                        {
                            clothInfo.id = int.Parse(xmlNode11.InnerText);
                        }
                        else if (xmlNode11.Name == "type")
                        {
                            clothInfo.type = int.Parse(xmlNode11.InnerText);
                        }
                        else if (xmlNode11.Name == "name")
                        {
                            clothInfo.name = xmlNode11.InnerText;
                        }
                        else if (xmlNode11.Name == "cost")
                        {
                            clothInfo.cost = int.Parse(xmlNode11.InnerText);
                        }
                        else if (xmlNode11.Name == "successp")
                        {
                            clothInfo.success = float.Parse(xmlNode11.InnerText);
                        }
                        else if (xmlNode11.Name == "criticalp")
                        {
                            clothInfo.critical = float.Parse(xmlNode11.InnerText);
                        }
                        else if (xmlNode11.Name == "sellprice")
                        {
                            clothInfo.sellprice = int.Parse(xmlNode11.InnerText);
                        }
                        else if (xmlNode11.Name == "lv")
                        {
                            clothInfo.level = int.Parse(xmlNode11.InnerText);
                        }
                    }
                    cloth_list.Add(clothInfo);
                }
                else if (xmlNode10.Name == "playermaker")
                {
                    XmlNodeList childNodes3 = xmlNode10.ChildNodes;
                    MiscMgr.WeaveWorker weaveWorker = new MiscMgr.WeaveWorker();
                    foreach (XmlNode xmlNode12 in childNodes3)
                    {
                        if (xmlNode12.Name == "id")
                        {
                            weaveWorker.id = int.Parse(xmlNode12.InnerText);
                        }
                        else if (xmlNode12.Name == "skill")
                        {
                            weaveWorker.skill = xmlNode12.InnerText;
                        }
                    }
                    worker_list.Add(weaveWorker);
                }

            }
            if (!teacher)
            {
                base.logInfo(logger, string.Format("当前布价: {0}{1}", price, (up < 0) ? "↓" : "↑"));
            }
            weave_state = status;
            if (firstrefresh)
            {
                int pos = 0;
                int formerSkillId = 0;
                foreach (MiscMgr.WeaveWorker current in worker_list)
                {
                    if (current.haveLowSkill(out pos, out formerSkillId))
                    {
                        this.refreshWorker(protocol, logger, current.id, pos, formerSkillId);
                        break;
                    }
                }
            }

            if (use >= maxdaytimes)
            {
                return 2;
            }
            else if (use > weave_count)
            {
                return 2;
            }
            else if (!teacher && price < weave_price)
            {
                return 4;
            }
            else if (status == 3)
            {
                return 2;
            }
            else if (only_task && user._weave_task_num <= 0)
            {
                return 2;
            }

            if (makenum > 0)
            {
                base.logInfo(logger, "发现免费纺织, 检查是否符合纺织条件");
            }
            if (makenum == 0)
            {
                if (only_free)
                {
                    return 0;
                }
                if (!do_tired_weave && status != 1)
                {
                    return 2;
                }
                if (status == 1 && user.CurMovable < cost1)
                {
                    return 3;
                }
                if (status == 2 && user.CurMovable < cost2)
                {
                    return 3;
                }
            }
            List<MiscMgr.WeaveAssist> assists = null;
            if (!teacher)
            {
                List<MiscMgr.WeaveAssist> weaveAssistInfo = this.getWeaveAssistInfo(protocol, logger);
                assists = this.getAssists(weaveAssistInfo);
                List<MiscMgr.ClothInfo> list3 = this.setAssist(protocol, logger, assists);
                if (list3 != null && list3.Count > 0)
                {
                    cloth_list = list3;
                }
            }
            else
            {
                base.logInfo(logger, "纺织大师, 无需布价, 妙手来钱~~");
            }
            int count = cloth_list.Count;
            for (int i = 0; i < count - 2; i++)
            {
                for (int j = i + 1; j < count - 1; j++)
                {
                    if (cloth_list[i].sellprice < cloth_list[j].sellprice)
                    {
                        MiscMgr.ClothInfo value = cloth_list[i];
                        cloth_list[i] = cloth_list[j];
                        cloth_list[j] = value;
                    }
                }
            }
            MiscMgr.ClothInfo clothInfo2 = null;
            int count2 = cloth_list.Count;
            for (int k = 0; k < count2; k++)
            {
                if ((double)cloth_list[k].success >= 1.0)
                {
                    clothInfo2 = cloth_list[k];
                    break;
                }
            }
            if (clothInfo2 == null)
            {
                clothInfo2 = cloth_list[0];
            }
            if (clothInfo2 != null)
            {
                if (this.make(protocol, logger, clothInfo2, assists, cardnum > 0))
                {
                    user._weave_task_num--;
                    return 0;
                }
                else
                {
                    return 10;
                }
            }
            else
            {
                return 0;
            }
        }

        private List<MiscMgr.WeaveAssist> getWeaveAssistInfo(ProtocolMgr protocol, ILogger logger)
        {
            List<MiscMgr.WeaveAssist> list = new List<MiscMgr.WeaveAssist>();
            string url = "/root/make!getWeaverAssInfo.action";
            ServerResult xml = protocol.getXml(url, "获取纺织辅助人信息");
            bool flag = xml == null || !xml.CmdSucceed;
            List<MiscMgr.WeaveAssist> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "assplayer";
                    if (flag2)
                    {
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        MiscMgr.WeaveAssist weaveAssist = new MiscMgr.WeaveAssist();
                        foreach (XmlNode xmlNode3 in childNodes2)
                        {
                            bool flag3 = xmlNode3.Name == "playerid";
                            if (flag3)
                            {
                                weaveAssist.id = int.Parse(xmlNode3.InnerText);
                            }
                            else
                            {
                                bool flag4 = xmlNode3.Name == "playername";
                                if (flag4)
                                {
                                    weaveAssist.name = xmlNode3.InnerText;
                                }
                                else
                                {
                                    bool flag5 = xmlNode3.Name == "weaverlevel";
                                    if (flag5)
                                    {
                                        weaveAssist.level = int.Parse(xmlNode3.InnerText);
                                    }
                                }
                            }
                        }
                        list.Add(weaveAssist);
                    }
                }
                result = list;
            }
            return result;
        }

        private List<MiscMgr.ClothInfo> setAssist(ProtocolMgr protocol, ILogger logger, List<MiscMgr.WeaveAssist> assists)
        {
            List<MiscMgr.ClothInfo> list = new List<MiscMgr.ClothInfo>();
            string url = "/root/make.action";
            string data = string.Format("secondAssPId={0}&firstAssPId={1}", assists[0].id, assists[1].id);
            string text = string.Format("设置纺织辅助人:[{0}({1}), {2}({3})] ", new object[]
			{
				assists[0].name,
				assists[0].level,
				assists[1].name,
				assists[0].level
			});
            ServerResult serverResult = protocol.postXml(url, data, "设置纺织辅助人");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            List<MiscMgr.ClothInfo> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    bool flag2 = xmlNode2.Name == "make";
                    if (flag2)
                    {
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        MiscMgr.ClothInfo clothInfo = new MiscMgr.ClothInfo();
                        foreach (XmlNode xmlNode3 in childNodes2)
                        {
                            bool flag3 = xmlNode3.Name == "id";
                            if (flag3)
                            {
                                clothInfo.id = int.Parse(xmlNode3.InnerText);
                            }
                            else
                            {
                                bool flag4 = xmlNode3.Name == "type";
                                if (flag4)
                                {
                                    clothInfo.type = int.Parse(xmlNode3.InnerText);
                                }
                                else
                                {
                                    bool flag5 = xmlNode3.Name == "name";
                                    if (flag5)
                                    {
                                        clothInfo.name = xmlNode3.InnerText;
                                    }
                                    else
                                    {
                                        bool flag6 = xmlNode3.Name == "cost";
                                        if (flag6)
                                        {
                                            clothInfo.cost = int.Parse(xmlNode3.InnerText);
                                        }
                                        else
                                        {
                                            bool flag7 = xmlNode3.Name == "successp";
                                            if (flag7)
                                            {
                                                clothInfo.success = float.Parse(xmlNode3.InnerText);
                                            }
                                            else
                                            {
                                                bool flag8 = xmlNode3.Name == "criticalp";
                                                if (flag8)
                                                {
                                                    clothInfo.critical = float.Parse(xmlNode3.InnerText);
                                                }
                                                else
                                                {
                                                    bool flag9 = xmlNode3.Name == "sellprice";
                                                    if (flag9)
                                                    {
                                                        clothInfo.sellprice = int.Parse(xmlNode3.InnerText);
                                                    }
                                                    else
                                                    {
                                                        bool flag10 = xmlNode3.Name == "lv";
                                                        if (flag10)
                                                        {
                                                            clothInfo.level = int.Parse(xmlNode3.InnerText);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        list.Add(clothInfo);
                    }
                }
                base.logInfo(logger, text);
                result = list;
            }
            return result;
        }

        private bool make(ProtocolMgr protocol, ILogger logger, MiscMgr.ClothInfo cloth, List<MiscMgr.WeaveAssist> assists, bool use_card)
        {
            string url = "/root/make!make.action";
            string data;
            string text;
            if (assists == null || assists.Count == 0)
            {
                data = string.Format("makeId={0}{1}", cloth.id, use_card ? "&mode=8" : "");
                text = string.Format("纺织{0}, 织布[{1}]", use_card ? "[使用纺织翻倍卡]" : "", cloth.name);
            }
            else
            {
                data = string.Format("makeId={0}&secondAssPId={1}&firstAssPId={2}{3}", new object[]
				{
					cloth.id,
					assists[0].id,
					assists[1].id,
					use_card ? "&mode=8" : ""
				});
                text = string.Format("纺织{0}, 辅助人::[{1}({2}), {3}({4})], 织布[{5}]", new object[]
				{
					use_card ? "[使用纺织翻倍卡]" : "",
					assists[0].name,
					assists[0].level,
					assists[1].name,
					assists[0].level,
					cloth.name
				});
            }
            ServerResult serverResult = protocol.postXml(url, data, "纺织");
            bool flag2 = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag2)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/playerupdateinfo/makeresult/makeresult/profit");
                bool flag3 = xmlNode == null;
                if (flag3)
                {
                    text += ", 纺织失败";
                    base.logInfo(logger, text);
                    result = true;
                }
                else
                {
                    int num = int.Parse(xmlNode.InnerText);
                    text += string.Format(", 银币+{0}", num);
                    base.logInfo(logger, text);
                    result = true;
                }
            }
            return result;
        }

        private string getTechName(int techId)
        {
            bool flag = techId == 0;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                string[] array = new string[]
				{
					"基础产量",
					"海外暴击",
					"大卖产量",
					"成功",
					"暴击"
				};
                int num = techId / 10;
                int num2 = techId - num * 10;
                result = array[num - 1] + num2;
            }
            return result;
        }

        private List<MiscMgr.WeaveAssist> getAssists(List<MiscMgr.WeaveAssist> assists)
        {
            List<MiscMgr.WeaveAssist> list = new List<MiscMgr.WeaveAssist>();
            bool flag = assists.Count != 0;
            if (flag)
            {
                bool flag2 = assists.Count == 1;
                if (flag2)
                {
                    list.Add(assists[0]);
                }
                else
                {
                    list.Add(assists[0]);
                    list.Add(assists[1]);
                }
            }
            bool flag3 = list.Count < 2;
            if (flag3)
            {
                list.Add(new MiscMgr.WeaveAssist());
            }
            bool flag4 = list.Count < 2;
            if (flag4)
            {
                list.Add(new MiscMgr.WeaveAssist());
            }
            return list;
        }

        public int handleRefineInfo(EquipMgr equipMgr, ProtocolMgr protocol, ILogger logger, User user, double goldPerGem, int gold_available, int reserve_count, bool do_tired_refine = false, bool do_high_refine = false)
        {
            if (user.Level < 130)
            {
                return 2;
            }
            string url = "/root/refine!getRefineInfo.action";
            ServerResult xml = protocol.getXml(url, "获取精炼信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            else
            {
                int cost1 = 0;
                int cost2 = 0;
                int status = 0;
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost1");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out cost1);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/cost2");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out cost2);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/status");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out status);
                }
                if (!do_tired_refine && status != 1)
                {
                    return 2;
                }
                else if (status == 1 && user.CurMovable < cost1)
                {
                    return 3;
                }
                else if (status == 2 && user.CurMovable < cost2)
                {
                    return 3;
                }
                else
                {
                    int refinelv = 1;
                    int remainhigh = 0;
                    int percost = 0;
                    int refinenum = 0;
                    int maxrefinenum = 0;
                    int onceplus = 1;
                    List<int> refinergroupList = new List<int>();
                    List<MiscMgr.Refiner> refinerList = new List<MiscMgr.Refiner>();
                    xmlNode = cmdResult.SelectSingleNode("/results/refinelv");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out refinelv);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/remainhigh");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out remainhigh);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/percost");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out percost);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/refinenum");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out refinenum);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/maxrefinenum");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out maxrefinenum);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/onceplus");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out onceplus);
                    }
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results");
                    XmlNodeList childNodes = xmlNode7.ChildNodes;
                    foreach (XmlNode xmlNode8 in childNodes)
                    {
                        if (xmlNode8.Name == "refinergroup")
                        {
                            XmlNodeList childNodes2 = xmlNode8.ChildNodes;
                            int item = 0;
                            long time = 0L;
                            foreach (XmlNode xmlNode9 in childNodes2)
                            {
                                if (xmlNode9.Name == "id")
                                {
                                    item = int.Parse(xmlNode9.InnerText);
                                }
                                else if (xmlNode9.Name == "cdflag")
                                {
                                    int.Parse(xmlNode9.InnerText);
                                }
                                else if (xmlNode9.Name == "time")
                                {
                                    time = long.Parse(xmlNode9.InnerText);
                                }
                            }
                            if (time == 0L)
                            {
                                refinergroupList.Add(item);
                            }
                        }
                        else if (xmlNode8.Name == "refiner")
                        {
                            XmlNodeList childNodes3 = xmlNode8.ChildNodes;
                            MiscMgr.Refiner refiner = new MiscMgr.Refiner();
                            foreach (XmlNode xmlNode10 in childNodes3)
                            {
                                if (xmlNode10.Name == "id")
                                {
                                    refiner.Id = int.Parse(xmlNode10.InnerText);
                                }
                                else if (xmlNode10.Name == "name")
                                {
                                    refiner.Name = xmlNode10.InnerText;
                                }
                                else if (xmlNode10.Name == "color")
                                {
                                    refiner.Color = xmlNode10.InnerText;
                                }
                                else if (xmlNode10.Name == "order")
                                {
                                    refiner.Order = int.Parse(xmlNode10.InnerText);
                                }
                            }
                            refinerList.Add(refiner);
                        }
                    }
                    if (refinergroupList.Count == 0)
                    {
                        return 4;
                    }
                    else if (refinenum < onceplus || refinenum < reserve_count)
                    {
                        if (equipMgr.handleWarChariotUpgrade(protocol, logger, user, true) == 0)
                        {
                            return 0;
                        }
                        return 2;
                    }
                    else if (percost > 0)
                    {
                        if (refinenum < maxrefinenum * 0.8)
                        {
                            if (equipMgr.handleWarChariotUpgrade(protocol, logger, user, true) == 0)
                            {
                                return 0;
                            }
                        }
                        else if (do_high_refine)
                        {
                            base.logInfo(logger, string.Format("余料: {0}/{1} 当前精炼队伍: {2} {3} {4}", refinenum, maxrefinenum, refinerList[0].Color, refinerList[1].Color, refinerList[2].Color));
                            return this.refine(protocol, logger);
                        }
                        return 2;
                    }
                    else
                    {
                        base.logInfo(logger, string.Format("余料: {0}/{1} 当前精炼队伍: {2} {3} {4}", refinenum, maxrefinenum, refinerList[0].Color, refinerList[1].Color, refinerList[2].Color));
                        int orderId = -1;
                        if (this.ifNeedRefreshRefiner(refinerList, out orderId))
                        {
                            this.refreshMiner(protocol, logger, percost, orderId);
                            return 0;
                        }
                        else
                        {
                            return this.refine(protocol, logger);
                        }
                    }
                }
            }
        }

        private bool ifNeedRefreshRefiner(List<MiscMgr.Refiner> _refiners, out int orderId)
        {
            orderId = -1;
            if (_refiners.Count != 3)
            {
                return false;
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    if (_refiners[i].Id < _refiners[j].Id)
                    {
                        MiscMgr.Refiner tmp = _refiners[i];
                        _refiners[i] = _refiners[j];
                        _refiners[j] = tmp;
                    }
                }
            }
            int sn = _refiners[0].Id * 100 + _refiners[1].Id * 10 + _refiners[2].Id;
            int idx = -1;
            switch (sn)
            {
                case 100:
                    idx = 0;
                    break;
                case 211:
                    idx = 2;
                    break;
                case 220:
                    idx = 1;
                    break;
                case 331:
                    idx = 1;
                    break;
                case 332:
                    idx = 1;
                    break;
                case 310:
                    idx = 1;
                    break;
                case 410:
                    idx = 1;
                    break;
                case 421:
                    idx = 1;
                    break;
                case 433:
                    idx = 2;
                    break;
                case 441:
                    idx = 2;
                    break;
                case 442:
                    idx = 2;
                    break;
            }
            if (idx >= 0)
            {
                orderId = _refiners[idx].Order;
                return true;
            }
            return false;
        }

        private bool ifNeedRefreshMiner(List<MiscMgr.Refiner> _miners, int upgrade_gold, double goldPerGem, out int index)
        {
            index = -1;
            if (_miners.Count < 3)
            {
                return false;
            }
            int colorInt = _miners[0].getColorInt();
            int colorInt2 = _miners[1].getColorInt();
            int colorInt3 = _miners[2].getColorInt();
            int num = 0;
            int refineEvent = this.getRefineEvent(colorInt, colorInt2, colorInt3, out num);
            if (refineEvent > 0)
            {
                return false;
            }
            else
            {
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5;
                if (colorInt >= 5)
                {
                    num5 = 0;
                }
                else
                {
                    num5 = this.getRefineEvent(colorInt + 1, colorInt2, colorInt3, out num2);
                }
                int num6;
                if (colorInt2 >= 5)
                {
                    num6 = 0;
                }
                else
                {
                    num6 = this.getRefineEvent(colorInt, colorInt2 + 1, colorInt3, out num3);
                }
                bool flag5 = colorInt3 >= 5;
                int num7;
                if (flag5)
                {
                    num7 = 0;
                }
                else
                {
                    num7 = this.getRefineEvent(colorInt, colorInt2, colorInt3 + 1, out num4);
                }
                bool flag6 = num5 > 0 || num6 > 0 || num7 > 0;
                if (flag6)
                {
                    bool flag7 = num2 > num3 && num2 > num4;
                    if (flag7)
                    {
                        index = 1;
                        bool flag8 = upgrade_gold == 0;
                        if (flag8)
                        {
                            return true;
                        }
                        double num8 = (double)upgrade_gold * 1.0 / (double)num2;
                        bool flag9 = num8 <= goldPerGem;
                        if (flag9)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        bool flag10 = num3 > num2 && num3 > num4;
                        if (flag10)
                        {
                            index = 2;
                            bool flag11 = upgrade_gold == 0;
                            if (flag11)
                            {
                                return true;
                            }
                            double num9 = (double)upgrade_gold * 1.0 / (double)num3;
                            bool flag12 = num9 <= goldPerGem;
                            if (flag12)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            bool flag13 = num4 > num2 && num4 > num3;
                            if (flag13)
                            {
                                index = 3;
                                bool flag14 = upgrade_gold == 0;
                                if (flag14)
                                {
                                    return true;
                                }
                                double num10 = (double)upgrade_gold * 1.0 / (double)num4;
                                bool flag15 = num10 <= goldPerGem;
                                if (flag15)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }

        private int getRefineEvent(int color1, int color2, int color3, out int eventAwardGemCount)
        {
            eventAwardGemCount = 0;
            int result;
            if (color1 == color2 && color1 == color3 && color2 == color3)
            {
                int[] array = new int[]
				{
					2,
					3,
					4,
					6,
					10,
					16
				};
                eventAwardGemCount = array[color1];
                result = 1;
            }
            else
            {
                int[] array2 = new int[]
				{
					color1,
					color2,
					color3
				};
                int num2;
                for (int i = 0; i < 2; i = num2 + 1)
                {
                    for (int j = i + 1; j < 3; j = num2 + 1)
                    {
                        bool flag2 = array2[i] > array2[j];
                        if (flag2)
                        {
                            int num = array2[i];
                            array2[i] = array2[j];
                            array2[j] = num;
                        }
                        num2 = j;
                    }
                    num2 = i;
                }
                bool flag3 = array2[0] + 1 == array2[1] && array2[1] + 1 == array2[2];
                if (flag3)
                {
                    int[] array3 = new int[]
					{
						3,
						4,
						6,
						10
					};
                    eventAwardGemCount = array3[array2[0]];
                    result = 2;
                }
                else
                {
                    bool flag4 = array2[2] >= 4 && array2[1] == array2[0] && array2[1] <= 1;
                    if (flag4)
                    {
                        int[] array4 = new int[]
						{
							6,
							8
						};
                        eventAwardGemCount = array4[array2[2] - 4];
                        result = 3;
                    }
                    else
                    {
                        bool flag5 = (array2[2] == array2[1] && array2[2] >= 3) || (array2[1] == array2[0] && array2[1] >= 3);
                        if (flag5)
                        {
                            int[] array5 = new int[]
							{
								4,
								6,
								10
							};
                            eventAwardGemCount = array5[array2[1] - 3];
                            result = 4;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        public int refine(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/refine!refine.action";
            string text = "精炼";
            ServerResult xml = protocol.getXml(url, text);
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                int bowlder = 0;
                int stone = 0;
                int baoshi = 0;
                int baoji = 0;
                string eventintro = "";
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    if (xmlNode2.Name == "bowlder")
                    {
                        bowlder = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "stone")
                    {
                        stone = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "baoshi")
                    {
                        baoshi = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "baoji")
                    {
                        baoji = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "eventintro")
                    {
                        eventintro = xmlNode2.InnerText;
                    }
                }
                if (baoji != 0)
                {
                    text += "<<暴击>>, ";
                }
                else
                {
                    text += ", ";
                }
                if (eventintro != "")
                {
                    text += string.Format("获得精炼事件:{0}, ", eventintro);
                }
                text += string.Format("获得玉石+{0}, 原石+{1}", bowlder, stone);
                if (baoshi > 0)
                {
                    text += string.Format(", 宝石+{0}", baoshi);
                }
                base.logInfo(logger, text);
                return 0;
            }
        }

        public bool refreshMiner(ProtocolMgr protocol, ILogger logger, int upgrade_gold, int orderId)
        {
            string url = "/root/refine!refreshOneRefiner.action";
            string data = "refinerOrder=" + orderId;
            string text = string.Format("升级精炼工人{0}, 消耗金币{1}", orderId, upgrade_gold);
            ServerResult serverResult = protocol.postXml(url, data, text);
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return false;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            cmdResult.SelectSingleNode("/results");
            base.logInfo(logger, text);
            return true;
        }

        public int handleRefineFactoryInfo(ProtocolMgr protocol, ILogger logger, User user, int refine_gold_limit, int refine_stone_limit, int gold_available, int stone_available, string _refine_weapon_name, int silver_available, bool do_tired_refine = false)
        {
            if (user.Silver < 10000000)
            {
                ticketExchangeMoney(protocol, logger, 5);
            }

            int result;
            if (user.Level < 161)
            {
                result = 2;
            }
            else
            {
                int red = 0;
                int zi = 0;
                int refine_level = 0;
                int cost1 = 0;
                int cost2 = 0;
                int status = 0;
                int remainhigh = 0;
                int refine_id = 0;
                string url = "/root/refine!getRefineFactory.action";
                ServerResult xml = protocol.getXml(url, "获取炼制信息");
                if (xml == null)
                {
                    result = 1;
                }
                else if (!xml.CmdSucceed)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    int goldcost = 1;
                    int bowldercost = 50;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/goldcost");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out goldcost);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/bowldercost");
                    if (xmlNode2 != null)
                    {
                        int.TryParse(xmlNode2.InnerText, out bowldercost);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/red");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out red);
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/zi");
                    if (xmlNode4 != null)
                    {
                        int.TryParse(xmlNode4.InnerText, out zi);
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/lv");
                    if (xmlNode5 != null)
                    {
                        int.TryParse(xmlNode5.InnerText, out refine_level);
                    }
                    XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/cost1");
                    if (xmlNode6 != null)
                    {
                        int.TryParse(xmlNode6.InnerText, out cost1);
                    }
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/cost2");
                    if (xmlNode7 != null)
                    {
                        int.TryParse(xmlNode7.InnerText, out cost2);
                    }
                    XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/status");
                    if (xmlNode8 != null)
                    {
                        int.TryParse(xmlNode8.InnerText, out status);
                    }
                    int newrefine = 0;
                    XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/newrefine");
                    if (xmlNode9 != null)
                    {
                        int.TryParse(xmlNode9.InnerText, out newrefine);
                    }
                    XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/remainhigh");
                    if (xmlNode10 != null)
                    {
                        int.TryParse(xmlNode10.InnerText, out remainhigh);
                    }
                    if (!do_tired_refine && status != 1)
                    {
                        result = 2;
                    }
                    else if (status == 1 && user.CurMovable < cost1)
                    {
                        result = 3;
                    }
                    else if (status == 2 && user.CurMovable < cost2)
                    {
                        result = 3;
                    }
                    else
                    {
                        int num9 = 5;
                        XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/troop");
                        foreach (XmlNode xmlNode11 in xmlNodeList)
                        {
                            string s = "";
                            string text = "";
                            XmlNodeList childNodes = xmlNode11.ChildNodes;
                            foreach (XmlNode xmlNode12 in childNodes)
                            {
                                if (xmlNode12.Name == "name")
                                {
                                    text = xmlNode12.InnerText;
                                }
                                else if (xmlNode12.Name == "troopid")
                                {
                                    s = xmlNode12.InnerText;
                                }
                                else if (xmlNode12.Name == "quality")
                                {
                                    num9 = int.Parse(xmlNode12.InnerText);
                                }
                            }
                            bool flag20 = text.IndexOf(_refine_weapon_name) >= 0;
                            if (flag20)
                            {
                                int.TryParse(s, out refine_id);
                                break;
                            }
                        }
                        bool flag21 = num9 == 5 && silver_available < red;
                        if (flag21)
                        {
                            result = 4;
                        }
                        else
                        {
                            bool flag22 = num9 == 6 && silver_available < zi;
                            if (flag22)
                            {
                                result = 4;
                            }
                            else
                            {
                                int mode = 0;
                                bool flag23 = goldcost <= refine_gold_limit && gold_available >= goldcost;
                                if (flag23)
                                {
                                    mode = 2;
                                }
                                else
                                {
                                    bool flag24 = bowldercost <= refine_stone_limit && stone_available >= bowldercost;
                                    if (flag24)
                                    {
                                        mode = 1;
                                    }
                                }
                                bool flag25 = this.refineItem(protocol, logger, mode, refine_id, _refine_weapon_name, refine_level, newrefine);
                                bool flag26 = flag25;
                                if (flag26)
                                {
                                    result = 0;
                                }
                                else
                                {
                                    result = 10;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool refineItem(ProtocolMgr protocol, ILogger logger, int mode, int _refine_id, string _refine_name, int refine_level, int newrefine)
        {
            bool flag = _refine_id == 0;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                string url = "/root/refine!doRefineFactory.action";
                bool flag2 = newrefine == 0;
                string data;
                if (flag2)
                {
                    data = string.Format("mode={0}&troopId={1}", mode, _refine_id);
                }
                else
                {
                    data = string.Format("troopId2={0}&troopId={1}&mode={2}&troopId3={3}", new object[]
					{
						_refine_id,
						_refine_id,
						mode,
						_refine_id
					});
                }
                ServerResult serverResult = protocol.postXml(url, data, "炼制物品");
                bool flag3 = serverResult == null || !serverResult.CmdSucceed;
                if (flag3)
                {
                    result = false;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    int num = 1;
                    int num2 = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cri");
                    bool flag4 = xmlNode != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/extra");
                    bool flag5 = xmlNode2 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    string arg = "普通炼制";
                    bool flag6 = mode == 1;
                    if (flag6)
                    {
                        arg = "玉石炼制";
                    }
                    else
                    {
                        bool flag7 = mode == 2;
                        if (flag7)
                        {
                            arg = "金币炼制";
                        }
                    }
                    string text = string.Format("{0}物品[{1}*{2}]成功", arg, _refine_name, refine_level * num + num2);
                    base.logInfo(logger, text);
                    result = true;
                }
            }
            return result;
        }

        public void handleNationTaskInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            bool flag = user._can_get_nation_task_reward && this.getNationTaskReward(protocol, logger);
            if (flag)
            {
                user._can_get_nation_task_reward = false;
            }
            string url = "/root/nation!getNationTask.action";
            ServerResult xml = protocol.getXml(url, "获取国家任务信息");
            bool flag2 = xml == null || !xml.CmdSucceed;
            if (!flag2)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNode xmlNode2 = xmlNode.SelectSingleNode("/results/nationtask");
                bool flag3 = xmlNode2 != null;
                if (!flag3)
                {
                    int num = 0;
                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("agreegold");
                    bool flag4 = xmlNode3 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode3.InnerText, out num);
                    }
                    int num2 = 0;
                    string support_player = "";
                    string support_name = "";
                    XmlNodeList childNodes = xmlNode.SelectSingleNode("jinyan").ChildNodes;
                    foreach (XmlNode xmlNode4 in childNodes)
                    {
                        bool flag5 = !(xmlNode4.Name != "nationtask");
                        if (flag5)
                        {
                            XmlNodeList childNodes2 = xmlNode4.ChildNodes;
                            bool flag6 = false;
                            foreach (XmlNode xmlNode5 in childNodes2)
                            {
                                bool flag7 = xmlNode5.Name == "agreestatus";
                                if (flag7)
                                {
                                    flag6 = (xmlNode5.InnerText != "0");
                                }
                                else
                                {
                                    bool flag8 = xmlNode5.Name == "id";
                                    if (flag8)
                                    {
                                        num2 = int.Parse(xmlNode5.InnerText);
                                    }
                                    else
                                    {
                                        bool flag9 = xmlNode5.Name == "name";
                                        if (flag9)
                                        {
                                            support_name = xmlNode5.InnerText;
                                        }
                                        else
                                        {
                                            bool flag10 = xmlNode5.Name == "playername";
                                            if (flag10)
                                            {
                                                support_player = xmlNode5.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            bool flag11 = !flag6;
                            if (flag11)
                            {
                                break;
                            }
                            num2 = 0;
                        }
                    }
                    bool flag12 = num == 0 && num2 > 0;
                    if (flag12)
                    {
                        this.supportTask(protocol, logger, num2, support_player, support_name);
                    }
                }
            }
        }

        public bool getNationTaskReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/nation!getNationTaskReward.action";
            ServerResult xml = protocol.getXml(url, "获取国家任务信息奖励");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                xml.CmdResult.SelectSingleNode("/results");
                result = true;
            }
            return result;
        }

        private bool supportTask(ProtocolMgr protocol, ILogger logger, int support_id, string support_player, string support_name)
        {
            string url = "/root/nation!agreeNationTask.action";
            string data = "jinyanId=" + support_id;
            string text = string.Format("支持 [{0}] 进言 [{1}]", support_player, support_name);
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        public int handleDailyTaskInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/task!getPerdayTask.action";
            ServerResult xml = protocol.getXml(url, "获取每日任务信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    int num = this.handleTaskInfo(protocol, logger, cmdResult);
                    bool flag3 = num == 1;
                    if (flag3)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        private int handleTaskInfo(ProtocolMgr protocol, ILogger logger, XmlDocument xml)
        {
            int[] array = new int[6];
            int num = 0;
            bool flag = false;
            List<int> list = new List<int>();
            int num2 = 0;
            List<MiscMgr.TaskItem> list2 = new List<MiscMgr.TaskItem>();
            int num3;
            for (int i = 0; i < array.Length; i = num3 + 1)
            {
                array[i] = 0;
                num3 = i;
            }
            XmlNode xmlNode = xml.SelectSingleNode("/results/scorelist");
            bool flag2 = xmlNode != null;
            if (flag2)
            {
                int num4 = 0;
                XmlNode xmlNode2 = xml.SelectSingleNode("/results/score");
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    int.TryParse(xmlNode2.InnerText, out num4);
                }
                string[] array2 = xmlNode.InnerText.Split(new char[]
				{
					','
				});
                List<int> list3 = new List<int>();
                string[] array3 = array2;
                for (int j = 0; j < array3.Length; j = num3 + 1)
                {
                    string text = array3[j];
                    bool flag4 = text != null && text.Length != 0;
                    if (flag4)
                    {
                        int item = 1000000;
                        int.TryParse(text, out item);
                        list3.Add(item);
                    }
                    num3 = j;
                }
                for (int k = 0; k < list3.Count; k = num3 + 1)
                {
                    bool flag5 = num4 >= list3[k];
                    if (flag5)
                    {
                        array[k] = 1;
                    }
                    num3 = k;
                }
                XmlNode xmlNode3 = xml.SelectSingleNode("/results/scorerewardlist");
                bool flag6 = xmlNode3 != null;
                if (flag6)
                {
                    string[] array4 = xmlNode3.InnerText.Split(new char[]
					{
						':'
					});
                    list3.Clear();
                    string[] array5 = array4;
                    for (int l = 0; l < array5.Length; l = num3 + 1)
                    {
                        string text2 = array5[l];
                        bool flag7 = text2 != null && text2.Length != 0;
                        if (flag7)
                        {
                            int num5 = 0;
                            int.TryParse(text2, out num5);
                            bool flag8 = num5 > 0;
                            if (flag8)
                            {
                                list3.Add(num5);
                            }
                        }
                        num3 = l;
                    }
                    int num6 = 0;
                    while (num6 < list3.Count && num6 < array.Length)
                    {
                        array[num6] = 0;
                        num3 = num6;
                        num6 = num3 + 1;
                    }
                }
            }
            XmlNode xmlNode4 = xml.SelectSingleNode("/results/status");
            bool flag9 = xmlNode4 != null;
            if (flag9)
            {
                flag = (xmlNode4.InnerText == "1");
            }
            XmlNode xmlNode5 = xml.SelectSingleNode("/results/secretarytime");
            bool flag10 = xmlNode5 != null;
            if (flag10)
            {
                long.Parse(xmlNode5.InnerText);
            }
            string text3 = "";
            string text4 = "";
            XmlNode xmlNode6 = xml.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode6.ChildNodes;
            foreach (XmlNode xmlNode7 in childNodes)
            {
                bool flag11 = xmlNode7.Name == "remainnum";
                if (flag11)
                {
                    num = int.Parse(xmlNode7.InnerText);
                }
                else
                {
                    bool flag12 = xmlNode7.Name == "combo";
                    if (flag12)
                    {
                        string[] array6 = xmlNode7.InnerText.Split(new char[]
						{
							','
						});
                        string[] array7 = array6;
                        for (int m = 0; m < array7.Length; m = num3 + 1)
                        {
                            string text5 = array7[m];
                            bool flag13 = text5 != null && !(text5 == "");
                            if (flag13)
                            {
                                string[] array8 = text5.Split(new char[]
								{
									':'
								});
                                text3 = text3 + array8[0] + ",";
                                int item2 = int.Parse(array8[0]);
                                bool flag14 = array8[1] == "0";
                                if (flag14)
                                {
                                    list.Add(item2);
                                }
                                else
                                {
                                    text4 = text4 + array8[0] + ",";
                                }
                            }
                            num3 = m;
                        }
                    }
                    else
                    {
                        bool flag15 = xmlNode7.Name == "comboreward";
                        if (flag15)
                        {
                            int.TryParse(xmlNode7.InnerText, out num2);
                        }
                        else
                        {
                            bool flag16 = xmlNode7.Name == "task";
                            if (flag16)
                            {
                                XmlNodeList childNodes2 = xmlNode7.ChildNodes;
                                MiscMgr.TaskItem taskItem = new MiscMgr.TaskItem();
                                foreach (XmlNode xmlNode8 in childNodes2)
                                {
                                    bool flag17 = xmlNode8.Name == "id";
                                    if (flag17)
                                    {
                                        taskItem.id = int.Parse(xmlNode8.InnerText);
                                    }
                                    else
                                    {
                                        bool flag18 = xmlNode8.Name == "type";
                                        if (flag18)
                                        {
                                            taskItem.type = int.Parse(xmlNode8.InnerText);
                                        }
                                        else
                                        {
                                            bool flag19 = xmlNode8.Name == "quality";
                                            if (flag19)
                                            {
                                                taskItem.quality = int.Parse(xmlNode8.InnerText);
                                            }
                                            else
                                            {
                                                bool flag20 = xmlNode8.Name == "name";
                                                if (flag20)
                                                {
                                                    taskItem.name = xmlNode8.InnerText;
                                                }
                                                else
                                                {
                                                    bool flag21 = xmlNode8.Name == "reward";
                                                    if (flag21)
                                                    {
                                                        taskItem.reward = int.Parse(xmlNode8.InnerText);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                list2.Add(taskItem);
                            }
                        }
                    }
                }
            }
            bool flag22 = num2 >= 50;
            if (flag22)
            {
                base.tryUseCard(protocol, logger, xml);
            }
            else
            {
                base.tryCancelCard(protocol, logger, xml);
            }
            for (int n = 0; n < array.Length; n = num3 + 1)
            {
                bool flag23 = array[n] > 0;
                if (flag23)
                {
                    this.getTaskAward(protocol, logger, n + 1);
                }
                num3 = n;
            }
            bool flag24 = num == 0;
            int result;
            if (flag24)
            {
                result = 1;
            }
            else
            {
                bool flag25 = flag;
                if (flag25)
                {
                    this.cancelTask(protocol, logger);
                }
                else
                {
                    MiscMgr.TaskItem taskItem2 = null;
                    MiscMgr.TaskItem taskItem3 = null;
                    base.logInfo(logger, "开始选择每日任务");
                    foreach (MiscMgr.TaskItem current in list2)
                    {
                        base.logInfo(logger, string.Format("任务{0}:[{1}, 类型{2}, 星级{3}, 积分{4}]", new object[]
						{
							current.id + 1,
							current.name,
							current.type,
							current.quality,
							current.reward
						}));
                    }
                    base.logInfo(logger, string.Format("当前combo积分:{0}, 所需任务类型[{1}], 已完成任务类型[{2}]", num2, text3, text4));
                    foreach (MiscMgr.TaskItem current2 in list2)
                    {
                        bool flag26 = list.IndexOf(current2.type) >= 0 && (taskItem2 == null || taskItem2.reward < current2.reward);
                        if (flag26)
                        {
                            taskItem2 = current2;
                        }
                        bool flag27 = taskItem3 == null || taskItem3.reward < current2.reward;
                        if (flag27)
                        {
                            taskItem3 = current2;
                        }
                    }
                    bool flag28 = taskItem3 != null;
                    if (flag28)
                    {
                        bool flag29 = taskItem2 != null;
                        if (flag29)
                        {
                            this.selectTask(protocol, logger, taskItem2);
                        }
                        else
                        {
                            this.selectTask(protocol, logger, taskItem3);
                        }
                        this.autoTask(protocol, logger);
                    }
                    else
                    {
                        bool flag30 = taskItem2 != null;
                        if (flag30)
                        {
                            this.selectTask(protocol, logger, taskItem2);
                            this.autoTask(protocol, logger);
                        }
                    }
                }
                result = 0;
            }
            return result;
        }

        private void selectTask(ProtocolMgr protocol, ILogger logger, MiscMgr.TaskItem task)
        {
            string url = "/root/task!choosePerdayTask.action";
            string data = "taskId=" + task.id;
            ServerResult serverResult = protocol.postXml(url, data, "选择每日任务");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, string.Format("选择新任务, 位置{0}:[{1}, 类型{2}, 星级{3}, 积分{4}]", new object[]
				{
					task.id + 1,
					task.name,
					task.type,
					task.quality,
					task.reward
				}));
            }
        }

        private void autoTask(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/secretary!addTask.action";
            ServerResult serverResult = protocol.postXml(url, "tid=13", "自动每日任务");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
            }
        }

        private void cancelTask(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/task!cancelPerdayTask.action";
            ServerResult xml = protocol.getXml(url, "取消每日任务");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                base.logInfo(logger, "取消原有任务");
                this.handleTaskInfo(protocol, logger, cmdResult);
            }
        }

        public void getTaskAward(ProtocolMgr protocol, ILogger logger, int rewardId)
        {
            string url = "/root/task!getPerdayTaskReward.action";
            string data = "rewardId=" + rewardId;
            ServerResult serverResult = protocol.postXml(url, data, "拿取每日任务奖励");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                string arg = "";
                string arg2 = "";
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/reward/item/name");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    arg = xmlNode.InnerText;
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/reward/item/num");
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    arg2 = xmlNode2.InnerText;
                }
                base.logInfo(logger, string.Format("拿取每日任务奖励, {0}+{1}", arg, arg2));
            }
        }

        public int handleSecretaryInfo(ProtocolMgr protocol, ILogger logger, out long next_cd)
        {
            next_cd = 0L;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            string url = "/root/secretary.action";
            ServerResult xml = protocol.getXml(url, "获取玩家小秘书信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/maxtokennum");
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/tokennum");
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/cd");
                    bool flag3 = xmlNode != null && xmlNode2 != null && xmlNode3 != null;
                    if (flag3)
                    {
                        int num5 = int.Parse(xmlNode.InnerText);
                        int num6 = int.Parse(xmlNode2.InnerText);
                        num = num5 - num6;
                        num2 = int.Parse(xmlNode3.InnerText);
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/lin");
                    bool flag4 = xmlNode4 != null;
                    if (flag4)
                    {
                        num3 = int.Parse(xmlNode4.InnerText);
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/remainharvesttime");
                    bool flag5 = xmlNode5 != null;
                    if (flag5)
                    {
                        num4 = int.Parse(xmlNode5.InnerText);
                    }
                    bool flag6 = num == 0 && num3 == 0 && num4 == 0;
                    if (flag6)
                    {
                        result = 2;
                    }
                    else
                    {
                        bool flag7 = false;
                        bool flag8 = false;
                        bool flag9 = false;
                        bool flag10 = num > 0 && num2 == 0;
                        if (flag10)
                        {
                            flag7 = (this.getFreeToken(protocol, logger, out num, out num2) && num == 1);
                        }
                        bool flag11 = num3 > 0;
                        if (flag11)
                        {
                            int num7;
                            for (int i = 0; i < num3; i = num7 + 1)
                            {
                                bool arg_195_0;
                                if (this.harvestFood(protocol, logger))
                                {
                                    num7 = num3 - 1;
                                    num3 = num7;
                                    arg_195_0 = (num7 == 0);
                                }
                                else
                                {
                                    arg_195_0 = false;
                                }
                                flag8 = arg_195_0;
                                num7 = i;
                            }
                        }
                        bool flag12 = num4 > 0;
                        if (flag12)
                        {
                            int num7;
                            for (int j = 0; j < num4; j = num7 + 1)
                            {
                                bool arg_1D9_0;
                                if (this.harvestStone(protocol, logger))
                                {
                                    num7 = num4 - 1;
                                    num4 = num7;
                                    arg_1D9_0 = (num7 == 0);
                                }
                                else
                                {
                                    arg_1D9_0 = false;
                                }
                                flag9 = arg_1D9_0;
                                num7 = j;
                            }
                        }
                        next_cd = (long)num2;
                        bool flag13 = flag7 & flag8 & flag9;
                        if (flag13)
                        {
                            result = 2;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        public void handleGiftInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/newGift!getNewGiftList.action";
            ServerResult xml = protocol.getXml(url, "获取玩家礼包信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/gift");
                new List<MiscMgr.GiftInfo>();
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    bool flag2 = xmlNode != null && xmlNode.HasChildNodes;
                    if (flag2)
                    {
                        XmlNodeList childNodes = xmlNode.ChildNodes;
                        MiscMgr.GiftInfo giftInfo = new MiscMgr.GiftInfo();
                        foreach (XmlNode xmlNode2 in childNodes)
                        {
                            bool flag3 = xmlNode2.Name == "id";
                            if (flag3)
                            {
                                giftInfo.id = xmlNode2.InnerText;
                            }
                            else
                            {
                                bool flag4 = xmlNode2.Name == "name";
                                if (flag4)
                                {
                                    giftInfo.name = xmlNode2.InnerText;
                                }
                                else
                                {
                                    bool flag5 = xmlNode2.Name == "content";
                                    if (flag5)
                                    {
                                        giftInfo.content = xmlNode2.InnerText;
                                    }
                                    else
                                    {
                                        bool flag6 = xmlNode2.Name == "intime";
                                        if (flag6)
                                        {
                                            giftInfo.intime = xmlNode2.InnerText;
                                        }
                                        else
                                        {
                                            bool flag7 = xmlNode2.Name == "statuts";
                                            if (flag7)
                                            {
                                                giftInfo.status = xmlNode2.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool flag8 = giftInfo.intime == "1" && giftInfo.status == "0";
                        if (flag8)
                        {
                            this.getGiftReward(protocol, logger, giftInfo);
                        }
                    }
                }
            }
        }

        private void getGiftReward(ProtocolMgr protocol, ILogger logger, MiscMgr.GiftInfo gift)
        {
            string url = "/root/newGift!getNewGiftReward.action";
            string data = string.Format("giftId={0}", gift.id);
            ServerResult serverResult = protocol.postXml(url, data, "领取玩家礼包");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                string text = string.Format("领取玩家礼包[{0}], 获得[{1}]", gift.name, gift.getContentDesc());
                base.logInfo(logger, text);
            }
        }

        private bool getFreeToken(ProtocolMgr protocol, ILogger logger, out int token_remains, out int get_token_cd)
        {
            get_token_cd = 0;
            token_remains = 0;
            string url = "/root/secretary!applyToken.action";
            ServerResult xml = protocol.getXml(url, "小秘书领取军令");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int num = 4;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/maxtokennum");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                int num2 = 4;
                xmlNode = cmdResult.SelectSingleNode("/results/tokennum");
                bool flag3 = xmlNode != null;
                if (flag3)
                {
                    int.TryParse(xmlNode.InnerText, out num2);
                }
                token_remains = num - num2;
                xmlNode = cmdResult.SelectSingleNode("/results/cd");
                bool flag4 = xmlNode != null;
                if (flag4)
                {
                    int.TryParse(xmlNode.InnerText, out get_token_cd);
                }
                base.logInfo(logger, string.Format("小秘书领取军令, 还剩{0}个", token_remains));
                result = true;
            }
            return result;
        }

        private bool harvestFood(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/world!impose.action";
            ServerResult serverResult = protocol.postXml(url, "resId=10", "收取军团农场");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int num = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gains");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                bool flag3 = num == 0;
                if (flag3)
                {
                    result = false;
                }
                else
                {
                    base.logInfo(logger, "收取军团农场, 获得银币 +" + num);
                    result = true;
                }
            }
            return result;
        }

        private bool harvestStone(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/bowlderRes!HarvestNationBowlderRes.action";
            ServerResult xml = protocol.getXml(url, "收取国家玉石矿");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/bowlderadd");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    int num = int.Parse(xmlNode.InnerText);
                    base.logInfo(logger, "收取国家玉石矿, 获得原石 +" + num);
                    bool flag3 = num == 0;
                    if (flag3)
                    {
                        result = false;
                        return result;
                    }
                }
                result = true;
            }
            return result;
        }

        public void handleNationTreasureInfo(ProtocolMgr protocol, ILogger logger, int box_reserve = 450)
        {
            string url = "/root/world!getNewAreaTreasureInfo.action";
            ServerResult xml = protocol.getXml(url, "查看国家宝箱");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/treasurenum");
                bool flag2 = xmlNode == null;
                if (!flag2)
                {
                    int num = 0;
                    int.TryParse(xmlNode.InnerText, out num);
                    bool flag3 = box_reserve < 0;
                    if (flag3)
                    {
                        box_reserve = -1 * box_reserve;
                        int num2 = box_reserve / 5 + 1;
                        int num3;
                        for (int i = 0; i < num2; i = num3 + 1)
                        {
                            this.openNationTreasure(protocol, logger);
                            num3 = i;
                        }
                    }
                    else
                    {
                        bool flag4 = box_reserve < 5;
                        if (flag4)
                        {
                            box_reserve = 5;
                        }
                        bool flag5 = num >= box_reserve;
                        if (flag5)
                        {
                            int num4 = (num - box_reserve) / 5 + 1;
                            int num3;
                            for (int j = 0; j < num4; j = num3 + 1)
                            {
                                this.openNationTreasure(protocol, logger);
                                num3 = j;
                            }
                        }
                    }
                }
            }
        }

        public void openNationTreasure(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/world!draw5NewAreaTreasure.action";
            string text = "打开国家宝箱*5";
            ServerResult xml = protocol.getXml(url, text);
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/reward");
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        bool flag2 = xmlNode2.Name == "rewardtype";
                        if (flag2)
                        {
                            int.TryParse(xmlNode2.InnerText, out num6);
                        }
                        else
                        {
                            bool flag3 = xmlNode2.Name == "rewardvalue";
                            if (flag3)
                            {
                                int.TryParse(xmlNode2.InnerText, out num7);
                            }
                            else
                            {
                                bool flag4 = xmlNode2.Name == "baowu";
                                if (flag4)
                                {
                                    int.TryParse(xmlNode2.InnerText, out num8);
                                }
                            }
                        }
                    }
                    bool flag5 = num6 == 1;
                    if (flag5)
                    {
                        num += num7;
                    }
                    else
                    {
                        bool flag6 = num6 == 2;
                        if (flag6)
                        {
                            num3 += num7;
                        }
                        else
                        {
                            bool flag7 = num6 == 3;
                            if (flag7)
                            {
                                num2 += num7;
                            }
                            else
                            {
                                bool flag8 = num6 == 4;
                                if (flag8)
                                {
                                    num4 += num7;
                                }
                            }
                        }
                    }
                    bool flag9 = num8 > 0;
                    if (flag9)
                    {
                        num5 += num8;
                    }
                }
                text += ", 获得奖励: ";
                bool flag10 = num > 0;
                if (flag10)
                {
                    text += string.Format("银币+{0} ", num);
                }
                bool flag11 = num2 > 0;
                if (flag11)
                {
                    text += string.Format("军令+{0} ", num2);
                }
                bool flag12 = num3 > 0;
                if (flag12)
                {
                    text += string.Format("攻击令+{0} ", num3);
                }
                bool flag13 = num4 > 0;
                if (flag13)
                {
                    text += string.Format("玉石+{0} ", num4);
                }
                bool flag14 = num5 > 0;
                if (flag14)
                {
                    text += string.Format("宝物+{0} ", num5);
                }
                base.logInfo(logger, text);
            }
        }

        public int handleDailyEventInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event.action";
            ServerResult xml = protocol.getXml(url, "获取玩家日常事件信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/curreventdto");
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    int num = 0;
                    long num2 = 0L;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        bool flag3 = xmlNode2.Name == "id";
                        if (flag3)
                        {
                            int.Parse(xmlNode2.InnerText);
                        }
                        else
                        {
                            bool flag4 = xmlNode2.Name == "cd";
                            if (flag4)
                            {
                                long.TryParse(xmlNode2.InnerText, out num2);
                            }
                            else
                            {
                                bool flag5 = xmlNode2.Name == "salary";
                                if (flag5)
                                {
                                    num = int.Parse(xmlNode2.InnerText);
                                }
                            }
                        }
                    }
                    bool flag6 = num2 == 0L && num > 0 && !this.getEventAward(protocol, logger);
                    if (flag6)
                    {
                        result = 10;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        private bool getEventAward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!saveSalary.action";
            ServerResult xml = protocol.getXml(url, "获取玩家日常事件奖励");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                string innerText = cmdResult.SelectSingleNode("/results/salary").InnerText;
                base.logInfo(logger, string.Format("领取日常事件奖励 {0}银币", innerText));
                result = true;
            }
            return result;
        }

        public int refillActive(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/refine!fullActive.action";
            ServerResult xml = protocol.getXml(url, "补充行动力");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    base.logInfo(logger, "补充行动力成功");
                    result = 0;
                }
            }
            return result;
        }

        public int handleGoldboxEventInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/giftEvent!getGoldBoxEventInfo.action";
            string desc = "获取充值赠礼信息";
            ServerResult xml = protocol.getXml(url, desc);
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/boxnum");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int num = 0;
                        int.TryParse(xmlNode.InnerText, out num);
                        int num2;
                        for (int i = 0; i < num; i = num2 + 1)
                        {
                            this.openGoldBoxEvent(protocol, logger);
                            num2 = i;
                        }
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/onlinereward");
                    bool flag4 = xmlNode2 != null && xmlNode2.InnerText != "0" && this.recvOnlineReward(protocol, logger) == 0;
                    if (flag4)
                    {
                        this.openGoldBoxEvent(protocol, logger);
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int recvOnlineReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/giftEvent!recvOnlineReward.action";
            string text = "领取充值赠礼在线礼包";
            ServerResult xml = protocol.getXml(url, text);
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    text += "成功";
                    base.logInfo(logger, text);
                    result = 0;
                }
            }
            return result;
        }

        public int openGoldBoxEvent(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/giftEvent!openGoldBoxEvent.action";
            string text = "开启充值赠礼礼包";
            ServerResult serverResult = protocol.postXml(url, "num=1", text);
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/reward/gold");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int num = 0;
                        int.TryParse(xmlNode.InnerText, out num);
                        base.logInfo(logger, string.Format("{0}, 金币+{1}", text, num));
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/reward/baoshi");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int num2 = 0;
                        int.TryParse(xmlNode2.InnerText, out num2);
                        base.logInfo(logger, string.Format("{0}, 宝石+{1}", text, num2));
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/reward/bowlder");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int num3 = 0;
                        int.TryParse(xmlNode3.InnerText, out num3);
                        base.logInfo(logger, string.Format("{0}, 玉石+{1}", text, num3));
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/reward/copper");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        int num4 = 0;
                        int.TryParse(xmlNode4.InnerText, out num4);
                        base.logInfo(logger, string.Format("{0}, 银币+{1}", text, num4));
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int handleLvGiftInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/mainCity!getLvGiftInfo.action";
            ServerResult serverResult = protocol.postXml(url, "num=1", "获取天命奖励信息");
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/reward/gold");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int num = 0;
                        int.TryParse(xmlNode.InnerText, out num);
                        base.logInfo(logger, string.Format("", new object[0]));
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int handleChampionInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/mainCity!getChampionInfo.action";
            ServerResult serverResult = protocol.getXml(url, "获取冠军信息");
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/canvisit");
                if (xmlNode != null)
                {
                    int canvisit = 0;
                    int.TryParse(xmlNode.InnerText, out canvisit);
                    if (canvisit == 1)
                    {
                        visitChampion(protocol, logger);
                    }
                }
                return 0;
            }
        }

        public void visitChampion(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/mainCity!visitChampion.action";
            ServerResult serverResult = protocol.getXml(url, "祝贺冠军");
            if (serverResult != null && serverResult.CmdSucceed)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tickets");
                if (xmlNode != null)
                {
                    int tickets = 0;
                    int.TryParse(xmlNode.InnerText, out tickets);
                    base.logInfo(logger, string.Format("获得点券+{0}", tickets));
                }
            }
        }

        public void getSeniorJailInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/jail!getSeniorJailInfo.action";
            ServerResult serverResult = protocol.getXml(url, "获取廷尉狱信息");
            if (serverResult != null && serverResult.CmdSucceed)
            {
                List<SlaveInfo> slaveInfoList = new List<SlaveInfo>();
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/slaveinfo");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    SlaveInfo slave = new SlaveInfo();
                    foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                    {
                        if (xmlNode2.Name == "slaveid")
                        {
                            slave.id_ = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "slavename")
                        {
                            slave.name_ = xmlNode2.InnerText;
                        }
                        else if (xmlNode2.Name == "slavelv")
                        {
                            slave.lv_ = int.Parse(xmlNode2.InnerText);
                        }
                    }
                    slaveInfoList.Add(slave);
                }
                slashSeniorSlaves(protocol, logger, slaveInfoList);
            }
        }

        public void slashSeniorSlaves(ProtocolMgr protocol, ILogger logger, List<SlaveInfo> list)
        {
            string url = "/root/jail!slashSeniorSlaves.action";
            foreach (SlaveInfo slave in list)
            {
                string data = string.Format("slaveId={0}", slave.id_);
                ServerResult serverResult = protocol.postXml(url, data, "高级劳工劳作");
                if (serverResult != null && serverResult.CmdSucceed)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    int baoshinum = 0;
                    int baoshilv = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi/baoshinum");
                    if (xmlNode != null)
                    {
                        baoshinum = int.Parse(xmlNode.InnerText);
                    }
                    xmlNode = cmdResult.SelectSingleNode("/results/baoshi/baoshilv");
                    if (xmlNode != null)
                    {
                        baoshilv = int.Parse(xmlNode.InnerText);
                    }
                    logInfo(logger, string.Format("对高级劳工({0} Lv.{1})进行劳作，获取宝石Lv.{2} + {3}", slave.name_, slave.lv_, baoshilv, baoshinum));
                }
            }
        }

        #region 点券商城
        public List<TicketItem> getTicketsInfo(ProtocolMgr protocol, ILogger logger, ref long tickets)
        {
            List<TicketItem> list = new List<TicketItem>();
            string url = "/root/tickets.action";
            ServerResult xml = protocol.getXml(url, "点券商城");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tickets");
                if (xmlNode != null)
                {
                    long.TryParse(xmlNode.InnerText, out tickets);
                }

                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/rewards/reward");
                foreach (XmlNode node in xmlNodeList)
                {
                    TicketItem item = new TicketItem();
                    XmlNode childnode = node.SelectSingleNode("id");
                    if (childnode != null)
                    {
                        item.id = int.Parse(childnode.InnerText);
                    }
                    childnode = node.SelectSingleNode("playerlevel");
                    if (childnode != null)
                    {
                        item.playerlevel = int.Parse(childnode.InnerText);
                    }
                    childnode = node.SelectSingleNode("tickets");
                    if (childnode != null)
                    {
                        item.tickets = int.Parse(childnode.InnerText);
                    }
                    childnode = node.SelectSingleNode("item/name");
                    if (childnode != null)
                    {
                        item.itemname = childnode.InnerText;
                    }
                    childnode = node.SelectSingleNode("item/num");
                    if (childnode != null)
                    {
                        item.itemcount = int.Parse(childnode.InnerText);
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public int getTicketsReward(ProtocolMgr protocol, ILogger logger, TicketItem item, int num)
        {
            string url = "/root/tickets!getTicketsReward.action";
            string data = string.Format("rewardId={0}&num={1}", item.id, num);
            ServerResult xml = protocol.postXml(url, data, "点券兑换");
            if (xml != null && xml.CmdSucceed)
            {
                logInfo(logger, string.Format("商品【{0}x{1}】, 兑换{2}次消耗点券{3}", item.itemname, item.itemcount, num, item.tickets * num));
                return num;
            }
            return 0;
        }

        public int ticketGetInfo(ProtocolMgr protocol, ILogger logger, ref int total_ticket, ref List<TicketItem> list)
        {
            list.Clear();
            string url = "/root/tickets.action";
            ServerResult xml = protocol.getXml(url, "获取玩家点券信息");
            if (xml == null) return 1;
            if (!xml.CmdSucceed) return 10;
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tickets");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out total_ticket);
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/rewards/reward");
            foreach (XmlNode node in xmlNodeList)
            {
                int id = 0;
                XmlNode childnode = node.SelectSingleNode("id");
                if (childnode != null)
                {
                    id = int.Parse(childnode.InnerText);
                }
                if (id == -1 || id == -2)
                {
                    TicketItem item = new TicketItem();
                    item.id = id;
                    childnode = node.SelectSingleNode("playerlevel");
                    if (childnode != null)
                    {
                        item.playerlevel = int.Parse(childnode.InnerText);
                    }
                    childnode = node.SelectSingleNode("tickets");
                    if (childnode != null)
                    {
                        item.tickets = int.Parse(childnode.InnerText);
                    }
                    childnode = node.SelectSingleNode("item/name");
                    if (childnode != null)
                    {
                        item.itemname = childnode.InnerText;
                    }
                    childnode = node.SelectSingleNode("item/num");
                    if (childnode != null)
                    {
                        item.itemcount = int.Parse(childnode.InnerText);
                    }
                    list.Add(item);
                }
            }
            return 0;
        }

        public bool ticketExchangeMoney(ProtocolMgr protocol, ILogger logger, int times)
        {
            while (times > 0)
            {
                string url = "/root/tickets!getTicketsReward.action";
                string data = string.Format("rewardId={0}&num={1}", 2, 10);
                ServerResult xml = protocol.postXml(url, data, "点券兑换银币");
                if (xml == null || !xml.CmdSucceed)
                {
                    logInfo(logger, string.Format("点券兑换银币10000000, 失败:{0}", xml.CmdError));
                    return false;
                }
                logInfo(logger, string.Format("点券兑换银币10000000, 消耗点券10000"));
                times--;
            }
            
            return true;
        }

        public bool ticketExchangeBigHero(ProtocolMgr protocol, ILogger logger, TicketItem item)
        {
            string url = "/root/tickets!getTicketsReward.action";
            string data = string.Format("rewardId={0}&num={1}", item.id, item.itemcount);
            ServerResult xml = protocol.postXml(url, data, "点券兑换大将令");
            if (xml != null && xml.CmdSucceed)
            {
                logInfo(logger, string.Format("点券兑换大将令[{0}]x{1}, 消耗点券{2}", item.itemname, item.itemcount, item.tickets));
                return true;
            }
            return false;
        }

        public int ticketExchangeWeapon(ProtocolMgr protocol, ILogger logger, int rewardId, string rewardName, int count)
        {
            string url = "/root/tickets!getTicketsReward.action";
            string data = string.Format("rewardId={0}&num={1}", rewardId, count);
            string text = string.Format("点券兑换{0}*{1}", rewardName, count);
            ServerResult serverResult = protocol.postXml(url, data, text);
            if (serverResult == null) return 1;
            if (!serverResult.CmdSucceed)return 10;
            XmlDocument cmdResult = serverResult.CmdResult;
            int num = 1000;
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/reward/cost");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out num);
            }
            base.logInfo(logger, string.Format("{0}, 消耗点券{1}", text, num * count));
            return 0;
        }
        #endregion

        #region 将军塔
        public void getGeneralTowerInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/mainCity!getGeneralTowerInfo.action";
            ServerResult xml = protocol.getXml(url, "将军塔");
            if (xml != null && xml.CmdSucceed)
            {
                //AstdLuaObject lua = new AstdLuaObject();
                //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
                //int buildingprogress = lua.GetIntValue("results.generaltower.buildingprogress");//进度
                //int leveluprequirement = lua.GetIntValue("results.generaltower.leveluprequirement");//升级要求
                //int buildingstone = lua.GetIntValue("results.generaltower.buildingstone");//筑造石
                //int gemstonenum = lua.GetIntValue("results.generaltower.gemstonenum");//今天获得宝石
                //int generaltowerlevel = lua.GetIntValue("results.generaltower.generaltowerlevel");//等级
                int buildingprogress = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/buildingprogress"));
                int leveluprequirement = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/leveluprequirement"));
                int buildingstone = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/buildingstone"));
                int gemstonenum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/gemstonenum"));
                int generaltowerlevel = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/generaltowerlevel"));

                while (buildingstone > 0)
                {
                    useBuildingStone(protocol, logger, out buildingstone);
                }
            }
        }

        public void useBuildingStone(ProtocolMgr protocol, ILogger logger, out int buildingstone)
        {
            buildingstone = 0;
            string url = "/root/mainCity!useBuildingStone.action";
            ServerResult xml = protocol.getXml(url, "");
            if (xml != null && xml.CmdSucceed)
            {
                //AstdLuaObject lua = new AstdLuaObject();
                //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
                //int levelup = lua.GetIntValue("results.generaltower.levelup");//升级
                //int addprogress = lua.GetIntValue("results.generaltower.addprogress");//增加进度
                //int buildingprogress = lua.GetIntValue("results.generaltower.buildingprogress");//进度
                //buildingstone = lua.GetIntValue("results.generaltower.buildingstone");//筑造石
                int levelup = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/levelup"));
                int addprogress = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/addprogress"));
                int buildingprogress = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/buildingprogress"));
                buildingstone = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaltower/buildingstone"));
                string leveluptext = "";
                if (levelup == 1)
                {
                    leveluptext = "将军塔升级，";
                }
                logInfo(logger, string.Format("将军塔进度增加{0}，{1}当前进度{2}，剩余筑造石{3}", addprogress, leveluptext, buildingprogress, buildingstone));
            }
        }
        #endregion

        #region 高级炼制工坊
        public int getRefineBintieFactory(ProtocolMgr protocol, ILogger logger, User user, int mode)
        {
            string url = "/root/refine!getRefineBintieFactory.action";
            ServerResult result = protocol.getXml(url, "高级炼制工坊-信息");
            if (result == null) return 1;
            if (!result.CmdSucceed) return 10;

            int remainhigh = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/remainhigh"));
            if (remainhigh <= 0 && user._refine_task_num <= 0) return 2;

            if (doRefineBintieFactory(protocol, logger, user, mode)) return 0;
            return 10;
        }

        public bool doRefineBintieFactory(ProtocolMgr protocol, ILogger logger, User user, int mode)
        {
            if (user.Silver < 10000000)
            {
                ticketExchangeMoney(protocol, logger, 5);
            }

            string url = "/root/refine!doRefineBintieFactory.action";
            string data = string.Format("mode={0}", mode);
            ServerResult result = protocol.postXml(url, data, "高级炼制工坊-炼制");
            if (result == null || !result.CmdSucceed) return false;

            user._refine_task_num--;
            int cri = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/cri"));
            //int basebintie = XmlHelper.GetValue<int>(result.CmdResult.SelectSingleNode("/results/basebintie"));
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(result.CmdResult.SelectSingleNode("/results/rewardinfo"));
            //string tips = string.Format("高级炼制，获得镔铁+{0}", basebintie * cri);
            //if (cri > 1) tips = string.Format("高级炼制，{0}倍暴击，获得镔铁+{1}", cri, basebintie * cri);
            string tips = string.Format("高级炼制，获得{0}", reward.ToString());
            if (cri > 1) tips = string.Format("高级炼制，{0}倍暴击，获得{1}", cri, reward.ToString());
            logInfo(logger, tips);
            return true;
        }
        #endregion

        #region 西域通商
        public int getWesternTradeInfo(ProtocolMgr protocol, ILogger logger, User user, int limit_active)
        {
            string url = "/root/caravan!getWesternTradeInfo.action";
            ServerResult xml = protocol.getXml(url, "西域通商");
            if (xml == null || !xml.CmdSucceed) return 10;

            int eventtype = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/eventtype"), 0);
            if (eventtype == 1)
            {
                int pos = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/pos"), -1);
                if (pos == 0)
                {
                    if (!getKingReward(protocol, logger, 0, 1)) return 10;
                    else return 0;
                }
                else if (pos > 0)
                {
                    if (!getKingReward(protocol, logger, -1, 1)) return 10;
                    else return 0;
                }
            }
            else if (eventtype == 2)
            {
                if (!getTraderReward(protocol, logger, -1)) return 10;
                else return 0;
            }

            List<TradeInfo> list;
            int nowplace = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/nowplace"));
            int needclicknext = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/needclicknext"), 0);
            if (needclicknext == 1)
            {
                list = nextPlace(protocol, logger);
            }
            else
            {
                list = XmlHelper.GetClassList<TradeInfo>(xml.CmdResult.SelectNodes("/results/tradeinfo"));
            }
            
            if (list != null)
            {
                list.Sort();
                foreach (TradeInfo item in list)
                {
                    if (user.CurMovable >= item.active && item.active <= limit_active)
                    {
                        if (westernTrade(protocol, logger, item))
                        {
                            getWesternTradeReward(protocol, logger);
                        }
                        break;
                    }
                }
            }

            return 10;
        }

        public List<TradeInfo> nextPlace(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/caravan!nextPlace.action";
            ServerResult xml = protocol.getXml(url, "西域通商-下一站");
            if (xml == null || !xml.CmdSucceed) return null;

            logInfo(logger, string.Format("西域通商, 进入下一个城市"));
            return XmlHelper.GetClassList<TradeInfo>(xml.CmdResult.SelectNodes("/results/tradeinfo"));
        }

        public bool westernTrade(ProtocolMgr protocol, ILogger logger, TradeInfo item)
        {
            string url = "/root/caravan!westernTrade.action";
            string data = string.Format("tradeId={0}", item.id);
            ServerResult xml = protocol.postXml(url, data, "西域通商-通商");
            if (xml == null || !xml.CmdSucceed) return false;

            StringBuilder sb = new StringBuilder("西域通商, 获取宝箱");
            XmlNodeList nodelist = xml.CmdResult.SelectNodes("/results/box");
            foreach (XmlNode node in nodelist)
            {
                RewardInfo info = new RewardInfo();
                info.handleXmlNode(node.SelectSingleNode("rewardinfo"));
                sb.AppendFormat(", {0}", info.ToString());
            }
            logInfo(logger, sb.ToString());
            return true;
        }

        public bool getWesternTradeReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/caravan!getWesternTradeReward.action";
            string data = string.Format("isdouble=0");
            ServerResult xml = protocol.postXml(url, data, "西域通商-领取奖励");
            if (xml == null || !xml.CmdSucceed) return false;

            StringBuilder sb = new StringBuilder("西域通商, 打开宝箱");
            XmlNodeList nodelist = xml.CmdResult.SelectNodes("/results/rewardinfo");
            foreach (XmlNode node in nodelist)
            {
                RewardInfo info = new RewardInfo();
                info.handleXmlNode(node);
                sb.AppendFormat(", {0}", info.ToString());
            }
            logInfo(logger, sb.ToString());
            return true;
        }

        public bool getKingReward(ProtocolMgr protocol, ILogger logger, int isdouble, int pos)
        {
            string url = "/root/caravan!getKingReward.action";
            string data = string.Format("isdouble={0}&pos={1}", isdouble, pos);
            ServerResult xml = protocol.postXml(url, data, "西域通商-国王事件");
            if (xml == null || !xml.CmdSucceed) return false;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            if (!string.IsNullOrEmpty(reward.ToString())) logInfo(logger, string.Format("西域通商-国王事件, 打开宝箱, 获得{0}", reward.ToString()));
            return true;
        }

        public bool getTraderReward(ProtocolMgr protocol, ILogger logger, int isBuy)
        {
            string url = "/root/caravan!getTraderReward.action";
            string data = string.Format("isBuy={0}", isBuy);
            ServerResult xml = protocol.postXml(url, data, "西域通商-神秘商人事件");
            if (xml == null || !xml.CmdSucceed) return false;

            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            if (!string.IsNullOrEmpty(reward.ToString())) logInfo(logger, string.Format("西域通商-神秘商人事件, 打开宝箱, 获得{0}", reward.ToString()));
            return true;
        }

        #endregion
    }
}
