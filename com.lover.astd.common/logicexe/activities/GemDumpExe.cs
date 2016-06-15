using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class GemDumpExe : ExeBase
	{
		public GemDumpExe()
		{
			this._name = "gemdump";
			this._readable = "宝石倾销";
		}

		public override long execute()
		{
			ActivityMgr activityManager = this._factory.getActivityManager();
			long result;
			if (!this._user.isActivityRunning(ActivityType.GemDump))
			{
				result = base.an_hour_later();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
				{
					result = base.an_hour_later();
				}
				else
				{
					int num = activityManager.gemDump_handleInfo(this._proto, this._logger, this._user);
					if (num == 10)
					{
						result = base.next_day();
					}
                    else if (num == 2)
                    {
                        result = base.an_hour_later();
                    }
                    else if (num == 0)
                    {
                        result = base.immediate();
                    }
                    else
                    {
                        result = base.next_hour();
                    }
				}
			}
			return result;
		}
	}
}
