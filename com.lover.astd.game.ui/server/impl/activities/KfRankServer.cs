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
            config.setConfig(this.ServerName, ConfigStrings.kfrank_point, _mainForm.nUD_kfrank_point.Value.ToString());
        }

        public override void renderSettings()
        {
            Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_kfrank.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
            int kfrank_point = 0;
            if (config.ContainsKey(ConfigStrings.kfrank_point))
            {
                int.TryParse(config[ConfigStrings.kfrank_point], out kfrank_point);
            }
            _mainForm.nUD_kfrank_point.Value = kfrank_point;
        }

        public override void loadDefaultSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, "false");
            config.setConfig(this.ServerName, ConfigStrings.kfrank_point, "800");
            this.renderSettings();
        }
    }
}
