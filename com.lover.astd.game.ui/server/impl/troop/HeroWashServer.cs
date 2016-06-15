using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server.impl.troop
{
	public class HeroWashServer : LogicServer
	{
		public HeroWashServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "wash";
			this.ServerReadableName = "武将洗属性";
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_hero_wash_enable.Checked.ToString());
            if (_mainForm.combo_hero_wash.SelectedValue == null)
			{
				config.setConfig(this.ServerName, "wash_hero", "");
			}
			else
			{
                config.setConfig(this.ServerName, "wash_hero", _mainForm.combo_hero_wash.SelectedValue.ToString());
			}
			string text = "";
            if (_mainForm.chk_hero_wash_tong.Checked)
			{
				text += "1";
			}
			else
			{
				text += "0";
			}
            if (_mainForm.chk_hero_wash_yong.Checked)
			{
				text += "1";
			}
			else
			{
				text += "0";
			}
            if (_mainForm.chk_hero_wash_zhi.Checked)
			{
				text += "1";
			}
			else
			{
				text += "0";
			}
			config.setConfig(this.ServerName, "wash_attrib", text);
		}

		public override void renderSettings()
		{
			if (this._mainForm.combo_hero_wash.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser().Heroes;
				this._mainForm.combo_hero_wash.DataSource = bindingSource;
			}
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_hero_wash_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("wash_hero") && config["wash_hero"] != "")
			{
				int num = 0;
				if (int.TryParse(config["wash_hero"], out num))
				{
                    _mainForm.combo_hero_wash.SelectedValue = num;
					string text = config["wash_attrib"];
					if (text.Length >= 3)
					{
                        _mainForm.chk_hero_wash_tong.Checked = (text[0] == '1');
                        _mainForm.chk_hero_wash_yong.Checked = (text[1] == '1');
                        _mainForm.chk_hero_wash_zhi.Checked = (text[2] == '1');
					}
				}
			}
			this.refreshData();
		}

		private void refreshData()
		{
			this._mainForm.combo_hero_wash.Refresh();
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "wash_hero", "");
			expr_06.setConfig(this.ServerName, "wash_attrib", "000");
			this.renderSettings();
		}
	}
}
