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

        public MoziBuilding getMoziBuilding(List<MoziBuilding> buildings, int buildingId)
        {
            if (buildings == null || buildings.Count == 0) return null;

            foreach (MoziBuilding current in buildings)
            {
                if (current.Id == buildingId) return current;
            }

            return null;
        }

		public Building getBuilding(List<Building> buildings, int buildingId)
		{
			if (buildings == null || buildings.Count == 0)
			{
				return null;
			}

            foreach (Building current in buildings)
            {
                if (current.Id == buildingId)
                {
                    return current;
                }
            }

            return null;
		}

		public Building getBuilding(List<Building> buildings, string buildingName)
		{
            if (buildings == null || buildings.Count == 0)
            {
                return null;
            }

            foreach (Building current in buildings)
            {
                if (current.Name.Contains(buildingName))
                {
                    return current;
                }
            }

            return null;
		}

        public Building getBuildingByBuildingId(List<Building> buildings, int buildingId)
        {
            if (buildings == null || buildings.Count == 0)
            {
                return null;
            }

            foreach (Building current in buildings)
            {
                if (current.BuildingId == buildingId)
                {
                    return current;
                }
            }

            return null;
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

        private void renderMoziBuildingNode(XmlNode building_node, List<Building> buildings, List<MoziBuilding> mozi_buildings)
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

                MoziBuilding mozi_building = this.getMoziBuilding(mozi_buildings, id);
                if (mozi_building == null)
                {
                    mozi_building = new MoziBuilding();
                    mozi_buildings.Add(mozi_building);
                }
                mozi_building.fillValues(building_node.ChildNodes);
            }
        }

        public bool getBuildingMainCity(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/mainCity.action";
            ServerResult xml = protocol.getXml(url, "获取玩家主城信息");
            if (xml == null || !xml.CmdSucceed) return false;

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
                    this.renderMoziBuildingNode(xmlNode2, user._buildings, user.mozi_buildings_);
                }
            }

            user.remainseniorslaves = XmlHelper.GetValue<int>(cmdResult.SelectSingleNode("/results/remainseniorslaves"));

            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/is51");
            if (xmlNode3 != null && xmlNode3.InnerText == "1")
                user.addActivity(ActivityType.WeaponEvent);
            else
                user.removeActivity(ActivityType.WeaponEvent);

            XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/hasarchevent");
            if (xmlNode4 != null && xmlNode4.InnerText == "1")
                user.addActivity(ActivityType.ArchEvent);
            else
                user.removeActivity(ActivityType.ArchEvent);

            XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/superfanpai");
            if (xmlNode5 != null && xmlNode5.InnerText == "1")
                user.addActivity(ActivityType.SuperFanpai);
            else
                user.removeActivity(ActivityType.SuperFanpai);

            XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/hasjailevent");
            if (xmlNode6 != null && xmlNode6.InnerText == "1")
                user.addActivity(ActivityType.JailEvent);
            else
                user.removeActivity(ActivityType.JailEvent);

            XmlNode xmlNode7 = cmdResult.SelectSingleNode("/results/hastroopfeedback");
            if (xmlNode7 != null && xmlNode7.InnerText == "1")
                user.addActivity(ActivityType.TroopFeedback);
            else
                user.removeActivity(ActivityType.TroopFeedback);

            XmlNode xmlNode8 = cmdResult.SelectSingleNode("/results/troopturntableevent");
            if (xmlNode8 != null && xmlNode8.InnerText == "1")
                user.addActivity(ActivityType.TroopTurntable);
            else
                user.removeActivity(ActivityType.TroopTurntable);

            XmlNode xmlNode9 = cmdResult.SelectSingleNode("/results/hascakeevent");
            if (xmlNode9 != null && xmlNode9.InnerText == "true")
                user.addActivity(ActivityType.CakeEvent);
            else
                user.removeActivity(ActivityType.CakeEvent);

            XmlNode xmlNode10 = cmdResult.SelectSingleNode("/results/goldgifttype");
            if (xmlNode10 != null && xmlNode10.InnerText == "2")
                user.addActivity(ActivityType.GemDump);
            else
                user.removeActivity(ActivityType.GemDump);

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

            xmlNode14 = cmdResult.SelectSingleNode("/results/snowtradingevent");
            if (xmlNode14 != null && xmlNode14.InnerText.Equals("1"))
                user.addActivity(ActivityType.SnowTradingEvent);
            else
                user.removeActivity(ActivityType.SnowTradingEvent);

            xmlNode14 = cmdResult.SelectSingleNode("/results/bombnianevent");
            if (xmlNode14 != null && xmlNode14.InnerText.Equals("1"))
                user.addActivity(ActivityType.BombNianEvent);
            else
                user.removeActivity(ActivityType.BombNianEvent);

            return true;
        }

		public bool getBuildingsOutCity(ProtocolMgr protocol, ILogger logger, User user)
		{
			string url = "/root/outCity.action";
			ServerResult xml = protocol.getXml(url, "获取玩家外城信息");
			if (xml == null || !xml.CmdSucceed) return false;

            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            foreach (XmlNode xmlNode2 in childNodes)
            {
                if (xmlNode2.Name == "outcitydto")
                {
                    this.renderBuildingNode(xmlNode2, user._buildings);
                }
                else if (xmlNode2.Name == "constructordto")
                {
                    this.updateBuildLine(xmlNode2.ChildNodes, user._buildingLines);
                }
            }
            return true;
		}

		private void updateBuildLine(XmlNodeList line_childs, Dictionary<int, BuildingLine> buildingLines)
		{
			BuildingLine buildingLine = new BuildingLine();

            foreach (XmlNode xmlNode in line_childs)
            {
                if (xmlNode.Name == "cid")
                {
                    buildingLine.Id = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "ctime")
                {
                    buildingLine.CdTime = long.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "cdflag")
                {
                    buildingLine.CdFlag = int.Parse(xmlNode.InnerText);
                }
            }

			if (buildingLines.ContainsKey(buildingLine.Id))
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
            if (isOutCity) url = "/root/outCity!upgradeLevel.action";
            string data = string.Format("player_BuildingId={0}", building.Id);
			ServerResult serverResult = protocol.postXml(url, data, "升级建筑");
			if (serverResult == null || !serverResult.CmdSucceed) return false;

            XmlDocument cmdResult = serverResult.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            foreach (XmlNode xmlNode2 in childNodes)
            {
                if (xmlNode2.Name == "maincitydto")
                {
                    XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                    building.fillValues(childNodes2);
                }
                else if (xmlNode2.Name == "outcitydto")
                {
                    XmlNodeList childNodes3 = xmlNode2.ChildNodes;
                    building.fillValues(childNodes3);
                }
                else if (xmlNode2.Name == "constructordto")
                {
                    this.updateBuildLine(xmlNode2.ChildNodes, buildingLines);
                }
            }
            base.logInfo(logger, string.Format("升级建筑 {0} {1}=>{2}, 花费{3}", building.Name, building.Level - 1, building.Level, cost));
            return true;
		}

		public bool hasEmptyLine(Dictionary<int, BuildingLine> buildingLines)
		{
			foreach (BuildingLine current in buildingLines.Values)
			{
				if (!current.AlreadyCd)
				{
					return true;
				}
			}

            return false;
		}

        public bool hasReadyCdLine(Dictionary<int, BuildingLine> buildingLines)
        {
            foreach (BuildingLine current in buildingLines.Values)
            {
                if (current.CdTime > 0 && current.CdFlag == 0)
                {
                    return true;
                }
            }

            return false;
        }

		public long recentLineCd(Dictionary<int, BuildingLine> buildingLines)
		{
			long num = -1L;
			foreach (BuildingLine current in buildingLines.Values)
			{
				if (num < 0L || num > current.CdTime)
				{
					num = current.CdTime;
				}
			}
			return num;
		}

        /// <summary>
        /// <results>
        ///     <state>1</state>
        ///     <mozi>
        ///         <buildid>1013</buildid>
        ///         <slaves>0</slaves>
        ///         <process>1</process>
        ///         <state>0</state>
        ///         <totalprocess>10</totalprocess>
        ///         <update>0</update>
        ///     </mozi>
        /// </results>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="building"></param>
        /// <returns></returns>
        public bool constructBuilding(ProtocolMgr protocol, ILogger logger, MoziBuilding building)
        {
            string url = "/root/refine!constructBuilding.action";
            string data = string.Format("buildId={0}", building.buildid);
            ServerResult xml = protocol.postXml(url, data, "改造建筑");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlNode node = xml.CmdResult.SelectSingleNode("/results/mozi");
            if (node != null) building.fillValues(node.ChildNodes);
            logInfo(logger, "改造建筑成功");
            return true;
        }

        /// <summary>
        /// <results>
        ///     <state>1</state>
        ///     <remainseniorslaves>8</remainseniorslaves>
        ///     <lvupto>2</lvupto>
        ///     <seniorprocess>0</seniorprocess>
        ///     <totalseniorprocess>0</totalseniorprocess>
        ///     <canupgrade>2</canupgrade>
        ///     <hpbuff>7</hpbuff>
        /// </results>
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="building"></param>
        /// <returns></returns>
        public bool upGradeMoziBuild(ProtocolMgr protocol, ILogger logger, User user, MoziBuilding building)
        {
            string url = "/root/mainCity!upGradeMoziBuild.action";
            string data = string.Format("player_BuildingId={0}", building.buildid);
            ServerResult xml = protocol.postXml(url, data, "升级建筑");
            if (xml == null || !xml.CmdSucceed) return false;

            user.remainseniorslaves = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/remainseniorslaves"));
            building.lv = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/lvupto"));
            building.seniorprocess = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/seniorprocess"));
            building.totalseniorprocess = XmlHelper.GetValue<int>(xml.CmdResult.SelectSingleNode("/results/totalseniorprocess"));
            logInfo(logger, string.Format("升级建筑成功，升级到lv.{0}", building.lv));
            return true;
        }
	}
}
