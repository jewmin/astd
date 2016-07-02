using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.lover.astd.common.config
{
	public class GlobalConfig
	{
		private static GlobalConfig _instance;

		public static bool isManager;

		public static string HomeUrl;

		public static string Version;

		private string _confFilePath = "account.ini";

		private List<AccountData> _accountAll = new List<AccountData>();

		public static bool isDebug;

		private AccountData _lastLoginAccount;

		public static GlobalConfig getInstance()
		{
			if (GlobalConfig._instance == null)
			{
				GlobalConfig._instance = new GlobalConfig();
			}
			return GlobalConfig._instance;
		}

		public static int getGemCount(int gem_level)
		{
			return (int)Math.Pow(2.0, (double)(gem_level - 1));
		}

		static GlobalConfig()
		{
			GlobalConfig.isManager = false;
			GlobalConfig.HomeUrl = "http://bbs.aoshitang.com";
			GlobalConfig.Version = "傲视天地小助手 lua版";
			GlobalConfig.isDebug = false;
			if (GlobalConfig.isManager)
			{
                GlobalConfig.HomeUrl = "http://bbs.aoshitang.com";
                GlobalConfig.Version = "傲视天地小助手 lua版";
			}
		}

		public void addAccount(AccountData acc)
		{
			bool flag = acc.Server_type == ServerType.Custom;
			if (flag)
			{
				int num;
				for (int i = 0; i < this._accountAll.Count; i = num + 1)
				{
					bool flag2 = this._accountAll[i].Server_type == ServerType.Custom;
					if (flag2)
					{
						this._accountAll.RemoveAt(i);
						num = i;
						i = num - 1;
					}
					num = i;
				}
			}
			this._accountAll.Add(acc);
			this.setLastLoginAccount(acc);
		}

		public void removeAccount(AccountData acc)
		{
			this._accountAll.Remove(acc);
			this.saveSettings();
		}

		public List<AccountData> getAllAccounts()
		{
			return this._accountAll;
		}

		public void setLastLoginAccount(AccountData acc)
		{
			foreach (AccountData current in this._accountAll)
			{
				bool flag = acc.Server_type == ServerType.Custom;
				if (flag)
				{
					bool flag2 = current.Server_type == ServerType.Custom;
					if (flag2)
					{
						current.IsLastLogin = true;
					}
					else
					{
						current.IsLastLogin = false;
					}
				}
				else
				{
					bool flag3 = current.Equals(acc);
					if (flag3)
					{
						current.IsLastLogin = true;
					}
					else
					{
						current.IsLastLogin = false;
					}
				}
			}
			this._lastLoginAccount = acc;
			this.saveSettings();
		}

		public AccountData getLastLoginAccount()
		{
			bool flag = this._lastLoginAccount != null;
			AccountData result;
			if (flag)
			{
				result = this._lastLoginAccount;
			}
			else
			{
				bool flag2 = this._accountAll.Count == 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					this._lastLoginAccount = this._accountAll[0];
					result = this._lastLoginAccount;
				}
			}
			return result;
		}

		public Dictionary<ServerType, List<AccountData>> getAllAccountByType()
		{
			Dictionary<ServerType, List<AccountData>> dictionary = new Dictionary<ServerType, List<AccountData>>();
			foreach (ServerType serverType in Enum.GetValues(typeof(ServerType)))
			{
				List<AccountData> list = new List<AccountData>();
				foreach (AccountData current in this._accountAll)
				{
					bool flag = current.Server_type == serverType;
					if (flag)
					{
						list.Add(current);
					}
				}
				dictionary.Add(serverType, list);
			}
			return dictionary;
		}

		public void delAccount(AccountData acc)
		{
			bool flag = false;
			int i = 0;
			int count = this._accountAll.Count;
			while (i < count)
			{
				AccountData obj = this._accountAll[i];
				bool flag2 = acc.Equals(obj);
				if (flag2)
				{
					this._accountAll.RemoveAt(i);
					flag = true;
					break;
				}
				int num = i;
				i = num + 1;
			}
			bool flag3 = flag;
			if (flag3)
			{
				this.saveSettings();
			}
		}

		public bool loadSettings()
		{
			FileInfo fileInfo = new FileInfo(this._confFilePath);
			bool result;
			if (!fileInfo.Exists)
			{
				result = false;
			}
			else
			{
				this._accountAll.Clear();
				StreamReader streamReader = fileInfo.OpenText();
				string text = streamReader.ReadLine();
				while (text != null)
				{
					text = text.Trim();
					if (text.Length > 0 && !text.StartsWith("#"))
					{
						int num = text.IndexOf('=');
						if (num >= 0)
						{
							string text2 = text.Substring(0, num);
							string text3 = text.Substring(num + 1);
							if (text2 != null && text3 != null && text2.Length > 0 && text3.Length > 0)
							{
								this.handleConfigPair(text2, text3);
							}
						}
					}
					text = streamReader.ReadLine();
				}
				streamReader.Close();
				result = true;
			}
			return result;
		}

		private void handleConfigPair(string key, string value)
		{
			bool flag = !key.ToLower().Equals("account");
			if (!flag)
			{
				string[] array = value.Split(new char[]
				{
					':'
				});
				bool flag2 = array.Length < 7;
				if (!flag2)
				{
					string serverType = array[0];
					string s = array[1];
					string userName = array[2];
					string s2 = array[3];
					bool isLastLogin = array[4] == "1";
					string text = array[5];
					string text2 = array[6];
					bool flag3 = text.Length > 0;
					if (flag3)
					{
						text = text.Replace('*', ':');
					}
					bool flag4 = text2.Length > 0;
					if (flag4)
					{
						text2 = text2.Replace('*', ':');
					}
					int serverId = 0;
					int.TryParse(s, out serverId);
					string roleName = "";
					bool flag5 = array.Length >= 8;
					if (flag5)
					{
						roleName = array[7];
					}
					AccountData accountData = new AccountData();
					accountData.setServerType(serverType);
					accountData.ServerId = serverId;
					accountData.UserName = userName;
					accountData.Password = Encoding.UTF8.GetString(Convert.FromBase64String(s2));
					accountData.RoleName = roleName;
					accountData.CustomLoginUrl = text;
					accountData.CustomGameUrl = text2;
					accountData.IsLastLogin = isLastLogin;
					bool isLastLogin2 = accountData.IsLastLogin;
					if (isLastLogin2)
					{
						this._lastLoginAccount = accountData;
					}
					this._accountAll.Add(accountData);
				}
			}
		}

		public void saveSettings()
		{
			FileInfo fileInfo = new FileInfo(this._confFilePath);
			FileStream fileStream = null;
			bool flag = !fileInfo.Exists;
			if (flag)
			{
				fileStream = fileInfo.Create();
			}
			else
			{
				fileStream = fileInfo.Open(FileMode.Truncate, FileAccess.Write);
			}
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("# astd account ini file, please donnot change this file by yourself");
			foreach (AccountData current in this._accountAll)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(current.Password);
				string text = Convert.ToBase64String(bytes);
				string value = string.Format("account={0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", new object[]
				{
					current.Server_type.ToString(),
					current.ServerId,
					current.UserName,
					text,
					current.IsLastLogin ? 1 : 0,
					current.CustomLoginUrl.Replace(':', '*'),
					current.CustomGameUrl.Replace(':', '*'),
					current.RoleName
				});
				streamWriter.WriteLine(value);
			}
			streamWriter.Close();
			fileStream.Close();
		}
	}
}
