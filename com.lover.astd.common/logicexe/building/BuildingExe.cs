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
			bool flag = this._user.Level > 100;
			if (flag)
			{
				this._factory.getBuildingManager().getBuildingsOutCity(this._proto, this._logger, this._user);
			}
			this.refreshUi();
		}

		private List<int> getBuildingsToBuild()
		{
			Dictionary<string, string> config = base.getConfig();
			string id_string = "";
			bool flag = config.ContainsKey("build_ids");
			if (flag)
			{
				id_string = config["build_ids"];
			}
			return base.generateIds(id_string);
		}

		public override void init_data()
		{
			this.getAllBuildings();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			bool flag = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
			long result;
			if (flag)
			{
				result = base.an_hour_later();
			}
			else
			{
				bool flag2 = config.ContainsKey("protect") && config["protect"].ToLower().Equals("true");
				bool flag3 = this._user.isActivityRunning(ActivityType.ArchEvent);
				if (flag3)
				{
					Building building = this._factory.getBuildingManager().getBuilding(this._user._buildings, "驿站");
					bool flag4 = building != null && building.Level < this._user.Level;
					if (flag4)
					{
						this.logInfo("当前为考古时间, 驿站尚未升满, 暂停其他建筑");
						result = base.next_hour();
						return result;
					}
				}
				this.getAllBuildings();
				bool flag5 = !this._factory.getBuildingManager().hasEmptyLine(this._user._buildingLines);
				if (flag5)
				{
					result = this._factory.getBuildingManager().recentLineCd(this._user._buildingLines);
				}
				else
				{
					int silverAvailable = base.getSilverAvailable();
					int num = base.getStoneAvailable();
					List<int> buildingsToBuild = this.getBuildingsToBuild();
					Building building2 = null;
					int num2 = 1000000;
					int num3 = 100000;
					bool flag6 = flag2;
					if (flag6)
					{
						using (List<Building>.Enumerator enumerator = this._user._buildings.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Building current = enumerator.Current;
								bool isOutCity = current.IsOutCity;
								if (isOutCity)
								{
									bool flag7 = current.Name.IndexOf("采集场") >= 0;
									if (flag7)
									{
										num3 = current.Level;
									}
								}
								else
								{
									bool flag8 = current.Name.IndexOf("民居") >= 0 && current.Level < num2;
									if (flag8)
									{
										num2 = current.Level;
									}
								}
							}
							goto IL_28B;
						}
					}
					foreach (Building current2 in this._user._buildings)
					{
						bool isOutCity2 = current2.IsOutCity;
						if (isOutCity2)
						{
							bool flag9 = current2.Name.IndexOf("采集场") >= 0;
							if (flag9)
							{
								num3 = current2.Level;
								break;
							}
							break;
						}
					}
					IL_28B:
					num *= 1000;
					foreach (int current3 in buildingsToBuild)
					{
						Building building3 = this._factory.getBuildingManager().getBuilding(this._user._buildings, current3);
						bool flag10 = building3 != null && building3.Level != this._user.Level && building3.Name.IndexOf("监狱") < 0 && (!building3.IsOutCity || building3.Name.IndexOf("采集场") >= 0 || building3.Level != num3) && (!flag2 || building3.IsOutCity || building3.Name.IndexOf("民居") >= 0 || building3.Level < num2 - 1) && (!building3.IsOutCity || num >= building3.UpgradeCost) && (building3.IsOutCity || silverAvailable >= building3.UpgradeCost);
						if (flag10)
						{
							building2 = building3;
							break;
						}
					}
					bool flag11 = building2 == null;
					if (flag11)
					{
						result = base.next_halfhour();
					}
					else
					{
						this._factory.getBuildingManager().upgradeBuilding(this._proto, this._logger, building2, this._user._buildingLines);
						result = base.immediate();
					}
				}
			}
			return result;
		}
	}
}
