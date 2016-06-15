using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.lover.astd.common.config
{
    public class OtherConfig
    {
        private const string ConfFileName = "other.ini";

		private const string LineSpliter = "\r\n";

		private string _dirname = "";

		private string _filename = "";

		private string _confString = "";

		private bool _inited;

		private Dictionary<string, Dictionary<string, string>> _configAll = new Dictionary<string, Dictionary<string, string>>();

		private List<string> _black_list = new List<string>();

		public bool Inited
		{
			get
			{
				return _inited;
			}
		}

		public List<string> BlackList
		{
			get
			{
				return _black_list;
			}
			set
			{
				_black_list = value;
			}
		}

		public static OtherConfig generateConfig(string confString)
		{
			return new OtherConfig(confString);
		}

		public Dictionary<string, Dictionary<string, string>> getAllConfig()
		{
			return _configAll;
		}

        public Dictionary<string, string> getConfig(string configName)
        {
            if (_configAll.ContainsKey(configName))
            {
                return _configAll[configName];
            }
            else
            {
                Dictionary<string, string> subConf = new Dictionary<string, string>();
                _configAll.Add(configName, subConf);
                return subConf;
            }
        }

		public void setConfig(string configName, Dictionary<string, string> conf)
		{
			foreach (string current in conf.Keys)
			{
				setConfig(configName, current, conf[current]);
			}
		}

		public void setConfig(string mainConfigName, string subConfigName, string conf)
		{
			if (!_configAll.ContainsKey(mainConfigName))
			{
				Dictionary<string, string> subConf = new Dictionary<string, string>();
				subConf.Add(subConfigName, conf);
				_configAll.Add(mainConfigName, subConf);
			}
			else
			{
				Dictionary<string, string> subConf = _configAll[mainConfigName];
				if (subConf.ContainsKey(subConfigName))
				{
					subConf[subConfigName] = conf;
				}
				else
				{
					subConf.Add(subConfigName, conf);
				}
			}
		}

        private OtherConfig(string confString)
        {
            _confString = confString;
            _inited = true;
            string[] separator = new string[] { "=" };
            string[] confs = confString.Split(new string[] { LineSpliter }, StringSplitOptions.RemoveEmptyEntries);
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
                            handleConfigPair(pairs[0], pairs[1]);
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
            this._filename = string.Format("{0}/{1}", _dirname, ConfFileName);
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
			return _confString;
		}

		public OtherConfig(string partner, int serverid, string rolename)
		{
			string dirName = "accounts";
            this._dirname = string.Format("{0}/{1}_{2}_{3}", dirName, partner, serverid, rolename);
			this._filename = string.Format("{0}/{1}", this._dirname, ConfFileName);
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
			_configAll.Clear();
		}

		public bool loadSettings()
		{
			FileInfo fileInfo = new FileInfo(_filename);
			if (!fileInfo.Exists)
			{
				return false;
			}
			else
			{
				_inited = true;
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
                                handleConfigPair(pairs[0], pairs[1]);
                            }
						}
						sb.Append(text + LineSpliter);
					}
				}
				reader.Close();
				_confString = sb.ToString();
				loadBlackList();
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
            setConfig(mainConfigName, subConfigName, value);
        }

		public void saveSettings()
		{
			FileInfo fileInfo = new FileInfo(_filename);
			FileStream fileStream = null;
			if (!fileInfo.Exists)
			{
				fileStream = fileInfo.Create();
			}
			else
			{
				fileStream = fileInfo.Open(FileMode.Truncate, FileAccess.Write);
			}
			_inited = true;
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("# astd other ini file, please donnot change this file by yourself");
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

		public void addBlackList(string name)
		{
			if (!_black_list.Contains(name))
			{
				_black_list.Add(name);
				StringBuilder sb = new StringBuilder();
	            foreach (string item in _black_list)
	            {
	                if (sb.Length > 0)
	                {
	                    sb.Append(";");
	                }
	                sb.Append(item);
	            }
	            setConfig(ConfigStrings.S_Attack, ConfigStrings.black_list, sb.ToString());
	            saveSettings();
			}
		}

		private void loadBlackList()
		{
            _black_list.Clear();
            Dictionary<string, string> config = getConfig(ConfigStrings.S_Attack);
            if (config.ContainsKey(ConfigStrings.black_list))
            {
                string[] list = config[ConfigStrings.black_list].Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
                if (list != null && list.Length > 0)
                {
                    foreach (string item in list)
                    {
                        _black_list.Add(item);
                    }
                }
            }
		}
    }
}
