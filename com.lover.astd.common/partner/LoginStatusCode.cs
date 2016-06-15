using System;

namespace com.lover.astd.common.partner
{
	public enum LoginStatusCode
	{
		Success,
		NeedVerifyCode,
		FailInGetToken,
		FailInLogin,
		FailInGetServerList,
		FailInFindingGameUrl,
		FailInGotoGameUrl,
		FailInGetSession
	}
}
