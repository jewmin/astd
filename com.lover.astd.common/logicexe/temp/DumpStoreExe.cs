using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.temp
{
	public class DumpStoreExe : TempExeBase, ITempExe, IExecute
	{
		private bool _finished;

		public DumpStoreExe()
		{
			this._name = "temp";
			this._readable = "临时任务";
		}

		public override long execute()
		{
			base.notifySingleExe("store");
			base.notifySingleExe("weapon");
			this._finished = true;
			return base.immediate();
		}

		public bool isFinished()
		{
			return this._finished;
		}

		public void setTarget(Dictionary<string, string> conf)
		{
			bool flag = conf != null && conf.ContainsKey("exchange_id") && conf.ContainsKey("exchange_id");
			if (flag)
			{
				conf.ContainsKey("exchange_name");
			}
		}

		public string getStatus()
		{
			return "进行中...";
		}
	}
}
