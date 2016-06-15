using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class IspeakLogin : LoginImplBase
	{
		public IspeakLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.iugame.cn/common/Login.ashx";
			string data = string.Format("uid={0}&pwd={1}", this._username, this._md5_password_lower);
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "http://www.iugame.cn/astd/fwq.aspx", null);
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
				bool flag2 = content.IndexOf("alert") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					bool flag3 = httpResult.getCookies().Count == 0;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string text = "";
						foreach (Cookie current in httpResult.getCookies())
						{
							bool flag4 = current != null && current.Name == "uscode";
							if (flag4)
							{
								text = current.Value;
								break;
							}
						}
						bool flag5 = text == "";
						if (flag5)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							string url2 = string.Format("http://gameroom1.iugame.cn/play.ashx?iusiteid=1&s={0}&gname=2&sname=270", text);
							httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
							bool flag6 = httpResult == null;
							if (flag6)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGetSession;
								result = loginResult;
							}
							else
							{
								string text2 = httpResult.getExtraHeader("Location");
								bool flag7 = text2 == null;
								if (flag7)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGetSession;
									result = loginResult;
								}
								else
								{
									bool flag8 = !text2.StartsWith("http");
									if (flag8)
									{
										text2 = "http://gameroom1.iugame.cn" + text2;
									}
									httpResult = TransferMgr.doGetPure(text2, ref cookies, "", null);
									bool flag9 = httpResult == null;
									if (flag9)
									{
										loginResult.StatusCode = LoginStatusCode.FailInGetSession;
										result = loginResult;
									}
									else
									{
										text2 = httpResult.getExtraHeader("Location");
										bool flag10 = text2 == null;
										if (flag10)
										{
											loginResult.StatusCode = LoginStatusCode.FailInGetSession;
											result = loginResult;
										}
										else
										{
											base.processRedirect(text2, loginResult, ref cookies);
											result = loginResult;
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
