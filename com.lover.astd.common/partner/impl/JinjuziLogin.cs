using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class JinjuziLogin : LoginImplBase
	{
		public JinjuziLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://as.jinjuzi.com/logincheck.php";
			string data = string.Format("LoginName={0}&txtPWD={1}&Operation=%E7%99%BB%E5%BD%95&FromURL=%2Findex.php", this._username, this._password);
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
				bool flag2 = content.IndexOf("fail") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = "http://as.jinjuzi.com/server-list.php";
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					string content2 = httpResult.getContent();
					string arg = string.Format("双线{0}区", this.translateNumber(this._acc.ServerId));
					Regex regex = new Regex(string.Format("<a href=\"([^\"']+)\" target=\"_blank\">{0}[^<>]*", arg));
					Match match = regex.Match(content2);
					bool flag3 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
						result = loginResult;
					}
					else
					{
						string text = match.Groups[1].Value;
						bool flag4 = text.StartsWith("./");
						if (flag4)
						{
							text = text.Substring(1);
						}
						base.makeSureValidUri("http://as.jinjuzi.com", ref text);
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
								base.makeSureValidUri("http://as.jinjuzi.com", ref extraHeader);
								httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
								bool flag7 = httpResult == null;
								if (flag7)
								{
									loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
									result = loginResult;
								}
								else
								{
									content2 = httpResult.getContent();
									regex = new Regex(".*?url=(.*?)'");
									match = regex.Match(content2);
									bool flag8 = match == null || match.Groups == null || match.Groups.Count < 2;
									if (flag8)
									{
										loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
										result = loginResult;
									}
									else
									{
										string value = match.Groups[1].Value;
										base.makeSureValidUri("http://as.jinjuzi.com", ref value);
										httpResult = TransferMgr.doGetPure(value, ref cookies, "", null);
										bool flag9 = httpResult == null;
										if (flag9)
										{
											loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
											result = loginResult;
										}
										else
										{
											content2 = httpResult.getContent();
											regex = new Regex("<frame src=\"(http.*?start\\.action.*?)\".*");
											match = regex.Match(content2);
											bool flag10 = match == null || match.Groups == null || match.Groups.Count < 2;
											if (flag10)
											{
												loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
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
					}
				}
			}
			return result;
		}

		private string translateNumber(int number)
		{
			string[] array = new string[]
			{
				"",
				"一",
				"二",
				"三",
				"四",
				"五",
				"六",
				"七",
				"八",
				"九",
				"十"
			};
			string result = number.ToString();
			bool flag = number <= 10;
			if (flag)
			{
				result = array[number];
			}
			else
			{
				result = number.ToString();
			}
			return result;
		}
	}
}
