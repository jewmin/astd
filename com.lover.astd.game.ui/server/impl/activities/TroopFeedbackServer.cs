using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class TroopFeedbackServer : LogicServer
	{
		public TroopFeedbackServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "troop_feedback";
			this.ServerReadableName = "兵器回馈";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_troop_feedback_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_troop_feedback_refine_notired.Checked = (config.ContainsKey("refine_notired") && config["refine_notired"].ToLower().Equals("true"));
            _mainForm.chk_troop_feedback_doubleweapon.Checked = (config.ContainsKey("double_weapon") && config["double_weapon"].ToLower().Equals("true"));
            _mainForm.chk_troop_feedback_opentreasure.Checked = (config.ContainsKey("open_treasure") && config["open_treasure"].ToLower().Equals("true"));
            _mainForm.chk_troop_feedback_openbox.Checked = (config.ContainsKey("auto_open_box") && config["auto_open_box"].ToLower().Equals("true"));
			if (config.ContainsKey("boxtype"))
			{
				int num = 0;
				int.TryParse(config["boxtype"], out num);
                if (num < 0 || num >= _mainForm.combo_troop_feedback_opentype.Items.Count)
				{
					num = 0;
				}
                _mainForm.combo_troop_feedback_opentype.SelectedIndex = num;
				return;
			}
            _mainForm.combo_troop_feedback_opentype.SelectedIndex = 0;
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_troop_feedback_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "refine_notired", _mainForm.chk_troop_feedback_refine_notired.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "double_weapon", _mainForm.chk_troop_feedback_doubleweapon.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "open_treasure", _mainForm.chk_troop_feedback_opentreasure.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "auto_open_box", _mainForm.chk_troop_feedback_openbox.Checked.ToString());
            int num = _mainForm.combo_troop_feedback_opentype.SelectedIndex;
			if (num < 0)
			{
				num = 0;
			}
			expr_0D.setConfig(this.ServerName, "boxtype", num.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "refine_notired", "true");
			expr_06.setConfig(this.ServerName, "double_weapon", "true");
			expr_06.setConfig(this.ServerName, "open_treasure", "true");
			expr_06.setConfig(this.ServerName, "auto_open_box", "true");
			expr_06.setConfig(this.ServerName, "boxtype", "0");
			this.renderSettings();
		}
	}
}
