using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class HuolawanLogin : LoginImplBase
	{
		public HuolawanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.huolawan.com/login";
			string data = string.Format("forward=1&email={0}&password={1}&bRememberMe=on&button", this._username, this._password);
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
				bool flag2 = content.Contains("登录帐号或密码错误");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					bool flag3 = TransferMgr.doGetPure("http://www.huolawan.com/", ref cookies, "", null) == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
						result = loginResult;
					}
					else
					{
						string redirecturl = string.Format("http://www.huolawan.com//ginterface/astdgameloading.php/gserver/{0}/noframe", this._acc.ServerId);
						base.processRedirect(redirecturl, loginResult, ref cookies);
						result = loginResult;
					}
				}
			}
			return result;
		}
	}
}
