using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class BoatExe : ExeBase
	{
		//private string _lastMemberName = "";

		public BoatExe()
		{
			this._name = "boat";
			this._readable = "龙舟";
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			long result;
			if (!config.ContainsKey("boatenabled") || !config["boatenabled"].ToLower().Equals("true"))
			{
				result = base.an_hour_later();
			}
			else
			{
				ActivityMgr activityManager = this._factory.getActivityManager();
				bool selfCreateTeam = config.ContainsKey("boatcreate") && config["boatcreate"].ToLower().Equals("true");
				string upgrades = "";
				if (config.ContainsKey("boatupgrade"))
				{
					upgrades = config["boatupgrade"];
				}
				bool selectTime = config.ContainsKey("boatselect") && config["boatselect"].ToLower().Equals("true");
				int maxcoin = 0;
				if (config.ContainsKey("boatcoin"))
				{
					int.TryParse(config["boatcoin"], out maxcoin);
				}
				int goldAvailable = base.getGoldAvailable();
				int num = activityManager.handleBoatEvent(this._proto, this._logger, ref goldAvailable, selfCreateTeam, upgrades, selectTime, maxcoin);
				if (num == 0)
				{
					result = base.next_day();
				}
                else if (num == 1)
                {
                    result = 10000L;
                }
                else if (num == 2)
                {
                    //result = base.next_boat();
                    result = 1000L;
                }
                else if (num == 3)
                {
                    result = 3000L;
                }
                else if (num == 10)
                {
                    result = base.next_day();
                }
                else
                {
                    result = 3000L;
                }
			}
			return result;
		}
	}
}
