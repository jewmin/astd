using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;

namespace com.lover.astd.common.logicexe.activities
{
    public class BGExe : ExeBase
    {
        public BGExe()
        {
            this._name = ConfigStrings.S_BG;
            this._readable = ConfigStrings.SR_BG;
        }

        public override long execute()
        {
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey(ConfigStrings.enabled) || !config[ConfigStrings.enabled].ToLower().Equals("true"))
            {
                return base.an_hour_later();
            }
            ActivityMgr activityManager = this._factory.getActivityManager();
            int maxcoin = 0;
            if (config.ContainsKey(ConfigStrings.BG_coin))
            {
                int.TryParse(config[ConfigStrings.BG_coin], out maxcoin);
            }
            int goldAvailable = base.getGoldAvailable();
            int result = activityManager.getBGEventInfo(this._proto, this._logger, ref goldAvailable, maxcoin);
            if (result == 0)
            {
                return base.next_day();
            }
            else
            {
                return 3000;
            }
        }
    }
}
