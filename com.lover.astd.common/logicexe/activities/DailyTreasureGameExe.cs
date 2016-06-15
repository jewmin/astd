using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class DailyTreasureGameExe : ExeBase
	{
		public DailyTreasureGameExe()
		{
			this._name = "daily_treasure_game";
			this._readable = "日常探宝";
		}

		public override long execute()
		{
			int minGoldSteps = 0;
			string open_box_type = "";
			string gold_move_box_type = "";
			Dictionary<string, string> config = base.getConfig();
			bool flag = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				bool flag2 = config.ContainsKey("goldstep");
				if (flag2)
				{
					int.TryParse(config["goldstep"], out minGoldSteps);
				}
				bool flag3 = config.ContainsKey("open_box_type");
				if (flag3)
				{
					open_box_type = config["open_box_type"];
				}
				bool flag4 = config.ContainsKey("gold_move_box_type");
				if (flag4)
				{
					gold_move_box_type = config["gold_move_box_type"];
				}
				bool super_gold_end = config.ContainsKey("super_gold_end") && config["super_gold_end"].ToLower().Equals("true");
				string text = "";
				bool flag5 = config.ContainsKey("sel_daily_treasure_game") && config["sel_daily_treasure_game"] != "";
				if (flag5)
				{
					text = config["sel_daily_treasure_game"];
				}
				bool flag6 = text == "";
				if (flag6)
				{
					result = base.next_hour();
				}
				else
				{
					string[] array = text.Split(new char[]
					{
						':'
					});
					bool flag7 = array.Length < 2;
					if (flag7)
					{
						result = base.next_hour();
					}
					else
					{
						int game_id = 0;
						int.TryParse(array[0], out game_id);
						ActivityMgr activityManager = this._factory.getActivityManager();
						int goldAvailable = base.getGoldAvailable();
						int silver = this._user.Silver;
						int num = activityManager.handleDailyTreasureGameInfo(this._proto, this._logger, this._user, game_id, open_box_type, gold_move_box_type, minGoldSteps, super_gold_end, ref goldAvailable, ref silver);
						bool flag8 = num == 1;
						if (flag8)
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
						else
						{
							bool flag9 = num == 2;
							if (flag9)
							{
								result = base.next_hour();
							}
							else
							{
								bool flag10 = num == 3;
								if (flag10)
								{
									this.logInfo("想用银币开启日常探宝, 奈何银币太少了吧, 下个小时再看看吧");
									result = base.next_hour();
								}
								else
								{
									bool flag11 = num == 10;
									if (flag11)
									{
										result = base.next_hour();
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
			}
			return result;
		}
	}
}
