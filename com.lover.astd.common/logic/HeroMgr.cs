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

		public int startWash(ProtocolMgr protocol, ILogger logger, User user, int hero_id, string wash_what, bool only_free, int totalCredit, int wash_count, ref int[] old_attrib)
		{
			int num = 0;
			this.getWashInfo(protocol, logger, user);
			List<Hero> heroes = user.Heroes;
			Hero hero = null;
			foreach (Hero current in heroes)
			{
				bool flag = current.Id == hero_id;
				if (flag)
				{
					hero = current;
					break;
				}
			}
			bool flag2 = hero == null;
			int result;
			if (flag2)
			{
				base.logInfo(logger, "未找到要洗点的将, ID为" + hero_id);
				result = 2;
			}
			else
			{
				int level = hero.Level;
				int[] array = new int[3];
				int[] array2 = array;
				int num2 = 0;
				int i = 0;
				int num3 = 0;
				int heroAttributes = this.getHeroAttributes(protocol, logger, hero, ref level, ref old_attrib, ref array2);
				bool flag3 = heroAttributes == 0;
				if (flag3)
				{
					base.logInfo(logger, "出现未知错误");
					result = 2;
				}
				else
				{
					bool flag4 = heroAttributes == 1;
					if (flag4)
					{
						this.confirmHeroAttrib(protocol, logger, hero.Id, false);
					}
					this.getWashModes(protocol, logger, ref num2, ref i, ref num3);
					bool flag5 = !this.isHeroNeedWash(wash_what, only_free, level, old_attrib);
					if (flag5)
					{
						base.logInfo(logger, "该武将不需要洗属性");
						result = 1;
					}
					else if (only_free)
					{
						bool flag6 = i == 0 && num3 == 0;
						if (flag6)
						{
							base.logInfo(logger, "没有免费的洗将次数了");
							result = 3;
						}
						else
						{
							while (i > 0)
							{
								bool flag7 = num >= wash_count;
								if (flag7)
								{
									break;
								}
								int num4 = this.doWash(protocol, logger, hero, 2, wash_what, ref old_attrib, ref array2);
								bool flag8 = num4 == 1;
								if (flag8)
								{
									result = 2;
									return result;
								}
								bool flag9 = num4 == 2;
								if (flag9)
								{
									result = 1;
									return result;
								}
								int num5 = i;
								i = num5 - 1;
								num5 = num;
								num = num5 + 1;
							}
							while (num3 > 0 && num < wash_count)
							{
								int num6 = this.doWash(protocol, logger, hero, 3, wash_what, ref old_attrib, ref array2);
								bool flag10 = num6 == 1;
								if (flag10)
								{
									result = 2;
									return result;
								}
								bool flag11 = num6 == 2;
								if (flag11)
								{
									result = 1;
									return result;
								}
								int num5 = num3;
								num3 = num5 - 1;
								num5 = num;
								num = num5 + 1;
							}
							result = 0;
						}
					}
					else
					{
						bool flag12 = totalCredit < num2;
						if (flag12)
						{
							base.logInfo(logger, "洗将军工不足");
							result = 2;
						}
						else
						{
							int num7 = totalCredit;
							while (num7 >= num2 && num < wash_count)
							{
								int num8 = this.doWash(protocol, logger, hero, 0, wash_what, ref old_attrib, ref array2);
								bool flag13 = num8 == 1;
								if (flag13)
								{
									result = 2;
									return result;
								}
								bool flag14 = num8 == 2;
								if (flag14)
								{
									result = 1;
									return result;
								}
								num7 -= num2;
								int num5 = num;
								num = num5 + 1;
							}
							result = 0;
						}
					}
				}
			}
			return result;
		}

		private int doWash(ProtocolMgr protocol, ILogger logger, Hero hero, int wash_mode, string wash_what, ref int[] old_attrib, ref int[] new_attrib)
		{
			string text = "";
			bool flag = wash_mode == 0;
			if (flag)
			{
				text = "军功";
			}
			else
			{
				bool flag2 = wash_mode == 1;
				if (flag2)
				{
					text = "加强";
				}
				else
				{
					bool flag3 = wash_mode == 2;
					if (flag3)
					{
						text = "白金";
					}
					else
					{
						bool flag4 = wash_mode == 3;
						if (flag4)
						{
							text = "至尊";
						}
					}
				}
			}
			bool flag5 = !this.washHero(protocol, logger, hero, wash_mode, ref new_attrib);
			int result;
			if (flag5)
			{
				base.logInfo(logger, "洗属性失败, 稍等几秒钟");
				result = 1;
			}
			else
			{
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
				bool flag6 = this.isNewAttribBetter(logger, wash_what, only_free, old_attrib, new_attrib);
				if (flag6)
				{
					this.confirmHeroAttrib(protocol, logger, hero.Id, true);
					int num;
					for (int i = 0; i < 3; i = num + 1)
					{
						old_attrib[i] = new_attrib[i];
						num = i;
					}
					bool flag7 = !this.isHeroNeedWash(wash_what, only_free, hero.Level, old_attrib);
					if (flag7)
					{
						base.logInfo(logger, string.Format("武将[{0}]洗属性完毕", hero.Name));
						result = 2;
						return result;
					}
				}
				else
				{
					this.confirmHeroAttrib(protocol, logger, hero.Id, false);
				}
				result = 0;
			}
			return result;
		}

		private bool isHeroNeedWash(string wash_attrib, bool only_free, int hero_level, int[] old_attrib)
		{
			int num = hero_level + 20;
			bool flag = true;
			int num2;
			for (int i = 0; i < wash_attrib.Length; i = num2 + 1)
			{
				bool flag2 = wash_attrib[i].Equals('1') && old_attrib[i] < num;
				if (flag2)
				{
					flag = false;
					break;
				}
				num2 = i;
			}
			return !flag;
		}

		private bool isNewAttribBetter(ILogger logger, string wash_attrib, bool only_free, int[] old_attrib, int[] new_attrib)
		{
			int[] array = new int[3];
			int num;
			for (int i = 0; i < wash_attrib.Length; i = num + 1)
			{
				bool flag = wash_attrib[i].Equals('1');
				if (flag)
				{
					array[i] = new_attrib[i] - old_attrib[i];
				}
				else
				{
					array[i] = 0;
				}
				num = i;
			}
			int num2 = array[0] + array[1] + array[2];
			bool result;
			if (only_free)
			{
				result = (num2 > 0);
			}
			else
			{
				bool flag2 = new_attrib[0] >= old_attrib[0] && new_attrib[1] >= old_attrib[1] && new_attrib[2] >= old_attrib[2];
				if (flag2)
				{
					result = true;
				}
				else
				{
					int num3 = -1;
					int num4 = 10000;
					int num5 = -1;
					for (int j = 0; j < 3; j = num + 1)
					{
						bool flag3 = wash_attrib[j].Equals('1') && old_attrib[j] > num3;
						if (flag3)
						{
							num3 = old_attrib[j];
						}
						bool flag4 = wash_attrib[j].Equals('1') && old_attrib[j] < num4;
						if (flag4)
						{
							num4 = old_attrib[j];
							num5 = j;
						}
						num = j;
					}
					bool flag5 = num3 - num4 < 20;
					if (flag5)
					{
						result = (num2 > 0);
					}
					else
					{
						base.logInfo(logger, "需要洗的属性值中最大最小值相差已经大于20, 必须最小属性提升了并且总和增加才能替换");
						result = (array[num5] > 0 && num2 > 0);
					}
				}
			}
			return result;
		}

		public void getWashModes(ProtocolMgr protocol, ILogger logger, ref int creditCostToWash, ref int goldWashCount, ref int superWashCount)
		{
			string url = "/root/general!getRefreshGeneralInfo.action";
			ServerResult xml = protocol.getXml(url, "获取武将洗属性参数");
			bool flag = xml == null || !xml.CmdSucceed;
			if (!flag)
			{
				XmlDocument cmdResult = xml.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/freebaijintime");
				bool flag2 = xmlNode != null;
				if (flag2)
				{
					int.TryParse(xmlNode.InnerText, out goldWashCount);
				}
				else
				{
					goldWashCount = 0;
				}
				XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/freezizuntime");
				bool flag3 = xmlNode2 != null;
				if (flag3)
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
					string text = "";
					string text2 = "";
					int num = 0;
					foreach (XmlNode xmlNode4 in childNodes)
					{
						bool flag4 = xmlNode4.Name == "name";
						if (flag4)
						{
							text = xmlNode4.InnerText;
						}
						else
						{
							bool flag5 = xmlNode4.Name == "cost";
							if (flag5)
							{
								text2 = xmlNode4.InnerText;
							}
							else
							{
								bool flag6 = xmlNode4.Name == "freetimes";
								if (flag6)
								{
									int.TryParse(xmlNode4.InnerText, out num);
								}
							}
						}
					}
					bool flag7 = text.Contains("普通洗属性") || text.Contains("普通洗屬性");
					if (flag7)
					{
						string s = text2.Replace("军功", "");
						int.TryParse(s, out creditCostToWash);
					}
				}
			}
		}

		public void getWashInfo(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/general!getTrainGeneralInfo.action";
			ServerResult xml = protocol.getXml(url, "获取武将信息");
			bool flag = xml == null || !xml.CmdSucceed;
			if (!flag)
			{
				XmlDocument cmdResult = xml.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
				XmlNodeList childNodes = xmlNode.ChildNodes;
				List<Hero> heroes = user.Heroes;
				List<int> list = new List<int>();
				foreach (XmlNode xmlNode2 in childNodes)
				{
					bool flag2 = xmlNode2.Name == "general";
					if (flag2)
					{
						int num = 0;
						XmlNode xmlNode3 = xmlNode2.SelectSingleNode("generalid");
						bool flag3 = xmlNode3 != null;
						if (flag3)
						{
							int.TryParse(xmlNode3.InnerText, out num);
						}
						bool flag4 = num != 0;
						if (flag4)
						{
							list.Add(num);
							XmlNodeList childNodes2 = xmlNode2.ChildNodes;
							Hero hero = this.getHero(heroes, num);
							bool flag5 = hero == null;
							if (flag5)
							{
								hero = new Hero();
								heroes.Add(hero);
							}
							hero.fillValues(childNodes2);
						}
					}
				}
				int num2;
				for (int i = 0; i < heroes.Count; i = num2 + 1)
				{
					int id = heroes[i].Id;
					bool flag6 = !list.Contains(id);
					if (flag6)
					{
						heroes.RemoveAt(i);
						num2 = i;
						i = num2 - 1;
					}
					num2 = i;
				}
				for (int j = 0; j < heroes.Count - 1; j = num2 + 1)
				{
					for (int k = j + 1; k < heroes.Count; k = num2 + 1)
					{
						bool flag7 = heroes[j].Level < heroes[k].Level;
						if (flag7)
						{
							Hero value = heroes[j];
							heroes[j] = heroes[k];
							heroes[k] = value;
						}
						num2 = k;
					}
					num2 = j;
				}
			}
		}

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

		private int getHeroAttributes(ProtocolMgr protocol, ILogger logger, Hero h, ref int heroLevel, ref int[] old_attrib, ref int[] new_attrib)
		{
			bool flag = h == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				string url = "/root/general!getRefreshGeneralDetailInfo.action";
				string data = "generalId=" + h.Id;
				ServerResult serverResult = protocol.postXml(url, data, "获取武将洗属性信息");
				bool flag2 = serverResult == null || !serverResult.CmdSucceed;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					XmlDocument cmdResult = serverResult.CmdResult;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/generaldto/generallevel");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						int num = 0;
						bool flag4 = int.TryParse(xmlNode.InnerText, out num);
						if (flag4)
						{
							h.Level = num;
							heroLevel = num;
						}
					}
					bool flag5 = cmdResult.SelectSingleNode("/results/general") == null;
					if (flag5)
					{
						result = 0;
					}
					else
					{
						XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/general/originalattr");
						foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
						{
							bool flag6 = xmlNode3.Name == "plusleader";
							if (flag6)
							{
								old_attrib[0] = int.Parse(xmlNode3.InnerText);
							}
							else
							{
								bool flag7 = xmlNode3.Name == "plusforces";
								if (flag7)
								{
									old_attrib[1] = int.Parse(xmlNode3.InnerText);
								}
								else
								{
									bool flag8 = xmlNode3.Name == "plusintelligence";
									if (flag8)
									{
										old_attrib[2] = int.Parse(xmlNode3.InnerText);
									}
								}
							}
						}
						XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/general/newattr");
						bool flag9 = xmlNode4 != null;
						if (flag9)
						{
							foreach (XmlNode xmlNode5 in xmlNode4.ChildNodes)
							{
								bool flag10 = xmlNode5.Name == "plusleader";
								if (flag10)
								{
									new_attrib[0] = int.Parse(xmlNode5.InnerText);
								}
								else
								{
									bool flag11 = xmlNode5.Name == "plusforces";
									if (flag11)
									{
										new_attrib[1] = int.Parse(xmlNode5.InnerText);
									}
									else
									{
										bool flag12 = xmlNode5.Name == "plusintelligence";
										if (flag12)
										{
											new_attrib[2] = int.Parse(xmlNode5.InnerText);
										}
									}
								}
							}
							result = 1;
						}
						else
						{
							result = 2;
						}
					}
				}
			}
			return result;
		}

		private bool washHero(ProtocolMgr protocol, ILogger logger, Hero h, int washMode, ref int[] new_attrib)
		{
			bool flag = h == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string url = "/root/general!refreshGeneral.action";
				string data = string.Format("generalId={0}&refreshModel={1}", h.Id, washMode);
				ServerResult serverResult = protocol.postXml(url, data, string.Format("为[{0}]洗属性", h.Name));
				bool flag2 = serverResult == null || !serverResult.CmdSucceed;
				if (flag2)
				{
					result = false;
				}
				else
				{
					XmlDocument cmdResult = serverResult.CmdResult;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/general");
					bool flag3 = xmlNode == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						XmlNodeList childNodes = xmlNode.ChildNodes;
						foreach (XmlNode xmlNode2 in childNodes)
						{
							bool flag4 = xmlNode2.Name == "plusleader";
							if (flag4)
							{
								new_attrib[0] = int.Parse(xmlNode2.InnerText);
							}
							else
							{
								bool flag5 = xmlNode2.Name == "plusforces";
								if (flag5)
								{
									new_attrib[1] = int.Parse(xmlNode2.InnerText);
								}
								else
								{
									bool flag6 = xmlNode2.Name == "plusintelligence";
									if (flag6)
									{
										new_attrib[2] = int.Parse(xmlNode2.InnerText);
									}
								}
							}
						}
						result = true;
					}
				}
			}
			return result;
		}

		private void confirmHeroAttrib(ProtocolMgr protocol, ILogger logger, int heroId, bool accept)
		{
			string url = "/root/general!refreshGeneralConfirm.action";
			string data = string.Format("choose={0}&generalId={1}", accept ? 1 : 0, heroId);
			ServerResult serverResult = protocol.postXml(url, data, "确认武将洗属性信息");
			bool flag = serverResult == null || !serverResult.CmdSucceed;
			if (!flag)
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				if (accept)
				{
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message");
					base.logInfo(logger, xmlNode.InnerText);
				}
				else
				{
					base.logInfo(logger, "保持原有属性不变");
				}
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

        public long handleXiZhuge(ProtocolMgr proto, ILogger logger, User user)
        {
            string url = "/root/equip!getEquip.action";
            ServerResult xml = proto.getXml(url, "武将装备");
            if (xml == null || !xml.CmdSucceed)
            {
                return next_halfhour();
            }
            XmlNodeList xmlnodelist = xml.CmdResult.SelectNodes("/results/general");
            if (xmlnodelist == null || xmlnodelist.Count == 0)
            {
                return next_halfhour();
            }
            Dictionary<int, int> generals = new Dictionary<int, int>();
            foreach (XmlNode xmlnode in xmlnodelist)
            {
                AstdLuaObject lua = new AstdLuaObject();
                lua.ParseXml(xmlnode);
                int generalid = lua.GetIntValue("general.generalid");
                int zhugeid = lua.GetIntValue("general.zhugeid");
                int zhugetimes = lua.GetIntValue("general.zhugetimes");
                if (zhugeid > 0 && zhugetimes > 0)
                {
                    generals.Add(generalid, zhugeid);
                }
            }
            if (generals.Count == 0)
            {
                return next_day();
            }
            bool finish = true;
            foreach (KeyValuePair<int, int> kvp in generals)
            {
                int result = getXiZhugeInfo(proto, logger, user, kvp.Key, kvp.Value);
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

        public int getXiZhugeInfo(ProtocolMgr proto, ILogger logger, User user, int generalId, int zhugeid)
        {
            string url = "/root/equip!getXiZhugeInfo.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = proto.postXml(url, data, "潜能");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            int maxattr = lua.GetIntValue("results.maxattr");
            int freenum = lua.GetIntValue("results.freenum");
            int curattr_lea = lua.GetIntValue("results.curattr.lea");
            int curattr_int = lua.GetIntValue("results.curattr.int");
            int curattr_str = lua.GetIntValue("results.curattr.str");
            int newattr_lea = lua.GetIntValue("results.newattr.lea");
            int newattr_int = lua.GetIntValue("results.newattr.int");
            int newattr_str = lua.GetIntValue("results.newattr.str");
            int resetattr_lea = lua.GetIntValue("results.resetattr.lea");
            int resetattr_int = lua.GetIntValue("results.resetattr.int");
            int resetattr_str = lua.GetIntValue("results.resetattr.str");
            if (newattr_lea != 0 && newattr_int != 0 && newattr_str != 0)
            {
                int total_cur = curattr_lea + curattr_int + curattr_str;
                int total_new = newattr_lea + newattr_int + newattr_str;
                int type = 2;
                if (total_new > total_cur)
                {
                    type = 1;
                }
                if (!xiZhugeConfirm(proto, logger, user, zhugeid, type, curattr_lea, curattr_int, curattr_str, newattr_lea, newattr_int, newattr_str))
                {
                    return 1;
                }
                return 0;
            }
            if (curattr_lea == maxattr && curattr_int == maxattr && curattr_str == maxattr)
            {
                return 2;
            }
            if (freenum == 0)
            {
                return 2;
            }
            return xiZhuge(proto, logger, user, zhugeid, curattr_lea, curattr_int, curattr_str);
        }

        public int xiZhuge(ProtocolMgr proto, ILogger logger, User user, int storeId, int curattr_lea, int curattr_int, int curattr_str)
        {
            string url = "/root/equip!xiZhuge.action";
            string data = string.Format("storeId={0}", storeId);
            ServerResult xml = proto.postXml(url, data, "潜能淬炼");
            if (xml == null || !xml.CmdSucceed)
            {
                return 1;
            }
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            string newattr = lua.GetStringValue("results.newattr");
            string[] attrs = newattr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (attrs.Length != 3)
            {
                return 1;
            }
            int newattr_lea = int.Parse(attrs[0]);
            int newattr_int = int.Parse(attrs[1]);
            int newattr_str = int.Parse(attrs[2]);
            int total_cur = curattr_lea + curattr_int + curattr_str;
            int total_new = newattr_lea + newattr_int + newattr_str;
            int type = 2;
            if (total_new > total_cur)
            {
                type = 1;
            }
            if (!xiZhugeConfirm(proto, logger, user, storeId, type, curattr_lea, curattr_int, curattr_str, newattr_lea, newattr_int, newattr_str))
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
        public bool xiZhugeConfirm(ProtocolMgr proto, ILogger logger, User user, int storeId, int type, int curattr_lea, int curattr_int, int curattr_str, int newattr_lea, int newattr_int, int newattr_str)
        {
            string url = "/root/equip!xiZhugeConfirm.action";
            string data = string.Format("type={0}&storeId={1}", type, storeId);
            ServerResult xml = proto.postXml(url, data, "潜能淬炼确认");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            logInfo(logger, string.Format("免费潜能淬炼,原属性:统+{0},勇+{1},智+{2},新属性:统+{3},勇+{4},智+{5},{6}",
                curattr_lea, curattr_int, curattr_str,
                newattr_lea, newattr_int, newattr_str,
                (type == 2 ? "维持" : "替换")));
            return true;
        }
	}
}
