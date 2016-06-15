using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class DailyWeaponServer : LogicServer
	{
		public DailyWeaponServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "daily_weapon";
			this.ServerReadableName = "每日兵器";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_daily_weapon.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("daily") && config["daily"] != "")
			{
                _mainForm.combo_daily_weapon_type.SelectedItem = config["daily"];
			}
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_daily_weapon.Checked.ToString());
            if (_mainForm.combo_daily_weapon_type.SelectedItem != null)
			{
                config.setConfig(this.ServerName, "daily", _mainForm.combo_daily_weapon_type.SelectedItem.ToString());
			}
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "daily", "无敌将军炮");
			this.renderSettings();
		}
	}
}
