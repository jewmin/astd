using System;

namespace com.lover.astd.common.logicexe
{
	public class PingExe : ExeBase
	{
		public PingExe()
		{
			this._name = "ping";
			this._readable = "会话保持";
		}

		public override void init_data()
		{
		}

		public override long execute()
		{
			this._factory.getMiscManager().getServerTime(this._proto, this._logger);
			this._factory.getMiscManager().getExtraInfo2(this._proto, this._logger, this._user);
			return 120000L;
		}
	}
}
