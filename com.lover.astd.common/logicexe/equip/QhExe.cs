using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class QhExe : ExeBase
	{
		public QhExe()
		{
			this._name = "qh";
			this._readable = "强化管理";
		}

		public override void init_data()
		{
			this._factory.getEquipManager().getEquipmentsCanQh(this._proto, this._logger, this._user, "");
			this.refreshUi();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			if (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"))
			{
				bool use_gold = config.ContainsKey("use_gold") && config["use_gold"].ToLower().Equals("true");
				int use_gold_silver = 1000000;
				int use_assist_silver = 200000;
				if (config.ContainsKey("use_gold_limit"))
				{
					int.TryParse(config["use_gold_limit"], out use_gold_silver);
				}
				if (config.ContainsKey("qh_ids"))
				{
					string qh_idstr = config["qh_ids"];
					bool qh_toukui = config.ContainsKey("toukui_enabled") && config["toukui_enabled"].ToLower().Equals("true");
					int toukui_ticket = 10000;
					if (config.ContainsKey("toukui_ticket"))
					{
						int.TryParse(config["toukui_ticket"], out toukui_ticket);
					}
					int num;
					do
					{
						int silverAvailable = base.getSilverAvailable();
						num = this._factory.getEquipManager().handleEquipmentsCanQh(this._proto, this._logger, this._user, silverAvailable, qh_idstr, use_assist_silver, use_gold, use_gold_silver, qh_toukui, toukui_ticket);
					}
					while (num == 0);
					this.refreshUi();
				}
			}
			return base.next_halfhour();
		}
	}
}
