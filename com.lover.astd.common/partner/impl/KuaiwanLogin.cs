using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class KuaiwanLogin : LoginImplBase
	{
		public KuaiwanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://web.teeqee.com/reg/login.php";
			string data = string.Format("username={0}&password={1}&next_url=http%3A%2F%2Fastd.teeqee.com&game=astd_home", this._username, this._password);
			bool flag = TransferMgr.doPostPure(url, data, ref cookies, "", null) == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				bool flag2 = false;
				foreach (Cookie current in cookies)
				{
					bool flag3 = current.Name == "userid";
					if (flag3)
					{
						flag2 = true;
						break;
					}
				}
				bool flag4 = !flag2;
				if (flag4)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string text = string.Format("http://web.teeqee.com/start/?t=astd&s={0}", this._acc.ServerId);
					HttpResult httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
					bool flag5 = httpResult == null;
					if (flag5)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
						result = loginResult;
					}
					else
					{
						string content = httpResult.getContent();
						Match match = Regex.Match(content, "\"(/start/main\\.php[^\"]+)\"");
						bool flag6 = !match.Success;
						if (flag6)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							base.makeSureValidUri(text, ref value);
							httpResult = TransferMgr.doGetPure(value, ref cookies, "http://astd.teeqee.com/", null);
							bool flag7 = httpResult == null;
							if (flag7)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
								result = loginResult;
							}
							else
							{
								content = httpResult.getContent();
								match = Regex.Match(content, "\"([^\"]+start\\.action[^\"]+)\"");
								bool flag8 = !match.Success;
								if (flag8)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
									result = loginResult;
								}
								else
								{
									string value2 = match.Groups[1].Value;
									base.makeSureValidUri(value, ref value2);
									base.processStartGame(value2, loginResult, ref cookies, "");
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
