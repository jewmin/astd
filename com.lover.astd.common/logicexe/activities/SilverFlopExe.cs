using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class SilverFlopExe : ExeBase
	{
		public SilverFlopExe()
		{
			this._name = "silver_flop";
			this._readable = "银币翻牌";
		}

		public override long execute()
		{
			bool flag = !this._user.isActivityRunning(ActivityType.SilverFlop);
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
					ActivityMgr activityManager = this._factory.getActivityManager();
					int num = activityManager.handleSilverFlopInfo(this._proto, this._logger);
					bool flag3 = num == 2;
					if (flag3)
					{
						result = base.next_hour();
					}
					else
					{
						bool flag4 = num == 10;
						if (flag4)
						{
							this._user.removeActivity(ActivityType.SilverFlop);
							result = base.next_day();
						}
						else
						{
							bool flag5 = num != 1;
							if (flag5)
							{
								result = base.immediate();
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
				}
			}
			return result;
		}
	}
}
