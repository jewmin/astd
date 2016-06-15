using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl
{
	public class QiFuServer : LogicServer
	{
		public QiFuServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "qifu";
			this.ServerReadableName = "祈福活动";
		}

		public override void renderSettings()
		{
			int value = 0;
            _mainForm.refreshPlayerInfo();
            if (_mainForm.getConfig() == null)
			{
				return;
			}
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.ContainsKey("qifuenable") && config["qifuenable"] != "")
			{
                _mainForm.checkBox_QiFu.Checked = config["qifuenable"].ToLower().Equals("true");
			}
			if (config.ContainsKey("qifucoin") && config["qifucoin"] != "")
			{
				int.TryParse(config["qifucoin"], out value);
                _mainForm.downUp_QiFuCoin.Value = value;
			}
			if (config.ContainsKey("qifudq") && config["qifudq"] != "")
			{
				int.TryParse(config["qifudq"], out value);
                _mainForm.downUp_QiFuDQ.Value = value;
			}
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            if (_mainForm.checkBox_QiFu.Checked)
			{
				config.setConfig(this.ServerName, "qifuenable", "true");
			}
			else
			{
				config.setConfig(this.ServerName, "qifuenable", "false");
			}
            config.setConfig(this.ServerName, "qifucoin", _mainForm.downUp_QiFuCoin.Value.ToString());
            config.setConfig(this.ServerName, "qifudq", _mainForm.downUp_QiFuDQ.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig conf = base.getConfig();
			conf.setConfig(this.ServerName, "qifuenable", "false");
			conf.setConfig(this.ServerName, "qifucoin", "0");
			conf.setConfig(this.ServerName, "qifudq", "1000000");
			this.renderSettings();
		}
	}
}
