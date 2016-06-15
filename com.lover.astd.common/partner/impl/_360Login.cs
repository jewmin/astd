using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System.Net.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class _360Login : LoginImplBase
	{
		public _360Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string text = "";
			string url = string.Format("https://login.360.cn/?func=jQuery111305993051263333831_1444136816685&src=pcw_wan&from=pcw_wan&charset=utf-8&requestScema=https&o=sso&m=getToken&userName={0}&_={1}", this._username, CommonUtils.getMSecondsNow());
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
				string text2 = httpResult.getContent();
				text2 = text2.Substring(text2.IndexOf('(') + 1);
				text2 = text2.Substring(0, text2.IndexOf(')'));
				JsonTextParser jsonTextParser = new JsonTextParser();
				JsonObjectCollection jsonObjectCollection = (JsonObjectCollection)jsonTextParser.Parse(text2);
				IEnumerator<JsonObject> enumerator = jsonObjectCollection.GetEnumerator();
				while (enumerator.MoveNext())
				{
					JsonStringValue jsonStringValue = enumerator.Current as JsonStringValue;
					bool flag2 = jsonStringValue != null && !(jsonStringValue.Name != "token");
					if (flag2)
					{
						text = jsonStringValue.Value;
					}
				}
				bool flag3 = text == "";
				if (flag3)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					text = text.ToLower();
					string url2 = "https://login.360.cn/";
					string data = string.Format("src=pcw_wan&from=pcw_wan&charset=utf-8&requestScema=https&o=sso&m=login&lm=0&captFlag=1&rtype=data&validatelm=0&isKeepAlive=1&captchaApp=i360&userName={0}&type=normal&account={1}&password={2}&captcha=&token={3}&proxy=http%3A%2F%2Fwan.360.cn%2Fpsp_jump.html&callback=QiUserJsonp{4}&func=QiUserJsonp{5}", new object[]
					{
						this._username,
						this._username,
						this._md5_password_lower,
						text,
						CommonUtils.getMSecondsNow(),
						CommonUtils.getMSecondsNow()
					});
					bool flag4 = verify_code != null && verify_code != "";
					if (flag4)
					{
						data = string.Format("src=pcw_wan&from=pcw_wan&charset=utf-8&requestScema=https&o=sso&m=login&lm=0&captFlag=1&rtype=data&validatelm=0&isKeepAlive=1&captchaApp=i360&userName={0}&type=normal&account={1}&password={2}&captcha={3}&token={4}&proxy=http%3A%2F%2Fwan.360.cn%2Fpsp_jump.html&callback=QiUserJsonp{5}&func=QiUserJsonp{6}", new object[]
						{
							this._username,
							this._username,
							this._md5_password_lower,
							verify_code,
							text,
							CommonUtils.getMSecondsNow(),
							CommonUtils.getMSecondsNow()
						});
					}
					httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "", null);
					bool flag5 = httpResult == null;
					if (flag5)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						text2 = httpResult.getContent();
						string text3 = text2.Substring(text2.IndexOf("errno") + 6);
						text3 = text3.Substring(0, text3.IndexOf('&'));
						bool flag6 = long.Parse(text3) > 0L;
						if (flag6)
						{
							string text4 = text2.Substring(text2.IndexOf("errmsg") + 7);
							text4 = text4.Substring(0, text4.IndexOf('&'));
							string text5 = text2.Substring(text2.IndexOf("captcha.php"));
							text5 = text5.Substring(0, text5.IndexOf('&'));
							text5 = Uri.UnescapeDataString(text5);
							text4 = Uri.UnescapeDataString(text4);
							loginResult.StatusCode = LoginStatusCode.NeedVerifyCode;
							loginResult.ErrMessage = text4;
							loginResult.CaptchaUrl = "https://passport.360.cn/" + text5;
							loginResult.CaptchaImage = this._loginMgr.getCaptchaImage(loginResult.CaptchaUrl, ref cookies);
							loginResult.WebCookies = cookies;
							result = loginResult;
						}
						else
						{
							string text6 = text2.Substring(text2.IndexOf("s=") + 2);
							text6 = text6.Substring(0, text6.IndexOf('&'));
							string url3 = string.Format("http://login.1360.com/?o=sso&m=setcookie&func=QHPass.loginUtils.setCookieCallback&s={0}&callback=QiUserJsonP{1}", text6, CommonUtils.getMSecondsNow());
							httpResult = TransferMgr.doGetPure(url3, ref cookies, "", null);
							bool flag7 = httpResult == null;
							if (flag7)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								text2 = httpResult.getContent();
								text2 = text2.Substring(text2.IndexOf('(') + 1);
								text2 = text2.Substring(0, text2.IndexOf(')'));
								JsonObjectCollection jsonObjectCollection2 = (JsonObjectCollection)jsonTextParser.Parse(text2);
								IEnumerator<JsonObject> enumerator2 = jsonObjectCollection2.GetEnumerator();
								bool flag8 = false;
								while (enumerator2.MoveNext())
								{
									JsonStringValue jsonStringValue2 = enumerator2.Current as JsonStringValue;
									bool flag9 = jsonStringValue2 != null && jsonStringValue2.Name == "errmsg";
									if (flag9)
									{
										bool flag10 = jsonStringValue2.Value == "";
										if (flag10)
										{
											flag8 = true;
											break;
										}
										break;
									}
								}
								bool flag11 = !flag8;
								if (flag11)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
								}
								else
								{
									string redirecturl = string.Format("http://dock.wan.360.cn/game_login.php?server_id=S{0}&src=iwan-yxlist-astd&gamekey=astd", this._acc.ServerId);
									base.processRedirect(redirecturl, loginResult, ref cookies);
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
