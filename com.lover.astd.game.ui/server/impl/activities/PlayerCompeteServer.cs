using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class PlayerCompeteServer : LogicServer
	{
        public PlayerCompeteServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "player_compete";
			this.ServerReadableName = "跨服武斗";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_kfwd_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_kfwd_buyreward.Checked = (config.ContainsKey("buy_reward") && config["buy_reward"].ToLower().Equals("true"));
            _mainForm.chk_kfwd_openbox.Checked = (config.ContainsKey("auto_open_box") && config["auto_open_box"].ToLower().Equals("true"));
			int selectedIndex = 0;
			if (config.ContainsKey("open_box_type"))
			{
				int.TryParse(config["open_box_type"], out selectedIndex);
			}
            _mainForm.combo_kfwd_openbox_type.SelectedIndex = selectedIndex;
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_kfwd_enable.Checked.ToString());
            config.setConfig(this.ServerName, "buy_reward", _mainForm.chk_kfwd_buyreward.Checked.ToString());
            config.setConfig(this.ServerName, "auto_open_box", _mainForm.chk_kfwd_openbox.Checked.ToString());
            if (_mainForm.combo_kfwd_openbox_type.SelectedIndex >= 0)
			{
                config.setConfig(this.ServerName, "open_box_type", _mainForm.combo_kfwd_openbox_type.SelectedIndex.ToString());
				return;
			}
			config.setConfig(this.ServerName, "open_box_type", "0");
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "buy_reward", "true");
			expr_06.setConfig(this.ServerName, "auto_open_box", "true");
			expr_06.setConfig(this.ServerName, "open_box_type", "0");
			this.renderSettings();
		}
	}
}
