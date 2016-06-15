using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System.Net.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.partner.impl
{
	public class _53wanLogin : LoginImplBase
	{
		public _53wanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override bool alwaysNeedVerifyCode()
		{
			return true;
		}

		public override string getAlwaysCaptchaImageUrl()
		{
			return "http://www.53wan.com/getCerification.do?login=1&r=" + this._rand.NextDouble();
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			bool flag = verify_code == null;
			LoginResult result;
			if (flag)
			{
				base.buildAlwaysVerifyResult(ref loginResult, ref cookies);
				result = loginResult;
			}
			else
			{
				string url = "http://www.53wan.com/login.do";
				string data = string.Format("userName={0}&password={1}&validCode={2}&flag=1&marks=0&RTN_CUSTOM=rtn_as", this._username, this._md5_password_lower, verify_code);
				HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
				bool flag2 = httpResult == null;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string content = httpResult.getContent();
					bool flag3 = content.Contains("alert");
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						base.goingToGameUrl();
						string url2 = "http://www.53wan.com/jumpServer.do";
						data = string.Format("gameId=23&serverId={0}", this._acc.ServerId);
						httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "", null);
						bool flag4 = httpResult == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGetSession;
							result = loginResult;
						}
						else
						{
							content = httpResult.getContent();
							JsonTextParser jsonTextParser = new JsonTextParser();
							JsonObjectCollection jsonObjectCollection = (JsonObjectCollection)jsonTextParser.Parse(content);
							IEnumerator<JsonObject> enumerator = jsonObjectCollection.GetEnumerator();
							string text = "";
							while (enumerator.MoveNext())
							{
								JsonStringValue jsonStringValue = enumerator.Current as JsonStringValue;
								bool flag5 = jsonStringValue != null && !(jsonStringValue.Name != "returnMsg");
								if (flag5)
								{
									text = jsonStringValue.Value;
								}
							}
							bool flag6 = text == "";
							if (flag6)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGetSession;
								result = loginResult;
							}
							else
							{
								base.processStartGame(text, loginResult, ref cookies, "");
								result = loginResult;
							}
						}
					}
				}
			}
			return result;
		}
	}
}
