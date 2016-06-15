using com.lover.astd.common.config;
using com.lover.astd.common.model.battle;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server.impl.battle
{
	internal class BattleServer : LogicServer
	{
		public BattleServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "battle";
			this.ServerReadableName = "征战";
		}

		private void renderData()
		{
			if (this._mainForm.dg_attack_army.DataSource == null)
			{
				BindingSource bindingSource = new BindingSource();
				bindingSource.DataSource = base.getUser()._all_armys;
				this._mainForm.dg_attack_army.AutoGenerateColumns = false;
				this._mainForm.dg_attack_army.DataSource = bindingSource;
			}
			this._mainForm.dg_attack_army.Refresh();
		}

		public override void renderSettings()
		{
			this.renderData();
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_defense_format.Checked = (config.ContainsKey("defense_enabled") && config["defense_enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("defense_formation"))
			{
                _mainForm.combo_defense_formation.SelectedItem = config["defense_formation"];
			}
			else
			{
                _mainForm.combo_defense_formation.SelectedItem = "不变阵";
			}
            _mainForm.chk_attack_npc_enable.Checked = (config.ContainsKey("npc_enabled") && config["npc_enabled"].ToLower().Equals("true"));
			if (config.ContainsKey("npc_reserve_token"))
			{
				int value = 40;
				int.TryParse(config["npc_reserve_token"], out value);
                _mainForm.num_attack_npc_reserve_order.Value = value;
			}
			if (config.ContainsKey("npc_ids"))
			{
				string text = config["npc_ids"];
				text = text.Replace(",", "");
				if (text == null || text.Length <= 0)
				{
					goto IL_1FF;
				}
				string[] array = text.Split(new char[]
				{
					':'
				});
				if (array.Length < 2)
				{
					goto IL_1FF;
				}
				int num = 0;
				int.TryParse(array[0], out num);
				string text2 = array[1];
				if (num <= 0)
				{
					goto IL_1FF;
				}
				using (List<Npc>.Enumerator enumerator = base.getUser()._all_npcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Npc current = enumerator.Current;
						if (current.Id == num)
						{
                            _mainForm.lbl_attackNpc.Text = string.Format("{0}:{1}:{2}:{3}", new object[]
							{
								current.Name,
								current.ItemName,
								text2,
								num
							});
							break;
						}
					}
					goto IL_1FF;
				}
			}
            _mainForm.lbl_attackNpc.Text = "";
			IL_1FF:
			if (config.ContainsKey("npc_force_ids"))
			{
				string text3 = config["npc_force_ids"];
				text3 = text3.Replace(",", "");
				if (text3 == null || text3.Length <= 0)
				{
					goto IL_30F;
				}
				string[] array2 = text3.Split(new char[]
				{
					':'
				});
				if (array2.Length < 2)
				{
					goto IL_30F;
				}
				int num2 = 0;
				int.TryParse(array2[0], out num2);
				string text4 = array2[1];
				if (num2 <= 0)
				{
					goto IL_30F;
				}
				using (List<Npc>.Enumerator enumerator2 = base.getUser()._all_npcs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Npc current2 = enumerator2.Current;
						if (current2.Id == num2)
						{
                            _mainForm.lbl_attackForceNpc.Text = string.Format("{0}:{1}:{2}:{3}", new object[]
							{
								current2.Name,
								current2.ItemName,
								text4,
								num2
							});
							break;
						}
					}
					goto IL_30F;
				}
			}
            _mainForm.lbl_attackForceNpc.Text = "";
			IL_30F:
            _mainForm.chk_attack_army.Checked = (config.ContainsKey("army_enabled") && config["army_enabled"].ToLower().Equals("true"));
            _mainForm.chk_attack_army_only_jingyingtime.Checked = (config.ContainsKey("attack_army_only_jingyingtime") && config["attack_army_only_jingyingtime"].ToLower().Equals("true"));
            _mainForm.chk_attack_army_first.Checked = (config.ContainsKey("attack_first") && config["attack_first"].ToLower().Equals("true"));
			if (config.ContainsKey("attack_first_interval"))
			{
				int value2 = 60;
				int.TryParse(config["attack_first_interval"], out value2);
                _mainForm.num_attack_army_first_interval.Value = value2;
			}
			if (config.ContainsKey("army_reserve_token"))
			{
				int value3 = 40;
				int.TryParse(config["army_reserve_token"], out value3);
                _mainForm.num_attack_army_reserve_order.Value = value3;
			}
			string idsToSelected = "";
			string firstIdsToSelected = "";
			if (config.ContainsKey("army_ids"))
			{
				idsToSelected = config["army_ids"];
			}
			if (config.ContainsKey("first_army_ids"))
			{
				firstIdsToSelected = config["first_army_ids"];
			}
            _mainForm.chk_battle_event_enabled.Checked = (config.ContainsKey("battle_event_enable") && config["battle_event_enable"].ToLower().Equals("true"));
			string text5 = "";
			if (config.ContainsKey("battle_event_id"))
			{
				text5 = config["battle_event_id"];
			}
            _mainForm.chk_battle_event_gem.Checked = text5.Contains("1");
            _mainForm.chk_battle_event_gem_gold.Checked = text5.Contains("4");
            _mainForm.chk_battle_event_stone.Checked = text5.Contains("2");
            _mainForm.chk_battle_event_stone_gold.Checked = text5.Contains("5");
            _mainForm.chk_battle_event_weapon.Checked = text5.Contains("3");
            _mainForm.chk_battle_event_weapon_gold.Checked = text5.Contains("6");
            this.renderDgToSelected(_mainForm.dg_attack_army, idsToSelected, firstIdsToSelected);
		}

		private void renderDgToSelected(DataGridView dg, string idsToSelected, string firstIdsToSelected)
		{
			List<Npc> list = (dg.DataSource as BindingSource).DataSource as List<Npc>;
			string[] array = idsToSelected.Split(new char[]
			{
				','
			});
			string[] array2 = firstIdsToSelected.Split(new char[]
			{
				','
			});
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].IsChecked = false;
				list[i].IsChecked2 = false;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					string[] array3 = array[j].Split(new char[]
					{
						':'
					});
					string arg_9D_0 = array3[0];
					int num2 = 0;
					if (int.TryParse(arg_9D_0, out num2))
					{
						for (int k = 0; k < list.Count; k++)
						{
							Npc npc = list[k];
							if (npc.Id == num2)
							{
								npc.IsChecked = true;
								if (array3.Length > 1)
								{
									npc.Formation = array3[1];
								}
								else
								{
									npc.Formation = "不变阵";
								}
								list.RemoveAt(k);
								list.Insert(num, npc);
								num++;
								break;
							}
						}
					}
				}
			}
			num = 0;
			for (int l = 0; l < array2.Length; l++)
			{
				if (array2[l] != null)
				{
					string[] array4 = array2[l].Split(new char[]
					{
						':'
					});
					string arg_14F_0 = array4[0];
					int num3 = 0;
					if (int.TryParse(arg_14F_0, out num3))
					{
						for (int m = 0; m < list.Count; m++)
						{
							Npc npc2 = list[m];
							if (npc2.Id == num3)
							{
								npc2.IsChecked = true;
								npc2.IsChecked2 = true;
								if (array4.Length > 1)
								{
									npc2.Formation = array4[1];
								}
								else
								{
									npc2.Formation = "不变阵";
								}
								list.RemoveAt(m);
								list.Insert(num, npc2);
								num++;
								break;
							}
						}
					}
				}
			}
		}

		private string getDgSelectdIds(DataGridView dg, bool checkFirstBattle)
		{
			BindingSource bindingSource = dg.DataSource as BindingSource;
			if (bindingSource == null)
			{
				return "";
			}
			List<Npc> list = bindingSource.DataSource as List<Npc>;
			string text = "";
			int num = 0;
			for (int i = 0; i < dg.Rows.Count; i++)
			{
				DataGridViewRow expr_3E = dg.Rows[i];
				DataGridViewCell dataGridViewCell = expr_3E.Cells[0];
				DataGridViewCell dataGridViewCell2 = expr_3E.Cells[1];
				if (checkFirstBattle)
				{
					if ((bool)dataGridViewCell.Value && (bool)dataGridViewCell2.Value)
					{
						Npc npc = list[num];
						int id = npc.Id;
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							id,
							":",
							npc.Formation,
							","
						});
					}
				}
				else if ((bool)dataGridViewCell.Value && !(bool)dataGridViewCell2.Value)
				{
					Npc npc2 = list[num];
					int id2 = npc2.Id;
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						id2,
						":",
						npc2.Formation,
						","
					});
				}
				num++;
			}
			if (text.Length > 0)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "defense_enabled", _mainForm.chk_defense_format.Checked.ToString());
            if (_mainForm.combo_defense_formation.SelectedItem != null)
			{
                config.setConfig(this.ServerName, "defense_formation", _mainForm.combo_defense_formation.SelectedItem.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "defense_formation", "不变阵");
			}
            config.setConfig(this.ServerName, "npc_enabled", _mainForm.chk_attack_npc_enable.Checked.ToString());
            config.setConfig(this.ServerName, "npc_reserve_token", _mainForm.num_attack_npc_reserve_order.Value.ToString());
            string[] array = _mainForm.lbl_attackNpc.Text.Split(new char[]
			{
				':'
			});
			if (array.Length >= 4)
			{
				config.setConfig(this.ServerName, "npc_ids", string.Format("{0}:{1}", array[3], array[2]));
			}
            array = _mainForm.lbl_attackForceNpc.Text.Split(new char[]
			{
				':'
			});
			if (array.Length >= 4)
			{
				config.setConfig(this.ServerName, "npc_force_ids", string.Format("{0}:{1}", array[3], array[2]));
			}
            string dgSelectdIds = this.getDgSelectdIds(_mainForm.dg_attack_army, false);
            string dgSelectdIds2 = this.getDgSelectdIds(_mainForm.dg_attack_army, true);
            config.setConfig(this.ServerName, "army_enabled", _mainForm.chk_attack_army.Checked.ToString());
            config.setConfig(this.ServerName, "attack_army_only_jingyingtime", _mainForm.chk_attack_army_only_jingyingtime.Checked.ToString());
            config.setConfig(this.ServerName, "attack_first", _mainForm.chk_attack_army_first.Checked.ToString());
            config.setConfig(this.ServerName, "attack_first_interval", _mainForm.num_attack_army_first_interval.Value.ToString());
            config.setConfig(this.ServerName, "army_reserve_token", _mainForm.num_attack_army_reserve_order.Value.ToString());
			config.setConfig(this.ServerName, "army_ids", dgSelectdIds);
			config.setConfig(this.ServerName, "first_army_ids", dgSelectdIds2);
            config.setConfig(this.ServerName, "battle_event_enable", _mainForm.chk_battle_event_enabled.Checked.ToString());
			string text = "";
            if (_mainForm.chk_battle_event_gem.Checked)
			{
				text += "1";
			}
            if (_mainForm.chk_battle_event_stone.Checked)
			{
				text += "2";
			}
            if (_mainForm.chk_battle_event_weapon.Checked)
			{
				text += "3";
			}
            if (_mainForm.chk_battle_event_gem_gold.Checked)
			{
				text += "4";
			}
            if (_mainForm.chk_battle_event_stone_gold.Checked)
			{
				text += "5";
			}
            if (_mainForm.chk_battle_event_weapon_gold.Checked)
			{
				text += "6";
			}
			config.setConfig(this.ServerName, "battle_event_id", text);
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "defense_enabled", "true");
			expr_06.setConfig(this.ServerName, "defense_formation", "不变阵");
			expr_06.setConfig(this.ServerName, "npc_enabled", "true");
			expr_06.setConfig(this.ServerName, "npc_reserve_token", "0");
			expr_06.setConfig(this.ServerName, "npc_ids", "");
			expr_06.setConfig(this.ServerName, "npc_force_ids", "");
			expr_06.setConfig(this.ServerName, "army_enabled", "true");
			expr_06.setConfig(this.ServerName, "attack_army_only_jingyingtime", "true");
			expr_06.setConfig(this.ServerName, "attack_first", "false");
			expr_06.setConfig(this.ServerName, "attack_first_interval", "60");
			expr_06.setConfig(this.ServerName, "army_reserve_token", "0");
			expr_06.setConfig(this.ServerName, "army_ids", "");
			expr_06.setConfig(this.ServerName, "first_army_ids", "");
			expr_06.setConfig(this.ServerName, "battle_event_enable", "true");
			expr_06.setConfig(this.ServerName, "battle_event_id", "0");
			this.renderSettings();
		}
	}
}
