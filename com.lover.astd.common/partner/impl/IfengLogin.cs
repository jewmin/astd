using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class IfengLogin : LoginImplBase
	{
		public IfengLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://play.ifeng.com/?_a=dologin";
			string data = string.Format("username={0}&password={1}&reurl=&game=&area=", this._username, this._password);
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
				string extraHeader = httpResult.getExtraHeader("Location");
				bool flag2 = extraHeader == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					bool flag3 = TransferMgr.doPostPure(url, data, ref cookies, "", null) == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string text = string.Format("http://play.ifeng.com/?_a=entergame&game=astd&area={0}", this._acc.ServerId);
						httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
						bool flag4 = httpResult == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							bool flag5 = httpResult.StatusCode == 302;
							if (flag5)
							{
								extraHeader = httpResult.getExtraHeader("Location");
								base.makeSureValidUri(text, ref extraHeader);
								httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
								bool flag6 = httpResult == null;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
									return result;
								}
								string content = httpResult.getContent();
								Regex regex = new Regex("\\(\"#game_iframe_id\"\\)\\.attr\\(\"src\",\"([^\"']*?)\"");
								Match match = regex.Match(content);
								bool flag7 = match == null || match.Groups == null || match.Groups.Count < 2;
								if (flag7)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
									return result;
								}
								string value = match.Groups[1].Value;
								base.processStartGame(value, loginResult, ref cookies, "");
							}
							else
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
							}
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
