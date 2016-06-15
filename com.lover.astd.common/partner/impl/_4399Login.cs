using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _4399Login : LoginImplBase
	{
		public _4399Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://ptlogin.4399.com/ptlogin/login.do?v=1";
			string data = string.Format("loginFrom=uframe&postLoginHandler=default&layoutSelfAdapting=true&externalLogin=qq&displayMode=embed&layout=vertical&appId=www_home&gameId=&css=&redirectUrl=&sessionId=&mainDivId=popup_login_div&includeFcmInfo=false&username={0}&password={1}", this._username, this._password);
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
					bool flag3 = current.Name.Equals("ck_accname");
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
					string url2 = string.Format("http://web.4399.com/stat/togame.php?target=astd&server_id=S{0}", this._acc.ServerId);
					HttpResult httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					string extraHeader = httpResult.getExtraHeader("Location");
					bool flag5 = extraHeader != null && extraHeader != "";
					if (flag5)
					{
						base.processStartGame(extraHeader, loginResult, ref cookies, "");
						result = loginResult;
					}
					else
					{
						string content = httpResult.getContent();
						Match match = Regex.Match(content, "<iframe.*?src=['\"]+([^'\"]*)['\"]+");
						bool flag6 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag6)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGetSession;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							base.processStartGame(value, loginResult, ref cookies, "");
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
