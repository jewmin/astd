using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using com.lover.astd.common;

namespace com.lover.astd.game.ui.server.impl
{
	public class CommonServer : LogicServer
	{
		public CommonServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "global";
			this.ServerReadableName = "通用";
		}

		public override void saveSettings()
		{
			NewMainForm mainForm = this._mainForm;
			if (mainForm.getConfig() == null)
			{
				return;
			}
			GameConfig config = base.getConfig();
			if (mainForm.chk_global_logonAward.Checked)
			{
				config.setConfig(this.ServerName, "auto_logon_award", "true");
			}
			else
			{
				config.setConfig(this.ServerName, "auto_logon_award", "false");
			}
			if (mainForm.chk_Cai.Checked)
			{
				config.setConfig(this.ServerName, "auto_cai", "true");
			}
			else
			{
				config.setConfig(this.ServerName, "auto_cai", "false");
			}
			if (mainForm.chk_AutoBuyCredit.Checked)
			{
				config.setConfig(this.ServerName, "auto_buy_credit", "true");
			}
			else
			{
				config.setConfig(this.ServerName, "auto_buy_credit", "false");
			}
			if (mainForm.chk_global_reserve_gold.Checked)
			{
				config.setConfig(this.ServerName, "gold_reserve", mainForm.num_global_reserve_gold.Value.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "gold_reserve", "0");
			}
			if (mainForm.chk_global_reserve_silver.Checked)
			{
				config.setConfig(this.ServerName, "silver_reserve", mainForm.num_global_reserve_silver.Value.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "silver_reserve", "0");
			}
			if (mainForm.chk_global_reserve_stone.Checked)
			{
				config.setConfig(this.ServerName, "stone_reserve", mainForm.num_global_reserve_stone.Value.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "stone_reserve", "0");
			}
			if (mainForm.chk_global_reserve_credit.Checked)
			{
				config.setConfig(this.ServerName, "credit_reserve", mainForm.num_global_reserve_credit.Value.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "credit_reserve", "0");
			}
			config.setConfig(this.ServerName, "gem_price", mainForm.num_global_gem_price.Value.ToString());
			bool @checked = mainForm.chk_global_boss_key_enable.Checked;
			bool checked2 = mainForm.chk_global_boss_key_ctrl.Checked;
			bool checked3 = mainForm.chk_global_boss_key_alt.Checked;
			bool checked4 = mainForm.chk_global_boss_key_shift.Checked;
			string text = "";
			if (mainForm.combo_global_boss_key_key.SelectedIndex >= 0)
			{
				text = mainForm.combo_global_boss_key_key.SelectedItem.ToString();
			}
			config.setConfig(this.ServerName, "boss_enable", @checked.ToString());
			config.setConfig(this.ServerName, "boss_shift", checked4.ToString());
			config.setConfig(this.ServerName, "boss_ctrl", checked2.ToString());
			config.setConfig(this.ServerName, "boss_alt", checked3.ToString());
			config.setConfig(this.ServerName, "boss_key", text);
			if (@checked && text != "")
			{
				if (Enum.IsDefined(typeof(Keys), text))
				{
					Keys key = (Keys)Enum.Parse(typeof(Keys), text);
					mainForm.setKeyHook(checked4, checked2, checked3, key);
				}
			}
			else
			{
				mainForm.removeKeyHook();
			}
			bool checked5 = mainForm.chk_auto_hide_flash.Checked;
			mainForm.Auto_hide_flash = checked5;
			config.setConfig(this.ServerName, "auto_hide_flash", checked5.ToString());
            config.setConfig(this.ServerName, ConfigStrings.ticket_bighero, mainForm.num_ticket_bighero.Value.ToString());
		}

		public override void renderSettings()
		{
            _mainForm.refreshPlayerInfo();
            if (_mainForm.getConfig() == null)
			{
				return;
			}
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.ContainsKey("auto_logon_award") && config["auto_logon_award"] != "")
			{
                _mainForm.chk_global_logonAward.Checked = config["auto_logon_award"].ToLower().Equals("true");
			}
			if (config.ContainsKey("auto_cai") && config["auto_cai"] != "")
			{
                _mainForm.chk_Cai.Checked = config["auto_cai"].ToLower().Equals("true");
			}
			if (config.ContainsKey("auto_buy_credit") && config["auto_buy_credit"] != "")
			{
                _mainForm.chk_AutoBuyCredit.Checked = config["auto_buy_credit"].ToLower().Equals("true");
			}
			int num = 0;
			if (config.ContainsKey("gold_reserve") && config["gold_reserve"] != "")
			{
				int.TryParse(config["gold_reserve"], out num);
				if (num > 0)
				{
                    _mainForm.chk_global_reserve_gold.Checked = true;
                    _mainForm.num_global_reserve_gold.Value = num;
				}
			}
			if (config.ContainsKey("silver_reserve") && config["silver_reserve"] != "")
			{
				int.TryParse(config["silver_reserve"], out num);
				if (num > 0)
				{
                    _mainForm.chk_global_reserve_silver.Checked = true;
                    _mainForm.num_global_reserve_silver.Value = num;
				}
			}
			if (config.ContainsKey("stone_reserve") && config["stone_reserve"] != "")
			{
				int.TryParse(config["stone_reserve"], out num);
				if (num > 0)
				{
                    _mainForm.chk_global_reserve_stone.Checked = true;
                    _mainForm.num_global_reserve_stone.Value = num;
				}
			}
			if (config.ContainsKey("credit_reserve") && config["credit_reserve"] != "")
			{
				int.TryParse(config["credit_reserve"], out num);
				if (num > 0)
				{
                    _mainForm.chk_global_reserve_credit.Checked = true;
                    _mainForm.num_global_reserve_credit.Value = num;
				}
			}
			if (config.ContainsKey("gem_price") && config["gem_price"] != "")
			{
				int.TryParse(config["gem_price"], out num);
				if (num > 0)
				{
                    _mainForm.num_global_gem_price.Value = num;
				}
			}
			bool flag = config.ContainsKey("boss_enable") && config["boss_enable"].ToLower().Equals("true");
            _mainForm.chk_global_boss_key_enable.Checked = flag;
			bool flag2 = config.ContainsKey("boss_shift") && config["boss_shift"].ToLower().Equals("true");
            _mainForm.chk_global_boss_key_shift.Checked = flag2;
			bool flag3 = config.ContainsKey("boss_ctrl") && config["boss_ctrl"].ToLower().Equals("true");
            _mainForm.chk_global_boss_key_ctrl.Checked = flag3;
			bool flag4 = config.ContainsKey("boss_alt") && config["boss_alt"].ToLower().Equals("true");
            _mainForm.chk_global_boss_key_alt.Checked = flag4;
			string text = "";
			if (config.ContainsKey("boss_key") && config["boss_key"] != "")
			{
				text = config["boss_key"];
                _mainForm.combo_global_boss_key_key.SelectedItem = text;
			}
			if (flag && text != "")
			{
				if (Enum.IsDefined(typeof(Keys), text))
				{
					Keys key = (Keys)Enum.Parse(typeof(Keys), text);
                    _mainForm.setKeyHook(flag2, flag3, flag4, key);
				}
			}
			else
			{
                _mainForm.removeKeyHook();
			}
			bool flag5 = config.ContainsKey("auto_hide_flash") && config["auto_hide_flash"].ToLower().Equals("true");
            _mainForm.Auto_hide_flash = flag5;
            _mainForm.chk_auto_hide_flash.Checked = flag5;
            if (config.ContainsKey(ConfigStrings.ticket_bighero) && config[ConfigStrings.ticket_bighero] != "")
            {
                int ticket = int.Parse(config[ConfigStrings.ticket_bighero]);
                _mainForm.num_ticket_bighero.Value = ticket;
            }
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "auto_logon_award", "true");
			expr_06.setConfig(this.ServerName, "auto_cai", "false");
			expr_06.setConfig(this.ServerName, "auto_buy_credit", "true");
			expr_06.setConfig(this.ServerName, "gold_reserve", "200");
			expr_06.setConfig(this.ServerName, "silver_reserve", "100000");
			expr_06.setConfig(this.ServerName, "stone_reserve", "500");
			expr_06.setConfig(this.ServerName, "credit_reserve", "20000");
			expr_06.setConfig(this.ServerName, "gem_price", "100");
			expr_06.setConfig(this.ServerName, "boss_enable", "false");
			expr_06.setConfig(this.ServerName, "boss_shift", "false");
			expr_06.setConfig(this.ServerName, "boss_ctrl", "false");
			expr_06.setConfig(this.ServerName, "boss_alt", "false");
			expr_06.setConfig(this.ServerName, "boss_key", "");
			expr_06.setConfig(this.ServerName, "auto_hide_flash", "false");
            expr_06.setConfig(this.ServerName, ConfigStrings.ticket_bighero, "0");
			this.renderSettings();
		}
	}
}
