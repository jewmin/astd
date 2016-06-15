using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class PluLogin : LoginImplBase
	{
		public PluLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://login.plu.cn/user/login";
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
				Regex regex = new Regex("<input type=\"hidden\" name=\"([^\"]+)\" value=\"([^\"]+)\"");
				Match match = regex.Match(content);
				bool flag2 = match == null || match.Groups == null || match.Groups.Count < 3;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string value = match.Groups[1].Value;
					string value2 = match.Groups[2].Value;
					string data = string.Format("name={0}&password={1}&{2}={3}&login_submit=%E7%99%BB%E5%BD%95", new object[]
					{
						this._username,
						this._password,
						value,
						value2
					});
					httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						bool flag4 = !content2.Contains("location.href='http://www.plu.cn'");
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							string redirecturl = string.Format("http://webgame.plu.cn/play/astd.php?sid={0}", this._acc.ServerId);
							base.processRedirect(redirecturl, loginResult, ref cookies);
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
