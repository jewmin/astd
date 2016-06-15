using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class ImposeServer : LogicServer
	{
        public ImposeServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "impose";
			this.ServerReadableName = "征收";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.Count == 0)
			{
				return;
			}
            _mainForm.chk_impose_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("impose_reserve"))
			{
				int value = 0;
				int.TryParse(config["impose_reserve"], out value);
                _mainForm.num_impose_reserve.Value = value;
			}
			if (config.ContainsKey("impose_loyalty"))
			{
				int value2 = 0;
				int.TryParse(config["impose_loyalty"], out value2);
                _mainForm.num_impose_loyalty.Value = value2;
			}
			if (config.ContainsKey("force_impose_gold"))
			{
				int value3 = 0;
				int.TryParse(config["force_impose_gold"], out value3);
                _mainForm.num_force_impose_gold.Value = value3;
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_impose_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "impose_reserve", _mainForm.num_impose_reserve.Value.ToString());
            expr_0D.setConfig(this.ServerName, "impose_loyalty", _mainForm.num_impose_loyalty.Value.ToString());
            expr_0D.setConfig(this.ServerName, "force_impose_gold", _mainForm.num_force_impose_gold.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "impose_reserve", "35");
			expr_06.setConfig(this.ServerName, "impose_loyalty", "85");
			expr_06.setConfig(this.ServerName, "force_impose_gold", "2");
			this.renderSettings();
		}
	}
}
