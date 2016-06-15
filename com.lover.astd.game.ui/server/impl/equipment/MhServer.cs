using com.lover.astd.common.config;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class MhServer : LogicServer
	{
		public MhServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "mh";
			this.ServerReadableName = "魔化管理";
		}

		public override void renderSettings()
		{
			this.renderData();
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_equip_mh_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			int value = 0;
			if (config.ContainsKey("fail_count"))
			{
				int.TryParse(config["fail_count"], out value);
			}
            _mainForm.num_mh_failcount.Value = value;
			if (config.ContainsKey("mh_ids"))
			{
                base.renderDataGridToSelected<Equipment>(_mainForm.dg_equipMh, config["mh_ids"]);
			}
			if (config.ContainsKey("mh_type") && config["mh_type"] != "")
			{
                _mainForm.combo_mh_type.SelectedItem = config["mh_type"];
			}
			this.renderData();
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_equip_mh_enable.Checked.ToString());
            config.setConfig(this.ServerName, "fail_count", _mainForm.num_mh_failcount.Value.ToString());
            config.setConfig(this.ServerName, "mh_ids", base.getDataGridSelectedIds<Equipment>(_mainForm.dg_equipMh));
            if (_mainForm.combo_mh_type.SelectedItem != null)
			{
                config.setConfig(this.ServerName, "mh_type", _mainForm.combo_mh_type.SelectedItem.ToString());
			}
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "mh_ids", "");
			expr_06.setConfig(this.ServerName, "fail_count", "3");
			expr_06.setConfig(this.ServerName, "mh_type", "少量");
			this.renderSettings();
		}

		private void renderData()
		{
			if (this._mainForm.dg_equipMh.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser()._mhEquips;
				this._mainForm.dg_equipMh.AutoGenerateColumns = false;
				this._mainForm.dg_equipMh.DataSource = bindingSource;
			}
			this._mainForm.dg_equipMh.Refresh();
		}

		private List<int> getEquipIdsToMh()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			string id_string = "";
			if (config.ContainsKey("enchant_ids"))
			{
				id_string = config["enchant_ids"];
			}
			return base.generateIds(id_string);
		}
	}
}
