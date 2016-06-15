using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class BoatServer : LogicServer
	{
        public BoatServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "boat";
			this.ServerReadableName = "龙舟";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_AutoBoat.Checked = (config.ContainsKey("boatenabled") && config["boatenabled"].ToLower().Equals("true"));
            _mainForm.chk_boat_creat.Checked = (config.ContainsKey("boatcreate") && config["boatcreate"].ToLower().Equals("true"));
			if (config.ContainsKey("boatupgrade"))
			{
                _mainForm.txt_boat_upgrade.Text = config["boatupgrade"];
			}
			else
			{
                _mainForm.txt_boat_upgrade.Text = "";
			}
            _mainForm.chk_AutoSelectTime.Checked = (config.ContainsKey("boatselect") && config["boatselect"].ToLower().Equals("true"));
			int value = 0;
			if (config.ContainsKey("boatcoin"))
			{
				int.TryParse(config["boatcoin"], out value);
			}
            _mainForm.nUD_boat_up_coin.Value = value;
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "boatenabled", _mainForm.chk_AutoBoat.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "boatcreate", _mainForm.chk_boat_creat.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "boatupgrade", _mainForm.txt_boat_upgrade.Text);
            expr_0D.setConfig(this.ServerName, "boatselect", _mainForm.chk_AutoSelectTime.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "boatcoin", _mainForm.nUD_boat_up_coin.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "boatenabled", "true");
			expr_06.setConfig(this.ServerName, "boatcreate", "true");
			expr_06.setConfig(this.ServerName, "boatselect", "true");
			expr_06.setConfig(this.ServerName, "boatupgrade", "");
			expr_06.setConfig(this.ServerName, "boatcoin", "0");
			this.renderSettings();
		}
	}
}
