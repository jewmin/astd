using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class RenrenLogin : LoginImplBase
	{
		public RenrenLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string text = Uri.EscapeDataString(this._acc.UserName);
			string arg = Uri.EscapeDataString(this._acc.Password);
			string url = "http://www.renren.com/";
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
			string url2 = string.Format("http://www.renren.com/ajaxLogin/login?1=1&uniqueTimestamp=201400150336", CommonUtils.getMSecondsNow(), text, this._acc.Password);
			string data = string.Format("email={0}&password={1}&icode=&origURL=http%3A%2F%2Fwww.renren.com%2Fhome&domain=renren.com&key_id=1&captcha_type=web_login&f=", text, arg);
			bool flag = verify_code != null && verify_code != "";
			if (flag)
			{
				data = string.Format("email={0}&password={1}&icode={2}&origURL=http%3A%2F%2Fwww.renren.com%2Fhome&domain=renren.com&key_id=1&captcha_type=web_login&f=", text, arg, verify_code);
			}
			httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "http://www.renren.com/ajaxproxy.htm", null);
			bool flag2 = httpResult == null;
			LoginResult result;
			if (flag2)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				string content = httpResult.getContent();
				bool flag3 = content.IndexOf("您输入的验证码不正确") >= 0;
				if (flag3)
				{
					loginResult.StatusCode = LoginStatusCode.NeedVerifyCode;
					loginResult.CaptchaUrl = "http://icode.renren.com/getcode.do?t=web_login&rnd=Math.random()";
					loginResult.CaptchaImage = this._loginMgr.getCaptchaImage(loginResult.CaptchaUrl, ref cookies);
					loginResult.ErrMessage = "需要输入验证码(请检查是否用户名/密码不匹配)";
					result = loginResult;
				}
				else
				{
					bool flag4 = content.IndexOf("您的用户名和密码不匹配") >= 0;
					if (flag4)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string starturl = string.Format("http://x{0}.aoshi.renren.com/root/start.action", this._acc.ServerId);
						base.processStartGame(starturl, loginResult, ref cookies, "");
						result = loginResult;
					}
				}
			}
			return result;
		}
	}
}
