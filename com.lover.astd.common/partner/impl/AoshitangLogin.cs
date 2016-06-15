using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System.Net.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class AoshitangLogin : LoginImplBase
	{
		public AoshitangLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://as.aoshitang.com/getsession.xhtml?x={0}&isgameLogin=true&top=1&gameid=as&source=", this._rand.NextDouble());
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
			url = string.Format("http://www.aoshitang.com/jsonp.action?jsoncallback=jsonp{0}&funName=checkLogin&x={1}", CommonUtils.getMSecondsNow(), this._rand.NextDouble());
			httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
			string url2 = string.Format("http://www.aoshitang.com/jsonp.action?jsoncallback=jsonp{0}&funName=indexLogin&username={1}&password={2}", CommonUtils.getMSecondsNow(), this._username, this._md5_password_lower);
			httpResult = TransferMgr.doGetPure(url2, ref cookies, "http://as.aoshitang.com/index.html", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				string text = httpResult.getContent();
				text = text.Substring(text.IndexOf('(') + 1);
				text = text.Substring(0, text.IndexOf(')'));
				JsonTextParser jsonTextParser = new JsonTextParser();
				JsonObjectCollection jsonObjectCollection = (JsonObjectCollection)jsonTextParser.Parse(text);
				IEnumerator<JsonObject> enumerator = jsonObjectCollection.GetEnumerator();
				string errMessage = "";
				bool flag2 = false;
				string text2 = "";
				string text3 = "";
				long num = 0L;
				while (enumerator.MoveNext())
				{
					bool flag3 = enumerator.Current.Name == "ts";
					if (flag3)
					{
						JsonNumericValue jsonNumericValue = enumerator.Current as JsonNumericValue;
						num = (long)jsonNumericValue.Value;
					}
					bool flag4 = enumerator.Current.Name == "success";
					if (flag4)
					{
						JsonBooleanValue jsonBooleanValue = enumerator.Current as JsonBooleanValue;
						flag2 = jsonBooleanValue.Value.Value;
					}
					bool flag5 = enumerator.Current.Name == "msg";
					if (flag5)
					{
						JsonStringValue jsonStringValue = enumerator.Current as JsonStringValue;
						errMessage = jsonStringValue.Value;
					}
					bool flag6 = enumerator.Current.Name == "key";
					if (flag6)
					{
						JsonStringValue jsonStringValue2 = enumerator.Current as JsonStringValue;
						text2 = jsonStringValue2.Value;
					}
					bool flag7 = enumerator.Current.Name == "current_user";
					if (flag7)
					{
						JsonObjectCollection jsonObjectCollection2 = enumerator.Current as JsonObjectCollection;
						StringWriter stringWriter = new StringWriter();
						stringWriter.Write("{");
						int num2;
						for (int i = 0; i < jsonObjectCollection2.Count; i = num2 + 1)
						{
							bool flag8 = i > 0;
							if (flag8)
							{
								stringWriter.Write(",");
							}
							jsonObjectCollection2[i].WriteTo(stringWriter);
							num2 = i;
						}
						stringWriter.Write("}");
						jsonObjectCollection2.Name = string.Empty;
						text3 = stringWriter.ToString();
					}
				}
				bool flag9 = !flag2 || text2 == "" || text3 == "";
				if (flag9)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					loginResult.ErrMessage = errMessage;
					result = loginResult;
				}
				else
				{
					url2 = "http://as.aoshitang.com/ssoLogin.xhtml";
					text3 = Uri.EscapeDataString(text3);
					text3 = text3.Replace("%20", "+");
					string data = string.Format("ts={0}&key={1}&userJson={2}&isgameLogin=true", num, text2, text3);
					httpResult = TransferMgr.doPostPure(url2, data, ref cookies, "http://as.aoshitang.com/index.html", new Dictionary<string, string>
					{
						{
							"X-Requested-With",
							"XMLHttpRequest"
						}
					});
					bool flag10 = httpResult == null;
					if (flag10)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						text = httpResult.getContent();
						JsonObjectCollection jsonObjectCollection3 = (JsonObjectCollection)jsonTextParser.Parse(text);
						IEnumerator<JsonObject> enumerator2 = jsonObjectCollection3.GetEnumerator();
						while (enumerator2.MoveNext())
						{
							bool flag11 = enumerator2.Current.Name == "playerid";
							if (flag11)
							{
								JsonStringValue jsonStringValue3 = enumerator2.Current as JsonStringValue;
								string value = jsonStringValue3.Value;
								bool flag12 = value == "";
								if (flag12)
								{
									loginResult.StatusCode = LoginStatusCode.FailInLogin;
									result = loginResult;
									return result;
								}
							}
						}
						string url3 = string.Format("http://as.aoshitang.com/gm.xhtml?gid=as&server={0}", this._acc.ServerId);
						httpResult = TransferMgr.doGetPure(url3, ref cookies, "", null);
						bool flag13 = httpResult == null;
						if (flag13)
						{
							loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
							result = loginResult;
						}
						else
						{
							text = httpResult.getContent();
							Match match = Regex.Match(text, ".*?window\\.location\\.href.*?=.*?\"([^\"']*?)\".*");
							bool flag14 = match == null || match.Groups == null || match.Groups.Count < 2;
							if (flag14)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
								result = loginResult;
							}
							else
							{
								string value2 = match.Groups[1].Value;
								base.processStartGame(value2, loginResult, ref cookies, "");
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
