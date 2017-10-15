using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using com.lover.astd.common.model.misc;
using com.lover.common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Xml;
using com.lover.astd.common.config;
using com.lover.astd.common.model.activity;
using com.lover.astd.common.activity;

namespace com.lover.astd.common.logic
{
    public class ActivityMgr : MgrBase
    {
        /// <summary>
        /// 雪地通商
        /// </summary>
        private ActivitySnowTrading snowTrading_;
        /// <summary>
        /// 草船借箭
        /// </summary>
        private BorrowingArrowsEvent borrowingArrows_;
        /// <summary>
        /// 端午活动
        /// </summary>
        private ArrestEvent arrestEvent_;
        /// <summary>
        /// 新国庆阅兵
        /// </summary>
        private ParadeEvent paradeEvent_;

        public class KfzbItem
        {
            public int pos;

            public int price;

            public string name;

            public int num;

            public int itemtype;

            public int marketid;

            public int remainnum;

            public int totalnum;

            public void fill(XmlNode node)
            {
                XmlNodeList childNodes = node.ChildNodes;
                foreach (XmlNode xmlNode in childNodes)
                {
                    bool flag = xmlNode.Name == "pos";
                    if (flag)
                    {
                        int.TryParse(xmlNode.InnerText, out this.pos);
                    }
                    else
                    {
                        bool flag2 = xmlNode.Name == "price";
                        if (flag2)
                        {
                            string s = xmlNode.InnerText.Replace("res:gold:", "");
                            int.TryParse(s, out this.price);
                            bool flag3 = this.price == 0;
                            if (flag3)
                            {
                                this.price = 100;
                            }
                        }
                        else
                        {
                            bool flag4 = xmlNode.Name == "name";
                            if (flag4)
                            {
                                this.name = xmlNode.InnerText;
                                bool flag5 = this.name == "银币";
                                if (flag5)
                                {
                                    this.itemtype = 1;
                                }
                                else
                                {
                                    bool flag6 = this.name == "玉石";
                                    if (flag6)
                                    {
                                        this.itemtype = 2;
                                    }
                                    else
                                    {
                                        bool flag7 = this.name == "宝石";
                                        if (flag7)
                                        {
                                            this.itemtype = 3;
                                        }
                                        else
                                        {
                                            bool flag8 = this.name == "无敌将军炮" || this.name == "玄霆角" || this.name == "蟠龙华盖";
                                            if (flag8)
                                            {
                                                this.itemtype = 4;
                                            }
                                            else
                                            {
                                                this.itemtype = 5;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool flag9 = xmlNode.Name == "num";
                                if (flag9)
                                {
                                    int.TryParse(xmlNode.InnerText, out this.num);
                                }
                                else
                                {
                                    bool flag10 = xmlNode.Name == "marketid";
                                    if (flag10)
                                    {
                                        int.TryParse(xmlNode.InnerText, out this.marketid);
                                    }
                                    else
                                    {
                                        bool flag11 = xmlNode.Name == "remainnum";
                                        if (flag11)
                                        {
                                            int.TryParse(xmlNode.InnerText, out this.remainnum);
                                        }
                                        else
                                        {
                                            bool flag12 = xmlNode.Name == "totalnum";
                                            if (flag12)
                                            {
                                                int.TryParse(xmlNode.InnerText, out this.totalnum);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public string desc()
            {
                bool flag = this.itemtype == 1;
                string result;
                if (flag)
                {
                    result = string.Format("[{0}金币]=>[{1}{2}]", this.price, CommonUtils.getShortReadable((long)this.num), this.name);
                }
                else
                {
                    result = string.Format("第{0}列 [{1}金币]=>[{2}{3}]", new object[]
					{
						this.pos,
						this.price,
						this.num,
						this.name
					});
                }
                return result;
            }
        }

        private class Slave
        {
            public int id;

            public string name;

            public int baoshi;

            public int state;
        }

        private class RankRoom
        {
            public int rank;

            public int nation;

            public string playername;

            public int serverid;

            public string servername;

            public int nationattr;

            public int weinum;

            public int shunum;

            public int wunum;

            public int bufnum;

            public bool is_nation_dinner()
            {
                return this.nation == this.nationattr;
            }
        }

        private class ArchTeam
        {
            public string teamid;

            public string teamname;

            public int nation;

            public string types;

            public string qualities;
        }

        private class ArchMember
        {
            public string name;

            public int quality;

            public int type;

            public bool state;

            public bool creater;

            public bool self;
        }

        protected class GameGrid
        {
            public int pos;

            public string name;

            public int star;

            public string info;
        }

        protected class Question
        {
            public string question;

            public string answer1;

            public string answer2;

            public int answer;

            public Question(string q, string a1, string a2, int ans)
            {
                this.question = q;
                this.answer1 = a1;
                this.answer2 = a2;
                this.answer = ans;
            }
        }

        private int[] _gemFlopLevelRewards = new int[]
		{
			6,
			12,
			20,
			30,
			60
		};

        private int _gemFlopMaxGemLevel = 5;

        private int _gemFlopUpgradeGold = 8;

        protected List<ActivityMgr.Question> _question_db = new List<ActivityMgr.Question>();

        private int _superFanpai_idx = 0;

        public ActivityMgr(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.SteelBlue;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
            snowTrading_ = new ActivitySnowTrading(tmrMgr, factory);
            borrowingArrows_ = new BorrowingArrowsEvent(tmrMgr, factory);
            arrestEvent_ = new ArrestEvent(tmrMgr, factory);
            paradeEvent_ = new ParadeEvent(tmrMgr, factory);
        }

        private bool doSilverFlopStep(char[,] s, int pos)
        {
            bool flag = pos == 0;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                switch (pos)
                {
                    case 1:
                        {
                            bool flag2 = s[0, 2] == s[0, 1] && s[0, 2] == s[0, 0];
                            if (flag2)
                            {
                                result = false;
                                return result;
                            }
                            char c = s[0, 0];
                            s[0, 0] = s[0, 1];
                            s[0, 1] = s[0, 2];
                            s[0, 2] = c;
                            break;
                        }
                    case 2:
                        {
                            bool flag3 = s[1, 2] == s[1, 1] && s[1, 2] == s[1, 0];
                            if (flag3)
                            {
                                result = false;
                                return result;
                            }
                            char c2 = s[1, 0];
                            s[1, 0] = s[1, 1];
                            s[1, 1] = s[1, 2];
                            s[1, 2] = c2;
                            break;
                        }
                    case 3:
                        {
                            bool flag4 = s[2, 2] == s[2, 1] && s[2, 2] == s[2, 0];
                            if (flag4)
                            {
                                result = false;
                                return result;
                            }
                            char c3 = s[2, 0];
                            s[2, 0] = s[2, 1];
                            s[2, 1] = s[2, 2];
                            s[2, 2] = c3;
                            break;
                        }
                    case 4:
                        {
                            bool flag5 = s[0, 0] == s[1, 0] && s[0, 0] == s[2, 0];
                            if (flag5)
                            {
                                result = false;
                                return result;
                            }
                            char c4 = s[2, 0];
                            s[2, 0] = s[1, 0];
                            s[1, 0] = s[0, 0];
                            s[0, 0] = c4;
                            break;
                        }
                    case 5:
                        {
                            bool flag6 = s[0, 1] == s[1, 1] && s[0, 1] == s[2, 1];
                            if (flag6)
                            {
                                result = false;
                                return result;
                            }
                            char c5 = s[2, 1];
                            s[2, 1] = s[1, 1];
                            s[1, 1] = s[0, 1];
                            s[0, 1] = c5;
                            break;
                        }
                    case 6:
                        {
                            bool flag7 = s[0, 2] == s[1, 2] && s[0, 2] == s[2, 2];
                            if (flag7)
                            {
                                result = false;
                                return result;
                            }
                            char c6 = s[2, 2];
                            s[2, 2] = s[1, 2];
                            s[1, 2] = s[0, 2];
                            s[0, 2] = c6;
                            break;
                        }
                    case 7:
                        {
                            bool flag8 = s[2, 2] == s[2, 1] && s[2, 2] == s[2, 0];
                            if (flag8)
                            {
                                result = false;
                                return result;
                            }
                            char c7 = s[2, 2];
                            s[2, 2] = s[2, 1];
                            s[2, 1] = s[2, 0];
                            s[2, 0] = c7;
                            break;
                        }
                    case 8:
                        {
                            bool flag9 = s[1, 2] == s[1, 1] && s[1, 2] == s[1, 0];
                            if (flag9)
                            {
                                result = false;
                                return result;
                            }
                            char c8 = s[1, 2];
                            s[1, 2] = s[1, 1];
                            s[1, 1] = s[1, 0];
                            s[1, 0] = c8;
                            break;
                        }
                    case 9:
                        {
                            bool flag10 = s[0, 2] == s[0, 1] && s[0, 2] == s[0, 0];
                            if (flag10)
                            {
                                result = false;
                                return result;
                            }
                            char c9 = s[0, 2];
                            s[0, 2] = s[0, 1];
                            s[0, 1] = s[0, 0];
                            s[0, 0] = c9;
                            break;
                        }
                    case 10:
                        {
                            bool flag11 = s[0, 2] == s[1, 2] && s[0, 2] == s[2, 2];
                            if (flag11)
                            {
                                result = false;
                                return result;
                            }
                            char c10 = s[0, 2];
                            s[0, 2] = s[1, 2];
                            s[1, 2] = s[2, 2];
                            s[2, 2] = c10;
                            break;
                        }
                    case 11:
                        {
                            bool flag12 = s[0, 1] == s[1, 1] && s[0, 1] == s[2, 1];
                            if (flag12)
                            {
                                result = false;
                                return result;
                            }
                            char c11 = s[0, 1];
                            s[0, 1] = s[1, 1];
                            s[1, 1] = s[2, 1];
                            s[2, 1] = c11;
                            break;
                        }
                    case 12:
                        {
                            bool flag13 = s[0, 0] == s[1, 0] && s[0, 0] == s[2, 0];
                            if (flag13)
                            {
                                result = false;
                                return result;
                            }
                            char c12 = s[0, 0];
                            s[0, 0] = s[1, 0];
                            s[1, 0] = s[2, 0];
                            s[2, 0] = c12;
                            break;
                        }
                }
                result = true;
            }
            return result;
        }

        private int getSilverFlopArrayCombo(char[,] s, out string result_string)
        {
            int num = 0;
            bool flag = s[0, 0] == s[0, 1] && s[0, 0] == s[0, 2];
            int num2;
            if (flag)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag2 = s[1, 0] == s[1, 1] && s[1, 0] == s[1, 2];
            if (flag2)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag3 = s[2, 0] == s[2, 1] && s[2, 0] == s[2, 2];
            if (flag3)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag4 = s[0, 0] == s[1, 0] && s[0, 0] == s[2, 0];
            if (flag4)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag5 = s[0, 1] == s[1, 1] && s[0, 1] == s[2, 1];
            if (flag5)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag6 = s[0, 2] == s[1, 2] && s[0, 2] == s[2, 2];
            if (flag6)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag7 = s[0, 0] == s[1, 1] && s[0, 0] == s[2, 2];
            if (flag7)
            {
                num2 = num;
                num = num2 + 1;
            }
            bool flag8 = s[2, 0] == s[1, 1] && s[2, 0] == s[0, 2];
            if (flag8)
            {
                num2 = num;
                num = num2 + 1;
            }
            result_string = "";
            for (int i = 0; i < 3; i = num2 + 1)
            {
                for (int j = 0; j < 3; j = num2 + 1)
                {
                    result_string += s[i, j].ToString();
                    num2 = j;
                }
                num2 = i;
            }
            return num;
        }

        public int handleSilverFlopInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/giftEvent!getCopperEventGiftInfo.action";
            ServerResult xml = protocol.getXml(url, "获取银币翻牌信息");
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
                    int value = 0;
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    this.handleSilverFlopNode(protocol, logger, cmdResult, ref value, ref num, ref num2, ref num3);
                    bool flag3 = num == 0;
                    if (flag3)
                    {
                        result = 2;
                    }
                    else
                    {
                        int num4 = 20;
                        while (num > 0 && num4 > 0)
                        {
                            int num5 = num4;
                            num4 = num5 - 1;
                            bool use_double = num3 > 0;
                            string text = Convert.ToString(value, 2);
                            bool flag4 = text.Length < 9;
                            if (flag4)
                            {
                                int i = 0;
                                int num6 = 9 - text.Length;
                                while (i < num6)
                                {
                                    text = "0" + text;
                                    num5 = i;
                                    i = num5 + 1;
                                }
                            }
                            string final_result = "";
                            int num7 = 0;
                            int step = 0;
                            int step2 = 0;
                            for (int j = 0; j < 13; j = num5 + 1)
                            {
                                char[,] array = new char[3, 3];
                                for (int k = 0; k < 3; k = num5 + 1)
                                {
                                    array[k, 0] = text[k * 3];
                                    array[k, 1] = text[k * 3 + 1];
                                    array[k, 2] = text[k * 3 + 2];
                                    num5 = k;
                                }
                                bool flag5 = this.doSilverFlopStep(array, j);
                                int num8;
                                if (flag5)
                                {
                                    num8 = j;
                                }
                                else
                                {
                                    num8 = 0;
                                }
                                string text2;
                                int silverFlopArrayCombo = this.getSilverFlopArrayCombo(array, out text2);
                                bool flag6 = silverFlopArrayCombo == 8;
                                if (flag6)
                                {
                                    this.getSilverFlopReward(protocol, logger, j, 0, text2, silverFlopArrayCombo, use_double, ref value, ref num, ref num2, ref num3);
                                    bool flag7 = num == 0;
                                    if (flag7)
                                    {
                                        result = 2;
                                        return result;
                                    }
                                }
                                else
                                {
                                    bool flag8 = silverFlopArrayCombo > num7;
                                    if (flag8)
                                    {
                                        final_result = text2;
                                        num7 = silverFlopArrayCombo;
                                        step = num8;
                                        step2 = 0;
                                    }
                                }
                                bool flag9 = num8 != 0;
                                if (flag9)
                                {
                                    for (int l = 0; l < 13; l = num5 + 1)
                                    {
                                        char[,] array2 = new char[3, 3];
                                        for (int m = 0; m < 3; m = num5 + 1)
                                        {
                                            for (int n = 0; n < 3; n = num5 + 1)
                                            {
                                                array2[m, n] = array[m, n];
                                                num5 = n;
                                            }
                                            num5 = m;
                                        }
                                        bool flag10 = this.doSilverFlopStep(array2, l);
                                        int num9;
                                        if (flag10)
                                        {
                                            num9 = l;
                                        }
                                        else
                                        {
                                            num9 = 0;
                                        }
                                        silverFlopArrayCombo = this.getSilverFlopArrayCombo(array2, out text2);
                                        bool flag11 = silverFlopArrayCombo == 8;
                                        if (flag11)
                                        {
                                            this.getSilverFlopReward(protocol, logger, j, l, text2, silverFlopArrayCombo, use_double, ref value, ref num, ref num2, ref num3);
                                            bool flag12 = num == 0;
                                            if (flag12)
                                            {
                                                result = 2;
                                                return result;
                                            }
                                        }
                                        else
                                        {
                                            bool flag13 = silverFlopArrayCombo > num7;
                                            if (flag13)
                                            {
                                                final_result = text2;
                                                num7 = silverFlopArrayCombo;
                                                step = num8;
                                                step2 = num9;
                                            }
                                        }
                                        num5 = l;
                                    }
                                }
                                num5 = j;
                            }
                            this.getSilverFlopReward(protocol, logger, step, step2, final_result, num7, use_double, ref value, ref num, ref num2, ref num3);
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        private void getSilverFlopReward(ProtocolMgr protocol, ILogger logger, int step1, int step2, string final_result, int combo, bool use_double, ref int _squareNow, ref int _remainNum, ref int _baseSilver, ref int _remainDoubleNum)
        {
            string url = "/root/giftEvent!getCopperEventReward.action";
            string text = string.Format("step2={0}&square={1}&combo={2}&step1={3}", new object[]
			{
				step2,
				final_result,
				combo,
				step1
			});
            if (use_double)
            {
                text += "&doubleCard=1";
            }
            ServerResult serverResult = protocol.postXml(url, text, "获取银币翻牌奖励");
            bool flag = serverResult == null;
            if (!flag)
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (!flag2)
                {
                    if (use_double)
                    {
                        base.logInfo(logger, string.Format("双倍银币翻牌,最终模型为[{0}],Combo为[{1}],翻动分别为[{2}, {3}], 获得银币[{4}]", new object[]
						{
							final_result,
							combo,
							step1,
							step2,
							2 * _baseSilver * (combo + 1)
						}));
                    }
                    else
                    {
                        base.logInfo(logger, string.Format("单倍银币翻牌,最终模型为[{0}],Combo为[{1}],翻动分别为[{2}, {3}], 获得银币[{4}]", new object[]
						{
							final_result,
							combo,
							step1,
							step2,
							_baseSilver * (combo + 1)
						}));
                    }
                    XmlDocument cmdResult = serverResult.CmdResult;
                    this.handleSilverFlopNode(protocol, logger, cmdResult, ref _squareNow, ref _remainNum, ref _baseSilver, ref _remainDoubleNum);
                }
            }
        }

        private void handleSilverFlopNode(ProtocolMgr protocol, ILogger logger, XmlDocument xml, ref int _squareNow, ref int _remainNum, ref int _baseSilver, ref int _remainDoubleNum)
        {
            XmlNode xmlNode = xml.SelectSingleNode("/results/square");
            bool flag = xmlNode != null;
            if (flag)
            {
                int.TryParse(xmlNode.InnerText, out _squareNow);
            }
            else
            {
                _squareNow = 0;
            }
            XmlNode xmlNode2 = xml.SelectSingleNode("/results/remainnum");
            bool flag2 = xmlNode2 != null;
            if (flag2)
            {
                int.TryParse(xmlNode2.InnerText, out _remainNum);
            }
            else
            {
                _remainNum = 0;
            }
            XmlNode xmlNode3 = xml.SelectSingleNode("/results/basecopper");
            bool flag3 = xmlNode3 != null;
            if (flag3)
            {
                int.TryParse(xmlNode3.InnerText, out _baseSilver);
            }
            else
            {
                _baseSilver = 0;
            }
            XmlNode xmlNode4 = xml.SelectSingleNode("/results/freedouble");
            bool flag4 = xmlNode4 != null;
            if (flag4)
            {
                int.TryParse(xmlNode4.InnerText, out _remainDoubleNum);
            }
            else
            {
                _remainDoubleNum = 0;
            }
            base.logInfo(logger, string.Format("银币翻牌还剩[{0}]次, 剩余翻倍[{1}]次", _remainNum, _remainDoubleNum));
        }

        private string getGemPatternLevelString(int level_0, int level_1, int level_2)
        {
            return string.Format("{0},{1},{2}", level_0, level_1, level_2);
        }

        private string getGemPatternString(int level_0, int level_1, int level_2)
        {
            string text = "";
            string[] array = new string[]
			{
				"蓝",
				"绿",
				"黄",
				"红",
				"紫"
			};
            bool flag = level_0 > 0 && level_0 <= array.Length;
            if (flag)
            {
                text += array[level_0 - 1];
            }
            bool flag2 = level_1 > 0 && level_1 <= array.Length;
            if (flag2)
            {
                text += array[level_1 - 1];
            }
            bool flag3 = level_2 > 0 && level_2 <= array.Length;
            if (flag3)
            {
                text += array[level_2 - 1];
            }
            return text;
        }

        private string getGemPatternString(string orig_pattern)
        {
            string[] array = orig_pattern.Split(new char[]
			{
				','
			});
            List<int> list = new List<int>();
            string[] array2 = array;
            int num2;
            for (int i = 0; i < array2.Length; i = num2 + 1)
            {
                string text = array2[i];
                bool flag = text != null && text.Length != 0;
                if (flag)
                {
                    int num = 0;
                    int.TryParse(text, out num);
                    bool flag2 = num > 0;
                    if (flag2)
                    {
                        list.Add(num);
                    }
                }
                num2 = i;
            }
            bool flag3 = list.Count < 3;
            string result;
            if (flag3)
            {
                result = "";
            }
            else
            {
                result = this.getGemPatternString(list[0], list[1], list[2]);
            }
            return result;
        }

        public string findGemPattern(int max_upgrade, double gem_price, int gold_available, int _upgradeGold, string orig_pattern, bool use_double, out int final_usegold, out int final_gemcount)
        {
            final_usegold = 0;
            final_gemcount = 0;
            string[] array = orig_pattern.Split(new char[]
			{
				','
			});
            List<int> list = new List<int>();
            string[] array2 = array;
            int num2;
            for (int i = 0; i < array2.Length; i = num2 + 1)
            {
                string text = array2[i];
                bool flag = text != null && text.Length != 0;
                if (flag)
                {
                    int num = 0;
                    int.TryParse(text, out num);
                    bool flag2 = num > 0;
                    if (flag2)
                    {
                        list.Add(num);
                    }
                }
                num2 = i;
            }
            bool flag3 = list.Count < 3;
            string result;
            if (flag3)
            {
                result = "";
            }
            else
            {
                int num3 = list[0];
                int num4 = list[1];
                int num5 = list[2];
                int num6 = 0;
                int patternGems = this.getPatternGems(num3, num4, num5, use_double, out num6);
                final_usegold = 0;
                final_gemcount = patternGems;
                string text2 = orig_pattern;
                bool flag4 = gold_available < _upgradeGold;
                if (flag4)
                {
                    result = text2;
                }
                else
                {
                    int num7 = this._gemFlopMaxGemLevel - list[0];
                    int num8 = this._gemFlopMaxGemLevel - list[1];
                    int num9 = this._gemFlopMaxGemLevel - list[2];
                    for (int j = 0; j <= num7; j = num2 + 1)
                    {
                        int num10 = j;
                        bool flag5 = num10 > max_upgrade;
                        if (flag5)
                        {
                            break;
                        }
                        int num11 = num10 * _upgradeGold;
                        bool flag6 = gold_available < num11;
                        if (flag6)
                        {
                            break;
                        }
                        int patternGems2 = this.getPatternGems(num3 + j, num4, num5, use_double, out num6);
                        int num12 = patternGems2 - patternGems;
                        bool flag7 = num12 > 0 && 1.0 * (double)num11 / (double)num12 <= gem_price && final_gemcount < patternGems2;
                        if (flag7)
                        {
                            final_usegold = num11;
                            final_gemcount = patternGems2;
                            text2 = this.getGemPatternLevelString(num3 + j, num4, num5);
                        }
                        for (int k = 0; k <= num8; k = num2 + 1)
                        {
                            num10 = j + k;
                            bool flag8 = num10 > max_upgrade;
                            if (flag8)
                            {
                                break;
                            }
                            num11 = num10 * _upgradeGold;
                            bool flag9 = gold_available < num11;
                            if (flag9)
                            {
                                break;
                            }
                            patternGems2 = this.getPatternGems(num3 + j, num4 + k, num5, use_double, out num6);
                            num12 = patternGems2 - patternGems;
                            bool flag10 = num12 > 0 && 1.0 * (double)num11 / (double)num12 <= gem_price && final_gemcount < patternGems2;
                            if (flag10)
                            {
                                final_usegold = num11;
                                final_gemcount = patternGems2;
                                text2 = this.getGemPatternLevelString(num3 + j, num4 + k, num5);
                            }
                            for (int l = 0; l <= num9; l = num2 + 1)
                            {
                                num10 = j + k + l;
                                bool flag11 = num10 > max_upgrade;
                                if (flag11)
                                {
                                    break;
                                }
                                num11 = num10 * _upgradeGold;
                                bool flag12 = gold_available < num11;
                                if (flag12)
                                {
                                    break;
                                }
                                patternGems2 = this.getPatternGems(num3 + j, num4 + k, num5 + l, use_double, out num6);
                                num12 = patternGems2 - patternGems;
                                bool flag13 = num12 > 0 && 1.0 * (double)num11 / (double)num12 <= gem_price && final_gemcount < patternGems2;
                                if (flag13)
                                {
                                    final_usegold = num11;
                                    final_gemcount = patternGems2;
                                    text2 = this.getGemPatternLevelString(num3 + j, num4 + k, num5 + l);
                                }
                                num2 = l;
                            }
                            num2 = k;
                        }
                        num2 = j;
                    }
                    result = text2;
                }
            }
            return result;
        }

        private int getPatternGems(int level_0, int level_1, int level_2, bool useDouble, out int hasCombo)
        {
            hasCombo = 0;
            int num = 0;
            bool flag = this._gemFlopLevelRewards.Length >= level_0;
            if (flag)
            {
                num += this._gemFlopLevelRewards[level_0 - 1];
            }
            bool flag2 = this._gemFlopLevelRewards.Length >= level_1;
            if (flag2)
            {
                num += this._gemFlopLevelRewards[level_1 - 1];
            }
            bool flag3 = this._gemFlopLevelRewards.Length >= level_2;
            if (flag3)
            {
                num += this._gemFlopLevelRewards[level_2 - 1];
            }
            if (useDouble)
            {
                num *= 2;
            }
            bool flag4 = level_0 == level_1 && level_0 == level_2;
            bool flag5 = flag4;
            int result;
            if (flag5)
            {
                hasCombo = 1;
                result = num * 3;
            }
            else
            {
                bool flag6 = false;
                int num2 = Math.Abs(level_0 - level_1);
                int num3 = Math.Abs(level_0 - level_2);
                int num4 = Math.Abs(level_1 - level_2);
                bool flag7 = num2 == 1 && num3 == 1 && num4 == 2;
                if (flag7)
                {
                    flag6 = true;
                }
                else
                {
                    bool flag8 = num2 == 1 && num3 == 2 && num4 == 1;
                    if (flag8)
                    {
                        flag6 = true;
                    }
                    else
                    {
                        bool flag9 = num2 == 2 && num3 == 1 && num4 == 1;
                        if (flag9)
                        {
                            flag6 = true;
                        }
                    }
                }
                bool flag10 = flag6;
                if (flag10)
                {
                    hasCombo = 2;
                    result = num * 3;
                }
                else
                {
                    result = num;
                }
            }
            return result;
        }

        public int handleGemFlopInfo(ProtocolMgr protocol, ILogger logger, double gem_price, int gold_available, int max_upgrade)
        {
            string url = "/root/gemCard!getGemCardInfo.action";
            ServerResult xml = protocol.getXml(url, "获取宝石翻牌信息");
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
                    string text = "";
                    int num = 0;
                    int num2 = 0;
                    this.handleGemFlopNode(protocol, logger, cmdResult, ref text, ref num, ref num2);
                    bool flag3 = num == 0;
                    if (flag3)
                    {
                        result = 2;
                    }
                    else
                    {
                        bool flag4 = num2 > 0;
                        int costGold = 0;
                        int gemcount = 0;
                        string final_pattern = this.findGemPattern(max_upgrade, gem_price, gold_available, this._gemFlopUpgradeGold, text, flag4, out costGold, out gemcount);
                        result = this.getGemFlopReward(protocol, logger, costGold, flag4, text, final_pattern, gemcount, ref text, ref num, ref num2);
                    }
                }
            }
            return result;
        }

        public int getGemFlopReward(ProtocolMgr protocol, ILogger logger, int costGold, bool useDouble, string origin_pattern, string final_pattern, int gemcount, ref string _origPattern, ref int _remainNum, ref int _freeDoubleNum)
        {
            string url = "/root/gemCard!receiveGem.action";
            string data = string.Format("cost={0}&doubleCard={1}&list={2}", costGold, useDouble ? 1 : 0, final_pattern);
            ServerResult serverResult = protocol.postXml(url, data, "获取宝石翻牌奖励");
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
                    base.logInfo(logger, string.Format("宝石翻牌,初始模型为[{0}], 最终模型为[{1}],花费[{2}]金币, 获得宝石[{3}]", new object[]
					{
						this.getGemPatternString(origin_pattern),
						this.getGemPatternString(final_pattern),
						costGold,
						gemcount
					}));
                    this.handleGemFlopNode(protocol, logger, serverResult.CmdResult, ref _origPattern, ref _remainNum, ref _freeDoubleNum);
                    result = 0;
                }
            }
            return result;
        }

        private void handleGemFlopNode(ProtocolMgr protocol, ILogger logger, XmlDocument xml, ref string _origPattern, ref int _remainNum, ref int _freeDoubleNum)
        {
            XmlNodeList xmlNodeList = xml.SelectNodes("/results/reward");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                bool flag = xmlNode != null && xmlNode.HasChildNodes;
                if (flag)
                {
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    int num = 0;
                    int num2 = 0;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        bool flag2 = xmlNode2.Name == "baoshilevel";
                        if (flag2)
                        {
                            int.TryParse(xmlNode2.InnerText, out num);
                        }
                        else
                        {
                            bool flag3 = xmlNode2.Name == "rewardbaoshinum";
                            if (flag3)
                            {
                                int.TryParse(xmlNode2.InnerText, out num2);
                            }
                        }
                    }
                    bool flag4 = num > 0 && num < this._gemFlopLevelRewards.Length;
                    if (flag4)
                    {
                        this._gemFlopLevelRewards[num - 1] = num2;
                    }
                }
            }
            XmlNode xmlNode3 = xml.SelectSingleNode("/results/gemcardinfo/upgradegold");
            bool flag5 = xmlNode3 != null;
            if (flag5)
            {
                int.TryParse(xmlNode3.InnerText, out this._gemFlopUpgradeGold);
            }
            else
            {
                this._gemFlopUpgradeGold = 0;
            }
            XmlNode xmlNode4 = xml.SelectSingleNode("/results/gemcardinfo/freetimes");
            bool flag6 = xmlNode4 != null;
            if (flag6)
            {
                int.TryParse(xmlNode4.InnerText, out _remainNum);
            }
            else
            {
                _remainNum = 0;
            }
            XmlNode xmlNode5 = xml.SelectSingleNode("/results/gemcardinfo/freedouble");
            bool flag7 = xmlNode5 != null;
            if (flag7)
            {
                int.TryParse(xmlNode5.InnerText, out _freeDoubleNum);
            }
            else
            {
                _freeDoubleNum = 0;
            }
            XmlNode xmlNode6 = xml.SelectSingleNode("/results/gemcardinfo/gemcardliststring");
            bool flag8 = xmlNode6 != null;
            if (flag8)
            {
                _origPattern = xmlNode6.InnerText;
            }
            base.logInfo(logger, string.Format("宝石翻牌还剩[{0}]次, 双倍次数还剩{1}次", _remainNum, _freeDoubleNum));
        }

        public int handle_SuperFanpai(ProtocolMgr protocol, ILogger logger, double gem_price, int gold_available, int max_buy_gold)
        {
            string url = "/root/superFanpai!getSuperFanpaiInfo.action";
            ServerResult xml = protocol.getXml(url, "获取超级翻牌信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }

            XmlDocument cmdResult = xml.CmdResult;
            int buyone = 1;
            int buyall = 20;
            int freetimes = 0;
            int superlv = 18;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/superfanpaiinfo/buyone");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out buyone);
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/superfanpaiinfo/buyall");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out buyall);
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/superfanpaiinfo/freetimes");
            if (xmlNode3 != null)
            {
                int.TryParse(xmlNode3.InnerText, out freetimes);
            }
            xmlNode3 = cmdResult.SelectSingleNode("/results/superfanpaiinfo/superlv");
            if (xmlNode3 != null)
            {
                int.TryParse(xmlNode3.InnerText, out superlv);
            }

            bool isfanpai = false;
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/superfanpaiinfo/isfanpai");
            if (xmlNode4 != null)
            {
                isfanpai = (xmlNode4.InnerText != "0");
            }
            if (isfanpai)
            {
                //Random random = new Random();
                //return this.SuperFanpai_fanOne(protocol, logger, random.Next(3) + 1);
                return this.SuperFanpai_fanOne(protocol, logger, _superFanpai_idx + 1);
            }

            int i = 0;
            int[] array_gemlevel = new int[3];
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/superfanpaiinfo/card/gemlevel");
            foreach (XmlNode xmlNode5 in xmlNodeList)
            {
                int num4 = 0;
                int.TryParse(xmlNode5.InnerText, out num4);
                array_gemlevel[i++] = num4;
            }

            i = 0;
            int[] array_gemnumber = new int[3];
            XmlNodeList xmlNodeList2 = cmdResult.SelectNodes("/results/superfanpaiinfo/card/gemnumber");
            foreach (XmlNode xmlNode6 in xmlNodeList2)
            {
                int num6 = 0;
                int.TryParse(xmlNode6.InnerText, out num6);
                array_gemnumber[i++] = num6;
            }

            int sum = 0;
            for (i = 0; i < 3; i++)
            {
                sum += (int)Math.Pow(2.0, (double)(array_gemlevel[i] - 1)) * array_gemnumber[i];
            }
            base.logInfo(logger, string.Format("当前超级翻牌还剩翻牌次数{0}次, 当前收益:[{1},{2},{3}]级, 全开{4}个宝石", new object[] { freetimes, array_gemlevel[0], array_gemlevel[1], array_gemlevel[2], sum }));

            int fanpai_baoshi_level = array_gemlevel[0];
            _superFanpai_idx = 0;
            for (int fanpai_idx = 1; fanpai_idx < 3; fanpai_idx++)
            {
                if (fanpai_baoshi_level > array_gemlevel[fanpai_idx])
                {
                    fanpai_baoshi_level = array_gemlevel[fanpai_idx];
                    _superFanpai_idx = fanpai_idx;
                }
            }

            if (freetimes == 0)
            {
                if (buyone <= max_buy_gold && max_buy_gold <= gold_available)
                {
                    return this.superFanpai_buyTimes(protocol, logger, buyone);
                }
                else
                {
                    return 2;
                }
            }

            bool all = true;
            for (i = 0; i < 3; i++)
            {
                if (array_gemlevel[i] < superlv)
                {
                    all = false;
                    break;
                }
            }
            //double price = (double)buyall * 1.0 / (double)sum;
            //if (price <= gem_price && buyall <= gold_available)
            if (all)
            {
                int result = this.SuperFanpai_getAll(protocol, logger);
                if (result == 0)
                {
                    base.logInfo(logger, "超级翻牌卡牌全开, 获得宝石+" + sum);
                    return 0;
                }
                else
                {
                    return result;
                }
            }

            int result1 = this.SuperFanpai_xiPai(protocol, logger);
            if (result1 == 0)
            {
                //Random random2 = new Random();
                //return this.SuperFanpai_fanOne(protocol, logger, random2.Next(3) + 1);
                return this.SuperFanpai_fanOne(protocol, logger, _superFanpai_idx + 1);
            }
            else
            {
                return result1;
            }
        }

        public int SuperFanpai_getAll(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/superFanpai!getAll.action";
            ServerResult xml = protocol.getXml(url, "超级翻牌卡牌全开");
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
                return 0;
            }
        }

        public int SuperFanpai_xiPai(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/superFanpai!xiPai.action";
            ServerResult xml = protocol.getXml(url, "超级翻牌卡牌洗牌");
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
                    base.logInfo(logger, "超级翻牌卡牌洗牌成功");
                    result = 0;
                }
            }
            return result;
        }

        public int SuperFanpai_fanOne(ProtocolMgr protocol, ILogger logger, int index)
        {
            string url = "/root/superFanpai!fanOne.action";
            string data = "cardId=" + index;
            ServerResult serverResult = protocol.postXml(url, data, "超级翻牌卡牌翻牌");
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/card");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("ischoose");
                    if (xmlNode2.InnerText == "1")
                    {
                        XmlNode xmlNode3 = xmlNode.SelectSingleNode("gemlevel");
                        if (xmlNode3 != null)
                        {
                            base.logInfo(logger, string.Format("超级翻牌卡牌翻牌, 翻牌序号为{0}, 获得{1}级宝石*1", index, xmlNode3.InnerText));
                            break;
                        }
                        break;
                    }
                }
                result = 0;
            }
            return result;
        }

        public int superFanpai_buyTimes(ProtocolMgr protocol, ILogger logger, int buyone)
        {
            string url = "/root/superFanpai!buyTimes.action";
            ServerResult xml = protocol.getXml(url, "超级翻牌购买次数");
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
                base.logInfo(logger, string.Format("超级翻牌购买次数成功，花费{0}金币", buyone));
                return 0;
            }
        }

        public int handleCrossPlatformCompeteInfo(ProtocolMgr protocol, ILogger logger, int gold_available, int max_gold_to_continue_support, out long cdMSeconds)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            string attackerName = "";
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            string defenderName = "";
            bool flag = false;
            int num9 = 100;
            string url = "/root/kfzb!getMatchDetail.action";
            ServerResult xml = protocol.getXml(url, "获取跨服争霸赛信息");
            bool flag2 = xml == null;
            int result;
            if (flag2)
            {
                cdMSeconds = 2000L;
                result = 1;
            }
            else
            {
                bool flag3 = !xml.CmdSucceed;
                if (flag3)
                {
                    cdMSeconds = base.an_hour_later();
                    result = 4;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/winnerinfo");
                    bool flag4 = xmlNode != null;
                    if (flag4)
                    {
                        cdMSeconds = base.next_day();
                        result = 3;
                    }
                    else
                    {
                        bool flag5 = cmdResult.SelectSingleNode("/results/message/attacker") == null;
                        if (flag5)
                        {
                            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/battletime1");
                            bool flag6 = xmlNode2 != null;
                            if (flag6)
                            {
                                DateTime d = DateTime.Parse(xmlNode2.InnerText);
                                DateTime now = DateTime.Now;
                                TimeSpan timeSpan = d - now;
                                bool flag7 = timeSpan.TotalMilliseconds > 0.0;
                                if (flag7)
                                {
                                    num = (int)timeSpan.TotalMilliseconds;
                                }
                            }
                            cdMSeconds = (long)(num - 30000);
                            result = 2;
                        }
                        else
                        {
                            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/message");
                            XmlNodeList childNodes = xmlNode3.ChildNodes;
                            foreach (XmlNode xmlNode4 in childNodes)
                            {
                                bool flag8 = xmlNode4.Name == "supportcompetitorid";
                                if (flag8)
                                {
                                    num2 = int.Parse(xmlNode4.InnerText);
                                }
                                else
                                {
                                    bool flag9 = xmlNode4.Name == "supporttimes";
                                    if (flag9)
                                    {
                                        int.Parse(xmlNode4.InnerText);
                                    }
                                    else
                                    {
                                        bool flag10 = xmlNode4.Name == "cd";
                                        if (flag10)
                                        {
                                            num = int.Parse(xmlNode4.InnerText);
                                        }
                                        else
                                        {
                                            bool flag11 = xmlNode4.Name == "attacker";
                                            if (flag11)
                                            {
                                                XmlNodeList childNodes2 = xmlNode4.ChildNodes;
                                                IEnumerator enumerator2 = childNodes2.GetEnumerator();
                                                try
                                                {
                                                    while (enumerator2.MoveNext())
                                                    {
                                                        XmlNode xmlNode5 = (XmlNode)enumerator2.Current;
                                                        bool flag12 = xmlNode5.Name == "playerlevel";
                                                        if (flag12)
                                                        {
                                                            num4 = int.Parse(xmlNode5.InnerText);
                                                        }
                                                        else
                                                        {
                                                            bool flag13 = xmlNode5.Name == "competitorid";
                                                            if (flag13)
                                                            {
                                                                num3 = int.Parse(xmlNode5.InnerText);
                                                            }
                                                            else
                                                            {
                                                                bool flag14 = xmlNode5.Name == "playername";
                                                                if (flag14)
                                                                {
                                                                    attackerName = xmlNode5.InnerText;
                                                                }
                                                                else
                                                                {
                                                                    bool flag15 = xmlNode5.Name == "revenge";
                                                                    if (flag15)
                                                                    {
                                                                        num5 = int.Parse(xmlNode5.InnerText);
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
                                                    bool flag16 = disposable != null;
                                                    if (flag16)
                                                    {
                                                        disposable.Dispose();
                                                    }
                                                }
                                            }
                                            bool flag17 = xmlNode4.Name == "defender";
                                            if (flag17)
                                            {
                                                XmlNodeList childNodes3 = xmlNode4.ChildNodes;
                                                foreach (XmlNode xmlNode6 in childNodes3)
                                                {
                                                    bool flag18 = xmlNode6.Name == "playerlevel";
                                                    if (flag18)
                                                    {
                                                        num7 = int.Parse(xmlNode6.InnerText);
                                                    }
                                                    else
                                                    {
                                                        bool flag19 = xmlNode6.Name == "competitorid";
                                                        if (flag19)
                                                        {
                                                            num6 = int.Parse(xmlNode6.InnerText);
                                                        }
                                                        else
                                                        {
                                                            bool flag20 = xmlNode6.Name == "playername";
                                                            if (flag20)
                                                            {
                                                                defenderName = xmlNode6.InnerText;
                                                            }
                                                            else
                                                            {
                                                                bool flag21 = xmlNode6.Name == "revenge";
                                                                if (flag21)
                                                                {
                                                                    num8 = int.Parse(xmlNode6.InnerText);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/message/canbuymorereward");
                            bool flag22 = xmlNode7 != null;
                            if (flag22)
                            {
                                flag = (xmlNode7.InnerText == "1");
                            }
                            XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/message/buymorerewardgold");
                            bool flag23 = xmlNode8 != null;
                            if (flag23)
                            {
                                int.TryParse(xmlNode8.InnerText, out num9);
                            }
                            bool flag24 = num9 <= 0;
                            if (flag24)
                            {
                                num9 = 5;
                            }
                            cdMSeconds = (long)num;
                            bool flag25 = num2 > 0 && (num9 > max_gold_to_continue_support || gold_available < num9);
                            if (flag25)
                            {
                                cdMSeconds = base.immediate();
                                result = 0;
                            }
                            else
                            {
                                bool flag26 = num3 == 0 || num6 == 0;
                                if (flag26)
                                {
                                    cdMSeconds = 250000L;
                                    result = 0;
                                }
                                else
                                {
                                    int num10 = 2;
                                    int num11 = 0;
                                    bool flag27 = num4 > num7;
                                    if (flag27)
                                    {
                                        num10 += 6;
                                    }
                                    else
                                    {
                                        bool flag28 = num4 < num7;
                                        if (flag28)
                                        {
                                            num11 += 6;
                                        }
                                    }
                                    num10 += num5;
                                    num11 += num8;
                                    int num12 = 0;
                                    while (flag && num12 < 3 && (num2 <= 0 || (gold_available >= num9 && num9 <= max_gold_to_continue_support)))
                                    {
                                        gold_available -= num9;
                                        bool flag29 = num10 >= num11;
                                        if (flag29)
                                        {
                                            this.supportCompete(protocol, logger, num3, num4, true, attackerName, defenderName, out flag, out num9);
                                            num2 = num3;
                                        }
                                        else
                                        {
                                            this.supportCompete(protocol, logger, num6, num7, false, attackerName, defenderName, out flag, out num9);
                                            num2 = num6;
                                        }
                                        int num13 = num12;
                                        num12 = num13 + 1;
                                    }
                                    bool flag30 = num > 300000;
                                    if (flag30)
                                    {
                                        cdMSeconds = (long)(num - 300000);
                                    }
                                    result = 0;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool supportCompete(ProtocolMgr protocol, ILogger logger, int competorId, int level, bool isattacker, string _attackerName, string _defenderName, out bool can_continue_support, out int support_gold)
        {
            can_continue_support = false;
            support_gold = 1000;
            string url = "/root/kfzb!support.action";
            string data = "competitorId=" + competorId;
            ServerResult serverResult = protocol.postXml(url, data, "支持争霸赛");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/canbuymorereward");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    can_continue_support = (xmlNode.InnerText == "1");
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/buymorerewardgold");
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    int.TryParse(xmlNode2.InnerText, out support_gold);
                }
                bool flag4 = support_gold <= 0;
                if (flag4)
                {
                    support_gold = 5;
                }
                string text = string.Format("支持争霸赛, 用户[{0}:{1}级]", isattacker ? _attackerName : _defenderName, level);
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        public int handleKfzbMarketInfo(ProtocolMgr protocol, ILogger logger, User user, int gold_limit, out long cdmseconds, string buy_conf, bool ignore_gold_limit)
        {
            string url = "/root/kfzb!getKfzbMarket.action";
            ServerResult xml = protocol.getXml(url, "获取争霸商城信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                cdmseconds = 2000L;
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    cdmseconds = 2000L;
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    List<KfzbItemConfig> list = new List<KfzbItemConfig>();
                    string[] array = buy_conf.Split(new char[]
					{
						';'
					});
                    int num;
                    for (int i = 0; i < array.Length; i = num + 1)
                    {
                        string text = array[i];
                        bool flag3 = text != null && !(text == "");
                        if (flag3)
                        {
                            KfzbItemConfig kfzbItemConfig = new KfzbItemConfig();
                            kfzbItemConfig.fill(text);
                            list.Add(kfzbItemConfig);
                        }
                        num = i;
                    }
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/nextcd");
                    bool flag4 = xmlNode != null;
                    if (flag4)
                    {
                        long.TryParse(xmlNode.InnerText, out cdmseconds);
                    }
                    else
                    {
                        cdmseconds = base.immediate();
                    }
                    List<ActivityMgr.KfzbItem> list2 = new List<ActivityMgr.KfzbItem>();
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/message/kfzbshop");
                    foreach (XmlNode xmlNode2 in xmlNodeList)
                    {
                        bool flag5 = xmlNode2 != null && xmlNode2.HasChildNodes;
                        if (flag5)
                        {
                            int usergold = user._usergold;
                            int sysgold = user._sysgold;
                            ActivityMgr.KfzbItem kfzbItem = new ActivityMgr.KfzbItem();
                            kfzbItem.fill(xmlNode2);
                            bool flag6 = this.ifBuyItem(kfzbItem, list, usergold, sysgold, gold_limit, ignore_gold_limit);
                            if (flag6)
                            {
                                list2.Add(kfzbItem);
                            }
                        }
                    }
                    bool flag7 = list2.Count == 0;
                    if (flag7)
                    {
                        result = 0;
                    }
                    else
                    {
                        for (int j = 0; j < list2.Count - 1; j = num + 1)
                        {
                            for (int k = j + 1; k < list2.Count; k = num + 1)
                            {
                                bool flag8 = list2[j].pos > list2[k].pos;
                                if (flag8)
                                {
                                    ActivityMgr.KfzbItem value = list2[j];
                                    list2[j] = list2[k];
                                    list2[k] = value;
                                }
                                num = k;
                            }
                            num = j;
                        }
                        cdmseconds = 3000L;
                        for (int l = 0; l < list2.Count; l = num + 1)
                        {
                            int usergold2 = user._usergold;
                            int sysgold2 = user._sysgold;
                            ActivityMgr.KfzbItem item = list2[l];
                            bool flag9 = this.ifBuyItem(item, list, usergold2, sysgold2, gold_limit, ignore_gold_limit);
                            if (flag9)
                            {
                                this.buyKfzbItem(protocol, logger, item);
                            }
                            num = l;
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        private bool ifBuyItem(ActivityMgr.KfzbItem item, List<KfzbItemConfig> configs, int user_gold_can_use, int sys_gold_can_use, int gold_limit, bool ignore_gold_limit)
        {
            bool flag = false;
            bool flag2 = item.remainnum == 0;
            bool result;
            if (flag2)
            {
                result = flag;
            }
            else
            {
                foreach (KfzbItemConfig current in configs)
                {
                    bool flag3 = (ignore_gold_limit || user_gold_can_use + sys_gold_can_use >= gold_limit - item.price) && (item.price != 20 || user_gold_can_use >= 20) && (item.price != 10 || user_gold_can_use + sys_gold_can_use >= 10) && current.item_type == item.itemtype && current.max_pos >= item.pos;
                    if (flag3)
                    {
                        flag = true;
                        break;
                    }
                }
                result = flag;
            }
            return result;
        }

        public int buyKfzbItem(ProtocolMgr protocol, ILogger logger, ActivityMgr.KfzbItem item)
        {
            string url = "/root/kfzb!buyFromKfzbMarket.action";
            string data = "marketId=" + item.marketid;
            ServerResult serverResult = protocol.postXml(url, data, "购买争霸商城物品");
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
                    base.logInfo(logger, string.Format("购买争霸商城物品, {0}, 成功", item.desc()));
                    result = 0;
                }
            }
            return result;
        }

        public int handleJailEventInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            bool flag = user.Level < User.Level_Jail;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                string url = "/root/jail!getJailEventInfo.action";
                ServerResult xml = protocol.getXml(url, "获取典狱活动信息");
                bool flag2 = xml == null;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    bool flag3 = !xml.CmdSucceed;
                    if (flag3)
                    {
                        user.removeActivity(ActivityType.JailEvent);
                        result = 10;
                    }
                    else
                    {
                        user.addActivity(ActivityType.JailEvent);
                        XmlDocument cmdResult = xml.CmdResult;
                        List<ActivityMgr.Slave> list = new List<ActivityMgr.Slave>();
                        XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/onenpc");
                        foreach (XmlNode xmlNode in xmlNodeList)
                        {
                            bool flag4 = xmlNode != null && xmlNode.HasChildNodes;
                            if (flag4)
                            {
                                XmlNode xmlNode2 = xmlNode.SelectSingleNode("state");
                                int num = -1;
                                bool flag5 = xmlNode2 != null;
                                if (flag5)
                                {
                                    int.TryParse(xmlNode2.InnerText, out num);
                                }
                                bool flag6 = num != -1 && num != 2;
                                if (flag6)
                                {
                                    ActivityMgr.Slave slave = new ActivityMgr.Slave();
                                    slave.state = num;
                                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("jailevent/id");
                                    bool flag7 = xmlNode3 != null;
                                    if (flag7)
                                    {
                                        slave.id = int.Parse(xmlNode3.InnerText);
                                    }
                                    XmlNode xmlNode4 = xmlNode.SelectSingleNode("jailevent/name");
                                    bool flag8 = xmlNode4 != null;
                                    if (flag8)
                                    {
                                        slave.name = xmlNode4.InnerText;
                                    }
                                    XmlNode xmlNode5 = xmlNode.SelectSingleNode("jailevent/baoshi");
                                    bool flag9 = xmlNode5 != null;
                                    if (flag9)
                                    {
                                        slave.baoshi = int.Parse(xmlNode5.InnerText);
                                    }
                                    list.Add(slave);
                                }
                            }
                        }
                        bool flag10 = list.Count == 0;
                        if (flag10)
                        {
                            result = 0;
                        }
                        else
                        {
                            foreach (ActivityMgr.Slave current in list)
                            {
                                bool flag11 = current.state == 0 || current.state == 3;
                                if (flag11)
                                {
                                    this.catchSlave(protocol, logger, current);
                                    this.slashSlave(protocol, logger, current);
                                }
                                else
                                {
                                    bool flag12 = current.state == 1;
                                    if (flag12)
                                    {
                                        this.slashSlave(protocol, logger, current);
                                    }
                                }
                            }
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        private int catchSlave(ProtocolMgr protocol, ILogger logger, ActivityMgr.Slave s)
        {
            string url = "/root/jail!catchJailEvent.action";
            string data = "slaveId=" + s.id;
            ServerResult serverResult = protocol.postXml(url, data, "抓捕典狱活动NPC");
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
                    int num = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/catchreward/baoshi");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    base.logInfo(logger, string.Format("抓捕典狱活动NPC [{0}] 成功, 获得宝石+{1}", s.name, num));
                    result = 0;
                }
            }
            return result;
        }

        private int slashSlave(ProtocolMgr protocol, ILogger logger, ActivityMgr.Slave s)
        {
            string url = "/root/jail!slash.action";
            string data = "slaveId=Event:" + s.id;
            ServerResult serverResult = protocol.postXml(url, data, "劳动典狱活动NPC");
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
                    int num = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    base.logInfo(logger, string.Format("劳动典狱活动NPC [{0}] 成功, 获得宝石+{1}", s.name, num));
                    result = 0;
                }
            }
            return result;
        }

        #region 跨服竞技场
        public void getWdMedalGift(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfwd!getWdMedalGift.action";
            ServerResult xml = protocol.getXml(url, "获取勋章信息");
            if (xml == null || !xml.CmdSucceed) return;

            XmlNodeList list = xml.CmdResult.SelectNodes("/results/message/medal");
            if (list == null) return;

            foreach (XmlNode node in list)
            {
                bool cangetreward = false;
                XmlNode node1 = node.SelectSingleNode("cangetreward");
                if (node1 != null)
                {
                    cangetreward = (node1.InnerText == "1");
                }
                if (cangetreward)
                {
                    XmlNode node2 = node.SelectSingleNode("id");
                    if (node2 != null)
                    {
                        int medalId = int.Parse(node2.InnerText);
                        if (recvWdMedalGift(protocol, logger, medalId) > 0) break;
                    }
                }
            }
        }

        public int recvWdMedalGift(ProtocolMgr protocol, ILogger logger, int medalId)
        {
            string url = "/root/kfwd!recvWdMedalGift.action";
            string data = string.Format("medalId={0}", medalId);
            string text = "领取勋章奖励";
            ServerResult xml = protocol.postXml(url, data, text);
            if (xml == null || !xml.CmdSucceed) return 10;

            int baoshilevel = 0, baoshinum = 0;
            XmlNode node1 = xml.CmdResult.SelectSingleNode("/results/message/baoshi/baoshilevel");
            if (node1 != null)
            {
                int.TryParse(node1.InnerText, out baoshilevel);
            }
            XmlNode node2 = xml.CmdResult.SelectSingleNode("/results/message/baoshi/baoshinum");
            if (node2 != null)
            {
                int.TryParse(node2.InnerText, out baoshinum);
            }
            logInfo(logger, string.Format("{0}, 宝石lv.{1}+{2}", text, baoshilevel, baoshinum));
            return 0;
        }

        public int handleKfwdInfo(ProtocolMgr protocol, ILogger logger, int gold_available, out long cdMSeconds)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 100;
            string url = "/root/kfwd!getMatchDetail.action";
            string str = "跨服武斗会";
            ServerResult xml = protocol.getXml(url, str + "跨服武斗会信息");
            if (xml == null)
            {
                cdMSeconds = 2000;
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                cdMSeconds = base.an_hour_later();
                return 4;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                bool scoreticketsreward = false;
                XmlNode node = cmdResult.SelectSingleNode("/results/message/scoreticketsreward");
                if (node != null)
                {
                    scoreticketsreward = (node.InnerText == "1");
                }
                if (scoreticketsreward)
                {
                    this.getScoreTicketReward(protocol, logger, false);
                }

                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/winnerinfo");
                if (xmlNode != null)
                {
                    cdMSeconds = base.next_day();
                    return 3;
                }
                else if (cmdResult.SelectSingleNode("/results/message/attacker") == null)
                {
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/battletime1");
                    if (xmlNode2 != null)
                    {
                        DateTime d = DateTime.Parse(xmlNode2.InnerText);
                        DateTime now = DateTime.Now;
                        TimeSpan timeSpan = d - now;
                        if (timeSpan.TotalMilliseconds > 0.0)
                        {
                            num = (int)timeSpan.TotalMilliseconds;
                        }
                    }
                    cdMSeconds = (long)(num - 30000);
                    return 2;
                }
                else
                {
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/message");
                    XmlNodeList childNodes = xmlNode3.ChildNodes;
                    foreach (XmlNode xmlNode4 in childNodes)
                    {
                        if (xmlNode4.Name == "supportcompetitorid")
                        {
                            int num9 = int.Parse(xmlNode4.InnerText);
                        }
                        else if (xmlNode4.Name == "supporttimes")
                        {
                            int.Parse(xmlNode4.InnerText);
                        }
                        else if (xmlNode4.Name == "cd")
                        {
                            num = int.Parse(xmlNode4.InnerText);
                        }
                        else if (xmlNode4.Name == "attacker")
                        {
                            XmlNodeList childNodes2 = xmlNode4.ChildNodes;
                            IEnumerator enumerator2 = childNodes2.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    XmlNode xmlNode5 = (XmlNode)enumerator2.Current;
                                    if (xmlNode5.Name == "playerlevel")
                                    {
                                        num3 = int.Parse(xmlNode5.InnerText);
                                    }
                                    else if (xmlNode5.Name == "competitorid")
                                    {
                                        num2 = int.Parse(xmlNode5.InnerText);
                                    }
                                    else if (xmlNode5.Name == "playername")
                                    {
                                        string innerText = xmlNode5.InnerText;
                                    }
                                    else if (xmlNode5.Name == "revenge")
                                    {
                                        num4 = int.Parse(xmlNode5.InnerText);
                                    }
                                }
                                continue;
                            }
                            finally
                            {
                                IDisposable disposable = enumerator2 as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                        else if (xmlNode4.Name == "defender")
                        {
                            XmlNodeList childNodes3 = xmlNode4.ChildNodes;
                            foreach (XmlNode xmlNode6 in childNodes3)
                            {
                                if (xmlNode6.Name == "playerlevel")
                                {
                                    num6 = int.Parse(xmlNode6.InnerText);
                                }
                                else if (xmlNode6.Name == "competitorid")
                                {
                                    num5 = int.Parse(xmlNode6.InnerText);
                                }
                                else if (xmlNode6.Name == "playername")
                                {
                                    string innerText2 = xmlNode6.InnerText;
                                }
                                else if (xmlNode6.Name == "revenge")
                                {
                                    num7 = int.Parse(xmlNode6.InnerText);
                                }
                            }
                        }
                    }
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/message/canbuymorereward");
                    if (xmlNode7 != null)
                    {
                        bool flag22 = xmlNode7.InnerText == "1";
                    }
                    XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/message/buymorerewardgold");
                    if (xmlNode8 != null)
                    {
                        int.TryParse(xmlNode8.InnerText, out num8);
                    }
                    if (num8 <= 0)
                    {
                        num8 = 5;
                    }
                    cdMSeconds = (long)num;
                    if (num2 == 0 || num5 == 0)
                    {
                        cdMSeconds = 250000L;
                        return 0;
                    }
                    else
                    {
                        int num10 = 2;
                        int num11 = 0;
                        if (num3 > num6)
                        {
                            num10 += 6;
                        }
                        else if (num3 < num6)
                        {
                            num11 += 6;
                        }
                        num10 += num4;
                        num11 += num7;
                        if (num > 300000)
                        {
                            cdMSeconds = (long)(num - 300000);
                        }
                        return 0;
                    }
                }
            }
        }

        public int handlePlayerCompeteSignupInfo(ProtocolMgr protocol, ILogger logger, User user, bool buy_reward, bool openbox, int openbox_type, int gold_available, bool isKfPvp)
        {
            string url = "/root/kfwd!getSignupList.action";
            string arg = "跨服武斗会";
            if (isKfPvp)
            {
                url = "/root/kfpvp!getSignupList.action";
                arg = "跨服竞技场";
            }
            ServerResult xml = protocol.getXml(url, string.Format("获取{0}报名信息", arg));
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
                int signupstate = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/signupstate");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out signupstate);
                }
                int currentgroup = 0;
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/currentgroup");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out currentgroup);
                }
                int minsignuplv = 0;
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/message/minsignuplv");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out minsignuplv);
                }
                int cd = 0;
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/message/cd");
                if (xmlNode4 != null)
                {
                    int.TryParse(xmlNode4.InnerText, out cd);
                }
                if ((currentgroup > 0 || (minsignuplv > 0 && minsignuplv <= user.Level)) && signupstate == 0)
                {
                    this.signupMe(protocol, logger, isKfPvp);
                }
                int boxnum = 0;
                XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/message/playerboxinfo/boxnum");
                if (xmlNode5 != null)
                {
                    int.TryParse(xmlNode5.InnerText, out boxnum);
                }
                if (openbox)
                {
                    int num6 = 0;
                    while (num6 == 0 && boxnum > 0)
                    {
                        num6 = this.openKfwdBox(protocol, logger, openbox_type, gold_available, out boxnum, isKfPvp);
                    }
                }
                if (cd < 0)
                {
                    this.getTributeDetail(protocol, logger, buy_reward, openbox, openbox_type, gold_available, isKfPvp);
                    this.getWdMedalGift(protocol, logger);
                }
                return 0;
            }
        }

        public int openKfwdBox(ProtocolMgr protocol, ILogger logger, int type, int gold_available, out int boxnum, bool isKfPvp)
        {
            boxnum = 0;
            string url = "/root/kfwd!openBoxById.action";
            int num = 0;
            string text = "开跨服武斗会获胜宝箱";
            if (isKfPvp)
            {
                url = "/root/kfpvp!openBoxById.action";
                text = "开跨服竞技场获胜宝箱";
            }
            if (type == 0)
            {
                num = 0;
                text = "免费模式" + text;
            }
            else if (type == 2)
            {
                num = 1;
                text = "2倍模式" + text;
            }
            else if (type == 4)
            {
                num = 5;
                text = "4倍模式" + text;
            }
            else if (type == 10)
            {
                num = 50;
                text = "10倍模式" + text;
            }
            if (gold_available < num)
            {
                type = 0;
            }
            string data = "gold=" + type;
            ServerResult serverResult = protocol.postXml(url, data, text);
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int num2 = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/tickets");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num2);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/playerboxinfo/boxnum");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out boxnum);
                }
                base.logInfo(logger, string.Format("{0}, 点券+{1}", text, num2));
                return 0;
            }
        }

        public int getTributeDetail(ProtocolMgr protocol, ILogger logger, bool buy_reward, bool openbox, int openbox_type, int gold_available, bool isKfPvp)
        {
            string url = "/root/kfwd!getTributeDetail.action";
            string arg = "跨服武斗会";
            if (isKfPvp)
            {
                url = "/root/kfpvp!getTributeDetail.action";
                arg = "跨服竞技场";
            }
            ServerResult xml = protocol.getXml(url, string.Format("获取{0}奖励信息", arg));
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                bool scoreticketsreward = false;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/scoreticketsreward");
                if (xmlNode != null)
                {
                    scoreticketsreward = (xmlNode.InnerText == "1");
                }
                if (scoreticketsreward)
                {
                    this.getScoreTicketReward(protocol, logger, isKfPvp);
                }
                int totalboxnum = 0;
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/playerboxinfo/totalboxnum");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out totalboxnum);
                }
                bool medalstate = false;
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/message/medalstate");
                if (xmlNode3 != null)
                {
                    medalstate = (xmlNode3.InnerText == "0");
                }
                if (medalstate && totalboxnum > 100)
                {
                    this.kfpvp_getMedal(protocol, logger);
                }
                int boxnum = 0;
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/message/playerboxinfo/boxnum");
                if (xmlNode4 != null)
                {
                    int.TryParse(xmlNode4.InnerText, out boxnum);
                }
                bool cangettopreward = false;
                bool cangetfinalreward = false;
                XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/message/cangettopreward");
                if (xmlNode5 != null)
                {
                    cangettopreward = (xmlNode5.InnerText == "1");
                }
                XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/message/cangetfinalreward");
                if (xmlNode6 != null)
                {
                    cangetfinalreward = (xmlNode6.InnerText == "1");
                }
                if (cangetfinalreward)
                {
                    this.getKfPvpReward(protocol, logger, 1, ref boxnum);
                }
                if (cangettopreward)
                {
                    this.getKfPvpReward(protocol, logger, 2, ref boxnum);
                }
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/message/tributeinfo/tributelist/tribute");
                foreach (XmlNode xmlNode7 in xmlNodeList)
                {
                    if (xmlNode7 != null && xmlNode7.HasChildNodes)
                    {
                        int gold = 0;
                        int ticket = 0;
                        int state = 2;
                        XmlNode xmlNode8 = xmlNode7.SelectSingleNode("tickets");
                        if (xmlNode8 != null)
                        {
                            int.TryParse(xmlNode8.InnerText, out ticket);
                        }
                        XmlNode xmlNode9 = xmlNode7.SelectSingleNode("gold");
                        if (xmlNode9 != null)
                        {
                            int.TryParse(xmlNode9.InnerText, out gold);
                        }
                        XmlNode xmlNode10 = xmlNode7.SelectSingleNode("state");
                        if (xmlNode10 != null)
                        {
                            int.TryParse(xmlNode10.InnerText, out state);
                        }
                        if (state != 1 && state == 2)
                        {
                            if (gold == 0)
                            {
                                this.buyTributeReward(protocol, logger, 0, ticket, isKfPvp);
                            }
                            else if (buy_reward && gold_available > gold)
                            {
                                this.buyTributeReward(protocol, logger, gold, ticket, isKfPvp);
                                gold_available -= gold;
                            }
                        }
                    }
                }
                if (openbox)
                {
                    int num5 = 0;
                    while (num5 == 0 && boxnum > 0)
                    {
                        num5 = this.openKfwdBox(protocol, logger, openbox_type, gold_available, out boxnum, isKfPvp);
                    }
                }
                return 0;
            }
        }

        public int getKfPvpReward(ProtocolMgr protocol, ILogger logger, int rewardId, ref int boxnum)
        {
            string url = "/root/kfpvp!recvRewardById.action";
            string data = "rewardId=" + rewardId;
            string text = "排名奖励";
            if (rewardId == 2)
            {
                text = "前三奖励";
            }
            text = "获取竞技场" + text;
            ServerResult serverResult = protocol.postXml(url, data, text);
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int rewardbox = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/rewardbox");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out rewardbox);
                }
                base.logInfo(logger, string.Format("{0}, 宝箱+{1}", text, rewardbox));
                boxnum += rewardbox;
                return 0;
            }
        }

        public int getScoreTicketReward(ProtocolMgr protocol, ILogger logger, bool isKfPvp)
        {
            string url = "/root/kfwd!getScoreTicketsReward.action";
            string text = "跨服武斗会";
            if (isKfPvp)
            {
                url = "/root/kfpvp!getScoreTicketsReward.action";
                text = "跨服竞技场";
            }
            ServerResult xml = protocol.getXml(url, "购买" + text + "积分奖励");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int tickets = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/tickets");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out tickets);
                }
                base.logInfo(logger, string.Format("获取{0}积分奖励, 点券+{1}", text, tickets));
                return 0;
            }
        }

        public int buyTributeReward(ProtocolMgr protocol, ILogger logger, int gold, int ticket, bool isKfPvp)
        {
            string url = "/root/kfwd!buyTribute.action";
            string text = "跨服武斗会";
            if (isKfPvp)
            {
                url = "/root/kfpvp!buyTribute.action";
                text = "跨服竞技场";
            }
            ServerResult xml = protocol.getXml(url, "购买" + text + "点券宝箱");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                base.logInfo(logger, string.Format("购买{0}点券宝箱, 使用金币[{1}], 宝箱+{2}, ", text, gold, ticket));
                return 0;
            }
        }

        public int signupMe(ProtocolMgr protocol, ILogger logger, bool isKfPvp)
        {
            string url = "/root/kfwd!signUp.action";
            string str = "跨服武斗会";
            if (isKfPvp)
            {
                url = "/root/kfpvp!signUp.action";
                str = "跨服竞技场";
            }
            ServerResult xml = protocol.getXml(url, "报名" + str);
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                base.logInfo(logger, "报名" + str + "成功");
                return 0;
            }
        }

        public int syncMyData(ProtocolMgr protocol, ILogger logger, bool isKfPvp)
        {
            string url = "/root/kfwd!syncData.action";
            string str = "跨服武斗会";
            if (isKfPvp)
            {
                url = "/root/kfpvp!syncData.action";
                str = "跨服竞技场";
            }
            ServerResult xml = protocol.getXml(url, str + "同步数据");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                base.logInfo(logger, str + "同步数据成功");
                return 0;
            }
        }

        public int kfpvp_getMedal(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfpvp!recvWdMedal.action";
            string text = "跨服竞技场获取徽章";
            ServerResult xml = protocol.getXml(url, text);
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                base.logInfo(logger, text);
                return 0;
            }
        }

        public int kfpvp_getMatchDetail(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfpvp!getMatchDetail.action";
            ServerResult xml = protocol.getXml(url, "跨服竞技场信息");
            if (xml == null)
            {
                return -1;
            }
            else if (!xml.CmdSucceed)
            {
                return -2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int roundid = 0;
                XmlNode node = cmdResult.SelectSingleNode("/results/message/roundid");
                if (node != null)
                {
                    int.TryParse(node.InnerText, out roundid);
                }
                int matchid = 0;
                node = cmdResult.SelectSingleNode("/results/message/matchid");
                if (node != null)
                {
                    int.TryParse(node.InnerText, out matchid);
                }
                int cd = 0;
                node = cmdResult.SelectSingleNode("/results/message/cd");
                if (node != null)
                {
                    int.TryParse(node.InnerText, out cd);
                }
                int caninspire = 0;
                node = cmdResult.SelectSingleNode("/results/message/caninspire");
                if (node != null)
                {
                    int.TryParse(node.InnerText, out caninspire);
                }
                int freeinspire = 0;
                node = cmdResult.SelectSingleNode("/results/message/freeinspire");
                if (node != null)
                {
                    int.TryParse(node.InnerText, out freeinspire);
                }
                if (caninspire == 1 && freeinspire > 0)
                {
                    kfpvp_inspire(protocol, logger, 1);
                    base.logInfo(logger, string.Format("跨服竞技场第{0}轮第{1}场，剩余免费鼓舞次数{2}", roundid, matchid, freeinspire - 1), Color.Green);
                }
                if (cd > 0)
                {
                    return cd + 5000;
                }
                return 0;
            }
        }

        public int kfpvp_inspire(ProtocolMgr protocol, ILogger logger, int count)
        {
            string url = "/root/kfpvp!inspire.action";
            string data = string.Format("count={0}", count);
            ServerResult xml = protocol.postXml(url, data, "跨服竞技场鼓舞");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public int kfpvp_getRankinglist(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfpvp!getRankingList.action";
            string desc = "跨服竞技场查看排名";
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
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    result = 0;
                }
            }
            return result;
        }

        public int handlePlayerCompeteEventInfo(ProtocolMgr protocol, ILogger logger, bool buy_reward, int gold_available)
        {
            string url = "/root/kfEvent!getKfwdEventInfo.action";
            ServerResult xml = protocol.getXml(url, "获取武斗会庆祝活动信息");
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
                int _max_event_reward_times = 0;
                int _got_event_reward_times = 0;
                int _event_reward_gold = 0;
                int _event_need_gold = 0;
                int _event_reward_tickets = 0;
                this.handleEventInfoNodes(cmdResult, ref _max_event_reward_times, ref _got_event_reward_times, ref _event_reward_gold, ref _event_need_gold, ref _event_reward_tickets);
                int num6 = 50;
                while (_max_event_reward_times > _got_event_reward_times && num6 > 0)
                {
                    if (_event_reward_gold > 0)
                    {
                        this.getEventReward(protocol, logger, ref _max_event_reward_times, ref _got_event_reward_times, ref _event_reward_gold, ref _event_need_gold, ref _event_reward_tickets);
                        gold_available += _event_reward_gold;
                    }
                    else if (_event_need_gold > 0)
                    {
                        if (!buy_reward || gold_available < _event_need_gold)
                        {
                            break;
                        }
                        this.getEventReward(protocol, logger, ref _max_event_reward_times, ref _got_event_reward_times, ref _event_reward_gold, ref _event_need_gold, ref _event_reward_tickets);
                        gold_available -= _event_need_gold;
                    }
                    num6--;
                }
                return 0;
            }
        }

        private void handleEventInfoNodes(XmlDocument xml, ref int _max_event_reward_times, ref int _got_event_reward_times, ref int _event_reward_gold, ref int _event_need_gold, ref int _event_reward_tickets)
        {
            XmlNode xmlNode = xml.SelectSingleNode("/results/maxtimes");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out _max_event_reward_times);
            }
            else
            {
                _max_event_reward_times = 0;
            }
            XmlNode xmlNode2 = xml.SelectSingleNode("/results/getrewardtimes");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out _got_event_reward_times);
            }
            else
            {
                _got_event_reward_times = _max_event_reward_times;
            }
            XmlNode xmlNode3 = xml.SelectSingleNode("/results/rewardgold");
            if (xmlNode3 != null)
            {
                int.TryParse(xmlNode3.InnerText, out _event_reward_gold);
            }
            else
            {
                _event_reward_gold = 0;
            }
            XmlNode xmlNode4 = xml.SelectSingleNode("/results/gold");
            if (xmlNode4 != null)
            {
                int.TryParse(xmlNode4.InnerText, out _event_need_gold);
            }
            else
            {
                _event_need_gold = 0;
            }
            XmlNode xmlNode5 = xml.SelectSingleNode("/results/tickets");
            if (xmlNode5 != null)
            {
                int.TryParse(xmlNode5.InnerText, out _event_reward_tickets);
            }
            else
            {
                _event_reward_tickets = 0;
            }
        }

        public int getEventReward(ProtocolMgr protocol, ILogger logger, ref int _max_event_reward_times, ref int _got_event_reward_times, ref int _event_reward_gold, ref int _event_need_gold, ref int _event_reward_tickets)
        {
            string url = "/root/kfEvent!getKfwdReward.action";
            ServerResult xml = protocol.getXml(url, "获取武斗会庆祝活动奖励");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 2;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                string text = "";
                if (_event_need_gold > 0)
                {
                    text += string.Format("使用[{0}]金币购买武斗会庆祝活动奖励, 获得宝箱+{1}", _event_need_gold, _event_reward_tickets);
                }
                else if (_event_reward_gold > 0)
                {
                    text += string.Format("领取武斗会庆祝活动奖励, 获得金币+{0}, 宝箱+{1}", _event_reward_gold, _event_reward_tickets);
                }
                base.logInfo(logger, text);
                cmdResult.SelectSingleNode("/results/playerboxinfo/boxnum");
                this.handleEventInfoNodes(cmdResult, ref _max_event_reward_times, ref _got_event_reward_times, ref _event_reward_gold, ref _event_need_gold, ref _event_reward_tickets);
                return 0;
            }
        }
        #endregion

        public int handleKfBanquetInfo(ProtocolMgr protocol, ILogger logger, User user, int buy_ticket_gold, bool only_nation, bool again, int gold_available, out long cdMSeconds)
        {
            string url = "/root/kfBanquet.action";
            ServerResult xml = protocol.getXml(url, "获取盛宴活动信息");
            if (xml == null)
            {
                cdMSeconds = 30000L;
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                cdMSeconds = base.next_day();
                return 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int basedoubletickets = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/basedoubletickets");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerXml, out basedoubletickets);
                    if (basedoubletickets > 0)
                    {
                        string url2 = "/root/kfBanquet!choosenDouble.action";
                        string text;
                        if (again && gold_available > 10)
                        {
                            string data = "type=1";
                            ServerResult serverResult = protocol.postXml(url2, data, "再喝一杯");
                            XmlDocument cmdResult2 = serverResult.CmdResult;
                            int basedoubletickets_2 = 0;
                            int tickets = 0;
                            XmlNode xmlNode2 = cmdResult2.SelectSingleNode("/results/basedoubletickets");
                            if (xmlNode2 != null)
                            {
                                int.TryParse(xmlNode2.InnerXml, out basedoubletickets_2);
                            }
                            xmlNode2 = cmdResult2.SelectSingleNode("/results/tickets");
                            if (xmlNode2 != null)
                            {
                                int.TryParse(xmlNode2.InnerXml, out tickets);
                            }
                            text = string.Concat("再喝一杯:得到基础点券+", basedoubletickets_2, ", 加倍奖励点券+", tickets);
                        }
                        else
                        {
                            string data = "type=0";
                            ServerResult serverResult2 = protocol.postXml(url2, data, "不胜酒力");
                            text = "不胜酒力:得到基础点券+" + basedoubletickets;
                        }
                        base.logInfo(logger, text);
                        cdMSeconds = 10000L;
                        return 0;
                    }
                }
                int canjoinnum = 0;
                int buyjoingold = 1000;
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/canjoinnum");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerXml, out canjoinnum);
                }
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/buyjoingold");
                if (xmlNode4 != null)
                {
                    int.TryParse(xmlNode4.InnerXml, out buyjoingold);
                }
                else
                {
                    buyjoingold = 1000;
                }
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/room");
                List<ActivityMgr.RankRoom> list = new List<ActivityMgr.RankRoom>();
                foreach (XmlNode xmlNode5 in xmlNodeList)
                {
                    if (xmlNode5 != null && xmlNode5.HasChildNodes)
                    {
                        XmlNodeList childNodes = xmlNode5.ChildNodes;
                        ActivityMgr.RankRoom rankRoom = new ActivityMgr.RankRoom();
                        foreach (XmlNode xmlNode6 in childNodes)
                        {
                            if (xmlNode6.Name == "rank")
                            {
                                rankRoom.rank = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "nation")
                            {
                                rankRoom.nation = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "nationattr")
                            {
                                rankRoom.nationattr = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "playername")
                            {
                                rankRoom.playername = xmlNode6.InnerText;
                            }
                            else if (xmlNode6.Name == "serverid")
                            {
                                rankRoom.serverid = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "servername")
                            {
                                rankRoom.servername = xmlNode6.InnerText;
                            }
                            else if (xmlNode6.Name == "weinum")
                            {
                                rankRoom.weinum = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "shunum")
                            {
                                rankRoom.shunum = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "wunum")
                            {
                                rankRoom.wunum = int.Parse(xmlNode6.InnerText);
                            }
                            else if (xmlNode6.Name == "bufnum")
                            {
                                rankRoom.bufnum = int.Parse(xmlNode6.InnerText);
                            }
                        }
                        list.Add(rankRoom);
                    }
                }
                if (gold_available >= buyjoingold && buyjoingold <= buy_ticket_gold)
                {
                    if (this.buyBanquet(protocol, logger, buyjoingold))
                    {
                        canjoinnum++;
                    }
                }
                if (canjoinnum == 0)
                {
                    cdMSeconds = base.next_day();
                    return 0;
                }
                else if (list.Count == 0)
                {
                    cdMSeconds = base.an_hour_later();
                    return 0;
                }
                else
                {
                    ActivityMgr.RankRoom rankRoom2 = null;
                    int nationInt = user.NationInt;
                    foreach (ActivityMgr.RankRoom current in list)
                    {
                        if (current.nation == nationInt)
                        {
                            if (only_nation)
                            {
                                if (current.is_nation_dinner() && (rankRoom2 == null || (current.bufnum > 0 && rankRoom2.bufnum == 0)))
                                {
                                    rankRoom2 = current;
                                }
                            }
                            else if (rankRoom2 == null || (current.bufnum > 0 && rankRoom2.bufnum == 0))
                            {
                                rankRoom2 = current;
                            }
                        }
                    }
                    if (rankRoom2 == null)
                    {
                        cdMSeconds = 10000L;
                        return 0;
                    }
                    else if (this.joinBanquet(protocol, logger, rankRoom2.rank, rankRoom2.playername))
                    {
                        cdMSeconds = 10000L;
                    }
                    else
                    {
                        cdMSeconds = 10000L;
                    }
                    return 0;
                }
            }
        }

        public bool joinBanquet(ProtocolMgr protocol, ILogger logger, int roomId, string name)
        {
            string url = "/root/kfBanquet!joinBanquet.action";
            string data = "room=" + roomId;
            string text = string.Format("参加第{0}名[{1}]的盛宴", roomId, name);
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

        public bool buyBanquet(ProtocolMgr protocol, ILogger logger, int _current_buygold)
        {
            string url = "/root/kfBanquet!buyBanquetNum.action";
            string data = "num=1";
            ServerResult serverResult = protocol.postXml(url, data, "购买邀请券");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, string.Format("使用[{0}]金, 购买邀请券*1", _current_buygold));
                result = true;
            }
            return result;
        }

        public void getRoomInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfBanquet!getRoomInfo.action";
            ServerResult xml = protocol.getXml(url, "获取参加盛宴信息");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
            }
        }

        public void getRoomReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfBanquet!getRewardInfo.action";
            ServerResult xml = protocol.getXml(url, "获取盛宴奖励");
            bool flag = xml == null || !xml.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = xml.CmdResult;
            }
        }

        public int handleWorldArmyInfo(ProtocolMgr protocol, ILogger logger, bool is_zongzi, ref int gold_available, bool auto_open_box, int open_box_type, bool buy_box1, bool buy_box2, int refresh_type, out long cdMSeconds)
        {
            string url = "/root/nian!getNianInfo.action";
            if (is_zongzi)
            {
                url = "/root/nian!getZongziInfo.action";
            }
            ServerResult xml = protocol.getXml(url, "获取世界军团信息");
            if (xml == null)
            {
                cdMSeconds = base.immediate();
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                cdMSeconds = base.next_day();
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int daojishi = 0;
            int boxnum = 0;
            int attlefttime = 0;
            bool recovering = false;
            bool can_attack = false;
            bool defeated = false;
            string army_info = "";
            int nextarmy = 0;
            this.handleArmyNode(protocol, logger, cmdResult, ref gold_available, buy_box1, buy_box2, ref daojishi, ref recovering, ref can_attack, ref boxnum, ref army_info, ref defeated, ref attlefttime, ref nextarmy);
            if (defeated || attlefttime == 0)
            {
                if (auto_open_box)
                {
                    this.openBoxMain(protocol, logger, ref gold_available, auto_open_box, open_box_type, boxnum);
                }
                cdMSeconds = base.next_day();
                return 2;
            }
            if (attlefttime < 0)
            {
                string url2 = "/root/nian!startAttZongzi.action";
                ServerResult xml2 = protocol.getXml(url2, "开始世界军团信息");
                ServerResult xml3 = protocol.getXml(url, "获取世界军团信息");
                XmlDocument cmdResult2 = xml3.CmdResult;
                this.handleArmyNode(protocol, logger, cmdResult2, ref gold_available, buy_box1, buy_box2, ref daojishi, ref recovering, ref can_attack, ref boxnum, ref army_info, ref defeated, ref attlefttime, ref nextarmy);
            }
            int round = 1;
            int times = 1;
            DateTime beforDT;
            DateTime afterDT;
            while (attlefttime > 0)
            {
                beforDT = System.DateTime.Now;
                int millisecond = attlefttime % 1000;
                int second = attlefttime / 1000 % 60;
                int minute = attlefttime / 60000;
                logInfo(logger, string.Format("剩余时间: {0}:{1} {2}", minute, second, millisecond));
                int armyid = 0;
                int armytype = 1;
                for (int j = 0; j < army_info.Length; j++)
                {
                    int num3 = int.Parse(army_info.Substring(j, 1));
                    if (num3 > armytype)
                    {
                        armyid = j;
                        armytype = num3;
                    }
                }
                this.refreshByGoldMain(protocol, logger, ref gold_available, refresh_type, army_info, armytype);
                int num5 = this.attackArmy(protocol, logger, is_zongzi, armyid, armytype, ref gold_available, buy_box1, buy_box2, ref daojishi, ref recovering, ref can_attack, ref boxnum, ref army_info, ref defeated, ref attlefttime, ref nextarmy, ref round, ref times);
                if ((defeated || num5 == 3 || attlefttime == 0) & auto_open_box)
                {
                    this.openBoxMain(protocol, logger, ref gold_available, auto_open_box, open_box_type, boxnum);
                    cdMSeconds = base.next_day();
                    return 2;
                }
                if (recovering)
                {
                    round++;
                    times = 1;
                    logInfo(logger, string.Format("下次攻击时间: {0}", nextarmy));
                    //Thread.Sleep(18000);
                    Thread.Sleep(nextarmy - 200);
                    ServerResult xml4 = protocol.getXml(url, "获取世界军团信息");
                    XmlDocument cmdResult3 = xml4.CmdResult;
                    this.handleArmyNode(protocol, logger, cmdResult3, ref gold_available, buy_box1, buy_box2, ref daojishi, ref recovering, ref can_attack, ref boxnum, ref army_info, ref defeated, ref attlefttime, ref nextarmy);
                }
                else
                {
                    times++;
                    Thread.Sleep(5800);
                    ServerResult xml5 = protocol.getXml(url, "获取世界军团信息");
                    XmlDocument cmdResult4 = xml5.CmdResult;
                    this.handleArmyNode(protocol, logger, cmdResult4, ref gold_available, buy_box1, buy_box2, ref daojishi, ref recovering, ref can_attack, ref boxnum, ref army_info, ref defeated, ref attlefttime, ref nextarmy);
                }
                afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                logInfo(logger, string.Format("花费{0}ms", ts.TotalMilliseconds));
            }
            cdMSeconds = 6000;
            return 0;
        }

        private void openBoxMain(ProtocolMgr protocol, ILogger logger, ref int gold_available, bool auto_open_box, int open_box_type, int boxnum)
        {
            int num = 0;
            bool flag = open_box_type == 1;
            if (flag)
            {
                open_box_type = 2;
                num = 2;
            }
            else
            {
                bool flag2 = open_box_type == 2;
                if (flag2)
                {
                    open_box_type = 4;
                    num = 10;
                }
                else
                {
                    bool flag3 = open_box_type == 3;
                    if (flag3)
                    {
                        open_box_type = 10;
                        num = 80;
                    }
                }
            }
            int num2;
            for (int i = 0; i < boxnum; i = num2 + 1)
            {
                bool flag4 = gold_available >= num;
                if (flag4)
                {
                    this.openBox(protocol, logger, open_box_type);
                    gold_available -= num;
                }
                else
                {
                    this.openBox(protocol, logger, 0);
                }
                num2 = i;
            }
        }

        private void refreshByGoldMain(ProtocolMgr protocol, ILogger logger, ref int gold_available, int refresh_type, string text, int armytype)
        {
            int num = 10;
            bool flag = refresh_type == 1;
            if (flag)
            {
                bool flag2 = armytype == 2 && gold_available >= num;
                if (flag2)
                {
                    bool flag3 = this.refreshArmyByGold(protocol, logger, 2, ref text);
                    bool flag4 = flag3;
                    if (flag4)
                    {
                        gold_available -= num;
                        armytype = 3;
                    }
                }
            }
            else
            {
                bool flag5 = refresh_type == 2;
                if (flag5)
                {
                    bool flag6 = armytype == 2;
                    if (flag6)
                    {
                        bool flag7 = gold_available >= num;
                        if (flag7)
                        {
                            bool flag8 = this.refreshArmyByGold(protocol, logger, 2, ref text);
                            bool flag9 = flag8;
                            if (flag9)
                            {
                                gold_available -= num;
                                armytype = 3;
                            }
                        }
                    }
                    else
                    {
                        bool flag10 = armytype == 1 && gold_available >= num;
                        if (flag10)
                        {
                            bool flag11 = this.refreshArmyByGold(protocol, logger, 1, ref text);
                            bool flag12 = flag11;
                            if (flag12)
                            {
                                gold_available -= num;
                                armytype = 2;
                                bool flag13 = gold_available >= num;
                                if (flag13)
                                {
                                    flag11 = this.refreshArmyByGold(protocol, logger, 2, ref text);
                                    bool flag14 = flag11;
                                    if (flag14)
                                    {
                                        gold_available -= num;
                                        armytype = 3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void handleArmyNode(ProtocolMgr protocol, ILogger logger, XmlDocument xml, ref int gold_available, bool buy_box1, bool buy_box2, ref int _daojishi, ref bool _recovering, ref bool _can_attack, ref int _hongbao_count, ref string _army_info, ref bool _defeated, ref int attlefttime, ref int nextarmy)
        {
            XmlNode xmlNode = xml.SelectSingleNode("/results/daojishi");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out _daojishi);
            }
            else
            {
                _daojishi = 0;
            }
            XmlNode xmlNode2 = xml.SelectSingleNode("/results/attacklefttime");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out attlefttime);
            }
            else
            {
                attlefttime = -1;
            }
            XmlNode xmlNode3 = xml.SelectSingleNode("/results/nextarmy");
            if (xmlNode3 != null)
            {
                int.TryParse(xmlNode3.InnerText, out nextarmy);
            }
            else
            {
                nextarmy = 0;
            }
            _recovering = (nextarmy > 0);
            XmlNode xmlNode4 = xml.SelectSingleNode("/results/canattack");
            if (xmlNode4 != null)
            {
                _can_attack = xmlNode4.InnerText.Equals("true");
            }
            else
            {
                _can_attack = false;
            }
            XmlNode xmlNode5 = xml.SelectSingleNode("/results/hongbao");
            if (xmlNode5 != null)
            {
                int.TryParse(xmlNode5.InnerText, out _hongbao_count);
            }
            else
            {
                _hongbao_count = 0;
            }
            XmlNode xmlNode6 = xml.SelectSingleNode("/results/army");
            if (xmlNode6 != null)
            {
                _army_info = xmlNode6.InnerText;
            }
            else
            {
                _army_info = "";
            }
            XmlNode xmlNode7 = xml.SelectSingleNode("/results/defeat");
            if (xmlNode7 != null)
            {
                _defeated = xmlNode7.InnerText.Equals("true");
            }
            else
            {
                _defeated = false;
            }
            if (_defeated)
            {
                if (buy_box1)
                {
                    XmlNode xmlNode8 = xml.SelectSingleNode("/results/reward1");
                    if (xmlNode8 != null && xmlNode8.InnerText.Equals("0"))
                    {
                        int num2 = 0;
                        int ticket = 0;
                        XmlNode xmlNode9 = xml.SelectSingleNode("/results/rewardgold1");
                        if (xmlNode9 != null)
                        {
                            int.TryParse(xmlNode9.InnerText, out num2);
                        }
                        XmlNode xmlNode10 = xml.SelectSingleNode("/results/rewardtickets1");
                        if (xmlNode10 != null)
                        {
                            int.TryParse(xmlNode10.InnerText, out ticket);
                        }
                        if (gold_available >= num2)
                        {
                            this.buyReward(protocol, logger, 1, num2, ticket);
                            gold_available -= num2;
                        }
                    }
                }
                if (buy_box2)
                {
                    XmlNode xmlNode11 = xml.SelectSingleNode("/results/reward2");
                    if (xmlNode11 != null && xmlNode11.InnerText.Equals("0"))
                    {
                        int num3 = 0;
                        int ticket2 = 0;
                        XmlNode xmlNode12 = xml.SelectSingleNode("/results/rewardgold2");
                        if (xmlNode12 != null)
                        {
                            int.TryParse(xmlNode12.InnerText, out num3);
                        }
                        XmlNode xmlNode13 = xml.SelectSingleNode("/results/rewardtickets2");
                        if (xmlNode13 != null)
                        {
                            int.TryParse(xmlNode13.InnerText, out ticket2);
                        }
                        if (gold_available >= num3)
                        {
                            this.buyReward(protocol, logger, 2, num3, ticket2);
                            gold_available -= num3;
                        }
                    }
                }
            }
        }

        private int attackArmy(ProtocolMgr protocol, ILogger logger, bool is_zongzi, int armyid, int army_type, ref int gold_available, bool buy_box1, bool buy_box2, ref int _daojishi, ref bool _recovering, ref bool _can_attack, ref int _hongbao_count, ref string _army_info, ref bool _defeated, ref int attlefttime, ref int nextarmy, ref int round, ref int times)
        {
            string url = "/root/nian!attNian.action";
            if (is_zongzi)
            {
                url = "/root/nian!attZongzi.action";
            }
            string data = "army=" + armyid;
            ServerResult serverResult = protocol.postXml(url, data, "攻击世界军团");
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (serverResult.CmdSucceed)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                this.handleArmyNode(protocol, logger, cmdResult, ref gold_available, buy_box1, buy_box2, ref _daojishi, ref _recovering, ref _can_attack, ref _hongbao_count, ref _army_info, ref _defeated, ref attlefttime, ref nextarmy);
                int addhongbao = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/addhongbao");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out addhongbao);
                }
                int totaladdhp = 0;
                int decreasehp = 0;
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/decreasehp");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out decreasehp);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/totaladdhp");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out totaladdhp);
                }
                string armyname;
                if (army_type == 1)
                {
                    armyname = "普通部队";
                }
                else if (army_type == 2)
                {
                    armyname = "精英部队";
                }
                else
                {
                    armyname = "首领部队";
                }
                string text2 = string.Format("第{0}轮 第{1}次, 攻打[{2}]位置[{3}], 获得红包[{4}]个{5}{6}", round, times, armyid, armyname, addhongbao, (decreasehp > 0) ? string.Format(", 共计掉血{0}", decreasehp) : "", (totaladdhp > 0) ? string.Format(", 粽子回血{0}", totaladdhp) : "");
                base.logInfo(logger, text2);
                result = 0;
            }
            else if (serverResult.CmdError.IndexOf("冷却中") >= 0)
            {
                result = 2;
            }
            else if (serverResult.CmdError.IndexOf("不能攻击") >= 0)
            {
                result = 3;
            }
            else
            {
                result = 10;
            }
            return result;
        }

        private bool openBox(ProtocolMgr protocol, ILogger logger, int mode)
        {
            string url = "/root/nian!openHonbao.action";
            bool flag = mode != 0 && mode != 2 && mode != 4 && mode != 10;
            if (flag)
            {
                mode = 0;
            }
            string data = "mode=" + mode;
            ServerResult serverResult = protocol.postXml(url, data, "打开红包");
            bool flag2 = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag2)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int num = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/addtickets");
                bool flag3 = xmlNode != null;
                if (flag3)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                string arg = "普通模式";
                bool flag4 = mode == 2;
                if (flag4)
                {
                    arg = "两倍模式";
                }
                else
                {
                    bool flag5 = mode == 4;
                    if (flag5)
                    {
                        arg = "4倍模式";
                    }
                    else
                    {
                        bool flag6 = mode == 10;
                        if (flag6)
                        {
                            arg = "10倍模式";
                        }
                    }
                }
                base.logInfo(logger, string.Format("{0}开启红包, 获得点券[{1}]", arg, num));
                result = true;
            }
            return result;
        }

        private bool buyReward(ProtocolMgr protocol, ILogger logger, int mode, int gold, int ticket)
        {
            string url = "/root/nian!getMoreReward.action";
            string data = "mode=" + mode;
            ServerResult serverResult = protocol.postXml(url, data, "购买奖励");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                bool flag2 = mode == 1;
                string text;
                if (flag2)
                {
                    text = string.Format("使用[{0}]金币购买世界军团排名奖励, 点券+{1}", gold, ticket);
                }
                else
                {
                    text = string.Format("使用[{0}]金币购买世界军团获胜奖励, 点券+{1}", gold, ticket);
                }
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        public bool refreshArmyByGold(ProtocolMgr protocol, ILogger logger, int uptype, ref string _army_info)
        {
            string url = "/root/nian!refreshNianTroop.action";
            ServerResult xml = protocol.getXml(url, "刷新部队");
            bool flag = xml == null || !xml.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/refreshtroop/trooptype");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    _army_info = xmlNode.InnerText;
                }
                else
                {
                    _army_info = "";
                }
                bool flag3 = uptype == 1;
                if (flag3)
                {
                    base.logInfo(logger, "使用10金币将普通部队升级为精英部队");
                }
                else
                {
                    base.logInfo(logger, "使用10金币将精英部队升级为首领部队");
                }
                result = true;
            }
            return result;
        }

        public int handleGiftEventInfo(ProtocolMgr protocol, ILogger logger, string eatSerial)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            string url = "/root/giftEvent!getDrangonBoatInfo.action";
            ServerResult xml = protocol.getXml(url, "获取玩家犒赏活动信息");
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
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/zongzi/rice");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/zongzi/bean");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/zongzi/meat");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out num3);
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/zongzi/egg");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        int.TryParse(xmlNode4.InnerText, out num4);
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/zongzi/ricebean/canmake");
                    bool flag7 = xmlNode5 != null;
                    if (flag7)
                    {
                        xmlNode5.InnerText = "1";
                    }
                    XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/zongzi/ricemeat/canmake");
                    bool flag8 = xmlNode6 != null;
                    if (flag8)
                    {
                        xmlNode6.InnerText = "1";
                    }
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/zongzi/riceegg/canmake");
                    bool flag9 = xmlNode7 != null;
                    if (flag9)
                    {
                        xmlNode7.InnerText = "1";
                    }
                    XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/zongzi/ricebean/ricebeannum");
                    bool flag10 = xmlNode8 != null;
                    if (flag10)
                    {
                        int.TryParse(xmlNode8.InnerText, out num5);
                    }
                    XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/zongzi/ricemeat/ricemeatnum");
                    bool flag11 = xmlNode9 != null;
                    if (flag11)
                    {
                        int.TryParse(xmlNode9.InnerText, out num6);
                    }
                    XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/zongzi/riceegg/riceeggnum");
                    bool flag12 = xmlNode10 != null;
                    if (flag12)
                    {
                        int.TryParse(xmlNode10.InnerText, out num7);
                    }
                    string text = "231";
                    bool flag13 = eatSerial != null && eatSerial.Length > 0;
                    if (flag13)
                    {
                        text = eatSerial;
                    }
                    List<int> list = new List<int>();
                    int i = 0;
                    int length = text.Length;
                    while (i < length)
                    {
                        char c = text[i];
                        bool flag14 = c.Equals('1');
                        if (flag14)
                        {
                            bool flag15 = !list.Contains(1);
                            if (flag15)
                            {
                                list.Add(1);
                            }
                        }
                        else
                        {
                            bool flag16 = c.Equals('2');
                            if (flag16)
                            {
                                bool flag17 = !list.Contains(2);
                                if (flag17)
                                {
                                    list.Add(2);
                                }
                            }
                            else
                            {
                                bool flag18 = c.Equals('3') && !list.Contains(3);
                                if (flag18)
                                {
                                    list.Add(3);
                                }
                            }
                        }
                        bool flag19 = list.Count >= 3;
                        if (flag19)
                        {
                            break;
                        }
                        int num8 = i;
                        i = num8 + 1;
                    }
                    int num9 = 50;
                    while (num >= 5 && (num4 > 0 || num3 > 0 || num2 > 0) && num9 > 0)
                    {
                        bool flag20 = false;
                        foreach (int current in list)
                        {
                            bool flag21 = current == 3;
                            if (flag21)
                            {
                                bool flag22 = num4 > 0;
                                if (flag22)
                                {
                                    this.makeZongzi(protocol, logger, 3, ref num, ref num2, ref num3, ref num4, ref num5, ref num6, ref num7);
                                    flag20 = true;
                                    break;
                                }
                            }
                            else
                            {
                                bool flag23 = current == 2;
                                if (flag23)
                                {
                                    bool flag24 = num3 > 0;
                                    if (flag24)
                                    {
                                        this.makeZongzi(protocol, logger, 2, ref num, ref num2, ref num3, ref num4, ref num5, ref num6, ref num7);
                                        flag20 = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    bool flag25 = current == 1 && num2 > 0;
                                    if (flag25)
                                    {
                                        this.makeZongzi(protocol, logger, 1, ref num, ref num2, ref num3, ref num4, ref num5, ref num6, ref num7);
                                        flag20 = true;
                                        break;
                                    }
                                }
                            }
                        }
                        bool flag26 = !flag20;
                        if (flag26)
                        {
                            break;
                        }
                        int num8 = num9;
                        num9 = num8 - 1;
                    }
                    num9 = 50;
                    while (num5 > 0 || num6 > 0 || (num7 > 0 && num9 > 0))
                    {
                        bool flag27 = num7 > 0;
                        if (flag27)
                        {
                            this.eatZongzi(protocol, logger, 3, ref num5, ref num6, ref num7);
                        }
                        else
                        {
                            bool flag28 = num6 > 0;
                            if (flag28)
                            {
                                this.eatZongzi(protocol, logger, 2, ref num5, ref num6, ref num7);
                            }
                            else
                            {
                                bool flag29 = num5 <= 0;
                                if (flag29)
                                {
                                    break;
                                }
                                this.eatZongzi(protocol, logger, 1, ref num5, ref num6, ref num7);
                            }
                        }
                        int num8 = num9;
                        num9 = num8 - 1;
                    }
                    result = 0;
                }
            }
            return result;
        }

        public void makeZongzi(ProtocolMgr protocol, ILogger logger, int dragonType, ref int _rice, ref int _bean, ref int _meat, ref int _egg, ref int _ricebean, ref int _ricemeat, ref int _riceegg)
        {
            string url = "/root/giftEvent!makeZongzi.action";
            string data = "dragonType=" + dragonType;
            string text = "";
            bool flag = dragonType == 1;
            if (flag)
            {
                text = "豆沙包";
            }
            else
            {
                bool flag2 = dragonType == 2;
                if (flag2)
                {
                    text = "鲜肉包";
                }
                else
                {
                    bool flag3 = dragonType == 3;
                    if (flag3)
                    {
                        text = "蛋黄包";
                    }
                }
            }
            ServerResult serverResult = protocol.postXml(url, data, "制作" + text);
            bool flag4 = serverResult == null || !serverResult.CmdSucceed;
            if (!flag4)
            {
                bool flag5 = dragonType == 1;
                if (flag5)
                {
                    int num = _ricebean;
                    _ricebean = num + 1;
                    num = _bean;
                    _bean = num - 1;
                }
                else
                {
                    bool flag6 = dragonType == 2;
                    if (flag6)
                    {
                        int num = _ricemeat;
                        _ricemeat = num + 1;
                        num = _meat;
                        _meat = num - 1;
                    }
                    else
                    {
                        bool flag7 = dragonType == 3;
                        if (flag7)
                        {
                            int num = _riceegg;
                            _riceegg = num + 1;
                            num = _egg;
                            _egg = num - 1;
                        }
                    }
                }
                _rice -= 5;
                base.logInfo(logger, string.Format("成功制作一个{0}", text));
            }
        }

        public void eatZongzi(ProtocolMgr protocol, ILogger logger, int dragonType, ref int _ricebean, ref int _ricemeat, ref int _riceegg)
        {
            string url = "/root/giftEvent!eatZongzi.action";
            string data = "dragonType=" + dragonType;
            string text = "";
            bool flag = dragonType == 1;
            if (flag)
            {
                text = "豆沙包";
            }
            else
            {
                bool flag2 = dragonType == 2;
                if (flag2)
                {
                    text = "鲜肉包";
                }
                else
                {
                    bool flag3 = dragonType == 3;
                    if (flag3)
                    {
                        text = "蛋黄包";
                    }
                }
            }
            ServerResult serverResult = protocol.postXml(url, data, "吃掉" + text);
            bool flag4 = serverResult == null || !serverResult.CmdSucceed;
            if (!flag4)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                string text2 = "";
                bool flag5 = dragonType == 1;
                if (flag5)
                {
                    int num = _ricebean;
                    _ricebean = num - 1;
                    int num2 = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/reward/copper");
                    bool flag6 = xmlNode != null;
                    if (flag6)
                    {
                        int.TryParse(xmlNode.InnerText, out num2);
                    }
                    text2 = string.Format("成功吃掉一个{0}, 银币+{1}", text, num2);
                }
                else
                {
                    bool flag7 = dragonType == 2;
                    if (flag7)
                    {
                        int num = _ricemeat;
                        _ricemeat = num - 1;
                        string arg = "";
                        int num3 = 0;
                        XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/reward/troopchip/goodsname");
                        bool flag8 = xmlNode2 != null;
                        if (flag8)
                        {
                            arg = xmlNode2.InnerText;
                        }
                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/reward/troopchip/num");
                        bool flag9 = xmlNode3 != null;
                        if (flag9)
                        {
                            int.TryParse(xmlNode3.InnerText, out num3);
                        }
                        text2 = string.Format("成功吃掉一个{0}, 获得{1}*{2}", text, arg, num3);
                    }
                    else
                    {
                        bool flag10 = dragonType == 3;
                        if (flag10)
                        {
                            int num = _riceegg;
                            _riceegg = num - 1;
                            int num4 = 0;
                            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/reward/baoshi");
                            bool flag11 = xmlNode4 != null;
                            if (flag11)
                            {
                                int.TryParse(xmlNode4.InnerText, out num4);
                            }
                            text2 = string.Format("成功吃掉一个{0}, 宝石+{1}", text, num4);
                        }
                    }
                }
                base.logInfo(logger, text2);
            }
        }

        private int upgradeTreasure(ProtocolMgr protocol, ILogger logger, ref int _currentTreasureQuality, int _currentUpgradeCostGold)
        {
            string url = "/root/archEvent!refreshArchTreasure.action";
            ServerResult xml = protocol.getXml(url, "升级考古地图");
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
                    string[] array = new string[]
					{
						"",
						"白色",
						"蓝色",
						"绿色",
						"黄色",
						"红色",
						"紫色"
					};
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/playerarchinfo/treasurequality");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out _currentTreasureQuality);
                    }
                    base.logInfo(logger, string.Format("升级考古地图成功, 从{0}升级到{1}, 花费{2}金币", array[_currentTreasureQuality], array[_currentTreasureQuality + 1], _currentUpgradeCostGold));
                    int num = _currentTreasureQuality;
                    _currentTreasureQuality = num + 1;
                    result = 0;
                }
            }
            return result;
        }

        private int createTeam(ProtocolMgr protocol, ILogger logger, ref bool _alreadyJoinedTeam)
        {
            string url = "/root/archEvent!createArchTeam.action";
            ServerResult xml = protocol.getXml(url, "创建考古队伍");
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
                    _alreadyJoinedTeam = true;
                    base.logInfo(logger, "创建考古队伍成功");
                    result = 0;
                }
            }
            return result;
        }

        private int leftTeam(ProtocolMgr protocol, ILogger logger, ref bool _alreadyJoinedTeam)
        {
            string url = "/root/archEvent!leftTeam.action";
            ServerResult xml = protocol.getXml(url, "离开考古队伍");
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
                    base.logInfo(logger, "离开考古队伍");
                    _alreadyJoinedTeam = false;
                    result = 0;
                }
            }
            return result;
        }

        private int kickPlayer(ProtocolMgr protocol, ILogger logger, int type)
        {
            string url = "/root/archEvent!kickPlayer.action";
            string data = "type=" + type;
            ServerResult serverResult = protocol.postXml(url, data, "踢出考古玩家");
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
                    base.logInfo(logger, "踢出不符合条件的考古玩家");
                    result = 0;
                }
            }
            return result;
        }

        private ActivityMgr.ArchTeam tryFindTeam(ProtocolMgr protocol, ILogger logger, List<ActivityMgr.ArchTeam> _curentTeams, int _currentTreasureType, int _currentTreasureQuality)
        {
            ActivityMgr.ArchTeam result;
            foreach (ActivityMgr.ArchTeam current in _curentTeams)
            {
                bool flag = current != null && current.types != null && !(current.types == "");
                if (flag)
                {
                    string[] array = current.types.Split(new char[]
					{
						','
					});
                    bool flag2 = !(array[_currentTreasureType - 1] != "0");
                    if (flag2)
                    {
                        bool flag3 = false;
                        string[] array2 = current.qualities.Split(new char[]
						{
							','
						});
                        string[] array3 = array2;
                        int num2;
                        for (int i = 0; i < array3.Length; i = num2 + 1)
                        {
                            string text = array3[i];
                            bool flag4 = text != null && text.Length != 0;
                            if (flag4)
                            {
                                int num = 1000;
                                int.TryParse(text, out num);
                                bool flag5 = num == _currentTreasureQuality;
                                if (flag5)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                            num2 = i;
                        }
                        bool flag6 = flag3;
                        if (flag6)
                        {
                            result = current;
                            return result;
                        }
                    }
                }
            }
            result = null;
            return result;
        }

        public int getArchInfo(ProtocolMgr protocol, ILogger logger, ref int gold_available, bool _selfCreateTeam, string _upgrades, ref string _lastMemberName, bool only_get_info = false)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            string url = "/root/archEvent!getArchInfo.action";
            ServerResult xml = protocol.getXml(url, "获取考古信息");
            bool flag4 = xml == null;
            int result;
            if (flag4)
            {
                result = 1;
            }
            else
            {
                bool flag5 = !xml.CmdSucceed;
                if (flag5)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/playerarchinfo");
                    bool flag6 = xmlNode != null;
                    if (flag6)
                    {
                        XmlNodeList childNodes = xmlNode.ChildNodes;
                        foreach (XmlNode xmlNode2 in childNodes)
                        {
                            bool flag7 = xmlNode2.Name == "treasurenum";
                            if (flag7)
                            {
                                int.TryParse(xmlNode2.InnerText, out num);
                            }
                            else
                            {
                                bool flag8 = xmlNode2.Name == "treasuretype";
                                if (flag8)
                                {
                                    int.TryParse(xmlNode2.InnerText, out num2);
                                }
                                else
                                {
                                    bool flag9 = xmlNode2.Name == "treasurequality";
                                    if (flag9)
                                    {
                                        int.TryParse(xmlNode2.InnerText, out num3);
                                    }
                                    else
                                    {
                                        bool flag10 = xmlNode2.Name == "upgradecost";
                                        if (flag10)
                                        {
                                            int.TryParse(xmlNode2.InnerText, out num4);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    List<ActivityMgr.ArchTeam> list = new List<ActivityMgr.ArchTeam>();
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/alltinfo/tinfo");
                    foreach (XmlNode xmlNode3 in xmlNodeList)
                    {
                        XmlNodeList childNodes2 = xmlNode3.ChildNodes;
                        ActivityMgr.ArchTeam archTeam = new ActivityMgr.ArchTeam();
                        foreach (XmlNode xmlNode4 in childNodes2)
                        {
                            bool flag11 = xmlNode4.Name == "teamid";
                            if (flag11)
                            {
                                archTeam.teamid = xmlNode4.InnerText;
                            }
                            else
                            {
                                bool flag12 = xmlNode4.Name == "pname";
                                if (flag12)
                                {
                                    archTeam.teamname = xmlNode4.InnerText;
                                }
                                else
                                {
                                    bool flag13 = xmlNode4.Name == "nation";
                                    if (flag13)
                                    {
                                        archTeam.nation = int.Parse(xmlNode4.InnerText);
                                    }
                                    else
                                    {
                                        bool flag14 = xmlNode4.Name == "types";
                                        if (flag14)
                                        {
                                            archTeam.types = xmlNode4.InnerText;
                                        }
                                        else
                                        {
                                            bool flag15 = xmlNode4.Name == "quas";
                                            if (flag15)
                                            {
                                                archTeam.qualities = xmlNode4.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool flag16 = archTeam.teamid != null || archTeam.teamid != "";
                        if (flag16)
                        {
                            list.Add(archTeam);
                        }
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/selfteam");
                    bool flag17 = xmlNode5 != null;
                    if (flag17)
                    {
                        flag = true;
                        ActivityMgr.ArchMember archMember = null;
                        ActivityMgr.ArchMember archMember2 = null;
                        XmlNodeList xmlNodeList2 = xmlNode5.SelectNodes("member");
                        foreach (XmlNode xmlNode6 in xmlNodeList2)
                        {
                            ActivityMgr.ArchMember archMember3 = new ActivityMgr.ArchMember();
                            XmlNodeList childNodes3 = xmlNode6.ChildNodes;
                            foreach (XmlNode xmlNode7 in childNodes3)
                            {
                                bool flag18 = xmlNode7.Name == "name";
                                if (flag18)
                                {
                                    archMember3.name = xmlNode7.InnerText;
                                }
                                else
                                {
                                    bool flag19 = xmlNode7.Name == "quality";
                                    if (flag19)
                                    {
                                        archMember3.quality = int.Parse(xmlNode7.InnerText);
                                    }
                                    else
                                    {
                                        bool flag20 = xmlNode7.Name == "type";
                                        if (flag20)
                                        {
                                            archMember3.type = int.Parse(xmlNode7.InnerText);
                                        }
                                        else
                                        {
                                            bool flag21 = xmlNode7.Name == "state";
                                            if (flag21)
                                            {
                                                archMember3.state = xmlNode7.InnerText.Equals("1");
                                            }
                                            else
                                            {
                                                bool flag22 = xmlNode7.Name == "creater";
                                                if (flag22)
                                                {
                                                    archMember3.creater = xmlNode7.InnerText.Equals("1");
                                                }
                                                else
                                                {
                                                    bool flag23 = xmlNode7.Name == "self";
                                                    if (flag23)
                                                    {
                                                        archMember3.self = xmlNode7.InnerText.Equals("1");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            bool self = archMember3.self;
                            if (self)
                            {
                                archMember = archMember3;
                            }
                            else
                            {
                                archMember2 = archMember3;
                            }
                        }
                        flag2 = archMember.creater;
                        bool flag24 = archMember2 == null || archMember2.name == "";
                        if (flag24)
                        {
                            flag3 = false;
                        }
                        else
                        {
                            bool flag25 = archMember2.quality != archMember.quality;
                            if (flag25)
                            {
                                bool flag26 = flag2;
                                if (flag26)
                                {
                                    base.logInfo(logger, string.Format("队友[{0}]与我品质不同, 将踢掉他", archMember2.name));
                                    this.kickPlayer(protocol, logger, archMember2.type);
                                    flag3 = false;
                                }
                                else
                                {
                                    flag3 = false;
                                }
                            }
                            else
                            {
                                bool state = archMember2.state;
                                if (state)
                                {
                                    flag3 = true;
                                }
                                else
                                {
                                    flag3 = false;
                                    bool flag27 = flag2;
                                    if (flag27)
                                    {
                                        bool flag28 = _lastMemberName != "" && _lastMemberName.Equals(archMember2.name);
                                        if (flag28)
                                        {
                                            base.logInfo(logger, string.Format("队友[{0}]检查2次仍然不准备, 将踢掉他", archMember2.name));
                                            this.kickPlayer(protocol, logger, archMember2.type);
                                        }
                                        else
                                        {
                                            _lastMemberName = archMember2.name;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/rewardcount");
                    XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/extrarewardcount");
                    int num5 = 0;
                    int num6 = 0;
                    bool flag29 = xmlNode8 != null;
                    if (flag29)
                    {
                        int.TryParse(xmlNode8.InnerText, out num5);
                    }
                    bool flag30 = xmlNode9 != null;
                    if (flag30)
                    {
                        int.TryParse(xmlNode9.InnerText, out num6);
                    }
                    bool flag31 = num5 > 0;
                    if (flag31)
                    {
                        base.logInfo(logger, string.Format("考古完成, 玉石+{0}, 宝石+{1}", num5, num6));
                    }
                    if (only_get_info)
                    {
                        result = 0;
                    }
                    else
                    {
                        bool flag32 = num < 3;
                        if (flag32)
                        {
                            result = 2;
                        }
                        else
                        {
                            bool flag33 = num2 == 0;
                            if (flag33)
                            {
                                result = 3;
                            }
                            else
                            {
                                bool flag34 = flag;
                                if (flag34)
                                {
                                    bool flag35 = !_selfCreateTeam;
                                    if (flag35)
                                    {
                                        bool flag36 = flag2;
                                        if (flag36)
                                        {
                                            this.leftTeam(protocol, logger, ref flag);
                                        }
                                        result = 0;
                                    }
                                    else
                                    {
                                        bool flag37 = !flag2;
                                        if (flag37)
                                        {
                                            this.leftTeam(protocol, logger, ref flag);
                                            result = 0;
                                        }
                                        else
                                        {
                                            bool flag38 = flag3;
                                            if (flag38)
                                            {
                                                _lastMemberName = "";
                                                this.doReady(protocol, logger);
                                                this.getArchInfo(protocol, logger, ref gold_available, _selfCreateTeam, _upgrades, ref _lastMemberName, true);
                                                result = 0;
                                            }
                                            else
                                            {
                                                result = 0;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string value = num3.ToString();
                                    bool flag39 = _upgrades.Contains(value);
                                    if (flag39)
                                    {
                                        int num7 = 6 - num3;
                                        int num10;
                                        for (int i = 0; i < num7; i = num10 + 1)
                                        {
                                            int num8 = num3;
                                            bool flag40 = gold_available >= num4;
                                            if (flag40)
                                            {
                                                int num9 = this.upgradeTreasure(protocol, logger, ref num3, num4);
                                                bool flag41 = num9 > 0;
                                                if (flag41)
                                                {
                                                    base.logInfo(logger, "升级失败, 下个半小时后继续");
                                                    result = 3;
                                                    return result;
                                                }
                                                bool flag42 = num8 == num3;
                                                if (flag42)
                                                {
                                                    num10 = num3;
                                                    num3 = num10 + 1;
                                                }
                                            }
                                            num10 = i;
                                        }
                                    }
                                    if (_selfCreateTeam)
                                    {
                                        int num11 = this.createTeam(protocol, logger, ref flag);
                                        bool flag43 = num11 == 2;
                                        if (flag43)
                                        {
                                            result = 10;
                                        }
                                        else
                                        {
                                            result = 0;
                                        }
                                    }
                                    else
                                    {
                                        ActivityMgr.ArchTeam archTeam2 = this.tryFindTeam(protocol, logger, list, num2, num3);
                                        bool flag44 = archTeam2 == null;
                                        if (flag44)
                                        {
                                            result = 0;
                                        }
                                        else
                                        {
                                            bool flag45 = this.joinTeam(protocol, logger, archTeam2.teamid, archTeam2.teamname);
                                            if (flag45)
                                            {
                                                this.doReady(protocol, logger);
                                                this.getArchInfo(protocol, logger, ref gold_available, _selfCreateTeam, _upgrades, ref _lastMemberName, true);
                                            }
                                            result = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool joinTeam(ProtocolMgr protocol, ILogger logger, string teamId, string leader)
        {
            string url = "/root/archEvent!joinTeam.action";
            string data = "teamId=" + teamId;
            string text = string.Format("加入[{0}]的考古队伍, 队伍id=[{1}]", leader, teamId);
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

        public bool doReady(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/archEvent!ready.action";
            ServerResult xml = protocol.getXml(url, "考古活动准备");
            return xml != null && xml.CmdSucceed;
        }

        /// <summary>
        /// 龙舟活动
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="gold_available"></param>
        /// <param name="_selfCreateTeam"></param>
        /// <param name="_upgrades"></param>
        /// <param name="_selectTime"></param>
        /// <param name="maxcoin"></param>
        /// <returns></returns>
        public int handleBoatEvent(ProtocolMgr protocol, ILogger logger, ref int gold_available, bool _selfCreateTeam, string _upgrades, bool _selectTime, int maxcoin)
        {
            string url = "/root/event!getBoatEventInfo.action";
            string data = "notice=0";
            ServerResult serverResult = protocol.postXml(url, data, "获取龙舟信息");
            if (serverResult == null)
            {
                base.logInfo(logger, "获取龙舟信息返回空值");
                return 4;
            }
            else if (!serverResult.CmdSucceed)
            {
                base.logInfo(logger, "龙舟活动：" + serverResult.CmdError);
                if (serverResult.CmdError.Contains("不在活动期间"))
                {
                    return 0;
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                int inteam = 0;
                int stage = 0;
                int remaintimes = -1;
                int quality = 1;
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/inteam");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out inteam);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/stage");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out stage);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/playerboat/remaintimes");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out remaintimes);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/playerboat/quality");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out quality);
                }
                if (inteam == 1)
                {
                    return 1;
                }
                else if (stage == 2)
                {
                    if (this.startBoat(protocol, logger))
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (remaintimes > 0)
                {
                    if (quality < 5 && _upgrades.Contains(quality.ToString()))
                    {
                        for (int i = quality; i < 5; i++)
                        {
                            if (!this.upgradeBoat(protocol, logger, ref gold_available))
                            {
                                break;
                            }
                            quality++;
                        }
                    }
                    string text = this.findBoatTeam(cmdResult, quality);
                    if (text != "")
                    {
                        base.logInfo(logger, "发现同类型龙舟队伍：" + text);
                        this.joinBoatTeam(protocol, logger, text);
                    }
                    else if (_selfCreateTeam)
                    {
                        this.createBoatTeam(protocol, logger);
                    }
                    return 1;
                }
                else if (remaintimes == 0)
                {
                    if (_selectTime && this.signUpBoatEvent(protocol, logger))
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (stage == 3)
                {
                    int dashcost = 100;
                    xmlNode = cmdResult.SelectSingleNode("/results/dashcost");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out dashcost);
                    }
                    while (dashcost >= 0 && dashcost <= maxcoin)
                    {
                        dashcost = this.dashBoat(protocol, logger, ref gold_available, dashcost);
                    }
                    return 3;
                }
                else if (stage == 4)
                {
                    int state = 0;
                    xmlNode = cmdResult.SelectSingleNode("/results/signreward/state");
                    if (xmlNode != null)
                    {
                        int.TryParse(xmlNode.InnerText, out state);
                    }
                    if (state == 0)
                    {
                        this.recvBoatEventFinalReward(protocol, logger);
                    }
                    return 10;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public bool signUpBoatEvent(ProtocolMgr protocol, ILogger logger)
        {
            string text = "报名";
            string url = "/root/event!signUpBoatEvent.action";
            string data;
            //int hour = DateTime.Now.Hour;
            //if (hour < 10)
            //{
            //    data = "signUpId=1";
            //    text += "10点龙舟大赛";
            //}
            //else if (hour < 15)
            //{
            //    data = "signUpId=2";
            //    text += "15点龙舟大赛";
            //}
            //else
            //{
            //    text += "21点龙舟大赛";
            //    data = "signUpId=3";
            //}
            text += "龙舟大赛";
            data = "signUpId=0";
            ServerResult serverResult = protocol.postXml(url, data, "报名龙舟大赛");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                base.logInfo(logger, "报名龙舟大赛失败" + serverResult.CmdError);
                return false;
            }
            else
            {
                base.logInfo(logger, text);
                return true;
            }
        }

        /// <summary>
        /// 开始龙舟比赛
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public bool startBoat(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!startBoatComp.action";
            ServerResult serverResult = protocol.getXml(url, "开始龙舟大赛");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                base.logInfo(logger, "开始龙舟大赛失败 " + serverResult.CmdError);
                return false;
            }
            else
            {
                base.logInfo(logger, "开始龙舟大赛");
                return true;
            }
        }

        /// <summary>
        /// 全力冲刺
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="gold_available"></param>
        /// <param name="dashcoin"></param>
        /// <returns></returns>
        public int dashBoat(ProtocolMgr protocol, ILogger logger, ref int gold_available, int dashcoin)
        {
            string text;
            if (dashcoin == 0)
            {
                text = "龙舟：免费冲刺1次";
            }
            else
            {
                text = string.Format("龙舟：花费{0}金币冲刺1次", dashcoin);
            }
            string url = "/root/event!dashBoatEvent.action";
            ServerResult xml = protocol.getXml(url, "龙舟冲刺");
            if (xml == null || !xml.CmdSucceed)
            {
                return -1;
            }
            else
            {
                gold_available -= dashcoin;
                base.logInfo(logger, text);
                int dashcost = 100;
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/dashcost");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out dashcost);
                }
                return dashcost;
            }
        }

        /// <summary>
        /// 升级龙舟
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="gold_available"></param>
        /// <returns></returns>
        public bool upgradeBoat(ProtocolMgr protocol, ILogger logger, ref int gold_available)
        {
            if (gold_available > 5)
            {
                string url = "/root/event!upgradeBoat.action";
                ServerResult xml = protocol.getXml(url, "升级龙舟");
                if (xml == null || !xml.CmdSucceed)
                {
                    base.logInfo(logger, "升级龙舟失败");
                    return false;
                }
                else
                {
                    base.logInfo(logger, "升级龙舟成功");
                    gold_available -= 5;
                    return true;
                }
            }
            base.logInfo(logger, "金币不够升级龙舟");
            return false;
        }

        /// <summary>
        /// 寻找龙舟队伍
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public string findBoatTeam(XmlDocument xml, int quality)
        {
            int teamquality = 0;
            string teamid = "";
            XmlNodeList xmlNodeList = xml.SelectNodes("/results/team");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode != null && xmlNode.HasChildNodes)
                {
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        if (xmlNode2.Name == "quality")
                        {
                            teamquality = int.Parse(xmlNode2.InnerText);
                        }
                        if (xmlNode2.Name == "teamid")
                        {
                            teamid = xmlNode2.InnerText;
                        }
                    }
                    if (teamquality == quality)
                    {
                        return teamid;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 加入龙舟队伍
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="teamid"></param>
        public void joinBoatTeam(ProtocolMgr protocol, ILogger logger, string teamid)
        {
            string url = "/root/event!joinBoatEventTeam.action";
            string data = "teamId=" + teamid;
            ServerResult serverResult = protocol.postXml(url, data, "加入龙舟队伍");
            if (serverResult == null)
            {
                base.logInfo(logger, "加入龙舟队伍失败：null");
            }
            else if (!serverResult.CmdSucceed)
            {
                base.logInfo(logger, "加入龙舟队伍失败：" + serverResult.CmdError);
            }
            else
            {
                base.logInfo(logger, "加入龙舟队伍：" + teamid);
            }
        }

        /// <summary>
        /// 创建龙舟队伍
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        public void createBoatTeam(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!creatBoatEventTeam.action";
            ServerResult xml = protocol.getXml(url, "创建龙舟队伍");
            if (xml == null || !xml.CmdSucceed)
            {
                base.logInfo(logger, "创建龙舟队伍失败：" + xml.CmdError);
            }
            else
            {
                base.logInfo(logger, "创建龙舟队伍成功");
            }
        }

        /// <summary>
        /// 领取龙舟奖励
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        public void recvBoatEventFinalReward(ProtocolMgr protocol, ILogger logger)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string url = "/root/event!recvBoatEventFinalReward.action";
            ServerResult xml = protocol.getXml(url, "领取龙舟大赛奖励");
            if (xml == null || !xml.CmdSucceed)
            {
                base.logInfo(logger, "领取龙舟大赛奖励失败:" + xml.CmdError);
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/signreward/rewardinfo/reward/num");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/rankreward/rewardinfo/reward/num");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num2);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/milesreward/rewardinfo/reward/num");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num3);
                }
                base.logInfo(logger, string.Format("领取龙舟大赛奖励：冲刺奖励{0}宝石，排名奖励{1}宝石，路程奖励{2}宝石", num, num2, num3));
            }
        }

        public int handleYueBingEvent(ProtocolMgr protocol, ILogger logger, ref int goldAvailable, bool bGoldTrain, bool bAutoOpenYuebingHongbao, int num)
        {
            int troopnum = 0;
            int stage = 0;
            int hongbaonum = 0;
            string text = "";
            int yueBingInfo = this.getYueBingInfo(protocol, logger, out troopnum, out stage, out hongbaonum);
            int result;
            if (yueBingInfo == 0 || yueBingInfo == 4)
            {
                result = yueBingInfo;
            }
            else if (stage == 5 && hongbaonum > 0)
            {
                if (bAutoOpenYuebingHongbao)
                {
                    this.openYuebingHongbao(protocol, logger, hongbaonum, num);
                }
                result = 0;
            }
            else if (stage == 3)
            {
                this.bigYueBing(protocol, logger);
                yueBingInfo = this.getYueBingInfo(protocol, logger, out troopnum, out stage, out hongbaonum);
                if (yueBingInfo == 0 || yueBingInfo == 4)
                {
                    result = yueBingInfo;
                }
                else
                {
                    this.recvBigYueBingHongBao(protocol, logger);
                    result = 1;
                }
            }
            else
            {
                if (stage == 2)
                {
                    this.revTrainHongbao(protocol, logger);
                }
                while (troopnum > 0)
                {
                    string url = "/root/event!startTrain.action";
                    ServerResult xml = protocol.getXml(url, "开始训练");
                    if (xml == null || !xml.CmdSucceed)
                    {
                        result = 4;
                        return result;
                    }
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/troopstate");
                    if (xmlNode != null)
                    {
                        text = xmlNode.InnerText;
                        base.logInfo(logger, "当前队列：" + text);
                    }
                    if (bGoldTrain && goldAvailable > 10)
                    {
                        this.fastTroop(protocol, logger);
                    }
                    else
                    {
                        if (text.Length >= 12)
                        {
                            result = 4;
                            return result;
                        }
                        string[] array = text.Split(new char[] { ',' });
                        for (int j = 0; j < array.Length - 1; j++)
                        {
                            int troop_count = int.Parse(array[j]);
                            for (int k = 0; k < troop_count; k++)
                            {
                                this.resetTroop(protocol, logger, j + 1);
                            }
                        }
                    }
                    yueBingInfo = this.getYueBingInfo(protocol, logger, out troopnum, out stage, out hongbaonum);
                    if (yueBingInfo == 0 || yueBingInfo == 4)
                    {
                        result = yueBingInfo;
                        return result;
                    }
                    this.revTrainHongbao(protocol, logger);
                }
                result = 1;
            }
            return result;
        }

        private int getYueBingInfo(ProtocolMgr protocol, ILogger logger, out int troopnum, out int stage, out int hongbaonum)
        {
            troopnum = 0;
            stage = 0;
            hongbaonum = 0;
            string url = "/root/event!getYuebingInfo.action";
            ServerResult xml = protocol.getXml(url, "获取阅兵信息");
            int result;
            if (xml == null)
            {
                base.logInfo(logger, "获取阅兵信息返回空值");
                result = 4;
            }
            else if (!xml.CmdSucceed)
            {
                base.logInfo(logger, "阅兵活动：" + xml.CmdError);
                if (xml.CmdError.Contains("不在活动期间"))
                {
                    result = 0;
                }
                else
                {
                    result = 4;
                }
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/troopnum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out troopnum);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/stage");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out stage);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/hongbaonum");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out hongbaonum);
                }
                result = 1;
            }
            return result;
        }

        private void resetTroop(ProtocolMgr protocol, ILogger logger, int troopid)
        {
            string url = "/root/event!resetTroop.action";
            string data = string.Format("troopId={0}", troopid);
            base.logInfo(logger, string.Format("纠正队伍:{0}", troopid));
            ServerResult serverResult = protocol.postXml(url, data, "纠正队伍");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                base.logInfo(logger, "纠正队伍出错");
            }
        }

        private void fastTroop(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!getYuebingInfo.action";
            ServerResult xml = protocol.getXml(url, "快速阅兵");
            if (xml == null || !xml.CmdSucceed)
            {
                base.logInfo(logger, "快速阅兵出错");
            }
        }

        private void revTrainHongbao(ProtocolMgr protocol, ILogger logger)
        {
            int num = 0;
            int num2 = 0;
            string url = "/root/event!recvTrainHongbao.action";
            ServerResult xml = protocol.getXml(url, "取训练红包");
            if (!(xml == null || !xml.CmdSucceed))
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gethongbao");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/hongbaonum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num2);
                }
                base.logInfo(logger, string.Format("得到训练红包{0}，合计{1}个", num, num2));
            }
        }

        private void bigYueBing(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!bigYuebing.action";
            ServerResult xml = protocol.getXml(url, "大阅兵");
            if (xml == null || !xml.CmdSucceed)
            {
                base.logInfo(logger, "大阅兵出错");
            }
        }

        private void recvBigYueBingHongBao(ProtocolMgr protocol, ILogger logger)
        {
            int num = 0;
            int num2 = 0;
            string url = "/root/event!recvBigYuebingHongbao.action";
            ServerResult xml = protocol.getXml(url, "取大阅兵红包");
            bool flag = xml == null || !xml.CmdSucceed;
            if (flag)
            {
                base.logInfo(logger, "取大阅兵红包出错");
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gethongbao");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/hongbaonum");
                bool flag3 = xmlNode != null;
                if (flag3)
                {
                    int.TryParse(xmlNode.InnerText, out num2);
                }
                base.logInfo(logger, string.Format("得到大阅兵红包{0}，合计{1}个", num, num2));
            }
        }

        private void openYuebingHongbao(ProtocolMgr protocol, ILogger logger, int total, int type)
        {
            string url = "/root/event!openHongbao.action";
            int num = 0;
            int num2 = 0;
            bool flag = type == 0;
            int num3;
            if (flag)
            {
                num3 = 1;
            }
            else
            {
                bool flag2 = type == 1;
                if (flag2)
                {
                    num3 = 2;
                }
                else
                {
                    bool flag3 = type == 3;
                    if (flag3)
                    {
                        num3 = 4;
                    }
                    else
                    {
                        num3 = 10;
                    }
                }
            }
            string data = string.Format("gold={0}", num3);
            int num4;
            for (int i = 0; i < total; i = num4 + 1)
            {
                num = 0;
                num2 = 0;
                ServerResult serverResult = protocol.postXml(url, data, "打开阅兵红包");
                bool flag4 = serverResult == null || !serverResult.CmdSucceed;
                if (flag4)
                {
                    base.logInfo(logger, "打开阅兵红包出错");
                    break;
                }
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
                bool flag5 = xmlNode != null;
                if (flag5)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/hongbaonum");
                bool flag6 = xmlNode != null;
                if (flag6)
                {
                    int.TryParse(xmlNode.InnerText, out num2);
                }
                base.logInfo(logger, string.Format("打开阅兵红包得到宝石{0}个，还剩红包{1}个", num, num2));
                num4 = i;
            }
        }

        public int handleTreasureGameInfo(ProtocolMgr protocol, ILogger logger, User user, string _open_box_type, string _gold_move_box_type, string _dice_type, int _minGoldSteps, ref int gold_available)
        {
            string url = "/root/treasureGame!getGameInfo.action";
            ServerResult xml = protocol.getXml(url, "获取古城探宝信息");
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
                List<ActivityMgr.GameGrid> list = new List<ActivityMgr.GameGrid>();
                int dice = 0;
                int remainonlinedice = 0;
                int playerpos = 0;
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/dice");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out dice);
                }
                else
                {
                    dice = 0;
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/remainonlinedice");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out remainonlinedice);
                }
                else
                {
                    remainonlinedice = 0;
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/playerpos");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out playerpos);
                }
                else
                {
                    playerpos = 0;
                }
                this.renderGrids(logger, cmdResult, list);
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/question");
                if (xmlNode4 != null)
                {
                    this.handleQuestion(protocol, logger, xmlNode4, null);
                }
                if (dice == 0)
                {
                    this.chooseBoxReward(protocol, logger);
                }
                if (dice == 0 && (playerpos == 0 || playerpos == 27))
                {
                    if (remainonlinedice == 0)
                    {
                        result = 3;
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    base.logInfo(logger, string.Format("获得古城寻宝信息, 当前还剩寻宝次数{0}次, 玩家目前位于{1}位置", dice, playerpos));
                    if (playerpos == list.Count && !this.playAgainTreasure(protocol, logger, list))
                    {
                        this.chooseBoxReward(protocol, logger);
                        result = 0;
                    }
                    else
                    {
                        int times = 27;
                        int[] dice_array = this.convertDiceType(_dice_type, times);
                        bool use_dice_array = _dice_type.Trim().Length > 0;
                        while (playerpos < 27 && times > 0)
                        {
                            int nextStepDice;
                            if (use_dice_array)
                            {
                                nextStepDice = getNextStepDice_new(gold_available, dice_array, playerpos);
                            }
                            else
                            {
                                nextStepDice = this.getNextStepDice(false, user, gold_available, _gold_move_box_type, _minGoldSteps, playerpos, list);
                            }
                            this.useDiceTreasure(protocol, logger, nextStepDice, _open_box_type, gold_available, list, ref playerpos);
                            times--;
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        private bool useDiceTreasure(ProtocolMgr protocol, ILogger logger, int diceId, string _open_box_type, int gold_available, List<ActivityMgr.GameGrid> _grids, ref int _player_pos_now)
        {
            string url = "/root/treasureGame!useDice.action";
            string data = "controlDiceId=" + diceId;
            string text = string.Format("古城探宝掷筛子, 玩家位置在{0}, ", _player_pos_now);
            if (diceId == 1)
            {
                text += "使用2金币遥控筛子,只走一步, ";
            }
            else if (diceId == 2)
            {
                text += "使用1金币遥控筛子,只走两步, ";
            }
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool result;
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                text += this.handleDiceResult(protocol, logger, cmdResult, _grids, ref _player_pos_now, ref gold_available, ref _open_box_type, null);
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        private bool playAgainTreasure(ProtocolMgr protocol, ILogger logger, List<ActivityMgr.GameGrid> _grids)
        {
            string url = "/root/treasureGame!playAgain.action";
            ServerResult xml = protocol.getXml(url, "开启新一轮古城探宝");
            bool result;
            if (xml == null || !xml.CmdSucceed)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                this.renderGrids(logger, cmdResult, _grids);
                base.logInfo(logger, "开启新一轮古城探宝");
                result = true;
            }
            return result;
        }

        private void chooseBoxReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/treasureGame!openGhostBox.action";
            ServerResult xml = protocol.getXml(url, "打开宝箱");
            while (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/reward");
                int pos1 = 1;
                int baoshi1 = 1;
                int pos2 = 1;
                int baoshi2 = 1;
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    int id = 0;
                    int type = 0;
                    int num = 0;
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("id");
                    if (xmlNode2 != null)
                    {
                        int.TryParse(xmlNode2.InnerText, out id);
                    }
                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("rewardinfo/reward/type");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out type);
                    }
                    XmlNode xmlNode4 = xmlNode.SelectSingleNode("rewardinfo/reward/num");
                    if (xmlNode4 != null)
                    {
                        int.TryParse(xmlNode4.InnerText, out num);
                    }
                    if (type == 5)
                    {
                        if (baoshi1 == 1)
                        {
                            pos1 = id;
                            baoshi1 = num;
                        }
                        else if (baoshi1 > 1 && baoshi2 == 1)
                        {
                            pos2 = id;
                            baoshi2 = num;
                        }
                        else if (baoshi1 > 1 && baoshi2 > 1)
                        {
                            if (baoshi1 > baoshi2 && num > baoshi2)
                            {
                                pos2 = id;
                                baoshi2 = num;
                            }
                            else if (baoshi1 <= baoshi2 && num > baoshi1)
                            {
                                pos1 = id;
                                baoshi1 = num;
                            }
                        }
                    }
                }
                string text = string.Format("开启荒原秘宝宝箱，选择宝箱1位置{0},宝石{1}  宝箱2位置{2},宝石{3}", pos1, baoshi1, pos2, baoshi2);
                base.logInfo(logger, text);
                url = "/root/treasureGame!chooseGhostBoxReward.action";
                string data = string.Format("newId={0}", pos1);
                ServerResult serverResult = protocol.postXml(url, data, "选择宝箱奖励");
                if (serverResult == null || !serverResult.CmdSucceed)
                {
                    break;
                }
                data = string.Format("newId={0}", pos2);
                serverResult = protocol.postXml(url, data, "选择宝箱奖励");
                if (serverResult == null || !serverResult.CmdSucceed)
                {
                    break;
                }
                url = "/root/treasureGame!openGhostBox.action";
                xml = protocol.getXml(url, "打开宝箱");
            }
        }

        public int handleNewTreasureGames(ProtocolMgr protocol, ILogger logger, int gold_available, int use_ticket_type, bool gold_to_end)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            string url = "/root/treasureGame!getNewGameInfo.action";
            string data = "newId=0";
            ServerResult serverResult = protocol.postXml(url, data, "获取超级探宝信息");
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
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/superticket");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/id");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/newpos");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out num3);
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/maxnewpos");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        int.TryParse(xmlNode4.InnerText, out num4);
                    }
                    bool flag7 = num == 0 && num2 == 0;
                    if (flag7)
                    {
                        result = 2;
                    }
                    else
                    {
                        int num5 = gold_available;
                        int num6 = 0;
                        int num7 = 0;
                        int num8 = 10;
                        bool flag8 = false;
                        bool flag9 = num2 > 0;
                        if (flag9)
                        {
                            int num9 = this.getNewGameInfo(protocol, logger, num2, out num6, out num7, out num8, out num4);
                            bool flag10 = num9 > 0;
                            if (flag10)
                            {
                                result = num9;
                                return result;
                            }
                            bool flag11 = num6 > 0 | gold_to_end;
                            if (flag11)
                            {
                                flag8 = true;
                            }
                            else
                            {
                                num9 = this.leaveSuperGame(protocol, logger);
                                bool flag12 = num9 > 0;
                                if (flag12)
                                {
                                    result = num9;
                                    return result;
                                }
                            }
                        }
                        bool flag13 = !flag8 && num > 0;
                        if (flag13)
                        {
                            int newGameInfo = this.getNewGameInfo(protocol, logger, use_ticket_type, out num6, out num7, out num8, out num4);
                            bool flag14 = newGameInfo > 0;
                            if (flag14)
                            {
                                result = newGameInfo;
                                return result;
                            }
                            flag8 = true;
                        }
                        bool flag15 = flag8;
                        if (flag15)
                        {
                            int num10 = 30;
                            while (num7 < num4 && num10 > 0)
                            {
                                int num11 = num10;
                                num10 = num11 - 1;
                                bool flag16 = num6 > 0;
                                if (flag16)
                                {
                                    int num12 = this.useNewDice(protocol, logger, num8, false, out num6, out num7);
                                    bool flag17 = num12 > 0;
                                    if (flag17)
                                    {
                                        result = 1;
                                        return result;
                                    }
                                }
                                else
                                {
                                    bool flag18 = !gold_to_end || num5 < num8;
                                    if (flag18)
                                    {
                                        int num13 = this.leaveSuperGame(protocol, logger);
                                        bool flag19 = num13 > 0;
                                        if (flag19)
                                        {
                                            result = num13;
                                            return result;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        int num14 = this.useNewDice(protocol, logger, num8, true, out num6, out num7);
                                        bool flag20 = num14 > 0;
                                        if (flag20)
                                        {
                                            result = 1;
                                            return result;
                                        }
                                        num5 -= num8;
                                    }
                                }
                            }
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        public int getNewGameInfo(ProtocolMgr protocol, ILogger logger, int gameId, out int game_free_dice_remains, out int player_now_pos, out int buy_dice_cost, out int max_pos)
        {
            game_free_dice_remains = 0;
            player_now_pos = 0;
            buy_dice_cost = 10;
            max_pos = 20;
            bool flag = gameId < 1 || gameId > 3;
            int result;
            if (flag)
            {
                result = 10;
            }
            else
            {
                string url = "/root/treasureGame!getNewGameInfo.action";
                string data = "newId=" + gameId;
                string[] array = new string[]
				{
					"探宝信息",
					"玉石探宝",
					"宝石探宝",
					"兵器探宝"
				};
                string text = "开启超级探宝之" + array[gameId];
                ServerResult serverResult = protocol.postXml(url, data, text);
                bool flag2 = serverResult == null;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    bool flag3 = !serverResult.CmdSucceed;
                    if (flag3)
                    {
                        result = 10;
                    }
                    else
                    {
                        XmlDocument cmdResult = serverResult.CmdResult;
                        XmlNode xmlNode = cmdResult.SelectSingleNode("/results/newdice");
                        bool flag4 = xmlNode != null;
                        if (flag4)
                        {
                            int.TryParse(xmlNode.InnerText, out game_free_dice_remains);
                        }
                        XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/playerpos");
                        bool flag5 = xmlNode2 != null;
                        if (flag5)
                        {
                            int.TryParse(xmlNode2.InnerText, out player_now_pos);
                        }
                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/buydicecost");
                        bool flag6 = xmlNode3 != null;
                        if (flag6)
                        {
                            int.TryParse(xmlNode3.InnerText, out buy_dice_cost);
                        }
                        XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/grid");
                        bool flag7 = xmlNodeList != null;
                        if (flag7)
                        {
                            max_pos = xmlNodeList.Count;
                        }
                        base.logInfo(logger, string.Format("{0}, 还剩免费骰子[{1}]个, 当前位置在[{2}]", text, game_free_dice_remains, player_now_pos));
                        result = 0;
                    }
                }
            }
            return result;
        }

        private int leaveSuperGame(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/treasureGame!leaveNewGame.action";
            ServerResult xml = protocol.getXml(url, "离开超级探宝");
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
                    base.logInfo(logger, "离开超级探宝");
                    result = 0;
                }
            }
            return result;
        }

        public int useNewDice(ProtocolMgr protocol, ILogger logger, int buy_dice_cost, bool useGold, out int game_free_dice_remains, out int player_now_pos)
        {
            game_free_dice_remains = 0;
            player_now_pos = 0;
            string url = "/root/treasureGame!useNewDice.action";
            string text = "超级探宝掷骰子";
            if (useGold)
            {
                url = "/root/treasureGame!buyNewGameDice.action";
                text = string.Format("超级探宝购买骰子, 花费{0}金币", buy_dice_cost);
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
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    int num = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/dicenum");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/newdice");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out game_free_dice_remains);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/playerpos");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out player_now_pos);
                    }
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(string.Format("{0}, 扔出了[{1}]点, 当前位置[{2}], 获得奖励: ", text, num, player_now_pos));
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/reward");
                    foreach (XmlNode xmlNode4 in xmlNodeList)
                    {
                        bool flag6 = xmlNode4 != null && xmlNode4.HasChildNodes;
                        if (flag6)
                        {
                            string arg = "";
                            string arg2 = "";
                            XmlNodeList childNodes = xmlNode4.ChildNodes;
                            foreach (XmlNode xmlNode5 in childNodes)
                            {
                                bool flag7 = xmlNode5.Name == "pos";
                                if (flag7)
                                {
                                    string innerText = xmlNode5.InnerText;
                                }
                                else
                                {
                                    bool flag8 = xmlNode5.Name == "rewardnum";
                                    if (flag8)
                                    {
                                        arg = xmlNode5.InnerText;
                                    }
                                    else
                                    {
                                        bool flag9 = xmlNode5.Name == "rewardname";
                                        if (flag9)
                                        {
                                            arg2 = xmlNode5.InnerText;
                                        }
                                    }
                                }
                            }
                            stringBuilder.Append(string.Format("{0}+{1} ", arg2, arg));
                        }
                    }
                    base.logInfo(logger, stringBuilder.ToString());
                    result = 0;
                }
            }
            return result;
        }

        public List<TGame> getAllDailyTreasureGames(ProtocolMgr protocol, ILogger logger)
        {
            List<TGame> list = new List<TGame>();
            string url = "/root/dayTreasureGame!getAllGameInfo.action";
            ServerResult xml = protocol.getXml(url, "获取日常探宝信息");
            bool flag = xml == null;
            List<TGame> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = list;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/game");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        bool flag3 = xmlNode != null && xmlNode.HasChildNodes;
                        if (flag3)
                        {
                            TGame tGame = new TGame();
                            XmlNodeList childNodes = xmlNode.ChildNodes;
                            foreach (XmlNode xmlNode2 in childNodes)
                            {
                                bool flag4 = xmlNode2.Name == "tid";
                                if (flag4)
                                {
                                    tGame.tid = int.Parse(xmlNode2.InnerText);
                                }
                                else
                                {
                                    bool flag5 = xmlNode2.Name == "cardreward";
                                    if (flag5)
                                    {
                                        tGame.cardreward = xmlNode2.InnerText;
                                    }
                                    else
                                    {
                                        bool flag6 = xmlNode2.Name == "powername";
                                        if (flag6)
                                        {
                                            tGame.powername = xmlNode2.InnerText;
                                        }
                                        else
                                        {
                                            bool flag7 = xmlNode2.Name == "coppercost";
                                            if (flag7)
                                            {
                                                tGame.coppercost = int.Parse(xmlNode2.InnerText);
                                            }
                                            else
                                            {
                                                bool flag8 = xmlNode2.Name == "open";
                                                if (flag8)
                                                {
                                                    tGame.open = int.Parse(xmlNode2.InnerText);
                                                }
                                                else
                                                {
                                                    bool flag9 = xmlNode2.Name == "maxpos";
                                                    if (flag9)
                                                    {
                                                        tGame.maxpos = int.Parse(xmlNode2.InnerText);
                                                    }
                                                    else
                                                    {
                                                        bool flag10 = xmlNode2.Name == "pos";
                                                        if (flag10)
                                                        {
                                                            tGame.pos = int.Parse(xmlNode2.InnerText);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            bool flag11 = tGame.open != 0;
                            if (flag11)
                            {
                                list.Add(tGame);
                            }
                        }
                    }
                    result = list;
                }
            }
            return result;
        }

        public int handleDailyTreasureGameInfo(ProtocolMgr protocol, ILogger logger, User user, int game_id, string _open_box_type, string _gold_move_box_type, int _minGoldSteps, bool super_gold_end, ref int gold_available, ref int silver_available)
        {
            bool flag = false;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string url = "/root/dayTreasureGame!getAllGameInfo.action";
            ServerResult xml = protocol.getXml(url, "获取日常探宝信息");
            bool flag2 = xml == null;
            int result;
            if (flag2)
            {
                result = 1;
            }
            else
            {
                bool flag3 = !xml.CmdSucceed;
                if (flag3)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cangeteventdice");
                    flag = (xmlNode != null && xmlNode.InnerText == "1");
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/dice");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/eventdice");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out num2);
                    }
                    num += num2;
                    List<TGame> list = new List<TGame>();
                    TGame tGame = null;
                    TGame tGame2 = null;
                    TGame tGame3 = null;
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/game");
                    foreach (XmlNode xmlNode4 in xmlNodeList)
                    {
                        bool flag6 = xmlNode4 != null && xmlNode4.HasChildNodes;
                        if (flag6)
                        {
                            TGame tGame4 = new TGame();
                            XmlNodeList childNodes = xmlNode4.ChildNodes;
                            foreach (XmlNode xmlNode5 in childNodes)
                            {
                                bool flag7 = xmlNode5.Name == "tid";
                                if (flag7)
                                {
                                    tGame4.tid = int.Parse(xmlNode5.InnerText);
                                }
                                else
                                {
                                    bool flag8 = xmlNode5.Name == "powername";
                                    if (flag8)
                                    {
                                        tGame4.powername = xmlNode5.InnerText;
                                    }
                                    else
                                    {
                                        bool flag9 = xmlNode5.Name == "coppercost";
                                        if (flag9)
                                        {
                                            tGame4.coppercost = int.Parse(xmlNode5.InnerText);
                                        }
                                        else
                                        {
                                            bool flag10 = xmlNode5.Name == "open";
                                            if (flag10)
                                            {
                                                tGame4.open = int.Parse(xmlNode5.InnerText);
                                            }
                                            else
                                            {
                                                bool flag11 = xmlNode5.Name == "maxpos";
                                                if (flag11)
                                                {
                                                    tGame4.maxpos = int.Parse(xmlNode5.InnerText);
                                                }
                                                else
                                                {
                                                    bool flag12 = xmlNode5.Name == "pos";
                                                    if (flag12)
                                                    {
                                                        tGame4.pos = int.Parse(xmlNode5.InnerText);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            bool flag13 = tGame4.open != 0;
                            if (flag13)
                            {
                                bool flag14 = tGame4.maxpos > 0;
                                if (flag14)
                                {
                                    tGame = tGame4;
                                }
                                bool flag15 = tGame2 == null || tGame2.tid < tGame4.tid;
                                if (flag15)
                                {
                                    tGame2 = tGame4;
                                }
                                bool flag16 = tGame4.tid == game_id;
                                if (flag16)
                                {
                                    tGame3 = tGame4;
                                }
                                list.Add(tGame4);
                            }
                        }
                    }
                    bool flag17 = tGame3 != null;
                    if (flag17)
                    {
                        tGame2 = tGame3;
                    }
                    bool flag18 = tGame != null;
                    if (flag18)
                    {
                        num3 = tGame.pos;
                    }
                    bool flag19 = game_id == 0;
                    if (flag19)
                    {
                        base.logInfo(logger, "没有选择使用哪个探宝副本, 将自动选择");
                    }
                    bool flag20 = list.Count == 0;
                    if (flag20)
                    {
                        result = 2;
                    }
                    else
                    {
                        bool flag21 = flag;
                        if (flag21)
                        {
                            this.getEventDiceDailyTreasure(protocol, logger);
                        }
                        bool flag22 = num > 0;
                        if (flag22)
                        {
                            base.logInfo(logger, string.Format("获得日常探宝信息, 当前还剩寻宝次数{0}次, 玩家目前位于{1}位置", num, num3));
                        }
                        List<ActivityMgr.GameGrid> grids = new List<ActivityMgr.GameGrid>();
                        bool flag23 = false;
                        int num4 = 0;
                        int num5 = 20;
                        bool flag24 = tGame != null;
                        if (flag24)
                        {
                            int gameInfoDailyTreasure = this.getGameInfoDailyTreasure(protocol, logger, tGame.tid, grids, ref num3, tGame, out flag23, out num4, out num5);
                            bool flag25 = gameInfoDailyTreasure == 1;
                            if (flag25)
                            {
                                result = 1;
                                return result;
                            }
                            bool flag26 = !flag23;
                            if (flag26)
                            {
                                this.do_game_DailyTreasure(protocol, logger, user, _open_box_type, _gold_move_box_type, _minGoldSteps, ref gold_available, grids, ref num3, ref tGame);
                            }
                            else
                            {
                                bool flag27 = num4 > 0 || (super_gold_end && gold_available >= num5);
                                if (flag27)
                                {
                                    this.do_game_DailyTreasure_super(protocol, logger, user, ref gold_available, super_gold_end, num5, ref num3, ref num4, ref tGame);
                                }
                                else
                                {
                                    this.leave_daily_treasure_SuperGame(protocol, logger, tGame.tid);
                                }
                            }
                        }
                        bool flag28 = num == 0;
                        if (flag28)
                        {
                            bool flag29 = tGame == null;
                            if (flag29)
                            {
                                result = 2;
                                return result;
                            }
                        }
                        else
                        {
                            bool flag30 = num > 0;
                            if (flag30)
                            {
                                bool flag31 = tGame2 == null;
                                if (flag31)
                                {
                                    result = 1;
                                    return result;
                                }
                                bool flag32 = silver_available < tGame2.coppercost;
                                if (flag32)
                                {
                                    result = 3;
                                    return result;
                                }
                                bool flag33 = this.openDailyTreasureBySilver(protocol, logger, tGame2.tid, list, ref num3, out tGame, out flag23) == 0;
                                if (flag33)
                                {
                                    bool flag34 = flag23;
                                    if (flag34)
                                    {
                                        this.do_game_DailyTreasure_super(protocol, logger, user, ref gold_available, super_gold_end, num5, ref num3, ref num4, ref tGame);
                                    }
                                    else
                                    {
                                        this.do_game_DailyTreasure(protocol, logger, user, _open_box_type, _gold_move_box_type, _minGoldSteps, ref gold_available, grids, ref num3, ref tGame);
                                    }
                                }
                            }
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        private void do_game_DailyTreasure(ProtocolMgr protocol, ILogger logger, User user, string _open_box_type, string _gold_move_box_type, int _minGoldSteps, ref int gold_available, List<ActivityMgr.GameGrid> _grids, ref int _player_pos_now, ref TGame _doing_game)
        {
            int num = 30;
            while (_player_pos_now < _doing_game.maxpos && num > 0)
            {
                int num2 = num;
                num = num2 - 1;
                int nextStepDice = this.getNextStepDice(true, user, gold_available, _gold_move_box_type, _minGoldSteps, _player_pos_now, _grids);
                bool flag = !this.useDiceDailyTreasure(protocol, logger, nextStepDice, _doing_game.tid, _grids, ref _player_pos_now, ref gold_available, ref _open_box_type, _doing_game);
                if (flag)
                {
                    break;
                }
            }
        }

        private void do_game_DailyTreasure_super(ProtocolMgr protocol, ILogger logger, User user, ref int gold_available, bool super_gold_end, int buydice_gold, ref int _player_pos_now, ref int super_treasure_remain_dice, ref TGame _doing_game)
        {
            int num = 30;
            while (_player_pos_now < _doing_game.maxpos && num > 0)
            {
                int num2 = num;
                num = num2 - 1;
                bool flag = super_treasure_remain_dice > 0;
                if (flag)
                {
                    bool flag2 = !this.useDiceDailyTreasure_super(protocol, logger, _doing_game.tid, ref _player_pos_now, ref gold_available, ref super_treasure_remain_dice, 0);
                    if (flag2)
                    {
                        break;
                    }
                }
                else
                {
                    bool flag3 = !super_gold_end;
                    if (flag3)
                    {
                        break;
                    }
                    bool flag4 = buydice_gold > gold_available;
                    if (flag4)
                    {
                        break;
                    }
                    bool flag5 = !this.useDiceDailyTreasure_super(protocol, logger, _doing_game.tid, ref _player_pos_now, ref gold_available, ref super_treasure_remain_dice, buydice_gold);
                    if (flag5)
                    {
                        break;
                    }
                    gold_available -= buydice_gold;
                }
            }
        }

        private int leave_daily_treasure_SuperGame(ProtocolMgr protocol, ILogger logger, int tid)
        {
            string url = "/root/dayTreasureGame!playAgain.action";
            string data = "tid=" + tid;
            ServerResult serverResult = protocol.postXml(url, data, "离开每日探宝之超级探宝");
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
                    base.logInfo(logger, "离开每日探宝之超级探宝");
                    result = 0;
                }
            }
            return result;
        }

        private int getEventDiceDailyTreasure(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/dayTreasureGame!getEventDice.action";
            ServerResult xml = protocol.getXml(url, "获取每日探宝免费令");
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
                    result = 0;
                }
            }
            return result;
        }

        private int openDailyTreasureBySilver(ProtocolMgr protocol, ILogger logger, int tid, List<TGame> _dailyGames, ref int _player_pos_now, out TGame _doing_game, out bool super_treasure)
        {
            super_treasure = false;
            _doing_game = null;
            string url = "/root/dayTreasureGame!getGameInfo.action";
            string data = "tid=" + tid;
            ServerResult serverResult = protocol.postXml(url, data, "开启探宝地图");
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
                    foreach (TGame current in _dailyGames)
                    {
                        bool flag3 = current.tid == tid;
                        if (flag3)
                        {
                            _doing_game = current;
                            _player_pos_now = 0;
                            break;
                        }
                    }
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/treasurestate");
                    bool flag4 = xmlNode != null;
                    if (flag4)
                    {
                        super_treasure = (xmlNode.InnerText == "1");
                    }
                    result = 0;
                }
            }
            return result;
        }

        private bool confirmFateDailyTreasure(ProtocolMgr protocol, ILogger logger, int tid, int fateId, ref int _player_pos_now)
        {
            string url = "/root/dayTreasureGame!useDice.action";
            string data = string.Format("useFateDice={0}&tid={1}", fateId, tid);
            string desc = string.Format("面对命运筛子, 我选择{0}, 玩家位置在{1}", (fateId == 1) ? "相信命运" : "自己掌握", _player_pos_now);
            ServerResult serverResult = protocol.postXml(url, data, desc);
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                result = true;
            }
            return result;
        }

        private int getGameInfoDailyTreasure(ProtocolMgr protocol, ILogger logger, int tid, List<ActivityMgr.GameGrid> _grids, ref int _player_pos_now, TGame _doing_game, out bool super_treasure, out int super_treasure_remain_dice, out int super_dice_buygold)
        {
            super_treasure = false;
            super_treasure_remain_dice = 0;
            super_dice_buygold = 20;
            string url = "/root/dayTreasureGame!getGameInfo.action";
            string data = "tid=" + tid;
            ServerResult serverResult = protocol.postXml(url, data, "获取日常探宝信息");
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
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/treasurestate");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        super_treasure = (xmlNode.InnerText == "1");
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/newdicenum");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out super_treasure_remain_dice);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/buynewdicecost");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out super_dice_buygold);
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/playerpos");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        int.TryParse(xmlNode4.InnerText, out _player_pos_now);
                    }
                    else
                    {
                        _player_pos_now = 0;
                    }
                    this.renderGrids(logger, cmdResult, _grids);
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/question");
                    bool flag7 = xmlNode5 != null;
                    if (flag7)
                    {
                        this.handleQuestion(protocol, logger, xmlNode5, _doing_game);
                    }
                    result = 0;
                }
            }
            return result;
        }

        private bool useDiceDailyTreasure(ProtocolMgr protocol, ILogger logger, int diceId, int tid, List<ActivityMgr.GameGrid> _grids, ref int _player_pos_now, ref int gold_available, ref string _open_box_type, TGame _doing_game)
        {
            string url = "/root/dayTreasureGame!useDice.action";
            string data = string.Format("controlDiceId={0}&tid={1}", diceId, tid);
            string text = string.Format("日常探宝掷筛子, 玩家位置在{0}, ", _player_pos_now);
            bool flag = diceId == 1;
            if (flag)
            {
                text += "使用2金币遥控筛子,只走一步, ";
            }
            else
            {
                bool flag2 = diceId == 2;
                if (flag2)
                {
                    text += "使用1金币遥控筛子,只走两步, ";
                }
            }
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag3 = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag3)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                text += this.handleDiceResult(protocol, logger, cmdResult, _grids, ref _player_pos_now, ref gold_available, ref _open_box_type, _doing_game);
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        private bool useDiceDailyTreasure_super(ProtocolMgr protocol, ILogger logger, int tid, ref int player_now_pos, ref int gold_available, ref int game_free_dice_remains, int super_buy_gold)
        {
            string url = "/root/dayTreasureGame!useDice.action";
            string data = string.Format("gold={0}&controlDiceId=0&tid={1}", (super_buy_gold > 0) ? 1 : 0, tid);
            string text = string.Format("日常探宝掷筛子, 玩家位置在{0}, ", player_now_pos);
            bool flag = super_buy_gold > 0;
            if (flag)
            {
                text += string.Format("花费金币[{0}]个", super_buy_gold);
            }
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag2 = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag2)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int num = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/dicenum");
                bool flag3 = xmlNode != null;
                if (flag3)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/step/newdicenum");
                bool flag4 = xmlNode2 != null;
                if (flag4)
                {
                    int.TryParse(xmlNode2.InnerText, out game_free_dice_remains);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/step/nextpos");
                bool flag5 = xmlNode3 != null;
                if (flag5)
                {
                    int.TryParse(xmlNode3.InnerText, out player_now_pos);
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format("{0}, 扔出了[{1}]点, 当前位置[{2}], 获得奖励: ", text, num, player_now_pos));
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/step/rewardinfo/reward");
                foreach (XmlNode xmlNode4 in xmlNodeList)
                {
                    bool flag6 = xmlNode4 != null && xmlNode4.HasChildNodes;
                    if (flag6)
                    {
                        string text2 = "";
                        string arg = "";
                        XmlNodeList childNodes = xmlNode4.ChildNodes;
                        foreach (XmlNode xmlNode5 in childNodes)
                        {
                            bool flag7 = xmlNode5.Name == "type";
                            if (flag7)
                            {
                                text2 = xmlNode5.InnerText;
                            }
                            else
                            {
                                bool flag8 = xmlNode5.Name == "num";
                                if (flag8)
                                {
                                    arg = xmlNode5.InnerText;
                                }
                            }
                        }
                        bool flag9 = text2 == "2";
                        string arg2;
                        if (flag9)
                        {
                            arg2 = "玉石";
                        }
                        else
                        {
                            bool flag10 = text2 == "5";
                            if (flag10)
                            {
                                arg2 = "宝石";
                            }
                            else
                            {
                                bool flag11 = text2 == "34";
                                if (flag11)
                                {
                                    arg2 = "行动力";
                                }
                                else
                                {
                                    arg2 = string.Format("[type={0}]", text2);
                                }
                            }
                        }
                        stringBuilder.Append(string.Format("{0}+{1} ", arg2, arg));
                    }
                }
                base.logInfo(logger, stringBuilder.ToString());
                result = true;
            }
            return result;
        }

        protected string handleDiceResult(ProtocolMgr protocol, ILogger logger, XmlDocument xml, List<ActivityMgr.GameGrid> _grids, ref int _player_pos_now, ref int gold_available, ref string _open_box_type, TGame _doing_game)
        {
            string text = "";
            int num = 0;
            XmlNode xmlNode = xml.SelectSingleNode("/results/dicenum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out num);
            }
            XmlNodeList xmlNodeList = xml.SelectNodes("/results/step/nextpos");
            if (xmlNodeList != null && xmlNodeList.Count > 0)
            {
                XmlNode xmlNode2 = xmlNodeList.Item(xmlNodeList.Count - 1);
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out _player_pos_now);
                }
            }
            ActivityMgr.GameGrid gameGrid = _grids[_player_pos_now - 1];
            text += string.Format("掷出了{0}点, ", num);
            XmlNodeList xmlNodeList2 = xml.SelectNodes("/results/step");
            foreach (XmlNode xmlNode3 in xmlNodeList2)
            {
                if (xmlNode3 != null && xmlNode3.HasChildNodes)
                {
                    XmlNodeList childNodes = xmlNode3.ChildNodes;
                    foreach (XmlNode xmlNode4 in childNodes)
                    {
                        if (xmlNode4.Name == "rewardinfo")
                        {
                            XmlNodeList childNodes2 = xmlNode4.ChildNodes;
                            foreach (XmlNode reward_node in childNodes2)
                            {
                                text += this.getRewardName(reward_node);
                            }
                        }
                        else if (xmlNode4.Name == "info")
                        {
                            string innerText = xmlNode4.InnerText;
                            if (innerText.Equals("goldbox"))
                            {
                                int num2 = 0;
                                XmlNode xmlNode5 = xml.SelectSingleNode("/results/step/goldboxcost");
                                if (xmlNode5 != null)
                                {
                                    int.TryParse(xmlNode5.InnerText, out num2);
                                }
                                if (num2 <= gold_available && ((gameGrid.star == 2 && _open_box_type.Contains("2")) || (gameGrid.star == 3 && _open_box_type.Contains("3"))))
                                {
                                    text += string.Format("运气不错, 碰上了{0}星金宝箱, 需要{1}金币才能开, 犹豫再三, 开了宝箱!! ", gameGrid.star, num2);
                                    string str = this.openBoxOrNot(protocol, logger, true, _doing_game);
                                    text += str;
                                    gold_available -= num2;
                                }
                                else
                                {
                                    this.openBoxOrNot(protocol, logger, false, _doing_game);
                                    text += string.Format("运气不好, 碰上了{0}星金宝箱, 需要{1}金币才能开, 不开! 走人~~", gameGrid.star, num2);
                                }
                            }
                            else if (innerText.Equals("box"))
                            {
                                text += "哇, 我们遇到了不花钱的宝箱耶, ";
                            }
                            else if (innerText.Equals("ghostbox"))
                            {
                                text += "哇塞，我们遇到了神秘宝箱";
                            }
                            else if (innerText.Equals("times"))
                            {
                                text += "天降某种机缘, ";
                            }
                            else if (innerText.Equals("battle"))
                            {
                                text += "打了一架, ";
                            }
                            else if (innerText.Equals("punish"))
                            {
                                text += "天有不测风云, ";
                            }
                            else if (innerText.Equals("move"))
                            {
                                text += "碰到移动神坛, 继续移动==>; ";
                            }
                            else if (innerText.Equals("tree"))
                            {
                                text += "哇塞, 碰到了摇钱树!!!! ";
                                text += this.doShakeTree(protocol, logger, _doing_game);
                            }
                            else if (innerText.Equals("fin"))
                            {
                                text += "到达终点, ";
                            }
                            else if (innerText.Equals("question"))
                            {
                            }
                        }
                        else if (xmlNode4.Name == "question")
                        {
                            this.handleQuestion(protocol, logger, xmlNode4, _doing_game);
                        }
                    }
                }
            }
            return text;
        }

        protected string getRewardName(XmlNode reward_node)
        {
            string result;
            if (reward_node == null || !reward_node.HasChildNodes)
            {
                result = "";
            }
            else
            {
                XmlNodeList childNodes = reward_node.ChildNodes;
                int type = 0;
                int num = 0;
                string itemname = "";
                foreach (XmlNode xmlNode in childNodes)
                {
                    if (xmlNode.Name == "type")
                    {
                        int.TryParse(xmlNode.InnerText, out type);
                    }
                    else if (xmlNode.Name == "num")
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    else if (xmlNode.Name == "itemname")
                    {
                        itemname = xmlNode.InnerText;
                    }
                }
                string reward = string.Format("[type={0},name={1}]", type, reward_node.InnerXml);
                if (type == 1)
                {
                    reward = "银币";
                }
                else if (type == 2)
                {
                    reward = "玉石";
                }
                else if (type == 5)
                {
                    reward = "宝石";
                }
                else if (type == 6)
                {
                    reward = string.Format("兵器[{0}]", itemname);
                }
                else if (type == 7)
                {
                    reward = string.Format("兵器碎片[{0}]", itemname);
                }
                else if (type == 8)
                {
                    reward = "征收次数";
                }
                else if (type == 9)
                {
                    reward = "纺织次数";
                }
                else if (type == 10)
                {
                    reward = "通商次数";
                }
                else if (type == 11)
                {
                    reward = "炼化次数";
                }
                else if (type == 12)
                {
                    reward = "兵力减少";
                }
                else if (type == 13)
                {
                    reward = "副本重置卡";
                }
                else if (type == 14)
                {
                    reward = "战役双倍卡";
                }
                else if (type == 15)
                {
                    reward = "强化暴击卡";
                }
                else if (type == 16)
                {
                    reward = "强化打折卡";
                }
                else if (type == 17)
                {
                    reward = "兵器提升卡";
                }
                else if (type == 18)
                {
                    reward = "兵器暴击卡";
                }
                else if (type == 27)
                {
                    reward = "进货令";
                }
                else if (type == 28)
                {
                    reward = "军令";
                }
                else if (type == 29)
                {
                    reward = "政绩翻倍卡";
                }
                else if (type == 30)
                {
                    reward = "征收翻倍卡";
                }
                else if (type == 31)
                {
                    reward = "商人召唤卡";
                }
                else if (type == 32)
                {
                    reward = "纺织翻倍卡";
                }
                else if (type == 34)
                {
                    reward = "行动力";
                }
                else if (type == 35)
                {
                    reward = "摇钱树";
                }
                else if (type == 36)
                {
                    reward = "超级门票";
                }
                else if (type == 43)
                {
                    reward = "神秘宝箱";
                }
                result = string.Format("{0}{1}{2}", reward, (num > 0) ? "+" : "", num);
            }
            return result;
        }

        protected string doShakeTree(ProtocolMgr protocol, ILogger logger, TGame _doing_game)
        {
            string url;
            if (_doing_game == null)
            {
                url = "/root/treasureGame!startShake.action";
            }
            else
            {
                url = "/root/dayTreasureGame!startShake.action";
            }
            string text = "摇晃摇钱树, 天灵灵地灵灵~~~~";
            int num = 3;
            while (num > 0)
            {
                ServerResult xml = protocol.getXml(url, "摇钱树摇钱");
                if (xml == null || !xml.CmdSucceed)
                {
                    break;
                }
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/copper");
                if (xmlNode != null)
                {
                    text = text + " 银币+" + xmlNode.InnerText;
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/bowlder");
                if (xmlNode2 != null)
                {
                    text = text + " 玉石+" + xmlNode2.InnerText;
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/baoshi");
                if (xmlNode3 != null)
                {
                    text = text + " 宝石+" + xmlNode3.InnerText;
                }
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/shake");
                if (xmlNode4 != null)
                {
                    int.TryParse(xmlNode4.InnerText, out num);
                }
            }
            return text;
        }

        protected void handleQuestion(ProtocolMgr protocol, ILogger logger, XmlNode xml, TGame _doing_game)
        {
            int num = 0;
            int num2 = 0;
            string question = "";
            string answer = "";
            string answer2 = "";
            XmlNode xmlNode = xml.SelectSingleNode("qnum");
            bool flag = xmlNode != null;
            if (flag)
            {
                int.TryParse(xmlNode.InnerText, out num);
            }
            else
            {
                num = 0;
            }
            XmlNode xmlNode2 = xml.SelectSingleNode("islast");
            bool flag2 = xmlNode2 != null;
            if (flag2)
            {
                int.TryParse(xmlNode2.InnerText, out num2);
            }
            else
            {
                num2 = 0;
            }
            XmlNode xmlNode3 = xml.SelectSingleNode("q");
            bool flag3 = xmlNode3 != null;
            if (flag3)
            {
                question = xmlNode3.InnerText;
            }
            XmlNode xmlNode4 = xml.SelectSingleNode("a1");
            bool flag4 = xmlNode4 != null;
            if (flag4)
            {
                answer = xmlNode4.InnerText;
            }
            XmlNode xmlNode5 = xml.SelectSingleNode("a2");
            bool flag5 = xmlNode5 != null;
            if (flag5)
            {
                answer2 = xmlNode5.InnerText;
            }
            int questionAnswer = this.getQuestionAnswer(logger, question, answer, answer2);
            bool flag6 = _doing_game == null;
            string url;
            string data;
            if (flag6)
            {
                url = "/root/treasureGame!doQuestion.action";
                data = string.Format("answerId={1}&qnum={0}", num, questionAnswer);
            }
            else
            {
                url = "/root/dayTreasureGame!doQuestion.action";
                data = string.Format("tid={0}&answerId={2}&qnum={1}", _doing_game.tid, num, questionAnswer);
            }
            ServerResult serverResult = protocol.postXml(url, data, "探宝回答问题");
            bool flag7 = serverResult == null || !serverResult.CmdSucceed;
            if (!flag7)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/correct");
                bool flag8 = xmlNode6 != null;
                if (flag8)
                {
                    base.logInfo(logger, string.Format("问题回答{0}", (xmlNode6.InnerText == "1") ? "正确" : "错误"));
                }
                XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/step/question");
                bool flag9 = xmlNode7 != null;
                if (flag9)
                {
                    bool flag10 = num2 == 0;
                    if (flag10)
                    {
                        this.handleQuestion(protocol, logger, xmlNode7, _doing_game);
                    }
                    else
                    {
                        base.logInfo(logger, xmlNode7.InnerXml);
                    }
                }
                else
                {
                    xmlNode7 = cmdResult.SelectSingleNode("/results/question");
                    bool flag11 = xmlNode7 != null;
                    if (flag11)
                    {
                        this.handleQuestion(protocol, logger, xmlNode7, _doing_game);
                    }
                    else
                    {
                        XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/rewardinfo");
                        bool flag12 = xmlNode8 != null;
                        if (flag12)
                        {
                            string text = "答题成功, 获得";
                            XmlNodeList childNodes = xmlNode8.ChildNodes;
                            foreach (XmlNode reward_node in childNodes)
                            {
                                text += this.getRewardName(reward_node);
                            }
                            base.logInfo(logger, text);
                        }
                    }
                }
            }
        }

        private int getQuestionAnswer(ILogger logger, string question, string answer1, string answer2)
        {
            bool flag = this._question_db.Count == 0;
            if (flag)
            {
                this.initQuestionDb();
            }
            int num = 1;
            bool flag2 = false;
            foreach (ActivityMgr.Question current in this._question_db)
            {
                bool flag3 = current.question.IndexOf(question) >= 0;
                if (flag3)
                {
                    num = current.answer;
                    flag2 = true;
                    break;
                }
            }
            bool flag4 = flag2;
            if (flag4)
            {
                string text = string.Format("回答问题:[{0}], 1:[{1}], 2:[{2}], 回答[{3}]", new object[]
				{
					question,
					answer1,
					answer2,
					num
				});
                base.logInfo(logger, text);
            }
            else
            {
                string text2 = string.Format("回答问题:[{0}], 1:[{1}], 2:[{2}], 题库未找到, 默认回答1", question, answer1, answer2);
                base.logInfo(logger, text2);
                logger.logSurprise(text2);
            }
            return num;
        }

        protected string openBoxOrNot(ProtocolMgr protocol, ILogger logger, bool open, TGame _doing_game)
        {
            bool flag = _doing_game == null;
            string url;
            string data;
            if (flag)
            {
                url = "/root/treasureGame!confirmOpenGoldBox.action";
                data = "openGoldBox=" + (open ? "1" : "0");
            }
            else
            {
                url = "/root/dayTreasureGame!confirmOpenGoldBox.action";
                data = string.Format("openGoldBox={0}&tid={1}", open ? "1" : "0", _doing_game.tid);
            }
            ServerResult serverResult = protocol.postXml(url, data, "探宝打开宝箱");
            bool flag2 = serverResult == null || !serverResult.CmdSucceed;
            string result;
            if (flag2)
            {
                result = "";
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/rewardinfo/reward");
                string text = "获得 ";
                foreach (XmlNode reward_node in xmlNodeList)
                {
                    text = text + this.getRewardName(reward_node) + "  ";
                }
                result = text;
            }
            return result;
        }

        protected int getNextStepDice(bool isDaily, User user, int gold_available, string _gold_move_box_type, int _minGoldSteps, int _player_pos_now, List<ActivityMgr.GameGrid> _grids)
        {
            int result;
            if (_minGoldSteps <= 0)
            {
                result = 0;
            }
            else
            {
                if (!_gold_move_box_type.Contains("0") && !_gold_move_box_type.Contains("1") && !_gold_move_box_type.Contains("2") && !_gold_move_box_type.Contains("3") && !_gold_move_box_type.Contains("4") && !_gold_move_box_type.Contains("5") && !_gold_move_box_type.Contains("6"))
                {
                    result = 0;
                }
                else
                {
                    int num = -1;
                    int num2 = -1;
                    for (int i = _player_pos_now; i < _grids.Count; i++)
                    {
                        ActivityMgr.GameGrid gameGrid = _grids[i];
                        if (gameGrid != null)
                        {
                            if (gameGrid.info.Equals("move"))
                            {
                                num2 = i + 1;
                            }
                            else if (gameGrid.info.Equals("box"))
                            {
                                if (gameGrid.star == 0 && _gold_move_box_type.Contains("0"))
                                {
                                    num = i + 1;
                                    break;
                                }
                                if (gameGrid.star == 1 && _gold_move_box_type.Contains("1"))
                                {
                                    num = i + 1;
                                    break;
                                }
                            }
                            else if (gameGrid.info.Equals("goldbox"))
                            {
                                if (gameGrid.star == 2)
                                {
                                    if (_gold_move_box_type.Contains("2"))
                                    {
                                        num = i + 1;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (gameGrid.star == 3 && _gold_move_box_type.Contains("3"))
                                    {
                                        num = i + 1;
                                        break;
                                    }
                                }
                            }
                            else if (gameGrid.info.Equals("ghostbox"))
                            {
                                if (gameGrid.star == 1 && _gold_move_box_type.Contains("1"))
                                {
                                    num = i + 1;
                                    break;
                                }
                                if (gameGrid.star == 2 && _gold_move_box_type.Contains("2"))
                                {
                                    num = i + 1;
                                    break;
                                }
                                if (gameGrid.star == 3 && _gold_move_box_type.Contains("3"))
                                {
                                    num = i + 1;
                                    break;
                                }
                            }
                            else if (gameGrid.info.Equals("tree"))
                            {
                                if (_gold_move_box_type.Contains("4"))
                                {
                                    num = i + 1;
                                    break;
                                }
                            }
                            else if (gameGrid.info.Equals("ticket"))
                            {
                                if (_gold_move_box_type.Contains("5"))
                                {
                                    num = i + 1;
                                    break;
                                }
                            }
                            else if (gameGrid.info.Equals("times"))
                            {
                                if (isDaily)
                                {
                                    if (user._is_new_trade && _gold_move_box_type.Contains("6"))
                                    {
                                        num = i + 1;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!gameGrid.name.Contains("民居") && user._is_new_trade && _gold_move_box_type.Contains("6"))
                                    {
                                        num = i + 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (num < 0)
                    {
                        result = 0;
                    }
                    else
                    {
                        int num4 = 0;
                        if (num2 > 0)
                        {
                            int num5 = isDaily ? 2 : 3;
                            if (num - num2 == num5)
                            {
                                num4 = num2 - _player_pos_now;
                            }
                            else if (num - num2 > num5)
                            {
                                if (num - _player_pos_now > _minGoldSteps)
                                {
                                    num4 = num - _player_pos_now;
                                }
                                else
                                {
                                    num4 = num2 - _player_pos_now;
                                }
                            }
                            else if (num - num2 < num5)
                            {
                                if (num2 - _player_pos_now == 1)
                                {
                                    num4 = num2 + 1 - _player_pos_now;
                                }
                                else
                                {
                                    if (num2 - _player_pos_now > 2)
                                    {
                                        if (num - _player_pos_now > _minGoldSteps)
                                        {
                                            num4 = num - _player_pos_now;
                                        }
                                        else
                                        {
                                            num4 = num2 - 1 - _player_pos_now;
                                        }
                                    }
                                    else
                                    {
                                        if (num2 - _player_pos_now == 2)
                                        {
                                            num4 = num2 - 1 - _player_pos_now;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            num4 = num - _player_pos_now;
                        }
                        if (num4 > _minGoldSteps)
                        {
                            result = 0;
                        }
                        else if (num4 >= 2)
                        {
                            if (gold_available >= 1)
                            {
                                result = 2;
                            }
                            else
                            {
                                result = 0;
                            }
                        }
                        else if (num4 != 1)
                        {
                            result = 0;
                        }
                        else
                        {
                            if (gold_available >= 2)
                            {
                                result = 1;
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

        protected int[] convertDiceType(string _dice_type, int length)
        {
            int[] diceArray = new int[length];
            if (_dice_type != null && _dice_type.Length > 0)
            {
                string[] separator = new string[] { ":" };
                string[] diceTypes = _dice_type.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < diceTypes.Length; i++)
                {
                    string discType = diceTypes[i].Trim();
                    if (discType.Length > 0)
                    {
                        string[] pairs = discType.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        if (pairs.Length == 2)
                        {
                            if (pairs[0] != null && pairs[1] != null && pairs[0].Length > 0 && pairs[1].Length > 0)
                            {
                                int pos = 0, dice = 0;
                                int.TryParse(pairs[0], out pos);
                                int.TryParse(pairs[1], out dice);
                                diceArray[pos] = dice;
                            }
                        }
                    }
                }
            }
            return diceArray;
        }

        protected int getNextStepDice_new(int gold_available, int[] _dice_array, int _player_pos_now)
        {
            if (_dice_array.Length > 0)
            {
                if (_dice_array[_player_pos_now] == 1 && gold_available >= 2)
                {
                    return 1;
                }
                else if (_dice_array[_player_pos_now] == 2 && gold_available >= 1)
                {
                    return 2;
                }
            }
            return 0;
        }

        protected void renderGrids(ILogger logger, XmlDocument xml, List<ActivityMgr.GameGrid> _grids)
        {
            _grids.Clear();
            string ghostbox = "";
            string goldbox = "";
            string box = "";
            string tree = "";
            string ticket = "";
            XmlNodeList xmlNodeList = xml.SelectNodes("/results/grid");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                ActivityMgr.GameGrid gameGrid = new ActivityMgr.GameGrid();
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    if (xmlNode2.Name == "pos")
                    {
                        gameGrid.pos = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "name")
                    {
                        gameGrid.name = xmlNode2.InnerText;
                    }
                    else if (xmlNode2.Name == "star")
                    {
                        gameGrid.star = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "info")
                    {
                        gameGrid.info = xmlNode2.InnerText;
                    }
                }
                _grids.Add(gameGrid);
                if (gameGrid.info.Equals("goldbox"))
                {
                    goldbox = goldbox + gameGrid.pos + ",";
                }
                else if (gameGrid.info.Equals("box"))
                {
                    box = box + gameGrid.pos + ",";
                }
                else if (gameGrid.info.Equals("tree"))
                {
                    tree = tree + gameGrid.pos + ",";
                }
                else if (gameGrid.info.Equals("ticket"))
                {
                    ticket = ticket + gameGrid.pos + ",";
                }
                else if (gameGrid.info.Equals("ghostbox"))
                {
                    ghostbox = ghostbox + gameGrid.pos + ",";
                }
            }
            string info = string.Format("金宝箱位置在: [{0}], 免费宝箱位置在: [{1}], 神秘宝箱位置在: [{2}]", goldbox, box, ghostbox);
            if (tree.Length > 0)
            {
                info += string.Format(", 摇钱树位置在:[{0}]", tree);
            }
            if (ticket.Length > 0)
            {
                info += string.Format(", 超级门票在:[{0}]", ticket);
            }
            base.logInfo(logger, info);
        }

        protected void initQuestionDb()
        {
            this._question_db.Clear();
            this._question_db.Add(new ActivityMgr.Question("玩家刚进入游戏是什么剧情？", "桃园结义", "黄巾起义", 2));
            this._question_db.Add(new ActivityMgr.Question("什么阵形增加战法伤害", "锋矢阵", "鱼鳞阵", 1));
            this._question_db.Add(new ActivityMgr.Question("多少个麒麟碎片可以合成一个麒麟", "10", "20", 2));
            this._question_db.Add(new ActivityMgr.Question("蜀后主是谁？", "刘封", "刘禅", 2));
            this._question_db.Add(new ActivityMgr.Question("下列谁是“汉初三杰”", "萧何", "曹参", 1));
            this._question_db.Add(new ActivityMgr.Question("科技中的“冲锋”升级会增加____", "战法攻击", "普通攻击", 1));
            this._question_db.Add(new ActivityMgr.Question("人中吕布，马中_____", "追风", "赤兔", 2));
            this._question_db.Add(new ActivityMgr.Question("征收银币在哪里征收的？", "主城", "商店", 1));
            this._question_db.Add(new ActivityMgr.Question("长蛇阵增加_____", "战法伤害", "普通伤害", 2));
            this._question_db.Add(new ActivityMgr.Question("_____之心，路人皆知", "司马昭", "司马懿", 1));
            this._question_db.Add(new ActivityMgr.Question("天公将军是谁", "张宝", "张角", 2));
            this._question_db.Add(new ActivityMgr.Question("玩家可以通过____进入收徒界面", "校场", "兵营", 1));
            this._question_db.Add(new ActivityMgr.Question("七步成诗的是谁", "曹丕", "曹植", 2));
            this._question_db.Add(new ActivityMgr.Question("兵力不足后需要使用什么补足兵力？", "银币", "军功", 1));
            this._question_db.Add(new ActivityMgr.Question("提升税收百分比是什么建筑", "账房", "主城", 1));
            this._question_db.Add(new ActivityMgr.Question("游戏中总共有几种阵形", "8种", "10种", 1));
            this._question_db.Add(new ActivityMgr.Question("张飞的武器是", "丈八蛇矛", "雌雄双股剑", 1));
            this._question_db.Add(new ActivityMgr.Question("刘备托孤是在哪", "成都", "白帝城", 2));
            this._question_db.Add(new ActivityMgr.Question("蜀中无大将，_____为先锋", "廖化", "关平", 1));
            this._question_db.Add(new ActivityMgr.Question("提升税收百分比是什么建筑", "账房", "主城", 1));
            this._question_db.Add(new ActivityMgr.Question("可以用_____训练武将", "军令", "突飞令", 2));
            this._question_db.Add(new ActivityMgr.Question("官渡之战的胜利者是？", "袁绍", "曹操", 2));
            this._question_db.Add(new ActivityMgr.Question("秦始皇是____", "嬴政", "异人", 1));
            this._question_db.Add(new ActivityMgr.Question("败走麦城的是谁", "关羽", "张飞", 1));
            this._question_db.Add(new ActivityMgr.Question("刘关张是在哪里桃园结义的？", "涿县", "长安", 1));
            this._question_db.Add(new ActivityMgr.Question("汉高祖是____", "刘备", "刘邦", 2));
            this._question_db.Add(new ActivityMgr.Question("下列谁不是五虎上将", "魏延", "黄忠", 1));
            this._question_db.Add(new ActivityMgr.Question("诸葛亮出山时是多少岁", "37岁", "27岁", 2));
            this._question_db.Add(new ActivityMgr.Question("军机处可以用来", "升级科技", "征兵", 1));
            this._question_db.Add(new ActivityMgr.Question("军令多久恢复一个？", "半个小时", "一个小时", 2));
            this._question_db.Add(new ActivityMgr.Question("曹操字____", "孟德", "玄德", 1));
            this._question_db.Add(new ActivityMgr.Question("提升征收时获取金币的概率是什么建筑", "主城", "铸币厂", 2));
            this._question_db.Add(new ActivityMgr.Question("祭祀神农能获得____", "金币", "银币", 2));
            this._question_db.Add(new ActivityMgr.Question("项庄舞剑，意在____", "沛公", "刘备", 1));
            this._question_db.Add(new ActivityMgr.Question("威震逍遥津的是魏国哪位武将", "于禁", "张辽", 2));
        }

        public int handleFestivalEventInfo(ProtocolMgr protocol, ILogger logger)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            string url = "/root/festaval!getFestavalNianHuoNew.action";
            ServerResult xml = protocol.getXml(url, "获取迎军活动信息");
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
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/nianhuoevent/fete");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    else
                    {
                        num = 0;
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/nianhuoevent/impose");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    else
                    {
                        num2 = 0;
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/nianhuoevent/attack");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out num3);
                    }
                    else
                    {
                        num3 = 0;
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/nianhuoevent/feteneed");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        int.TryParse(xmlNode4.InnerText, out num4);
                    }
                    else
                    {
                        num4 = 0;
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/nianhuoevent/imposeneed");
                    bool flag7 = xmlNode5 != null;
                    if (flag7)
                    {
                        int.TryParse(xmlNode5.InnerText, out num5);
                    }
                    else
                    {
                        num5 = 0;
                    }
                    XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/nianhuoevent/attackneed");
                    bool flag8 = xmlNode6 != null;
                    if (flag8)
                    {
                        int.TryParse(xmlNode6.InnerText, out num6);
                    }
                    else
                    {
                        num6 = 0;
                    }
                    int num7 = 100;
                    while (num >= num4 && num2 >= num5 && num3 >= num6 && num7 > 0)
                    {
                        int num8 = num7;
                        num7 = num8 - 1;
                        this.deliverNianHuo(protocol, logger, ref num, ref num2, ref num3);
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int deliverNianHuo(ProtocolMgr protocol, ILogger logger, ref int _fete, ref int _impose, ref int _attack)
        {
            string url = "/root/festaval!deliverNianHuoNew.action";
            ServerResult xml = protocol.getXml(url, "上交年货");
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
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/nianhuoevent/fete");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out _fete);
                    }
                    else
                    {
                        _fete = 0;
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/nianhuoevent/impose");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out _impose);
                    }
                    else
                    {
                        _impose = 0;
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/nianhuoevent/attack");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out _attack);
                    }
                    else
                    {
                        _attack = 0;
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/reward");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        int num = 0;
                        int num2 = 0;
                        XmlNode xmlNode5 = xmlNode4.SelectSingleNode("baoshi");
                        bool flag7 = xmlNode5 != null;
                        if (flag7)
                        {
                            int.TryParse(xmlNode5.InnerText, out num);
                        }
                        XmlNode xmlNode6 = xmlNode4.SelectSingleNode("bowlder");
                        bool flag8 = xmlNode6 != null;
                        if (flag8)
                        {
                            int.TryParse(xmlNode6.InnerText, out num2);
                        }
                        base.logInfo(logger, string.Format("上交年货成功, 宝石+{0}, 玉石+{1}", num, num2));
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int eatNianHuo(ProtocolMgr protocol, ILogger logger, int nianhuoId)
        {
            string url = "/root/festaval!eatNianhuoNew.action";
            string data = "nianhuoId=" + nianhuoId;
            ServerResult serverResult = protocol.postXml(url, data, "吃掉年货");
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool cmdSucceed = serverResult.CmdSucceed;
                if (cmdSucceed)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    base.logInfo(logger, string.Format("吃掉一个年货, 行动力+3", new object[0]));
                    result = 0;
                }
                else
                {
                    bool flag2 = serverResult.CmdError.IndexOf("行动力") >= 0;
                    if (flag2)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 3;
                    }
                }
            }
            return result;
        }

        public int handleFestivalEventVisitInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/festaval!getFeatavalVisitInfoNew.action";
            ServerResult xml = protocol.getXml(url, "获取新年拜年活动信息");
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
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/visitinfo/hongbao");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    else
                    {
                        num = 0;
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/visitinfo/baozhu");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    else
                    {
                        num2 = 0;
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/visitinfo/randomvist");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out num3);
                    }
                    else
                    {
                        num3 = 0;
                    }
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/officerinfos/officer");
                    foreach (XmlNode xmlNode4 in xmlNodeList)
                    {
                        string pid = "";
                        string pname = "";
                        bool flag6 = false;
                        XmlNodeList childNodes = xmlNode4.ChildNodes;
                        foreach (XmlNode xmlNode5 in childNodes)
                        {
                            bool flag7 = xmlNode5.Name == "playerid";
                            if (flag7)
                            {
                                pid = xmlNode5.InnerText;
                            }
                            else
                            {
                                bool flag8 = xmlNode5.Name == "playername";
                                if (flag8)
                                {
                                    pname = xmlNode5.InnerText;
                                }
                                else
                                {
                                    bool flag9 = xmlNode5.Name == "visitornot";
                                    if (flag9)
                                    {
                                        flag6 = (xmlNode5.InnerText == "1");
                                    }
                                }
                            }
                        }
                        bool flag10 = !flag6;
                        if (flag10)
                        {
                            this.visitOfficer(protocol, logger, ref num, ref num2, pid, pname);
                        }
                    }
                    int num4 = 30;
                    while (num3 > 0 && num4 > 0)
                    {
                        int num5 = num4;
                        num4 = num5 - 1;
                        this.visitOfficer(protocol, logger, ref num, ref num2, "%2D100", "");
                        num5 = num3;
                        num3 = num5 - 1;
                    }
                    num4 = 30;
                    while (num > 0 && num4 > 0)
                    {
                        int num5 = num4;
                        num4 = num5 - 1;
                        this.openFestavalGift(protocol, logger, 1);
                        num5 = num;
                        num = num5 - 1;
                    }
                    num4 = 30;
                    while (num2 > 0 && num4 > 0)
                    {
                        int num5 = num4;
                        num4 = num5 - 1;
                        this.openFestavalGift(protocol, logger, 2);
                        num5 = num2;
                        num2 = num5 - 1;
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int visitOfficer(ProtocolMgr protocol, ILogger logger, ref int _visitHongbao, ref int _visitBaozhu, string pid, string pname = "")
        {
            string url = "/root/festaval!festavalVisitNew.action";
            string data = string.Format("toPid={0}&gold=0", pid);
            ServerResult serverResult = protocol.postXml(url, data, "拜年");
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
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/hongbao");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out _visitHongbao);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/baozhu");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out _visitBaozhu);
                    }
                    string arg = "";
                    string arg2 = "";
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/visitname");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        pname = xmlNode3.InnerText;
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/reward/visithongbao");
                    bool flag6 = xmlNode4 != null;
                    if (flag6)
                    {
                        arg = xmlNode4.InnerText;
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/reward/visitbaozhu");
                    bool flag7 = xmlNode5 != null;
                    if (flag7)
                    {
                        arg2 = xmlNode5.InnerText;
                    }
                    base.logInfo(logger, string.Format("向[{0}]拜年成功, 获得红包+{1}, 爆竹+{2}", pname, arg, arg2));
                    result = 0;
                }
            }
            return result;
        }

        public int openFestavalGift(ProtocolMgr protocol, ILogger logger, int type)
        {
            string url = "/root/festaval!openFestavalGiftNew.action";
            string data = string.Format("type={0}", type);
            string text = (type == 1) ? "打开拜年红包" : "燃放爆竹";
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
                    XmlNodeList childNodes = cmdResult.SelectSingleNode("/results").ChildNodes;
                    text += ", 获得";
                    foreach (XmlNode xmlNode in childNodes)
                    {
                        bool flag3 = xmlNode.Name == "ticket";
                        if (flag3)
                        {
                            text = text + " 点券+" + xmlNode.InnerText;
                        }
                        else
                        {
                            bool flag4 = xmlNode.Name == "hongbao";
                            if (flag4)
                            {
                                text = text + " 红包+" + xmlNode.InnerText;
                            }
                            else
                            {
                                bool flag5 = xmlNode.Name == "baoshi";
                                if (flag5)
                                {
                                    text = text + " 宝石+" + xmlNode.InnerText;
                                }
                                else
                                {
                                    bool flag6 = xmlNode.Name == "bowlder";
                                    if (flag6)
                                    {
                                        text = text + " 玉石+" + xmlNode.InnerText;
                                    }
                                }
                            }
                        }
                    }
                    base.logInfo(logger, text);
                    result = 0;
                }
            }
            return result;
        }

        public int handleTroopFeedbackInfo(ProtocolMgr protocol, ILogger logger, ref int gold_available, bool refine_notired, bool doubleweapon, bool openbox, int open_box_type)
        {
            string url = "/root/troopEquip!getTroopFeedBackInfo.action";
            string desc = "获取兵器回馈信息";
            ServerResult xml = protocol.getXml(url, desc);
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
                int cost = 10;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out cost);
                }
                int boxnum = 0;
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/boxnum");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out boxnum);
                }
                int curgetnum = 0;
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/curgetnum");
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out curgetnum);
                }
                if (openbox)
                {
                    for (int i = 0; i < boxnum; i++)
                    {
                        if (open_box_type == 0)
                        {
                            this.openTroopFeedbackBox(protocol, logger, false);
                        }
                        else
                        {
                            if (gold_available >= cost)
                            {
                                if (this.openTroopFeedbackBox(protocol, logger, true) == 0)
                                {
                                    gold_available -= cost;
                                }
                            }
                            else
                            {
                                this.openTroopFeedbackBox(protocol, logger, false);
                            }
                        }
                    }
                }
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/troopfeedback/status");
                int[] status_array = new int[5];
                int idx = 0;
                foreach (XmlNode xmlNode4 in xmlNodeList)
                {
                    int status = 0;
                    int.TryParse(xmlNode4.InnerText, out status);
                    status_array[idx] = status;
                    if (status == 1)
                    {
                        this.getTroopFeedbackBox(protocol, logger, idx + 1);
                    }
                    idx++;
                }
                //this.getTroopFeedbackBox(protocol, logger, 0);
                bool marketeffect = false;
                bool refineeffect = false;
                bool battleeffect = false;
                int refinecost = 20;
                int battlecost = 100;
                XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/marketeffect");
                if (xmlNode5 != null)
                {
                    marketeffect = (xmlNode5.InnerText == "1");
                }
                XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/refineeffect");
                if (xmlNode6 != null)
                {
                    refineeffect = (xmlNode6.InnerText == "1");
                }
                XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/battleeffect");
                if (xmlNode7 != null)
                {
                    battleeffect = (xmlNode7.InnerText == "1");
                }
                XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/refinecost");
                if (xmlNode8 != null)
                {
                    int.TryParse(xmlNode8.InnerText, out refinecost);
                }
                XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/battlecost");
                if (xmlNode9 != null)
                {
                    int.TryParse(xmlNode9.InnerText, out battlecost);
                }
                bool open_refine = true;
                bool open_battle = true;
                if (!marketeffect)
                {
                    this.enableTroopFeedbackFunction(protocol, logger, 3);
                }
                if (refine_notired && !refineeffect && refinecost <= gold_available)
                {
                    if (this.enableTroopFeedbackFunction(protocol, logger, 2) == 0)
                    {
                        gold_available -= refinecost;
                    }
                    else
                    {
                        open_refine = false;
                    }
                }
                if (doubleweapon && !battleeffect && battlecost <= gold_available)
                {
                    if (this.enableTroopFeedbackFunction(protocol, logger, 1) == 0)
                    {
                        gold_available -= battlecost;
                    }
                    else
                    {
                        open_battle = false;
                    }
                }
                if (!open_refine || !open_battle)
                {
                    result = 2;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public int enableTroopFeedbackFunction(ProtocolMgr protocol, ILogger logger, int type)
        {
            string url = "/root/troopEquip!getTroopFeedBackAch.action";
            string text = "兵器回馈功能开启";
            string data = "storeId=" + type;
            ServerResult serverResult = protocol.postXml(url, data, text);
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, text);
                result = 0;
            }
            return result;
        }

        public int getTroopFeedbackBox(ProtocolMgr protocol, ILogger logger, int awardId)
        {
            string url = "/root/troopEquip!recvTroopFeedBackById.action";
            string text = string.Format("获取兵器回馈宝箱(阶段{0})", awardId);
            string data = "storeId=" + awardId;
            ServerResult serverResult = protocol.postXml(url, data, text);
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int num = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/thisboxnum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                text += string.Format(", 奖励箱子+{0}", num);
                base.logInfo(logger, text);
                result = 0;
            }
            return result;
        }

        public int openTroopFeedbackBox(ProtocolMgr protocol, ILogger logger, bool useGold)
        {
            string url = "/root/troopEquip!openTroopFeedBackBox.action";
            string text = string.Format("{0}兵器回馈宝箱", useGold ? "四倍开启" : "免费开启");
            string data = "gold=0";
            if (useGold)
            {
                data = "gold=1";
            }
            ServerResult serverResult = protocol.postXml(url, data, text);
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int chipnum = 5;
                int cri = 1;
                string goodsname = "";
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/chipnum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out chipnum);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cri");
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out cri);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/goodsname");
                if (xmlNode3 != null)
                {
                    goodsname = xmlNode3.InnerText;
                }
                if (cri > 1)
                {
                    text += string.Format(", {0}倍暴击", cri);
                }
                text += string.Format("{0}{1}", goodsname, chipnum);
                base.logInfo(logger, text);
                result = 0;
            }
            return result;
        }

        public int handleTowerEventInfo(ProtocolMgr protocol, ILogger logger, int tower_type)
        {
            string url = "/root/festaval!getTowerEventInfo.action";
            string desc = "获取宝塔活动信息";
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
                    bool flag3 = tower_type < 1 || tower_type > 4;
                    if (flag3)
                    {
                        tower_type = 1;
                    }
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/curstate");
                    bool flag4 = xmlNode != null;
                    if (flag4)
                    {
                        string innerText = xmlNode.InnerText;
                        int num = 0;
                        int num2 = -1;
                        XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/towerbaoshi/baoshi");
                        bool flag5 = xmlNode2 != null;
                        if (flag5)
                        {
                            int.TryParse(xmlNode2.InnerText, out num);
                        }
                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/curbaoshi");
                        bool flag6 = xmlNode3 != null;
                        if (flag6)
                        {
                            int.TryParse(xmlNode3.InnerText, out num2);
                        }
                        bool flag7 = num2 >= num && innerText == "0" && this.getTowerEventReward(protocol, logger) == 0;
                        if (flag7)
                        {
                            result = 2;
                            return result;
                        }
                    }
                    else
                    {
                        bool flag8 = cmdResult.SelectSingleNode("/results/curtowerid") == null;
                        if (flag8)
                        {
                            this.selectTowerEventTower(protocol, logger, tower_type);
                        }
                    }
                    result = 0;
                }
            }
            return result;
        }

        private int selectTowerEventTower(ProtocolMgr protocol, ILogger logger, int tower_type)
        {
            string url = "/root/festaval!acceptByTowerId.action";
            string[] array = new string[]
			{
				"青玉百灵塔(1万)",
				"黄玉玲珑塔(3万)",
				"墨月麒麟塔(6万)",
				"紫灵至尊塔(12万)"
			};
            string text = string.Format("选择宝塔:{0}", array[tower_type - 1]);
            string data = "towerId=" + tower_type;
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

        private int getTowerEventReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/festaval!finishTower.action";
            string text = "获取宝塔奖励";
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
                    int num = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    base.logInfo(logger, string.Format("{0}, 宝石+{1}", text, num));
                    result = 0;
                }
            }
            return result;
        }

        public int handleTroopTurntableInfo(ProtocolMgr protocol, ILogger logger, string make_type, bool gold_ratio, int gold_available, int make_gold, int outside_num)
        {
            string url = "/root/troopTurntableEvent!getTroopTurntableEventInfo.action";
            string desc = "获取兵器转盘活动信息";
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
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/troop");
                    int num = 0;
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        XmlNode xmlNode2 = xmlNode.SelectSingleNode("name");
                        bool flag3 = xmlNode2.InnerText == make_type;
                        if (flag3)
                        {
                            num = int.Parse(xmlNode.SelectSingleNode("troopid").InnerText);
                            break;
                        }
                    }
                    bool flag4 = num == 0;
                    if (flag4)
                    {
                        base.logInfo(logger, "未找到兵器转盘要获取的兵器");
                        result = 2;
                    }
                    else
                    {
                        int num2 = 0;
                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/freetimes");
                        bool flag5 = xmlNode3 != null;
                        if (flag5)
                        {
                            int.TryParse(xmlNode3.InnerText, out num2);
                        }
                        int num3 = 0;
                        XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/goldcost");
                        bool flag6 = xmlNode4 != null;
                        if (flag6)
                        {
                            int.TryParse(xmlNode4.InnerText, out num3);
                        }
                        base.logInfo(logger, string.Format("兵器转盘还剩免费次数{0}次, 购买次数金币为{1}金币", num2, num3));
                        int num4 = 0;
                        XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/freedouble");
                        bool flag7 = xmlNode5 != null;
                        if (flag7)
                        {
                            int.TryParse(xmlNode5.InnerText, out num4);
                        }
                        int num5 = 20;
                        XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/goldmulti");
                        bool flag8 = xmlNode6 != null;
                        if (flag8)
                        {
                            int.TryParse(xmlNode6.InnerText, out num5);
                        }
                        int num6 = 0;
                        XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/hasoutside");
                        bool flag9 = xmlNode7 != null;
                        if (flag9)
                        {
                            int.TryParse(xmlNode7.InnerText, out num6);
                        }
                        int num7 = 0;
                        XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/outsidevalue");
                        bool flag10 = xmlNode8 != null;
                        if (flag10)
                        {
                            int.TryParse(xmlNode8.InnerText, out num7);
                        }
                        bool flag11 = num4 == 0 && gold_available < num5;
                        if (flag11)
                        {
                            result = 4;
                        }
                        else
                        {
                            bool flag12 = num6 == 0;
                            int num8;
                            if (flag12)
                            {
                                bool flag13 = num2 == 0 && num3 > make_gold;
                                if (flag13)
                                {
                                    result = 4;
                                    return result;
                                }
                                num8 = this.troopTurntable_doevent(protocol, logger, num, make_type, 0, 1);
                            }
                            else
                            {
                                bool flag14 = num4 > 0 && num7 >= outside_num;
                                if (flag14)
                                {
                                    base.logInfo(logger, string.Format("兵器转盘外圈个数{0},大于{1}，使用免费双倍次数", num7, outside_num));
                                    num8 = this.troopTurntable_doevent(protocol, logger, num, make_type, 1, 0);
                                }
                                else
                                {
                                    num8 = this.troopTurntable_doevent(protocol, logger, num, make_type, 0, 0);
                                }
                            }
                            bool flag15 = num8 != 0;
                            if (flag15)
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

        public int troopTurntable_goldenreset(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/troopTurntableEvent!goldReset.action";
            ServerResult xml = protocol.getXml(url, "兵器转盘提升倍率");
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
                    base.logInfo(logger, "兵器转盘提升倍率");
                    result = 0;
                }
            }
            return result;
        }

        public int troopTurntable_doevent(ProtocolMgr protocol, ILogger logger, int troopId, string troopName, int isgold, int isout)
        {
            string url = "/root/troopTurntableEvent!doTroopTurntableEvent.action";
            string data = string.Concat(new object[]
			{
				"troopId=",
				troopId,
				"&isGold=",
				isgold
			});
            ServerResult serverResult = protocol.postXml(url, data, "兵器转盘获取兵器");
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
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/troopcount");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/insidevalue");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/outsidevalue");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out num3);
                    }
                    bool flag6 = isout == 0;
                    if (flag6)
                    {
                        base.logInfo(logger, string.Format("兵器转盘获取兵器, 内圈倍数[{0}], 外圈个数[{1}], {2}+{3}", new object[]
						{
							num2,
							num3,
							troopName,
							num
						}));
                    }
                    result = 0;
                }
            }
            return result;
        }

        public int handleCakeEventInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/cakeEvent!getCakeEventInfo.action";
            string desc = "获取月饼活动信息";
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
                    int num = 0;
                    int num2 = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/boxnum");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cakenum");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    bool flag5 = num2 == 0 && num == 0;
                    if (flag5)
                    {
                        result = 2;
                    }
                    else
                    {
                        bool flag6 = num2 > 0;
                        if (flag6)
                        {
                            this.cake_useCake(protocol, logger, ref num, ref num2);
                        }
                        bool flag7 = num > 0;
                        if (flag7)
                        {
                            this.cake_openCakeBox(protocol, logger);
                        }
                        result = 0;
                    }
                }
            }
            return result;
        }

        public int cake_useCake(ProtocolMgr protocol, ILogger logger, ref int boxnum, ref int cakenum)
        {
            string url = "/root/cakeEvent!useCake.action";
            string desc = "消耗月饼";
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
                    int num = 0;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/addboxnum");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/boxnum");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out boxnum);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/cakenum");
                    bool flag5 = xmlNode3 != null;
                    if (flag5)
                    {
                        int.TryParse(xmlNode3.InnerText, out cakenum);
                    }
                    base.logInfo(logger, string.Format("消耗月饼砸月兽, 礼盒+{0}, 目前礼盒数{1}, 剩余月饼数{2}", num, boxnum, cakenum));
                    result = 0;
                }
            }
            return result;
        }

        public int cake_openCakeBox(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/cakeEvent!openCakeBox.action";
            string desc = "打开月饼礼盒";
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
                    int num = 0;
                    int num2 = 0;
                    string arg = "宝石";
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/rewardinfo/reward/num");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/rewardinfo/reward/type");
                    bool flag4 = xmlNode2 != null;
                    if (flag4)
                    {
                        int.TryParse(xmlNode2.InnerText, out num2);
                    }
                    bool flag5 = num2 != 5;
                    if (flag5)
                    {
                        arg = string.Format("[type={0}]", num2);
                    }
                    base.logInfo(logger, string.Format("打开月饼礼盒, {0}+{1}", arg, num));
                    result = 0;
                }
            }
            return result;
        }

        public int gemDump_handleInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/dumpEvent!getDetail.action";
            string desc = "获取宝石倾销信息";
            ServerResult xml = protocol.getXml(url, desc);
            int result;
            if (xml == null)
            {
                result = 1;
            }
            else if (!xml.CmdSucceed)
            {
                user.removeActivity(ActivityType.GemDump);
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                int totalbags = 0;
                int numleft = 0;
                int goldenough = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/totalbags");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out totalbags);
                }
                this.gemDump_goldEnough(cmdResult, out numleft, out goldenough);
                if (numleft > 0 && goldenough == 1)
                {
                    int num3 = 30;
                    int num4 = this.gemDump_buyGem(protocol, logger, 1, out totalbags);
                    while (num4 == 0 && num3 > 0)
                    {
                        num3--;
                        num4 = this.gemDump_buyGem(protocol, logger, 1, out totalbags);
                    }
                    result = num4;
                }
                else if (numleft > 0)
                {
                    result = 2;
                }
                else
                {
                    result = 3;
                }
                while (totalbags > 0)
                {
                    this.gemDump_openJingNang(protocol, logger, out totalbags);
                }
            }
            return result;
        }

        private void gemDump_goldEnough(XmlDocument xml, out int numleft, out int goldenough)
        {
            numleft = 0;
            goldenough = 0;
            XmlNodeList xmlNodeList = xml.SelectNodes("/results/goodlist/good");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.SelectSingleNode("baoshilevel") == null)
                {
                    break;
                }
                XmlNode xmlNode2 = xmlNode.SelectSingleNode("id");
                if (xmlNode2 != null && !(xmlNode2.InnerText != "1"))
                {
                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("numleft");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out numleft);
                    }
                    XmlNode xmlNode4 = xmlNode.SelectSingleNode("goldenough");
                    if (xmlNode4 != null)
                    {
                        int.TryParse(xmlNode4.InnerText, out goldenough);
                        break;
                    }
                    break;
                }
            }
        }

        private int gemDump_buyGem(ProtocolMgr protocol, ILogger logger, int index, out int totalbags)
        {
            totalbags = 0;
            string url = "/root/dumpEvent!buy.action";
            string data = "id=" + index;
            string text = "点券购买宝石倾销, ";
            ServerResult serverResult = protocol.postXml(url, data, text);
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/baoshi");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    int baoshilevel = 0;
                    int baoshinum = 0;
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("baoshilevel");
                    if (xmlNode2 != null)
                    {
                        int.TryParse(xmlNode2.InnerText, out baoshilevel);
                    }
                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("baoshinum");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out baoshinum);
                    }
                    if (baoshilevel > 0 && baoshinum > 0)
                    {
                        text += string.Format("[{0}级宝石+{1}] ", baoshilevel, baoshinum);
                    }
                }
                int extrabags = 0;
                XmlNode xmlNodeBag = cmdResult.SelectSingleNode("/results/bags/num");
                if (xmlNodeBag != null)
                {
                    int.TryParse(xmlNodeBag.InnerText, out extrabags);
                }
                XmlNode xmlNodeTotalBags = cmdResult.SelectSingleNode("/results/totalbags");
                if (xmlNodeTotalBags != null)
                {
                    int.TryParse(xmlNodeTotalBags.InnerText, out totalbags);
                }
                text += string.Format("[锦囊+{0} 总锦囊{1}个]", extrabags, totalbags);
                base.logInfo(logger, text);
                int numleft = 0;
                int goldenough = 0;
                this.gemDump_goldEnough(cmdResult, out numleft, out goldenough);
                if (numleft > 0 && goldenough == 1)
                {
                    result = 0;
                }
                else if (numleft > 0 && goldenough != 1)
                {
                    result = 2;
                }
                else if (numleft == 0)
                {
                    result = 3;
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }

        private void gemDump_openJingNang(ProtocolMgr protocol, ILogger logger, out int totalbags)
        {
            totalbags = 0;
            string url = "/root/dumpEvent!openBags.action";
            string desc = "开启锦囊, ";
            string data = "num=1";
            ServerResult xml = protocol.postXml(url, data, desc);
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                int baoshi = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi/num");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out baoshi);
                }
                if (baoshi > 0)
                {
                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("/results/totalbags");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out totalbags);
                    }
                    desc += string.Format("宝石+{0}, 剩余锦囊{1}个", baoshi, totalbags);
                    base.logInfo(logger, desc);
                }
            }
        }

        public int Dump_handleInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/server!get51EventTime.action";
            string desc = "获取兵器活动信息";
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
                    int num = 0;
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/good/id");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        int.TryParse(xmlNode.InnerText, out num);
                    }
                    bool flag4 = num == 1;
                    if (flag4)
                    {
                        int num2 = 0;
                        xmlNode = cmdResult.SelectSingleNode("/results/good/numleft");
                        bool flag5 = xmlNode != null;
                        if (flag5)
                        {
                            int.TryParse(xmlNode.InnerText, out num2);
                        }
                        int num3;
                        for (int i = 0; i < num2; i = num3 + 1)
                        {
                            this.Dump_buyGem(protocol, logger);
                            num3 = i;
                        }
                    }
                    result = 3;
                }
            }
            return result;
        }

        private int Dump_buyGem(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/dumpEvent!buyBowlderDump.action";
            string data = "id=1";
            string desc = "点券购买玉石";
            ServerResult serverResult = protocol.postXml(url, data, desc);
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
                    base.logInfo(logger, "兵器活动：花费500点券购买3000玉石");
                    result = 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 终极探宝
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="open_box_type"></param>
        /// <param name="shake_tree"></param>
        /// <param name="gem_price"></param>
        /// <param name="gold_available"></param>
        /// <returns></returns>
        public int handleNewTreasureGameInfo(ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            string url = "/root/dayTreasureGame!getNewTreasureGameInfo.action";
            ServerResult xml = protocol.getXml(url, "获取终极探宝信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int dicenum = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/dicenum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out dicenum);
            }
            base.logInfo(logger, string.Format("剩余骰子: {0}", dicenum));
            if (dicenum == 0)
            {
                return 10;
            }
            return this.startNewTGame(protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
        }

        /// <summary>
        /// 进入探宝
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="open_box_type"></param>
        /// <param name="shake_tree"></param>
        /// <param name="gem_price"></param>
        /// <param name="gold_available"></param>
        /// <returns></returns>
        private int startNewTGame(ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            string url = "/root/dayTreasureGame!startNewTGame.action";
            ServerResult xml = protocol.getXml(url, "进入终极探宝");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int dicenum = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/dicenum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out dicenum);
            }
            if (dicenum == 0)
            {
                return 10;
            }
            return this.useNewTDice(protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
        }

        /// <summary>
        /// 掷骰子
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="open_box_type"></param>
        /// <param name="shake_tree"></param>
        /// <param name="gem_price"></param>
        /// <param name="gold_available"></param>
        /// <returns></returns>
        private int useNewTDice(ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            string url = "/root/dayTreasureGame!useNewTDice.action";
            ServerResult xml = protocol.getXml(url, "免费掷骰");
            if (xml == null || !xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int movenum = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/movenum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out movenum);
            }
            base.logInfo(logger, string.Format("掷骰子点数: {0}", movenum));
            return handleNewTMoveAction(cmdResult, protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
        }

        private int handleNewTMoveAction(XmlDocument cmdResult, ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            int changemap = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/changemap");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out changemap);
            }
            int needfinish = 0;
            xmlNode = cmdResult.SelectSingleNode("/results/needfinish");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out needfinish);
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/pointreward");
            foreach (XmlNode childXmlNode in xmlNodeList)
            {
                int pos = 0;
                xmlNode = childXmlNode.SelectSingleNode("pos");
                if (xmlNode != null)
                {
                    pos = int.Parse(xmlNode.InnerText);
                }
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(childXmlNode.SelectSingleNode("rewardinfo"));
                base.logInfo(logger, string.Format("移动到[{0}] {1}", pos, reward.ToString()));
                xmlNode = childXmlNode.SelectSingleNode("eventtype");
                if (xmlNode != null)
                {
                    int eventtype = int.Parse(xmlNode.InnerText);
                    switch (eventtype)
                    {
                        case 1://探索路径
                            {
                                base.logInfo(logger, "遇到[探索路径]事件");
                                return handleNewTEvent1(protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
                            }
                        case 2://摇钱树
                            {
                                base.logInfo(logger, "遇到[摇钱树]事件");
                                int goldshakecost = 0;
                                XmlNode xmlNode2 = childXmlNode.SelectSingleNode("goldshakecost");
                                if (xmlNode2 != null)
                                {
                                    int.TryParse(xmlNode2.InnerText, out goldshakecost);
                                }
                                RewardInfo nextbaoshireward = new RewardInfo();
                                nextbaoshireward.handleXmlNode(childXmlNode.SelectSingleNode("nextbaoshi/rewardinfo"));
                                return handleNewTEvent2(goldshakecost, nextbaoshireward.getReward(0), protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
                            }
                        case 3://购买铁锤
                            {
                                base.logInfo(logger, "遇到[购买铁锤]事件");
                                int goldboxcost = 0;
                                XmlNode xmlNode2 = childXmlNode.SelectSingleNode("goldboxcost");
                                if (xmlNode2 != null)
                                {
                                    int.TryParse(xmlNode2.InnerText, out goldboxcost);
                                }
                                Hammer hammer = new Hammer();
                                xmlNode2 = childXmlNode.SelectSingleNode("rewardname");
                                hammer.handleXmlNode(xmlNode2);
                                if (xmlNode2 != null)
                                {
                                    base.logInfo(logger, xmlNode2.InnerText);
                                }
                                return handleNewTEvent3(goldboxcost, hammer, protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
                            }
                        default:
                            {
                                base.logInfo(logger, string.Format("遇到[未知{0}]事件", eventtype));
                                return 10;
                            }
                    }
                }
            }
            if (needfinish == 1)
            {
                string url = "/root/dayTreasureGame!awayNewTGame.action";
                ServerResult xml = protocol.getXml(url, "探索完成");
                if (xml == null || !xml.CmdSucceed)
                {
                    return 10;
                }
                base.logInfo(logger, "探索完成");
                return 0;
            }
            else if (changemap == 0)
            {
                return 0;
            }
            else
            {
                string url = "/root/dayTreasureGame!transfer.action";
                ServerResult xml = protocol.getXml(url, "跳转地图");
                if (xml == null || !xml.CmdSucceed)
                {
                    return 10;
                }
                base.logInfo(logger, "切换宝藏");
                return handleNewTMoveAction(xml.CmdResult, protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
            }
        }

        private int handleNewTEvent1(ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            string url = "/root/dayTreasureGame!handlerEvent.action";
            string data = "open=1";
            ServerResult xml = protocol.postXml(url, data, "探索路径");
            if (xml == null || !xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
            base.logInfo(logger, string.Format("探索路径 {0}", reward.ToString()));
            int footfail = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/footfail");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out footfail);
            }
            if (footfail == 1)
            {
                return handleNewTMoveAction(cmdResult, protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
            }
            return handleNewTEvent1(protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
        }

        private int handleNewTEvent2(int goldshakecost, Reward nextbaoshi, ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            string url = "/root/dayTreasureGame!handlerEvent.action";
            string data = "open=0";
            if (nextbaoshi != null)
            {
                int baoshi = GlobalConfig.getGemCount(nextbaoshi.Lv) * nextbaoshi.Num;
                double price = (double)goldshakecost * 1.0 / baoshi;
                if (price <= gem_price && goldshakecost <= gold_available)
                {
                    data = "open=1";
                    gold_available -= goldshakecost;
                }
            }
            ServerResult xml = protocol.postXml(url, data, "摇钱树");
            if (xml == null || !xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int close = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/close");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out close);
            }
            if (close == 1)
            {
                return handleNewTMoveAction(cmdResult, protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
            }
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
            base.logInfo(logger, string.Format("摇钱树 花费 {0} 金币 {1}", goldshakecost, reward.ToString()));
            xmlNode = cmdResult.SelectSingleNode("/results/goldshakecost");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out goldshakecost);
            }
            RewardInfo nextbaoshireward = new RewardInfo();
            nextbaoshireward.handleXmlNode(cmdResult.SelectSingleNode("/results/nextbaoshi/rewardinfo"));
            return handleNewTEvent2(goldshakecost, nextbaoshireward.getReward(0), protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
        }

        private int handleNewTEvent3(int goldboxcost, Hammer hammer, ProtocolMgr protocol, ILogger logger, User user, string open_box_type, bool shake_tree, double gem_price, ref int gold_available)
        {
            string url = "/root/dayTreasureGame!handlerEvent.action";
            string data = "open=0";
            if (hammer != null)
            {
                if (open_box_type.Contains(hammer.Cri.ToString()) && goldboxcost <= gold_available)
                {
                    data = "open=1";
                    gold_available -= goldboxcost;
                }
            }
            ServerResult xml = protocol.postXml(url, data, "购买铁锤");
            if (xml == null || !xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            if (data == "open=1")
            {
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
                base.logInfo(logger, string.Format("购买铁锤 花费 {0} 金币 {1}", goldboxcost, reward.ToString()));
            }
            return handleNewTMoveAction(cmdResult, protocol, logger, user, open_box_type, shake_tree, gem_price, ref gold_available);
        }

        #region 充值送红包
        public int getPayHongbaoEventInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!getPayHongbaoEventInfo.action";
            ServerResult xml = protocol.getXml(url, "获取充值送红包信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int hongbaonum = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/hongbaonum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out hongbaonum);
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/hongbaoinfo");
            foreach (XmlNode node in xmlNodeList)
            {
                if (node != null && node.HasChildNodes)
                {
                    int playerid = 0;
                    int rewardid = 0;
                    string playername = "";
                    foreach (XmlNode item in node)
                    {
                        if (item.Name == "playerid")
                        {
                            playerid = int.Parse(item.InnerText);
                        }
                        else if (item.Name == "rewardid")
                        {
                            rewardid = int.Parse(item.InnerText);
                        }
                        else if (item.Name == "playername")
                        {
                            playername = item.InnerText;
                        }
                    }
                    if (playerid > 0)
                    {
                        recvShareHongbao(protocol, logger, playerid, rewardid, playername, ref hongbaonum);
                    }
                }
            }
            while (hongbaonum > 0)
            {
                hongbaonum--;
                openPayHongbao(protocol, logger);
            }
            return 0;
        }

        public void recvShareHongbao(ProtocolMgr protocol, ILogger logger, int playerid, int rewardid, string playername, ref int hongbaonum)
        {
            string url = "/root/event!recvShareHongbao.action";
            string data = string.Format("rewardId={0}&playerId={1}", rewardid, playerid);
            ServerResult xml = protocol.postXml(url, data, "打开共享红包");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                int thishongbaonum = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/thishongbaonum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out thishongbaonum);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/hongbaonum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out hongbaonum);
                }
                logInfo(logger, string.Format("获得{0}的共享红包，获得{1}个红包，共{2}个红包", playername, thishongbaonum, hongbaonum));
            }
        }

        public void openPayHongbao(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!openPayHongbao.action";
            string data = "num=0";
            ServerResult xml = protocol.postXml(url, data, "打开红包");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(cmdResult.SelectSingleNode("/results/hongbaoreward/rewardinfo"));
                int hongbaonum = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/hongbaonum");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out hongbaonum);
                }
                logInfo(logger, string.Format("获得 {0} 剩余{1}个红包", reward.ToString(), hongbaonum));
            }
        }
        #endregion

        #region 大宴群雄
        public int getBGEventInfo(ProtocolMgr protocol, ILogger logger, ref int gold_available, int maxcoin)
        {
            string url = "/root/event!getBGEventInfo.action";
            ServerResult serverResult = protocol.getXml(url, "获取大宴信息");
            if (serverResult == null)
            {
                base.logInfo(logger, "获取大宴信息返回空值");
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                base.logInfo(logger, "大宴活动：" + serverResult.CmdError);
                if (serverResult.CmdError.Contains("不在活动期间"))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                int xiqi = 0;
                int maxxiqi = 0;
                int goldcost = 999;
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/xiqi");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out xiqi);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/maxxiqi");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out maxxiqi);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/goldcost");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out goldcost);
                }
                List<ProgressState> progressstate_list = XmlHelper.GetClassList<ProgressState>(cmdResult.SelectNodes("/results/progressstate"));
                if (progressstate_list.Count > 0)
                {
                    foreach (ProgressState current in progressstate_list)
                    {
                        this.getBanquetReward(protocol, logger, current);
                    }
                }
                if (xiqi >= maxxiqi)
                {
                    this.consumeXiqi(protocol, logger);
                }
                while (goldcost >= 0 && goldcost <= maxcoin)
                {
                    goldcost = this.doBGEvent(protocol, logger, ref gold_available, goldcost);
                }
                return 0;
            }
        }

        public int doBGEvent(ProtocolMgr protocol, ILogger logger, ref int gold_available, int coin)
        {
            string text;
            if (coin == 0)
            {
                text = "大宴：免费宴会1次";
            }
            else
            {
                text = string.Format("大宴：花费{0}金币宴会1次", coin);
            }
            string url = "/root/event!doBGEvent.action";
            ServerResult xml = protocol.getXml(url, "大宴宴会");
            if (xml == null || !xml.CmdSucceed)
            {
                return -1;
            }
            else
            {
                gold_available -= coin;
                int xiqi = 0;
                int maxxiqi = 0;
                int goldcost = 999;
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/xiqi");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out xiqi);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/maxxiqi");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out maxxiqi);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/goldcost");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out goldcost);
                }
                RewardInfo rewardInfo = new RewardInfo();
                rewardInfo.handleXmlNode(cmdResult.SelectSingleNode("/results/bginfo/rewardinfo"));
                base.logInfo(logger, string.Format("{0}, {1}", text, rewardInfo.ToString()));
                if (xiqi >= maxxiqi)
                {
                    this.consumeXiqi(protocol, logger);
                }
                List<ProgressState> progressstate_list = XmlHelper.GetClassList<ProgressState>(cmdResult.SelectNodes("/results/progressstate"));
                if (progressstate_list.Count > 0)
                {
                    foreach (ProgressState current in progressstate_list)
                    {
                        this.getBanquetReward(protocol, logger, current);
                    }
                }
                return goldcost;
            }
        }

        public void consumeXiqi(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!consumeXiqi.action";
            ServerResult xml = protocol.getXml(url, "大宴宴会消耗喜气");
            if (xml != null && xml.CmdSucceed)
            {
                RewardInfo rewardInfo = new RewardInfo();
                rewardInfo.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
                base.logInfo(logger, string.Format("消耗喜气, {0}", rewardInfo.ToString()));
            }
        }

        public void getBanquetReward(ProtocolMgr protocol, ILogger logger, ProgressState state)
        {
            string url = "/root/event!getBanquetReward.action";
            string data = string.Format("rewardId={0}", state.id);
            ServerResult xml = protocol.postXml(url, data, "大宴宴会开启宝箱");
            if (xml != null && xml.CmdSucceed)
            {
                RewardInfo rewardInfo = new RewardInfo();
                rewardInfo.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
                base.logInfo(logger, string.Format("开启宝箱, {0}", rewardInfo.ToString()));
            }
        }
        #endregion

        #region 对战
        public long getMatchDetail(ProtocolMgr protocol, ILogger logger, User user, int point, string ack_formation, string def_formation)
        {
            string url = "/root/kfrank!getMatchDetail.action";
            ServerResult xml = protocol.getXml(url, "获取对战信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return next_hour();
            }
            //logInfo(logger, "对战信息：" + xml.getDebugInfo());
            int canready = 0;
            int status = 0;
            int canget = 0;
            int boxnum = 0;
            int needtoken = 100;
            int havegetlast = 1;
            int nextbattlecd = 3000;
            int globalstate = 0;
            TaskInfo taskInfo = new TaskInfo();
            int score = -1;
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode node = cmdResult.SelectSingleNode("/results/message/canready");
            if (node != null)
            {
                int.TryParse(node.InnerText, out canready);
            }
            node = cmdResult.SelectSingleNode("/results/message/status");
            if (node != null)
            {
                int.TryParse(node.InnerText, out status);
            }
            node = cmdResult.SelectSingleNode("/results/message/boxinfo/canget");
            if (node != null)
            {
                int.TryParse(node.InnerText, out canget);
            }
            node = cmdResult.SelectSingleNode("/results/message/boxinfo/boxnum");
            if (node != null)
            {
                int.TryParse(node.InnerText, out boxnum);
            }
            node = cmdResult.SelectSingleNode("/results/message/needtoken");
            if (node != null)
            {
                int.TryParse(node.InnerText, out needtoken);
            }
            node = cmdResult.SelectSingleNode("/results/message/boxinfo/havegetlast");
            if (node != null)
            {
                int.TryParse(node.InnerText, out havegetlast);
            }
            node = cmdResult.SelectSingleNode("/results/message/nextbattlecd");
            if (node != null)
            {
                int.TryParse(node.InnerText, out nextbattlecd);
            }
            node = cmdResult.SelectSingleNode("/results/message/globalstate");
            if (node != null)
            {
                int.TryParse(node.InnerText, out globalstate);
            }
            node = cmdResult.SelectSingleNode("/results/message/taskinfo");
            if (node != null)
            {
                taskInfo.handleXmlNode(node);
            }
            XmlNodeList nodeList = cmdResult.SelectNodes("/results/message/selfrank/playerinfo");
            if (nodeList != null)
            {
                foreach (XmlNode childnode in nodeList)
                {
                    node = childnode.SelectSingleNode("self");
                    if (node != null)
                    {
                        node = childnode.SelectSingleNode("score");
                        if (node != null)
                        {
                            int.TryParse(node.InnerText, out score);
                        }
                        break;
                    }
                }
            }
            //有任务奖励
            if (taskInfo.state_ == 1)
            {
                recvTaskReward(protocol, logger);
            }
            //有上届排行奖励
            if (havegetlast == 0)
            {
                recvLastReward(protocol, logger);
            }
            //刷新任务
            for (int i = 0; i < 20 && !taskInfo.isLianShengChuanQi(); i++)
            {
                changeTask(protocol, logger, ref taskInfo);
            }
            //开宝箱
            for (int i = boxnum; i > 0; i--)
            {
                openBox(protocol, logger);
            }
            //准备就绪
            if (canready == 1)
            {
                string myformation = _factory.getBattleManager().getFormation(protocol, logger);
                string formation = ack_formation;
                if (canget == 0)
                {
                    formation = def_formation;
                }
                _factory.getBattleManager().changeFormation(protocol, logger, formation);
                syncData(protocol, logger);
                ready(protocol, logger);
                _factory.getBattleManager().changeFormation(protocol, logger, myformation);
                return nextbattlecd;
            }
            if (globalstate == 2)
            {
                return next_day_eight();
            }
            else if (status == 2)
            {
                return 60000;
            }
            else if (status == 1)
            {
                return 10000;
            }
            else
            {
                //每天对战5场，且对战军令消耗小于等于20
                if (canget == 1 && needtoken <= 20)
                {
                    if (user.Token >= needtoken)
                    {
                        startMatch(protocol, logger, needtoken, user.Token);
                        return 3000;
                    }
                    else
                    {
                        logInfo(logger, string.Format("军令不足，当前{0}军令，需要{1}军令", user.Token, needtoken));
                        return next_halfhour();
                    }
                }
            }
            //对战积分<=point，且对战军令消耗小于等于10
            if (score >= point && needtoken <= 10)
            {
                logger.logInfo(string.Format("乱世风云榜 - 我的积分:{0}", score));
                if (user.Token >= needtoken)
                {
                    startMatch(protocol, logger, needtoken, user.Token);
                    return 3000;
                }
                else
                {
                    logInfo(logger, string.Format("军令不足，当前{0}军令，需要{1}军令", user.Token, needtoken));
                    return next_halfhour();
                }
            }
            return next_day_eight();
        }

        public void startMatch(ProtocolMgr protocol, ILogger logger, int needtoken, int currtoken)
        {
            string url = "/root/kfrank!startMatch.action";
            ServerResult xml = protocol.getXml(url, "开始匹配对手");
            if (xml != null && xml.CmdSucceed)
            {
                logInfo(logger, string.Format("当前{0}军令，消耗{1}军令，开始匹配对手", currtoken, needtoken));
            }
        }

        public void ready(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfrank!ready.action";
            ServerResult xml = protocol.getXml(url, "准备就绪");
            if (xml != null && xml.CmdSucceed)
            {
                logInfo(logger, "准备就绪");
            }
        }

        public void openBox(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfrank!openBox.action";
            ServerResult xml = protocol.getXml(url, "打开对战宝箱");
            if (xml != null)
            {
                if (!xml.CmdSucceed)
                {
                    logInfo(logger, string.Format("打开对战宝箱：{0}", xml.CmdError));
                    return;
                }
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode node = cmdResult.SelectSingleNode("/results/message/rewardgeneral");
                if (node != null && node.HasChildNodes)
                {
                    string name = "";
                    int num = 0;
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "name")
                        {
                            name = node2.InnerText;
                        }
                        else if (node2.Name == "num")
                        {
                            int.TryParse(node2.InnerText, out num);
                        }
                    }
                    logInfo(logger, string.Format("打开对战宝箱，获得大将令{0}+{1}", name, num));
                }
                node = cmdResult.SelectSingleNode("/results/message/tickets/num");
                if (node != null)
                {
                    logInfo(logger, string.Format("打开对战宝箱，获得点券+{0}", node.InnerText));
                }
            }
        }

        public void recvTaskReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfrank!recvTaskReward.action";
            ServerResult xml = protocol.getXml(url, "领取对战任务奖励");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/boxreward");
                if (xmlNode != null)
                {
                    logInfo(logger, string.Format("领取对战任务奖励, 宝箱+{0}", xmlNode.InnerText));
                }
            }
        }

        public void recvLastReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfrank!recvLastReward.action";
            ServerResult xml = protocol.getXml(url, "领取对战上届排名奖励");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/boxreward");
                if (xmlNode != null)
                {
                    logInfo(logger, string.Format("领取对战上届排名奖励, 宝箱+{0}", xmlNode.InnerText));
                }
            }
        }

        public bool syncData(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/kfrank!syncData.action";
            ServerResult xml = protocol.getXml(url, "同步对战阵型");
            if (xml == null || !xml.CmdSucceed)
            {
                logInfo(logger, "同步对战阵型失败");
                return false;
            }
            else
            {
                logInfo(logger, "同步对战阵型成功");
                return true;
            }
        }

        public void changeTask(ProtocolMgr protocol, ILogger logger, ref TaskInfo taskInfo)
        {
            string url = "/root/kfrank!changeTask.action";
            ServerResult xml = protocol.getXml(url, "刷新对战任务");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNode node = cmdResult.SelectSingleNode("/results/message/taskinfo");
                if (node != null)
                {
                    taskInfo.handleXmlNode(node);
                    logInfo(logger, string.Format("刷新对战任务，获得新任务【{0} - {1}】", taskInfo.name_, taskInfo.intro_));
                }
            }
        }

        #endregion

        #region 清明煮酒
        public long getQingmingInfo(ProtocolMgr protocol, ILogger logger, int round_gold, int drink_gold, int gold_available)
        {
            string url = "/root/event!getQingmingInfo.action";
            ServerResult xml = protocol.getXml(url, "获取清明煮酒信息");
            if (xml == null)
            {
                return 300000;//5分钟
            }
            else if (!xml.CmdSucceed)
            {
                logInfo(logger, "清明煮酒: " + xml.CmdError);
                if (xml.CmdError.Contains("不在活动期间"))
                {
                    return next_day();
                }
                return 600000;//10分钟
            }
            XmlDocument cmdResult = xml.CmdResult;
            int winenum = 0;
            int maxnum = 100;
            int roundnum = 0;
            int buycost = 100;
            int golddrinkcost = 100;
            int havebigreward = 0;
            XmlNode node = cmdResult.SelectSingleNode("/results/winenum");
            if (node != null)
            {
                int.TryParse(node.InnerText, out winenum);
            }
            node = cmdResult.SelectSingleNode("/results/maxnum");
            if (node != null)
            {
                int.TryParse(node.InnerText, out maxnum);
            }
            node = cmdResult.SelectSingleNode("/results/roundnum");
            if (node != null)
            {
                int.TryParse(node.InnerText, out roundnum);
            }
            node = cmdResult.SelectSingleNode("/results/buycost");
            if (node != null)
            {
                int.TryParse(node.InnerText, out buycost);
            }
            node = cmdResult.SelectSingleNode("/results/golddrinkcost");
            if (node != null)
            {
                int.TryParse(node.InnerText, out golddrinkcost);
            }
            node = cmdResult.SelectSingleNode("/results/havebigreward");
            if (node != null)
            {
                int.TryParse(node.InnerText, out havebigreward);
            }
            if (havebigreward == 1)
            {
                getQingmingBigReward(protocol, logger);
                return immediate();
            }
            else if (roundnum == 0)
            {
                if (buycost <= round_gold)
                {
                    if (buycost <= gold_available)
                    {
                        buyQingmingRound(protocol, logger, buycost);
                        return immediate();
                    }
                    return next_hour();
                }
                return next_day();
            }
            bool can_gold_drink = false;
            if (golddrinkcost <= drink_gold)
            {
                can_gold_drink = true;
            }
            int generalnum = 0;
            XmlNodeList nodeList = cmdResult.SelectNodes("/results/generalinfo");
            foreach (XmlNode node2 in nodeList)
            {
                XmlNode node3 = node2.SelectSingleNode("state");
                if (node3 != null && node3.InnerText == "1")
                {
                    generalnum++;
                }
            }
            List<WineInfo> wineinfo = new List<WineInfo>();
            nodeList = cmdResult.SelectNodes("/results/wineinfo/wine");
            foreach (XmlNode node2 in nodeList)
            {
                WineInfo wine = new WineInfo();
                wine.fillValues(node2.ChildNodes);
                wineinfo.Add(wine);
            }
            qingmingDrink(protocol, logger, winenum, maxnum, generalnum, wineinfo, can_gold_drink, golddrinkcost);
            return immediate();
        }

        public void getQingmingBigReward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/event!getQingmingBigReward.action";
            ServerResult xml = protocol.getXml(url, "获取清明煮酒大礼");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                RewardInfo rewardinfo = new RewardInfo();
                rewardinfo.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
                logInfo(logger, string.Format("获取清明煮酒大礼, 获得{0}", rewardinfo.ToString()));
            }
        }

        public void buyQingmingRound(ProtocolMgr protocol, ILogger logger, int buycost)
        {
            string url = "/root/event!buyQingmingRound.action";
            ServerResult xml = protocol.getXml(url, "购买清明煮酒圈数");
            if (xml != null && xml.CmdSucceed)
            {
                logInfo(logger, string.Format("花费{0}金币购买清明煮酒圈数", buycost));
            }
        }

        public void qingmingDrink(ProtocolMgr protocol, ILogger logger, int winenum, int maxnum, int generalnum, List<WineInfo> wineinfo, bool can_gold_drink, int gold_drink)
        {
            int gold = 0;
            WineInfo selected_wine = null;
            if (can_gold_drink && generalnum < 7)
            {
                foreach (WineInfo wine in wineinfo)
                {
                    if (wine.winenum_ >= 40)
                    {
                        selected_wine = wine;
                        gold = 1;
                        break;
                    }
                }
            }
            if (selected_wine == null)
            {
                if (generalnum < 4)
                {
                    foreach (WineInfo wine in wineinfo)
                    {
                        if (selected_wine == null || selected_wine.winenum_ > wine.winenum_)
                        {
                            selected_wine = wine;
                        }
                    }
                }
                else if (generalnum < 5)
                {
                    foreach (WineInfo wine in wineinfo)
                    {
                        if (winenum + wine.winenum_ <= 55 && (selected_wine == null || selected_wine.winenum_ < wine.winenum_))
                        {
                            selected_wine = wine;
                        }
                    }
                    if (selected_wine == null)
                    {
                        foreach (WineInfo wine in wineinfo)
                        {
                            if (selected_wine == null || selected_wine.winenum_ > wine.winenum_)
                            {
                                selected_wine = wine;
                            }
                        }
                    }
                }
                else if (generalnum < 6)
                {
                    foreach (WineInfo wine in wineinfo)
                    {
                        if (winenum + wine.winenum_ <= 70 && (selected_wine == null || selected_wine.winenum_ < wine.winenum_))
                        {
                            selected_wine = wine;
                        }
                    }
                    if (selected_wine == null)
                    {
                        foreach (WineInfo wine in wineinfo)
                        {
                            if (selected_wine == null || selected_wine.winenum_ > wine.winenum_)
                            {
                                selected_wine = wine;
                            }
                        }
                    }
                }
                else if (generalnum < 7)
                {
                    foreach (WineInfo wine in wineinfo)
                    {
                        if (winenum + wine.winenum_ < maxnum && (selected_wine == null || selected_wine.winenum_ < wine.winenum_))
                        {
                            selected_wine = wine;
                        }
                    }
                    if (selected_wine == null)
                    {
                        foreach (WineInfo wine in wineinfo)
                        {
                            if (selected_wine == null || selected_wine.winenum_ < wine.winenum_)
                            {
                                selected_wine = wine;
                            }
                        }
                    }
                }
                else
                {
                    foreach (WineInfo wine in wineinfo)
                    {
                        if (selected_wine == null || selected_wine.winenum_ < wine.winenum_)
                        {
                            selected_wine = wine;
                        }
                    }
                }
            }
            string url = "/root/event!qingmingDrink.action";
            string data = string.Format("wineId={0}&gold={1}", selected_wine.id_, gold);
            ServerResult xml = protocol.postXml(url, data, "喝酒");
            if (xml != null && xml.CmdSucceed)
            {
                XmlDocument cmdResult = xml.CmdResult;
                RewardInfo rewardinfo = new RewardInfo();
                rewardinfo.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
                string text = "";
                if (gold == 1)
                {
                    text = string.Format(", 花费{0}金币酒仙附体", gold_drink);
                }
                string text2 = "";
                XmlNode node = cmdResult.SelectSingleNode("/results/winenum");
                if (node != null)
                {
                    int new_winenum = int.Parse(node.InnerText);
                    text2 = string.Format(", 醉意增加{0}, 当前醉意{1}/{2}", new_winenum - winenum, new_winenum, maxnum);
                }
                logger.log(string.Format("第{0}次喝酒{1}, 获得{2}{3}", generalnum + 1, text, rewardinfo.ToString(), text2), Color.Red);
            }
        }
        #endregion

        #region 雪地通商
        public int snowTradingGetSnowTradingInfo(ProtocolMgr protocol, ILogger logger, User user, int buyroundcostlimit, int goldavailable, bool isreinforce, int reinforcecostlimit)
        {
            if (user.isActivityRunning(ActivityType.SnowTradingEvent))
                return snowTrading_.getSnowTradingInfo(protocol, logger, user, buyroundcostlimit, goldavailable, isreinforce, reinforcecostlimit);
            else
                return 10;
        }
        #endregion

        #region 草船借箭
        public long borrowingArrowsExecute(ProtocolMgr protocol, ILogger logger, User user, int goldavailable, int buyboatcostlimit, bool calculatestream, int calculatestreamcostlimit, int costlimit, float percent)
        {
            return borrowingArrows_.execute(protocol, logger, user, goldavailable, buyboatcostlimit, calculatestream, calculatestreamcostlimit, costlimit, percent);
        }
        #endregion

        #region 端午
        public long ArrestEventExecute(ProtocolMgr protocol, ILogger logger, User user, int goldavailable, int hishengold, int ricedumplingcostgold, int arresttokencostgold)
        {
            return arrestEvent_.execute(protocol, logger, user, goldavailable, hishengold, ricedumplingcostgold, arresttokencostgold);
        }
        #endregion

        #region 新国庆阅兵
        public long ParadeEventExecute(ProtocolMgr protocol, ILogger logger, User user, int gold_available, int limit_cost, int limit_roundcost)
        {
            return paradeEvent_.execute(protocol, logger, user, gold_available, limit_cost, limit_roundcost);
        }
        #endregion

        #region 辞旧迎新
        public int getSpringFestivalWishInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/springFestivalWish!getSpringFestivalWishInfo.action";
            ServerResult xml = protocol.getXml(url, "许愿");
            if (xml == null || !xml.CmdSucceed)
            {
                return 10;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int haschoose = lua.GetIntValue("results.haschoose");
            //int nowevent = lua.GetIntValue("results.nowevent");
            //string wishstate = lua.GetStringValue("results.wishstate");
            //int cangetreward = lua.GetIntValue("results.cangetreward");
            int haschoose = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/haschoose"));
            int nowevent = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/nowevent"));
            string wishstate = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/wishstate"));
            int cangetreward = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/cangetreward"));
            if (nowevent == 1)
            {
                if (haschoose == 0)
                {
                    if (!hangInTheTree(protocol, logger, user))
                    {
                        return 10;
                    }
                    return 0;
                }
                else if (cangetreward == 0)
                {
                    if (!openCijiuReward(protocol, logger, user))
                    {
                        return 10;
                    }
                    return 0;
                }
            }
            else if (nowevent == 2)
            {
                if (cangetreward == 0)
                {
                    if (!openYinxingReward(protocol, logger, user))
                    {
                        return 10;
                    }
                    return 0;
                }
            }
            else if (nowevent == 3)
            {
                string[] wishs = wishstate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < wishs.Length; i++)
                {
                    if (wishs[i] == "0")
                    {
                        if (!receiveWishReward(protocol, logger, user, i + 1))
                        {
                            return 10;
                        }
                    }
                }
            }
            return 2;
        }

        public bool hangInTheTree(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/springFestivalWish!hangInTheTree.action";
            ServerResult xml = protocol.getXml(url, "开始许愿");
            if (xml != null && xml.CmdSucceed)
            {
                logInfo(logger, "许愿成功");
                return true;
            }
            return false;
        }

        public bool openCijiuReward(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/springFestivalWish!openCijiuReward.action";
            ServerResult xml = protocol.getXml(url, "领取辞旧奖励");
            if (xml != null && xml.CmdSucceed)
            {
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
                logInfo(logger, string.Format("辞旧，获得{0}", reward.ToString()));
                return true;
            }
            return false;
        }

        public bool openYinxingReward(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/springFestivalWish!openYinxingReward.action";
            ServerResult xml = protocol.getXml(url, "领取迎新奖励");
            if (xml != null && xml.CmdSucceed)
            {
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
                logInfo(logger, string.Format("迎新，获得{0}", reward.ToString()));
                return true;
            }
            return false;
        }

        public bool receiveWishReward(ProtocolMgr protocol, ILogger logger, User user, int id)
        {
            string url = "/root/springFestivalWish!receiveWishReward.action";
            string data = string.Format("id={0}", id);
            ServerResult xml = protocol.postXml(url, data, "领取愿望奖励");
            if (xml != null && xml.CmdSucceed)
            {
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
                logInfo(logger, string.Format("愿望，获得{0}", reward.ToString()));
                return true;
            }
            return false;
        }
        #endregion

        #region 抓年兽
        /// <summary>
        /// <playerbombnianeventinfo>
        ///     <niannum>0</niannum>
        ///     <nianhp>350</nianhp>
        ///     <nianmaxhp>350</nianmaxhp>
        ///     <firecrackersnum>5</firecrackersnum>
        ///     <stringfirecrackersnum>3</stringfirecrackersnum>
        ///     <springthundernum>2</springthundernum>
        ///     <niantype>0</niantype>
        /// </playerbombnianeventinfo>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="cost1"></param>
        /// <param name="cost5"></param>
        /// <param name="cost10"></param>
        /// <returns></returns>
        public int getBombNianInfo(ProtocolMgr protocol, ILogger logger, User user, bool onlyfree, int cost1, int cost5, int cost10)
        {
            if (!user.isActivityRunning(ActivityType.BombNianEvent)) return 10;

            try
            {
                string url = "/root/bombNianEvent!getBombNianInfo.action";
                ServerResult xml = protocol.getXml(url, "抓年兽");
                if (xml == null || !xml.CmdSucceed) return 10;

                //logInfo(logger, xml.getDebugInfo());
                //AstdLuaObject lua = new AstdLuaObject();
                //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
                //int niannum = lua.GetIntValue("results.playerbombnianeventinfo.niannum");
                //int nianhp = lua.GetIntValue("results.playerbombnianeventinfo.nianhp");
                //int nianmaxhp = lua.GetIntValue("results.playerbombnianeventinfo.nianmaxhp");
                //int firecrackersnum = lua.GetIntValue("results.playerbombnianeventinfo.firecrackersnum");
                //int stringfirecrackersnum = lua.GetIntValue("results.playerbombnianeventinfo.stringfirecrackersnum");
                //int springthundernum = lua.GetIntValue("results.playerbombnianeventinfo.springthundernum");
                //int niantype = lua.GetIntValue("results.playerbombnianeventinfo.niantype");
                //int firecrackerscost = lua.GetIntValue("results.cost.firecrackerscost");
                //int stringfirecrackerscost = lua.GetIntValue("results.cost.stringfirecrackerscost");
                //int springthundercost = lua.GetIntValue("results.cost.springthundercost");
                int niannum = 0, niantype = 0, firecrackersnum = 0, stringfirecrackersnum = 0, springthundernum = 0;
                int nianhp = 0, nianmaxhp = 0, firecrackerscost = 0, stringfirecrackerscost = 0, springthundercost = 0;
                niannum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/niannum"));
                nianhp = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/nianhp"));
                nianmaxhp = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/nianmaxhp"));
                firecrackersnum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/firecrackersnum"));
                stringfirecrackersnum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/stringfirecrackersnum"));
                springthundernum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/springthundernum"));
                niantype = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/playerbombnianeventinfo/niantype"));
                firecrackerscost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/cost/firecrackerscost"));
                stringfirecrackerscost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/cost/stringfirecrackerscost"));
                springthundercost = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/cost/springthundercost"));
                bool result = false;
                //float precent = (float)nianhp / (float)nianmaxhp;
                //if (precent >= 0.75)
                //{
                //    if (springthundercost <= cost10)
                //    {
                //        result = bombNian(protocol, logger, user, 3);
                //    }
                //    else if (stringfirecrackerscost <= cost5)
                //    {
                //        result = bombNian(protocol, logger, user, 2);
                //    }
                //    else if (firecrackerscost <= cost1)
                //    {
                //        result = bombNian(protocol, logger, user, 1);
                //    }
                //    else if (precent < 1.0)
                //    {
                //        result = huntNian(protocol, logger, user);
                //    }
                //    else
                //    {
                //        return 2;
                //    }
                //}
                //else if (precent >= 0.5)
                //{
                //    if (stringfirecrackerscost <= cost5)
                //    {
                //        result = bombNian(protocol, logger, user, 2);
                //    }
                //    else if (firecrackerscost <= cost1)
                //    {
                //        result = bombNian(protocol, logger, user, 1);
                //    }
                //    else if (springthundercost <= cost10)
                //    {
                //        result = bombNian(protocol, logger, user, 3);
                //    }
                //    else
                //    {
                //        result = huntNian(protocol, logger, user);
                //    }
                //}
                //else if (precent >= 0.2)
                //{
                //    if (firecrackerscost <= cost1)
                //    {
                //        result = bombNian(protocol, logger, user, 1);
                //    }
                //    else if (stringfirecrackerscost <= cost5)
                //    {
                //        result = bombNian(protocol, logger, user, 2);
                //    }
                //    else if (springthundercost <= cost10)
                //    {
                //        result = bombNian(protocol, logger, user, 3);
                //    }
                //    else
                //    {
                //        result = huntNian(protocol, logger, user);
                //    }
                //}
                //else
                //{
                //    result = huntNian(protocol, logger, user);
                //}
                if (nianhp > 100)
                {
                    if (onlyfree)
                    {
                        if (springthundernum > 0) result = bombNian(protocol, logger, user, 3);
                        else if (stringfirecrackersnum > 0) result = bombNian(protocol, logger, user, 2);
                        else if (firecrackersnum > 0) result = bombNian(protocol, logger, user, 1);
                        else return 2;
                    }
                    else
                    {
                        if (springthundercost <= cost10) result = bombNian(protocol, logger, user, 3);
                        else if (stringfirecrackerscost <= cost5) result = bombNian(protocol, logger, user, 2);
                        else if (firecrackerscost <= cost1) result = bombNian(protocol, logger, user, 1);
                        else return 2;
                    }
                }
                else if (nianhp > 50)
                {
                    if (onlyfree)
                    {
                        if (stringfirecrackersnum > 0) result = bombNian(protocol, logger, user, 2);
                        else if (firecrackersnum > 0) result = bombNian(protocol, logger, user, 1);
                        else if (springthundernum > 0) result = bombNian(protocol, logger, user, 3);
                        else return 2;
                    }
                    else
                    {
                        if (stringfirecrackerscost <= cost5) result = bombNian(protocol, logger, user, 2);
                        else if (firecrackerscost <= cost1) result = bombNian(protocol, logger, user, 1);
                        else if (springthundercost <= cost10) result = bombNian(protocol, logger, user, 3);
                        else return 2;
                    }
                }
                else if (nianhp > 10)
                {
                    if (onlyfree)
                    {
                        if (firecrackersnum > 0) result = bombNian(protocol, logger, user, 1);
                        else if (stringfirecrackersnum > 0) result = bombNian(protocol, logger, user, 2);
                        else if (springthundernum > 0) result = bombNian(protocol, logger, user, 3);
                        else return 2;
                    }
                    else
                    {
                        if (firecrackerscost <= cost1) result = bombNian(protocol, logger, user, 1);
                        else if (stringfirecrackerscost <= cost5) result = bombNian(protocol, logger, user, 2);
                        else if (springthundercost <= cost10) result = bombNian(protocol, logger, user, 3);
                        else return 2;
                    }
                }
                else
                {
                    result = huntNian(protocol, logger, user);
                }

                if (!result) return 1;
                return 0;
            }
            catch (Exception ex)
            {
                logInfo(logger, string.Format("getBombNianInfo error: {0}", ex.ToString()));
                return 10;
            }
        }

        public bool bombNian(ProtocolMgr protocol, ILogger logger, User user, int bombType)
        {
            string url = "/root/bombNianEvent!bombNian.action";
            string data = string.Format("bombType={0}", bombType);
            ServerResult xml = protocol.postXml(url, data, "放鞭炮");
            if (xml == null || !xml.CmdSucceed) return false;

            //logInfo(logger, xml.getDebugInfo());
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int bombattack = lua.GetIntValue("results.bombattack");
            int bombattack = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/bombattack"));
            RewardInfo reward = new RewardInfo();
            reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/bombnianreward/rewardinfo"));
            logInfo(logger, string.Format("放鞭炮，年兽血量减少{0}，获得{1}", bombattack, reward.ToString()));
            return true;
        }

        public bool huntNian(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/bombNianEvent!huntNian.action";
            ServerResult xml = protocol.getXml(url, "抓年兽");
            if (xml == null || !xml.CmdSucceed) return false;

            //logInfo(logger, xml.getDebugInfo());
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int huntstate = lua.GetIntValue("results.huntstate");
            int huntstate = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/huntstate"));
            if (huntstate == 1)
            {
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/huntnianreward/rewardinfo"));
                logInfo(logger, string.Format("年兽捕抓成功，获得{0}", reward.ToString()));
            }
            else
            {
                logInfo(logger, string.Format("年兽捕抓失败"));
            }
            return true;
        }
        #endregion
    }
}
