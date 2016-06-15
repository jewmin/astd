using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class QidianLogin : LoginImplBase
	{
		public QidianLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "https://cas.sdo.com/CAS/Login.Validate.Account?service=http%3A%2F%2Fwww%2Esdo%2Ecom%2Findex%2Easp";
			string data = string.Format("lt=test&idtype=0&gamearea=0&gametype=0&username={0}&password={1}&ptname={0}&ptpwd={1}", this._username, this._password);
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
				Match match = new Regex("location\\.href=(?<url>.+?);", RegexOptions.IgnoreCase).Match(content);
				bool flag2 = !match.Success || match.Groups["url"].Value.IndexOf("error=0") < 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					httpResult = TransferMgr.doGetPure(string.Format("https://cas.sdo.com/cas/login?service=http%3a%2f%2fastd{0:d2}.game.qidian.com%2froot%2fstart.action", this._acc.ServerId), ref cookies, "", null);
					content = httpResult.getContent();
					match = new Regex("varpath=(?<url>.+?);", RegexOptions.IgnoreCase).Match(content);
					bool flag3 = !match.Success;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
						result = loginResult;
					}
					else
					{
						string value = match.Groups["url"].Value;
						base.processRedirect(value, loginResult, ref cookies);
						result = loginResult;
					}
				}
			}
			return result;
		}
	}
}
