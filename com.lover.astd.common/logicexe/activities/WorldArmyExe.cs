using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class WorldArmyExe : ExeBase
	{
		public WorldArmyExe()
		{
			this._name = "worldarmy";
			this._readable = "世界军团";
		}

		public override long execute()
		{
			bool flag = !this._user.isActivityRunning(ActivityType.WorldArmy) && !this._user.isActivityRunning(ActivityType.WorldArmyZongzi);
			long result;
			if (flag)
			{
				result = base.next_hour();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				bool flag2 = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
				if (flag2)
				{
					result = base.an_hour_later();
				}
				else
				{
					int refresh_type = 0;
					bool flag3 = config.ContainsKey("refresh");
					if (flag3)
					{
						int.TryParse(config["refresh"], out refresh_type);
					}
					bool buy_box = config.ContainsKey("buybox1") && config["buybox1"].ToLower().Equals("true");
					bool buy_box2 = config.ContainsKey("buybox2") && config["buybox2"].ToLower().Equals("true");
					ActivityMgr activityManager = this._factory.getActivityManager();
					bool auto_open_box = config.ContainsKey("auto_open_box") && config["auto_open_box"].ToLower().Equals("true");
					int open_box_type = 0;
					bool flag4 = config.ContainsKey("boxtype");
					if (flag4)
					{
						int.TryParse(config["boxtype"], out open_box_type);
					}
					int goldAvailable = base.getGoldAvailable();
					long num = 0L;
					bool is_zongzi = this._user.isActivityRunning(ActivityType.WorldArmyZongzi);
					int num2 = activityManager.handleWorldArmyInfo(this._proto, this._logger, is_zongzi, ref goldAvailable, auto_open_box, open_box_type, buy_box, buy_box2, refresh_type, out num);
					bool flag5 = num2 == 2 || num2 == 10;
					if (flag5)
					{
						result = base.next_day();
					}
					else
					{
						bool flag6 = num2 == 1;
						if (flag6)
						{
							bool isRunningServer = this._isRunningServer;
							if (isRunningServer)
							{
								result = 60000L;
							}
							else
							{
								result = base.next_hour();
							}
						}
						else
						{
							bool flag7 = num > 0L;
							if (flag7)
							{
								result = num;
							}
							else
							{
								result = base.immediate();
							}
						}
					}
				}
			}
			return result;
		}
	}
}
