using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _51Login : LoginImplBase
	{
		public _51Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://passport.51.com/login/submit";
			string data = string.Format("passport_51_user={0}&passport_51_password={1}&passport_cookie_login=0&from=www&gourl=http://my.51.com/&passport_auto_login=0&passport_51_ishidden=0&chn=www&ie=0&version=2012&passport_51_ajax=true", this._username, this._password);
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
				bool flag2 = !content.Contains("\"errno\":0");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = string.Format("http://s{0}.astd.51.com/", this._acc.ServerId);
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
						result = loginResult;
					}
					else
					{
						string input = httpResult.getContent();
						Regex regex = new Regex("loginCheck\\('([^']*)',");
						Match match = regex.Match(input);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							string text = "";
							foreach (Cookie current in cookies)
							{
								bool flag5 = current.Name == "FO_LSTATE";
								if (flag5)
								{
									string[] separator = new string[]
									{
										"%7C"
									};
									string value2 = current.Value;
									string[] array = value2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
									text = array[array.Length - 1];
									break;
								}
							}
							bool flag6 = text == "";
							if (flag6)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								url2 = string.Format("http://gameapi.51.com/jsclient/passport/{0}/?now_user={1}&callback=jsonp{2}&_={2}", value, text, CommonUtils.getMSecondsNow());
								httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
								bool flag7 = httpResult == null;
								if (flag7)
								{
									loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
									result = loginResult;
								}
								else
								{
									input = httpResult.getContent();
									string domain = string.Format("s{0}.astd.51.com", this._acc.ServerId);
									match = new Regex("fogameclient\\.passportcallback\\(\\{(?<data>.+?)\\}\\);", RegexOptions.IgnoreCase).Match(input);
									bool flag8 = !match.Success;
									if (flag8)
									{
										loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
										result = loginResult;
									}
									else
									{
										input = match.Groups["data"].Value;
										MatchCollection matchCollection = new Regex("(?<key>.+?):\"(?<val>.+?)\"", RegexOptions.IgnoreCase).Matches(input);
										string text2 = "";
										foreach (Match match2 in matchCollection)
										{
											bool flag9 = match2.Groups["key"].Value == "app_key";
											if (flag9)
											{
												text2 = match2.Groups["val"].Value;
											}
											else
											{
												bool flag10 = match2.Groups["key"].Value.Replace(",", "") == "sig";
												if (flag10)
												{
													Cookie cookie = new Cookie();
													cookie.Name = text2;
													cookie.Value = match2.Groups["val"].Value;
													cookie.Path = "/";
													cookie.Domain = domain;
													cookies.Add(cookie);
												}
												else
												{
													Cookie cookie2 = new Cookie();
													cookie2.Name = text2 + "_" + match2.Groups["key"].Value.Replace(",", "");
													cookie2.Value = match2.Groups["val"].Value;
													cookie2.Path = "/";
													cookie2.Domain = domain;
													cookies.Add(cookie2);
												}
											}
										}
										string starturl = string.Format("http://s{0}.astd.51.com/root/start.action", this._acc.ServerId);
										base.processStartGame(starturl, loginResult, ref cookies, "");
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
