using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class TowerServer : LogicServer
	{
		public TowerServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "tower";
			this.ServerReadableName = "宝塔活动";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_tower_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("tower_type"))
			{
				int num = 0;
				int.TryParse(config["tower_type"], out num);
                if (num < 0 || num > _mainForm.combo_tower_type.Items.Count)
				{
					num = 0;
				}
				num--;
				if (num < -1)
				{
					num = -1;
				}
                _mainForm.combo_tower_type.SelectedIndex = num;
				return;
			}
            _mainForm.combo_tower_type.SelectedIndex = 0;
		}

		public override void saveSettings()
		{            
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_tower_enable.Checked.ToString());
            int num = _mainForm.combo_tower_type.SelectedIndex;
			if (num < 0)
			{
				num = 0;
			}
			num++;
			expr_0D.setConfig(this.ServerName, "tower_type", num.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "tower_type", "1");
			this.renderSettings();
		}
	}
}
