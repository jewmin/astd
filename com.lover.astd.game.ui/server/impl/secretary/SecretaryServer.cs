using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.secretary
{
	internal class SecretaryServer : LogicServer
	{
		public SecretaryServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "secretary";
			this.ServerReadableName = "小秘书";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_dailytask_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_secretary_open_treasure.Checked = (config.ContainsKey("open_treasure") && config["open_treasure"].ToLower().Equals("true"));
			int value = 450;
			if (config.ContainsKey("reserve_treasure"))
			{
				int.TryParse(config["reserve_treasure"], out value);
			}
            _mainForm.num_secretary_open_treasure.Value = value;
            _mainForm.chk_secretary_giftbox.Checked = (config.ContainsKey("gift_box") && config["gift_box"].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
			GameConfig arg_0D_0 = base.getConfig();
            arg_0D_0.setConfig(this.ServerName, "enabled", _mainForm.chk_dailytask_enable.Checked.ToString());
            arg_0D_0.setConfig(this.ServerName, "open_treasure", _mainForm.chk_secretary_open_treasure.Checked.ToString());
            arg_0D_0.setConfig(this.ServerName, "reserve_treasure", _mainForm.num_secretary_open_treasure.Value.ToString());
            arg_0D_0.setConfig(this.ServerName, "gift_box", _mainForm.chk_secretary_giftbox.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "open_treasure", "true");
			expr_06.setConfig(this.ServerName, "reserve_treasure", "450");
			expr_06.setConfig(this.ServerName, "gift_box", "true");
			this.renderSettings();
		}
	}
}
