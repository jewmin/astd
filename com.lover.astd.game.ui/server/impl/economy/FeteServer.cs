using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class FeteServer : LogicServer
	{
		public FeteServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "fete";
			this.ServerReadableName = "祭祀";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.Count == 0)
			{
				return;
			}
			if (config.ContainsKey("enabled"))
			{
                _mainForm.chk_fete_enable.Checked = config["enabled"].ToLower().Equals("true");
			}
			if (config.ContainsKey("fete_gold"))
			{
				string[] array = config["fete_gold"].Split(new char[]
				{
					','
				});
				int value = 0;
				if (array.Length != 0)
				{
					int.TryParse(array[0], out value);
                    _mainForm.num_fete_1.Value = value;
				}
				if (array.Length > 1)
				{
					int.TryParse(array[1], out value);
                    _mainForm.num_fete_2.Value = value;
				}
				if (array.Length > 2)
				{
					int.TryParse(array[2], out value);
                    _mainForm.num_fete_3.Value = value;
				}
				if (array.Length > 3)
				{
					int.TryParse(array[3], out value);
                    _mainForm.num_fete_4.Value = value;
				}
				if (array.Length > 4)
				{
					int.TryParse(array[4], out value);
                    _mainForm.num_fete_5.Value = value;
				}
				if (array.Length > 5)
				{
					int.TryParse(array[5], out value);
                    _mainForm.num_fete_6.Value = value;
				}
			}
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_fete_enable.Checked.ToString());
			string conf = string.Format("{0},{1},{2},{3},{4},{5}", new object[]
			{
				_mainForm.num_fete_1.Value,
				_mainForm.num_fete_2.Value,
				_mainForm.num_fete_3.Value,
				_mainForm.num_fete_4.Value,
				_mainForm.num_fete_5.Value,
				_mainForm.num_fete_6.Value
			});
			config.setConfig(this.ServerName, "fete_gold", conf);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "fete_gold", "2,0,0,2,2,6");
			this.renderSettings();
		}
	}
}
