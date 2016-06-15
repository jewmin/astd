using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.lover.astd.common;
using com.lover.astd.common.config;

namespace com.lover.astd.game.ui.server.impl.activities
{
    internal class NewDayTreasureGameServer : LogicServer
    {
        public NewDayTreasureGameServer(NewMainForm frm)
        {
            this._mainForm = frm;
            this.ServerName = ConfigStrings.S_New_Day_Treasure_Game;
            this.ServerReadableName = ConfigStrings.SR_New_Day_Treasure_Game;
        }

        public override void renderSettings()
        {
            Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_new_day_treasure_game.Checked = (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"));
            _mainForm.txt_new_day_treasure_game_boxtype.Text = (config.ContainsKey(ConfigStrings.open_box_type) ? config[ConfigStrings.open_box_type] : "");
            _mainForm.chk_new_day_treasure_game_shake_tree.Checked = (config.ContainsKey(ConfigStrings.shake_tree) && config[ConfigStrings.shake_tree].ToLower().Equals("true"));
        }

        public override void saveSettings()
        {
            GameConfig conf = base.getConfig();
            conf.setConfig(this.ServerName, ConfigStrings.enabled, _mainForm.chk_new_day_treasure_game.Checked.ToString());
            conf.setConfig(this.ServerName, ConfigStrings.open_box_type, _mainForm.txt_new_day_treasure_game_boxtype.Text);
            conf.setConfig(this.ServerName, ConfigStrings.shake_tree, _mainForm.chk_new_day_treasure_game_shake_tree.Checked.ToString());
        }

        public override void loadDefaultSettings()
        {
            GameConfig conf = base.getConfig();
            conf.setConfig(this.ServerName, ConfigStrings.enabled, "true");
            conf.setConfig(this.ServerName, ConfigStrings.open_box_type, "");
            conf.setConfig(this.ServerName, ConfigStrings.shake_tree, "false");
            this.renderSettings();
        }
    }
}
