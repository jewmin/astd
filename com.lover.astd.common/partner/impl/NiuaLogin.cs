using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class NiuaLogin : LoginImplBase
	{
		public NiuaLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override bool alwaysNeedVerifyCode()
		{
			return true;
		}

		public override string getAlwaysCaptchaImageUrl()
		{
			return "http://www.niua.com/seccode.php";
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			bool flag = verify_code == null;
			LoginResult result;
			if (flag)
			{
				base.buildAlwaysVerifyResult(ref loginResult, ref cookies);
				result = loginResult;
			}
			else
			{
				string url = "http://www.niua.com/login/";
				string data = string.Format("refer=http%3A%2F%2Fwww.niua.com%2Findex.php&name={0}&password={1}&seccode={2}&keeplive=1", this._username, this._md5_password_lower, verify_code);
				HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
				bool flag2 = httpResult == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string content = httpResult.getContent();
					bool flag3 = content.Contains("/login/");
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string redirecturl = string.Format("http://www.niua.com/playGame/code/astd{0}/", this._acc.ServerId);
						base.processRedirect(redirecturl, loginResult, ref cookies);
						result = loginResult;
					}
				}
			}
			return result;
		}
	}
}
