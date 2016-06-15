using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class TowerExe : ExeBase
	{
		public TowerExe()
		{
			this._name = "tower";
			this._readable = "宝塔活动";
		}

		public override long execute()
		{
			ActivityMgr activityManager = this._factory.getActivityManager();
			Dictionary<string, string> config = base.getConfig();
			bool flag = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
			long result;
			if (flag)
			{
				result = base.an_hour_later();
			}
			else
			{
				int num = 1;
				bool flag2 = config.ContainsKey("tower_type");
				if (flag2)
				{
					int.TryParse(config["tower_type"], out num);
					bool flag3 = num < 1 || num > 4;
					if (flag3)
					{
						num = 1;
					}
				}
				int num2 = activityManager.handleTowerEventInfo(this._proto, this._logger, num);
				bool flag4 = num2 == 10;
				if (flag4)
				{
					result = base.next_day();
				}
				else
				{
					bool flag5 = num2 != 1;
					if (flag5)
					{
						result = base.next_hour();
					}
					else
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
				}
			}
			return result;
		}
	}
}
