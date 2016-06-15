using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class MarketServer : LogicServer
	{
        public MarketServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "market";
			this.ServerReadableName = "集市";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_market_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_market_notbuy_iffail.Checked = (config.ContainsKey("notbuy_iffail") && config["notbuy_iffail"].ToLower().Equals("true"));
            _mainForm.chk_market_drop_notbuy.Checked = (config.ContainsKey("drop_notbuy") && config["drop_notbuy"].ToLower().Equals("true"));
            _mainForm.chk_market_super.Checked = (config.ContainsKey("super_supply") && config["super_supply"].ToLower().Equals("true"));
            _mainForm.chk_market_usetoken.Checked = (config.ContainsKey("use_token") && config["use_token"].ToLower().Equals("true"));
			if (config.ContainsKey("items"))
			{
				string[] arg_1D4_0 = config["items"].Split(new char[]
				{
					','
				});
                _mainForm.chk_market_gem.Checked = (_mainForm.chk_market_gem_gold.Checked = false);
                _mainForm.chk_market_stone.Checked = (_mainForm.chk_market_stone_gold.Checked = false);
                _mainForm.chk_market_silver.Checked = false;
                _mainForm.chk_market_silver_gold.Checked = true;
                _mainForm.chk_market_redweapon.Checked = (_mainForm.chk_market_redweapon_gold.Checked = false);
                _mainForm.chk_market_purpleweapon.Checked = (_mainForm.chk_market_purpleweapon_gold.Checked = false);
                _mainForm.chk_market_force_attack.Checked = false;
				string[] array = arg_1D4_0;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text != null && text.Length != 0)
					{
						string[] array2 = text.Split(new char[]
						{
							':'
						});
						if (array2.Length >= 3)
						{
							string text2 = array2[0];
							string s = array2[1];
							string text3 = array2[2];
							int num = -1;
							if (!int.TryParse(s, out num))
							{
								num = 5;
							}
							if (text2.Equals("宝石"))
							{
                                _mainForm.chk_market_gem.Checked = true;
                                _mainForm.chk_market_gem_gold.Checked = text3.ToLower().Equals("true");
								if (num <= 5)
								{
                                    _mainForm.combo_market_gem_type.SelectedIndex = num - 1;
								}
							}
							else if (text2.Equals("玉石"))
							{
                                _mainForm.chk_market_stone.Checked = true;
                                _mainForm.chk_market_stone_gold.Checked = text3.ToLower().Equals("true");
								if (num <= 5)
								{
                                    _mainForm.combo_market_stone_type.SelectedIndex = num - 1;
								}
							}
							else if (text2.Equals("银币"))
							{
                                _mainForm.chk_market_silver.Checked = true;
                                _mainForm.chk_market_silver_gold.Checked = true;
								if (num <= 5)
								{
                                    _mainForm.combo_market_silver_type.SelectedIndex = num - 1;
								}
							}
							else if (text2.Equals("redweapon"))
							{
                                _mainForm.chk_market_redweapon.Checked = true;
                                _mainForm.chk_market_redweapon_gold.Checked = text3.ToLower().Equals("true");
								if (num <= 5)
								{
                                    _mainForm.combo_market_redweapon_type.SelectedIndex = num - 1;
								}
							}
							else if (text2.Equals("purpleweapon"))
							{
                                _mainForm.chk_market_purpleweapon.Checked = true;
                                _mainForm.chk_market_purpleweapon_gold.Checked = text3.ToLower().Equals("true");
								if (num <= 5)
								{
                                    _mainForm.combo_market_purpleweapon_type.SelectedIndex = num - 1;
								}
							}
							else if (text2.Equals("强攻令"))
							{
                                _mainForm.chk_market_force_attack.Checked = true;
							}
						}
					}
				}
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_market_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "notbuy_iffail", _mainForm.chk_market_notbuy_iffail.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "drop_notbuy", _mainForm.chk_market_drop_notbuy.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "super_supply", _mainForm.chk_market_super.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "use_token", _mainForm.chk_market_usetoken.Checked.ToString());
			string text = "";
            if (_mainForm.chk_market_gem.Checked)
			{
                text += string.Format("宝石:{0}:{1},", _mainForm.combo_market_gem_type.SelectedIndex + 1, _mainForm.chk_market_gem_gold.Checked.ToString());
			}
            if (_mainForm.chk_market_stone.Checked)
			{
                text += string.Format("玉石:{0}:{1},", _mainForm.combo_market_stone_type.SelectedIndex + 1, _mainForm.chk_market_stone_gold.Checked.ToString());
			}
            if (_mainForm.chk_market_silver.Checked)
			{
                text += string.Format("银币:{0}:{1},", _mainForm.combo_market_silver_type.SelectedIndex + 1, _mainForm.chk_market_silver_gold.Checked.ToString());
			}
            if (_mainForm.chk_market_redweapon.Checked)
			{
                text += string.Format("redweapon:{0}:{1},", _mainForm.combo_market_redweapon_type.SelectedIndex + 1, _mainForm.chk_market_redweapon_gold.Checked.ToString());
			}
            if (_mainForm.chk_market_purpleweapon.Checked)
			{
                text += string.Format("purpleweapon:{0}:{1},", _mainForm.combo_market_purpleweapon_type.SelectedIndex + 1, _mainForm.chk_market_purpleweapon_gold.Checked.ToString());
			}
            if (_mainForm.chk_market_force_attack.Checked)
			{
				text += "强攻令:1:true,";
			}
			if (text.Length > 0)
			{
				text = text.Substring(0, text.Length - 1);
			}
			expr_0D.setConfig(this.ServerName, "items", text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "notbuy_iffail", "false");
			expr_06.setConfig(this.ServerName, "drop_notbuy", "false");
			expr_06.setConfig(this.ServerName, "super_supply", "true");
			expr_06.setConfig(this.ServerName, "use_token", "true");
			expr_06.setConfig(this.ServerName, "items", "宝石:1:false,玉石:2:false,purpleweapon:2:false,强攻令:1:true");
			this.renderSettings();
		}
	}
}
