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
			Building result;
			if (buildings == null || buildings.Count == 0)
			{
				result = null;
			}
			else
			{
				foreach (Building current in buildings)
				{
					if (current.Id == buildingId)
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
			Building result;
			if (buildings == null || buildings.Count == 0)
			{
				result = null;
			}
			else
			{
				foreach (Building current in buildings)
				{
					if (current.Name.Contains(buildingName))
					{
						result = current;
						return result;
					}
				}
				result = null;
			}
			return result;
		}

        public Building getBuildingByBuildingId(List<Building> buildings, int buildingId)
        {
            Building result;
            if (buildings == null || buildings.Count == 0)
            {
                result = null;
            }
            else
            {
                foreach (Building current in buildings)
                {
                    if (current.BuildingId == buildingId)
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
			int id = 0;
			XmlNode xmlNode = building_node.SelectSingleNode("id");
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.InnerText, out id);
			}
			if (id != 0)
			{
				Building building = this.getBuilding(buildings, id);
				if (building == null)
				{
					building = new Building();
					buildings.Add(building);
				}
				building.fillValues(building_node.ChildNodes);
			}
		}

        private void renderMoziBuildingNode(XmlNode building_node, List<Building> buildings)
        {
            int id = 0;
            XmlNode xmlNode = building_node.SelectSingleNode("id");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out id);
            }
            if (id != 0)
            {
                Building building = this.getBuildingByBuildingId(buildings, id);
                if (building != null)
                {
                    building.fillMoziValues(building_node.ChildNodes);
                }
            }
        }

		public bool getBuildingMainCity(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/mainCity.action";
			ServerResult xml = protocol.getXml(url, "获取玩家主城信息");
			if (xml == null || !xml.CmdSucceed)
			{
				return false;
			}

            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            foreach (XmlNode xmlNode2 in childNodes)
            {
                if (xmlNode2.Name == "maincitydto")
                {
                    this.renderBuildingNode(xmlNode2, user._buildings);
                }
                else if (xmlNode2.Name == "constructordto")
                {
                    this.updateBuildLine(xmlNode2.ChildNodes, user._buildingLines);
                }
                else if (xmlNode2.Name == "mozibuilding")
                {
                    this.renderMoziBuildingNode(xmlNode2, user._buildings);
                }
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/is51");
            if (xmlNode3 != null && xmlNode3.InnerText == "1")
            {
                user.addActivity(ActivityType.WeaponEvent);
            }
            else
            {
                user.removeActivity(ActivityType.WeaponEvent);
            }
            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/hasarchevent");
            if (xmlNode4 != null && xmlNode4.InnerText == "1")
            {
                user.addActivity(ActivityType.ArchEvent);
            }
            else
            {
                user.removeActivity(ActivityType.ArchEvent);
            }
            XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/superfanpai");
            if (xmlNode5 != null && xmlNode5.InnerText == "1")
            {
                user.addActivity(ActivityType.SuperFanpai);
            }
            else
            {
                user.removeActivity(ActivityType.SuperFanpai);
            }
            XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/hasjailevent");
            if (xmlNode6 != null && xmlNode6.InnerText == "1")
            {
                user.addActivity(ActivityType.JailEvent);
            }
            else
            {
                user.removeActivity(ActivityType.JailEvent);
            }
            XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/hastroopfeedback");
            if (xmlNode7 != null && xmlNode7.InnerText == "1")
            {
                user.addActivity(ActivityType.TroopFeedback);
            }
            else
            {
                user.removeActivity(ActivityType.TroopFeedback);
            }
            XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/troopturntableevent");
            if (xmlNode8 != null && xmlNode8.InnerText == "1")
            {
                user.addActivity(ActivityType.TroopTurntable);
            }
            else
            {
                user.removeActivity(ActivityType.TroopTurntable);
            }
            XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/hascakeevent");
            if (xmlNode9 != null && xmlNode9.InnerText == "true")
            {
                user.addActivity(ActivityType.CakeEvent);
            }
            else
            {
                user.removeActivity(ActivityType.CakeEvent);
            }
            XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/goldgifttype");
            if (xmlNode10 != null)
            {
                if (xmlNode10.InnerText == "2")
                {
                    user.addActivity(ActivityType.GemDump);
                }
            }
            else
            {
                user.removeActivity(ActivityType.GemDump);
            }
            XmlNode xmlNode11 = cmdResult.SelectSingleNode("/results/showkfwd");
            if (xmlNode11 != null && !xmlNode11.InnerText.Equals("0"))
            {
                user.addActivity(ActivityType.PlayerCompete);
                if (xmlNode11.InnerText == "3")
                {
                    user.addActivity(ActivityType.PlayerCompeteEvent);
                }
            }
            XmlNode xmlNode12 = cmdResult.SelectSingleNode("/results/kfwdeventreward");
            if (xmlNode12 != null && !xmlNode12.InnerText.Equals("0"))
            {
                user.addActivity(ActivityType.PlayerCompeteEvent);
            }
            XmlNode xmlNode13 = cmdResult.SelectSingleNode("/results/showkfpvp");
            if (xmlNode13 != null && !xmlNode13.InnerText.Equals("0"))
            {
                user.addActivity(ActivityType.KfPvp);
            }
            XmlNode xmlNode14 = cmdResult.SelectSingleNode("/results/kfpvpshow");
            if (xmlNode14 != null && !xmlNode14.InnerText.Equals("0"))
            {
                user.addActivity(ActivityType.KfPvp);
            }
            return true;
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
