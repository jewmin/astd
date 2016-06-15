using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class DinnerExe : ExeBase
	{
		public DinnerExe()
		{
			this._name = "dinner";
			this._readable = "宴会";
		}

		public override long execute()
		{
			bool flag = this._user.Level < 80;
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				bool flag2 = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
				if (flag2)
				{
					result = base.next_day();
				}
				else
				{
					MiscMgr miscManager = this._factory.getMiscManager();
					this._user._dinner_joined = false;
					miscManager.getDinnerInfo(this._proto, this._logger, this._user);
					bool flag3 = this._user._dinner_count == 0;
					if (flag3)
					{
						result = base.next_dinner();
					}
					else
					{
						bool flag4 = this._user._dinner_in_time == 0;
						if (flag4)
						{
							result = base.next_dinner();
						}
						else
						{
							bool flag5 = this._user._dinner_team_id == "" || this._user._dinner_joined;
							if (flag5)
							{
								result = 30000L;
							}
							else
							{
								this.logInfo(string.Format("发现宴会, [{0}][{1}]", this._user._dinner_team_creator, this._user._dinner_team_id));
								miscManager.joinDinner(this._proto, this._logger, this._user);
								result = base.immediate();
							}
						}
					}
				}
			}
			return result;
		}
	}
}
