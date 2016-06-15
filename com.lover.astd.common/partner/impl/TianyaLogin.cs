using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class TianyaLogin : LoginImplBase
	{
		public TianyaLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string text = "http://passport.tianya.cn/login";
			string data = string.Format("fowardURL=http%3A%2F%2Fgame.tianya.cn%2F&method=name&vwriter={0}&vpassword={1}&submit=+", this._username, this._password);
			HttpResult httpResult = TransferMgr.doPostPure(text, data, ref cookies, text, null);
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
				Regex regex = new Regex("location.href=\"([^\"]+)\"");
				Match match = regex.Match(content);
				bool flag2 = match == null || match.Groups == null || match.Groups.Count < 2;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string value = match.Groups[1].Value;
					httpResult = TransferMgr.doGetPure(value, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						content = httpResult.getContent();
						regex = new Regex("src=\"([^\"]+)\"");
						MatchCollection matchCollection = regex.Matches(content);
						bool flag4 = matchCollection == null || matchCollection.Count == 0;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							foreach (Match match2 in matchCollection)
							{
								bool flag5 = match2 != null && match2.Groups != null && match2.Groups.Count >= 2;
								if (flag5)
								{
									string value2 = match2.Groups[1].Value;
									base.makeSureValidUri("http://passport.tianya.cn", ref value2);
									TransferMgr.doGetPure(value2, ref cookies, value, null);
								}
							}
							string text2 = "http://astd.tianya.cn/app/server.jsp";
							httpResult = TransferMgr.doGetPure(text2, ref cookies, value, null);
							bool flag6 = httpResult == null;
							if (flag6)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								content = httpResult.getContent();
								int num = 0;
								bool flag7 = this._acc.ServerId == 1;
								if (flag7)
								{
									num = 56;
								}
								else
								{
									bool flag8 = this._acc.ServerId >= 2 && this._acc.ServerId <= 4;
									if (flag8)
									{
										num = 61;
									}
									else
									{
										bool flag9 = this._acc.ServerId == 5;
										if (flag9)
										{
											num = 183;
										}
									}
								}
								text2 = "http://game.tianya.cn/intf/login_intf.jsp?gid=1050&serverid=" + num;
								httpResult = TransferMgr.doGetPure(text2, ref cookies, value, null);
								bool flag10 = httpResult == null;
								if (flag10)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
								}
								else
								{
									content = httpResult.getContent();
									regex = new Regex("'(http://s\\d+\\.astd\\.tianya\\.cn[^'\"]+)'");
									match = regex.Match(content);
									bool flag11 = match == null || match.Groups == null || match.Groups.Count < 2;
									if (flag11)
									{
										loginResult.StatusCode = LoginStatusCode.FailInLogin;
										result = loginResult;
									}
									else
									{
										text2 = match.Groups[1].Value;
										base.processStartGame(text2, loginResult, ref cookies, "");
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
