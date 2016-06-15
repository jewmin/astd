using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl
{
	public class ShenHuoServer : LogicServer
	{
        public ShenHuoServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "shenhuo";
			this.ServerReadableName = "百炼精铁";
		}

		public override void renderSettings()
		{
            _mainForm.refreshPlayerInfo();
            if (_mainForm.getConfig() == null)
			{
				return;
			}
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.ContainsKey("shenhuoenable") && config["shenhuoenable"] != "")
			{
                _mainForm.chk_ShenHuo.Checked = config["shenhuoenable"].ToLower().Equals("true");
			}
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            if (_mainForm.chk_ShenHuo.Checked)
			{
				config.setConfig(this.ServerName, "shenhuoenable", "true");
				return;
			}
			config.setConfig(this.ServerName, "shenhuoenable", "false");
		}

		public override void loadDefaultSettings()
		{
			base.getConfig().setConfig(this.ServerName, "shenhuoenable", "false");
			this.renderSettings();
		}
	}
}
