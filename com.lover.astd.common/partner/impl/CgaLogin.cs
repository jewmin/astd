using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class CgaLogin : LoginImplBase
	{
		public CgaLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override bool alwaysNeedVerifyCode()
		{
			return true;
		}

		public override string getAlwaysCaptchaImageUrl()
		{
			return "http://passport.cga.com.cn/login/VCode.ashx";
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
				string url = "http://passport.cga.com.cn/login/Service/LoginService.ashx?op=CheckUserLogin&jsoncallback=jsonp" + CommonUtils.getMSecondsNow();
				bool flag2 = TransferMgr.doGetPure(url, ref cookies, "", null) == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string url2 = "http://passport.cga.com.cn/login/LoginPost.aspx";
					string data = string.Format("returnurl=http%3A%2F%2Fastd.cga.com.cn%2Fgame.html%3Fgameinfo%3D77%2C{0}&loginname={1}&password={2}&checkcode={3}&select={0}&x=75&y=23", new object[]
					{
						this._acc.ServerId,
						this._username,
						this._password,
						verify_code
					});
					HttpResult httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "http://astd.cga.com.cn/", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						httpResult.getContent();
						bool flag4 = httpResult.StatusCode != 302;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							string redirecturl = string.Format("http://passport.cga.com.cn/login/sp/togame.aspx?gameinfo=77,{0}", this._acc.ServerId);
							base.processRedirect(redirecturl, loginResult, ref cookies);
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
