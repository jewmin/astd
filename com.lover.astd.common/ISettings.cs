using com.lover.astd.common.config;
using com.lover.astd.common.model;
using System;

namespace com.lover.astd.common
{
	public interface ISettings
	{
		void setName(string name);

		string getName();

		void setVariables(User u, GameConfig conf);

		void init();

		void saveSettings();

		void renderSettings();

		void loadDefaultSettings();
	}
}
