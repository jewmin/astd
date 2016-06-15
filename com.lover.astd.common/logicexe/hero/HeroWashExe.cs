using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.hero
{
	public class HeroWashExe : ExeBase
	{
		public HeroWashExe()
		{
			this._name = "wash";
			this._readable = "武将洗属性";
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
				bool flag2 = config.ContainsKey("wash_hero") && config["wash_hero"] != "";
				if (flag2)
				{
					int num = 0;
					bool flag3 = int.TryParse(config["wash_hero"], out num);
					if (flag3)
					{
						string text = config["wash_attrib"];
						bool flag4 = text.Length < 3;
						if (flag4)
						{
							int num2;
							for (int i = 0; i < 3 - text.Length; i = num2 + 1)
							{
								text += "0";
								num2 = i;
							}
						}
						HeroMgr heroManager = this._factory.getHeroManager();
						bool flag5 = num == 0 || text == "";
						if (flag5)
						{
							this.logInfo("没有可洗的武将");
							result = base.next_hour();
							return result;
						}
						int[] array = new int[3];
						int[] array2 = array;
						int num3 = heroManager.startWash(this._proto, this._logger, this._user, num, text, true, base.getCreditAvailable(), 20, ref array2);
						bool flag6 = num3 == 1;
						if (flag6)
						{
							this.logInfo("武将已经洗属性完毕, 将在1小时候再次检查");
							result = 3600000L;
							return result;
						}
						bool flag7 = num3 == 2;
						if (flag7)
						{
							this.logInfo("洗将出错, 半小时后继续");
							result = 1800000L;
							return result;
						}
						bool flag8 = num3 == 3;
						if (flag8)
						{
							this.logInfo("条件不足, 今天不再洗属性");
							result = base.next_day();
							return result;
						}
						this.refreshUi();
						result = base.next_day();
						return result;
					}
				}
				result = base.immediate();
			}
			return result;
		}
	}
}
