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

        private bool CanBuild(Building build, bool protect, int maincitylevel, int outcitylevel, int silverAvailable, int stone)
        {
            if (build != null && build.Level != this._user.Level && build.Name.IndexOf("监狱") < 0 && (!build.IsOutCity || build.Name.IndexOf("采集场") >= 0 || build.Level != outcitylevel) && (!protect || build.IsOutCity || build.Name.IndexOf("民居") >= 0 || build.Level < maincitylevel - 1) && (!build.IsOutCity || stone >= build.UpgradeCost) && (build.IsOutCity || silverAvailable >= build.UpgradeCost))
            {
                return true;
            }

            return false;
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

            //按cd时间分两列
            List<Building> low_buildings = new List<Building>();
            List<Building> high_buildings = new List<Building>();
            foreach (Building b in this._user._buildings)
            {
                if (b.Cdtime < 240) low_buildings.Add(b);
                else high_buildings.Add(b);
            }

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

            //没有建造队列
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
                }
            }
            else
            {
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
            }

            stone *= 1000;

            if (this._factory.getBuildingManager().hasReadyCdLine(this._user._buildingLines))//建造队列已有建造时间，但未cd前，优先建造高cd时间的
            {
                foreach (int current3 in buildingsToBuild)
                {
                    Building building3 = this._factory.getBuildingManager().getBuilding(high_buildings, current3);
                    if (CanBuild(building3, protect, maincitylevel, outcitylevel, silverAvailable, stone))
                    {
                        building2 = building3;
                        break;
                    }
                }

                if (building2 == null)
                {
                    foreach (int current3 in buildingsToBuild)
                    {
                        Building building3 = this._factory.getBuildingManager().getBuilding(low_buildings, current3);
                        if (CanBuild(building3, protect, maincitylevel, outcitylevel, silverAvailable, stone))
                        {
                            building2 = building3;
                            break;
                        }
                    }
                }
            }
            else//优先建造低cd时间的
            {
                foreach (int current3 in buildingsToBuild)
                {
                    Building building3 = this._factory.getBuildingManager().getBuilding(low_buildings, current3);
                    if (CanBuild(building3, protect, maincitylevel, outcitylevel, silverAvailable, stone))
                    {
                        building2 = building3;
                        break;
                    }
                }

                if (building2 == null)
                {
                    foreach (int current3 in buildingsToBuild)
                    {
                        Building building3 = this._factory.getBuildingManager().getBuilding(high_buildings, current3);
                        if (CanBuild(building3, protect, maincitylevel, outcitylevel, silverAvailable, stone))
                        {
                            building2 = building3;
                            break;
                        }
                    }
                }
            }

            //foreach (int current3 in buildingsToBuild)
            //{
            //    Building building3 = this._factory.getBuildingManager().getBuilding(this._user._buildings, current3);
            //    if (CanBuild(building3, protect, maincitylevel, outcitylevel, silverAvailable, stone))
            //    {
            //        building2 = building3;
            //        break;
            //    }
            //}

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
