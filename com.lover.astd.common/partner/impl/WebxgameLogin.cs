using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class WebxgameLogin : LoginImplBase
	{
		public WebxgameLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.webxgame.com/interface/home/login.php";
			string data = string.Format("username={0}&password={1}&button.x=38&button.y=23&tempurl=http%3A%2F%2Ftd.webxgame.com%2Fservermorelist.php&sitename=astd", this._username, this._password);
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
				bool flag2 = content.IndexOf("alert") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = string.Format("http://td.webxgame.com/openserver.php?serverid={0}", this._acc.ServerId);
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
						Match match = Regex.Match(content2, "src=\"(http:\\/\\/td\\d+\\.webxgame\\.com\\/root\\/start\\.action[^\"']+)\"");
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
