using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class StoneExe : ExeBase
	{
		public StoneExe()
		{
			this._name = "stone";
			this._readable = "玉石采集";
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			bool flag = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				int gather_percent = 50;
				bool flag2 = config.ContainsKey("ratio");
				if (flag2)
				{
					int.TryParse(config["ratio"], out gather_percent);
				}
				MiscMgr miscManager = this._factory.getMiscManager();
				miscManager.handleStoneInfo(this._proto, this._logger, gather_percent);
				result = base.next_halfhour();
			}
			return result;
		}
	}
}
