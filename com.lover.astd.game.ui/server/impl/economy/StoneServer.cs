using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class StoneServer : LogicServer
	{
        public StoneServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "stone";
			this.ServerReadableName = "玉石采集";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_get_stone.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("ratio"))
			{
				int value = 0;
				int.TryParse(config["ratio"], out value);
                _mainForm.num_get_stone.Value = value;
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_get_stone.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "ratio", _mainForm.num_get_stone.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "ratio", "50");
			this.renderSettings();
		}
	}
}
