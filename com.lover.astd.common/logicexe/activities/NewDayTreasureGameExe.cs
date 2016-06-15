using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;

namespace com.lover.astd.common.logicexe.activities
{
    public class NewDayTreasureGameExe : ExeBase
    {
        public NewDayTreasureGameExe()
        {
            this._name = ConfigStrings.S_New_Day_Treasure_Game;
            this._readable = ConfigStrings.SR_New_Day_Treasure_Game;
        }

        public override long execute()
        {
            string open_box_type = "";
            bool shake_tree = false;
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey(ConfigStrings.enabled) || !config[ConfigStrings.enabled].ToLower().Equals("true"))
            {
                return base.next_day();
            }
            if (config.ContainsKey(ConfigStrings.open_box_type))
            {
                open_box_type = config[ConfigStrings.open_box_type];
            }
            shake_tree = config.ContainsKey(ConfigStrings.shake_tree) && config[ConfigStrings.shake_tree].ToLower().Equals("true");

            ActivityMgr activityManager = this._factory.getActivityManager();
            int goldAvailable = base.getGoldAvailable();
            int ret = activityManager.handleNewTreasureGameInfo(this._proto, this._logger, this._user, open_box_type, shake_tree, base.getGemPrice(), ref goldAvailable);
            if (ret == 1)
            {
                if (this._isRunningServer)
                {
                    return 60000L;
                }
                else
                {
                    return base.next_hour();
                }
            }
            else if (ret == 10)
            {
                return base.next_day();
            }
            else
            {
                return base.immediate();
            }
        }
    }
}
