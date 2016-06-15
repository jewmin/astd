using com.lover.astd.common.config;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using com.lover.astd.common;

namespace com.lover.astd.game.ui.server.impl.troop
{
	public class HeroTrainServer : LogicServer
	{
		public HeroTrainServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "hero";
			this.ServerReadableName = "武将突飞";
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_hero_tufei_enable.Checked.ToString());
            config.setConfig(this.ServerName, "nocd", _mainForm.chk_hero_nocd.Checked.ToString());
            config.setConfig(this.ServerName, "giftevent", _mainForm.chk_hero_giftevent.Checked.ToString());
            string dataGridSelectedIds = base.getDataGridSelectedIds<Hero>(_mainForm.dg_heros);
			config.setConfig(this.ServerName, "train_heros", dataGridSelectedIds);
            config.setConfig(ConfigStrings.S_BigHeroTrain, ConfigStrings.enabled, _mainForm.chk_big_hero_tufei_enable.Checked.ToString());

		}

		public override void renderSettings()
		{
			if (this._mainForm.dg_heros.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser().Heroes;
				this._mainForm.dg_heros.AutoGenerateColumns = false;
				this._mainForm.dg_heros.DataSource = bindingSource;
			}
            Dictionary<string, string> config1 = getConfig(ConfigStrings.S_BigHeroTrain);
            _mainForm.chk_big_hero_tufei_enable.Checked = (config1.ContainsKey(ConfigStrings.enabled) && config1[ConfigStrings.enabled].ToLower().Equals("true"));
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.Count == 0)
			{
				return;
			}
            _mainForm.chk_hero_tufei_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_hero_nocd.Checked = (config.ContainsKey("nocd") && config["nocd"].ToLower().Equals("true"));
            _mainForm.chk_hero_giftevent.Checked = (config.ContainsKey("giftevent") && config["giftevent"].ToLower().Equals("true"));
			if (config.ContainsKey("train_heros") && config["train_heros"] != "")
			{
				string idsToSelected = config["train_heros"];
                base.renderDataGridToSelected<Hero>(_mainForm.dg_heros, idsToSelected);
			}
		}

		public override void loadDefaultSettings()
		{
			GameConfig config = base.getConfig();
			config.setConfig(this.ServerName, "enabled", "true");
			config.setConfig(this.ServerName, "giftevent", "true");
			config.setConfig(this.ServerName, "nocd", "false");
			config.setConfig(this.ServerName, "train_heros", "");
            config.setConfig(ConfigStrings.S_BigHeroTrain, ConfigStrings.enabled, "true");
			this.renderSettings();
		}
	}
}
