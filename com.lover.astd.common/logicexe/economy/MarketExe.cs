using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class MarketExe : ExeBase
	{
		public MarketExe()
		{
			this._name = "market";
			this._readable = "集市";
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
				bool notbuy_if_fail = config.ContainsKey("notbuy_iffail") && config["notbuy_iffail"].ToLower().Equals("true");
				bool drop_notbuy = config.ContainsKey("drop_notbuy") && config["drop_notbuy"].ToLower().Equals("true");
				bool buy_super = config.ContainsKey("super_supply") && config["super_supply"].ToLower().Equals("true");
				bool use_token = config.ContainsKey("use_token") && config["use_token"].ToLower().Equals("true");
				string configstr = "";
				bool flag2 = config.ContainsKey("items");
				if (flag2)
				{
					configstr = config["items"];
				}
				MiscMgr miscManager = this._factory.getMiscManager();
				int silver = this._user.Silver;
				int goldAvailable = base.getGoldAvailable();
				int num = miscManager.handleMarketInfo(this._proto, this._logger, configstr, notbuy_if_fail, drop_notbuy, buy_super, silver, goldAvailable, use_token);
				bool flag3 = num == 0;
				if (flag3)
				{
					result = base.immediate();
				}
				else
				{
					bool flag4 = num == 1 && this._isRunningServer;
					if (flag4)
					{
						result = 60000L;
					}
					else
					{
						bool flag5 = num == 2;
						if (flag5)
						{
							result = base.next_hour();
						}
						else
						{
							result = base.next_hour();
						}
					}
				}
			}
			return result;
		}
	}
}
