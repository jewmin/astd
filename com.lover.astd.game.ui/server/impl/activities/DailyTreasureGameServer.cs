using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class DailyTreasureGameServer : LogicServer
	{
        public DailyTreasureGameServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "daily_treasure_game";
			this.ServerReadableName = "日常探宝";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_daily_treasure_game.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.txt_daily_treasure_game_boxtype.Text = (config.ContainsKey("open_box_type") ? config["open_box_type"] : "");
            _mainForm.txt_daily_treasure_game_goldmove_boxtype.Text = (config.ContainsKey("gold_move_box_type") ? config["gold_move_box_type"] : "");
			int value = 0;
			if (config.ContainsKey("goldstep"))
			{
				int.TryParse(config["goldstep"], out value);
			}
            _mainForm.num_daily_treasure_game_goldstep.Value = value;
            _mainForm.chk_daily_treasure_game_super_gold_end.Checked = (config.ContainsKey("super_gold_end") && config["super_gold_end"].ToLower().Equals("true"));
			if (config.ContainsKey("sel_daily_treasure_game"))
			{
                _mainForm.lbl_day_treasure_game_sel.Text = config["sel_daily_treasure_game"];
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_daily_treasure_game.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "open_box_type", _mainForm.txt_daily_treasure_game_boxtype.Text);
            expr_0D.setConfig(this.ServerName, "gold_move_box_type", _mainForm.txt_daily_treasure_game_goldmove_boxtype.Text);
            expr_0D.setConfig(this.ServerName, "goldstep", _mainForm.num_daily_treasure_game_goldstep.Value.ToString());
            expr_0D.setConfig(this.ServerName, "super_gold_end", _mainForm.chk_daily_treasure_game_super_gold_end.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "sel_daily_treasure_game", _mainForm.lbl_day_treasure_game_sel.Text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "open_box_type", "01");
			expr_06.setConfig(this.ServerName, "gold_move_box_type", "");
			expr_06.setConfig(this.ServerName, "goldstep", "0");
			expr_06.setConfig(this.ServerName, "super_gold_end", "false");
			expr_06.setConfig(this.ServerName, "sel_daily_treasure_game", "");
			this.renderSettings();
		}
	}
}
