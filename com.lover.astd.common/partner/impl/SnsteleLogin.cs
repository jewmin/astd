using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class SnsteleLogin : LoginImplBase
	{
		public SnsteleLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			string text = "http://www.snstele.com/index.php";
			HttpResult httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInGetToken;
				result = loginResult;
			}
			else
			{
				string content = httpResult.getContent();
				Regex regex = new Regex("action=\"(do\\.php\\?ac=[^\"]*)\"");
				Match match = regex.Match(content);
				bool flag2 = match == null || match.Groups == null || match.Groups.Count < 2;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInGetToken;
					result = loginResult;
				}
				else
				{
					string value = match.Groups[1].Value;
					base.makeSureValidUri(text, ref value);
					regex = new Regex("name=\"formhash\" value=\"([^\"]*)\"");
					match = regex.Match(content);
					bool flag3 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetToken;
						result = loginResult;
					}
					else
					{
						string value2 = match.Groups[1].Value;
						base.logging();
						string data = string.Format("username={0}&password={1}&refer=space.php%3Fdo%3Dhome&loginsubmit=%E7%99%BB+%E5%BD%95&formhash={2}", this._username, this._password, value2);
						bool flag4 = TransferMgr.doPostPure(value, data, ref cookies, "", null) == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							bool flag5 = false;
							int i = 0;
							while (i < cookies.Count)
							{
								Cookie cookie = cookies[i];
								bool flag6 = cookie.Name == "uchome_auth";
								if (flag6)
								{
									bool flag7 = cookie.Value != "deleted";
									if (flag7)
									{
										flag5 = true;
										break;
									}
									break;
								}
								else
								{
									int num = i;
									i = num + 1;
								}
							}
							bool flag8 = !flag5;
							if (flag8)
							{
								loginResult.StatusCode = LoginStatusCode.FailInLogin;
								result = loginResult;
							}
							else
							{
								string url = string.Format("http://www.snstele.com/space.php?do=game&gtype=11&game_area={0}", this._acc.ServerId);
								httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
								bool flag9 = httpResult == null;
								if (flag9)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGetSession;
									result = loginResult;
								}
								else
								{
									string extraHeader = httpResult.getExtraHeader("Location");
									bool flag10 = extraHeader == null || extraHeader == "";
									if (flag10)
									{
										loginResult.StatusCode = LoginStatusCode.FailInGetSession;
										result = loginResult;
									}
									else
									{
										httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
										bool flag11 = httpResult == null;
										if (flag11)
										{
											loginResult.StatusCode = LoginStatusCode.FailInGetSession;
											result = loginResult;
										}
										else
										{
											extraHeader = httpResult.getExtraHeader("Location");
											bool flag12 = extraHeader == null || extraHeader == "";
											if (flag12)
											{
												loginResult.StatusCode = LoginStatusCode.FailInGetSession;
												result = loginResult;
											}
											else
											{
												httpResult = TransferMgr.doGetPure(extraHeader, ref cookies, "", null);
												bool flag13 = httpResult == null;
												if (flag13)
												{
													loginResult.StatusCode = LoginStatusCode.FailInGetSession;
													result = loginResult;
												}
												else
												{
													content = httpResult.getContent();
													string url2 = "";
													string text2 = "";
													string text3 = "";
													string text4 = "";
													string text5 = "";
													string text6 = "";
													string text7 = "";
													string text8 = "";
													string text9 = "";
													regex = new Regex("action=\"([^\"]+)\"");
													match = regex.Match(content);
													bool flag14 = match == null || match.Groups == null || match.Groups.Count < 2;
													if (flag14)
													{
														loginResult.StatusCode = LoginStatusCode.FailInGetToken;
														result = loginResult;
													}
													else
													{
														url2 = match.Groups[1].Value;
														regex = new Regex("name=\"([^\"]+)\" value=\"([^\"]+)\"");
														MatchCollection matchCollection = regex.Matches(content);
														foreach (Match match2 in matchCollection)
														{
															bool flag15 = match2.Groups != null && match2.Groups.Count >= 3;
															if (flag15)
															{
																string value3 = match2.Groups[1].Value;
																string value4 = match2.Groups[2].Value;
																bool flag16 = value3 == "accid";
																if (flag16)
																{
																	text2 = value4;
																}
																else
																{
																	bool flag17 = value3 == "accname";
																	if (flag17)
																	{
																		text3 = value4;
																	}
																	else
																	{
																		bool flag18 = value3 == "serverid";
																		if (flag18)
																		{
																			text4 = value4;
																		}
																		else
																		{
																			bool flag19 = value3 == "tstamp";
																			if (flag19)
																			{
																				text5 = value4;
																			}
																			else
																			{
																				bool flag20 = value3 == "fcm";
																				if (flag20)
																				{
																					text6 = value4;
																				}
																				else
																				{
																					bool flag21 = value3 == "ticket";
																					if (flag21)
																					{
																						text7 = value4;
																					}
																					else
																					{
																						bool flag22 = value3 == "game_type";
																						if (flag22)
																						{
																							text8 = value4;
																						}
																						else
																						{
																							bool flag23 = value3 == "game_area";
																							if (flag23)
																							{
																								text9 = value4;
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
														data = string.Format("accid={0}&accname={1}&serverid={2}&tstamp={3}&fcm={4}&ticket={5}&game_type={6}&game_area={7}", new object[]
														{
															text2,
															text3,
															text4,
															text5,
															text6,
															text7,
															text8,
															text9
														});
														httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "", null);
														bool flag24 = httpResult == null;
														if (flag24)
														{
															loginResult.StatusCode = LoginStatusCode.FailInGetSession;
															result = loginResult;
														}
														else
														{
															content = httpResult.getContent();
															regex = new Regex("\"(http://s\\d+\\.astd\\.snstele\\.com/root/start\\.action[^\"]*)\"");
															match = regex.Match(content);
															bool flag25 = match == null || match.Groups == null || match.Groups.Count < 2;
															if (flag25)
															{
																loginResult.StatusCode = LoginStatusCode.FailInGetToken;
																result = loginResult;
															}
															else
															{
																string value5 = match.Groups[1].Value;
																base.processStartGame(value5, loginResult, ref cookies, "");
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
						}
					}
				}
			}
			return result;
		}
	}
}
