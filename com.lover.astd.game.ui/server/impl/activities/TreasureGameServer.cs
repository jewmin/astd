using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class TreasureGameServer : LogicServer
	{
		public TreasureGameServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "treasure_game";
			this.ServerReadableName = "古城寻宝";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_treasure_game_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.txt_treasure_game_boxtype.Text = (config.ContainsKey("open_box_type") ? config["open_box_type"] : "");
            _mainForm.txt_treasure_game_goldmove_boxtype.Text = (config.ContainsKey("gold_move_box_type") ? config["gold_move_box_type"] : "");
            _mainForm.txt_treasure_game_dice_type.Text = (config.ContainsKey("dice_type") ? config["dice_type"] : "");
			int value = 0;
			if (config.ContainsKey("goldstep"))
			{
				int.TryParse(config["goldstep"], out value);
			}
            _mainForm.num_treasure_game_goldstep.Value = value;
            _mainForm.chk_treasure_game_use_ticket.Checked = (config.ContainsKey("use_ticket") && config["use_ticket"].ToLower().Equals("true"));
			int num = 2;
			if (config.ContainsKey("use_ticket_type"))
			{
				int.TryParse(config["use_ticket_type"], out num);
			}
			if (num > 3 || num < 1)
			{
				num = 2;
			}
            _mainForm.combo_treasure_game_use_ticket_type.SelectedIndex = num - 1;
            _mainForm.chk_treasure_game_ticket_gold_end.Checked = (config.ContainsKey("ticket_gold_end") && config["ticket_gold_end"].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_treasure_game_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "open_box_type", _mainForm.txt_treasure_game_boxtype.Text);
            expr_0D.setConfig(this.ServerName, "gold_move_box_type", _mainForm.txt_treasure_game_goldmove_boxtype.Text);
            expr_0D.setConfig(this.ServerName, "dice_type", _mainForm.txt_treasure_game_dice_type.Text);
            expr_0D.setConfig(this.ServerName, "goldstep", _mainForm.num_treasure_game_goldstep.Value.ToString());
            expr_0D.setConfig(this.ServerName, "use_ticket", _mainForm.chk_treasure_game_use_ticket.Checked.ToString());
            int num = _mainForm.combo_treasure_game_use_ticket_type.SelectedIndex;
			if (num < 0)
			{
				num = 0;
			}
			num++;
			expr_0D.setConfig(this.ServerName, "use_ticket_type", num.ToString());
            expr_0D.setConfig(this.ServerName, "ticket_gold_end", _mainForm.chk_treasure_game_ticket_gold_end.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "open_box_type", "01");
			expr_06.setConfig(this.ServerName, "gold_move_box_type", "");
            expr_06.setConfig(this.ServerName, "dice_type", "");
			expr_06.setConfig(this.ServerName, "goldstep", "0");
			expr_06.setConfig(this.ServerName, "use_ticket", "true");
			expr_06.setConfig(this.ServerName, "use_ticket_type", "2");
			expr_06.setConfig(this.ServerName, "ticket_gold_end", "false");
			this.renderSettings();
		}
	}
}
