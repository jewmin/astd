using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server.impl.equipment
{
	public class WeaponServer : LogicServer
	{
        public WeaponServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "weapon";
			this.ServerReadableName = "兵器升级";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_upgrade_weapone_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			this._mainForm.txt_weaponinfo.Clear();
			string[] array = base.getUser()._weapon_info.Split(new char[]
			{
				'/'
			});
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				string text = array[i];
				string[] array2 = text.Split(new char[]
				{
					'|'
				});
				if (array2.Length == 1)
				{
					this.renderText(text, "black", true);
				}
				else
				{
					this.renderText(array2[1], array2[0], i < num - 1);
				}
				i++;
			}
		}

		private void renderText(string text, string colorstr = "black", bool addEnter = true)
		{
			Color selectionColor = Color.Black;
			if (colorstr == "gray")
			{
				selectionColor = Color.Gray;
			}
			else if (colorstr == "blue")
			{
				selectionColor = Color.Blue;
			}
			else if (colorstr == "green")
			{
				selectionColor = Color.Green;
			}
			else if (colorstr == "yellow")
			{
				selectionColor = Color.Yellow;
			}
			else if (colorstr == "red")
			{
				selectionColor = Color.Red;
			}
			else if (colorstr == "purple")
			{
				selectionColor = Color.Purple;
			}
			RichTextBox txt_weaponinfo = this._mainForm.txt_weaponinfo;
			txt_weaponinfo.AppendText(text);
			RichTextBox expr_96 = txt_weaponinfo;
			expr_96.Select(expr_96.Text.Length - text.Length, text.Length);
			txt_weaponinfo.SelectionColor = selectionColor;
			RichTextBox expr_BB = txt_weaponinfo;
			expr_BB.Select(expr_BB.Text.Length, 0);
			if (addEnter)
			{
				txt_weaponinfo.AppendText("\n");
			}
			txt_weaponinfo.ScrollToCaret();
		}

		public override void saveSettings()
		{
            base.getConfig().setConfig(this.ServerName, "enabled", _mainForm.chk_upgrade_weapone_enable.Checked.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig arg_17_0 = base.getConfig();
			string mainConfigName = "upgradeweapon";
			arg_17_0.setConfig(mainConfigName, "enabled", "true");
			this.renderSettings();
		}
	}
}
