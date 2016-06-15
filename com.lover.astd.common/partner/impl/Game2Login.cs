using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class Game2Login : LoginImplBase
	{
		public Game2Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.game2.cn/logWebsite.php?skin=astd&gid=40&gc=astd";
			string data = string.Format("code={0}&op=login&&password={1}", this._username, this._password);
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
				bool flag2 = httpResult.StatusCode != 302;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string redirecturl = string.Format("http://www.game2.cn/playGame/code/astd{0}/", this._acc.ServerId);
					base.processRedirect(redirecturl, loginResult, ref cookies);
					result = loginResult;
				}
			}
			return result;
		}
	}
}
