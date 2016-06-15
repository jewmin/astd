using com.lover.astd.common;
using com.lover.astd.common.config;
using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.lover.astd.game.ui.server
{
	public abstract class LogicServer : ISettings
	{
		protected ProtocolMgr _proto;

		protected NewMainForm _mainForm;

		public string ServerName = "";

		public string ServerReadableName = "";

		public virtual void release()
		{
			this._mainForm = null;
		}

		protected GameConfig getConfig()
		{
			return this._mainForm.getConfig();
		}

		protected Dictionary<string, string> getConfig(string confName)
		{
			return this._mainForm.getConfig().getConfig(confName);
		}

		protected User getUser()
		{
			return this._mainForm.GameUser;
		}

		protected double getGemPrice()
		{
			Dictionary<string, string> config = this.getConfig("global");
			double num = 20.0;
			if (config.ContainsKey("gem_price"))
			{
				double.TryParse(config["gem_price"], out num);
			}
			if (num > 0.0)
			{
				num = 10.0 / num;
			}
			else
			{
				num = 0.5;
			}
			return num;
		}

		protected List<int> generateIds(string id_string)
		{
			List<int> list = new List<int>();
			if (id_string == null || id_string == "")
			{
				return list;
			}
			string[] array = id_string.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text != null && !(text == ""))
				{
					int num = 0;
					int.TryParse(text, out num);
					if (num != 0)
					{
						list.Add(num);
					}
				}
			}
			return list;
		}

		protected string getDataGridSelectedIds<T>(DataGridView dg)
		{
			if (dg == null)
			{
				return "";
			}
			BindingSource bindingSource = dg.DataSource as BindingSource;
			if (bindingSource == null)
			{
				return "";
			}
			List<T> list = bindingSource.DataSource as List<T>;
			if (list == null)
			{
				return "";
			}
			string text = "";
			int num = 0;
			int i = 0;
			int count = dg.Rows.Count;
			while (i < count)
			{
				if ((bool)dg.Rows[i].Cells[0].Value)
				{
					int id = (list[num] as AsObject).Id;
					text = text + id + ",";
				}
				num++;
				i++;
			}
			if (text.Length > 0)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		protected void renderDataGridToSelected<T>(DataGridView dg, string idsToSelected)
		{
			if (dg == null)
			{
				return;
			}
			BindingSource bindingSource = dg.DataSource as BindingSource;
			if (bindingSource == null)
			{
				return;
			}
			List<T> list = bindingSource.DataSource as List<T>;
			if (list == null)
			{
				return;
			}
			string[] array = idsToSelected.Split(new char[]
			{
				','
			});
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				(list[i] as AsObject).IsChecked = false;
			}
			for (int j = 0; j < array.Length; j++)
			{
				string arg_73_0 = array[j];
				int num2 = 0;
				if (int.TryParse(arg_73_0, out num2))
				{
					for (int k = 0; k < list.Count; k++)
					{
						T t = list[k];
						AsObject asObject = t as AsObject;
						if (asObject != null && asObject.Id == num2)
						{
							asObject.IsChecked = true;
							list.RemoveAt(k);
							list.Insert(num, t);
							num++;
							break;
						}
					}
				}
			}
		}

		public void setName(string name)
		{
			this.ServerName = name;
		}

		public string getName()
		{
			return this.ServerName;
		}

		public void setVariables(User u, GameConfig conf)
		{
			throw new NotImplementedException();
		}

		public void init()
		{
			throw new NotImplementedException();
		}

		public virtual void saveSettings()
		{
		}

		public virtual void renderSettings()
		{
		}

		public virtual void loadDefaultSettings()
		{
		}
	}
}
