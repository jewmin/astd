using com.lover.astd.common.config;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class QhServer : LogicServer
	{
		public QhServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "qh";
			this.ServerReadableName = "强化管理";
		}

		public override void renderSettings()
		{
			this.refreshData();
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_equip_qh_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("qh_ids"))
			{
                base.renderDataGridToSelected<Equipment>(_mainForm.dg_equipQh, config["qh_ids"]);
			}
            _mainForm.chk_equip_qh_use_gold.Checked = (config.ContainsKey("use_gold") && config["use_gold"].ToLower().Equals("true"));
			if (config.ContainsKey("use_gold_limit") && config["use_gold_limit"] != "")
			{
				int value = 0;
				int.TryParse(config["use_gold_limit"], out value);
                _mainForm.num_equip_qh_use_gold_limit.Value = value;
			}
            _mainForm.chk_upgrade_toukui.Checked = (config.ContainsKey("toukui_enabled") && config["toukui_enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("toukui_ticket") && config["toukui_ticket"] != "")
			{
				int value2 = 0;
				int.TryParse(config["toukui_ticket"], out value2);
                _mainForm.nUD_toukui_ticket.Value = value2;
			}
			this.refreshData();
		}

		private void refreshData()
		{
			if (this._mainForm.dg_equipQh.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser()._qhEquips;
				this._mainForm.dg_equipQh.AutoGenerateColumns = false;
				this._mainForm.dg_equipQh.DataSource = bindingSource;
			}
			this._mainForm.dg_equipQh.Refresh();
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_equip_qh_enable.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "qh_ids", base.getDataGridSelectedIds<Equipment>(_mainForm.dg_equipQh));
            expr_0D.setConfig(this.ServerName, "use_gold", _mainForm.chk_equip_qh_use_gold.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "use_gold_limit", _mainForm.num_equip_qh_use_gold_limit.Value.ToString());
            expr_0D.setConfig(this.ServerName, "toukui_enabled", _mainForm.chk_upgrade_toukui.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "toukui_ticket", _mainForm.nUD_toukui_ticket.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "qh_ids", "");
			expr_06.setConfig(this.ServerName, "use_gold", "true");
			expr_06.setConfig(this.ServerName, "use_gold_limit", "5000000");
			expr_06.setConfig(this.ServerName, "toukui_enabled", "false");
			expr_06.setConfig(this.ServerName, "toukui_ticket", "10000");
			this.renderSettings();
		}

		private List<int> getEquipIdsToQianghua()
		{
			Dictionary<string, string> config = base.getConfig("equip_qh");
			string id_string = "";
			if (config.ContainsKey("qh_ids"))
			{
				id_string = config["qh_ids"];
			}
			return base.generateIds(id_string);
		}
	}
}
