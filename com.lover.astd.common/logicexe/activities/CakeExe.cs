using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class CakeExe : ExeBase
	{
		public CakeExe()
		{
			this._name = "cake";
			this._readable = "月饼活动";
		}

		public override long execute()
		{
			ActivityMgr activityManager = this._factory.getActivityManager();
			bool flag = !this._user.isActivityRunning(ActivityType.CakeEvent);
			long result;
			if (flag)
			{
				result = base.an_hour_later();
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
					int num = activityManager.handleCakeEventInfo(this._proto, this._logger);
					bool flag3 = num == 10;
					if (flag3)
					{
						result = base.next_day();
					}
					else
					{
						bool flag4 = num == 2;
						if (flag4)
						{
							result = base.an_hour_later();
						}
						else
						{
							bool flag5 = num == 0;
							if (flag5)
							{
								result = base.immediate();
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
