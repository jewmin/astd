using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class FestivalEventServer : LogicServer
	{
		public FestivalEventServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "festival";
			this.ServerReadableName = "迎军活动";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_festival_event.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
            base.getConfig().setConfig(this.ServerName, "enabled", _mainForm.chk_festival_event.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "enabled", "true");
			this.renderSettings();
		}
	}
}
