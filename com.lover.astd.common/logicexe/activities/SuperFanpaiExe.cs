using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class SuperFanpaiExe : ExeBase
	{
		public SuperFanpaiExe()
		{
			this._name = "super_fanpai";
			this._readable = "超级翻牌";
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
				bool flag2 = !this._user.isActivityRunning(ActivityType.SuperFanpai);
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
						int max_buy_gold = 0;
						bool flag4 = config.ContainsKey("max_buy");
						if (flag4)
						{
							int.TryParse(config["max_buy"], out max_buy_gold);
						}
						ActivityMgr activityManager = this._factory.getActivityManager();
						int num = activityManager.handle_SuperFanpai(this._proto, this._logger, base.getGemPrice(), base.getGoldAvailable(), max_buy_gold);
						bool flag5 = num == 10;
						if (flag5)
						{
							this._user.removeActivity(ActivityType.SuperFanpai);
							result = base.next_day();
						}
						else
						{
							this._user.addActivity(ActivityType.SuperFanpai);
							bool flag6 = num == 2;
							if (flag6)
							{
								result = base.next_day();
							}
							else
							{
								bool flag7 = num != 1;
								if (flag7)
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
			}
			return result;
		}
	}
}
