using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.secretary
{
	public class SecretaryExe : ExeBase
	{
		private long daily_task_cd;

		public SecretaryExe()
		{
			this._name = "secretary";
			this._readable = "小秘书";
		}

		public override long execute()
		{
			long result;
			if (this._user.Level < 30)
			{
				result = base.next_hour();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				int box_reserve = 450;
				if (config.ContainsKey("reserve_treasure"))
				{
					int.TryParse(config["reserve_treasure"], out box_reserve);
				}
				MiscMgr miscManager = this._factory.getMiscManager();
				long num = 0L;
				int num2 = miscManager.handleSecretaryInfo(this._proto, this._logger, out num);
				miscManager.handleGiftInfo(this._proto, this._logger);
				bool flag3 = config.ContainsKey("open_treasure") && config["open_treasure"].ToLower().Equals("true");
				if (flag3)
				{
					miscManager.handleNationTreasureInfo(this._proto, this._logger, box_reserve);
				}
				miscManager.handleNationTaskInfo(this._proto, this._logger, this._user);
				miscManager.handleDailyEventInfo(this._proto, this._logger);
				miscManager.handleOfficerInfo(this._proto, this._logger);
				bool flag4 = config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true");
				if (flag4)
				{
					if (this._user.Level < 220)
					{
						num2 = miscManager.handleDailyTaskInfo(this._proto, this._logger);
						bool flag6 = num2 == 0;
						if (flag6)
						{
							this.daily_task_cd = 1320000L;
						}
						else
						{
							bool flag7 = num2 == 2;
							if (flag7)
							{
								this.daily_task_cd = base.next_hour();
							}
							else
							{
								this.daily_task_cd = 0L;
							}
						}
					}
				}
				if (config.ContainsKey("gift_box") && config["gift_box"].ToLower().Equals("true"))
				{
					miscManager.handleGoldboxEventInfo(this._proto, this._logger);
				}
				bool flag9 = num > 0L;
				if (flag9)
				{
					bool flag10 = num > this.daily_task_cd;
					if (flag10)
					{
						result = num;
					}
					else
					{
						result = this.daily_task_cd;
					}
				}
				else
				{
					bool flag11 = this.daily_task_cd > 0L;
					if (flag11)
					{
						result = this.daily_task_cd;
					}
					else
					{
						result = base.next_hour();
					}
				}
			}
			return result;
		}
	}
}
