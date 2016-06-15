using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.activities
{
	internal class YueBingServer : LogicServer
	{
        public YueBingServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "yuebing";
			this.ServerReadableName = "阅兵";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_AutoYuebing.Checked = (config.ContainsKey("yuebingenabled") && config["yuebingenabled"].ToLower().Equals("true"));
            _mainForm.chk_GoldTrain.Checked = (config.ContainsKey("goldtrain") && config["goldtrain"].ToLower().Equals("true"));
            _mainForm.chk_AutoYuebingHongbao.Checked = (config.ContainsKey("yuebinghongbao") && config["yuebinghongbao"].ToLower().Equals("true"));
			if (config.ContainsKey("yuebingboxtype"))
			{
				int num = 0;
				int.TryParse(config["yuebingboxtype"], out num);
                if (num < 0 || num >= _mainForm.cb_OpenYuebingHongbaoType.Items.Count)
				{
					num = 0;
				}
                _mainForm.cb_OpenYuebingHongbaoType.SelectedIndex = num;
				return;
			}
            _mainForm.cb_OpenYuebingHongbaoType.SelectedIndex = 0;
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "yuebingenabled", _mainForm.chk_AutoYuebing.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "goldtrain", _mainForm.chk_GoldTrain.Checked.ToString());
            expr_0D.setConfig(this.ServerName, "yuebinghongbao", _mainForm.chk_AutoYuebingHongbao.Checked.ToString());
            int num = _mainForm.cb_OpenYuebingHongbaoType.SelectedIndex;
			if (num < 0)
			{
				num = 0;
			}
			expr_0D.setConfig(this.ServerName, "yuebingboxtype", num.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "yuebingenabled", "true");
			expr_06.setConfig(this.ServerName, "goldtrain", "false");
			expr_06.setConfig(this.ServerName, "yuebinghongbao", "true");
			expr_06.setConfig(this.ServerName, "yuebingboxtype", "0");
			this.renderSettings();
		}
	}
}
