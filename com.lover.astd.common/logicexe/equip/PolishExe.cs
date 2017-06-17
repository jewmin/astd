using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class PolishExe : ExeBase
	{
		public PolishExe()
		{
			this._name = "polish";
			this._readable = "炼化玉佩";
		}

		public override long execute()
		{
			if (this._user.Level < 101)
			{
				return base.next_day();
			}
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
            {
                return base.next_halfhour();
            }
            else
            {
                int reserve_count = 5;
                int reserve_item_count = 5;
                int gold_merge_attrib = 10;
                int melt_failcount = 0;
                if (config.ContainsKey("reserve_count"))
                {
                    int.TryParse(config["reserve_count"], out reserve_count);
                }
                if (config.ContainsKey("reserve_item_count"))
                {
                    int.TryParse(config["reserve_item_count"], out reserve_item_count);
                }
                if (config.ContainsKey("melt_failcount"))
                {
                    int.TryParse(config["melt_failcount"], out melt_failcount);
                }
                if (config.ContainsKey("gold_merge_attrib"))
                {
                    int.TryParse(config["gold_merge_attrib"], out gold_merge_attrib);
                }
                if (config.ContainsKey("lh_ids"))
                {
                    string lh_ids = config["lh_ids"];
                    int ret = this._factory.getEquipManager().handlePolishInfo(this._proto, this._logger, this._user, base.getGoldAvailable(), lh_ids, this._user.Magic, reserve_count, reserve_item_count, gold_merge_attrib, melt_failcount);
                    this.refreshUi();
                    if (ret == 3)
                    {
                        return base.next_hour();
                    }
                    else if (ret == 4)
                    {
                        return base.immediate();
                    }
                    else if (ret == 0)
                    {
                        return 600000;
                    }
                    else
                    {
                        return base.next_hour();
                    }
                }
                return base.next_hour();
            }
		}

        public override void init_data()
        {
            this._factory.getEquipManager().getBaowuPolishInfo(this._proto, this._logger, this._user, "");
            this.refreshUi();
        }
	}
}
