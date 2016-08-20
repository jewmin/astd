using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.attack;
using com.lover.astd.common.model.battle;
using com.lover.common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;
using com.lover.astd.common.model.misc;

namespace com.lover.astd.common.logic
{
	public class BattleMgr : MgrBase
	{
		private string[] _formations = new string[]
		{
			"不变阵",
			"鱼鳞阵",
			"长蛇阵",
			"锋矢阵",
			"偃月阵",
			"锥形阵",
			"八卦阵",
			"七星阵",
			"雁行阵"
		};

		public BattleMgr(TimeMgr tmrMgr, ServiceFactory factory)
		{
			this._logColor = Color.Purple;
			this._tmrMgr = tmrMgr;
			this._factory = factory;
		}

		public void getWeaponEventInfo(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/server!get51EventTime.action";
			ServerResult xml = protocol.getXml(url, "获取兵器活动信息");
			bool flag = xml == null || !xml.CmdSucceed;
			if (!flag)
			{
				XmlDocument cmdResult = xml.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/message/canrecvforcetoken");
				bool flag2 = xmlNode != null;
				if (flag2)
				{
					user._battle_got_weapon_event_free_token = xmlNode.InnerText.Equals("0");
				}
				else
				{
					user._battle_got_weapon_event_free_token = false;
				}
				int num = 0;
				XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/message/forcetokennum");
				bool flag3 = xmlNode2 != null;
				if (flag3)
				{
					int.TryParse(xmlNode2.InnerText, out num);
				}
				bool flag4 = !user._battle_got_weapon_event_free_token;
				if (flag4)
				{
					url = "/root/market!recvForceToken.action";
					xml = protocol.getXml(url, "获取兵器活动免费强攻令");
					bool flag5 = xml == null;
					if (!flag5)
					{
						bool flag6 = !xml.CmdSucceed && xml.CmdError.Contains("已领取");
						if (flag6)
						{
							user._battle_got_weapon_event_free_token = true;
						}
						else
						{
							cmdResult = xml.CmdResult;
							base.logInfo(logger, string.Format("领取兵器活动免费强攻令[{0}]个", num));
							user._battle_got_weapon_event_free_token = true;
						}
					}
				}
			}
		}

		public int tryFindArmy(ProtocolMgr protocol, ILogger logger, User user, string armyid, bool onlyFirstBattle)
		{
			string url = "/root/multiBattle!getTeamInfo.action";
			string data = "armiesId=" + armyid;
			ServerResult serverResult = protocol.postXml(url, data, "获取军团征战信息");
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
					string innerText = cmdResult.SelectSingleNode("/results/armies/name").InnerText;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/currentnum");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						user._battle_current_army_id = armyid;
						result = 0;
					}
					else
					{
						XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/sceneevent");
						bool flag4 = xmlNode2 != null;
						if (flag4)
						{
							string arg = "";
							string text = "";
							XmlNode xmlNode3 = xmlNode2.SelectSingleNode("battlereport/report/describe");
							bool flag5 = xmlNode3 != null;
							if (flag5)
							{
								arg = xmlNode3.InnerText;
							}
							XmlNode xmlNode4 = xmlNode2.SelectSingleNode("battlereport/report/gains");
							bool flag6 = xmlNode4 != null;
							if (flag6)
							{
								text = xmlNode4.InnerText;
							}
							bool flag7 = text != "";
							if (flag7)
							{
								string text2 = string.Format("攻打{0}, {1}, {2}", innerText, arg, text);
								base.logInfo(logger, text2);
							}
							user._battle_current_army_id = "";
							XmlNode xmlNode5 = xmlNode2.SelectSingleNode("playerbattleinfo");
							bool flag8 = xmlNode5 != null;
							if (flag8)
							{
								user.refreshPlayerInfo(xmlNode5);
								user.addUiToQueue("global");
							}
						}
						XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results");
						XmlNodeList childNodes = xmlNode6.ChildNodes;
						string nation = user.Nation;
						string group = user.Group;
						int level = user.Level;
						int num = -1;
						foreach (XmlNode xmlNode7 in childNodes)
						{
							bool flag9 = xmlNode7.Name == "team";
							if (flag9)
							{
								XmlNodeList childNodes2 = xmlNode7.ChildNodes;
								string text3 = "";
								string teamname = "";
								string text4 = "";
								int num2 = 0;
								int num3 = 0;
								int num4 = 0;
								foreach (XmlNode xmlNode8 in childNodes2)
								{
									bool flag10 = xmlNode8.Name == "teamid";
									if (flag10)
									{
										text3 = xmlNode8.InnerText;
									}
									else
									{
										bool flag11 = xmlNode8.Name == "teamname";
										if (flag11)
										{
											teamname = xmlNode8.InnerText;
										}
										else
										{
											bool flag12 = xmlNode8.Name == "maxnum";
											if (flag12)
											{
												num3 = int.Parse(xmlNode8.InnerText);
											}
											else
											{
												bool flag13 = xmlNode8.Name == "currentnum";
												if (flag13)
												{
													num2 = int.Parse(xmlNode8.InnerText);
												}
												else
												{
													bool flag14 = xmlNode8.Name == "condition";
													if (flag14)
													{
														text4 = xmlNode8.InnerText;
													}
													else
													{
														bool flag15 = xmlNode8.Name == "firstbattle";
														if (flag15)
														{
															num4 = int.Parse(xmlNode8.InnerText);
														}
													}
												}
											}
										}
									}
								}
								bool flag16 = !(text3 == "") && num3 != num2 && (!onlyFirstBattle || num4 != 0) && (text4.IndexOf("军团") < 0 || text4.IndexOf(group) >= 0) && this.getArmyJoinLevel(text4) <= level;
								if (flag16)
								{
									num = this.tryJoinTeam(protocol, logger, text3, innerText, teamname);
									bool flag17 = num >= 0;
									if (flag17)
									{
										bool flag18 = num == 0;
										if (flag18)
										{
											user._battle_current_army_id = armyid;
										}
										result = num;
										return result;
									}
								}
							}
						}
						result = num;
					}
				}
			}
			return result;
		}

		public int tryJoinTeam(ProtocolMgr protocol, ILogger logger, string teamid, string armyname, string teamname)
		{
			string url = "/root/multiBattle!joinTeam.action";
			string data = "teamId=" + teamid;
			string text = string.Format("攻打[ {0} ], 加入[ {1} ]队伍", armyname, teamname);
			ServerResult serverResult = protocol.postXml(url, data, text);
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
					base.logInfo(logger, text);
					result = 0;
				}
				else
				{
					string cmdError = serverResult.CmdError;
					bool flag2 = cmdError == null;
					if (flag2)
					{
						result = 1;
					}
					else
					{
						bool flag3 = cmdError.IndexOf("军令还没有冷却") >= 0;
						if (flag3)
						{
							result = 2;
						}
						else
						{
							bool flag4 = cmdError.IndexOf("没有可用的军令") >= 0;
							if (flag4)
							{
								result = 3;
							}
							else
							{
								bool flag5 = cmdError.IndexOf("不能攻击") >= 0;
								if (flag5)
								{
									result = 4;
								}
								else
								{
									bool flag6 = cmdError.IndexOf("满") >= 0;
									if (flag6)
									{
										result = -1;
									}
									else
									{
										result = 5;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		public int attackNpc(ProtocolMgr protocol, ILogger logger, User user, string npcid, bool force, string formation)
		{
			string url = "/root/battle!battleArmy.action";
			if (force)
			{
				url = "/root/battle!forceBattleArmy.action";
			}
			string data = "armyId=" + npcid;
			ServerResult serverResult = protocol.postXml(url, data, "攻打Npc");
			int result;
			if (serverResult == null)
			{
				result = 1;
			}
			else if (serverResult.CmdSucceed)
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/playerbattleinfo");
				if (xmlNode != null)
				{
					user.refreshPlayerInfo(xmlNode);
					user.addUiToQueue("global");
				}
				XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/battlereport");
				XmlNodeList childNodes = xmlNode2.ChildNodes;
				string text = "";
				string text2 = "";
				string text3 = "";
				foreach (XmlNode xmlNode3 in childNodes)
				{
					if (xmlNode3.Name == "attlost")
					{
						text = xmlNode3.InnerText;
					}
					else if (xmlNode3.Name == "deflost")
					{
						text2 = xmlNode3.InnerText;
					}
					else if (xmlNode3.Name == "message")
					{
						text3 = xmlNode3.InnerText;
					}
				}
				string text4 = string.Format("{0}NPC, {1}, 你损失兵力{2}, 敌方损失兵力{3} ", force ? "强征" : "征战", text3, text, text2);
				XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/items/list");
				if (xmlNode4.HasChildNodes)
				{
					XmlNodeList childNodes2 = xmlNode4.ChildNodes;
					text4 += ", 获得";
					foreach (XmlNode xmlNode5 in childNodes2)
					{
						if (xmlNode5.Name == "value")
						{
							string[] array = this.translateGainItem(xmlNode5.InnerText);
							string text5 = array[0];
							string text6 = array[1];
							if (text5 == null || text5 == "")
							{
								text5 = xmlNode5.InnerText;
							}
							text4 += text5 + "  ";
						}
					}
				}
				base.logInfo(logger, text4);
				result = 0;
			}
			else
			{
				if (serverResult.CmdError.IndexOf("还没有冷却") >= 0)
				{
					result = 2;
				}
				else
				{
					result = 3;
				}
			}
			return result;
		}

		public string getFormation(ProtocolMgr protocol, ILogger logger)
		{
			string text = this._formations[0];
			string url = "/root/general!formation.action";
			ServerResult xml = protocol.getXml(url, "获取阵型");
			bool flag = xml == null || !xml.CmdSucceed;
			string result;
			if (flag)
			{
				result = text;
			}
			else
			{
				bool flag2 = !xml.CmdSucceed;
				if (flag2)
				{
					result = text;
				}
				else
				{
					XmlDocument cmdResult = xml.CmdResult;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/formation/formationid");
					bool flag3 = xmlNode == null;
					if (flag3)
					{
						result = text;
					}
					else
					{
						int num = 0;
						int.TryParse(xmlNode.InnerText, out num);
						bool flag4 = num == 0;
						if (flag4)
						{
							result = text;
						}
						else
						{
							num /= 20;
							bool flag5 = num > this._formations.Length;
							if (flag5)
							{
								result = text;
							}
							else
							{
								result = this._formations[num];
							}
						}
					}
				}
			}
			return result;
		}

		public bool changeFormation(ProtocolMgr protocol, ILogger logger, string formation)
		{
            int formationId = 0;
            for (int i = 0; i < this._formations.Length; i++)
            {
                if (this._formations[i].Equals(formation))
                {
                    formationId = i;
                    break;
                }
            }
            if (formationId == 0)
            {
                return true;
            }
            else
            {
                formationId *= 20;
                string url = "/root/general!saveDefaultFormation.action";
                string data = string.Format("formationId={0}", formationId);
                string text = string.Format("变换阵型为{0}", formation);
                ServerResult xml = protocol.postXml(url, data, text);
                if (xml == null || !xml.CmdSucceed)
                {
                    return false;
                }
                else
                {
                    logInfo(logger, text);
                    return true;
                }
            }
		}

		public List<int> getAllPowerList(ProtocolMgr protocol, ILogger logger, User user, List<int> nowPowerIds)
		{
			int powerId = nowPowerIds[nowPowerIds.Count - 1];
			int num = 0;
			List<int> powerList = this.getPowerList(protocol, logger, user, powerId, out num);
			int num2;
			for (int i = 0; i < powerList.Count; i = num2 + 1)
			{
				bool flag = !nowPowerIds.Contains(powerList[i]);
				if (flag)
				{
					nowPowerIds.Add(powerList[i]);
				}
				num2 = i;
			}
			bool flag2 = num == 0;
			List<int> result;
			if (flag2)
			{
				result = nowPowerIds;
			}
			else
			{
				nowPowerIds.Add(num);
				result = this.getAllPowerList(protocol, logger, user, nowPowerIds);
			}
			return result;
		}

		public List<int> getPowerList(ProtocolMgr protocol, ILogger logger, User user, int powerId, out int nextId)
		{
			nextId = 0;
			List<int> list = new List<int>();
			string url = "/root/battle!getPowerList.action";
			ServerResult serverResult = protocol.postXml(url, "powerId=" + powerId, "获取征战军团信息");
			bool flag = serverResult == null || !serverResult.CmdSucceed;
			List<int> result;
			if (flag)
			{
				result = list;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/power");
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					XmlNodeList childNodes = xmlNode.ChildNodes;
					bool flag2 = false;
					int item = 0;
					int num = 0;
					foreach (XmlNode xmlNode2 in childNodes)
					{
						bool flag3 = xmlNode2.Name == "attackable";
						if (flag3)
						{
							flag2 = (xmlNode2.InnerText == "1");
						}
						else
						{
							bool flag4 = xmlNode2.Name == "ratio";
							if (flag4)
							{
								int.TryParse(xmlNode2.InnerText, out num);
							}
							else
							{
								bool flag5 = xmlNode2.Name == "powerid";
								if (flag5)
								{
									int.TryParse(xmlNode2.InnerText, out item);
								}
							}
						}
					}
					bool flag6 = flag2 && num == 100;
					if (flag6)
					{
						list.Add(item);
					}
				}
				XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/nextid");
				bool flag7 = xmlNode3 != null;
				if (flag7)
				{
					int.TryParse(xmlNode3.InnerText, out nextId);
				}
				result = list;
			}
			return result;
		}

		public void getAllPowerInfo(ProtocolMgr protocol, ILogger logger, User user)
		{
			for (int i = 90; i <= 300; i++)
			{
				bool flag = true;
				this.getPowerInfo(protocol, logger, user, i, out flag);
				if (!flag && i > 30)
				{
					break;
				}
			}
		}

		public List<int> getPowerInfo(ProtocolMgr protocol, ILogger logger, User user, int powerId, out bool success)
		{
			success = true;
			List<int> list = new List<int>();
			string url = "/root/battle!getPowerInfo.action";
			ServerResult serverResult = protocol.postXml(url, "powerId=" + powerId, "获取征战信息");
			List<int> result;
			if (serverResult == null || !serverResult.CmdSucceed)
			{
				success = false;
				result = list;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/power");
				int armyId = 0;
				string arg = "";
				if (xmlNode != null)
				{
					XmlNode xmlNode2 = xmlNode.SelectSingleNode("powerid");
					if (xmlNode2 != null)
					{
						armyId = int.Parse(xmlNode2.InnerText);
					}
					XmlNode xmlNode3 = xmlNode.SelectSingleNode("powername");
					if (xmlNode3 != null)
					{
						arg = xmlNode3.InnerText;
					}
					XmlNode xmlNode4 = xmlNode.SelectSingleNode("nextpower");
					if (xmlNode4 != null && xmlNode4.HasChildNodes)
					{
						XmlNodeList childNodes = xmlNode4.ChildNodes;
						int item = 0;
						int num = 0;
						foreach (XmlNode xmlNode5 in childNodes)
						{
							if (xmlNode5.HasChildNodes)
							{
								item = 0;
								num = 0;
								foreach (XmlNode xmlNode6 in xmlNode5.ChildNodes)
								{
									if (xmlNode6.Name == "powerid")
									{
										item = int.Parse(xmlNode6.InnerText);
									}
									else if (xmlNode6.Name == "attackable")
									{
										num = int.Parse(xmlNode6.InnerText);
									}
								}
								if (num == 1)
								{
									list.Add(item);
								}
							}
						}
					}
				}
				XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/army");
				foreach (XmlNode xmlNode7 in xmlNodeList)
				{
					XmlNodeList childNodes2 = xmlNode7.ChildNodes;
					int id = 0;
					int num2 = 0;
					int num3 = 0;
					string arg2 = "";
					string text = "";
					string itemColor = "";
					foreach (XmlNode xmlNode8 in childNodes2)
					{
						if (xmlNode8.Name == "armyid")
						{
							id = int.Parse(xmlNode8.InnerText);
						}
						else if (xmlNode8.Name == "armyname")
						{
							arg2 = xmlNode8.InnerText;
						}
						else if (xmlNode8.Name == "itemname")
						{
							string[] array = this.translateItem(xmlNode8.InnerText);
							text = array[0];
							itemColor = array[1];
						}
						else if (xmlNode8.Name == "type")
						{
							num2 = int.Parse(xmlNode8.InnerText);
						}
						else if (xmlNode8.Name == "attackable")
						{
							num3 = int.Parse(xmlNode8.InnerText);
						}
					}
					if (num3 != 0 && ((text == "" && num2 == 5) || text != ""))
					{
						Npc npc = new Npc();
						npc.Id = id;
						npc.ArmyId = armyId;
						npc.ItemName = text;
						npc.Name = string.Format("{0}({1})", arg2, arg);
						npc.Type = num2;
						npc.ItemColor = itemColor;
						if (npc.Type == 5)
						{
							this.addArmies(user, npc);
						}
						else
						{
							this.addNpcs(user, npc);
						}
					}
				}
				result = list;
			}
			return result;
		}

		private void addArmies(User user, Npc army)
		{
			bool flag = false;
			int i = 0;
			int count = user._all_armys.Count;
			while (i < count)
			{
				bool flag2 = user._all_armys[i].Id == army.Id;
				if (flag2)
				{
					flag = true;
					break;
				}
				int num = i;
				i = num + 1;
			}
			bool flag3 = !flag;
			if (flag3)
			{
				bool flag4 = user._battle_max_army_id < army.Id;
				if (flag4)
				{
					user._battle_max_army_id = army.Id;
				}
				user._all_armys.Insert(0, army);
			}
		}

		private void addNpcs(User user, Npc npc)
		{
			bool flag = false;
			int i = 0;
			int count = user._all_npcs.Count;
			while (i < count)
			{
				bool flag2 = user._all_npcs[i].Id == npc.Id;
				if (flag2)
				{
					flag = true;
					break;
				}
				int num = i;
				i = num + 1;
			}
			bool flag3 = !flag;
			if (flag3)
			{
				user._all_npcs.Insert(0, npc);
			}
		}

		private string[] translateItem(string rawtext)
		{
			string[] array = new string[2];
			Regex regex = new Regex("(#\\w{6})'>(.+)</font>(.+)");
			Match match = regex.Match(rawtext);
			GroupCollection groups = match.Groups;
			array[1] = groups[1].Value;
			array[0] = groups[2].Value + groups[3].Value;
			return array;
		}

		private string[] translateGainItem(string rawtext)
		{
			string[] array = new string[2];
			bool flag = rawtext == null || rawtext == "";
			string[] result;
			if (flag)
			{
				result = array;
			}
			else
			{
				rawtext = rawtext.Replace("\r", "").Replace("\n", "");
				Regex regex = new Regex("([^<]*?).*?(#\\w{6})'>(.+)</font>");
				Match match = regex.Match(rawtext);
				GroupCollection groups = match.Groups;
				array[1] = groups[2].Value;
				array[0] = groups[3].Value;
				result = array;
			}
			return result;
		}

		private int getArmyJoinLevel(string rawtext)
		{
			Regex regex = new Regex(".*(\\d+)级.*");
			Match match = regex.Match(rawtext);
			GroupCollection groups = match.Groups;
			return int.Parse(groups[1].Value);
		}

		public void getBattleInfo(ProtocolMgr protocol, ILogger logger, User user, int gold_available, bool do_event, string eventId)
		{
			string url = "/root/battle.action";
			ServerResult xml = protocol.getXml(url, "获取战争信息");
			if (xml == null || !xml.CmdSucceed)
			{
				return;
			}
			XmlDocument cmdResult = xml.CmdResult;
			XmlNode xmlNode = cmdResult.SelectSingleNode("/results/freeattnum");
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.InnerText, out user._battle_free_force_token);
			}
			else
			{
				user._battle_free_force_token = 0;
			}
			if (do_event)
			{
				int eventid = 0;
				XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/battleevent");
				if (xmlNode2 == null || !xmlNode2.HasChildNodes)
				{
					return;
				}
				if (xmlNode2 != null)
				{
					XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/battleevent/state");
					int state = 0;
					if (xmlNode3 != null)
					{
						int.TryParse(xmlNode3.InnerText, out state);
					}
					if (state == 2)
					{
						this.recvBattleEventReward(protocol, logger);
					}
					else if (state == 0)
					{
						int repeatgold = 5;
						XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/battleevent/repeatgold");
						if (xmlNode4 != null)
						{
							int.TryParse(xmlNode4.InnerText, out repeatgold);
						}
						int[] array = new int[3];
						XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/battleevent/event");
						foreach (XmlNode xmlNode5 in xmlNodeList)
						{
							int id = 0;
							int free = 0;
							XmlNode xmlNode6 = xmlNode5.SelectSingleNode("id");
							if (xmlNode6 != null)
							{
								int.TryParse(xmlNode6.InnerText, out id);
							}
							XmlNode xmlNode7 = xmlNode5.SelectSingleNode("free");
							if (xmlNode7 != null)
							{
								int.TryParse(xmlNode7.InnerText, out free);
							}
							array[id - 1] = free;
						}
						for (int i = 1; i <= 3 && eventid <= 0; i++)
						{
							if (eventId.Contains(i.ToString()))
							{
								if (array[i - 1] == 0)
								{
									if (eventId.Contains((i + 3).ToString()) && gold_available >= repeatgold)
									{
										eventid = i;
									}
								}
								else
								{
									eventid = i;
								}
							}
						}
						if (eventid == 0)
						{
							return;
						}
						this.chooseBattleEvent(protocol, logger, eventid);
					}
					else
					{
						XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/battleevent/eventid");
						if (xmlNode8 != null)
						{
							int.TryParse(xmlNode8.InnerText, out eventid);
						}
					}
					if (eventid == 1)
					{
						int times = 10;
						while (user.Token > 0)
						{
							if (times <= 0)
							{
								break;
							}
							this.doBattleEvent(protocol, logger, out times);
							if (times == 0)
							{
								this.recvBattleEventReward(protocol, logger);
							}
						}
					}
					else if (eventid == 2)
					{
						int handletime = 10000;
						XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/battleevent/handletime");
						if (xmlNode9 != null)
						{
							int.TryParse(xmlNode9.InnerText, out handletime);
						}
						if (handletime <= 0)
						{
							this.recvBattleEventReward(protocol, logger);
						}
					}
					else if (eventid == 3)
					{
						XmlNodeList xmlNodeList2 = cmdResult.SelectNodes("/results/battleevent/armys");
						foreach (XmlNode xmlNode10 in xmlNodeList2)
						{
							int armyid = 0;
							int isatt = 0;
							XmlNode xmlNode11 = xmlNode10.SelectSingleNode("armyid");
							if (xmlNode11 != null)
							{
								int.TryParse(xmlNode11.InnerText, out armyid);
							}
							XmlNode xmlNode12 = xmlNode10.SelectSingleNode("isatt");
							if (xmlNode12 != null)
							{
								int.TryParse(xmlNode12.InnerText, out isatt);
							}
							if (armyid > 0 && isatt == 0)
							{
								if (user.Token <= 0)
								{
									return;
								}
								base.logInfo(logger, "征战事件攻打NPC");
								if (this.attackNpc(protocol, logger, user, string.Concat(armyid), false, "不变阵") > 0)
								{
									return;
								}
							}
						}
						this.recvBattleEventReward(protocol, logger);
					}
				}
			}
		}

		public bool chooseBattleEvent(ProtocolMgr protocol, ILogger logger, int eventId)
		{
			string url = "/root/battle!chooseBattleEvent.action";
			string data = "eventId=" + eventId;
			string str = "";
			bool flag = eventId < 1 || eventId > 3;
			if (flag)
			{
				eventId = 1;
			}
			bool flag2 = eventId == 1;
			if (flag2)
			{
				str = "宝石事件";
			}
			else
			{
				bool flag3 = eventId == 2;
				if (flag3)
				{
					str = "玉石事件";
				}
				else
				{
					bool flag4 = eventId == 3;
					if (flag4)
					{
						str = "兵器事件";
					}
				}
			}
			ServerResult serverResult = protocol.postXml(url, data, "选择征战事件:" + str);
			bool flag5 = serverResult == null || !serverResult.CmdSucceed;
			bool result;
			if (flag5)
			{
				result = false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				base.logInfo(logger, "选择征战事件:" + str);
				result = true;
			}
			return result;
		}

		public void doBattleEvent(ProtocolMgr protocol, ILogger logger, out int remains)
		{
			remains = 0;
			string url = "/root/battle!doBattleEvent.action";
			ServerResult xml = protocol.getXml(url, "进行征战事件(宝石)");
			if (xml == null || !xml.CmdSucceed)
			{
				return;
			}
			XmlDocument cmdResult = xml.CmdResult;
			XmlNode xmlNode = cmdResult.SelectSingleNode("/results/battleevent/process");
			int num = 0;
			int num2 = 10;
			string text;
			if (xmlNode != null)
			{
				text = xmlNode.InnerText;
				int num3 = text.IndexOf('/');
				if (num3 >= 0)
				{
					int.TryParse(text.Substring(0, num3), out num);
					int.TryParse(text.Substring(num3 + 1), out num2);
					remains = num2 - num;
				}
			}
			else
			{
				text = "完毕";
			}
			base.logInfo(logger, "进行征战事件(宝石): " + text);
		}

		public void recvBattleEventReward(ProtocolMgr protocol, ILogger logger)
		{
			string url = "/root/battle!recvBattleEventReward.action";
			ServerResult xml = protocol.getXml(url, "获取征战事件奖励");
			if (xml == null || !xml.CmdSucceed)
			{
				return;
			}
			XmlDocument cmdResult = xml.CmdResult;
			string text = "获取征战事件奖励: ";
			XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/rewardinfo/reward");
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				int num = int.Parse(xmlNode.SelectSingleNode("type").InnerText);
				int num2 = int.Parse(xmlNode.SelectSingleNode("num").InnerText);
				if (num == 2)
				{
					text += string.Format("玉石+{0}", num2);
				}
				else if (num == 5)
				{
					text += string.Format("宝石+{0}", num2);
				}
				else if (num == 7)
				{
					string innerText = xmlNode.SelectSingleNode("itemname").InnerText;
					text += string.Format("{0}碎片*{1}", innerText, num2);
				}
			}
			base.logInfo(logger, text);
		}

		public void getNewAreaInfo(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/world!getNewArea.action";
			ServerResult xml = protocol.getXml(url, "新世界世界界面");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/targetid");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out user._attack_cityevent_targetid);
            }
            else
            {
                user._attack_cityevent_targetid = 0;
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/targetareaid");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out user._attack_cityevent_target_areaid);
            }
            else
            {
                user._attack_cityevent_target_areaid = 0;
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/targetscopeid");
            if (xmlNode3 != null)
            {
                int.TryParse(xmlNode3.InnerText, out user._attack_cityevent_target_scopeid);
            }
            else
            {
                user._attack_cityevent_target_scopeid = 0;
            }
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/tranfercd");
            if (xmlNode4 != null)
            {
                int.TryParse(xmlNode4.InnerText, out user._attack_transfer_cd);
            }
            else
            {
                user._attack_transfer_cd = 0;
            }
            XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/battlescore");
            if (xmlNode5 != null)
            {
                int.TryParse(xmlNode5.InnerText, out user._attack_battleScore);
            }
            xmlNode5 = cmdResult.SelectSingleNode("/results/daoju/flagnum");
            if (xmlNode5 != null)
            {
                int.TryParse(xmlNode5.InnerText, out user._attack_daojuFlag);
            }
            else
            {
                user._attack_daojuFlag = 0;
            }
            xmlNode5 = cmdResult.SelectSingleNode("/results/daoju/jinnum");
            if (xmlNode5 != null)
            {
                int.TryParse(xmlNode5.InnerText, out user._attack_daojuJin);
            }
            else
            {
                user._attack_daojuJin = 0;
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/newarea");
            user._attack_spy_city = "";
            user._attack_all_areas.Clear();
            foreach (XmlNode xmlNode6 in xmlNodeList)
            {
                XmlNodeList childNodes = xmlNode6.ChildNodes;
                AreaInfo areaInfo = new AreaInfo();
                foreach (XmlNode xmlNode7 in childNodes)
                {
                    if (xmlNode7.Name == "areaid")
                    {
                        areaInfo.areaid = int.Parse(xmlNode7.InnerText);
                    }
                    else if (xmlNode7.Name == "nation")
                    {
                        areaInfo.nation = int.Parse(xmlNode7.InnerText);
                    }
                    else if (xmlNode7.Name == "areaname")
                    {
                        areaInfo.areaname = xmlNode7.InnerText;
                    }
                    else if (xmlNode7.Name == "transferable")
                    {
                        areaInfo.transferable = (xmlNode7.InnerText == "1");
                    }
                    else if (xmlNode7.Name == "enterable")
                    {
                        areaInfo.enterable = (xmlNode7.InnerText == "1");
                    }
                    else if (xmlNode7.Name == "isselfarea")
                    {
                        areaInfo.isselfarea = (xmlNode7.InnerText == "1");
                    }
                    else if (xmlNode7.Name == "scopecount")
                    {
                        areaInfo.scopecount = int.Parse(xmlNode7.InnerText);
                    }
                    else if (xmlNode7.Name == "ziyuan")
                    {
                        areaInfo.ziyuan = int.Parse(xmlNode7.InnerText);
                    }
                }
                if (areaInfo.ziyuan == 100)
                {
                    user._attack_spy_city = areaInfo.areaname;
                }
                if (areaInfo.isselfarea)
                {
                    user._attack_selfCityId = areaInfo.areaid;
                }
                if (areaInfo.areaid > 0)
                {
                    user.setNewAreaCityNation(areaInfo.areaid, areaInfo.nation);
                    user._attack_all_areas.Add(areaInfo);
                }
            }
		}

        public void getBattleScoreInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/world!getBattleRankingInfo.action";
            ServerResult xml = protocol.getXml(url, "新世界战绩界面");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/score");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out user._attack_battleScore);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/box");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out user._attack_battleScore_box);
            }
            int lastrankingreward = 0;
            xmlNode = cmdResult.SelectSingleNode("/results/lastrankingreward");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out lastrankingreward);
            }
            int getlast = 0;
            xmlNode = cmdResult.SelectSingleNode("/results/getlast");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out getlast);
            }
            if (lastrankingreward > 0)
            {
                if (getlast == 1)
                {
                    user._attack_last_awardGot = 2;
                }
                else
                {
                    user._attack_last_awardGot = 1;
                }
            }
            else
            {
                user._attack_last_awardGot = 0;
            }
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/scorerewardinfo/rinfo");
            foreach (XmlNode xmlNode2 in xmlNodeList)
            {
                XmlNodeList childNodes = xmlNode2.ChildNodes;
                bool cangetflag = false;
                bool getflag = false;
                int id = 0;
                foreach (XmlNode xmlNode3 in childNodes)
                {
                    if (xmlNode3.Name == "id")
                    {
                        id = int.Parse(xmlNode3.InnerText);
                    }
                    else if (xmlNode3.Name == "get")
                    {
                        getflag = (xmlNode3.InnerText == "1");
                    }
                    else if (xmlNode3.Name == "canget")
                    {
                        cangetflag = (xmlNode3.InnerText == "1");
                    }
                }
                if (cangetflag)
                {
                    if (getflag)
                    {
                        user._attack_battleScore_awardGot[id - 1] = 2;
                    }
                    else
                    {
                        user._attack_battleScore_awardGot[id - 1] = 1;
                    }
                }
                else
                {
                    user._attack_battleScore_awardGot[id - 1] = 0;
                }
            }
        }

		public void getBattleScoreAward(ProtocolMgr protocol, ILogger logger, int position)
		{
			string url = "/root/world!getBattleScoreReward.action";
			string data = "pos=" + position;
			ServerResult serverResult = protocol.postXml(url, data, "新世界获取战绩宝箱");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            int baoshi = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out baoshi);
            }
            base.logInfo(logger, string.Format("领取战绩奖励, 宝石+{0}", baoshi));
		}

        public void openScoreBox(ProtocolMgr protocol, ILogger logger, int box)
        {
            while (box > 0)
            {
                box--;
                string url = "/root/world!openScoreBox.action";
                ServerResult serverResult = protocol.getXml(url, "新世界获取战绩宝箱");
                if (serverResult == null || !serverResult.CmdSucceed)
                {
                    return;
                }
                XmlDocument cmdResult = serverResult.CmdResult;
                RewardInfo reward = new RewardInfo();
                reward.handleXmlNode(cmdResult.SelectSingleNode("/results/rewardinfo"));
                base.logInfo(logger, string.Format("领取战绩宝箱, {0}", reward.ToString()));
            }
        }

        public void getRankAward(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/world!getBattleRankReward.action";
            ServerResult serverResult = protocol.getXml(url, "新世界获取战绩排名奖励");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            int baoshi = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out baoshi);
            }
            base.logInfo(logger, string.Format("领取排名奖励, 宝石+{0}", baoshi));
        }

		public void getUserTokens(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/world!getNewAreaToken.action";
			ServerResult xml = protocol.getXml(url, "新世界获取个人令");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            user._attack_user_tokens.Clear();
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/tokenlist");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                UserToken userToken = new UserToken();
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    if (xmlNode2.Name == "id")
                    {
                        userToken.id = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "tokenid")
                    {
                        userToken.tokenid = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "name")
                    {
                        userToken.name = xmlNode2.InnerText;
                    }
                    else if (xmlNode2.Name == "level")
                    {
                        userToken.level = int.Parse(xmlNode2.InnerText);
                    }
                }
                user._attack_user_tokens.Add(userToken);
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/playercityevent/canreward");
            if (xmlNode3 != null)
            {
                int.TryParse(xmlNode3.InnerText, out user._attack_can_reward_cityevent);
            }
            else
            {
                user._attack_can_reward_cityevent = 0;
            }
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/playercityevent/remaintimes");
            if (xmlNode4 != null)
            {
                int.TryParse(xmlNode4.InnerText, out user._attack_remain_cityevent_num);
            }
            else
            {
                user._attack_remain_cityevent_num = 0;
            }
            XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/playercityevent/cityevent/name");
            if (xmlNode5 != null)
            {
                user._attack_current_cityevent_name = xmlNode5.InnerText;
            }
            XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/playercityevent/cdtime");
            if (xmlNode6 != null)
            {
                int.TryParse(xmlNode6.InnerText, out user._attack_current_cityevent_cdtime);
            }
		}

		private bool useInspireToken(ProtocolMgr protocol, ILogger logger, int newTokenId)
		{
			string url = "/root/world!useInspireToken.action";
			string data = "newTokenId=" + newTokenId;
			ServerResult serverResult = protocol.postXml(url, data, "使用鼓舞令");
			if (serverResult == null || !serverResult.CmdSucceed)
			{
				return false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
                int tokenlevel = 0;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tokenlevel");
				if (xmlNode != null)
				{
                    int.TryParse(xmlNode.InnerText, out tokenlevel);
				}
                float effect = 0f;
				XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/effect");
				if (xmlNode2 != null)
				{
                    float.TryParse(xmlNode2.InnerText, out effect);
				}
                base.logInfo(logger, string.Format("使用[{0}级鼓舞令], 成功, 攻防+{1}%, 持续24小时", tokenlevel, effect));
				return true;
			}
		}

		private bool useConstructToken(ProtocolMgr protocol, ILogger logger, int newTokenId)
		{
			string url = "/root/world!useConstuctToken.action";
			string data = "newTokenId=" + newTokenId;
			ServerResult serverResult = protocol.postXml(url, data, "使用建造令");
			if (serverResult == null || !serverResult.CmdSucceed)
			{
				return false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
                int tokenlevel = 0;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tokenlevel");
				if (xmlNode != null)
				{
                    int.TryParse(xmlNode.InnerText, out tokenlevel);
				}
                float effect = 0f;
				XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/effect");
				if (xmlNode2 != null)
				{
                    float.TryParse(xmlNode2.InnerText, out effect);
				}
                base.logInfo(logger, string.Format("使用[{0}级建造令], 成功, 城防+{1}", tokenlevel, effect));
				return true;
			}
		}

		private bool useScoreToken(ProtocolMgr protocol, ILogger logger, int newTokenId)
		{
			string url = "/root/world!useScoreToken.action";
			string data = "newTokenId=" + newTokenId;
			ServerResult serverResult = protocol.postXml(url, data, "使用战绩令");
			if (serverResult == null || !serverResult.CmdSucceed)
			{
				return false;
			}
			else
			{
				base.logInfo(logger, "使用战绩令");
				return true;
			}
		}

		public void useToken(ProtocolMgr protocol, ILogger logger, User user, int target_min_battle_score)
		{
            if (user._attack_user_tokens == null || user._attack_user_tokens.Count == 0)
            {
                return;
            }
            foreach (UserToken current in user._attack_user_tokens)
            {
                if (current.tokenid == 1)
                {
                    if (user._attack_maxCityHp - user._attack_cityHp > 50)
                    {
                        this.useConstructToken(protocol, logger, current.id);
                    }
                }
                else if (current.tokenid == 4)
                {
                    bool use_flag = false;
                    if (user._attack_inspiredState == 0)
                    {
                        use_flag = true;
                    }
                    else if (user._attack_inspiredEffect < (float)current.level)
                    {
                        use_flag = true;
                    }
                    if (use_flag)
                    {
                        this.useInspireToken(protocol, logger, current.id);
                    }
                }
                else if (current.tokenid == 8 && user._attack_scoreTokencd == 0 && user._attack_cityHpRecoverCd == 0 && this.useScoreToken(protocol, logger, current.id))
                {
                    user._attack_scoreTokencd = (10 + current.level) * 60 * 1000;
                }
            }
		}

        public AreaInfo getNextMoveArea(ProtocolMgr protocol, ILogger logger, User user, AreaInfo target_area, bool is_doing_cityevent, bool is_doing_nation)
		{
			AreaInfo areaById = user.getAreaById(user._attack_selfCityId);
			AreaInfo areaInfo = user._get_attack_move_target();
			AreaInfo result;
			if (areaInfo != null && areaInfo.areaid == target_area.areaid && user._attack_move_path.Count > 0)
			{
				AreaInfo areaInfo2 = user._attack_move_path[0];
				string text = string.Format("移动目标为[{0}],当前在[{1}], 途经城市: ", target_area.areaname, areaById.areaname);
				foreach (AreaInfo current in user._attack_move_path)
				{
					if (current.areaid != areaById.areaid && current.areaid != target_area.areaid)
					{
						text += string.Format("[{0}],", current.areaname);
					}
				}
				if (areaInfo2.areaid == areaById.areaid)
				{
					areaInfo2 = null;
					text = string.Format("移动目标为[{0}], 当前已在该城市, 不移动", areaById.areaname);
				}
				base.logInfo(logger, text);
				return areaInfo2;
			}
			else
			{
				List<AreaInfo> list = this.findAttackPath(user, user._attack_selfCityId, target_area.areaid, is_doing_cityevent, is_doing_nation);
				if (list.Count == 0)
				{
					base.logInfo(logger, string.Format("设定移动目标为[{0}], 该目标从当前城市无法到达, 重新计算最近城市", target_area.areaname));
					int num = 0;
					int num2 = (areaById.areaid - 101) / 6;
					int num3 = (areaById.areaid - 101) % 6;
					int num4 = (target_area.areaid - 101) / 6;
					int num5 = (target_area.areaid - 101) % 6;
					bool flag5 = num2 < num4;
					bool flag6 = num3 < num5;
					int num6 = num4;
					int num7 = num5;
					while (list.Count == 0 && num < 30 && (num6 != num2 || num7 != num3))
					{
						int num8;
						if (num % 2 == 1)
						{
							if (num6 != num2)
							{
								if (flag5)
								{
									num8 = num6;
									num6 = num8 - 1;
								}
								else
								{
									num8 = num6;
									num6 = num8 + 1;
								}
							}
							else
							{
								if (flag6)
								{
									num8 = num7;
									num7 = num8 - 1;
								}
								else
								{
									num8 = num7;
									num7 = num8 + 1;
								}
							}
						}
						else
						{
							if (num7 != num3)
							{
								if (flag6)
								{
									num8 = num7;
									num7 = num8 - 1;
								}
								else
								{
									num8 = num7;
									num7 = num8 + 1;
								}
							}
							else
							{
								if (flag5)
								{
									num8 = num6;
									num6 = num8 - 1;
								}
								else
								{
									num8 = num6;
									num6 = num8 + 1;
								}
							}
						}
						num8 = num;
						num = num8 + 1;
						int num9 = 101 + num6 * 6 + num7;
						list = this.findAttackPath(user, areaById.areaid, num9, is_doing_cityevent, is_doing_nation);
						if (list.Count > 0)
						{
							target_area = user.getAreaById(num9);
							break;
						}
					}
					if (list.Count > 0)
					{
						base.logInfo(logger, string.Format("重新计算移动城市为[{0}], 开始计算路径", target_area.areaname));
					}
				}
				if (list.Count > 0)
				{
					AreaInfo areaInfo3 = null;
					string text2 = string.Format("移动目标为[{0}],当前在[{1}], 途经城市: ", target_area.areaname, areaById.areaname);
					if (target_area.areaid == areaById.areaid)
					{
						areaInfo3 = null;
						text2 = string.Format("移动目标为[{0}], 当前已在该城市, 不移动", areaById.areaname);
					}
					else
					{
						foreach (AreaInfo current2 in list)
						{
							if (current2.areaid != areaById.areaid && current2.areaid != target_area.areaid)
							{
								if (areaInfo3 == null)
								{
									areaInfo3 = current2;
								}
								text2 += string.Format("[{0}],", current2.areaname);
							}
						}
						if (areaInfo3 == null)
						{
							areaInfo3 = target_area;
						}
						if (areaInfo3.areaid == areaById.areaid)
						{
							areaInfo3 = null;
							text2 = string.Format("移动目标为[{0}], 当前已在该城市, 不移动", areaById.areaname);
						}
					}
					base.logInfo(logger, text2);
					user._attack_move_path.Clear();
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						if (list[i].areaid != user._attack_selfCityId && list[i].areaid != areaInfo3.areaid)
						{
							user._attack_move_path.Add(list[i]);
						}
						int num8 = i;
						i = num8 + 1;
					}
					result = areaInfo3;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

        public List<AreaInfo> findAttackPath(User user, int selfAreaId, int targetAreaId, bool is_doing_cityevent = false, bool is_doing_nation = false)
		{
			List<AreaInfo> list = new List<AreaInfo>();
			int x = (selfAreaId - 101) / 6;
			int y = (selfAreaId - 101) % 6;
			int x2 = (targetAreaId - 101) / 6;
			int y2 = (targetAreaId - 101) % 6;
			int[,] array = new int[6, 6];
			int num;
			for (int i = 0; i < 6; i = num + 1)
			{
				for (int j = 0; j < 6; j = num + 1)
				{
					array[i, j] = -1;
					num = j;
				}
				num = i;
			}
			int nationInt = user.NationInt;
			foreach (AreaInfo current in user._attack_all_areas)
			{
				int areaid = current.areaid;
				if (areaid != 112 && areaid != 113 && areaid != 134)
				{
					int num2 = (areaid - 101) / 6;
					int num3 = (areaid - 101) % 6;
                    if (is_doing_nation)
                    {
                        array[num2, num3] = 1;
                    }
                    else if (is_doing_cityevent)
					{
						array[num2, num3] = 1;
					}
					else
					{
						if (current.nation == nationInt || current.nation == 0)
						{
							array[num2, num3] = 1;
						}
						else
						{
							array[num2, num3] = -1;
						}
					}
				}
			}
			List<Point> aStarPath = CommonUtils.getAStarPath(array, new Point(x, y), new Point(x2, y2));
			foreach (Point current2 in aStarPath)
			{
				int areaId = 101 + current2.X * 6 + current2.Y;
				AreaInfo areaById = user.getAreaById(areaId);
				if (areaById != null)
				{
					list.Add(areaById);
				}
			}
			return list;
		}

        /// <summary>
        /// 获取相邻的地区
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public List<AreaInfo> getNearAreas(User user)
		{
			int attack_selfCityId = user._attack_selfCityId;
			List<AreaInfo> list = new List<AreaInfo>();
			int up = attack_selfCityId - 6;
			int down = attack_selfCityId + 6;
			int left = attack_selfCityId - 1;
			int right = attack_selfCityId + 1;
            int self = attack_selfCityId;
			if ((attack_selfCityId - 100) % 6 == 0 || attack_selfCityId == 105)
			{
				right = 0;
			}
            else if ((attack_selfCityId - 100) % 6 == 1 || attack_selfCityId == 102)
            {
                left = 0;
            }
			if (up == 112 || up == 113 || up == 134)
			{
				up = 0;
			}
			if (down == 112 || down == 113 || down == 134)
			{
				down = 0;
			}
			if (left == 112 || left == 113 || left == 134)
			{
				left = 0;
			}
			if (right == 112 || right == 113 || right == 134)
			{
				right = 0;
			}
            if (self == 112 || self == 113 || self == 134)
            {
                self = 0;
            }
			AreaInfo areaById;
            if (up > 0)
            {
                areaById = user.getAreaById(up);
                if (areaById != null)
                {
                    list.Add(areaById);
                }
                else
                {
                    list.Add(null);
                }
            }
            else
            {
                list.Add(null);
            }
            if (down > 0)
            {
                areaById = user.getAreaById(down);
                if (areaById != null)
                {
                    list.Add(areaById);
                }
                else
                {
                    list.Add(null);
                }
            }
            else
            {
                list.Add(null);
            }
			if (left > 0)
			{
				areaById = user.getAreaById(left);
				if (areaById != null)
				{
					list.Add(areaById);
				}
				else
				{
					list.Add(null);
				}
			}
			else
			{
				list.Add(null);
			}
			if (right > 0)
			{
				areaById = user.getAreaById(right);
				if (areaById != null)
				{
					list.Add(areaById);
				}
				else
				{
					list.Add(null);
				}
			}
			else
			{
				list.Add(null);
            }
            if (self > 0)
            {
                areaById = user.getAreaById(self);
                if (areaById != null)
                {
                    list.Add(areaById);
                }
                else
                {
                    list.Add(null);
                }
            }
            else
            {
                list.Add(null);
            }
			return list;
		}

        /// <summary>
        /// 获取附近的都城
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public AreaInfo getNearCapital(User user)
        {
            int up = user._attack_selfCityId - 6;
            int down = user._attack_selfCityId + 6;
            int left = user._attack_selfCityId - 1;
            int right = user._attack_selfCityId + 1;
            int areaId = 0;
            if (user.NationInt == 1)
            {
                areaId = 112;
            }
            else if (user.NationInt == 2)
            {
                areaId = 113;
            }
            else if (user.NationInt == 3)
            {
                areaId = 134;
            }
            else
            {
                return null;
            }
            if ((user._attack_selfCityId - 100) % 6 == 0 || user._attack_selfCityId == 105)
            {
                right = 0;
            }
            else if ((user._attack_selfCityId - 100) % 6 == 1 || user._attack_selfCityId == 102)
            {
                left = 0;
            }
            if ((up == 112 || up == 113 || up == 134) && up != areaId)
            {
                return user.getAreaById(up);
            }
            if ((down == 112 || down == 113 || down == 134) && down != areaId)
            {
                return user.getAreaById(down);
            }
            if ((left == 112 || left == 113 || left == 134) && left != areaId)
            {
                return user.getAreaById(left);
            }
            if ((right == 112 || right == 113 || right == 134) && right != areaId)
            {
                return user.getAreaById(right);
            }
            return null;
        }

		public int moveToArea(ProtocolMgr protocol, ILogger logger, User user, int areaId)
		{
			string url = "/root/world!transferInNewArea.action";
			string data = "areaId=" + areaId;
			ServerResult serverResult = protocol.postXml(url, data, "新世界移动城市");
			bool flag = serverResult == null;
			int result;
			if (flag)
			{
				result = 1;
			}
            else if (!serverResult.CmdSucceed)
            {
                if (serverResult.CmdError.IndexOf("城防自动恢复完毕") >= 0)
                {
                    result = 2;
                }
                else if (serverResult.CmdError.IndexOf("不能移动到那里") >= 0)
                {
                    user._attack_move_path.Clear();
                    result = 3;
                }
                else
                {
                    result = 4;
                }
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                AreaInfo areaById = user.getAreaById(areaId);
                if (areaById != null)
                {
                    user._attack_selfCityId = areaById.areaid;
                    base.logInfo(logger, string.Format("移动到城市[{0}]", areaById.areaname));
                    user._remove_attack_move_now(areaById.areaid);
                    result = 0;
                }
                else
                {
                    result = 3;
                }
            }
			return result;
		}

		public int newMoveToArea(ProtocolMgr protocol, ILogger logger, User user, int areaId, out long remaintime)
		{
			remaintime = 120000L;
			string url = "/root/world!transferInNewArea.action";
			string data = "areaId=" + areaId;
			ServerResult serverResult = protocol.postXml(url, data, "新世界移动城市");
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
					bool flag3 = serverResult.CmdError.IndexOf("城防自动恢复完毕") >= 0;
					if (flag3)
					{
						result = 2;
					}
					else
					{
						bool flag4 = serverResult.CmdError.IndexOf("不能移动到那里") >= 0;
						if (flag4)
						{
							user._attack_move_path.Clear();
							result = 3;
						}
						else
						{
							result = 4;
						}
					}
				}
				else
				{
					XmlDocument cmdResult = serverResult.CmdResult;
					AreaInfo areaById = user.getAreaById(areaId);
					bool flag5 = areaById != null;
					if (flag5)
					{
						user._attack_selfCityId = areaById.areaid;
						base.logInfo(logger, string.Format("移动到城市[{0}]", areaById.areaname));
						user._remove_attack_move_now(areaById.areaid);
						XmlNode xmlNode = cmdResult.SelectSingleNode("/results/playerupdateinfo/transfercd");
						bool flag6 = xmlNode != null;
						if (flag6)
						{
							long.TryParse(xmlNode.InnerText, out remaintime);
						}
						string url2 = "/root/world!getNewArea.action";
						ServerResult xml = protocol.getXml(url2, "新世界界面");
						bool flag7 = xml == null || !xml.CmdSucceed;
						if (flag7)
						{
							result = 1;
						}
						else
						{
							double num = 0.0;
							XmlDocument cmdResult2 = xml.CmdResult;
							XmlNode xmlNode2 = cmdResult2.SelectSingleNode("/results/freeclearmovetime");
							bool flag8 = xmlNode2 != null;
							if (flag8)
							{
								double.TryParse(xmlNode2.InnerText, out num);
							}
							int num2 = (int)num;
							bool flag9 = num2 > 0;
							if (flag9)
							{
								base.logInfo(logger, string.Format("有免费清除CD[{0}]次", num2));
								string url3 = "/root/world!cdMoveRecoverConfirm.action";
								ServerResult xml2 = protocol.getXml(url3, "免费CD");
								bool flag10 = xml2 == null || !xml2.CmdSucceed;
								if (flag10)
								{
									result = 1;
									return result;
								}
								base.logInfo(logger, string.Format("清除迁移CD", new object[0]));
								remaintime = 2000L;
							}
							result = 0;
						}
					}
					else
					{
						result = 3;
					}
				}
			}
			return result;
		}

        /// <summary>
        /// 寻找并攻击玩家
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="_reserve_token"></param>
        /// <param name="_attack_only_not_injail">不打被抓的</param>
        /// <param name="_attack_npc"></param>
        /// <param name="_level_min"></param>
        /// <param name="_level_max"></param>
        /// <param name="attack_filter_type"></param>
        /// <param name="attack_filter_content"></param>
        /// <returns></returns>
        public long find_and_attack(ProtocolMgr protocol, ILogger logger, User user, int _reserve_token, bool _attack_only_not_injail, bool _attack_npc, int _level_min, int _level_max, int attack_filter_type, string attack_filter_content)
        {
            if (user._arrest_state == 100)
            {
                this.escapeFromJail(protocol, logger);
                user._arrest_state = 0;
                return 60000;
            }
            else if (user._arrest_state > 10)
            {
                return 60000;
            }
            int draughtRet = this._factory.getTroopManager().makeSureForce(protocol, logger, 0.5);
            if (draughtRet != 0)
            {
                base.logInfo(logger, "征兵失败, 暂停攻击玩家10分钟");
                return 600000;
            }
            int nationInt = user.NationInt;
            int attack_nation = 0;
            if (attack_filter_type > 0 && attack_filter_type == 1)
            {
                string[] array = new string[] { "中立", "魏国", "蜀国", "吴国" };
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Contains(attack_filter_content))
                    {
                        attack_nation = i;
                        break;
                    }
                }
            }
            bool find = false;
            bool attack = false;
            int protectcd = 0;
            List<AreaInfo> nearAreas = this.getNearAreas(user);
            foreach (AreaInfo area in nearAreas)
            {
                if (area != null && area.scopecount != 0)
                {
                    for (int scopeId = 1; scopeId <= area.scopecount; scopeId++)
                    {
                        //获取地区区域信息
                        List<ScopeCity> areaScopeInfo = this.getAreaScopeInfo(protocol, logger, user, area.areaid, scopeId);
                        if (areaScopeInfo == null || areaScopeInfo.Count == 0)
                        {
                            continue;
                        }
                        foreach (ScopeCity city in areaScopeInfo)
                        {
                            if ((!_attack_only_not_injail || city.arreststate != 1) && city.nation != nationInt)
                            {
                                if (city.protectcd > 0)
                                {
                                    if (protectcd == 0)
                                    {
                                        protectcd = city.protectcd;
                                    }
                                    else if (city.protectcd < protectcd)
                                    {
                                        protectcd = city.protectcd;
                                    }
                                }
                                else
                                {
                                    if (city.playerid < 0)
                                    {
                                        if (!_attack_npc)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (city.citylevel > _level_max || city.citylevel < _level_min)
                                        {
                                            continue;
                                        }
                                    }
                                    if (attack_filter_type == 1)
                                    {
                                        if (attack_nation > 0 && city.nation != attack_nation)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (attack_filter_type == 2 && city.playername != attack_filter_content)
                                        {
                                            continue;
                                        }
                                    }
                                    find = true;
                                    int worldevent;
                                    int seniorslaves;
                                    int attackRet = this.attackPlayer(protocol, logger, user, area.areaid, scopeId, city.cityid, out worldevent, out seniorslaves);
                                    if (seniorslaves == 1)
                                    {
                                        _factory.getMiscManager().getSeniorJailInfo(protocol, logger);
                                    }
                                    if (worldevent == 2)
                                    {
                                        _factory.getMiscManager().handleTradeFriend(protocol, logger, user);
                                    }
                                    else if (worldevent == 1)
                                    {
                                        this.AttackSpy(protocol, logger, user);
                                    }
                                    if (attackRet <= 0)
                                    {
                                        attack = true;
                                    }
                                    else if (attackRet == 2)
                                    {
                                        return user.TokenCdTime;
                                    }
                                    else if (attackRet == 3)
                                    {
                                        this.escapeFromJail(protocol, logger);
                                        user._arrest_state = 0;
                                        return 60000;
                                    }
                                    else if (attackRet == 4)
                                    {
                                        if (user._attack_cityHpRecoverCd > 0)
                                        {
                                            return user._attack_cityHpRecoverCd;
                                        }
                                        return 300000;
                                    }
                                    else if (attackRet == 5)
                                    {
                                        return base.next_hour();
                                    }
                                    else if (attackRet == 6)
                                    {
                                        return 120000;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (attack)
            {
                return base.immediate();
            }
            else if (find)
            {
                return protectcd;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取区域城池信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <param name="scopeId"></param>
        /// <returns></returns>
		public List<ScopeCity> getAreaScopeInfo(ProtocolMgr protocol, ILogger logger, User user, int areaId, int scopeId)
		{
			AreaInfo areaById = user.getAreaById(areaId);
			if (areaById == null)
			{
				return null;
			}
            List<ScopeCity> list = new List<ScopeCity>();
            string url = "/root/area!getAllCity.action";
            string data = string.Format("areaId={0}&scopeId={1}", areaId, scopeId);
            ServerResult serverResult = protocol.postXml(url, data, string.Format("新世界查看城市[{0}(区域数量{1}), 查看区域{2}]", areaById.areaname, areaById.scopecount, scopeId));
            if (serverResult == null)
            {
                return null;
            }
            else if (!serverResult.CmdSucceed)
            {
                return null;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/city");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    ScopeCity scopeCity = new ScopeCity();
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        if (xmlNode2.Name == "cityid")
                        {
                            scopeCity.cityid = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "scopeid")
                        {
                            scopeCity.scopeid = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "playerid")
                        {
                            scopeCity.playerid = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "hostility")
                        {
                            scopeCity.hostility = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "playername")
                        {
                            scopeCity.playername = xmlNode2.InnerText;
                        }
                        else if (xmlNode2.Name == "citytype")
                        {
                            scopeCity.citytype = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "arreststate")
                        {
                            scopeCity.arreststate = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "nation")
                        {
                            scopeCity.nation = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "citylevel")
                        {
                            scopeCity.citylevel = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "protectcd")
                        {
                            scopeCity.protectcd = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "myspy")
                        {
                            scopeCity.myspy = int.Parse(xmlNode2.InnerText);
                        }
                    }
                    list.Add(scopeCity);
                }
                return list;
            }
		}

        /// <summary>
        /// 攻击玩家 0:成功,-1:失败,>0:出错
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <param name="scopeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public int attackPlayer(ProtocolMgr protocol, ILogger logger, User user, int areaId, int scopeId, int cityId, out int worldevent, out int seniorslaves)
		{
            worldevent = 0;
            seniorslaves = 0;
			string url = "/root/world!attackOtherAreaCity.action";
			string data = string.Format("areaId={0}&scopeId={1}&cityId={2}", areaId, scopeId, cityId);
			ServerResult serverResult = protocol.postXml(url, data, "攻击玩家");
            //logger.logInfo("攻击结果：" + serverResult.getDebugInfo());
			if (serverResult == null)
			{
				return 1;
			}
            else if (!serverResult.CmdSucceed)
            {
                if (serverResult.CmdError.IndexOf("军令还没有冷却") >= 0)
                {
                    return 2;
                }
                else if (serverResult.CmdError.IndexOf("请先逃跑") >= 0)
                {
                    return 3;
                }
                else if (serverResult.CmdError.IndexOf("距离太远") >= 0)
                {
                    return 4;
                }
                else if (serverResult.CmdError.IndexOf("没有足够的攻击令") >= 0)
                {
                    return 5;
                }
                else if (serverResult.CmdError.IndexOf("组队") >= 0)
                {
                    return 6;
                }
                else if (serverResult.CmdError.IndexOf("不能在都城攻击敌方玩家") >= 0)
                {
                    return 4;
                }
                else
                {
                    return 7;
                }
            }
            else
            {
                bool winside = false;
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode seniorslavesXmlNode = cmdResult.SelectSingleNode("/results/seniorslaves");
                if (seniorslavesXmlNode != null)
                {
                    int.TryParse(seniorslavesXmlNode.InnerText, out seniorslaves);
                }
                XmlNode worldeventXmlNode = cmdResult.SelectSingleNode("/results/worldevent");
                if (worldeventXmlNode != null)
                {
                    int.TryParse(worldeventXmlNode.InnerText, out worldevent);
                }
                if (worldevent == 1)
                {
                    base.logInfo(logger, "发现间谍");
                    logger.logSurprise("发现间谍");
                }
                else if (worldevent == 2)
                {
                    base.logInfo(logger, "发现商盟之友");
                    logger.logSurprise("发现商盟之友");
                }
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/battlereport");
                int attcityhpchange = 0;
                int defcityhpchange = 0;
                int attscore = 0;
                if (xmlNode == null)
                {
                    winside = true;
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/attcityhpchange");
                    if (xmlNode2 != null)
                    {
                        int.TryParse(xmlNode2.InnerText, out attcityhpchange);
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/defcityhpchange");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out defcityhpchange);
                    }
                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/attscore");
                    if (xmlNode4 != null)
                    {
                        int.TryParse(xmlNode4.InnerText, out attscore);
                    }
                    int playertype = 3;
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/defender/playertype");
                    if (xmlNode5 != null)
                    {
                        int.TryParse(xmlNode5.InnerText, out playertype);
                    }
                    int myspy = 0;
                    XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/myspy");
                    if (xmlNode6 != null)
                    {
                        int.TryParse(xmlNode6.InnerText, out myspy);
                    }
                    string worldeventreward = "";
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/worldeventreward");
                    if (xmlNode7 != null)
                    {
                        worldeventreward = " 获得 " + xmlNode7.InnerText;
                    }
                    string text = (playertype == 3) ? "守备军" : "禁卫军";
                    if (myspy > 0) text = "间谍";
                    string text2 = string.Format("攻打{0}, 您城防减少{1}, {0}城防减少{2}, 您获得战绩{3}{4}", text, attcityhpchange, defcityhpchange, attscore, worldeventreward);
                    base.logInfo(logger, text2);
                }
                else
                {
                    int attlost = 0;
                    int deflost = 0;
                    int num7 = 0;
                    int playerlevel = 0;
                    string message = "";
                    string playername = "";
                    XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/battlereport/attcityhpchange");
                    if (xmlNode6 != null)
                    {
                        int.TryParse(xmlNode6.InnerText, out attcityhpchange);
                    }
                    XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/battlereport/defcityhpchange");
                    if (xmlNode7 != null)
                    {
                        int.TryParse(xmlNode7.InnerText, out defcityhpchange);
                    }
                    XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/battlereport/attscore");
                    if (xmlNode8 != null)
                    {
                        int.TryParse(xmlNode8.InnerText, out attscore);
                    }
                    XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/attacker/playerlevel");
                    if (xmlNode9 != null)
                    {
                        int.TryParse(xmlNode9.InnerText, out num7);
                    }
                    XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/defender/playerlevel");
                    if (xmlNode10 != null)
                    {
                        int.TryParse(xmlNode10.InnerText, out playerlevel);
                    }
                    XmlNode xmlNode11 = cmdResult.SelectSingleNode("/results/defender/playername");
                    if (xmlNode11 != null)
                    {
                        playername = xmlNode11.InnerText;
                    }
                    XmlNode xmlNode12 = cmdResult.SelectSingleNode("/results/battlereport/attlost");
                    if (xmlNode12 != null)
                    {
                        int.TryParse(xmlNode12.InnerText, out attlost);
                    }
                    XmlNode xmlNode13 = cmdResult.SelectSingleNode("/results/battlereport/deflost");
                    if (xmlNode13 != null)
                    {
                        int.TryParse(xmlNode13.InnerText, out deflost);
                    }
                    XmlNode xmlNode14 = cmdResult.SelectSingleNode("/results/battlereport/message");
                    if (xmlNode14 != null)
                    {
                        message = xmlNode14.InnerText;
                    }
                    XmlNode xmlNode15 = cmdResult.SelectSingleNode("/results/reporturl");
                    if (xmlNode15 != null)
                    {
                        string innerText = xmlNode15.InnerText;
                    }
                    string slavename = "";
                    XmlNode xmlNode16 = cmdResult.SelectSingleNode("/results/slavename");
                    if (xmlNode16 != null)
                    {
                        slavename = xmlNode16.InnerText;
                    }
                    XmlNode xmlNode17 = cmdResult.SelectSingleNode("/results/battlereport/winside");
                    if (xmlNode17 != null && xmlNode17.InnerText == "1")
                    {
                        winside = true;
                    }
                    string text6 = string.Format("您攻打{0}, {1}, {2}{3} 获得战绩{4}, 您/敌({5}级)城防减少{6}/{7}", playername, winside ? "成功" : "失败", message, slavename, attscore, playerlevel, attcityhpchange, defcityhpchange);
                    base.logInfo(logger, text6);
                }
                if (winside)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
		}

        /// <summary>
        /// 攻击天降奇兵,0:成功,1:空值,2:军令还没有冷却,3:请先逃跑,4:距离太远or不能在都城攻击敌方玩家,5:没有足够的攻击令,6:组队,7:其他错误
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <param name="force"></param>
        /// <returns></returns>
		public int attackNationDayNpc(ProtocolMgr protocol, ILogger logger, User user, int areaId, int force)
		{
			string[] array = new string[]
			{
				"蛮族密探",
				"蛮族山贼",
				"蛮族队长",
				"蛮族统领",
				"蛮族头目"
			};
			string url = "/root/world!lookAreaCity.action";
			string data = string.Format("areaId={0}", areaId);
			ServerResult serverResult = protocol.postXml(url, data, "地区信息");
			if (serverResult == null)
			{
				return 1;
			}
            url = "/root/nationDay!getNationDayNpcInfo.action";
            serverResult = protocol.getXml(url, "奇兵信息");
            if (serverResult == null)
            {
                return 1;
            }
            url = "/root/nationDay!attackNationDayNpc.action";
            data = string.Format("force={0}", force);
            serverResult = protocol.postXml(url, data, "攻击奇兵");
            if (serverResult == null)
            {
                return 1;
            }
            if (!serverResult.CmdSucceed)
            {
                if (serverResult.CmdError.IndexOf("军令还没有冷却") >= 0)
                {
                    return 2;
                }
                else if (serverResult.CmdError.IndexOf("请先逃跑") >= 0)
                {
                    return 3;
                }
                else if (serverResult.CmdError.IndexOf("距离太远") >= 0)
                {
                    return 4;
                }
                else if (serverResult.CmdError.IndexOf("没有足够的攻击令") >= 0)
                {
                    return 5;
                }
                else if (serverResult.CmdError.IndexOf("组队") >= 0)
                {
                    return 6;
                }
                else if (serverResult.CmdError.IndexOf("不能在都城攻击敌方玩家") >= 0)
                {
                    return 4;
                }
                else
                {
                    return 7;
                }
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/rewardinfo/reward/num");
                int num = 0;
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/zhangong");
                int num2 = 0;
                if (xmlNode2 != null)
                {
                    int.TryParse(xmlNode2.InnerText, out num2);
                }
                XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/attscore");
                int num3 = 0;
                if (xmlNode3 != null)
                {
                    int.TryParse(xmlNode3.InnerText, out num3);
                }
                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/catches");
                string a = "false";
                if (xmlNode4 != null)
                {
                    a = xmlNode4.InnerText;
                }
                string text;
                if (a == "true")
                {
                    text = string.Format("攻打{0}成功, 您获得宝石{1}，战功{2}，战绩{3}", new object[]
								{
									array[user._attack_nationevent_targetid - 1],
									num,
									num2,
									num3
									});
                }
                else
                {
                    text = string.Format("攻打{0}失败, 您获得宝石{1}，战功{2}，战绩{3}", new object[]
								{
									array[user._attack_nationevent_targetid - 1],
									num,
									num2,
									num3
									});
                }
                base.logInfo(logger, text);
                return 0;
            }
		}

		public void doJail(ProtocolMgr protocol, ILogger logger, User user, bool _do_jail_tech, int gold_available)
		{
			bool flag = user.Level < User.Level_Jail;
			if (!flag)
			{
				bool flag2 = !user._attack_have_jail;
				if (!flag2)
				{
					List<int> jailInfo = this.getJailInfo(protocol, logger, user, _do_jail_tech, gold_available);
					bool flag3 = !user._attack_have_jail;
					if (!flag3)
					{
						bool flag4 = jailInfo == null;
						if (!flag4)
						{
							bool flag5 = jailInfo.Count > 0;
							if (flag5)
							{
								foreach (int current in jailInfo)
								{
									this.slashJailWorker(protocol, logger, current);
								}
							}
						}
					}
				}
			}
		}

		public bool escapeFromJail(ProtocolMgr protocol, ILogger logger)
		{
			string url = "/root/jail!escape.action";
			ServerResult xml = protocol.getXml(url, "从监狱逃跑");
			bool flag = xml == null || !xml.CmdSucceed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				base.logInfo(logger, "从监狱逃跑, 1分钟后成功");
				result = true;
			}
			return result;
		}

		public List<int> getJailInfo(ProtocolMgr protocol, ILogger logger, User user, bool _do_jail_tech, int gold_available)
		{
			List<int> list = new List<int>();
			if (user.Level < User.Level_Jail)
			{
				return list;
			}
            string url = "/root/jail.action";
            ServerResult xml = protocol.getXml(url, "获取监狱信息");
            if (xml == null)
            {
                return null;
            }
            else if (!xml.CmdSucceed)
            {
                user._attack_have_jail = false;
                return null;
            }
            else
            {
                user._attack_have_jail = true;
                XmlDocument cmdResult = xml.CmdResult;
                int techstate = 0;
                XmlNode techstateXmlNode = cmdResult.SelectSingleNode("/results/techstate");
                if (techstateXmlNode != null)
                {
                    int.TryParse(techstateXmlNode.InnerText, out techstate);
                }
                int jailneednum = 0;
                XmlNode jailneednumXmlNode = cmdResult.SelectSingleNode("/results/jailneednum");
                if (jailneednumXmlNode != null)
                {
                    int.TryParse(jailneednumXmlNode.InnerText, out jailneednum);
                }
                int dayslaves = 0;
                XmlNode dayslavesXmlNode = cmdResult.SelectSingleNode("/results/dayslaves");
                if (dayslavesXmlNode != null)
                {
                    int.TryParse(dayslavesXmlNode.InnerText, out dayslaves);
                }
                if (techstate == 0 && dayslaves >= jailneednum)
                {
                    recvExtraTech(protocol, logger);
                }
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/prisoner");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    int item = 0;
                    bool flag4 = false;
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        if (xmlNode2.Name == "slaveid")
                        {
                            item = int.Parse(xmlNode2.InnerText);
                        }
                        else if (xmlNode2.Name == "slashstate")
                        {
                            flag4 = (xmlNode2.InnerText != "1");
                        }
                    }
                    if (!flag4)
                    {
                        list.Add(item);
                    }
                }
                this.slashFreeWorker(protocol, logger, cmdResult);
                if (_do_jail_tech)
                {
                    int num = 20;
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/pergold");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out num);
                    }
                    if (num <= gold_available && cmdResult.SelectSingleNode("/results/remaintime") == null)
                    {
                        this.doJailTech(protocol, logger, num);
                    }
                }
                return list;
            }
		}

        public void recvExtraTech(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/jail!recvExtraTech.action";
            ServerResult xml = protocol.getXml(url, "典狱劳力充足额外奖励");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int baoshi = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out baoshi);
            }
            base.logInfo(logger, string.Format("典狱劳力充足额外奖励, 宝石+{0}", baoshi));
        }

		public void doJailTech(ProtocolMgr protocol, ILogger logger, int gold_need)
		{
			string url = "/root/jail!techResearch.action";
			ServerResult xml = protocol.getXml(url, "典狱技术研究");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            base.logInfo(logger, "典狱劳作技术研究成功, 花费金币" + gold_need);
            XmlDocument cmdResult = xml.CmdResult;
            this.slashFreeWorker(protocol, logger, cmdResult);
		}

        public void slashFreeWorker(ProtocolMgr protocol, ILogger logger, XmlDocument xml)
        {
            if (xml == null)
            {
                return;
            }
            int num = 0;
            XmlNode xmlNode = xml.SelectSingleNode("/results/freeprisoner");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out num);
            }
            if (num == 0)
            {
                return;
            }
            for (int i = 0; i < num; i++)
            {
                this.slashJailWorker(protocol, logger, 0);
            }
        }

		public void slashJailWorker(ProtocolMgr protocol, ILogger logger, int slaveId)
		{
			string url = "/root/jail!slash.action";
			string data;
            if (slaveId != 0)
			{
				data = "slaveId=" + slaveId;
			}
			else
			{
				data = "slaveId=Free";
			}
			ServerResult serverResult = protocol.postXml(url, data, "监狱训诫");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/baoshi");
            if (xmlNode != null)
            {
                int num = 0;
                int.TryParse(xmlNode.InnerText, out num);
                if (num > 0)
                {
                    base.logInfo(logger, string.Format("典狱训诫成功, 获得宝石{0}个", num));
                }
            }
		}

		public void handleTransferInfo(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/world!getTranferInfo.action";
			ServerResult xml = protocol.getXml(url, "获取马车信息");
			bool flag = xml == null;
			if (!flag)
			{
				bool flag2 = !xml.CmdSucceed;
				if (!flag2)
				{
					XmlDocument cmdResult = xml.CmdResult;
					bool flag3 = false;
					int num = 0;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/canget");
					bool flag4 = xmlNode != null;
					if (flag4)
					{
						flag3 = xmlNode.InnerText.Equals("1");
					}
					XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/token");
					bool flag5 = xmlNode2 != null;
					if (flag5)
					{
						int.TryParse(xmlNode2.InnerText, out num);
					}
					bool flag6 = !flag3;
					if (!flag6)
					{
						url = "/root/world!getTransferToken.action";
						xml = protocol.getXml(url, "获取个人攻击令");
						bool flag7 = xml == null;
						if (!flag7)
						{
							bool flag8 = !xml.CmdSucceed;
							if (!flag8)
							{
								cmdResult = xml.CmdResult;
								base.logInfo(logger, string.Format("获取个人攻击令成功, 攻击令+{0}", num));
							}
						}
					}
				}
			}
		}

		public void getNationBattleInfo(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/nation!getNationTaskNewInfo.action";
			ServerResult xml = protocol.getXml(url, "获取攻坚战信息");
			bool flag = xml == null;
			if (!flag)
			{
				bool flag2 = !xml.CmdSucceed;
				if (!flag2)
				{
					XmlDocument cmdResult = xml.CmdResult;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/status");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						int.TryParse(xmlNode.InnerText, out user._attack_gongjian_status);
					}
					else
					{
						user._attack_gongjian_status = -1;
					}
					XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/remaintime");
					bool flag4 = xmlNode2 != null;
					if (flag4)
					{
						long.TryParse(xmlNode2.InnerText, out user._attack_nationBattleRemainTime);
					}
					else
					{
						user._attack_nationBattleRemainTime = 86400000L;
					}
					XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/masscity");
					bool flag5 = xmlNode3 != null;
					if (flag5)
					{
						user._attack_nation_battle_city = xmlNode3.InnerText;
					}
					else
					{
						user._attack_nation_battle_city = "";
					}
				}
			}
		}

		public void getNationBattleReward(ProtocolMgr protocol, ILogger logger)
		{
			string url = "/root/nation!getNationTaskNewReward.action";
			ServerResult xml = protocol.getXml(url, "获取攻坚战奖励");
			bool flag = xml == null;
			if (!flag)
			{
				bool flag2 = !xml.CmdSucceed;
				if (!flag2)
				{
					XmlDocument cmdResult = xml.CmdResult;
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/box");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						base.logInfo(logger, string.Format("获取攻坚战奖励, 国家宝箱+{0}", xmlNode.InnerText));
					}
				}
			}
		}

		private void getWorldTokenInfo(ProtocolMgr protocol, ILogger logger)
		{
			string url = "/root/world!getNewAreaNationToken.action";
			ServerResult xml = protocol.getXml(url, "获取国家攻击/防御令城市信息");
			bool flag = xml == null;
			if (!flag)
			{
				bool flag2 = !xml.CmdSucceed;
				if (!flag2)
				{
					XmlDocument cmdResult = xml.CmdResult;
				}
			}
		}

        /// <summary>
        /// 处理天降奇兵活动,0:有目标,1:空值,10:出错,9:领取奖励
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="remain_time"></param>
        /// <returns></returns>
		public int handleNationEventInfo(ProtocolMgr protocol, ILogger logger, User user, out long remain_time)
		{
			remain_time = 1800000;
			string[] array = new string[]
			{
				"蛮族密探",
				"蛮族山贼",
				"蛮族队长",
				"蛮族统领",
				"蛮族头目"
			};
			string url = "/root/nationDay!getNationDayEventInfo.action";
			ServerResult xml = protocol.getXml(url, "获取天降奇兵信息");
			if (xml == null)
			{
				return 1;
			}
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int taskid = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/taskid");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out taskid);
            }
            if (taskid > 0)
            {
                xmlNode = cmdResult.SelectSingleNode("/results/cityname");
                if (xmlNode != null)
                {
                    base.logInfo(logger, string.Format("发现天降奇兵任务：目标为【{0}】中的【{1}】", xmlNode.InnerText, array[taskid - 1]));
                }
                int attack_nationevent_target_areaid = 0;
                xmlNode = cmdResult.SelectSingleNode("/results/nationdaynpc");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out attack_nationevent_target_areaid);
                }
                user._attack_nationevent_target_areaid = attack_nationevent_target_areaid;
                user._attack_nationevent_targetid = taskid;
                return 0;
            }
            int hasfinalreward = 0;
            xmlNode = cmdResult.SelectSingleNode("/results/hasfinalreward");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out hasfinalreward);
            }
            for (int i = 1; i < 5; i++)
            {
                string url2 = "/root/nationDay!recvGongReward.action";
                string data = "rId=" + i;
                ServerResult serverResult = protocol.postXml(url2, data, "天降奇兵领取奖励");
            }
            if (hasfinalreward == 1)
            {
                int cost = 10;
                xmlNode = cmdResult.SelectSingleNode("/results/cost");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out cost);
                }
                ServerResult serverResult = null;
                XmlDocument xmlDoc = null;
                XmlNode xmlNode2 = null;
                string url2 = "/root/nationDay!recvFinalReward.action";
                string text = null;
                while (cost < 10)
                {
                    if (cost > 0)
                    {
                        text = string.Format("天降奇兵花费{0}金币领取终极奖励", cost);
                    }
                    else
                    {
                        text = "天降奇兵免费领取终极奖励";
                    }
                    base.logInfo(logger, text);
                    cost = 10;
                    serverResult = protocol.getXml(url2, "天降奇兵领取终极奖励");
                    if (serverResult != null && serverResult.CmdSucceed)
                    {
                        xmlDoc = serverResult.CmdResult;
                        xmlNode2 = xmlDoc.SelectSingleNode("/results/cost");
                        if (xmlNode2 != null)
                        {
                            int.TryParse(xmlNode2.InnerText, out cost);
                        }
                    }
                }
            }
            return 9;
		}

        /// <summary>
        /// 处理新世界悬赏事件 10:失败,15:已到保留次数,0:已接,未完成,1:空结果,2:完成,未到最后一次,3:完成最后一次,准备移动,4:完成最后一次,移动完毕,5:已接,任务星级>最大星级
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="max_star">最大星级</param>
        /// <param name="reserved_num">保留次数</param>
        /// <param name="move_target">移动目标</param>
        /// <param name="remain_time">返回剩余时间</param>
        /// <returns></returns>
		public int handleNewCityEventInfo(ProtocolMgr protocol, ILogger logger, User user, int max_star, int reserved_num, string move_target, out long remain_time)
		{
			remain_time = 0;
			string url = "/root/world!getNewCityEventInfo.action";
			ServerResult xml = protocol.getXml(url, "获取悬赏事件信息");
			if (xml == null)
			{
				return 1;
			}
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int remaintimes = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/remaintimes");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out remaintimes);
            }
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/taskremaintime");
            if (xmlNode4 != null)
            {
                long.TryParse(xmlNode4.InnerText, out remain_time);
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/taskstate");
            if (xmlNode2 != null && xmlNode2.InnerText == "1")
            {
                //if (remaintimes != 1)
                //{
                //    base.logInfo(logger, "悬赏已完成");
                //    this.getCityEventAward(protocol, logger);
                //    return 2;
                //}
                //最后一次悬赏,移动到目标
                AreaInfo areaInfo = user.getAreaByName(move_target);
                if (areaInfo == null)
                {
                    int nationInt = user.NationInt;
                    if (nationInt == 1)
                    {
                        areaInfo = user.getAreaById(108);//剑阁
                    }
                    else if (nationInt == 2)
                    {
                        areaInfo = user.getAreaById(117);//隆中
                    }
                    else if (nationInt == 3)
                    {
                        areaInfo = user.getAreaById(125);//涪陵
                    }
                }
                int areaId = 0;
                if (user.NationInt == 1)
                {
                    areaId = 112;//许都
                }
                else if (user.NationInt == 2)
                {
                    areaId = 113;//成都
                }
                else if (user.NationInt == 3)
                {
                    areaId = 134;//武昌
                }
                if (areaInfo.areaid == user._attack_selfCityId)
                {
                    base.logInfo(logger, "悬赏已完成, 已退回移动目标城市");
                    this.getCityEventAward(protocol, logger);
                    return 4;
                }
                else if (user._attack_selfCityId == areaId)
                {
                    base.logInfo(logger, "悬赏已完成, 已被打回都城");
                    this.getCityEventAward(protocol, logger);
                    return 4;
                }
                else if (remain_time <= 300000)//悬赏时间低于5分钟
                {
                    base.logInfo(logger, "悬赏时间到");
                    this.getCityEventAward(protocol, logger);
                    return 4;
                }
                else
                {
                    base.logInfo(logger, "悬赏已完成, 开始返回移动目标城市");
                    return 3;
                }
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/targetname");
            if (xmlNode3 != null)//已接任务
            {
                XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/targetareaname");
                if (xmlNode5 != null)
                {
                    base.logInfo(logger, string.Format("发现已接受世界悬赏, 目标为[{0}]中的[{1}]", xmlNode5.InnerText, xmlNode3.InnerText));
                }
                XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/taskstr");
                if (xmlNode6 == null)
                {
                    return 10;
                }
                string innerText = xmlNode6.InnerText;
                string[] array = innerText.Split(new char[] { ',' });
                int[] array2 = new int[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Length != 0)
                    {
                        string[] array3 = array[i].Split(new char[] { ':' });
                        if (array3.Length >= 2)
                        {
                            int target_star = 0;
                            int num5 = 0;
                            int.TryParse(array3[0], out target_star);
                            int.TryParse(array3[1], out num5);
                            array2[i] = target_star;
                        }
                    }
                }
                int star = 0;
                int taskid = 0;
                XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/taskid");
                if (xmlNode7 != null)
                {
                    int.TryParse(xmlNode7.InnerText, out taskid);
                }
                if (taskid > 0 && taskid < array2.Length)
                {
                    star = array2[taskid - 1];
                }
                if (star > max_star)
                {
                    base.logInfo(logger, string.Format("当前悬赏任务星级为{0}星, 大于设置最高星级, 将等待任务取消, 开始其他敌国任务", star));
                    return 5;
                }
                else
                {
                    this.getNewAreaInfo(protocol, logger, user);
                    return 0;
                }
            }
            //星数奖励
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/starreward");
            int pos = 0;
            foreach (XmlNode xmlNode8 in xmlNodeList)
            {
                pos++;
                XmlNode xmlNode9 = xmlNode8.SelectSingleNode("state");
                if (xmlNode9.InnerText == "1")
                {
                    this.recvCityEventStarAward(protocol, logger, pos);
                }
            }
            if (remaintimes == 0)
            {
                return 4;
            }
            base.logInfo(logger, string.Format("世界悬赏还剩[{0}]次", remaintimes));
            if (remaintimes <= reserved_num)
            {
                base.logInfo(logger, "世界悬赏已到保留次数，不再接受任务");
                return 15;
            }
            XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/taskstr");
            if (xmlNode10 == null)
            {
                return 10;
            }
            string innerText2 = xmlNode10.InnerText;
            string[] array4 = innerText2.Split(new char[] { ',' });
            int max_get_id = -1;
            int max_get_star = 0;
            int min_get_id = 0;
            int min_get_star = 0;
            for (int j = 0; j < array4.Length; j++)
            {
                if (array4[j].Length != 0)
                {
                    string[] array5 = array4[j].Split(new char[] { ':' });
                    if (array5.Length >= 2)
                    {
                        int target_star = 0;
                        int num16 = 0;
                        int.TryParse(array5[0], out target_star);
                        int.TryParse(array5[1], out num16);
                        if (num16 == 0)
                        {
                            if (min_get_star == 0 || min_get_star > target_star)
                            {
                                min_get_id = j;
                                min_get_star = target_star;
                            }
                            if ((max_get_id < 0 && target_star <= max_star) || (max_get_star < target_star && target_star <= max_star))
                            {
                                max_get_id = j;
                                max_get_star = target_star;
                            }
                        }
                    }
                }
            }
            if (max_get_id < 0)
            {
                base.logInfo(logger, "全部悬赏任务都大于设定的星级, 将接最低星级任务等待取消后再选择");
                this.acceptNewCityEvent(protocol, logger, min_get_id + 1, min_get_star);
                this.getNewAreaInfo(protocol, logger, user);
                return 5;
            }
            else
            {
                this.acceptNewCityEvent(protocol, logger, max_get_id + 1, max_get_star);
                this.getNewAreaInfo(protocol, logger, user);
                return 0;
            }
		}

		public bool acceptNewCityEvent(ProtocolMgr protocol, ILogger logger, int pos, int star)
		{
			string url = "/root/world!acceptNewCityEvent.action";
			string data = "pos=" + pos;
			ServerResult serverResult = protocol.postXml(url, data, "新世界接受悬赏事件");
			bool flag = serverResult == null || !serverResult.CmdSucceed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				base.logInfo(logger, string.Format("新世界接受悬赏事件, 星级[{0}]星", star));
				result = true;
			}
			return result;
		}

		public bool getCityEventAward(ProtocolMgr protocol, ILogger logger)
		{
			string url = "/root/world!deliverNewCityEvent.action";
			ServerResult xml = protocol.getXml(url, "新世界领取悬赏奖励");
			bool flag = xml == null || !xml.CmdSucceed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				XmlDocument cmdResult = xml.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/rewardinfo/reward/num");
				bool flag2 = xmlNode != null;
				if (flag2)
				{
					base.logInfo(logger, string.Format("新世界领取悬赏奖励, 宝石+{0}", xmlNode.InnerText));
				}
				result = true;
			}
			return result;
		}

		public bool recvCityEventStarAward(ProtocolMgr protocol, ILogger logger, int pos)
		{
			string url = "/root/world!recvNewCityEventStarReward.action";
			string data = "pos=" + pos;
			ServerResult serverResult = protocol.postXml(url, data, "新世界领取悬赏星数奖励");
			bool flag = serverResult == null || !serverResult.CmdSucceed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/rewardinfo/reward/num");
				bool flag2 = xmlNode != null;
				if (flag2)
				{
					base.logInfo(logger, string.Format("新世界领取悬赏星数奖励, 宝石+{0}", xmlNode.InnerText));
				}
				result = true;
			}
			return result;
		}

		public List<ResCampaign> getResCampaigns(ProtocolMgr protocol, ILogger logger)
		{
			int num = 0;
			List<ResCampaign> list = new List<ResCampaign>();
			string url = "/root/resCampaign!getResCamList.action";
			ServerResult xml = protocol.getXml(url, "获取资源副本信息");
			bool flag = xml == null;
			List<ResCampaign> result;
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
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/resetnum");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						int.TryParse(xmlNode.InnerText, out num);
					}
					XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/cam");
					foreach (XmlNode xmlNode2 in xmlNodeList)
					{
						bool flag4 = xmlNode2 != null && xmlNode2.HasChildNodes;
						if (flag4)
						{
							XmlNodeList childNodes = xmlNode2.ChildNodes;
							ResCampaign resCampaign = new ResCampaign();
							foreach (XmlNode xmlNode3 in childNodes)
							{
								bool flag5 = xmlNode3.Name == "id";
								if (flag5)
								{
									resCampaign.id = int.Parse(xmlNode3.InnerText);
								}
								else
								{
									bool flag6 = xmlNode3.Name == "name";
									if (flag6)
									{
										resCampaign.name = xmlNode3.InnerText;
									}
									else
									{
										bool flag7 = xmlNode3.Name == "armies";
										if (flag7)
										{
											resCampaign.armies = xmlNode3.InnerText;
										}
										else
										{
											bool flag8 = xmlNode3.Name == "status";
											if (flag8)
											{
												resCampaign.status = int.Parse(xmlNode3.InnerText);
											}
											else
											{
												bool flag9 = xmlNode3.Name == "reward";
												if (flag9)
												{
													resCampaign.reward = xmlNode3.InnerText;
												}
												else
												{
													bool flag10 = xmlNode3.Name == "finishreward";
													if (flag10)
													{
														resCampaign.finishreward = xmlNode3.InnerText;
													}
												}
											}
										}
									}
								}
							}
							list.Add(resCampaign);
						}
					}
					result = list;
				}
			}
			return result;
		}

		public int handleResCampaign(ProtocolMgr protocol, ILogger logger, int target_campaign_id = 1)
		{
			int num = 0;
			List<ResCampaign> list = new List<ResCampaign>();
			string url = "/root/resCampaign!getResCamList.action";
			ServerResult xml = protocol.getXml(url, "获取资源副本信息");
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
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/resetnum");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						int.TryParse(xmlNode.InnerText, out num);
					}
					XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/cam");
					foreach (XmlNode xmlNode2 in xmlNodeList)
					{
						bool flag4 = xmlNode2 != null && xmlNode2.HasChildNodes;
						if (flag4)
						{
							XmlNodeList childNodes = xmlNode2.ChildNodes;
							ResCampaign resCampaign = new ResCampaign();
							foreach (XmlNode xmlNode3 in childNodes)
							{
								bool flag5 = xmlNode3.Name == "id";
								if (flag5)
								{
									resCampaign.id = int.Parse(xmlNode3.InnerText);
								}
								else
								{
									bool flag6 = xmlNode3.Name == "name";
									if (flag6)
									{
										resCampaign.name = xmlNode3.InnerText;
									}
									else
									{
										bool flag7 = xmlNode3.Name == "armies";
										if (flag7)
										{
											resCampaign.armies = xmlNode3.InnerText;
										}
										else
										{
											bool flag8 = xmlNode3.Name == "status";
											if (flag8)
											{
												resCampaign.status = int.Parse(xmlNode3.InnerText);
											}
											else
											{
												bool flag9 = xmlNode3.Name == "reward";
												if (flag9)
												{
													resCampaign.reward = xmlNode3.InnerText;
												}
												else
												{
													bool flag10 = xmlNode3.Name == "finishreward";
													if (flag10)
													{
														resCampaign.finishreward = xmlNode3.InnerText;
													}
												}
											}
										}
									}
								}
							}
							list.Add(resCampaign);
						}
					}
					ResCampaign resCampaign2 = null;
					foreach (ResCampaign current in list)
					{
						bool flag11 = current.id == target_campaign_id;
						if (flag11)
						{
							resCampaign2 = current;
							break;
						}
					}
					bool flag12 = resCampaign2 == null;
					if (flag12)
					{
						result = 2;
					}
					else
					{
						bool flag13 = resCampaign2.status == 2 && num == 0;
						if (flag13)
						{
							result = 3;
						}
						else
						{
							bool flag14 = resCampaign2.status == 1;
							if (flag14)
							{
								string armies = resCampaign2.armies;
								int[] array = new int[armies.Length];
								int num3;
								for (int i = 0; i < armies.Length; i = num3 + 1)
								{
									int num2 = 0;
									int.TryParse(string.Format("{0}", armies[i]), out num2);
									array[i] = num2;
									num3 = i;
								}
								for (int j = 0; j < array.Length; j = num3 + 1)
								{
									for (int k = 3 - array[j]; k > 0; k = num3 - 1)
									{
										int num4 = this.attackCampaignNpc(protocol, logger, resCampaign2, j);
										bool flag15 = num4 == 1;
										if (flag15)
										{
											result = 1;
											return result;
										}
										bool flag16 = num4 == 2;
										if (flag16)
										{
											result = 2;
											return result;
										}
										bool flag17 = num4 == 3;
										if (flag17)
										{
											result = 4;
											return result;
										}
										num3 = k;
									}
									num3 = j;
								}
							}
							else
							{
								bool flag18 = num > 0;
								if (flag18)
								{
									this.useFreeTokenReleaseCampaign(protocol, logger, resCampaign2, ref num);
									int[] array2 = new int[5];
									int[] array3 = array2;
									int num3;
									for (int l = 0; l < array3.Length; l = num3 + 1)
									{
										for (int m = 3 - array3[l]; m > 0; m = num3 - 1)
										{
											int num5 = this.attackCampaignNpc(protocol, logger, resCampaign2, l);
											bool flag19 = num5 == 1;
											if (flag19)
											{
												result = 1;
												return result;
											}
											bool flag20 = num5 == 2;
											if (flag20)
											{
												result = 2;
												return result;
											}
											bool flag21 = num5 == 3;
											if (flag21)
											{
												result = 4;
												return result;
											}
											num3 = m;
										}
										num3 = l;
									}
								}
							}
							bool flag22 = num - 1 > 0;
							if (flag22)
							{
								result = 5;
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

		private int useFreeTokenReleaseCampaign(ProtocolMgr protocol, ILogger logger, ResCampaign campaign, ref int _freeTokenNumber)
		{
			string url = "/root/resCampaign!resetResCamNum.action";
			string data = "id=" + campaign.id;
			ServerResult serverResult = protocol.postXml(url, data, "使用副本重置卡");
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
					base.logInfo(logger, string.Format("使用副本重置卡重置副本[{0}], 副本重置卡还剩[{1}]个", campaign.name, _freeTokenNumber - 1));
					result = 0;
				}
			}
			return result;
		}

		private void getCampaignInfo(ProtocolMgr protocol, ILogger logger, int campaignId)
		{
			string url = "/root/resCampaign!getInfo.action";
			string data = "id=" + campaignId;
			ServerResult serverResult = protocol.postXml(url, data, "获取副本信息");
			bool flag = serverResult == null || !serverResult.CmdSucceed;
			if (!flag)
			{
				XmlDocument cmdResult = serverResult.CmdResult;
			}
		}

		private int attackCampaignNpc(ProtocolMgr protocol, ILogger logger, ResCampaign campaign, int npcIndex)
		{
			string url = "/root/resCampaign!attack.action";
			string data = string.Format("armyIndex={0}&id={1}", npcIndex, campaign.id);
			ServerResult serverResult = protocol.postXml(url, data, "攻击副本NPC");
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
					string text = "攻击副本NPC, ";
					XmlNode xmlNode = cmdResult.SelectSingleNode("/results/battlereport/message");
					bool flag3 = xmlNode != null;
					if (flag3)
					{
						text += xmlNode.InnerText;
					}
					text = text + " 您获得了 " + campaign.getRewardByNpcIndex(npcIndex);
					base.logInfo(logger, text);
					result = 0;
				}
			}
			return result;
		}

        /// <summary>
        /// 使用道具,0:成功,-1:没有道具,-2:目标太远,-3:没有找到目标,-4:不能对本国玩家使用,-5:请求空值或出错,-6:区域不存在,-7:先逃跑或保护cd,-8:不存在道具类型
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <param name="type">道具类型, 1:决斗战旗, 2:诱敌锦囊</param>
        /// <param name="min_level"></param>
        /// <param name="max_level"></param>
        /// <returns></returns>
        public long handleDaoju(ProtocolMgr protocol, ILogger logger, User user, int areaId, int type, int min_level, int max_level, List<string> blacklist)
        {
            if (type == 1)
            {
                return useJueDouZhanQi(protocol, logger, user);
            }
            else if (type == 2)
            {
                return useXiuDiJinNang(protocol, logger, user, areaId, min_level, max_level, blacklist);
            }
            return -8;
        }

        /// <summary>
        /// 使用诱敌锦囊,0:成功,-1:没有锦囊,-2:目标太远,-3:没有找到目标,-4:不能对本国玩家使用,-5:请求空值或出错
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <param name="min_level"></param>
        /// <param name="max_level"></param>
        /// <returns></returns>
        private long useXiuDiJinNang(ProtocolMgr protocol, ILogger logger, User user, int areaId, int min_level, int max_level, List<string> blacklist)
        {
            if (user._attack_daojuJin > 0)
            {
                if (areaId > 0)
                {
                    for (int scopeId = 1; scopeId < 10; scopeId++)
                    {
                        List<ScopeCity> areaScopeInfo = this.getAreaScopeInfo(protocol, logger, user, areaId, scopeId);
                        if (areaScopeInfo == null || areaScopeInfo.Count == 0)
                        {
                            continue;
                        }
                        foreach (ScopeCity city in areaScopeInfo)
                        {
                            if (city.citylevel >= min_level && city.citylevel <= max_level && city.arreststate != 1 && city.protectcd == 0 && city.citytype == 1)
                            {
                                if (blacklist.Contains(city.playername))
                                {
                                    base.logInfo(logger, string.Format("决斗黑名单[{0}], 最大城防值低于350", city.playername));
                                    continue;
                                }
                                int ret = useWorldDaoju(protocol, logger, user, areaId, city.scopeid, city.cityid, 2);
                                if (ret == 0)
                                {
                                    base.logInfo(logger, string.Format("对玩家{0}使用诱敌锦囊", city.playername));
                                    return 0;
                                }
                                else if (ret == 3)
                                {
                                    return -1;
                                }
                                else if (ret == 2 || ret == 5)
                                {
                                    return -2;
                                }
                                else if (ret == 4)
                                {
                                    return -4;
                                }
                                else if (ret == 1 || ret == 10)
                                {
                                    return -5;
                                }
                            }
                        }
                    }
                    return -3;
                }
                return -2;
            }
            return -1;
        }

        /// <summary>
        /// 使用决斗战旗,0:成功,-1:没有战旗,-2:目标太远,-3:没有找到目标,-4:不能对本国玩家使用,-5:请求空值或出错,-6:区域不存在,-7:先逃跑或保护cd
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private long useJueDouZhanQi(ProtocolMgr protocol, ILogger logger, User user)
        {
            if (user._attack_daojuFlag > 0)
            {
                if (user._attack_xiudi_target_areaid > 0)
                {
                    List<ScopeCity> areaScopeInfo = this.getAreaScopeInfo(protocol, logger, user, user._attack_xiudi_target_areaid, user._attack_xiudi_target_scopeid);
                    if (areaScopeInfo == null || areaScopeInfo.Count == 0)
                    {
                        return -6;
                    }
                    foreach (ScopeCity city in areaScopeInfo)
                    {
                        if (city.playerid == user._attack_xiudi_target_playerid)
                        {
                            int ret = useWorldDaoju(protocol, logger, user, user._attack_xiudi_target_areaid, user._attack_xiudi_target_scopeid, city.cityid, 1);
                            if (ret == 0)
                            {
                                base.logInfo(logger, string.Format("对玩家{0}使用决斗战旗", city.playername));
                                return 0;
                            }
                            else if (ret == 3)
                            {
                                return -1;
                            }
                            else if (ret == 2 || ret == 5)
                            {
                                return -2;
                            }
                            else if (ret == 4)
                            {
                                return -4;
                            }
                            else if (ret == 1 || ret == 10)
                            {
                                return -5;
                            }
                            else if (ret == 7 || ret == 8)
                            {
                                return -7;
                            }
                        }
                    }
                }
                return -3;
            }
            return -1;
        }

        /// <summary>
        /// 新世界使用道具,0:成功,1:返回空值,2:您不能在都城使用道具,3:道具数不足,4:不能对本国玩家使用,5:您和目标的距离太远,6:诱敌次数已满,7:请先逃跑,8:该玩家处于保护CD中,9:该玩家不存在,10:请求出错,11:当前城池正在补充城防
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <param name="scopeId"></param>
        /// <param name="cityId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int useWorldDaoju(ProtocolMgr protocol, ILogger logger, User user, int areaId, int scopeId, int cityId, int type)
        {
            string url = "/root/world!useWorldDaoju.action";
            string data = string.Format("areaId={0}&cityId={1}&type={2}&scopeId={3}", areaId, cityId, type, scopeId);
            ServerResult serverResult = protocol.postXml(url, data, "新世界使用道具");
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                base.logInfo(logger, string.Format("使用{0}失败, {1}", type == 1 ? "决斗战旗" : "诱敌锦囊", serverResult.CmdError));
                if (serverResult.CmdError.IndexOf("您不能在都城使用道具") >= 0)
                {
                    return 2;
                }
                else if (serverResult.CmdError.IndexOf("道具数不足") >= 0)
                {
                    return 3;
                }
                else if (serverResult.CmdError.IndexOf("不能对本国玩家使用") >= 0)
                {
                    return 4;
                }
                else if (serverResult.CmdError.IndexOf("您和目标的距离太远") >= 0)
                {
                    return 5;
                }
                else if (serverResult.CmdError.IndexOf("该玩家今日诱敌次数已满") >= 0)
                {
                    return 6;
                }
                else if (serverResult.CmdError.IndexOf("请先逃跑") >= 0)
                {
                    return 7;
                }
                else if (serverResult.CmdError.IndexOf("该玩家处于保护CD中") >= 0)
                {
                    return 8;
                }
                else if (serverResult.CmdError.IndexOf("该玩家不存在") >= 0)
                {
                    return 9;
                }
                else if (serverResult.CmdError.IndexOf("当前城池正在补充城防") >= 0)
                {
                    return 11;
                }
                else
                {
                    return 10;
                }
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/toareaid");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out user._attack_xiudi_target_areaid);
                }
                else
                {
                    user._attack_xiudi_target_areaid = 0;
                }
                xmlNode = cmdResult.SelectSingleNode("/results/toscopeid");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out user._attack_xiudi_target_scopeid);
                }
                else
                {
                    user._attack_xiudi_target_scopeid = 0;
                }
                xmlNode = cmdResult.SelectSingleNode("/results/toplayerid");
                if (xmlNode != null)
                {
                    int.TryParse(xmlNode.InnerText, out user._attack_xiudi_target_playerid);
                }
                else
                {
                    user._attack_xiudi_target_playerid = 0;
                }
                return 0;
            }
        }

        /// <summary>
        /// 决斗信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public PkInfo getPkInfo(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/world!getPkInfo.action";
            string data = "type=0";
            PkInfo pkInfo = new PkInfo();
            ServerResult serverResult = protocol.postXml(url, data, "获取决斗信息");
            if (serverResult == null)
            {
                pkInfo.result = -1;
            }
            else if (!serverResult.CmdSucceed)
            {
                if (serverResult.CmdError.IndexOf("您不在决斗中") >= 0)
                {
                    pkInfo.result = 1;
                }
                else
                {
                    pkInfo.result = -2;
                }
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/stage");
                if (xmlNode != null)
                {
                    pkInfo.stage = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/pkresult");
                if (xmlNode != null)
                {
                    pkInfo.pkresult = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/gong/cityhp");
                if (xmlNode != null)
                {
                    pkInfo.gong_cityhp = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/gong/maxcityhp");
                if (xmlNode != null)
                {
                    pkInfo.gong_maxcityhp = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/gong/name");
                if (xmlNode != null)
                {
                    pkInfo.gong_name = xmlNode.InnerText;
                }
                xmlNode = cmdResult.SelectSingleNode("/results/fang/cityhp");
                if (xmlNode != null)
                {
                    pkInfo.fang_cityhp = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/fang/maxcityhp");
                if (xmlNode != null)
                {
                    pkInfo.fang_maxcityhp = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/fang/name");
                if (xmlNode != null)
                {
                    pkInfo.fang_name = xmlNode.InnerText;
                }
                xmlNode = cmdResult.SelectSingleNode("/results/pkinfo/areaid");
                if (xmlNode != null)
                {
                    pkInfo.areaId = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/pkinfo/scopeid");
                if (xmlNode != null)
                {
                    pkInfo.scopeid = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/pkinfo/cityid");
                if (xmlNode != null)
                {
                    pkInfo.cityid = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/reward/score");
                if (xmlNode != null)
                {
                    pkInfo.score = int.Parse(xmlNode.InnerText);
                }
                xmlNode = cmdResult.SelectSingleNode("/results/reward/baoshi");
                if (xmlNode != null)
                {
                    pkInfo.baoshi = int.Parse(xmlNode.InnerText);
                }
                pkInfo.result = 0;
            }
            return pkInfo;
        }

        //搜索本城并攻击间谍
        public void AttackSpy(ProtocolMgr protocol, ILogger logger, User user)
        {
            getNewAreaInfo(protocol, logger, user);
            AreaInfo self_area = user.getAreaById(user._attack_selfCityId);
            if (self_area != null && self_area.ziyuan == 100)
            {
                int spynum;
                LookAreaCity(protocol, logger, self_area.areaid, out spynum);
                logInfo(logger, string.Format("搜索间谍，找到{0}个间谍", spynum));
                if (spynum > 0)
                {
                    for (int scopeId = 1; scopeId <= self_area.scopecount; scopeId++)
                    {
                        //获取地区区域信息
                        List<ScopeCity> areaScopeInfo = this.getAreaScopeInfo(protocol, logger, user, self_area.areaid, scopeId);
                        if (areaScopeInfo == null || areaScopeInfo.Count == 0)
                        {
                            continue;
                        }
                        foreach (ScopeCity city in areaScopeInfo)
                        {
                            if (spynum <= 0)
                            {
                                user._attack_spy_city = "";
                                return;
                            }

                            if (city.myspy > 0)
                            {
                                int worldevent;
                                int seniorslaves;
                                int attackRet = this.attackPlayer(protocol, logger, user, self_area.areaid, scopeId, city.cityid, out worldevent, out seniorslaves);
                                if (worldevent == 2)
                                {
                                    _factory.getMiscManager().handleTradeFriend(protocol, logger, user);
                                }
                                if (attackRet == 2 || attackRet == 3 || attackRet == 4 || attackRet == 5 || attackRet == 6)
                                {
                                    if (attackRet == 3)
                                    {
                                        escapeFromJail(protocol, logger);
                                        user._arrest_state = 0;
                                    }
                                    return;
                                }
                                spynum--;
                            }
                        }
                    }
                }
            }
        }

        //查看城市
        public void LookAreaCity(ProtocolMgr protocol, ILogger logger, int areaId, out int spynum)
        {
            spynum = 0;
            string url = "/root/world!lookAreaCity.action";
            string data = string.Format("areaId={0}", areaId);
            ServerResult serverResult = protocol.postXml(url, data, "地区信息");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/area/spynum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out spynum);
            }
        }
	}
}
