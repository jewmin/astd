using com.lover.astd.common.config;
using com.lover.astd.common.model.building;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server.impl
{
	public class BuildingServer : LogicServer
	{
		public BuildingServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "building";
			this.ServerReadableName = "建筑";
		}

		private List<int> getBuildingsToBuild()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			string id_string = "";
			if (config.ContainsKey("build_ids"))
			{
				id_string = config["build_ids"];
			}
			return base.generateIds(id_string);
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			this.renderData();
            _mainForm.chk_building_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_building_protect.Checked = (config.ContainsKey("protect") && config["protect"].ToLower().Equals("true"));
			if (config.ContainsKey("build_ids") && config["build_ids"] != "")
			{
				string idsToSelected = config["build_ids"];
                base.renderDataGridToSelected<Building>(_mainForm.dg_buildings, idsToSelected);
			}
			this.refreshData();
		}

		private void renderData()
		{
			if (this._mainForm.dg_buildings.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser()._buildings;
				this._mainForm.dg_buildings.AutoGenerateColumns = false;
				this._mainForm.dg_buildings.DataSource = bindingSource;
			}
		}

		private void refreshData()
		{
			this._mainForm.dg_buildings.Refresh();
		}

		public override void saveSettings()
		{
			GameConfig arg_0D_0 = base.getConfig();
            arg_0D_0.setConfig(this.ServerName, "enabled", _mainForm.chk_building_enable.Checked.ToString());
            arg_0D_0.setConfig(this.ServerName, "protect", _mainForm.chk_building_protect.Checked.ToString());
            string dataGridSelectedIds = base.getDataGridSelectedIds<Building>(_mainForm.dg_buildings);
			arg_0D_0.setConfig(this.ServerName, "build_ids", dataGridSelectedIds);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "protect", "true");
			expr_06.setConfig(this.ServerName, "build_ids", "");
			this.renderSettings();
		}
	}
}
