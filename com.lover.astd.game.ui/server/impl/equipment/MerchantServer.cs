using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class MerchantServer : LogicServer
	{
        public MerchantServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "merchant";
			this.ServerReadableName = "委派";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_merchant.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("type"))
			{
                _mainForm.combo_merchant_type.SelectedItem = config["type"];
			}
            _mainForm.chk_merchant_onlyfree.Checked = (config.ContainsKey("only_free") && config["only_free"].ToLower().Equals("true"));
			if (config.ContainsKey("sell_quality"))
			{
                _mainForm.combo_merchant_sell_level.SelectedItem = config["sell_quality"];
			}
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_merchant.Checked.ToString());
            if (_mainForm.combo_merchant_type.SelectedItem != null)
			{
                config.setConfig(this.ServerName, "type", _mainForm.combo_merchant_type.SelectedItem.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "type", "马");
			}
            config.setConfig(this.ServerName, "only_free", _mainForm.chk_merchant_onlyfree.Checked.ToString());
            if (_mainForm.combo_merchant_sell_level.SelectedItem != null)
			{
                config.setConfig(this.ServerName, "sell_quality", _mainForm.combo_merchant_sell_level.SelectedItem.ToString());
				return;
			}
			config.setConfig(this.ServerName, "sell_quality", "蓝色");
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "type", "马");
			expr_06.setConfig(this.ServerName, "only_free", "true");
			expr_06.setConfig(this.ServerName, "sell_quality", "红色");
			this.renderSettings();
		}
	}
}
