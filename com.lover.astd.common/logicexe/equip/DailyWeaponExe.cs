using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class DailyWeaponExe : ExeBase
	{
		public DailyWeaponExe()
		{
			this._name = "daily_weapon";
			this._readable = "每日兵器";
		}

		public override long execute()
		{
			bool flag = this._user.Level < 120 || this._user.Level >= 182;
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				bool flag2 = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true") || !config.ContainsKey("daily") || !(config["daily"] != "");
				if (flag2)
				{
					result = base.next_day();
				}
				else
				{
					string text = config["daily"];
					bool flag3 = false;
					int num = this._factory.getEquipManager().handleDailyWeapon(this._proto, this._logger, out flag3, text);
					bool flag4 = flag3;
					if (flag4)
					{
						result = base.next_day();
					}
					else
					{
						bool flag5 = num == 3;
						if (flag5)
						{
							this.logInfo(string.Format("未找到所需要的兵器:{0}", text));
							result = base.an_hour_later();
						}
						else
						{
							result = base.immediate();
						}
					}
				}
			}
			return result;
		}
	}
}
