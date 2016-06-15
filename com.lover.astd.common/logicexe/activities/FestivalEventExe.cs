using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class FestivalEventExe : ExeBase
	{
		public FestivalEventExe()
		{
			this._name = "festival";
			this._readable = "迎军活动";
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
				int num = activityManager.handleFestivalEventInfo(this._proto, this._logger);
				int num2 = 10;
				bool flag2 = num == 10 && num2 == 10;
				if (flag2)
				{
					result = base.next_day();
				}
				else
				{
					result = base.next_hour();
				}
			}
			return result;
		}
	}
}
