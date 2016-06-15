using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class DinnerServer : LogicServer
	{
        public DinnerServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "dinner";
			this.ServerReadableName = "宴会";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.Count == 0 || !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
			{
                _mainForm.chk_dinner.Checked = false;
				return;
			}
            _mainForm.chk_dinner.Checked = true;
		}

		public override void saveSettings()
		{
			GameConfig arg_2B_0 = base.getConfig();
            arg_2B_0.setConfig(this.ServerName, "enabled", _mainForm.chk_dinner.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "enabled", "true");
			this.renderSettings();
		}
	}
}
