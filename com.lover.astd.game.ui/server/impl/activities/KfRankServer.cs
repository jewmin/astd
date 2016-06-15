using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.lover.astd.common;
using com.lover.astd.common.config;

namespace com.lover.astd.game.ui.server.impl.activities
{
    internal class KfRankServer : LogicServer
    {
        public KfRankServer(NewMainForm frm)
        {
            this._mainForm = frm;
            this.ServerName = ConfigStrings.S_KFRank;
            this.ServerReadableName = ConfigStrings.SR_KFRank;
        }

        public override void saveSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_kfrank.Checked.ToString());
        }

        public override void renderSettings()
        {
            Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_kfrank.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
        }

        public override void loadDefaultSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, "false");
            this.renderSettings();
        }
    }
}
