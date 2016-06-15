using com.lover.astd.common.config;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.manager
{
	public class SettingsMgr
	{
		private GameConfig _conf;

		private User _user;

		private List<ISettings> _settings = new List<ISettings>();

		public SettingsMgr(GameConfig conf, User u)
		{
			this._conf = conf;
			this._user = u;
		}

		private ISettings findSetting(string setting_name)
		{
            for (int i = 0; i < this._settings.Count; i++)
            {
                if (this._settings[i].getName().Equals(setting_name))
                {
                    return this._settings[i];
                }
            }
            return null;
		}

		public void addSetting(ISettings setting)
		{
            if (setting == null || setting.getName() == null)
            {
                return;
            }
            if (this.findSetting(setting.getName()) != null)
            {
                return;
            }
            this._settings.Add(setting);
		}

		public void renderSettings()
		{
			string setting_name = this._user.popUiFromQueue();
			while (setting_name != null && setting_name != "")
			{
				ISettings settings = this.findSetting(setting_name);
				if (settings != null)
				{
					settings.renderSettings();
				}
				setting_name = this._user.popUiFromQueue();
			}
		}

		public void init()
		{
            for (int i = 0; i < this._settings.Count; i++)
            {
                ISettings settings = this._settings[i];
                if (settings != null)
                {
                    settings.renderSettings();
                }
            }
		}

        public void saveSettings()
        {
            for (int i = 0; i < this._settings.Count; i++)
            {
                ISettings settings = this._settings[i];
                if (settings != null)
                {
                    settings.saveSettings();
                }
            }
        }

        public void loadDefaultSettings()
        {
            for (int i = 0; i < this._settings.Count; i++)
            {
                ISettings settings = this._settings[i];
                if (settings != null)
                {
                    settings.loadDefaultSettings();
                }
            }
        }
	}
}
