using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;

namespace com.lover.astd.common.logicexe.activities
{
    public class QingmingExe : ExeBase
    {
        public QingmingExe()
		{
			this._name = ConfigStrings.S_Qingming;
			this._readable = ConfigStrings.SR_Qingming;
		}

        public override long execute()
        {
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey(ConfigStrings.enabled) || !config[ConfigStrings.enabled].ToLower().Equals("true"))
            {
                return base.an_hour_later();
            }
            ActivityMgr activityManager = this._factory.getActivityManager();
            int qm_round_coin = 0;
            if (config.ContainsKey(ConfigStrings.qm_round_coin))
            {
                int.TryParse(config[ConfigStrings.qm_round_coin], out qm_round_coin);
            }
            int qm_drink_coin = 0;
            if (config.ContainsKey(ConfigStrings.qm_drink_coin))
            {
                int.TryParse(config[ConfigStrings.qm_drink_coin], out qm_drink_coin);
            }
            return activityManager.getQingmingInfo(_proto, _logger, qm_round_coin, qm_drink_coin, getGoldAvailable());
        }
    }
}
