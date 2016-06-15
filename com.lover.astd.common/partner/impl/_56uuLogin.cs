using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _56uuLogin : LoginImplBase
	{
		public _56uuLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.56uu.com/login.html";
			string data = string.Format("rnd={0}&act=ajlogin&getlog=ptlogin&username={1}&password={2}&save_id=1", this._rand.NextDouble(), this._username, this._password);
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
				bool flag2 = !content.Contains("member_id");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = "http://as.56uu.com/server_list.html";
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
						string arg = string.Format("(\\[{0}\\]|56uu{0}服)", this._acc.ServerId);
						Regex regex = new Regex(string.Format("<a.*href=[\"']+?([^\"']*?)[\"']+?.*>.*?{0}.*?</a>", arg));
						Match match = regex.Match(content2);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							httpResult = TransferMgr.doGetPure(value, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGetSession;
								result = loginResult;
							}
							else
							{
								content2 = httpResult.getContent();
								regex = new Regex("<frame src=[\"']+?(.*?)[\"']+?.*");
								match = regex.Match(content2);
								bool flag6 = match == null || match.Groups == null || match.Groups.Count < 2;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGetSession;
									result = loginResult;
								}
								else
								{
									string value2 = match.Groups[1].Value;
									base.processStartGame(value2, loginResult, ref cookies, "");
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
