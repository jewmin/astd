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
			if (this._user.Level < User.Level_Gem_Events)
			{
				return base.next_hour();
			}
            else if (!this._user.isActivityRunning(ActivityType.SuperFanpai))
            {
                return base.next_hour();
            }
            else
            {
                Dictionary<string, string> config = base.getConfig();
                if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
                {
                    return base.an_hour_later();
                }
                else
                {
                    int max_buy_gold = 0;
                    if (config.ContainsKey("max_buy"))
                    {
                        int.TryParse(config["max_buy"], out max_buy_gold);
                    }
                    ActivityMgr activityManager = this._factory.getActivityManager();
                    int num = activityManager.handle_SuperFanpai(this._proto, this._logger, base.getGemPrice(), base.getGoldAvailable(), max_buy_gold);
                    if (num == 10)
                    {
                        this._user.removeActivity(ActivityType.SuperFanpai);
                        return base.next_day();
                    }
                    else
                    {
                        this._user.addActivity(ActivityType.SuperFanpai);
                        if (num == 2)
                        {
                            return base.next_day();
                        }
                        else if (num != 1)
                        {
                            return base.immediate();
                        }
                        else if (this._isRunningServer)
                        {
                            return 60000L;
                        }
                        else
                        {
                            return base.next_hour();
                        }
                    }
                }
            }
		}
	}
}
