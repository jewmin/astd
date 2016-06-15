using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe
{
	public class BaiShenExe : ExeBase
	{
		public BaiShenExe()
		{
			this._name = "baishen";
			this._readable = "拜神";
		}

		public override void init_data()
		{
			this.refreshUi();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			int advance_baoshi = 3000;
			int advance_dianquan = 60000;
			int cishu = 0;
			bool flag = config.ContainsKey("baishenenable") && config["baishenenable"].ToLower().Equals("true");
			bool flag2 = config.ContainsKey("advancebaoshi");
			if (flag2)
			{
				advance_baoshi = int.Parse(config["advancebaoshi"]);
			}
			bool flag3 = config.ContainsKey("advancedianquan");
			if (flag3)
			{
				advance_dianquan = int.Parse(config["advancedianquan"]);
			}
			bool b_usefree = config.ContainsKey("superusefree") && config["superusefree"].ToLower().Equals("true");
			bool flag4 = config.ContainsKey("cishu");
			if (flag4)
			{
				cishu = int.Parse(config["cishu"]);
			}
			bool flag5 = flag;
			if (flag5)
			{
				this._factory.getMiscManager().baiShen(this._proto, this._logger, this._user, base.getGoldAvailable(), advance_baoshi, advance_dianquan, b_usefree, cishu);
			}
			return base.next_day();
		}
	}
}
