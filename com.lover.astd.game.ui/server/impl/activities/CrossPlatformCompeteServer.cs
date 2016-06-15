using com.lover.astd.common.config;
using com.lover.astd.common.model.misc;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class CrossPlatformCompeteServer : LogicServer
	{
		public CrossPlatformCompeteServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "crossplatform_compete";
			this.ServerReadableName = "跨服争霸赛";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_crossplatm_compete.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			int value = 0;
			if (config.ContainsKey("support_gold"))
			{
				int.TryParse(config["support_gold"], out value);
			}
            _mainForm.num_crossplatm_compete_gold.Value = value;
            _mainForm.chk_kfzb_market.Checked = (config.ContainsKey("kfzb_market") && config["kfzb_market"].ToLower().Equals("true"));
            _mainForm.chk_kfzb_market_ignore.Checked = (config.ContainsKey("kfzb_market_ignore") && config["kfzb_market_ignore"].ToLower().Equals("true"));
            _mainForm.chk_kfzb_market_silver.Checked = false;
            _mainForm.chk_kfzb_market_stone.Checked = false;
            _mainForm.chk_kfzb_market_gem.Checked = false;
            _mainForm.chk_kfzb_market_weapon_1.Checked = false;
			if (config.ContainsKey("kfzb_market_conf"))
			{
				string[] array = config["kfzb_market_conf"].Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text != null && !(text == ""))
					{
						KfzbItemConfig kfzbItemConfig = new KfzbItemConfig();
						kfzbItemConfig.fill(text);
						if (kfzbItemConfig.item_type == 1)
						{
                            _mainForm.chk_kfzb_market_silver.Checked = true;
                            _mainForm.combo_kfzb_market_silver_pos.SelectedIndex = kfzbItemConfig.max_pos - 1;
						}
						else if (kfzbItemConfig.item_type == 2)
						{
                            _mainForm.chk_kfzb_market_stone.Checked = true;
                            _mainForm.combo_kfzb_market_stone_pos.SelectedIndex = kfzbItemConfig.max_pos - 1;
						}
						else if (kfzbItemConfig.item_type == 3)
						{
                            _mainForm.chk_kfzb_market_gem.Checked = true;
                            _mainForm.combo_kfzb_market_gem_pos.SelectedIndex = kfzbItemConfig.max_pos - 1;
						}
						else if (kfzbItemConfig.item_type == 4)
						{
                            _mainForm.chk_kfzb_market_weapon_1.Checked = true;
                            _mainForm.combo_kfzb_market_weapon_pos_1.SelectedIndex = kfzbItemConfig.max_pos - 1;
						}
						else if (kfzbItemConfig.item_type == 5)
						{
                            _mainForm.chk_kfzb_market_weapon_2.Checked = true;
                            _mainForm.combo_kfzb_market_weapon_pos_2.SelectedIndex = kfzbItemConfig.max_pos - 1;
						}
					}
				}
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_crossplatm_compete.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "support_gold", _mainForm.num_crossplatm_compete_gold.Value.ToString());
            expr_0D.setConfig(this.ServerName, "kfzb_market", _mainForm.chk_kfzb_market.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "kfzb_market_ignore", _mainForm.chk_kfzb_market_ignore.Checked.ToString());
			string text = "";
            if (_mainForm.chk_kfzb_market_silver.Checked)
			{
                int num = _mainForm.combo_kfzb_market_silver_pos.SelectedIndex;
				if (num == -1)
				{
					num = 0;
				}
				num++;
				text += string.Format("1:{0};", num);
			}
            if (_mainForm.chk_kfzb_market_stone.Checked)
			{
                int num2 = _mainForm.combo_kfzb_market_stone_pos.SelectedIndex;
				if (num2 == -1)
				{
					num2 = 0;
				}
				num2++;
				text += string.Format("2:{0};", num2);
			}
            if (_mainForm.chk_kfzb_market_gem.Checked)
			{
                int num3 = _mainForm.combo_kfzb_market_gem_pos.SelectedIndex;
				if (num3 == -1)
				{
					num3 = 0;
				}
				num3++;
				text += string.Format("3:{0};", num3);
			}
            if (_mainForm.chk_kfzb_market_weapon_1.Checked)
			{
                int num4 = _mainForm.combo_kfzb_market_weapon_pos_1.SelectedIndex;
				if (num4 == -1)
				{
					num4 = 0;
				}
				num4++;
				text += string.Format("4:{0};", num4);
			}
            if (_mainForm.chk_kfzb_market_weapon_2.Checked)
			{
                int num5 = _mainForm.combo_kfzb_market_weapon_pos_2.SelectedIndex;
				if (num5 == -1)
				{
					num5 = 0;
				}
				num5++;
				text += string.Format("5:{0};", num5);
			}
			expr_0D.setConfig(this.ServerName, "kfzb_market_conf", text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "support_gold", "0");
			expr_06.setConfig(this.ServerName, "kfzb_market", "true");
			expr_06.setConfig(this.ServerName, "kfzb_market_conf", "3:3;4:3;");
			expr_06.setConfig(this.ServerName, "kfzb_market_ignore", "true");
			this.renderSettings();
		}
	}
}
