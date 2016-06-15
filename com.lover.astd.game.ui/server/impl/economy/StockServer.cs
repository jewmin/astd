using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	internal class StockServer : LogicServer
	{
		public StockServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "stock";
			this.ServerReadableName = "市场";
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
                _mainForm.chk_stock_enable.Checked = config["enabled"].ToLower().Equals("true");
			}
			if (config.ContainsKey("stock_id"))
			{
                _mainForm.chk_stock_1.Checked = false;
                _mainForm.chk_stock_2.Checked = false;
                _mainForm.chk_stock_3.Checked = false;
                _mainForm.chk_stock_4.Checked = false;
                _mainForm.chk_stock_5.Checked = false;
                _mainForm.chk_stock_6.Checked = false;
                _mainForm.chk_stock_7.Checked = false;
				string[] array = config["stock_id"].Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text.Length != 0)
					{
						int num = 0;
						int.TryParse(text, out num);
						if (num >= 1 && num <= 7)
						{
							if (num == 1)
							{
                                _mainForm.chk_stock_1.Checked = true;
							}
							if (num == 2)
							{
                                _mainForm.chk_stock_2.Checked = true;
							}
							if (num == 3)
							{
                                _mainForm.chk_stock_3.Checked = true;
							}
							if (num == 4)
							{
                                _mainForm.chk_stock_4.Checked = true;
							}
							if (num == 5)
							{
                                _mainForm.chk_stock_5.Checked = true;
							}
							if (num == 6)
							{
                                _mainForm.chk_stock_6.Checked = true;
							}
							if (num == 7)
							{
                                _mainForm.chk_stock_7.Checked = true;
							}
						}
					}
				}
			}
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_stock_enable.Checked.ToString());
			string text = "";
            if (_mainForm.chk_stock_1.Checked)
			{
				text += "1,";
			}
            if (_mainForm.chk_stock_2.Checked)
			{
				text += "2,";
			}
            if (_mainForm.chk_stock_3.Checked)
			{
				text += "3,";
			}
            if (_mainForm.chk_stock_4.Checked)
			{
				text += "4,";
			}
            if (_mainForm.chk_stock_5.Checked)
			{
				text += "5,";
			}
            if (_mainForm.chk_stock_6.Checked)
			{
				text += "6,";
			}
            if (_mainForm.chk_stock_7.Checked)
			{
				text += "7,";
			}
			if (text.Length > 0)
			{
				string expr_F0 = text;
				if (expr_F0[expr_F0.Length - 1].Equals(","))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			expr_0D.setConfig(this.ServerName, "stock_id", text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "stock_id", "2,3");
			this.renderSettings();
		}
	}
}
