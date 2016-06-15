using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class ResCampaignServer : LogicServer
	{
		public ResCampaignServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "res_campaign";
			this.ServerReadableName = "资源副本";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_res_campaign_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("res_campaign"))
			{
                _mainForm.lbl_attack_resCampaign.Text = config["res_campaign"];
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_res_campaign_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "res_campaign", _mainForm.lbl_attack_resCampaign.Text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "res_campaign", "");
			this.renderSettings();
		}
	}
}
