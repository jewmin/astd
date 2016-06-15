using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class _91wanLogin : LoginImplBase
	{
		public _91wanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string text = "http://www.91wan.com/user/login.php?e=index";
			HttpResult httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
			string url = string.Format("http://www.91wan.com/api/check_login_user.php?act=login&login_user={0}&login_pwd={1}&callback=jsonp{2}&_={2}", this._username, this._password, CommonUtils.getMSecondsNow());
			httpResult = TransferMgr.doGetPure(url, ref cookies, text, null);
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
				bool flag2 = content.IndexOf("\"code\":\"10\"") < 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string redirecturl = string.Format("http://www.91wan.com/user/game_login.php?game_id=23&server_id={0}", this._acc.ServerId);
					base.processRedirect(redirecturl, loginResult, ref cookies);
					result = loginResult;
				}
			}
			return result;
		}
	}
}
