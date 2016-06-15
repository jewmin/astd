using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe
{
	public class QiFuExe : ExeBase
	{
		public QiFuExe()
		{
			this._name = "qifu";
			this._readable = "祈福活动";
		}

		public override void init_data()
		{
			this.refreshUi();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			if (config.ContainsKey("qifuenable") && config["qifuenable"].ToLower().Equals("true"))
			{
                int result = this._factory.getMiscManager().newQiFu(this._proto, this._logger, this._user, base.getGoldAvailable(), int.Parse(config["qifucoin"]), int.Parse(config["qifudq"]));
                if (result == 0)
                {
                    return base.next_day();
                }
                else if (result == 1)
                {
                    return 10000L;
                }
                else if (result == 2)
                {
                    return 1000L;
                }
                else if (result == 3)
                {
                    return 3000L;
                }
                else if (result == 10)
                {
                    return base.next_day();
                }
                else
                {
                    return 3000L;
                }
			}
			return base.next_halfhour();
		}
	}
}
