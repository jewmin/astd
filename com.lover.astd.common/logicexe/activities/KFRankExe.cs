using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;

namespace com.lover.astd.common.logicexe.activities
{
    public class KFRankExe : ExeBase
    {
        public KFRankExe()
        {
            this._name = ConfigStrings.S_KFRank;
            this._readable = ConfigStrings.SR_KFRank;
        }

        public override long execute()
        {
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey(ConfigStrings.enabled) || !config[ConfigStrings.enabled].ToLower().Equals("true"))
            {
                return base.an_hour_later();
            }
            int kfrank_point = 0;
            if (config.ContainsKey(ConfigStrings.kfrank_point))
            {
                int.TryParse(config[ConfigStrings.kfrank_point], out kfrank_point);
            }
            ActivityMgr activityManager = this._factory.getActivityManager();
            return activityManager.getMatchDetail(this._proto, this._logger, this._user, kfrank_point);
        }
    }
}
