using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace com.lover.astd.common.logic
{
	public class HeroMgr : MgrBase
	{
		public HeroMgr(TimeMgr tmrMgr, ServiceFactory factory)
		{
			this._logColor = Color.LimeGreen;
			this._tmrMgr = tmrMgr;
			this._factory = factory;
		}
        /// <summary>
        /// 开始洗属性 0:成功 1:不需要洗 2:出错 3:没免费次数
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="hero_id"></param>
        /// <param name="wash_what"></param>
        /// <param name="only_free"></param>
        /// <param name="totalCredit"></param>
        /// <param name="wash_count"></param>
        /// <param name="old_attrib"></param>
        /// <returns></returns>
		public int startWash(ProtocolMgr protocol, ILogger logger, User user, int hero_id, string wash_what, bool only_free, int totalCredit, int wash_count, ref int[] old_attrib)
		{
			int has_wash_count = 0;
			this.getWashInfo(protocol, logger, user);
			List<Hero> heroes = user.Heroes;
			Hero hero = null;
			foreach (Hero current in heroes)
			{
				if (current.Id == hero_id)
				{
					hero = current;
					break;
				}
			}
			if (hero == null)
			{
				base.logInfo(logger, "未找到要洗点的将, ID为" + hero_id);
				return 2;
			}

            int level = hero.Level;
            int[] new_attrib = new int[3];
            int creditCostToWash = 0;
            int goldWashCount = 0;
            int superWashCount = 0;
            int canawaken = 0;
            int heroAttributes = this.getHeroAttributes(protocol, logger, hero, ref level, ref old_attrib, ref new_attrib);
            if (heroAttributes == 0)
            {
                base.logInfo(logger, "出现未知错误");
                return 2;
            }
            else if (heroAttributes == 1)
            {
                this.confirmHeroAttrib(protocol, logger, hero.Id, false);
            }
            this.getWashModes(protocol, logger, ref creditCostToWash, ref goldWashCount, ref superWashCount, ref canawaken);
            if (level == 400)
            {
                level = user.Level;
            }
            else if (level > user.Level)
            {
                level = user.Level;
            }
            if (!this.isHeroNeedWash(wash_what, only_free, level, old_attrib))
            {
                base.logInfo(logger, "该武将不需要洗属性");
                return 1;
            }
            else if (only_free)
            {
                if (goldWashCount == 0 && superWashCount == 0)
                {
                    base.logInfo(logger, "没有免费的洗将次数了");
                    return 3;
                }

                while (goldWashCount > 0 && has_wash_count < wash_count)
                {
                    int result = this.doWash(protocol, logger, hero, level, 2, wash_what, ref old_attrib, ref new_attrib);
                    if (result == 1)
                    {
                        return 2;
                    }
                    else if (result == 2)
                    {
                        return 1;
                    }
                    goldWashCount--;
                    has_wash_count++;
                }

                while (superWashCount > 0 && has_wash_count < wash_count)
                {
                    int result = this.doWash(protocol, logger, hero, level, 3, wash_what, ref old_attrib, ref new_attrib);
                    if (result == 1)
                    {
                        return 2;
                    }
                    else if (result == 2)
                    {
                        return 1;
                    }
                    superWashCount--;
                    has_wash_count++;
                }

                return 0;
            }
            else if (totalCredit < creditCostToWash)
            {
                base.logInfo(logger, "洗将军工不足");
                return 2;
            }
            else
            {
                while (totalCredit >= creditCostToWash && has_wash_count < wash_count)
                {
                    int result = this.doWash(protocol, logger, hero, level, 0, wash_what, ref old_attrib, ref new_attrib);
                    if (result == 1)
                    {
                        return 2;
                    }
                    else if (result == 2)
                    {
                        return 1;
                    }
                    totalCredit -= creditCostToWash;
                    has_wash_count++;
                }

                return 0;
            }
		}
        /// <summary>
        /// 执行洗属性 0:成功 1:出错 2:完成
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="hero"></param>
        /// <param name="wash_mode"></param>
        /// <param name="wash_what"></param>
        /// <param name="old_attrib"></param>
        /// <param name="new_attrib"></param>
        /// <returns></returns>
        private int doWash(ProtocolMgr protocol, ILogger logger, Hero hero, int herolevel, int wash_mode, string wash_what, ref int[] old_attrib, ref int[] new_attrib)
        {
            string text = "";
            if (wash_mode == 0)
            {
                text = "军功";
            }
            else if (wash_mode == 1)
            {
                text = "加强";
            }
            else if (wash_mode == 2)
            {
                text = "白金";
            }
            else if (wash_mode == 3)
            {
                text = "至尊";
            }
            if (!this.washHero(protocol, logger, hero, wash_mode, ref new_attrib))
            {
                base.logInfo(logger, "洗属性失败, 稍等几秒钟");
                return 1;
            }

            string text2 = string.Format("为武将[{0}]{1}洗属性, 获得属性[统:{2}, 勇:{3}, 智:{4}], 原属性为[统:{5}, 勇:{6}, 智:{7}]", new object[]
				{
					hero.Name,
					text,
					new_attrib[0],
					new_attrib[1],
					new_attrib[2],
					old_attrib[0],
					old_attrib[1],
					old_attrib[2]
				});
            base.logInfo(logger, text2);
            bool only_free = wash_mode != 0;
            if (this.isNewAttribBetter(logger, wash_what, only_free, herolevel, old_attrib, new_attrib))
            {
                this.confirmHeroAttrib(protocol, logger, hero.Id, true);
                for (int i = 0; i < 3; i++)
                {
                    old_attrib[i] = new_attrib[i];
                }
                if (!this.isHeroNeedWash(wash_what, only_free, herolevel, old_attrib))
                {
                    base.logInfo(logger, string.Format("武将[{0}]洗属性完毕", hero.Name));
                    return 2;
                }
            }
            else
            {
                this.confirmHeroAttrib(protocol, logger, hero.Id, false);
            }
            return 0;
        }
        /// <summary>
        /// 还需要洗?
        /// </summary>
        /// <param name="wash_attrib"></param>
        /// <param name="only_free"></param>
        /// <param name="hero_level"></param>
        /// <param name="old_attrib"></param>
        /// <returns></returns>
		private bool isHeroNeedWash(string wash_attrib, bool only_free, int hero_level, int[] old_attrib)
		{
			int maxattr = hero_level + 19;
			bool no_wash = true;
			for (int i = 0; i < wash_attrib.Length; i++)
			{
				if (wash_attrib[i].Equals('1') && old_attrib[i] < maxattr)
				{
					no_wash = false;
					break;
				}
			}
			return !no_wash;
		}
        /// <summary>
        /// 新属性更好?
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="wash_attrib">111</param>
        /// <param name="only_free"></param>
        /// <param name="old_attrib"></param>
        /// <param name="new_attrib"></param>
        /// <returns></returns>
        private bool isNewAttribBetter(ILogger logger, string wash_attrib, bool only_free, int hero_level, int[] old_attrib, int[] new_attrib)
		{
            int maxattr = hero_level + 19;
            int oldmaxdiff = 0;
            int newmaxdiff = 0;
			int[] attr = new int[3];
			for (int i = 0; i < wash_attrib.Length; i++)
			{
				if (wash_attrib[i].Equals('1'))
				{
					attr[i] = new_attrib[i] - old_attrib[i];
                    if (maxattr - old_attrib[i] > oldmaxdiff)
                    {
                        oldmaxdiff = maxattr - old_attrib[i];
                    }
                    if (maxattr - new_attrib[i] > newmaxdiff)
                    {
                        newmaxdiff = maxattr - new_attrib[i];
                    }
				}
				else
				{
					attr[i] = 0;
				}
			}
			int total = attr[0] + attr[1] + attr[2];
			if (only_free)
			{
                if (total == 0 && newmaxdiff < oldmaxdiff)
                {
                    return true;
                }
                else
                {
                    return (total > 0);
                }
			}

            if (new_attrib[0] >= old_attrib[0] && new_attrib[1] >= old_attrib[1] && new_attrib[2] >= old_attrib[2])
            {
                return true;
            }

            int max = -1;
            int min = 10000;
            int minidx = -1;
            for (int j = 0; j < 3; j++)
            {
                if (wash_attrib[j].Equals('1') && old_attrib[j] > max)
                {
                    max = old_attrib[j];
                }
                if (wash_attrib[j].Equals('1') && old_attrib[j] < min)
                {
                    min = old_attrib[j];
                    minidx = j;
                }
            }
            if (max - min < 20)
            {
                return (total > 0);
            }

            base.logInfo(logger, "需要洗的属性值中最大最小值相差已经大于20, 必须最小属性提升了并且总和增加才能替换");
            return (attr[minidx] > 0 && total > 0);
		}
        /// <summary>
        /// 获得洗属性模式
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="creditCostToWash">军功消耗</param>
        /// <param name="goldWashCount">免费白金洗</param>
        /// <param name="superWashCount">免费至尊洗</param>
        /// <param name="canawaken">觉醒</param>
        public List<BigHero> getWashModes(ProtocolMgr protocol, ILogger logger, ref int creditCostToWash, ref int goldWashCount, ref int superWashCount, ref int canawaken)
		{
            List<BigHero> list = new List<BigHero>();
			string url = "/root/general!getRefreshGeneralInfo.action";
			ServerResult xml = protocol.getXml(url, "获取武将洗属性参数");
			if (xml == null || !xml.CmdSucceed)
			{
                return list;
			}
            XmlDocument cmdResult = xml.CmdResult;
            canawaken = XmlHelper.GetValue<int>(cmdResult.SelectSingleNode("/results/canawaken"));
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/freebaijintime");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out goldWashCount);
            }
            else
            {
                goldWashCount = 0;
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/freezizuntime");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out superWashCount);
            }
            else
            {
                superWashCount = 0;
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/refreshmodel");
            foreach (XmlNode xmlNode3 in xmlNodeList)
            {
                XmlNodeList childNodes = xmlNode3.ChildNodes;
                string name = "";
                string cost = "";
                int freetimes = 0;
                foreach (XmlNode xmlNode4 in childNodes)
                {
                    if (xmlNode4.Name == "name")
                    {
                        name = xmlNode4.InnerText;
                    }
                    else if (xmlNode4.Name == "cost")
                    {
                        cost = xmlNode4.InnerText;
                    }
                    else if (xmlNode4.Name == "freetimes")
                    {
                        int.TryParse(xmlNode4.InnerText, out freetimes);
                    }
                }
                if (name.Contains("普通洗属性") || name.Contains("普通洗屬性"))
                {
                    string s = cost.Replace("军功", "");
                    int.TryParse(s, out creditCostToWash);
                }
            }
            Dictionary<string, int> indexs_ = this._factory.getBigHeroManager().indexs_;
            XmlNodeList generalList = cmdResult.SelectNodes("/results/general");
            foreach (XmlNode general in generalList)
            {
                BigHero hero = new BigHero();
                XmlNodeList generalattrs = general.ChildNodes;
                foreach (XmlNode attrs in generalattrs)
                {
                    if (attrs.Name == "generalname")
                    {
                        hero.Name = attrs.InnerText;
                    }
                    else if (attrs.Name == "generalid")
                    {
                        hero.Id = int.Parse(attrs.InnerText);
                    }
                    else if (attrs.Name == "isawaken")
                    {
                        hero.CanAwaken = 1;
                    }
                }
                if (hero.Id > 0 && hero.Name != null)
                {
                    if (indexs_.ContainsKey(hero.Name))
                    {
                        hero.Index = indexs_[hero.Name];
                    }
                    else
                    {
                        hero.Index = 9999;
                    }
                    list.Add(hero);
                }
            }
            return list;
		}
        /// <summary>
        /// 获得所有武将信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public void getWashInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/general!getTrainGeneralInfo.action";
            ServerResult xml = protocol.getXml(url, "获取武将信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }

            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            List<Hero> heroes = user.Heroes;
            List<int> list = new List<int>();
            foreach (XmlNode xmlNode2 in childNodes)
            {
                if (xmlNode2.Name == "general")
                {
                    int generalid = 0;
                    XmlNode xmlNode3 = xmlNode2.SelectSingleNode("generalid");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out generalid);
                    }
                    if (generalid != 0)
                    {
                        list.Add(generalid);
                        XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                        Hero hero = this.getHero(heroes, generalid);
                        if (hero == null)
                        {
                            hero = new Hero();
                            heroes.Add(hero);
                        }
                        hero.fillValues(childNodes2);
                    }
                }
            }

            for (int i = 0; i < heroes.Count; i++)
            {
                if (!list.Contains(heroes[i].Id))
                {
                    heroes.RemoveAt(i);
                    i--;
                }
            }

            for (int j = 0; j < heroes.Count - 1; j++)
            {
                for (int k = j + 1; k < heroes.Count; k++)
                {
                    if (heroes[j].Level < heroes[k].Level)
                    {
                        Hero value = heroes[j];
                        heroes[j] = heroes[k];
                        heroes[k] = value;
                    }
                }
            }
        }
        /// <summary>
        /// 根据武将id获得武将
        /// </summary>
        /// <param name="heroes"></param>
        /// <param name="heroId"></param>
        /// <returns></returns>
		private Hero getHero(List<Hero> heroes, int heroId)
		{
            for (int i = 0; i < heroes.Count; i++)
			{
				if (heroes[i].Id == heroId)
				{
                    return heroes[i];
				}
			}
            return null;
		}
        /// <summary>
        /// 获取武将属性 0:出错 1:有新属性 2:无新属性
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="hero"></param>
        /// <param name="heroLevel"></param>
        /// <param name="old_attrib"></param>
        /// <param name="new_attrib"></param>
        /// <returns></returns>
		private int getHeroAttributes(ProtocolMgr protocol, ILogger logger, Hero hero, ref int heroLevel, ref int[] old_attrib, ref int[] new_attrib)
		{
			if (hero == null)
			{
                return 0;
			}

            string url = "/root/general!getRefreshGeneralDetailInfo.action";
            string data = string.Format("generalId={0}", hero.Id);
            ServerResult xml = protocol.postXml(url, data, "获取武将洗属性信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return 0;
            }

            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int level = lua.GetIntValue("results.generaldto.generallevel");
            int level = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/generaldto/generallevel"));
            if (level > 0)
            {
                hero.Level = heroLevel = level;
            }
            //if (lua.IsNull("results.general"))
            //{
            //    return 0;
            //}
            XmlNode node = xml.CmdResult.SelectSingleNode("/results/general");
            if (node == null)
            {
                return 0;
            }

            //old_attrib[0] = lua.GetIntValue("results.general.originalattr.plusleader");
            //old_attrib[1] = lua.GetIntValue("results.general.originalattr.plusforces");
            //old_attrib[2] = lua.GetIntValue("results.general.originalattr.plusintelligence");
            old_attrib[0] = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/originalattr/plusleader"));
            old_attrib[1] = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/originalattr/plusforces"));
            old_attrib[2] = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/originalattr/plusintelligence"));
            //if (lua.IsNull("results.general.newattr"))
            //{
            //    return 2;
            //}
            node = xml.CmdResult.SelectSingleNode("/results/general/newattr");
            if (node == null)
            {
                return 2;
            }

            //new_attrib[0] = lua.GetIntValue("results.general.newattr.plusleader");
            //new_attrib[1] = lua.GetIntValue("results.general.newattr.plusforces");
            //new_attrib[2] = lua.GetIntValue("results.general.newattr.plusintelligence");
            new_attrib[0] = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/newattr/plusleader"));
            new_attrib[1] = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/newattr/plusforces"));
            new_attrib[2] = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/newattr/plusintelligence"));
            return 1;
		}
        /// <summary>
        /// 洗属性
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="hero"></param>
        /// <param name="washMode"></param>
        /// <param name="new_attrib"></param>
        /// <returns></returns>
        private bool washHero(ProtocolMgr protocol, ILogger logger, Hero hero, int washMode, ref int[] new_attrib)
        {
            if (hero == null)
            {
                return false;
            }

            string url = "/root/general!refreshGeneral.action";
            string data = string.Format("generalId={0}&refreshModel={1}", hero.Id, washMode);
            ServerResult xml = protocol.postXml(url, data, string.Format("为[{0}]洗属性", hero.Name));
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }

            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/general");
            if (xmlNode == null)
            {
                return false;
            }

            XmlNodeList childNodes = xmlNode.ChildNodes;
            foreach (XmlNode xmlNode2 in childNodes)
            {
                if (xmlNode2.Name == "plusleader")
                {
                    new_attrib[0] = int.Parse(xmlNode2.InnerText);
                }
                else if (xmlNode2.Name == "plusforces")
                {
                    new_attrib[1] = int.Parse(xmlNode2.InnerText);
                }
                else if (xmlNode2.Name == "plusintelligence")
                {
                    new_attrib[2] = int.Parse(xmlNode2.InnerText);
                }
            }
            return true;
        }
        /// <summary>
        /// 确认洗属性结果
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="heroId"></param>
        /// <param name="accept"></param>
		private void confirmHeroAttrib(ProtocolMgr protocol, ILogger logger, int heroId, bool accept)
		{
			string url = "/root/general!refreshGeneralConfirm.action";
			string data = string.Format("choose={0}&generalId={1}", accept ? 1 : 0, heroId);
			ServerResult serverResult = protocol.postXml(url, data, "确认武将洗属性信息");
			if (serverResult == null || !serverResult.CmdSucceed)
			{
                return;
			}
            if (accept)
            {
                XmlNode xmlNode = serverResult.CmdResult.SelectSingleNode("/results/message");
                base.logInfo(logger, xmlNode.InnerText);
            }
            else
            {
                base.logInfo(logger, "保持原有属性不变");
            }
		}
        /// <summary>
        /// 获取武将信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
		public void getAllHeros(ProtocolMgr protocol, ILogger logger, ref User user)
		{
			string url = "/root/general!getTrainGeneralInfo.action";
			ServerResult xml = protocol.getXml(url, "获取武将信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cd");
            if (xmlNode2 != null)
            {
                int cd = 0;
                int.TryParse(xmlNode2.InnerText, out cd);
                user.TuFeiCd = (long)cd;
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/cdflag");
            if (xmlNode3 != null)
            {
                user.TuFeiCdFlag = (xmlNode3.InnerText == "1");
            }
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/tfnum");
            if (xmlNode4 != null)
            {
                int tufeiNumber = 0;
                int.TryParse(xmlNode4.InnerText, out tufeiNumber);
                user.TufeiNumber = tufeiNumber;
            }
            int jyungong = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            List<Hero> heroes = user.Heroes;
            List<int> list = new List<int>();
            foreach (XmlNode xmlNode5 in childNodes)
            {
                if (xmlNode5.Name == "general")
                {
                    int generalid = 0;
                    XmlNode xmlNode6 = xmlNode5.SelectSingleNode("generalid");
                    if (xmlNode6 != null)
                    {
                        int.TryParse(xmlNode6.InnerText, out generalid);
                    }
                    if (generalid != 0)
                    {
                        list.Add(generalid);
                        XmlNodeList childNodes2 = xmlNode5.ChildNodes;
                        Hero hero = this.getHero(heroes, generalid);
                        if (hero == null)
                        {
                            hero = new Hero();
                            heroes.Add(hero);
                        }
                        hero.TrainPositionId = 0;
                        hero.fillValues(childNodes2);
                        XmlNode xmlNode7 = xmlNode5.SelectSingleNode("jyungong");
                        if (xmlNode7 != null)
                        {
                            int.TryParse(xmlNode7.InnerText, out jyungong);
                        }
                        if (jyungong > 0)
                        {
                            user.TufeiCredit = jyungong;
                        }
                    }
                }
            }
            for (int i = 0; i < heroes.Count; i++)
            {
                int id = heroes[i].Id;
                if (!list.Contains(id))
                {
                    heroes.RemoveAt(i);
                    i--;
                }
            }
            foreach (XmlNode xmlNode8 in childNodes)
            {
                if (xmlNode8.Name == "trainpos")
                {
                    XmlNodeList childNodes3 = xmlNode8.ChildNodes;
                    int traingeneralid = 0;
                    int trainPositionId = 0;
                    foreach (XmlNode xmlNode9 in childNodes3)
                    {
                        if (xmlNode9.Name == "traingeneralid")
                        {
                            traingeneralid = int.Parse(xmlNode9.InnerText);
                        }
                        else if (xmlNode9.Name == "trainposid")
                        {
                            trainPositionId = int.Parse(xmlNode9.InnerText);
                        }
                    }
                    if (traingeneralid > 0)
                    {
                        Hero hero2 = this.getHero(heroes, traingeneralid);
                        hero2.TrainPositionId = trainPositionId;
                    }
                }
            }
		}

        public bool openAllTrainer(ProtocolMgr proto, ILogger logger)
        {
            string url = "/root/trainer!openAllTrainer.action";
            ServerResult xml = proto.postXml(url, "gold=0", "训练师全开");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int totalcost = 0;
            XmlNode node = cmdResult.SelectSingleNode("/results/trainers/totalcost");
            if (node != null)
            {
                int.TryParse(node.InnerText, out totalcost);
            }
            logger.logInfo(string.Format("花费{0}银币，训练师全开", totalcost));
            return true;
        }

		public bool removeTrainCd(ProtocolMgr proto, ILogger logger)
		{
			string url = "/root/general!cdRecoverConfirm.action";
			ServerResult xml = proto.getXml(url, "秒突飞cd");
			bool flag = xml == null || !xml.CmdSucceed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				XmlDocument cmdResult = xml.CmdResult;
				base.logInfo(logger, "秒突飞CD");
				result = true;
			}
			return result;
		}

		public bool trainHero(ProtocolMgr proto, ILogger logger, User user, int heroId)
		{
			string url = "/root/general!guideGeneral.action";
			string data = string.Format("guideModel=1&generalId={0}", heroId);
			List<Hero> heroes = user.Heroes;
			Hero hero = this.getHero(heroes, heroId);
			if (hero == null)
			{
				return false;
			}
            else if (hero.TrainPositionId == 0)
            {
                return false;
            }
            else
            {
                ServerResult serverResult = proto.postXml(url, data, "训练武将");
                if (serverResult == null)
                {
                    return false;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNodeList childNodes = cmdResult.SelectSingleNode("/results").ChildNodes;
                    int level = hero.Level;
                    int shiftLevel = hero.ShiftLevel;
                    List<int> list = new List<int>();
                    int num = 0;
                    foreach (XmlNode xmlNode in childNodes)
                    {
                        if (xmlNode.Name == "effect")
                        {
                            XmlNodeList childNodes2 = xmlNode.ChildNodes;
                            IEnumerator enumerator2 = childNodes2.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    XmlNode xmlNode2 = (XmlNode)enumerator2.Current;
                                    if (xmlNode2.Name == "guideexp")
                                    {
                                        num = int.Parse(xmlNode2.InnerText);
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
                        if (xmlNode.Name == "general")
                        {
                            int num2 = 0;
                            XmlNode xmlNode3 = xmlNode.SelectSingleNode("generalid");
                            if (xmlNode3 != null)
                            {
                                int.TryParse(xmlNode3.InnerText, out num2);
                            }
                            if (num2 != 0)
                            {
                                list.Add(num2);
                                XmlNodeList childNodes3 = xmlNode.ChildNodes;
                                Hero hero2 = this.getHero(heroes, num2);
                                if (hero2 == null)
                                {
                                    hero2 = new Hero();
                                    heroes.Add(hero2);
                                }
                                hero2.TrainPositionId = 0;
                                hero2.fillValues(childNodes3);
                                int num3 = 0;
                                XmlNode xmlNode4 = xmlNode.SelectSingleNode("jyungong");
                                if (xmlNode4 != null)
                                {
                                    int.TryParse(xmlNode4.InnerText, out num3);
                                }
                                if (num3 > 0)
                                {
                                    user.TufeiCredit = num3;
                                }
                            }
                        }
                    }
                    int num4;
                    for (int i = 0; i < heroes.Count; i = num4 + 1)
                    {
                        int id = heroes[i].Id;
                        bool flag13 = !list.Contains(id);
                        if (flag13)
                        {
                            heroes.RemoveAt(i);
                            num4 = i;
                            i = num4 - 1;
                        }
                        num4 = i;
                    }
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/cd");
                    bool flag14 = xmlNode5 != null;
                    int num5;
                    if (flag14)
                    {
                        int.TryParse(xmlNode5.InnerText, out num5);
                    }
                    else
                    {
                        num5 = 0;
                    }
                    XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/cdflag");
                    bool flag15 = xmlNode6 != null;
                    if (flag15)
                    {
                        user.TuFeiCdFlag = (xmlNode6.InnerText == "1");
                    }
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/tfnum");
                    bool flag16 = xmlNode7 != null;
                    int num6;
                    if (flag16)
                    {
                        int.TryParse(xmlNode7.InnerText, out num6);
                    }
                    else
                    {
                        num6 = 0;
                    }
                    user.TuFeiCd = (long)num5;
                    user.TufeiNumber = num6;
                    string text = string.Format("突飞武将 [{0}], 获得经验 [{1}]", hero.Name, num);
                    bool flag17 = num6 > 0;
                    if (flag17)
                    {
                        text += string.Format(", 剩余突飞令 [{0}]个", num6);
                    }
                    bool flag18 = level != hero.Level;
                    if (flag18)
                    {
                        text += string.Format(", 武将升级 {0}=>{1}", level, hero.Level);
                    }
                    bool flag19 = shiftLevel != hero.ShiftLevel;
                    if (flag19)
                    {
                        text += string.Format(", 将星增加{0}", hero.ShiftLevel - shiftLevel);
                    }
                    base.logInfo(logger, text);
                    return true;
                }
            }
		}
        /// <summary>
        /// 潜能淬炼
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public long handleXiZhuge(ProtocolMgr proto, ILogger logger, User user)
        {
            string url = "/root/equip!getEquip.action";
            ServerResult xml = proto.getXml(url, "武将装备");
            if (xml == null || !xml.CmdSucceed)
            {
                return next_halfhour();
            }
            //XmlNodeList xmlnodelist = xml.CmdResult.SelectNodes("/results/general");
            //if (xmlnodelist == null || xmlnodelist.Count == 0)
            //{
            //    return next_halfhour();
            //}
            //Dictionary<int, int> generals = new Dictionary<int, int>();
            //foreach (XmlNode xmlnode in xmlnodelist)
            //{
            //    //AstdLuaObject lua = new AstdLuaObject();
            //    //lua.ParseXml(xmlnode);
            //    //int generalid = lua.GetIntValue("general.generalid");
            //    //int zhugeid = lua.GetIntValue("general.zhugeid");
            //    //int zhugetimes = lua.GetIntValue("general.zhugetimes");
            //    int generalid = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/generalid"));
            //    int zhugeid = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/zhugeid"));
            //    int zhugetimes = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/general/zhugetimes"));
            //    if (zhugeid > 0 && zhugetimes > 0)
            //    {
            //        generals.Add(generalid, zhugeid);
            //    }
            //}
            List<General> generals = XmlHelper.GetClassList<General>(xml.CmdResult.SelectNodes("/results/general"));
            if (generals.Count == 0)
            {
                return next_day();
            }
            bool finish = true;
            //foreach (KeyValuePair<int, int> kvp in generals)
            //{
            //    int result = getXiZhugeInfo(proto, logger, user, kvp.Key, kvp.Value);
            //    if (result != 2)
            //    {
            //        finish = false;
            //    }
            //}
            foreach (General current in generals)
            {
                int result = getXiZhugeInfo(proto, logger, user, current.generalid, current.zhugeid);
                if (result != 2)
                {
                    finish = false;
                }
            }
            if (finish)
            {
                return next_day();
            }
            return immediate();
        }
        /// <summary>
        /// 获取诸葛套装
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="generalId"></param>
        /// <param name="zhugeid"></param>
        /// <returns></returns>
        public int getXiZhugeInfo(ProtocolMgr proto, ILogger logger, User user, int generalId, int zhugeid)
        {
            string url = "/root/equip!getXiZhugeInfo.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = proto.postXml(url, data, "潜能");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //int maxattr = lua.GetIntValue("results.maxattr");
            //int freenum = lua.GetIntValue("results.freenum");
            //int curattr_lea = lua.GetIntValue("results.curattr.lea");
            //int curattr_str = lua.GetIntValue("results.curattr.str");
            //int curattr_int = lua.GetIntValue("results.curattr.int");
            //int newattr_lea = lua.GetIntValue("results.newattr.lea");
            //int newattr_str = lua.GetIntValue("results.newattr.str");
            //int newattr_int = lua.GetIntValue("results.newattr.int");
            //int resetattr_lea = lua.GetIntValue("results.resetattr.lea");
            //int resetattr_str = lua.GetIntValue("results.resetattr.str");
            //int resetattr_int = lua.GetIntValue("results.resetattr.int");
            int maxattr = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/maxattr"));
            int freenum = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/freenum"));
            int curattr_lea = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/curattr/lea"));
            int curattr_str = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/curattr/str"));
            int curattr_int = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/curattr/int"));
            int newattr_lea = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/newattr/lea"));
            int newattr_str = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/newattr/str"));
            int newattr_int = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/newattr/int"));
            int resetattr_lea = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/resetattr/lea"));
            int resetattr_str = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/resetattr/str"));
            int resetattr_int = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/resetattr/int"));
            if (newattr_lea != 0 && newattr_str != 0 && newattr_int != 0)
            {
                int total_cur = curattr_lea + curattr_str + curattr_int;
                int total_new = newattr_lea + newattr_str + newattr_int;
                int type = 2;
                if (total_new > total_cur)
                {
                    type = 1;
                }
                if (!xiZhugeConfirm(proto, logger, user, zhugeid, type, curattr_lea, curattr_str, curattr_int, newattr_lea, newattr_str, newattr_int))
                {
                    return 1;
                }
                return 0;
            }
            if (curattr_lea == maxattr && curattr_str == maxattr && curattr_int == maxattr)
            {
                return 2;
            }
            if (freenum == 0)
            {
                return 2;
            }
            return xiZhuge(proto, logger, user, zhugeid, curattr_lea, curattr_str, curattr_int);
        }
        /// <summary>
        /// 淬炼
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="storeId"></param>
        /// <param name="curattr_lea"></param>
        /// <param name="curattr_str"></param>
        /// <param name="curattr_int"></param>
        /// <returns></returns>
        public int xiZhuge(ProtocolMgr proto, ILogger logger, User user, int storeId, int curattr_lea, int curattr_str, int curattr_int)
        {
            string url = "/root/equip!xiZhuge.action";
            string data = string.Format("storeId={0}", storeId);
            ServerResult xml = proto.postXml(url, data, "潜能淬炼");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            //AstdLuaObject lua = new AstdLuaObject();
            //lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            //string newattr = lua.GetStringValue("results.newattr");
            string newattr = XmlHelper.GetString(xml.CmdResult.SelectSingleNode("/results/newattr"));
            string[] attrs = newattr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (attrs.Length != 3)
            {
                return 1;
            }
            int newattr_lea = int.Parse(attrs[0]);
            int newattr_str = int.Parse(attrs[1]);
            int newattr_int = int.Parse(attrs[2]);
            int total_cur = curattr_lea + curattr_str + curattr_int;
            int total_new = newattr_lea + newattr_str + newattr_int;
            int type = 2;
            if (total_new > total_cur)
            {
                type = 1;
            }
            if (!xiZhugeConfirm(proto, logger, user, storeId, type, curattr_lea, curattr_str, curattr_int, newattr_lea, newattr_str, newattr_int))
            {
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 潜能淬炼确认
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="storeId"></param>
        /// <param name="type">1:替换 2:维持</param>
        /// <returns></returns>
        public bool xiZhugeConfirm(ProtocolMgr proto, ILogger logger, User user, int storeId, int type, int curattr_lea, int curattr_str, int curattr_int, int newattr_lea, int newattr_str, int newattr_int)
        {
            string url = "/root/equip!xiZhugeConfirm.action";
            string data = string.Format("type={0}&storeId={1}", type, storeId);
            ServerResult xml = proto.postXml(url, data, "潜能淬炼确认");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            logInfo(logger, string.Format("免费潜能淬炼,原属性:统+{0},勇+{1},智+{2},新属性:统+{3},勇+{4},智+{5},{6}",
                curattr_lea, curattr_str, curattr_int,
                newattr_lea, newattr_str, newattr_int,
                (type == 2 ? "维持" : "替换")));
            return true;
        }
        /// <summary>
        /// 获取觉醒信息
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="logger"></param>
        public GeneralAwakeInfo getAwakenGeneralInfo(ProtocolMgr proto, ILogger logger, BigHero hero)
        {
            string url = "/root/general!getAwakenGeneralInfo.action";
            string data = string.Format("generalId={0}", hero.Id);
            ServerResult xml = proto.postXml(url, data, "觉醒信息");
            if (xml == null || !xml.CmdSucceed) return null;

            return XmlHelper.GetClass<GeneralAwakeInfo>(xml.CmdResult.SelectSingleNode("/results/generalawakeinfo"));
        }
        /// <summary>
        /// 觉醒
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="logger"></param>
        /// <param name="hero"></param>
        public void awakenGeneral(ProtocolMgr proto, ILogger logger, BigHero hero)
        {
            string url = "/root/general!awakenGeneral.action";
            string data = string.Format("generalId={0}", hero.Id);
            ServerResult xml = proto.postXml(url, data, "觉醒");
            if (xml == null || !xml.CmdSucceed) return;

            logInfo(logger, string.Format("觉醒大将[{0}]", hero.Name));
        }
	}
}
