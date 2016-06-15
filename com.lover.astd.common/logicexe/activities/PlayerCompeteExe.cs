using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class PlayerCompeteExe : ExeBase
	{
		public PlayerCompeteExe()
		{
			this._name = "player_compete";
			this._readable = "跨服武斗";
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			long result;
			if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
			{
				return base.an_hour_later();
			}
            bool buy_reward = config.ContainsKey("buy_reward") && config["buy_reward"].ToLower().Equals("true");
            bool openbox = config.ContainsKey("auto_open_box") && config["auto_open_box"].ToLower().Equals("true");
            int open_box_type = 0;
            if (config.ContainsKey("open_box_type"))
            {
                int.TryParse(config["open_box_type"], out open_box_type);
            }
            if (open_box_type == 0)
            {
                open_box_type = 0;
            }
            else if (open_box_type == 1)
            {
                open_box_type = 2;
            }
            else if (open_box_type == 2)
            {
                open_box_type = 4;
            }
            else if (open_box_type == 3)
            {
                open_box_type = 10;
            }
            ActivityMgr activityManager = this._factory.getActivityManager();
            long time = base.next_day();
            long time2 = base.next_day();
            long time3 = base.next_day();
            long time4 = base.next_day();
            if (this._user.isActivityRunning(ActivityType.PlayerCompete))
            {
                bool isKfPvp = false;
                int num2 = activityManager.handlePlayerCompeteSignupInfo(this._proto, this._logger, this._user, buy_reward, openbox, open_box_type, base.getGoldAvailable(), isKfPvp);
                if (num2 == 1)
                {
                    if (!this._isRunningServer)
                    {
                        time = base.next_hour();
                    }
                }
                else if (num2 == 10)
                {
                    time = base.next_day();
                }
                else
                {
                    time = base.next_hour();
                }
            }
            if (this._user.isActivityRunning(ActivityType.KfPvp))
            {
                bool isKfPvp2 = true;
                int num3 = activityManager.handlePlayerCompeteSignupInfo(this._proto, this._logger, this._user, buy_reward, openbox, open_box_type, base.getGoldAvailable(), isKfPvp2);
                if (num3 == 1)
                {
                    if (!this._isRunningServer)
                    {
                        time2 = base.next_hour();
                    }
                }
                else if (num3 == 10)
                {
                    time2 = base.next_day();
                }
                else
                {
                    time2 = base.next_hour();
                }
                int num6 = activityManager.kfpvp_getMatchDetail(this._proto, this._logger);
                if (num6 > 0)
                {
                    time4 = (long)num6;
                }
                else if (num6 == -1)
                {
                    if (!this._isRunningServer)
                    {
                        time4 = base.next_hour();
                    }
                }
                else if (num6 == -2)
                {
                    time4 = base.next_day();
                }
                else
                {
                    time4 = base.next_hour();
                }
            }
            if (this._user.isActivityRunning(ActivityType.PlayerCompeteEvent))
            {
                int num4 = activityManager.handlePlayerCompeteEventInfo(this._proto, this._logger, buy_reward, base.getGoldAvailable());
                if (num4 == 1)
                {
                    if (!this._isRunningServer)
                    {
                        time3 = base.next_hour();
                    }
                }
                else if (num4 == 10)
                {
                    this._user.removeActivity(ActivityType.PlayerCompeteEvent);
                    time3 = base.next_day();
                }
                else
                {
                    time3 = base.next_hour();
                }
            }
            return base.smallest_time(new List<long> { time, time2, time3, time4 });
		}
	}
}
