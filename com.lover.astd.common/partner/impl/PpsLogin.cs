using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class PpsLogin : LoginImplBase
	{
		public PpsLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://game.pps.tv/passport/api/login?charset=gbk";
			this._username = base.getGBKEscape(this._acc.UserName);
			string data = string.Format("preurl=http%253A%252F%252Fgame.pps.tv%252Fastd_login.php%253F&position=website_138&username={0}&userpasswd={1}&captcha=", this._username, this._password);
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "http://game.pps.tv/astd_login.php?", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				string extraHeader = httpResult.getExtraHeader("Location");
				bool flag2 = extraHeader == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					base.findingServerUrl();
					httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
					string content = httpResult.getContent(Encoding.GetEncoding("GBK"));
					string arg = string.Format("{0}服 ", this._acc.ServerId);
					Regex regex = new Regex(string.Format("<a.*href=\"([^\"']*?)\".*>{0}.*</a>", arg));
					MatchCollection matchCollection = regex.Matches(content);
					bool flag3 = matchCollection == null || matchCollection.Count == 0 || matchCollection[0] == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
						result = loginResult;
					}
					else
					{
						bool flag4 = matchCollection[0].Groups == null || matchCollection[0].Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							string value = matchCollection[0].Groups[1].Value;
							base.processRedirect(value, loginResult, ref cookies);
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
