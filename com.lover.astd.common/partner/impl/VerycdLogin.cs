using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class VerycdLogin : LoginImplBase
	{
		public VerycdLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://game.verycd.com/proxy?url=/signin";
			string data = string.Format("continue=http%3A%2F%2Fgame.verycd.com%2Fas%2F&login_submit=%E7%99%BB%E5%BD%95&username={0}&password={1}", this._username, this._password);
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "http://game.verycd.com/as/", new Dictionary<string, string>
			{
				{
					"X-Requested-With",
					"XMLHttpRequest"
				}
			});
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
				bool flag2 = content.IndexOf("\"status\":false") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = string.Format("http://secure.verycd.com/signin?ak=astd&sid=S{0}", this._acc.ServerId);
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetSession;
						result = loginResult;
					}
					else
					{
						string extraHeader = httpResult.getExtraHeader("Location");
						bool flag4 = extraHeader == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGetSession;
							result = loginResult;
						}
						else
						{
							httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGetSession;
								result = loginResult;
							}
							else
							{
								string content2 = httpResult.getContent();
								Regex regex = new Regex(".*document\\.getElementById\\(\"play_frame\"\\)\\.src=\"([^\"']*?)\";.*");
								Match match = regex.Match(content2);
								bool flag6 = match == null || match.Groups == null || match.Groups.Count < 2;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
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
				}
			}
			return result;
		}
	}
}
