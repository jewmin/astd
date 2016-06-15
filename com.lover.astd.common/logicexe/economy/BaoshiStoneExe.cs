using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class BaoshiStoneExe : ExeBase
	{
		public BaoshiStoneExe()
		{
			this._name = ConfigStrings.S_Baoshi_Stone;
			this._readable = ConfigStrings.SR_Baoshi_Stone;
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			if (!config.ContainsKey(ConfigStrings.enabled) || !config[ConfigStrings.enabled].ToLower().Equals("true"))
			{
				return base.next_day();
			}
			else
			{
				int gather_percent = 50;
				if (config.ContainsKey(ConfigStrings.ratio))
				{
					int.TryParse(config[ConfigStrings.ratio], out gather_percent);
				}
				MiscMgr miscManager = this._factory.getMiscManager();
				miscManager.handleBaoshiStoneInfo(this._proto, this._logger, gather_percent);
				return base.next_halfhour();
			}
		}
	}
}
