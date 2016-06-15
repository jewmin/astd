using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class _8zyLogin : LoginImplBase
	{
		public _8zyLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://webgame.8zy.com/as/index.php?do=login1";
			string data = string.Format("userid={0}&fmdo=login&dopost=login&gourl=http%3A%2F%2Fas.8zy.com%2Findex.html&pwd={1}&x=93&y=17", this._username, this._password);
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				string content = httpResult.getContent();
				bool flag2 = !content.Contains("登录成功");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string redirecturl = string.Format("http://webgame.8zy.com/as/index.php?serverid={0}", this._acc.ServerId);
					base.processRedirect(redirecturl, loginResult, ref cookies);
					result = loginResult;
				}
			}
			return result;
		}
	}
}
