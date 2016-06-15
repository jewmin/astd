using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class GiftEventServer : LogicServer
	{
		public GiftEventServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "giftevent";
			this.ServerReadableName = "犒赏活动";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_giftevent_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("serial"))
			{
                _mainForm.txt_giftevent_serial.Text = config["serial"];
				return;
			}
            _mainForm.txt_giftevent_serial.Text = "231";
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_giftevent_enable.Checked.ToString());
            string conf = _mainForm.txt_giftevent_serial.Text.Trim();
			expr_0D.setConfig(this.ServerName, "serial", conf);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "serial", "231");
			this.renderSettings();
		}
	}
}
