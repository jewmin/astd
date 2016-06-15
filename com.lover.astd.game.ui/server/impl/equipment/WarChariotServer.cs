using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.lover.astd.common;
using com.lover.astd.common.config;

namespace com.lover.astd.game.ui.server.impl.equipment
{
    public class WarChariotServer : LogicServer
    {
        public WarChariotServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = ConfigStrings.S_War_Chariot;
			this.ServerReadableName = ConfigStrings.SR_War_Chariot;
		}

        public override void renderSettings()
        {
            Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_upgrade_war_chariot.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
        }

        public override void saveSettings()
        {
            base.getConfig().setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_upgrade_war_chariot.Checked.ToString());
        }

        public override void loadDefaultSettings()
        {
            GameConfig conf = base.getConfig();
            conf.setConfig(this.ServerName, ConfigStrings.enabled, "true");
            this.renderSettings();
        }
    }
}
