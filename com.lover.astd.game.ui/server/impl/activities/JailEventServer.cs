using System;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class JailEventServer : LogicServer
	{
		public JailEventServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "jail_event";
			this.ServerReadableName = "典狱活动";
		}

		public override void renderSettings()
		{
			base.getConfig(this.ServerName);
		}

		public override void saveSettings()
		{
			base.getConfig();
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "enabled", "true");
			this.renderSettings();
		}
	}
}
