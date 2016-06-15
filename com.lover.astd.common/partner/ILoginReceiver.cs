using com.lover.astd.common.model;
using System;

namespace com.lover.astd.common.partner
{
	public interface ILoginReceiver
	{
		void setLoginStatus(string status);

		AccountData getAccount();
	}
}
