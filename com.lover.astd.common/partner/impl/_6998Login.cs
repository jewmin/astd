using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _6998Login : LoginImplBase
	{
		public _6998Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "https://passport.6998.com/login";
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
				string text = httpResult.getContent();
				Regex regex = new Regex(".*?[\"']+?(LT-\\w+?-\\w+?)[\"']+?.*?");
				Match match = regex.Match(text);
				bool flag2 = match == null || match.Groups == null || match.Groups.Count < 2;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string value = match.Groups[1].Value;
					base.logging();
					string url2 = "https://passport.6998.com/login";
					string data = string.Format("hidUrl=&hidLoginTimes=&Username={0}&Password={1}&OTP=&LT={2}&LoginView=Login&UserLoginFrom=0&IsAccount=1", this._username, this._password, value);
					bool flag3 = verify_code != null;
					if (flag3)
					{
						data = string.Format("hidUrl=&hidLoginTimes=&Username={0}&Password={1}&OTP=&VerifyCode={2}&CaptchaFileName={3}&LT={4}&LoginView=Login&UserLoginFrom=0&IsAccount=1", new object[]
						{
							this._username,
							this._password,
							verify_code,
							extra,
							value
						});
					}
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
						bool flag5 = content.Contains("多次登录失败") || content.Contains("验证码输入错误");
						if (flag5)
						{
							regex = new Regex("<input type=\"hidden\" name=\"CaptchaFileName\" id=\"CaptchaFileName\" value=\"([^\"]+)\"");
							match = regex.Match(content);
							bool flag6 = match == null || match.Groups == null || match.Groups.Count < 2;
							if (flag6)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGetToken;
								result = loginResult;
							}
							else
							{
								string value2 = match.Groups[1].Value;
								loginResult.StatusCode = LoginStatusCode.NeedVerifyCode;
								loginResult.CaptchaExtra = value2;
								loginResult.CaptchaUrl = string.Format("https://api.6998.com/1/csec/captcha_show?filename={0}&rand={1}", value2, this._rand.NextDouble());
								loginResult.CaptchaImage = this._loginMgr.getCaptchaImage(loginResult.CaptchaUrl, ref cookies);
								result = loginResult;
							}
						}
						else
						{
							bool flag7 = !content.Contains("JSONP.src");
							if (flag7)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								base.findingServerUrl();
								url = "https://passport.6998.com/api/GetClientToken?jsoncallback=updatestart&rand=" + this._rand.NextDouble();
								httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
								bool flag8 = httpResult == null;
								if (flag8)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGetToken;
									result = loginResult;
								}
								else
								{
									text = httpResult.getContent();
									text = text.Replace("updatestart(\"", "");
									text = text.Replace("\")", "");
									string arg = text;
									string text2 = string.Format("https://passport.6998.com/jump?clienttoken={0}&service=http://account.6998.com/mygame_login_astd-3-0.html", arg);
									httpResult = TransferMgr.doGetPure(text2, ref cookies, "", null);
									bool flag9 = httpResult == null;
									if (flag9)
									{
										loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
										result = loginResult;
									}
									else
									{
										string base_url = text2;
										text2 = httpResult.getExtraHeader("Location");
										base.makeSureValidUri(base_url, ref text2);
										bool flag10 = text2 == null || text2 == "";
										if (flag10)
										{
											loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
											result = loginResult;
										}
										else
										{
											httpResult = TransferMgr.doGetPure(text2, ref cookies, "", null);
											bool flag11 = httpResult == null;
											if (flag11)
											{
												loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
												result = loginResult;
											}
											else
											{
												base.goingToGameUrl();
												string value4;
												string extraHeader;
												while (true)
												{
													text = httpResult.getContent();
													regex = new Regex(".*?JSONP\\.src.*?=.*?[\"']+?([^\"']*?)[\"']+?.*");
													match = regex.Match(text);
													bool flag12 = match == null || match.Groups == null || match.Groups.Count < 2;
													if (flag12)
													{
														break;
													}
													string value3 = match.Groups[1].Value;
													regex = new Regex(".*?rCallbackURL.*?=.*?[\"']+?([^\"']*?)[\"']+?.*");
													match = regex.Match(text);
													bool flag13 = match == null || match.Groups == null || match.Groups.Count < 2;
													if (flag13)
													{
														goto Block_23;
													}
													value4 = match.Groups[1].Value;
													base.makeSureValidUri(text2, ref value3);
													base.makeSureValidUri(text2, ref value4);
													bool flag14 = TransferMgr.doGetPure(value3 + "&rand=" + this._rand.NextDouble(), ref cookies, "", null) == null;
													if (flag14)
													{
														goto Block_24;
													}
													httpResult = TransferMgr.doGetPure(value4, ref cookies, "", null);
													bool flag15 = httpResult == null;
													if (flag15)
													{
														goto Block_25;
													}
													base.gettingSession();
													bool flag16 = httpResult.StatusCode != 302;
													if (flag16)
													{
														goto Block_26;
													}
													extraHeader = httpResult.getExtraHeader("Location");
													base.makeSureValidUri(value4, ref extraHeader);
													httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
													bool flag17 = httpResult == null;
													if (flag17)
													{
														goto Block_27;
													}
													bool flag18 = httpResult.StatusCode != 302;
													if (flag18)
													{
														goto Block_28;
													}
													string extraHeader2 = httpResult.getExtraHeader("Location");
													httpResult = TransferMgr.doGetPure(extraHeader2, ref cookies, "", null);
													bool flag19 = httpResult == null;
													if (flag19)
													{
														goto Block_29;
													}
												}
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
												result = loginResult;
												return result;
												Block_23:
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
												result = loginResult;
												return result;
												Block_24:
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
												result = loginResult;
												return result;
												Block_25:
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
												result = loginResult;
												return result;
												Block_26:
												text = httpResult.getContent();
												regex = new Regex("<frame.*?src=[\"']+?(.*?start\\.action.*?)[\"']+?.*");
												match = regex.Match(text);
												bool flag20 = match == null || match.Groups == null || match.Groups.Count < 2;
												if (flag20)
												{
													loginResult.StatusCode = LoginStatusCode.FailInGetSession;
													result = loginResult;
													return result;
												}
												string value5 = match.Groups[1].Value;
												base.processStartGame(value5, loginResult, ref cookies, value4);
												result = loginResult;
												return result;
												Block_27:
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
												result = loginResult;
												return result;
												Block_28:
												text = httpResult.getContent();
												regex = new Regex("<frame.*?src=[\"']+?(.*?start\\.action.*?)[\"']+?.*");
												match = regex.Match(text);
												bool flag21 = match == null || match.Groups == null || match.Groups.Count < 2;
												if (flag21)
												{
													loginResult.StatusCode = LoginStatusCode.FailInGetSession;
													result = loginResult;
													return result;
												}
												string text3 = match.Groups[1].Value;
												text3 = text3.Replace("&amp;", "&");
												base.processStartGame(text3, loginResult, ref cookies, extraHeader);
												result = loginResult;
												return result;
												Block_29:
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
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
