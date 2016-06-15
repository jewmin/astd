using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class FunshionLogin : LoginImplBase
	{
		public FunshionLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://game.funshion.com/index/account/index.php";
			string data = string.Format("c=index&a=ajax_login&username={0}&password={1}&auto_login=0&cache={2}", this._username, this._password, CommonUtils.getMSecondsNow());
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
				bool flag2 = !content.Contains("ok");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string serverUrl = this.getServerUrl(ref cookies);
					string url2 = string.Format("http://game.funshion.com/index/game/index.php?c=web&a=enter&sid={0}&cache={1}", serverUrl, CommonUtils.getMSecondsNow());
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						Match match = new Regex("location\\.href=\"(?<url>.+?)\";", RegexOptions.IgnoreCase).Match(content2);
						bool flag4 = !match.Success;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInLogin;
							result = loginResult;
						}
						else
						{
							string value = match.Groups["url"].Value;
							base.processStartGame(value, loginResult, ref cookies, "");
							result = loginResult;
						}
					}
				}
			}
			return result;
		}

		private string getServerUrl(ref List<Cookie> cookies)
		{
			string url = "http://game.funshion.com/xf/22/";
			string filename = "as_funshion_server_list.html";
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
			string arg = string.Format("风行{0}区", this._acc.ServerId);
			Regex regex = new Regex(string.Format("value=\"(\\d+)\">{0}</option>", arg));
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
