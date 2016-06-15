using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class CakeServer : LogicServer
	{
		public CakeServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "cake";
			this.ServerReadableName = "月饼活动";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_cake_event.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
            base.getConfig().setConfig(this.ServerName, "enabled", _mainForm.chk_cake_event.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "enabled", "true");
			this.renderSettings();
		}
	}
}
