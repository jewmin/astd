using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class YaoWanLogin : LoginImplBase
	{
		public YaoWanLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://www.yaowan.com/?m=user&action=loginform&subdomain=as";
			string data = string.Format("username={0}&password={1}", this._username, this._password);
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
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
					base.findingServerUrl();
					string serverUrl = this.getServerUrl(ref cookies);
					bool flag3 = serverUrl.Length == 0;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
						result = loginResult;
					}
					else
					{
						base.processRedirect(serverUrl, loginResult, ref cookies);
						result = loginResult;
					}
				}
			}
			return result;
		}

		private string getServerUrl(ref List<Cookie> cookies)
		{
			string url = "http://as.yaowan.com/as_server_list.html";
			string filename = "as_yaowan_server_list.html";
			string text = base.getCacheFile(filename);
			bool flag = text.Length == 0;
			string result;
			if (flag)
			{
				HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
				bool flag2 = httpResult == null;
				if (flag2)
				{
					result = "";
				}
				else
				{
					text = httpResult.getContent();
					string text2 = this.findServerUrlFromString(text);
					bool flag3 = text2.Length > 0;
					if (flag3)
					{
						base.saveCacheFile(filename, text);
						result = text2;
					}
					else
					{
						result = "";
					}
				}
			}
			else
			{
				string text3 = this.findServerUrlFromString(text);
				bool flag4 = text3.Length > 0;
				if (flag4)
				{
					result = text3;
				}
				else
				{
					HttpResult httpResult2 = TransferMgr.doGetPure(url, ref cookies, "", null);
					bool flag5 = httpResult2 == null;
					if (flag5)
					{
						result = "";
					}
					else
					{
						text = httpResult2.getContent();
						text3 = this.findServerUrlFromString(text);
						bool flag6 = text3.Length > 0;
						if (flag6)
						{
							base.saveCacheFile(filename, text);
							result = text3;
						}
						else
						{
							result = "";
						}
					}
				}
			}
			return result;
		}

		private string findServerUrlFromString(string content)
		{
			string arg = (this._acc.ServerId == 500) ? "虎贲营" : string.Format("双线{0}区", this._acc.ServerId);
			Regex regex = new Regex(string.Format("<a.*href=\"([^\"']*?)\".*>{0}.*</a>", arg));
			Match match = regex.Match(content);
			bool flag = match == null || match.Groups == null || match.Groups.Count < 2;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				result = match.Groups[1].Value;
			}
			return result;
		}
	}
}
