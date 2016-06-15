using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class TreasureGameExe : ExeBase
	{
		public TreasureGameExe()
		{
			this._name = "treasure_game";
			this._readable = "古城寻宝";
		}

        public override long execute()
        {
            int minGoldSteps = 0;
            string open_box_type = "";
            string gold_move_box_type = "";
            string dice_type = "";
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
            {
                return base.next_day();
            }
            else
            {
                if (config.ContainsKey("goldstep"))
                {
                    int.TryParse(config["goldstep"], out minGoldSteps);
                }
                if (config.ContainsKey("open_box_type"))
                {
                    open_box_type = config["open_box_type"];
                }
                if (config.ContainsKey("gold_move_box_type"))
                {
                    gold_move_box_type = config["gold_move_box_type"];
                }
                if (config.ContainsKey("dice_type"))
                {
                    dice_type = config["dice_type"];
                }
                bool use_ticket = config.ContainsKey("use_ticket") && config["use_ticket"].ToLower().Equals("true");
                int num = 2;
                if (config.ContainsKey("use_ticket_type"))
                {
                    int.TryParse(config["use_ticket_type"], out num);
                }
                if (num > 3 || num < 1)
                {
                    num = 2;
                }
                bool gold_to_end = config.ContainsKey("ticket_gold_end") && config["ticket_gold_end"].ToLower().Equals("true");
                ActivityMgr activityManager = this._factory.getActivityManager();
                int goldAvailable = base.getGoldAvailable();
                int num2 = activityManager.handleTreasureGameInfo(this._proto, this._logger, this._user, open_box_type, gold_move_box_type, dice_type, minGoldSteps, ref goldAvailable);
                if (num2 == 10)
                {
                    return base.next_day();
                }
                else if (num2 == 1)
                {
                    if (this._isRunningServer)
                    {
                        return 60000L;
                    }
                    else
                    {
                        return base.next_halfhour();
                    }
                }
                else if (num2 == 2)
                {
                    if (!use_ticket)
                    {
                        return base.next_hour();
                    }
                    else
                    {
                        int num3 = activityManager.handleNewTreasureGames(this._proto, this._logger, base.getGoldAvailable(), num, gold_to_end);
                        if (num3 == 1)
                        {
                            if (this._isRunningServer)
                            {
                                return 60000L;
                            }
                            else
                            {
                                return base.next_halfhour();
                            }
                        }
                        else if (num3 == 0)
                        {
                            return base.immediate();
                        }
                        else
                        {
                            return base.next_hour();
                        }
                    }
                }
                else if (num2 != 3)
                {
                    return base.immediate();
                }
                else
                {
                    if (!use_ticket)
                    {
                        return base.next_day();
                    }
                    else
                    {
                        int num4 = activityManager.handleNewTreasureGames(this._proto, this._logger, base.getGoldAvailable(), num, gold_to_end);
                        if (num4 == 1)
                        {
                            if (this._isRunningServer)
                            {
                                return 60000L;
                            }
                            else
                            {
                                return base.next_halfhour();
                            }
                        }
                        else if (num4 == 0)
                        {
                            return base.immediate();
                        }
                        else
                        {
                            return base.next_day();
                        }
                    }
                }
            }
        }
	}
}
