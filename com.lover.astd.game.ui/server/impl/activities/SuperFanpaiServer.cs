using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class SuperFanpaiServer : LogicServer
	{
        public SuperFanpaiServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "super_fanpai";
			this.ServerReadableName = "超级翻牌";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_super_fanpai.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            int max_buy = 0;
            if (config.ContainsKey("max_buy"))
            {
                int.TryParse(config["max_buy"], out max_buy);
            }
            _mainForm.num_super_fanpai_buy_gold.Value = max_buy;
		}

		public override void saveSettings()
		{
            base.getConfig().setConfig(this.ServerName, "enabled", _mainForm.chk_super_fanpai.Checked.ToString());
            base.getConfig().setConfig(this.ServerName, "max_buy", _mainForm.num_super_fanpai_buy_gold.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "enabled", "true");
            base.getConfig().setConfig(this.ServerName, "max_buy", "0");
			this.renderSettings();
		}
	}
}
