using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class TroopFeedbackExe : ExeBase
	{
		public TroopFeedbackExe()
		{
			this._name = "troop_feedback";
			this._readable = "兵器回馈";
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
				bool refine_notired = config.ContainsKey("refine_notired") && config["refine_notired"].ToLower().Equals("true");
				bool doubleweapon = config.ContainsKey("double_weapon") && config["double_weapon"].ToLower().Equals("true");
				bool flag2 = config.ContainsKey("open_treasure") && config["open_treasure"].ToLower().Equals("true");
				bool openbox = config.ContainsKey("auto_open_box") && config["auto_open_box"].ToLower().Equals("true");
				int num = 0;
				bool flag3 = config.ContainsKey("boxtype");
				if (flag3)
				{
					int.TryParse(config["boxtype"], out num);
					bool flag4 = num < 0 || num >= 2;
					if (flag4)
					{
						num = 0;
					}
				}
				ActivityMgr activityManager = this._factory.getActivityManager();
				User user = this._user;
				bool flag5 = !user.isActivityRunning(ActivityType.TroopFeedback);
				if (flag5)
				{
					result = base.next_hour();
				}
				else
				{
					int goldAvailable = base.getGoldAvailable();
					int num2 = activityManager.handleTroopFeedbackInfo(this._proto, this._logger, ref goldAvailable, refine_notired, doubleweapon, openbox, num);
					bool flag6 = num2 == 10;
					if (flag6)
					{
						user.removeActivity(ActivityType.TroopFeedback);
						result = base.next_day();
					}
					else
					{
						user.addActivity(ActivityType.TroopFeedback);
						bool flag7 = flag2;
						if (flag7)
						{
							this._factory.getMiscManager().handleNationTreasureInfo(this._proto, this._logger, 0);
						}
						bool flag8 = num2 == 2;
						if (flag8)
						{
							result = base.next_hour();
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
