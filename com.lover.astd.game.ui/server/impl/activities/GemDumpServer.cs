using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class GemDumpServer : LogicServer
	{
		public GemDumpServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "gemdump";
			this.ServerReadableName = "宝石倾销";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_gemdump_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
            base.getConfig().setConfig(this.ServerName, "enabled", _mainForm.chk_gemdump_enable.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "enabled", "true");
			this.renderSettings();
		}
	}
}
