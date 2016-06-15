using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class MhExe : ExeBase
	{
		public MhExe()
		{
			this._name = "mh";
			this._readable = "魔化管理";
		}

        public override void init_data()
        {
            if (this._user.Level < 101)
            {
                return;
            }
            this._factory.getEquipManager().getEquipmentsCanMh(this._proto, this._logger, this._user, "");
            this.refreshUi();
        }

		public override long execute()
		{
			if (this._user.Level < 101)
			{
				return base.next_day();
			}
            Dictionary<string, string> config = base.getConfig();
            if (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"))
            {
                int fail_count_max = 3;
                if (config.ContainsKey("fail_count"))
                {
                    int.TryParse(config["fail_count"], out fail_count_max);
                }
                int stoneAvailable = base.getStoneAvailable();
                if (config.ContainsKey("mh_ids"))
                {
                    string mh_type = "少量";
                    if (config.ContainsKey("mh_type"))
                    {
                        mh_type = config["mh_type"];
                    }
                    string mh_idstr = config["mh_ids"];
                    this._factory.getEquipManager().handleEquipmentsCanMh(this._proto, this._logger, this._user, mh_idstr, fail_count_max, mh_type, stoneAvailable);
                    this.refreshUi();
                }
            }
            return base.next_halfhour();
		}
	}
}
