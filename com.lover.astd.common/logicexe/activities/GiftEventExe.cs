using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class GiftEventExe : ExeBase
	{
		public GiftEventExe()
		{
			this._name = "giftevent";
			this._readable = "犒赏活动";
		}

		public override long execute()
		{
			bool flag = !this._user.isActivityRunning(ActivityType.GiftEvent);
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
					string text = "231";
					bool flag3 = config.ContainsKey("serial");
					if (flag3)
					{
						text = config["serial"];
						bool flag4 = text == null || text.Trim() == "";
						if (flag4)
						{
							text = "231";
						}
					}
					ActivityMgr activityManager = this._factory.getActivityManager();
					int num = activityManager.handleGiftEventInfo(this._proto, this._logger, text);
					bool flag5 = num == 10;
					if (flag5)
					{
						result = base.next_day();
					}
					else
					{
						bool flag6 = num != 1;
						if (flag6)
						{
							result = base.next_hour();
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
			return result;
		}
	}
}
