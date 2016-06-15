using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _789hiLogin : LoginImplBase
	{
		public _789hiLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.789hi.com/reg/login.html";
			string data = string.Format("username={0}&userpass={1}&long=0&save=0", this._username, this._password);
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
				bool flag2 = content.Contains("\"ok\":0");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					base.findingServerUrl();
					string text = "http://www.789hi.com/astd.html";
					httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						Regex regex = new Regex(string.Format("\"(astd_\\w+\\.html)\">双线{0}服</a>", this._acc.ServerId));
						Match match = regex.Match(content2);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							base.makeSureValidUri(text, ref value);
							httpResult = TransferMgr.doGetPure(value, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
								result = loginResult;
							}
							else
							{
								content2 = httpResult.getContent();
								regex = new Regex("<iframe.*?src=\"([^\"]*)\"");
								match = regex.Match(content2);
								bool flag6 = match == null || match.Groups == null || match.Groups.Count < 2;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
									result = loginResult;
								}
								else
								{
									value = match.Groups[1].Value;
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
