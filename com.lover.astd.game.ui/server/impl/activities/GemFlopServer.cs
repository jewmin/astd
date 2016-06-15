using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class GemFlopServer : LogicServer
	{
        public GemFlopServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "gem_flop";
			this.ServerReadableName = "宝石翻牌";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_gem_flop_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			int value = 2;
			if (config.ContainsKey("max_upgrade"))
			{
				int.TryParse(config["max_upgrade"], out value);
			}
            _mainForm.num_gem_flop_upgrade_count.Value = value;
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_gem_flop_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "max_upgrade", _mainForm.num_gem_flop_upgrade_count.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "max_upgrade", "2");
			this.renderSettings();
		}
	}
}
