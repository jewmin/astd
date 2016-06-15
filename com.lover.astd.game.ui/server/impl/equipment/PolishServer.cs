using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class PolishServer : LogicServer
	{
        public PolishServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "polish";
			this.ServerReadableName = "炼化玉佩";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_polish_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			int value = 5;
			int value2 = 5;
			int value3 = 10;
			int value4 = 2;
			if (config.ContainsKey("reserve_count"))
			{
				int.TryParse(config["reserve_count"], out value);
			}
			if (config.ContainsKey("reserve_item_count"))
			{
				int.TryParse(config["reserve_item_count"], out value2);
			}
			if (config.ContainsKey("melt_failcount"))
			{
				int.TryParse(config["melt_failcount"], out value4);
			}
			if (config.ContainsKey("gold_merge_attrib"))
			{
				int.TryParse(config["gold_merge_attrib"], out value3);
			}
            _mainForm.num_polish_reserve.Value = value;
            _mainForm.num_polish_item_reserve.Value = value2;
            _mainForm.num_polish_melt_failcount.Value = value4;
            _mainForm.num_polish_goon.Value = value3;
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_polish_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "reserve_count", _mainForm.num_polish_reserve.Value.ToString());
            expr_0D.setConfig(this.ServerName, "reserve_item_count", _mainForm.num_polish_item_reserve.Value.ToString());
            expr_0D.setConfig(this.ServerName, "melt_failcount", _mainForm.num_polish_melt_failcount.Value.ToString());
            expr_0D.setConfig(this.ServerName, "gold_merge_attrib", _mainForm.num_polish_goon.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "reserve_count", "5");
			expr_06.setConfig(this.ServerName, "reserve_item_count", "5");
			expr_06.setConfig(this.ServerName, "melt_failcount", "2");
			expr_06.setConfig(this.ServerName, "gold_merge_attrib", "10");
			this.renderSettings();
		}
	}
}
