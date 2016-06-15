using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class XdwanLogin : LoginImplBase
	{
		public XdwanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://service.xdwan.com/user/login?jsoncallback=jQuery11100805599416950391_{0}&t=login&account={1}&password={2}&cookie=0&r={3}", new object[]
			{
				CommonUtils.getMSecondsNow(),
				this._username,
				this._password,
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
				bool flag2 = !content.Contains("\"flag\":true");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = "http://as.xdwan.com/servers.aspx";
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						string arg = string.Format("兄弟玩{0}服", this._acc.ServerId);
						Regex regex = new Regex(string.Format(".*<a.*?href=[\"']+?([^\"']*?)[\"']+?>[\\w\\r\\n\\t\\s]*?{0}.*?</a>.*", arg));
						Match match = regex.Match(content2);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
							result = loginResult;
						}
						else
						{
							string text = match.Groups[1].Value;
							text = text.Replace("?Gameid", "start.ashx?Gameid");
							base.processRedirect(text, loginResult, ref cookies);
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
