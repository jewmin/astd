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
            int advance_baoshi = 50000000;
            int advance_dianquan = 50000000;
            int advance_bintie = 50000000;
			int cishu = 0;
			if (config.ContainsKey("advancebaoshi"))
			{
				advance_baoshi = int.Parse(config["advancebaoshi"]);
			}
			if (config.ContainsKey("advancedianquan"))
			{
				advance_dianquan = int.Parse(config["advancedianquan"]);
            }
            if (config.ContainsKey("advancebintie"))
            {
                advance_bintie = int.Parse(config["advancebintie"]);
            }
			bool b_usefree = config.ContainsKey("superusefree") && config["superusefree"].ToLower().Equals("true");
			if (config.ContainsKey("cishu"))
			{
				cishu = int.Parse(config["cishu"]);
			}
			if (config.ContainsKey("baishenenable") && config["baishenenable"].ToLower().Equals("true"))
			{
                this._factory.getMiscManager().baiShen(this._proto, this._logger, this._user, base.getGoldAvailable(), advance_baoshi, advance_dianquan, advance_bintie, b_usefree, cishu);
			}
			return base.next_day();
		}
	}
}
