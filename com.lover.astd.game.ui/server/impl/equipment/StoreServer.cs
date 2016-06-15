using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using com.lover.astd.common;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class StoreServer : LogicServer
	{
        public StoreServer(NewMainForm frm)
		{
			this._mainForm = frm;
            this.ServerName = ConfigStrings.S_Store;
            this.ServerReadableName = ConfigStrings.SR_Store;
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_upgrade_gem.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
            if (config.ContainsKey(ConfigStrings.merge_gem_level))
			{
				int value = 0;
                int.TryParse(config[ConfigStrings.merge_gem_level], out value);
                _mainForm.num_upgrade_gem.Value = value;
			}
            _mainForm.chk_openbox_for_yupei.Checked = (config.ContainsKey(ConfigStrings.open_box_for_yupei) && config[ConfigStrings.open_box_for_yupei].ToLower().Equals("true"));
            _mainForm.chk_upgrade_crystal.Checked = (config.ContainsKey(ConfigStrings.upgrade_crystal) && config[ConfigStrings.upgrade_crystal].ToLower().Equals("true"));
            if (config.ContainsKey(ConfigStrings.upgrade_crystal_level))
            {
                int value = 0;
                int.TryParse(config[ConfigStrings.upgrade_crystal_level], out value);
                _mainForm.num_upgrade_crystal.Value = value;
            }
            _mainForm.chk_auto_melt_yupei.Checked = (config.ContainsKey(ConfigStrings.auto_melt_yupei) && config[ConfigStrings.auto_melt_yupei].ToLower().Equals("true"));
            if (config.ContainsKey(ConfigStrings.yupei_attr))
            {
                int value = 0;
                int.TryParse(config[ConfigStrings.yupei_attr], out value);
                _mainForm.num_yupei_attr.Value = value;
            }
		}

		public override void saveSettings()
		{
			GameConfig conf = base.getConfig();
            conf.setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_upgrade_gem.Checked.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.merge_gem_level, _mainForm.num_upgrade_gem.Value.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.open_box_for_yupei, _mainForm.chk_openbox_for_yupei.Checked.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.upgrade_crystal, _mainForm.chk_upgrade_crystal.Checked.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.upgrade_crystal_level, _mainForm.num_upgrade_crystal.Value.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.auto_melt_yupei, _mainForm.chk_auto_melt_yupei.Checked.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.yupei_attr, _mainForm.num_yupei_attr.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig conf = base.getConfig();
            conf.setConfig(this.ServerName, ConfigStrings.enabled, "true");
            conf.setConfig(this.ServerName, ConfigStrings.merge_gem_level, "12");
            conf.setConfig(this.ServerName, ConfigStrings.open_box_for_yupei, "false");
            conf.setConfig(this.ServerName, ConfigStrings.upgrade_crystal, "true");
            conf.setConfig(this.ServerName, ConfigStrings.upgrade_crystal_level, "25");
            conf.setConfig(this.ServerName, ConfigStrings.auto_melt_yupei, "true");
            conf.setConfig(this.ServerName, ConfigStrings.yupei_attr, "3");
			this.renderSettings();
		}
	}
}
