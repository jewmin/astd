using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.battle
{
	internal class KfBanquetServer : LogicServer
	{
        public KfBanquetServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "kf_banquet";
			this.ServerReadableName = "盛宴活动";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_kf_banquet_enabled.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_kf_banquet_nation.Checked = (config.ContainsKey("nation") && config["nation"].ToLower().Equals("true"));
            _mainForm.chk_again.Checked = (config.ContainsKey("again") && config["again"].ToLower().Equals("true"));
			if (config.ContainsKey("buygold"))
			{
				int value = 0;
				int.TryParse(config["buygold"], out value);
                _mainForm.num_kf_banquet_buygold.Value = value;
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_kf_banquet_enabled.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "nation", _mainForm.chk_kf_banquet_nation.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "again", _mainForm.chk_again.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "buygold", _mainForm.num_kf_banquet_buygold.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "nation", "true");
			expr_06.setConfig(this.ServerName, "again", "false");
			expr_06.setConfig(this.ServerName, "buygold", "0");
			this.renderSettings();
		}
	}
}
