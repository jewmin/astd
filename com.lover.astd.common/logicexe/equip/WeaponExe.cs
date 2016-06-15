using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class WeaponExe : ExeBase
	{
		public WeaponExe()
		{
			this._name = "weapon";
			this._readable = "兵器升级";
		}

		public override void init_data()
		{
			bool flag = this._user.Level < 120;
			if (!flag)
			{
				this._factory.getEquipManager().handleWeaponsUpgrade(this._proto, this._logger, this._user, true);
				this.refreshUi();
			}
		}

		public override long execute()
		{
			bool flag = this._user.Level < 120;
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				bool flag2 = config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true");
				if (flag2)
				{
					int num;
					do
					{
						num = this._factory.getEquipManager().handleWeaponsUpgrade(this._proto, this._logger, this._user, false);
					}
					while (num == 0);
					this.refreshUi();
				}
				result = base.an_hour_later();
			}
			return result;
		}
	}
}
