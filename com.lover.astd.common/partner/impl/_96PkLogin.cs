using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _96PkLogin : LoginImplBase
	{
		public _96PkLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string text = "http://www.96pk.com";
			HttpResult httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
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
				Regex regex = new Regex("action=\"([^\"]*)\"");
				Match match = regex.Match(content);
				bool flag2 = match == null || match.Groups == null || match.Groups.Count < 2;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string value = match.Groups[1].Value;
					base.makeSureValidUri(text, ref value);
					regex = new Regex("name=\"refer\" value=\"([^\"]*)\"");
					match = regex.Match(content);
					bool flag3 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetToken;
						result = loginResult;
					}
					else
					{
						string value2 = match.Groups[1].Value;
						regex = new Regex("name=\"formhash\" value=\"([^\"]*)\"");
						match = regex.Match(content);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGetToken;
							result = loginResult;
						}
						else
						{
							string value3 = match.Groups[1].Value;
							string data = string.Format("username={0}&password={1}&refer={2}&loginsubmit=%C2%A0&formhash={3}", new object[]
							{
								this._username,
								this._password,
								value2,
								value3
							});
							bool flag5 = TransferMgr.doPostPure(value, data, ref cookies, "", null) == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								bool flag6 = false;
								foreach (Cookie current in cookies)
								{
									bool flag7 = current.Name == "uchome_auth" && current.Value != "deleted";
									if (flag7)
									{
										flag6 = true;
										break;
									}
								}
								bool flag8 = !flag6;
								if (flag8)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
								}
								else
								{
									string url = "http://www.96pk.com/static/js/user_visit.php";
									bool flag9 = TransferMgr.doGetPure(url, ref cookies, "", null) == null;
									if (flag9)
									{
										loginResult.StatusCode = LoginStatusCode.FailInLogin;
										result = loginResult;
									}
									else
									{
										string url2 = string.Format("http://www.96pk.com/gamelogin.php?gamename=as&servername=S{0}", this._acc.ServerId);
										httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
										bool flag10 = httpResult == null;
										if (flag10)
										{
											loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
											result = loginResult;
										}
										else
										{
											content = httpResult.getContent();
											regex = new Regex("<iframe src=\"([^\"]*start\\.action[^\"]*)\"");
											match = regex.Match(content);
											bool flag11 = match == null || match.Groups == null || match.Groups.Count < 2;
											if (flag11)
											{
												loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
												result = loginResult;
											}
											else
											{
												string text2 = "";
												int num;
												for (int i = 1; i < match.Groups.Count; i = num + 1)
												{
													string value4 = match.Groups[i].Value;
													bool flag12 = value4.Contains("start.action?");
													if (flag12)
													{
														text2 = value4;
														break;
													}
													num = i;
												}
												bool flag13 = text2 == "";
												if (flag13)
												{
													loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
													result = loginResult;
												}
												else
												{
													base.processStartGame(text2, loginResult, ref cookies, "");
													result = loginResult;
												}
											}
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
