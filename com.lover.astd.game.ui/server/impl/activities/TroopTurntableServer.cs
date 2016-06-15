using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class TroopTurntableServer : LogicServer
	{
		public TroopTurntableServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "troop_turntable";
			this.ServerReadableName = "兵器转盘";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_troop_turntable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("type") && config["type"] != "")
			{
                _mainForm.combo_troop_turntable_type.SelectedItem = config["type"];
			}
            _mainForm.chk_troop_turntable_goldratio.Checked = (config.ContainsKey("gold_ratio") && config["gold_ratio"].ToLower().Equals("true"));
			int value = 0;
			if (config.ContainsKey("max_buy"))
			{
				int.TryParse(config["max_buy"], out value);
			}
            _mainForm.num_troop_turntable_buygold.Value = value;
			value = 12;
			if (config.ContainsKey("outside_num"))
			{
				int.TryParse(config["outside_num"], out value);
			}
            _mainForm.nm_Out.Value = value;
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_troop_turntable.Checked.ToString());
            config.setConfig(this.ServerName, "gold_ratio", _mainForm.chk_troop_turntable_goldratio.Checked.ToString());
            if (_mainForm.combo_troop_turntable_type.SelectedItem != null)
			{
                config.setConfig(this.ServerName, "type", _mainForm.combo_troop_turntable_type.SelectedItem.ToString());
			}
            config.setConfig(this.ServerName, "max_buy", _mainForm.num_troop_turntable_buygold.Value.ToString());
            config.setConfig(this.ServerName, "outside_num", _mainForm.nm_Out.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "gold_ratio", "true");
			expr_06.setConfig(this.ServerName, "type", "无敌将军炮");
			expr_06.setConfig(this.ServerName, "max_buy", "0");
			expr_06.setConfig(this.ServerName, "outside_num", "12");
			this.renderSettings();
		}
	}
}
