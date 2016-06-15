using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using com.lover.astd.common;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class BaoshiStoneServer : LogicServer
	{
        public BaoshiStoneServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = ConfigStrings.S_Baoshi_Stone;
			this.ServerReadableName = ConfigStrings.SR_Baoshi_Stone;
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_get_baoshi_stone.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
			if (config.ContainsKey(ConfigStrings.ratio))
			{
				int value = 0;
				int.TryParse(config[ConfigStrings.ratio], out value);
                _mainForm.num_get_baoshi_stone.Value = value;
			}
		}

		public override void saveSettings()
		{
			GameConfig conf = base.getConfig();
            conf.setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_get_baoshi_stone.Checked.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.ratio, _mainForm.num_get_baoshi_stone.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig conf = base.getConfig();
			conf.setConfig(this.ServerName, ConfigStrings.enabled, "true");
			conf.setConfig(this.ServerName, ConfigStrings.ratio, "50");
			this.renderSettings();
		}
	}
}
