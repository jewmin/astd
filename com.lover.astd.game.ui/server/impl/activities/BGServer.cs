using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.lover.astd.common;
using com.lover.astd.common.config;

namespace com.lover.astd.game.ui.server.impl.activities
{
    internal class BGServer : LogicServer
    {
        public BGServer(NewMainForm frm)
        {
            this._mainForm = frm;
            this.ServerName = ConfigStrings.S_BG;
            this.ServerReadableName = ConfigStrings.SR_BG;
        }

        public override void saveSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_BGActivity.Checked.ToString());
            config.setConfig(this.ServerName, ConfigStrings.BG_coin, _mainForm.nUD_BGActivity_coin.Value.ToString());
        }

        public override void renderSettings()
        {
            Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_BGActivity.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
            int value = 0;
            if (config.ContainsKey(ConfigStrings.BG_coin))
            {
                int.TryParse(config[ConfigStrings.BG_coin], out value);
            }
            _mainForm.nUD_BGActivity_coin.Value = value;
        }

        public override void loadDefaultSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, "false");
            config.setConfig(this.ServerName, ConfigStrings.BG_coin, "0");
            this.renderSettings();
        }
    }
}
