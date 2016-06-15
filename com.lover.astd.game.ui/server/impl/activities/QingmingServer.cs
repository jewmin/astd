using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.lover.astd.common;
using com.lover.astd.common.config;

namespace com.lover.astd.game.ui.server.impl.activities
{
    internal class QingmingServer : LogicServer
    {
        public QingmingServer(NewMainForm frm)
        {
            this._mainForm = frm;
            this.ServerName = ConfigStrings.S_Qingming;
            this.ServerReadableName = ConfigStrings.SR_Qingming;
        }

        public override void saveSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_qingming.Checked.ToString());
            config.setConfig(this.ServerName, ConfigStrings.qm_round_coin, _mainForm.nUD_qm_round_coin.Value.ToString());
            config.setConfig(this.ServerName, ConfigStrings.qm_drink_coin, _mainForm.nUD_qm_drink_coin.Value.ToString());
        }

        public override void renderSettings()
        {
            Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_qingming.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
            int qm_round_coin = 0;
            if (config.ContainsKey(ConfigStrings.qm_round_coin))
            {
                int.TryParse(config[ConfigStrings.qm_round_coin], out qm_round_coin);
            }
            _mainForm.nUD_qm_round_coin.Value = qm_round_coin;
            int qm_drink_coin = 0;
            if (config.ContainsKey(ConfigStrings.qm_drink_coin))
            {
                int.TryParse(config[ConfigStrings.qm_drink_coin], out qm_drink_coin);
            }
            _mainForm.nUD_qm_drink_coin.Value = qm_drink_coin;
        }

        public override void loadDefaultSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, "false");
            config.setConfig(this.ServerName, ConfigStrings.qm_round_coin, "0");
            config.setConfig(this.ServerName, ConfigStrings.qm_drink_coin, "0");
            this.renderSettings();
        }
    }
}
