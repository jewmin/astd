using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.activities
{
	public class TroopTurntableExe : ExeBase
	{
		public TroopTurntableExe()
		{
			this._name = "troop_turntable";
			this._readable = "兵器转盘";
		}

		public override long execute()
		{
			ActivityMgr activityManager = this._factory.getActivityManager();
			bool flag = !this._user.isActivityRunning(ActivityType.TroopTurntable);
			long result;
			if (flag)
			{
				result = base.an_hour_later();
			}
			else
			{
				bool flag2 = this._user.Level < 120;
				if (flag2)
				{
					result = base.an_hour_later();
				}
				else
				{
					Dictionary<string, string> config = base.getConfig();
					bool flag3 = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
					if (flag3)
					{
						result = base.an_hour_later();
					}
					else
					{
						int make_gold = 0;
						int outside_num = 12;
						string text = "";
						bool flag4 = config.ContainsKey("type");
						if (flag4)
						{
							text = config["type"];
						}
						bool flag5 = text == "";
						if (flag5)
						{
							result = base.an_hour_later();
						}
						else
						{
							bool flag6 = config.ContainsKey("max_buy");
							if (flag6)
							{
								int.TryParse(config["max_buy"], out make_gold);
							}
							bool flag7 = config.ContainsKey("outside_num");
							if (flag7)
							{
								int.TryParse(config["outside_num"], out outside_num);
							}
							bool gold_ratio = config.ContainsKey("gold_ratio") && config["gold_ratio"].ToLower().Equals("true");
							int num = activityManager.handleTroopTurntableInfo(this._proto, this._logger, text, gold_ratio, base.getGoldAvailable(), make_gold, outside_num);
							bool flag8 = num == 10;
							if (flag8)
							{
								result = base.next_day();
							}
							else
							{
								bool flag9 = num == 4;
								if (flag9)
								{
									result = base.next_day();
								}
								else
								{
									bool flag10 = num == 0;
									if (flag10)
									{
										result = base.immediate();
									}
									else
									{
										result = base.next_hour();
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
