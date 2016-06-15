using com.lover.astd.common.manager;
using com.lover.common;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl.sf
{
	public class SfLogin : LoginImplBase
	{
		public SfLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult result = new LoginResult();
			base.logging();
			TimeMgr timeMgr = new TimeMgr();
			string url = string.Format("http://{0}/root/login.action?{1}", verify_code, timeMgr.TimeStamp);
			string data = string.Format("password={1}&yx=&userName={0}", this._username, this._password);
			TransferMgr.doPost(url, data, ref cookies);
			string redirecturl = string.Format("http://{0}/astd2/", extra);
			base.processRedirect(redirecturl, result, ref cookies);
			return result;
		}
	}
}
