using com.lover.astd.common.model;
using com.lover.astd.common.partner;
using System;
using System.ComponentModel;

namespace com.lover.astd.common
{
	public class CommonReceiver : ILoginReceiver
	{
		private AccountData _acc;

		private BackgroundWorker _worker;

		public CommonReceiver(AccountData dt, BackgroundWorker worker)
		{
			this._acc = dt;
			this._worker = worker;
		}

		public void setLoginStatus(string status)
		{
			this._worker.ReportProgress(0, status);
		}

		public AccountData getAccount()
		{
			return this._acc;
		}
	}
}
