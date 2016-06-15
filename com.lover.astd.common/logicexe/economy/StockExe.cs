using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class StockExe : ExeBase
	{
		public StockExe()
		{
			this._name = "stock";
			this._readable = "市场";
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
				bool flag2 = !config.ContainsKey("stock_id");
				if (flag2)
				{
					result = base.next_day();
				}
				else
				{
					string configstr = config["stock_id"];
					MiscMgr miscManager = this._factory.getMiscManager();
					int num = miscManager.handleStockInfo(this._proto, this._logger, configstr, this._user);
					bool flag3 = num == 2;
					if (flag3)
					{
						result = base.next_hour();
					}
					else
					{
						bool flag4 = num == 3;
						if (flag4)
						{
							result = (long)this._user._stock_cd;
						}
						else
						{
							result = base.next_halfhour();
						}
					}
				}
			}
			return result;
		}
	}
}
