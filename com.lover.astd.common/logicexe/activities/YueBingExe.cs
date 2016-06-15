using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class YueBingExe : ExeBase
	{
		public YueBingExe()
		{
			this._name = "yuebing";
			this._readable = "国庆阅兵";
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			long result;
			if (!config.ContainsKey("yuebingenabled") || !config["yuebingenabled"].ToLower().Equals("true"))
			{
				result = base.next_day();
			}
			else
			{
				ActivityMgr activityManager = this._factory.getActivityManager();
				bool bGoldTrain = config.ContainsKey("goldtrain") && config["goldtrain"].ToLower().Equals("true");
				bool bAutoOpenYuebingHongbao = config.ContainsKey("yuebinghongbao") && config["yuebinghongbao"].ToLower().Equals("true");
				int boxtype = 0;
				if (config.ContainsKey("yuebingboxtype"))
				{
					int.TryParse(config["yuebingboxtype"], out boxtype);
					if (boxtype < 0 || boxtype > 3)
					{
                        boxtype = 0;
					}
				}
				int goldAvailable = base.getGoldAvailable();
                int yuebingresult = activityManager.handleYueBingEvent(this._proto, this._logger, ref goldAvailable, bGoldTrain, bAutoOpenYuebingHongbao, boxtype);
				if (yuebingresult == 0)
				{
					result = base.next_day();
				}
                else if (yuebingresult == 1)
                {
                    result = 10000L;
                }
                else if (yuebingresult == 2)
                {
                    result = base.next_boat();
                }
                else if (yuebingresult == 3)
                {
                    result = 3000L;
                }
                else if (yuebingresult == 10)
                {
                    result = base.next_day();
                }
                else
                {
                    result = 3000L;
                }
			}
			return result;
		}
	}
}
