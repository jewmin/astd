using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class ArchExe : ExeBase
	{
		private string _lastMemberName = "";

		public ArchExe()
		{
			this._name = "arch";
			this._readable = "考古";
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
				ActivityMgr activityManager = this._factory.getActivityManager();
				User user = this._user;
				bool flag2 = !user.isActivityRunning(ActivityType.ArchEvent);
				if (flag2)
				{
					result = base.next_hour();
				}
				else
				{
					bool selfCreateTeam = config.ContainsKey("create") && config["create"].ToLower().Equals("true");
					string upgrades = "";
					bool flag3 = config.ContainsKey("upgrade");
					if (flag3)
					{
						upgrades = config["upgrade"];
					}
					int goldAvailable = base.getGoldAvailable();
					int archInfo = activityManager.getArchInfo(this._proto, this._logger, ref goldAvailable, selfCreateTeam, upgrades, ref this._lastMemberName, false);
					bool flag4 = archInfo == 10;
					if (flag4)
					{
						user.removeActivity(ActivityType.ArchEvent);
						result = base.next_day();
					}
					else
					{
						bool flag5 = archInfo == 2;
						if (flag5)
						{
							result = base.next_hour();
						}
						else
						{
							result = 5000L;
						}
					}
				}
			}
			return result;
		}
	}
}
