using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.lover.astd.common.config
{
	public class GameConfig
	{
		private const string ConfFileName = "astool.ini";

		private const string LineSpliter = "\r\n";

		private string _dirname = "";

		private string _filename = "";

		private string _confString = "";

		private bool _inited;

        private List<TreasureConfig> _treasureConfig = new List<TreasureConfig>();

		private Dictionary<string, Dictionary<string, string>> _configAll = new Dictionary<string, Dictionary<string, string>>();

		public bool Inited
		{
			get
			{
				return this._inited;
			}
		}

		public static GameConfig generateConfig(string confString)
		{
			return new GameConfig(confString);
		}

		public Dictionary<string, Dictionary<string, string>> getAllConfig()
		{
			return this._configAll;
		}

        public Dictionary<string, string> getConfig(string configName)
        {
            if (this._configAll.ContainsKey(configName))
            {
                return this._configAll[configName];
            }
            else
            {
                Dictionary<string, string> subConf = new Dictionary<string, string>();
                this._configAll.Add(configName, subConf);
                return subConf;
            }
        }

		public void setConfig(string configName, Dictionary<string, string> conf)
		{
			foreach (string current in conf.Keys)
			{
				this.setConfig(configName, current, conf[current]);
			}
		}

		public void setConfig(string mainConfigName, string subConfigName, string conf)
		{
			if (!this._configAll.ContainsKey(mainConfigName))
			{
				Dictionary<string, string> subConf = new Dictionary<string, string>();
				subConf.Add(subConfigName, conf);
				this._configAll.Add(mainConfigName, subConf);
			}
			else
			{
				Dictionary<string, string> subConf = this._configAll[mainConfigName];
				if (subConf.ContainsKey(subConfigName))
				{
					subConf[subConfigName] = conf;
				}
				else
				{
					subConf.Add(subConfigName, conf);
				}
			}
            if (mainConfigName.Equals("treasure_huangyuan") && subConfigName.Equals("use_dice"))
            {
                setTreasureConfig(conf);
            }
		}

        private GameConfig(string confString)
        {
            this._confString = confString;
            this._inited = true;
            string[] separator = new string[] { "=" };
            string[] confs = confString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < confs.Length; i++)
            {
                string conf = confs[i].Trim();
                if (conf.Length > 0 && !conf.StartsWith("#"))
                {
                    string[] pairs = conf.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (pairs.Length == 2)
                    {
                        if (pairs[0] != null && pairs[1] != null && pairs[0].Length > 0 && pairs[1].Length > 0)
                        {
                            this.handleConfigPair(pairs[0], pairs[1]);
                        }
                    }
                }
            }
        }

        public void setRoleName(string partner, int serverid, string rolename, string _userDirName = "")
        {
            string dirName = "accounts";
            if (_userDirName != "")
            {
                dirName = dirName + "/" + _userDirName;
            }
            this._dirname = string.Format("{0}/{1}_{2}_{3}", dirName, partner, serverid, rolename);
            this._filename = string.Format("{0}/{1}", this._dirname, "astool.ini");
            DirectoryInfo directoryInfo = new DirectoryInfo(this._dirname);
            if (!directoryInfo.Exists)
            {
                if (Directory.Exists(rolename))
                {
                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }
                    Directory.Move(rolename, this._dirname);
                }
                string oldDirName = string.Format("{0}/{1}", dirName, rolename);
                if (Directory.Exists(oldDirName))
                {
                    Directory.Move(oldDirName, this._dirname);
                }
                if (!Directory.Exists(this._dirname))
                {
                    directoryInfo.Create();
                }
            }
        }

		public string getRawString()
		{
			return this._confString;
		}

		public GameConfig(string partner, int serverid, string rolename)
		{
			string dirName = "accounts";
            this._dirname = string.Format("{0}/{1}_{2}_{3}", dirName, partner, serverid, rolename);
			this._filename = string.Format("{0}/{1}", this._dirname, "astool.ini");
			DirectoryInfo directoryInfo = new DirectoryInfo(this._dirname);
			if (!directoryInfo.Exists)
			{
				if (Directory.Exists(rolename))
				{
					if (!Directory.Exists(dirName))
					{
						Directory.CreateDirectory(dirName);
					}
					Directory.Move(rolename, this._dirname);
				}
                string oldDirName = string.Format("{0}/{1}", dirName, rolename);
				if (Directory.Exists(oldDirName))
				{
					Directory.Move(oldDirName, this._dirname);
				}
				if (!Directory.Exists(this._dirname))
				{
					directoryInfo.Create();
				}
			}
		}

		public void clearSettingDict()
		{
			this._configAll.Clear();
		}

		public bool loadSettings()
		{
			FileInfo fileInfo = new FileInfo(this._filename);
			if (!fileInfo.Exists)
			{
				return false;
			}
			else
			{
				this._inited = true;
				StringBuilder sb = new StringBuilder();
				StreamReader reader = fileInfo.OpenText();
                string[] separator = new string[] { "=" };
				for (string text = reader.ReadLine(); text != null; text = reader.ReadLine())
				{
					text = text.Trim();
					if (text.Length > 0 && !text.StartsWith("#"))
					{
						string[] pairs = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (pairs.Length == 2)
						{
                            if (pairs[0] != null && pairs[1] != null && pairs[0].Length > 0 && pairs[1].Length > 0)
                            {
                                this.handleConfigPair(pairs[0], pairs[1]);
                            }
						}
						sb.Append(text + "\r\n");
					}
				}
				reader.Close();
				this._confString = sb.ToString();
				return true;
			}
		}

        private void handleConfigPair(string key, string value)
        {
            string[] configName = key.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (configName.Length < 2)
            {
                return;
            }
            string mainConfigName = configName[0].Trim();
            string subConfigName = configName[1].Trim();
            this.setConfig(mainConfigName, subConfigName, value);
        }

		public void saveSettings()
		{
			FileInfo fileInfo = new FileInfo(this._filename);
			FileStream fileStream = null;
			if (!fileInfo.Exists)
			{
				fileStream = fileInfo.Create();
			}
			else
			{
				fileStream = fileInfo.Open(FileMode.Truncate, FileAccess.Write);
			}
			this._inited = true;
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("# astd tools ini file, please donnot change this file by yourself");
			foreach (string current in this._configAll.Keys)
			{
				streamWriter.WriteLine("# " + current);
				Dictionary<string, string> dictionary = this._configAll[current];
				foreach (string current2 in dictionary.Keys)
				{
					string value = string.Format("{0:s}.{1:s}={2:s}", current, current2, dictionary[current2]);
					streamWriter.WriteLine(value);
				}
				streamWriter.WriteLine();
			}
			streamWriter.Close();
			fileStream.Close();
		}

        private void setTreasureConfig(string conf)
        {
            _treasureConfig.Clear();
            string[] separator = new string[] { ":" };
            string[] dice_type = conf.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string current in dice_type)
            {
                string[] pairs = current.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length == 2)
                {
                    if (pairs[0] != null && pairs[1] != null && pairs[0].Length > 0 && pairs[1].Length > 0)
                    {
                        int pos = 0, dice = 0;
                        int.TryParse(pairs[0], out pos);
                        int.TryParse(pairs[1], out dice);
                        TreasureConfig treasureConfig = new TreasureConfig(pos, dice);
                        _treasureConfig.Add(treasureConfig);
                    }
                }
            }
        }
	}
}
