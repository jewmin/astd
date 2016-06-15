using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class ArchServer : LogicServer
	{
		public ArchServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "arch";
			this.ServerReadableName = "考古";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_arch_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_arch_create.Checked = (config.ContainsKey("create") && config["create"].ToLower().Equals("true"));
			if (config.ContainsKey("upgrade"))
			{
                _mainForm.txt_arch_upgrade.Text = config["upgrade"];
				return;
			}
            _mainForm.txt_arch_upgrade.Text = "";
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_arch_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "create", _mainForm.chk_arch_create.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "upgrade", _mainForm.txt_arch_upgrade.Text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "samecolor", "true");
			expr_06.setConfig(this.ServerName, "create", "true");
			expr_06.setConfig(this.ServerName, "upgrade", "");
			this.renderSettings();
		}
	}
}
