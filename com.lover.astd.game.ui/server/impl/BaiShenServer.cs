using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl
{
	public class BaiShenServer : LogicServer
	{
        public BaiShenServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "baishen";
			this.ServerReadableName = "拜神";
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
			if (config.ContainsKey("baishenenable") && config["baishenenable"] != "")
			{
                _mainForm.chk_AutoBaiShen.Checked = config["baishenenable"].ToLower().Equals("true");
			}
			if (config.ContainsKey("advancebaoshi") && config["advancebaoshi"] != "")
			{
				int.TryParse(config["advancebaoshi"], out value);
                _mainForm.nUD_BaoShi.Value = value;
			}
			if (config.ContainsKey("advancedianquan") && config["advancedianquan"] != "")
			{
				int.TryParse(config["advancedianquan"], out value);
                _mainForm.nUD_DianQuan.Value = value;
			}
			if (config.ContainsKey("superusefree") && config["superusefree"] != "")
			{
                _mainForm.chk_SuperUseFree.Checked = config["superusefree"].ToLower().Equals("true");
			}
			if (config.ContainsKey("cishu") && config["cishu"] != "")
			{
				int.TryParse(config["cishu"], out value);
                _mainForm.nUD_CiShu.Value = value;
			}
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            if (_mainForm.chk_AutoBaiShen.Checked)
			{
				config.setConfig(this.ServerName, "baishenenable", "true");
			}
			else
			{
				config.setConfig(this.ServerName, "baishenenable", "false");
			}
            config.setConfig(this.ServerName, "advancebaoshi", _mainForm.nUD_BaoShi.Value.ToString());
            config.setConfig(this.ServerName, "advancedianquan", _mainForm.nUD_DianQuan.Value.ToString());
            if (_mainForm.chk_SuperUseFree.Checked)
			{
				config.setConfig(this.ServerName, "superusefree", "true");
			}
			else
			{
				config.setConfig(this.ServerName, "superusefree", "false");
			}
            config.setConfig(this.ServerName, "cishu", _mainForm.nUD_CiShu.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "baishenenable", "false");
			expr_06.setConfig(this.ServerName, "advancebaoshi", "3000");
			expr_06.setConfig(this.ServerName, "advancedianquan", "60000");
			expr_06.setConfig(this.ServerName, "superusefree", "false");
			expr_06.setConfig(this.ServerName, "cishu", "0");
			this.renderSettings();
		}
	}
}
