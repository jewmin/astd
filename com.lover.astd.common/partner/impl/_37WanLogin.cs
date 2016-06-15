using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _37WanLogin : LoginImplBase
	{
		public _37WanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string text = Uri.EscapeDataString(this._acc.UserName);
			string text2 = CommonUtils.generateStringMd5(this._acc.Password, null);
			text2 = text2.ToLower();
			string text3 = string.Format("http://my.37.com/api/login.php?callback=jQuery18306438154398316079_{0}&action=login&login_account={1}&password={2}&ajax=0&remember_me=0&_={0}", CommonUtils.getMSecondsNow(), text, this._acc.Password);
			bool flag = verify_code != null && verify_code != "";
			if (flag)
			{
				text3 = string.Format("http://my.37.com/api/login.php?callback=jQuery18306438154398316079_{0}&action=login&login_account={1}&password={2}&ajax=0&save_code={3}&remember_me=0&_={0}", new object[]
				{
					CommonUtils.getMSecondsNow(),
					text,
					this._acc.Password,
					verify_code
				});
			}
			HttpResult httpResult = TransferMgr.doGetPure(text3, ref cookies, "http://www.37.com", null);
			bool flag2 = httpResult == null;
			LoginResult result;
			if (flag2)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				bool flag3 = httpResult.StatusCode == 302 || httpResult.StatusCode == 301;
				if (flag3)
				{
					string extraHeader = httpResult.getExtraHeader("Location");
					bool flag4 = extraHeader != null && extraHeader != "";
					if (flag4)
					{
						httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, text3, null);
						bool flag5 = httpResult == null;
						if (flag5)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
							return result;
						}
					}
				}
				string content = httpResult.getContent();
				bool flag6 = content.Contains("\"code\":-7") || content.Contains("\"code\":-11");
				if (flag6)
				{
					loginResult.StatusCode = LoginStatusCode.NeedVerifyCode;
					loginResult.ErrMessage = "需要输入验证码";
					loginResult.CaptchaUrl = "http://my.37.com/code.php?t=" + CommonUtils.getMSecondsNow().ToString();
					loginResult.CaptchaImage = this._loginMgr.getCaptchaImage(loginResult.CaptchaUrl, ref cookies);
					loginResult.WebCookies = cookies;
					result = loginResult;
				}
				else
				{
					bool flag7 = false;
					foreach (Cookie current in cookies)
					{
						bool flag8 = current.Name == "ispass_37wan_com";
						if (flag8)
						{
							flag7 = true;
							break;
						}
					}
					bool flag9 = !flag7;
					if (flag9)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						content = httpResult.getContent();
						Regex regex = new Regex("\"msg\":\"([^\"]+)\"");
						Match match = regex.Match(content);
						bool flag10 = match.Groups != null && match.Groups.Count >= 2;
						if (flag10)
						{
							try
							{
								string arg = Regex.Unescape(match.Groups[1].Value);
								loginResult.ErrMessage = string.Format("服务器返回:{0}", arg);
							}
							catch
							{
							}
						}
						result = loginResult;
					}
					else
					{
						string redirecturl = string.Format("http://astd.37.com/entergame.php?server=S{0}&tuitan=", this._acc.ServerId);
						base.processRedirect(redirecturl, loginResult, ref cookies);
						result = loginResult;
					}
				}
			}
			return result;
		}
	}
}
