using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class YouwoLogin : LoginImplBase
	{
		public YouwoLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			HttpResult httpResult = TransferMgr.doGetPure("http://www.youwo.com/gameindexv2.php", ref cookies, "", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInGetToken;
				result = loginResult;
			}
			else
			{
				string content = httpResult.getContent();
				Match match = new Regex("<form.+?action=\"(?<url>.+?)\".+?>", RegexOptions.IgnoreCase).Match(content);
				bool flag2 = !match.Success;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string value = match.Groups["url"].Value;
					match = new Regex("<input\\stype=\"hidden\"\\sname=\"formhash\"\\svalue=\"(?<hash>.+?)\".*?/>", RegexOptions.IgnoreCase).Match(content);
					bool flag3 = !match.Success;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetToken;
						result = loginResult;
					}
					else
					{
						string value2 = match.Groups["hash"].Value;
						bool flag4 = TransferMgr.doPostPure(string.Format("http://www.youwo.com/{0}", value), string.Format("refer=.%2F&formhash={0}&username={1}&password={2}&loginsubmit=%C1%A2%BC%B4%B5%C7%C2%BD", value2, base.getGBKEscape(this._username), base.getGBKEscape(this._password)), ref cookies, "", null) == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							bool flag5 = false;
							int num;
							for (int i = 0; i < cookies.Count; i = num + 1)
							{
								Cookie cookie = cookies[i];
								bool flag6 = cookie != null && cookie.Name == "ucgh_loginuser";
								if (flag6)
								{
									flag5 = true;
									break;
								}
								num = i;
							}
							bool flag7 = !flag5;
							if (flag7)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								string serverUrl = this.getServerUrl(ref cookies);
								bool flag8 = serverUrl.Length == 0;
								if (flag8)
								{
									loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
									result = loginResult;
								}
								else
								{
									httpResult = TransferMgr.doGetPure(serverUrl, ref cookies, "", null);
									content = httpResult.getContent();
									match = new Regex("self\\.location\\.href=\"(?<url>.+?)\"", RegexOptions.IgnoreCase).Match(content);
									bool flag9 = !match.Success;
									if (flag9)
									{
										MatchCollection matchCollection = new Regex("<iframe.+?src=\"(?<url>.+?)\".+?></iframe>", RegexOptions.IgnoreCase).Matches(content);
										foreach (Match match2 in matchCollection)
										{
											bool flag10 = match2.Groups["url"].Value.IndexOf("start.action") > 0;
											if (flag10)
											{
												string value3 = match2.Groups["url"].Value;
												base.processStartGame(value3, loginResult, ref cookies, "");
												result = loginResult;
												return result;
											}
										}
									}
									string value4 = match.Groups["url"].Value;
									base.processRedirect(value4, loginResult, ref cookies);
									result = loginResult;
								}
							}
						}
					}
				}
			}
			return result;
		}

		private string getServerUrl(ref List<Cookie> cookies)
		{
			HttpResult httpResult = TransferMgr.doGetPure("http://astd.youwo.com/login/", ref cookies, "", null);
			bool flag = httpResult == null;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				string content = httpResult.getContent();
				MatchCollection matchCollection = new Regex("<a\\shref=\"http://www\\.youwo\\.com/gamelogin\\.php\\?gameid=17&serverid=(?<id>\\d+)\">.+?</a>", RegexOptions.IgnoreCase).Matches(content);
				string text = "";
				int num = 0;
				int num2;
				for (int i = matchCollection.Count - 1; i >= 0; i = num2 - 1)
				{
					num2 = num;
					num = num2 + 1;
					bool flag2 = num == this._acc.ServerId;
					if (flag2)
					{
						text = matchCollection[i].Groups["id"].Value;
						break;
					}
					num2 = i;
				}
				bool flag3 = text == "";
				if (flag3)
				{
					result = "";
				}
				else
				{
					result = string.Format("http://www.youwo.com/gamelogin.php?gameid=17&serverid={0}", text);
				}
			}
			return result;
		}
	}
}
