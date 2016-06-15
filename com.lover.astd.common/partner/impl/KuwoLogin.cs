using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class KuwoLogin : LoginImplBase
	{
		public KuwoLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://kuwo.cn/mlog/st/LoginBox";
			string data = string.Format("username={0}&password={1}&useCookie=undefined", this._username, this._password);
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
					string url2 = string.Format("http://astd.kuwo.cn/g/st/JumpAstd?s={0}", this._acc.ServerId);
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						Regex regex = new Regex("window\\.location\\.href='([^'\"]*)';");
						Match match = regex.Match(content2);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							base.processStartGame(value, loginResult, ref cookies, "");
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
