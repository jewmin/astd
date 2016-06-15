using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class HuanlangLogin : LoginImplBase
	{
		public HuanlangLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://wwwv3.huanlang.com/Handlers/LoginHandlerGetJOSN.ashx?jsoncallback=jsonp{0}&uname={1}&Pwd={2}&gameid=19", CommonUtils.getMSecondsNow(), this._username, this._password);
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
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
				text = text.Substring(text.IndexOf(':') + 1);
				text = text.Substring(0, text.IndexOf('}'));
				bool flag2 = text.Equals("0");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					base.findingServerUrl();
					string url2 = "http://astd.huanlang.com/server_list.aspx";
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
						result = loginResult;
					}
					else
					{
						string content = httpResult.getContent();
						string arg = (this._acc.ServerId >= 10) ? this._acc.ServerId.ToString() : string.Format("0{0}", this._acc.ServerId);
						string arg2 = string.Format("双线{0}服", arg);
						Regex regex = new Regex(string.Format("<a.*href=\"([^\"']*?)\".*>{0}.*</a>", arg2));
						MatchCollection matchCollection = regex.Matches(content);
						bool flag4 = matchCollection == null || matchCollection.Count == 0 || matchCollection[0] == null;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							bool flag5 = matchCollection[0].Groups == null || matchCollection[0].Groups.Count < 2;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
								result = loginResult;
							}
							else
							{
								string value = matchCollection[0].Groups[1].Value;
								base.processRedirect(value, loginResult, ref cookies);
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
