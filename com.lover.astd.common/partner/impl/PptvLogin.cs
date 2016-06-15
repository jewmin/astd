using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class PptvLogin : LoginImplBase
	{
		public PptvLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://user.vas.pptv.com/api/post/login.php";
			string data = string.Format("app=game&do=1&go=http%3A%2F%2Fg.pptv.com%2F&username={0}&password={1}&code={2}", this._username, this._password, (verify_code == null) ? "" : verify_code);
			bool flag = TransferMgr.doPostPure(url, data, ref cookies, "", null) == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				bool flag2 = false;
				foreach (Cookie current in cookies)
				{
					bool flag3 = current.Name == "PPKey";
					if (flag3)
					{
						flag2 = true;
						break;
					}
				}
				bool flag4 = !flag2;
				if (flag4)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = string.Format("http://game.g.pptv.com/go?gid=astd&sid={0}", this._acc.ServerId);
					HttpResult httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag5 = httpResult == null;
					if (flag5)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
						result = loginResult;
					}
					else
					{
						string content = httpResult.getContent();
						Match match = Regex.Match(content, "\"([^\"]+)\"");
						bool flag6 = !match.Success;
						if (flag6)
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
