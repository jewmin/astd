using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class KfBanquetExe : ExeBase
	{
		public KfBanquetExe()
		{
			this._name = "kf_banquet";
			this._readable = "盛宴活动";
		}

		public override long execute()
		{
			bool flag = !this._user.isActivityRunning(ActivityType.KfBanquet);
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
					bool only_nation = config.ContainsKey("nation") && config["nation"].ToLower().Equals("true");
					bool again = config.ContainsKey("again") && config["again"].ToLower().Equals("true");
					int buy_ticket_gold = 0;
					bool flag3 = config.ContainsKey("buygold");
					if (flag3)
					{
						int.TryParse(config["buygold"], out buy_ticket_gold);
					}
					ActivityMgr activityManager = this._factory.getActivityManager();
					long num = 0L;
					int num2 = activityManager.handleKfBanquetInfo(this._proto, this._logger, this._user, buy_ticket_gold, only_nation, again, base.getGoldAvailable(), out num);
					bool flag4 = num2 == 1;
					if (flag4)
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
						bool flag5 = num2 == 10;
						if (flag5)
						{
							result = base.next_day();
						}
						else
						{
							bool flag6 = num > 0L;
							if (flag6)
							{
								result = num;
							}
							else
							{
								result = base.next_hour();
							}
						}
					}
				}
			}
			return result;
		}
	}
}
