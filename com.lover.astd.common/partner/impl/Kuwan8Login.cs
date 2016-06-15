using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class Kuwan8Login : LoginImplBase
	{
		private string _randNum = "88234";

		private string _checkCode = "3607";

		public Kuwan8Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://www.kuwan8.com/user/userloginajax.aspx?a=login&RandNum={0}&username={1}&code={2}{3}&cache={4}&checked=0;", new object[]
			{
				this._randNum,
				this._username,
				this._md5_password_lower,
				CommonUtils.generateStringMd5(this._checkCode, null),
				this._rand.NextDouble()
			});
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
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
				bool flag2 = content.IndexOf("ERR$$$") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string redirecturl = string.Format("http://www.kuwan8.com/user/gotogame.aspx?gid=4&zid={0}", this._acc.ServerId);
					base.processRedirect(redirecturl, loginResult, ref cookies);
					result = loginResult;
				}
			}
			return result;
		}
	}
}
