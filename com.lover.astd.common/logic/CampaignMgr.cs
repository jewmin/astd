using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.battle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace com.lover.astd.common.logic
{
	public class CampaignMgr : MgrBase
	{
		public CampaignMgr(TimeMgr tmrMgr, ServiceFactory factory)
		{
			this._logColor = Color.DeepSkyBlue;
			this._tmrMgr = tmrMgr;
			this._factory = factory;
		}

		public List<CampaignItem> getCampaigns(ProtocolMgr protocol, ILogger logger)
		{
			List<CampaignItem> list = new List<CampaignItem>();
			string url = "/root/campaign!getCampaignList.action";
			ServerResult xml = protocol.getXml(url, "获取单人战役信息");
			bool flag = xml == null;
			List<CampaignItem> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = !xml.CmdSucceed;
				if (flag2)
				{
					result = null;
				}
				else
				{
					XmlDocument cmdResult = xml.CmdResult;
					base.tryUseCard(protocol, logger, cmdResult);
					XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/campaignnew");
					foreach (XmlNode xmlNode in xmlNodeList)
					{
						XmlNodeList childNodes = xmlNode.ChildNodes;
						CampaignItem campaignItem = new CampaignItem();
						foreach (XmlNode xmlNode2 in childNodes)
						{
							bool flag3 = xmlNode2.Name == "id";
							if (flag3)
							{
								campaignItem.id = int.Parse(xmlNode2.InnerText);
							}
							else
							{
								bool flag4 = xmlNode2.Name == "name";
								if (flag4)
								{
									campaignItem.name = xmlNode2.InnerText;
								}
								else
								{
									bool flag5 = xmlNode2.Name == "armynum";
									if (flag5)
									{
										campaignItem.armynum = int.Parse(xmlNode2.InnerText);
									}
									else
									{
										bool flag6 = xmlNode2.Name == "times";
										if (flag6)
										{
											campaignItem.times = int.Parse(xmlNode2.InnerText);
										}
										else
										{
											bool flag7 = xmlNode2.Name == "remain6num";
											if (flag7)
											{
												campaignItem.remain6num = int.Parse(xmlNode2.InnerText);
											}
										}
									}
								}
							}
						}
						bool flag8 = campaignItem.times < 100;
						if (flag8)
						{
							list.Add(campaignItem);
						}
					}
					result = list;
				}
			}
			return result;
		}

		public int startCampaign(ProtocolMgr protocol, ILogger logger, CampaignItem item)
		{
			bool flag = item == null;
			int result;
			if (flag)
			{
				base.logInfo(logger, "战役已打完");
				result = 10;
			}
			else
			{
				int num = 0;
				int num2 = this.innerStartCampaign(protocol, logger, item.id, item.name);
				bool flag2 = num2 == 1;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = num2 == 2;
					if (flag3)
					{
						string campaignInfo = this.getCampaignInfo(protocol, logger, out num);
						bool flag4 = num > 0;
						if (flag4)
						{
							bool flag5 = item.remain6num > 0;
							bool campaignReward;
							if (flag5)
							{
								campaignReward = this.getCampaignReward(protocol, logger, 2);
							}
							else
							{
								campaignReward = this.getCampaignReward(protocol, logger, 0);
							}
							bool flag6 = !campaignReward;
							if (flag6)
							{
								base.logInfo(logger, "获取战役奖励失败, 停止战役");
								result = 7;
								return result;
							}
							base.logInfo(logger, string.Format("获取战役奖励: {0}", campaignInfo));
							int times = item.times;
							item.times = times + 1;
							result = 0;
							return result;
						}
					}
					else
					{
						bool flag7 = num2 == 3;
						if (flag7)
						{
							base.logInfo(logger, "军令已经CD");
							result = 3;
							return result;
						}
						bool flag8 = num2 == 4;
						if (flag8)
						{
							base.logInfo(logger, "没有令了");
							result = 4;
							return result;
						}
						bool flag9 = num2 == 5;
						if (flag9)
						{
							base.logInfo(logger, "征兵失败");
							result = 5;
							return result;
						}
					}
					bool flag10 = !this.completeCampaign(protocol, logger);
					if (flag10)
					{
						base.logInfo(logger, "战役未完成, 请检查是否你的实力不足以一半兵力完成此战役");
						result = 6;
					}
					else
					{
						string campaignInfo2 = this.getCampaignInfo(protocol, logger, out num);
						bool flag11 = num == 0;
						if (flag11)
						{
							base.logInfo(logger, "战役未完成, 请检查是否你的实力不足以一半兵力完成此战役");
							result = 6;
						}
						else
						{
							bool flag12 = item.remain6num > 0;
							bool campaignReward2;
							if (flag12)
							{
								campaignReward2 = this.getCampaignReward(protocol, logger, 2);
							}
							else
							{
								campaignReward2 = this.getCampaignReward(protocol, logger, 0);
							}
							bool flag13 = !campaignReward2;
							if (flag13)
							{
								base.logInfo(logger, "获取战役奖励失败, 停止战役");
								result = 7;
							}
							else
							{
								base.logInfo(logger, string.Format("获取战役奖励: {0}", campaignInfo2));
								int times = item.times;
								item.times = times + 1;
								result = 0;
							}
						}
					}
				}
			}
			return result;
		}

		private int innerStartCampaign(ProtocolMgr protocol, ILogger logger, int id, string name)
		{
			int num = this._factory.getTroopManager().makeSureForce(protocol, logger, 0.8);
			bool flag = num > 0;
			int result;
			if (flag)
			{
				base.logInfo(logger, "征兵失败, 停止战役");
				result = 5;
			}
			else
			{
				string url = "/root/campaign!startWar.action";
				string data = "campaignId=" + id;
				string text = string.Format("开始战役[{0}]", name);
				ServerResult serverResult = protocol.postXml(url, data, text);
				bool flag2 = serverResult == null;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool cmdSucceed = serverResult.CmdSucceed;
					if (cmdSucceed)
					{
						logger.log(text, this._logColor);
						result = 0;
					}
					else
					{
						string cmdError = serverResult.CmdError;
						bool flag3 = cmdError.Contains("正在战役中") || cmdError.Contains("正在戰役中");
						if (flag3)
						{
							result = 2;
						}
						else
						{
							bool flag4 = cmdError.Contains("还没有冷却") || cmdError.Contains("還沒有冷卻");
							if (flag4)
							{
								result = 3;
							}
							else
							{
								bool flag5 = cmdError.Contains("没有可用的军令") || cmdError.Contains("沒有可用的軍令");
								if (flag5)
								{
									result = 4;
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
			return result;
		}

		private bool completeCampaign(ProtocolMgr protocol, ILogger logger)
		{
			string url = "/root/campaign!attackAll.action";
			ServerResult xml = protocol.getXml(url, "自动战役");
			return xml != null && xml.CmdSucceed;
		}

		private string getCampaignInfo(ProtocolMgr protocol, ILogger logger, out int completed)
		{
			completed = 0;
			string url = "/root/campaign!getCampaignNewInfo.action";
			ServerResult xml = protocol.getXml(url, "获取战役信息");
			bool flag = xml == null || !xml.CmdSucceed;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				XmlDocument cmdResult = xml.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results/finish");
				bool flag2 = xmlNode != null && xmlNode.InnerText.Equals("0");
				if (flag2)
				{
					result = "";
				}
				else
				{
					completed = 1;
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/finishreward");
					foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
					{
						bool flag3 = xmlNode3.Name == "copper";
						if (flag3)
						{
							num4 += int.Parse(xmlNode3.InnerText);
						}
						else
						{
							bool flag4 = xmlNode3.Name == "baoshi";
							if (flag4)
							{
								num += int.Parse(xmlNode3.InnerText);
							}
							else
							{
								bool flag5 = xmlNode3.Name == "bowlder";
								if (flag5)
								{
									num5 += int.Parse(xmlNode3.InnerText);
								}
								else
								{
									bool flag6 = xmlNode3.Name == "tf";
									if (flag6)
									{
										num3 += int.Parse(xmlNode3.InnerText);
									}
									else
									{
										bool flag7 = xmlNode3.Name == "jungong";
										if (flag7)
										{
											num2 += int.Parse(xmlNode3.InnerText);
										}
									}
								}
							}
						}
					}
					XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/reward");
					foreach (XmlNode xmlNode5 in xmlNode4.ChildNodes)
					{
						bool flag8 = xmlNode5.Name == "copper";
						if (flag8)
						{
							num4 += int.Parse(xmlNode5.InnerText);
						}
						else
						{
							bool flag9 = xmlNode5.Name == "baoshi";
							if (flag9)
							{
								num += int.Parse(xmlNode5.InnerText);
							}
							else
							{
								bool flag10 = xmlNode5.Name == "bowlder";
								if (flag10)
								{
									num5 += int.Parse(xmlNode5.InnerText);
								}
								else
								{
									bool flag11 = xmlNode5.Name == "tf";
									if (flag11)
									{
										num3 += int.Parse(xmlNode5.InnerText);
									}
									else
									{
										bool flag12 = xmlNode5.Name == "jungong";
										if (flag12)
										{
											num2 += int.Parse(xmlNode5.InnerText);
										}
									}
								}
							}
						}
					}
					result = string.Format("银币+{0}, 玉石+{1}, 军工+{2}, 突飞令+{3}, 宝石+{4}", new object[]
					{
						num4,
						num5,
						num2,
						num3,
						num
					});
				}
			}
			return result;
		}

		private bool getCampaignReward(ProtocolMgr protocol, ILogger logger, int mode)
		{
			string url = "/root/campaign!getCampaignNewReward.action";
			string data = "mode=" + mode;
			ServerResult serverResult = protocol.postXml(url, data, "获取战役奖励");
			return serverResult != null && serverResult.CmdSucceed;
		}
	}
}
