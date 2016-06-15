using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.equip
{
	public class MerchantExe : ExeBase
	{
		public MerchantExe()
		{
			this._name = "merchant";
			this._readable = "委派";
		}

		public override long execute()
		{
			bool flag = this._user.Level < 30;
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				bool flag2 = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
				if (flag2)
				{
					result = base.an_hour_later();
				}
				else
				{
					bool only_free = config.ContainsKey("only_free") && config["only_free"].ToLower().Equals("true");
					string merchant_type = "马";
					string sell_quality = "红色";
					bool flag3 = config.ContainsKey("type");
					if (flag3)
					{
						merchant_type = config["type"];
					}
					bool flag4 = config.ContainsKey("sell_quality");
					if (flag4)
					{
						sell_quality = config["sell_quality"];
					}
					int silverAvailable = base.getSilverAvailable();
					int num = this._factory.getEquipManager().handleMerchants(this._proto, this._logger, silverAvailable, only_free, merchant_type, sell_quality);
					bool flag5 = num == 3;
					if (flag5)
					{
						result = base.next_day();
					}
					else
					{
						bool flag6 = num == 0;
						if (flag6)
						{
							result = base.immediate();
						}
						else
						{
							result = base.an_hour_later();
						}
					}
				}
			}
			return result;
		}
	}
}
