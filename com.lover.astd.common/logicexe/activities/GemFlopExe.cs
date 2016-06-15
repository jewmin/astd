using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class GemFlopExe : ExeBase
	{
		public GemFlopExe()
		{
			this._name = "gem_flop";
			this._readable = "宝石翻牌";
		}

		public override long execute()
		{
			bool flag = this._user.Level < User.Level_Gem_Events;
			long result;
			if (flag)
			{
				result = base.next_hour();
			}
			else
			{
				bool flag2 = !this._user.isActivityRunning(ActivityType.GemFlop);
				if (flag2)
				{
					result = base.next_hour();
				}
				else
				{
					Dictionary<string, string> config = base.getConfig();
					bool flag3 = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
					if (flag3)
					{
						result = base.an_hour_later();
					}
					else
					{
						int max_upgrade = 0;
						bool flag4 = config.ContainsKey("max_upgrade");
						if (flag4)
						{
							int.TryParse(config["max_upgrade"], out max_upgrade);
						}
						ActivityMgr activityManager = this._factory.getActivityManager();
						int num = activityManager.handleGemFlopInfo(this._proto, this._logger, base.getGemPrice(), base.getGoldAvailable(), max_upgrade);
						bool flag5 = num == 2 || num == 10;
						if (flag5)
						{
							result = base.next_day();
						}
						else
						{
							bool flag6 = num != 1;
							if (flag6)
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
