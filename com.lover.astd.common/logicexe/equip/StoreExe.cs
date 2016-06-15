using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class StoreExe : ExeBase
	{
		public StoreExe()
		{
			this._name = ConfigStrings.S_Store;
			this._readable = ConfigStrings.SR_Store;
		}

		public override long execute()
		{
			int merge_gem_level = 100;
            int upgrade_crystal_level = 30;
            int yupei_attr = 3;
			Dictionary<string, string> config = base.getConfig();
            bool enabled = config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true");
            bool open_box_for_yupei = config.ContainsKey(ConfigStrings.open_box_for_yupei) && config[ConfigStrings.open_box_for_yupei].ToLower().Equals("true");
            bool upgrade_crystal = config.ContainsKey(ConfigStrings.upgrade_crystal) && config[ConfigStrings.upgrade_crystal].ToLower().Equals("true");
            bool auto_melt_yupei = config.ContainsKey(ConfigStrings.auto_melt_yupei) && config[ConfigStrings.auto_melt_yupei].ToLower().Equals("true");
            if (!enabled && !open_box_for_yupei && !upgrade_crystal && !auto_melt_yupei)
			{
				return base.an_hour_later();
			}
			else
			{
				if (config.ContainsKey(ConfigStrings.merge_gem_level))
				{
					int.TryParse(config[ConfigStrings.merge_gem_level], out merge_gem_level);
				}
                if (config.ContainsKey(ConfigStrings.upgrade_crystal_level))
                {
                    int.TryParse(config[ConfigStrings.upgrade_crystal_level], out upgrade_crystal_level);
                }
                if (config.ContainsKey(ConfigStrings.yupei_attr))
                {
                    int.TryParse(config[ConfigStrings.yupei_attr], out yupei_attr);
                }
				if (this._factory.getEquipManager().handleStore(this._proto, this._logger, this._user, enabled, merge_gem_level, open_box_for_yupei, upgrade_crystal, upgrade_crystal_level, auto_melt_yupei, yupei_attr) == 0)
				{
					return base.immediate();
				}
				else
				{
					return base.an_hour_later();
				}
			}
		}
	}
}
