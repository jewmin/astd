using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class KunlunLogin : LoginImplBase
	{
		public KunlunLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override bool alwaysNeedVerifyCode()
		{
			return true;
		}

		public override string getAlwaysCaptchaImageUrl()
		{
			return string.Format("http://login.kunlun.tw/?act=index.captcha&r={0}%27%20onerror=", this._rand.NextDouble());
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
				string url = "http://login.kunlun.tw/?act=user.weblogin&lang=tw";
				string data = string.Format("returl=http%3A%2F%2Fstatic.kunlun.tw%2Fwww%2Fsso_js_v2%2Fcallback.html&u=&u2=&username={0}&userpass={1}&usercode={2}", this._username, this._password, verify_code);
				HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
				bool flag2 = httpResult == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string extraHeader = httpResult.getExtraHeader("Location");
					bool flag3 = extraHeader == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						bool flag4 = !extraHeader.Contains("retcode=0");
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							string url2 = "http://go.kunlun.tw/redirect2.php?ref=http://www.kunlun.tw/zh-tw/";
							httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								string extraHeader2 = httpResult.getExtraHeader("Location");
								bool flag6 = extraHeader2 == null;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
								}
								else
								{
									bool flag7 = TransferMgr.doGetPure(extraHeader2, ref cookies, "", null) == null;
									if (flag7)
									{
										loginResult.StatusCode = LoginStatusCode.FailInLogin;
										result = loginResult;
									}
									else
									{
										base.findingServerUrl();
										string starturl = string.Format("http://s{0}.as.kimi.com.tw/root/start.action", this._acc.ServerId);
										base.processStartGame(starturl, loginResult, ref cookies, "");
										result = loginResult;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
