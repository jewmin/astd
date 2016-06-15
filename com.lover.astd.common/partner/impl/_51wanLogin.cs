using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _51wanLogin : LoginImplBase
	{
		public _51wanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://user.51wan.com/login_index_theLogin_0.html";
			string data = string.Format("gamename=&serverid=0&refer=http%3A%2F%2Fmy.51wan.com%2Fgame.html&coid=0&needValidate=&username={0}&password={1}&code=&rememberYN=1", this._username, this._password);
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
				bool flag2 = httpResult.StatusCode != 302;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = "http://my.51wan.com/game_0_gameserverList_as.html";
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
						result = loginResult;
					}
					else
					{
						string content = httpResult.getContent();
						string arg = string.Format(">{0}<\\/em>", this._acc.ServerId);
						Regex regex = new Regex(string.Format("<a.*href=[\"']+?(game_login_0_as-[^\"']*?)[\"']+?.*>.*?{0}.*?</a>", arg));
						Match match = regex.Match(content);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							string text = match.Groups[1].Value;
							text = "http://my.51wan.com/" + text;
							httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
								result = loginResult;
							}
							else
							{
								string extraHeader = httpResult.getExtraHeader("Location");
								bool flag6 = extraHeader == null;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
									result = loginResult;
								}
								else
								{
									httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
									bool flag7 = httpResult == null;
									if (flag7)
									{
										loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
										result = loginResult;
									}
									else
									{
										content = httpResult.getContent();
										regex = new Regex(".*?<IFRAME.*?SRC=[\"']+?(.*?)[\"']+?.*");
										match = regex.Match(content);
										bool flag8 = match == null || match.Groups == null || match.Groups.Count < 2;
										if (flag8)
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
				}
			}
			return result;
		}
	}
}
