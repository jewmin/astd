using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class Letou8Login : LoginImplBase
	{
		public Letou8Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			string url = "http://www.letou8.com/aoshi/index.aspx";
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
				Regex regex = new Regex("id=\"__EVENTVALIDATION\" value=\"([^\"]*)\"");
				Match match = regex.Match(content);
				bool flag2 = match == null || match.Groups == null || match.Groups.Count < 2;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string text = Uri.EscapeDataString(match.Groups[1].Value);
					regex = new Regex("id=\"__VIEWSTATE\" value=\"([^\"]*)\"");
					match = regex.Match(content);
					bool flag3 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetToken;
						result = loginResult;
					}
					else
					{
						string text2 = Uri.EscapeDataString(match.Groups[1].Value);
						base.logging();
						string url2 = "http://www.letou8.com/aoshi/index.aspx";
						this._username = base.getGBKEscape(this._acc.UserName);
						string data = string.Format("__EVENTARGUMENT=&__EVENTTARGET=&__EVENTVALIDATION={0}&__VIEWSTATE={1}&brServer=1&left1$account={2}&left1$btn_login=&left1$pwd={3}&sel_guo=weiguo", new object[]
						{
							text,
							text2,
							this._username,
							this._password
						});
						httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "", null);
						bool flag4 = httpResult == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							bool flag5 = httpResult.StatusCode != 302;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								string url3 = "http://www.letou8.com/aoshi/gm.aspx";
								httpResult = TransferMgr.doGetPure(url3, ref cookies, "", null);
								bool flag6 = httpResult == null;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
								}
								else
								{
									content = httpResult.getContent();
									regex = new Regex(string.Format("\"(http://astd\\d+\\.78bar\\.com[^\"]*)\"", new object[0]));
									match = regex.Match(content);
									bool flag7 = match == null || match.Groups == null || match.Groups.Count < 2;
									if (flag7)
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
			return result;
		}
	}
}
