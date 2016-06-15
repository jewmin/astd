using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _3896Login : LoginImplBase
	{
		public _3896Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://www.3896.com/user/denglu.aspx?login_name={0}&forget_pass={1}&che=false&url=", this._username, this._password);
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
				bool flag2 = content.IndexOf("alert") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = "http://www.3896.com/game/serverlist.aspx?yid=502";
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						string arg = string.Format("\\){0}区", this._acc.ServerId);
						Regex regex = new Regex(string.Format("<a href=\"+?([^\"']*?)\"+?[^>]*?>[^<]*?{0}[^<]*?</a>", arg));
						Match match = regex.Match(content2);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							string text = match.Groups[1].Value;
							bool flag5 = !text.StartsWith("http");
							if (flag5)
							{
								text = "http://www.3896.com/game/" + text;
							}
							bool flag6 = TransferMgr.doGetPure(text, ref cookies, "", null) == null;
							if (flag6)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
								result = loginResult;
							}
							else
							{
								base.processRedirect(text, loginResult, ref cookies);
								result = loginResult;
							}
						}
					}
				}
			}
			return result;
		}
	}
}
