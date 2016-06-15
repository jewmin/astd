using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _29wwLogin : LoginImplBase
	{
		public _29wwLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://29ww.com/CommonSource/UserLogin.ashx";
			string data = string.Format("uName={0}&uPwd={1}&CRM=true&method=Login", this._username, this._password);
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
				bool flag2 = !content.Equals("Y");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					base.findingServerUrl();
					string text = "http://astd.29ww.com/Y.html";
					httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
						result = loginResult;
					}
					else
					{
						string input = httpResult.getContent(Encoding.GetEncoding("GBK"));
						Regex regex = new Regex(string.Format("<a href='(.*)' target=\"_blank\">([^<>])*?双线{0}服([^<>])*?</a>", this.translateNumber(this._acc.ServerId)));
						Match match = regex.Match(input);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							input = match.Groups[1].Value;
							string value = match.Groups[1].Value;
							base.goingToGameUrl();
							base.makeSureValidUri(text, ref value);
							httpResult = TransferMgr.doGetPure(value, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
								result = loginResult;
							}
							else
							{
								bool flag6 = httpResult.StatusCode != 302;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
									result = loginResult;
								}
								else
								{
									string extraHeader = httpResult.getExtraHeader("Location");
									bool flag7 = extraHeader == null || extraHeader == "";
									if (flag7)
									{
										loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
										result = loginResult;
									}
									else
									{
										base.makeSureValidUri(text, ref extraHeader);
										base.processStartGame(extraHeader, loginResult, ref cookies, "");
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
				"九"
			};
			string result = number.ToString();
			bool flag = number < 100;
			if (flag)
			{
				bool flag2 = number < 10;
				if (flag2)
				{
					result = array[number];
				}
				else
				{
					int num = number / 10;
					int num2 = number % 10;
					result = ((num > 1) ? array[num] : ("十" + array[num2]));
				}
			}
			return result;
		}
	}
}
