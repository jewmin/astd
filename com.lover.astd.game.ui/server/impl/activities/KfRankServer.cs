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
            if (_mainForm.combo_kfrank_ack_formation.SelectedItem != null)
			{
                config.setConfig(this.ServerName, ConfigStrings.kfrank_ack_formation, _mainForm.combo_kfrank_ack_formation.SelectedItem.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, ConfigStrings.kfrank_ack_formation, "不变阵");
			}
            if (_mainForm.combo_kfrank_def_formation.SelectedItem != null)
            {
                config.setConfig(this.ServerName, ConfigStrings.kfrank_def_formation, _mainForm.combo_kfrank_def_formation.SelectedItem.ToString());
            }
            else
            {
                config.setConfig(this.ServerName, ConfigStrings.kfrank_def_formation, "不变阵");
            }
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
            if (config.ContainsKey(ConfigStrings.kfrank_ack_formation))
            {
                _mainForm.combo_kfrank_ack_formation.SelectedItem = config[ConfigStrings.kfrank_ack_formation];
            }
            else
            {
                _mainForm.combo_kfrank_ack_formation.SelectedItem = "不变阵";
            }
            if (config.ContainsKey(ConfigStrings.kfrank_def_formation))
            {
                _mainForm.combo_kfrank_def_formation.SelectedItem = config[ConfigStrings.kfrank_def_formation];
            }
            else
            {
                _mainForm.combo_kfrank_def_formation.SelectedItem = "不变阵";
            }
            _mainForm.nUD_kfrank_point.Value = kfrank_point;
        }

        public override void loadDefaultSettings()
        {
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, ConfigStrings.enabled, "false");
            config.setConfig(this.ServerName, ConfigStrings.kfrank_point, "800");
            config.setConfig(this.ServerName, ConfigStrings.kfrank_ack_formation, "不变阵");
            config.setConfig(this.ServerName, ConfigStrings.kfrank_def_formation, "不变阵");
            this.renderSettings();
        }
    }
}
