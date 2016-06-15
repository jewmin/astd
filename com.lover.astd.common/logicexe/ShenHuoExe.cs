using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe
{
	public class ShenHuoExe : ExeBase
	{
		public ShenHuoExe()
		{
			this._name = "shenhuo";
			this._readable = "百炼精铁";
		}

		public override void init_data()
		{
			this.refreshUi();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			bool flag = config.ContainsKey("shenhuoenable") && config["shenhuoenable"].ToLower().Equals("true");
			bool flag2 = flag;
			if (flag2)
			{
				this._factory.getMiscManager().ShenHuo(this._proto, this._logger, this._user, base.getGoldAvailable());
			}
			return base.next_day();
		}
	}
}
