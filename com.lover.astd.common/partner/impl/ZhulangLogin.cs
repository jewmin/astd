using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class ZhulangLogin : LoginImplBase
	{
		public ZhulangLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			string url = "http://td.zhulang.com/";
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInGetToken;
				result = loginResult;
			}
			else
			{
				string text = "";
				foreach (Cookie current in httpResult.getCookies())
				{
					bool flag2 = current != null && current.Name == "PHPSESSID";
					if (flag2)
					{
						text = current.Value;
						break;
					}
				}
				bool flag3 = text == null || text == "";
				if (flag3)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					base.logging();
					string url2 = "http://game.zhulang.com/login.php";
					string data = string.Format("PHPSESSID={0}&reurl=http%253A%252F%252Ftd.zhulang.com%252F&username={1}&password={2}&imageField.x=0&imageField.y=0", text, this._username, this._password);
					httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "", null);
					bool flag4 = httpResult == null;
					if (flag4)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string content = httpResult.getContent();
						bool flag5 = content.IndexOf("window.alert") >= 0;
						if (flag5)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							string redirecturl = string.Format("http://td.zhulang.com/checkuser.php?s_id={0}", this._acc.ServerId);
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
