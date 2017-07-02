using com.lover.astd.common.model.building;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.building
{
	public class BuildingExe : ExeBase
	{
		public BuildingExe()
		{
			this._name = "building";
			this._readable = "建筑";
		}

		public void getAllBuildings()
		{
			this._factory.getBuildingManager().getBuildingMainCity(this._proto, this._logger, this._user);
            if (this._user.Level > 100) this._factory.getBuildingManager().getBuildingsOutCity(this._proto, this._logger, this._user);
			this.refreshUi();
		}

		private List<int> getBuildingsToBuild()
		{
			Dictionary<string, string> config = base.getConfig();
			string id_string = "";
            if (config.ContainsKey("build_ids")) id_string = config["build_ids"];
			return base.generateIds(id_string);
		}

		public override void init_data()
		{
			this.getAllBuildings();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
			{
				return base.an_hour_later();
			}

            if ( this._user.isActivityRunning(ActivityType.ArchEvent))
            {
                Building building = this._factory.getBuildingManager().getBuilding(this._user._buildings, "驿站");
                if (building != null && building.Level < this._user.Level)
                {
                    this.logInfo("当前为考古时间, 驿站尚未升满, 暂停其他建筑");
                    return base.next_hour();
                }
            }

            this.getAllBuildings();

            //改造
            foreach (MoziBuilding current in this._user.mozi_buildings_)
            {
                while (current.state == 0 && current.slaves > 0)
                {
                    if (!this._factory.getBuildingManager().constructBuilding(this._proto, this._logger, current)) break;
                }
            }

            if (this._user.remainseniorslaves > 0)
            {
                foreach (MoziBuilding current in this._user.mozi_buildings_)
                {
                    while (current.state == 1 && current.seniorprocess < current.totalseniorprocess)
                    {
                        if (!this._factory.getBuildingManager().upGradeMoziBuild(this._proto, this._logger, this._user, current)) break;
                    }
                }
            }

            if (!this._factory.getBuildingManager().hasEmptyLine(this._user._buildingLines))
            {
                return this._factory.getBuildingManager().recentLineCd(this._user._buildingLines);
            }

            int silverAvailable = base.getSilverAvailable();
            int stone = base.getStoneAvailable();
            List<int> buildingsToBuild = this.getBuildingsToBuild();
            Building building2 = null;
            int maincitylevel = 1000000;
            int outcitylevel = 100000;
            bool protect = config.ContainsKey("protect") && config["protect"].ToLower().Equals("true");
            if (protect)
            {
                using (List<Building>.Enumerator enumerator = this._user._buildings.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Building current = enumerator.Current;
                        if (current.IsOutCity)
                        {
                            if (current.Name.IndexOf("采集场") >= 0)
                            {
                                outcitylevel = current.Level;
                            }
                        }
                        else
                        {
                            if (current.Name.IndexOf("民居") >= 0 && current.Level < maincitylevel)
                            {
                                maincitylevel = current.Level;
                            }
                        }
                    }
                    goto IL_28B;
                }
            }

            foreach (Building current2 in this._user._buildings)
            {
                if (current2.IsOutCity)
                {
                    if (current2.Name.IndexOf("采集场") >= 0)
                    {
                        outcitylevel = current2.Level;
                        break;
                    }
                    break;
                }
            }

        IL_28B:
            stone *= 1000;
            foreach (int current3 in buildingsToBuild)
            {
                Building building3 = this._factory.getBuildingManager().getBuilding(this._user._buildings, current3);
                if (building3 != null && building3.Level != this._user.Level && building3.Name.IndexOf("监狱") < 0 && (!building3.IsOutCity || building3.Name.IndexOf("采集场") >= 0 || building3.Level != outcitylevel) && (!protect || building3.IsOutCity || building3.Name.IndexOf("民居") >= 0 || building3.Level < maincitylevel - 1) && (!building3.IsOutCity || stone >= building3.UpgradeCost) && (building3.IsOutCity || silverAvailable >= building3.UpgradeCost))
                {
                    building2 = building3;
                    break;
                }
            }
            if (building2 == null)
            {
                return base.next_halfhour();
            }
            else
            {
                this._factory.getBuildingManager().upgradeBuilding(this._proto, this._logger, building2, this._user._buildingLines);
                return base.immediate();
            }
		}
	}
}
