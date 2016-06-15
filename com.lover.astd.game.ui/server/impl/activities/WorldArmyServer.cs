using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class WorldArmyServer : LogicServer
	{
		public WorldArmyServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "worldarmy";
			this.ServerReadableName = "世界军团";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_world_army_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_world_army_openbox.Checked = (config.ContainsKey("auto_open_box") && config["auto_open_box"].ToLower().Equals("true"));
			if (config.ContainsKey("boxtype"))
			{
				int num = 0;
				int.TryParse(config["boxtype"], out num);
                if (num < 0 || num >= _mainForm.combo_world_army_boxtype.Items.Count)
				{
					num = 0;
				}
                _mainForm.combo_world_army_boxtype.SelectedIndex = num;
			}
			else
			{
                _mainForm.combo_world_army_boxtype.SelectedIndex = 0;
			}
			if (config.ContainsKey("refresh"))
			{
				int num2 = 0;
				int.TryParse(config["refresh"], out num2);
                if (num2 < 0 || num2 >= _mainForm.combo_world_army_refresh_type.Items.Count)
				{
					num2 = 0;
				}
                _mainForm.combo_world_army_refresh_type.SelectedIndex = num2;
			}
			else
			{
                _mainForm.combo_world_army_refresh_type.SelectedIndex = 0;
			}
            _mainForm.chk_world_army_buybox1.Checked = (config.ContainsKey("buybox1") && config["buybox1"].ToLower().Equals("true"));
            _mainForm.chk_world_army_buybox2.Checked = (config.ContainsKey("buybox2") && config["buybox2"].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_world_army_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "auto_open_box", _mainForm.chk_world_army_openbox.Checked.ToString());
            int num = _mainForm.combo_world_army_boxtype.SelectedIndex;
			if (num < 0)
			{
				num = 0;
			}
			expr_0D.setConfig(this.ServerName, "boxtype", num.ToString());
            int num2 = _mainForm.combo_world_army_refresh_type.SelectedIndex;
			if (num2 < 0)
			{
				num2 = 0;
			}
			expr_0D.setConfig(this.ServerName, "refresh", num2.ToString());
            expr_0D.setConfig(this.ServerName, "buybox1", _mainForm.chk_world_army_buybox1.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "buybox2", _mainForm.chk_world_army_buybox2.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "auto_open_box", "true");
			expr_06.setConfig(this.ServerName, "boxtype", "0");
			expr_06.setConfig(this.ServerName, "refresh", "0");
			expr_06.setConfig(this.ServerName, "buybox1", "false");
			expr_06.setConfig(this.ServerName, "buybox2", "false");
			this.renderSettings();
		}
	}
}
