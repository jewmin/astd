using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.building;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace com.lover.astd.common.logic
{
	public class BuildingMgr : MgrBase
	{
		public BuildingMgr(TimeMgr tmrMgr, ServiceFactory factory)
		{
			this._logColor = Color.DeepPink;
			this._tmrMgr = tmrMgr;
			this._factory = factory;
		}

		public Building getBuilding(List<Building> buildings, int buildingId)
		{
			bool flag = buildings == null || buildings.Count == 0;
			Building result;
			if (flag)
			{
				result = null;
			}
			else
			{
				foreach (Building current in buildings)
				{
					bool flag2 = current.Id == buildingId;
					if (flag2)
					{
						result = current;
						return result;
					}
				}
				result = null;
			}
			return result;
		}

		public Building getBuilding(List<Building> buildings, string buildingName)
		{
			bool flag = buildings == null || buildings.Count == 0;
			Building result;
			if (flag)
			{
				result = null;
			}
			else
			{
				foreach (Building current in buildings)
				{
					bool flag2 = current.Name.Contains(buildingName);
					if (flag2)
					{
						result = current;
						return result;
					}
				}
				result = null;
			}
			return result;
		}

		private void renderBuildingNode(XmlNode building_node, List<Building> buildings)
		{
			int num = 0;
			XmlNode xmlNode = building_node.SelectSingleNode("id");
			bool flag = xmlNode != null;
			if (flag)
			{
				int.TryParse(xmlNode.InnerText, out num);
			}
			bool flag2 = num == 0;
			if (!flag2)
			{
				Building building = this.getBuilding(buildings, num);
				bool flag3 = building == null;
				if (flag3)
				{
					building = new Building();
					buildings.Add(building);
				}
				building.fillValues(building_node.ChildNodes);
			}
		}

		public bool getBuildingMainCity(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/mainCity.action";
			ServerResult xml = protocol.getXml(url, "获取玩家主城信息");
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
					bool flag2 = xmlNode2.Name == "maincitydto";
					if (flag2)
					{
						this.renderBuildingNode(xmlNode2, user._buildings);
					}
					else
					{
						bool flag3 = xmlNode2.Name == "constructordto";
						if (flag3)
						{
							this.updateBuildLine(xmlNode2.ChildNodes, user._buildingLines);
						}
					}
				}
				XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/is51");
				bool flag4 = xmlNode3 != null && xmlNode3.InnerText == "1";
				if (flag4)
				{
					user.addActivity(ActivityType.WeaponEvent);
				}
				else
				{
					user.removeActivity(ActivityType.WeaponEvent);
				}
				XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/hasarchevent");
				bool flag5 = xmlNode4 != null && xmlNode4.InnerText == "1";
				if (flag5)
				{
					user.addActivity(ActivityType.ArchEvent);
				}
				else
				{
					user.removeActivity(ActivityType.ArchEvent);
				}
				XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/superfanpai");
				bool flag6 = xmlNode5 != null && xmlNode5.InnerText == "1";
				if (flag6)
				{
					user.addActivity(ActivityType.SuperFanpai);
				}
				else
				{
					user.removeActivity(ActivityType.SuperFanpai);
				}
				XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/hasjailevent");
				bool flag7 = xmlNode6 != null && xmlNode6.InnerText == "1";
				if (flag7)
				{
					user.addActivity(ActivityType.JailEvent);
				}
				else
				{
					user.removeActivity(ActivityType.JailEvent);
				}
				XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/hastroopfeedback");
				bool flag8 = xmlNode7 != null && xmlNode7.InnerText == "1";
				if (flag8)
				{
					user.addActivity(ActivityType.TroopFeedback);
				}
				else
				{
					user.removeActivity(ActivityType.TroopFeedback);
				}
				XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/troopturntableevent");
				bool flag9 = xmlNode8 != null && xmlNode8.InnerText == "1";
				if (flag9)
				{
					user.addActivity(ActivityType.TroopTurntable);
				}
				else
				{
					user.removeActivity(ActivityType.TroopTurntable);
				}
				XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/hascakeevent");
				bool flag10 = xmlNode9 != null && xmlNode9.InnerText == "true";
				if (flag10)
				{
					user.addActivity(ActivityType.CakeEvent);
				}
				else
				{
					user.removeActivity(ActivityType.CakeEvent);
				}
				XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/goldgifttype");
				bool flag11 = xmlNode10 != null;
				if (flag11)
				{
					bool flag12 = xmlNode10.InnerText == "2";
					if (flag12)
					{
						user.addActivity(ActivityType.GemDump);
					}
				}
				else
				{
					user.removeActivity(ActivityType.GemDump);
				}
				XmlNode xmlNode11 = cmdResult.SelectSingleNode("/results/showkfwd");
				bool flag13 = xmlNode11 != null && !xmlNode11.InnerText.Equals("0");
				if (flag13)
				{
					user.addActivity(ActivityType.PlayerCompete);
					bool flag14 = xmlNode11.InnerText == "3";
					if (flag14)
					{
						user.addActivity(ActivityType.PlayerCompeteEvent);
					}
				}
				XmlNode xmlNode12 = cmdResult.SelectSingleNode("/results/kfwdeventreward");
				bool flag15 = xmlNode12 != null && !xmlNode12.InnerText.Equals("0");
				if (flag15)
				{
					user.addActivity(ActivityType.PlayerCompeteEvent);
				}
				XmlNode xmlNode13 = cmdResult.SelectSingleNode("/results/showkfpvp");
				bool flag16 = xmlNode13 != null && !xmlNode13.InnerText.Equals("0");
				if (flag16)
				{
					user.addActivity(ActivityType.KfPvp);
				}
				XmlNode xmlNode14 = cmdResult.SelectSingleNode("/results/kfpvpshow");
				bool flag17 = xmlNode14 != null && !xmlNode14.InnerText.Equals("0");
				if (flag17)
				{
					user.addActivity(ActivityType.KfPvp);
				}
				result = true;
			}
			return result;
		}

		public bool getBuildingsOutCity(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/outCity.action";
			ServerResult xml = protocol.getXml(url, "获取玩家外城信息");
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
					bool flag2 = xmlNode2.Name == "outcitydto";
					if (flag2)
					{
						this.renderBuildingNode(xmlNode2, user._buildings);
					}
					else
					{
						bool flag3 = xmlNode2.Name == "constructordto";
						if (flag3)
						{
							this.updateBuildLine(xmlNode2.ChildNodes, user._buildingLines);
						}
					}
				}
				result = true;
			}
			return result;
		}

		private void updateBuildLine(XmlNodeList line_childs, Dictionary<int, BuildingLine> buildingLines)
		{
			BuildingLine buildingLine = new BuildingLine();
			foreach (XmlNode xmlNode in line_childs)
			{
				bool flag = xmlNode.Name == "cid";
				if (flag)
				{
					buildingLine.Id = int.Parse(xmlNode.InnerText);
				}
				else
				{
					bool flag2 = xmlNode.Name == "ctime";
					if (flag2)
					{
						buildingLine.CdTime = long.Parse(xmlNode.InnerText);
					}
					else
					{
						bool flag3 = xmlNode.Name == "cdflag";
						if (flag3)
						{
							buildingLine.CdFlag = int.Parse(xmlNode.InnerText);
						}
					}
				}
			}
			bool flag4 = buildingLines.ContainsKey(buildingLine.Id);
			if (flag4)
			{
				buildingLines[buildingLine.Id].CdTime = buildingLine.CdTime;
				buildingLines[buildingLine.Id].CdFlag = buildingLine.CdFlag;
			}
			else
			{
				buildingLines.Add(buildingLine.Id, buildingLine);
			}
		}

		public bool upgradeBuilding(ProtocolMgr protocol, ILogger logger, Building building, Dictionary<int, BuildingLine> buildingLines)
		{
			string cost = building.getCost();
			string url = "/root/mainCity!upgradeLevel.action";
			bool isOutCity = building.IsOutCity;
			if (isOutCity)
			{
				url = "/root/outCity!upgradeLevel.action";
			}
			string data = "player_BuildingId=" + building.Id;
			ServerResult serverResult = protocol.postXml(url, data, "升级建筑");
			bool flag = serverResult == null || !serverResult.CmdSucceed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
				XmlNodeList childNodes = xmlNode.ChildNodes;
				foreach (XmlNode xmlNode2 in childNodes)
				{
					bool flag2 = xmlNode2.Name == "maincitydto";
					if (flag2)
					{
						XmlNodeList childNodes2 = xmlNode2.ChildNodes;
						building.fillValues(childNodes2);
					}
					else
					{
						bool flag3 = xmlNode2.Name == "outcitydto";
						if (flag3)
						{
							XmlNodeList childNodes3 = xmlNode2.ChildNodes;
							building.fillValues(childNodes3);
						}
						else
						{
							bool flag4 = xmlNode2.Name == "constructordto";
							if (flag4)
							{
								this.updateBuildLine(xmlNode2.ChildNodes, buildingLines);
							}
						}
					}
				}
				base.logInfo(logger, string.Format("升级建筑 {0} {1}=>{2}, 花费{3}", new object[]
				{
					building.Name,
					building.Level - 1,
					building.Level,
					cost
				}));
				result = true;
			}
			return result;
		}

		public bool hasEmptyLine(Dictionary<int, BuildingLine> buildingLines)
		{
			bool result = false;
			foreach (BuildingLine current in buildingLines.Values)
			{
				bool flag = !current.AlreadyCd;
				if (flag)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public long recentLineCd(Dictionary<int, BuildingLine> buildingLines)
		{
			long num = -1L;
			foreach (BuildingLine current in buildingLines.Values)
			{
				bool flag = num < 0L || num > current.CdTime;
				if (flag)
				{
					num = current.CdTime;
				}
			}
			return num;
		}
	}
}
