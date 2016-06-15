using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class CrossplatformCompeteExe : ExeBase
	{
		public CrossplatformCompeteExe()
		{
			this._name = "crossplatform_compete";
			this._readable = "跨服争霸赛";
		}

		public override long execute()
		{
			bool flag = !this._user.isActivityRunning(ActivityType.CrossplatformCompete);
			long result;
			if (flag)
			{
				result = base.next_hour();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				bool flag2 = config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true");
				bool flag3 = config.ContainsKey("kfzb_market") && config["kfzb_market"].ToLower().Equals("true");
				bool flag4 = !flag2 && !flag3;
				if (flag4)
				{
					result = base.an_hour_later();
				}
				else
				{
					ActivityMgr activityManager = this._factory.getActivityManager();
					long num = 0L;
					long num2 = 0L;
					bool flag5 = flag2;
					if (flag5)
					{
						int max_gold_to_continue_support = 0;
						bool flag6 = config.ContainsKey("support_gold");
						if (flag6)
						{
							int.TryParse(config["support_gold"], out max_gold_to_continue_support);
						}
						activityManager.handleCrossPlatformCompeteInfo(this._proto, this._logger, base.getGoldAvailable(), max_gold_to_continue_support, out num);
					}
					bool flag7 = flag3;
					if (flag7)
					{
						bool ignore_gold_limit = config.ContainsKey("kfzb_market_ignore") && config["kfzb_market_ignore"].ToLower().Equals("true");
						string buy_conf = "";
						bool flag8 = config.ContainsKey("kfzb_market_conf");
						if (flag8)
						{
							buy_conf = config["kfzb_market_conf"];
						}
						Dictionary<string, string> config2 = this._conf.getConfig("global");
						int gold_limit = 0;
						bool flag9 = config2.ContainsKey("gold_reserve");
						if (flag9)
						{
							int.TryParse(config2["gold_reserve"], out gold_limit);
						}
						activityManager.handleKfzbMarketInfo(this._proto, this._logger, this._user, gold_limit, out num2, buy_conf, ignore_gold_limit);
					}
					bool flag10 = flag2 & flag3;
					if (flag10)
					{
						long num3 = base.smaller_time(num, num2);
						bool flag11 = num3 > 0L;
						if (flag11)
						{
							result = num3;
						}
						else
						{
							result = base.immediate();
						}
					}
					else
					{
						bool flag12 = flag2;
						if (flag12)
						{
							bool flag13 = num > 0L;
							if (flag13)
							{
								result = num;
							}
							else
							{
								result = base.immediate();
							}
						}
						else
						{
							bool flag14 = !flag3;
							if (flag14)
							{
								result = base.next_hour();
							}
							else
							{
								bool flag15 = num2 > 0L;
								if (flag15)
								{
									result = num2;
								}
								else
								{
									result = base.immediate();
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
