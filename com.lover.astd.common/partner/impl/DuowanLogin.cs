using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class DuowanLogin : LoginImplBase
	{
		public DuowanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "https://udb.duowan.com/login.do";
			bool flag = verify_code != null && verify_code != "";
			string data;
			if (flag)
			{
				data = string.Format("url=&username={0}&password={1}&securityCode={2}", this._username, this._password, verify_code);
			}
			else
			{
				data = string.Format("url=&username={0}&password={1}&securityCode=", this._username, this._password);
			}
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
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
				bool flag3 = false;
				bool flag4 = false;
				int num;
				for (int i = 0; i < cookies.Count; i = num + 1)
				{
					Cookie cookie = cookies[i];
					bool flag5 = cookie != null;
					if (flag5)
					{
						bool flag6 = cookie.Name == "securityCode";
						if (flag6)
						{
							bool flag7 = cookie.Value != null && cookie.Value != "";
							if (flag7)
							{
								flag3 = true;
							}
						}
						else
						{
							bool flag8 = cookie.Name == "oauthCookie";
							if (flag8)
							{
								flag4 = true;
							}
						}
					}
					num = i;
				}
				bool flag9 = !flag4;
				if (flag9)
				{
					bool flag10 = flag3;
					if (flag10)
					{
						loginResult.StatusCode = LoginStatusCode.NeedVerifyCode;
						loginResult.ErrMessage = "您本次登录需要验证码";
						loginResult.CaptchaUrl = "https://udb.duowan.com/verify/login.do?" + CommonUtils.getMSecondsNow().ToString();
						loginResult.CaptchaImage = this._loginMgr.getCaptchaImage(loginResult.CaptchaUrl, ref cookies);
						loginResult.WebCookies = cookies;
						result = loginResult;
					}
					else
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
				}
				else
				{
					string url2 = string.Format("http://udblogin.duowan.com/login.do?game=AS&server=s{0}&serverType=GENERAL&pid=&report_ver=new&from=webyygame&rso=webyygame&webyygame&pro=webyygame&ref=WEB/XF&ref_desc=%E7%BD%91%E9%A1%B5%2F%E9%80%89%E6%9C%8D&showtools=0", this._acc.ServerId);
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					content = httpResult.getContent();
					Match match = Regex.Match(content, "<iframe src=['\"]+([^'\"]*)['\"]+");
					bool flag11 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag11)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
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
			return result;
		}
	}
}
