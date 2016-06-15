using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class PeiyouLogin : LoginImplBase
	{
		public PeiyouLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = "http://as.peiyou.com/Default.php?m=user&action=loginform&astd=1";
			string data = string.Format("username={0}&password={1}&submit=%E7%99%BB%E5%BD%95%E5%B8%90%E5%8F%B7%3E%3E", this._acc.UserName, this._acc.Password);
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
			LoginResult result;
			if (httpResult == null)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				string content = httpResult.getContent();
				if (content.IndexOf("alert") >= 0)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					base.findingServerUrl();
					string serverUrl = this.getServerUrl(ref cookies);
					if (serverUrl.Length == 0)
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

		private string translateNumber(int number)
		{
			string[] array = new string[]
			{
				"",
				"一",
				"二",
				"三",
				"四",
				"五",
				"六",
				"七",
				"八",
				"九"
			};
			string result = number.ToString();
			if (number < 100)
			{
				if (number < 10)
				{
					result = array[number];
				}
				else
				{
					int num = number / 10;
					int num2 = number % 10;
                    result = "十" + array[num2];
                    if (num > 1)
                    {
                        result = array[num] + result;
                    }
				}
			}
			return result;
		}

		private string getServerUrl(ref List<Cookie> cookies)
		{
			string url = "http://as.peiyou.com/as_server_list.html";
			string filename = "as_peiyou_server_list.html";
			string text = base.getCacheFile(filename);
			string result;
			if (text.Length == 0)
			{
				HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
				if (httpResult == null)
				{
					result = "";
				}
				else
				{
					text = httpResult.getContent();
					string text2 = this.findServerUrlFromString(text);
					if (text2.Length > 0)
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
				if (text3.Length > 0)
				{
					result = text3;
				}
				else
				{
					HttpResult httpResult2 = TransferMgr.doGetPure(url, ref cookies, "", null);
					if (httpResult2 == null)
					{
						result = "";
					}
					else
					{
						text = httpResult2.getContent();
						text3 = this.findServerUrlFromString(text);
						if (text3.Length > 0)
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
			string arg = string.Format("双线{0}区", this.translateNumber(this._acc.ServerId));
			Regex regex = new Regex(string.Format("<a.*href=\"([^\"']*?)\".*>{0}.*</a>", arg));
			Match match = regex.Match(content);
			string result;
			if (match == null || match.Groups == null || match.Groups.Count < 2)
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
