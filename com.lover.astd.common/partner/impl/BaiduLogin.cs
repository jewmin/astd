using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class BaiduLogin : LoginImplBase
	{
		public BaiduLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("https://passport.baidu.com/v2/api/?getapi&tpl=mn&apiver=v3&tt={0}&class=login&logintype=dialogLogin&callback=bd__cbs__apsigl", CommonUtils.getMSecondsNow());
			bool flag = TransferMgr.doGetPure(url, ref cookies, "", null) == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInGetToken;
				result = loginResult;
			}
			else
			{
				HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
				bool flag2 = httpResult == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string content = httpResult.getContent();
					Regex regex = new Regex("\"token\"\\s*:\\s*\"(.*?)\"");
					Match match = regex.Match(content);
					bool flag3 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetToken;
						result = loginResult;
					}
					else
					{
						string value = match.Groups[1].Value;
						bool flag4 = value == null || value == "";
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGetToken;
							result = loginResult;
						}
						else
						{
							string url2 = "https://passport.baidu.com/v2/api/?login";
							string data = string.Format("staticpage=http%3A%2F%2Fwww.baidu.com%2Fcache%2Fuser%2Fhtml%2Fv3Jump.html&charset=UTF-8&token={0}&tpl=mn&apiver=v3&tt={1}&codestring={2}&safeflg=0&u=http%3A%2F%2Fwww.baidu.com%2F&isPhone=false&quick_user=0&loginmerge=true&logintype=dialogLogin&splogin=rate&username={3}&password={4}&verifycode={5}&mem_pass=on&ppui_logintime=180209&callback=parent.bd__pcbs__uvdb8z", new object[]
							{
								value,
								CommonUtils.getMSecondsNow(),
								(extra == null) ? "" : extra,
								this._username,
								this._password,
								(verify_code == null) ? "" : verify_code
							});
							httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								content = httpResult.getContent();
								bool flag6 = !content.Contains("err_no=0");
								if (flag6)
								{
									bool flag7 = !content.Contains("&codeString=");
									if (flag7)
									{
										loginResult.StatusCode = LoginStatusCode.FailInLogin;
										result = loginResult;
									}
									else
									{
										regex = new Regex("&codeString=([^=&]*?)&");
										match = regex.Match(content);
										bool flag8 = match == null || match.Groups == null || match.Groups.Count < 2;
										if (flag8)
										{
											loginResult.StatusCode = LoginStatusCode.FailInGetToken;
											result = loginResult;
										}
										else
										{
											string value2 = match.Groups[1].Value;
											loginResult.StatusCode = LoginStatusCode.NeedVerifyCode;
											loginResult.ErrMessage = "需要输入验证码";
											loginResult.CaptchaUrl = "https://passport.baidu.com/cgi-bin/genimage?" + value2;
											loginResult.CaptchaExtra = value2;
											loginResult.CaptchaImage = this._loginMgr.getCaptchaImage(loginResult.CaptchaUrl, ref cookies);
											loginResult.WebCookies = cookies;
											result = loginResult;
										}
									}
								}
								else
								{
									string url3 = string.Format("http://youxi.baidu.com/login_game_by_post.xhtml?id=177&serverId=S{0}&frameFlag=true&source=31", this._acc.ServerId);
									httpResult = TransferMgr.doGetPure(url3, ref cookies, "", null);
									bool flag9 = httpResult == null;
									if (flag9)
									{
										loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
										result = loginResult;
									}
									else
									{
										content = httpResult.getContent();
										regex = new Regex("oForm\\.action\\s*?=\\s*?\"(http:\\/\\/s.*?)\"");
										match = regex.Match(content);
										bool flag10 = match == null || match.Groups == null || match.Groups.Count < 2;
										if (flag10)
										{
											loginResult.StatusCode = LoginStatusCode.FailInGetToken;
											result = loginResult;
										}
										else
										{
											string value3 = match.Groups[1].Value;
											regex = new Regex("oInput\\.value\\s*?=\\s*?\"([^\"']*?)\"");
											MatchCollection matchCollection = regex.Matches(content);
											bool flag11 = matchCollection == null || matchCollection.Count < 10;
											if (flag11)
											{
												loginResult.StatusCode = LoginStatusCode.FailInGetToken;
												result = loginResult;
											}
											else
											{
												string value4 = matchCollection[0].Groups[1].Value;
												string value5 = matchCollection[1].Groups[1].Value;
												string value6 = matchCollection[2].Groups[1].Value;
												string value7 = matchCollection[3].Groups[1].Value;
												string value8 = matchCollection[4].Groups[1].Value;
												string value9 = matchCollection[5].Groups[1].Value;
												string text = matchCollection[6].Groups[1].Value;
												string value10 = matchCollection[7].Groups[1].Value;
												string value11 = matchCollection[8].Groups[1].Value;
												string value12 = matchCollection[9].Groups[1].Value;
												text = Uri.EscapeDataString(text);
												text = text.Replace("%20", "+");
												string data2 = string.Format("user_id={0}&api_key={1}&server_id={2}&cm_flag={3}&timestamp={4}&sign={5}", new object[]
												{
													value4,
													value5,
													value6,
													value9,
													text,
													value11
												});
												base.postStartGame(value3, data2, loginResult, ref cookies, "");
												result = loginResult;
											}
										}
									}
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
