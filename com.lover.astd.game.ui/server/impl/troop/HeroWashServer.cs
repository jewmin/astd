using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using com.lover.astd.common.model;

namespace com.lover.astd.game.ui.server.impl.troop
{
	public class HeroWashServer : LogicServer
	{
        BindingSource bindingSource;

		public HeroWashServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "wash";
			this.ServerReadableName = "武将洗属性";
            bindingSource = new BindingSource();
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
            config.setConfig(this.ServerName, "open_awaken", this._mainForm.chk_OpenAwaken.Checked.ToString());
            config.setConfig(this.ServerName, "use_awaken_wine", this._mainForm.chk_UseAwakenWine.Checked.ToString());
            config.setConfig(this.ServerName, "only_awaken", this._mainForm.chk_OnlyAwaken.Checked.ToString());
            config.setConfig(this.ServerName, "bh_ids", base.getDataGridSelectedIds<BigHero>(this._mainForm.dgv_AwakenGeneral));
		}

		public override void renderSettings()
		{
			if (this._mainForm.combo_hero_wash.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser().Heroes;
				this._mainForm.combo_hero_wash.DataSource = bindingSource;
			}

            if (this._mainForm.dgv_AwakenGeneral.DataSource == null)
            {
                bindingSource.DataSource = base.getUser().BigHeroList;
                this._mainForm.dgv_AwakenGeneral.AutoGenerateColumns = false;
                this._mainForm.dgv_AwakenGeneral.DataSource = bindingSource;
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
            this._mainForm.chk_OpenAwaken.Checked = config.ContainsKey("open_awaken") && config["open_awaken"].ToLower().Equals("true");
            this._mainForm.chk_UseAwakenWine.Checked = config.ContainsKey("use_awaken_wine") && config["use_awaken_wine"].ToLower().Equals("true");
            this._mainForm.chk_OnlyAwaken.Checked = config.ContainsKey("only_awaken") && config["only_awaken"].ToLower().Equals("true");
            if (config.ContainsKey("bh_ids"))
            {
                base.renderDataGridToSelected<BigHero>(this._mainForm.dgv_AwakenGeneral, config["bh_ids"]);
            }
			this.refreshData();
		}

		private void refreshData()
		{
            this._mainForm.combo_hero_wash.Refresh();
            bindingSource.DataSource = base.getUser().BigHeroList;
            this._mainForm.dgv_AwakenGeneral.Refresh();
		}

		public override void loadDefaultSettings()
		{
            GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", "true");
            config.setConfig(this.ServerName, "wash_hero", "");
            config.setConfig(this.ServerName, "wash_attrib", "000");
            config.setConfig(this.ServerName, "open_awaken", "false");
            config.setConfig(this.ServerName, "use_awaken_wine", "false");
            config.setConfig(this.ServerName, "only_awaken", "false");
            config.setConfig(this.ServerName, "bh_ids", "");
			this.renderSettings();
		}
	}
}
