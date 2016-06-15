using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.battle
{
	public class ResCampaignExe : ExeBase
	{
		public ResCampaignExe()
		{
			this._name = "res_campaign";
			this._readable = "资源副本";
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			bool flag = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
			long result;
			if (flag)
			{
				result = base.an_hour_later();
			}
			else
			{
				string text = "";
				bool flag2 = config.ContainsKey("res_campaign") && config["res_campaign"] != "";
				if (flag2)
				{
					text = config["res_campaign"];
				}
				bool flag3 = text == "";
				if (flag3)
				{
					result = base.next_hour();
				}
				else
				{
					string[] array = text.Split(new char[]
					{
						':'
					});
					bool flag4 = array.Length < 2;
					if (flag4)
					{
						result = base.next_hour();
					}
					else
					{
						int num = 0;
						int.TryParse(array[0], out num);
						bool flag5 = num == 0;
						if (flag5)
						{
							result = base.next_hour();
						}
						else
						{
							int num2 = this._factory.getBattleManager().handleResCampaign(this._proto, this._logger, num);
							bool flag6 = num2 == 5;
							if (flag6)
							{
								result = base.immediate();
							}
							else
							{
								result = base.next_hour();
							}
						}
					}
				}
			}
			return result;
		}
	}
}
