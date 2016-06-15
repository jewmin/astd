using com.lover.astd.common.config;
using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using com.lover.astd.common.partner;
using com.lover.astd.common.partner.impl.sf;
using com.lover.common;
using com.lover.common.http;
using System.Net.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace com.lover.astd.common
{
	public class UserSessionMgr
	{
		private static UserSessionMgr _instance = new UserSessionMgr();

		private string _host = GlobalConfig.HomeUrl;

		private bool _isSf;

		private int _userId;

		private string _userAuth;

		private int _accId;

		private AccountData _account = new AccountData();

		public static UserSessionMgr instance
		{
			get
			{
				return UserSessionMgr._instance;
			}
		}

		public bool is_init_from_args
		{
			get
			{
				return this._userId > 0;
			}
		}

		public bool isSf
		{
			get
			{
				return this._isSf;
			}
			set
			{
				this._isSf = value;
			}
		}

		public int UserId
		{
			get
			{
				return this._userId;
			}
			set
			{
				this._userId = value;
			}
		}

		public string UserAuth
		{
			get
			{
				return this._userAuth;
			}
			set
			{
				this._userAuth = value;
			}
		}

		public int AccId
		{
			get
			{
				return this._accId;
			}
			set
			{
				this._accId = value;
			}
		}

		public AccountData Account
		{
			get
			{
				return this._account;
			}
		}

		public string getSfConfig()
		{
			string url = string.Format("{0}/api/get_sfconf", this._host);
			string data = string.Format("uid={0}&uauth={1}&accid={2}", this._userId, this._userAuth, this._accId);
			List<Cookie> list = new List<Cookie>();
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref list, "", null);
			bool flag = httpResult.StatusCode != 200;
			string result;
			if (flag)
			{
				result = "http访问失败";
			}
			else
			{
				string content = httpResult.getContent();
				JsonTextParser jsonTextParser = new JsonTextParser();
				JsonObjectCollection jsonObjectCollection = jsonTextParser.Parse(content) as JsonObjectCollection;
				JsonBooleanValue jsonBooleanValue = jsonObjectCollection["success"] as JsonBooleanValue;
				bool flag2 = !jsonBooleanValue.Value.Value;
				if (flag2)
				{
					JsonStringValue jsonStringValue = jsonObjectCollection["msg"] as JsonStringValue;
					result = jsonStringValue.Value;
				}
				else
				{
					JsonObjectCollection jsonObjectCollection2 = jsonObjectCollection["msg"] as JsonObjectCollection;
					JsonStringValue jsonStringValue2 = jsonObjectCollection2["login_url"] as JsonStringValue;
					string value = jsonStringValue2.Value;
					JsonStringValue jsonStringValue3 = jsonObjectCollection2["game_url"] as JsonStringValue;
					string value2 = jsonStringValue3.Value;
					JsonStringValue jsonStringValue4 = jsonObjectCollection2["server_name"] as JsonStringValue;
					string value3 = jsonStringValue4.Value;
					JsonNumericValue jsonNumericValue = jsonObjectCollection2["server_id"] as JsonNumericValue;
					int num = (int)jsonNumericValue.Value;
					JsonStringValue jsonStringValue5 = jsonObjectCollection2["account_name"] as JsonStringValue;
					string value4 = jsonStringValue5.Value;
					JsonStringValue jsonStringValue6 = jsonObjectCollection2["account_password"] as JsonStringValue;
					string value5 = jsonStringValue6.Value;
					JsonStringValue jsonStringValue7 = jsonObjectCollection2["role_name"] as JsonStringValue;
					string value6 = jsonStringValue7.Value;
					this._account = new AccountData();
					this._account.setServerName(string.Format("{0}-{1}区", value3, num));
					this._account.UserName = value4;
					this._account.Password = value5;
					this._account.ServerId = num;
					this._account.RoleName = value6;
					LoginMgr loginMgr = new LoginMgr();
					SfLogin sfLogin = new SfLogin(loginMgr);
					sfLogin.setAccount(this._account);
					List<Cookie> list2 = new List<Cookie>();
					LoginResult loginResult = sfLogin.login(ref list2, value, value2);
					bool flag3 = loginResult.StatusCode > LoginStatusCode.Success;
					if (flag3)
					{
						result = "登录失败, 如果确认[用户名/密码/服务器/区号]都没有问题, 请重试, 如果重试10次以上都不行, 请联系管理员";
					}
					else
					{
						this._account.GameUrl = loginResult.GameUrl;
						this._account.JsessionId = loginResult.JSessionID;
						this._account.Cookies = loginResult.WebCookies;
						this._account.GameConf = new GameConfig(value3, num, value6);
						result = "";
					}
				}
			}
			return result;
		}

		public string getSession()
		{
			string url = string.Format("{0}/api/get_session", this._host);
			string data = string.Format("uid={0}&uauth={1}&accid={2}", this._userId, this._userAuth, this._accId);
			List<Cookie> list = new List<Cookie>();
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref list, "", null);
			bool flag = httpResult.StatusCode != 200;
			string result;
			if (flag)
			{
				result = "http访问失败";
			}
			else
			{
				string content = httpResult.getContent();
				JsonTextParser jsonTextParser = new JsonTextParser();
				JsonObjectCollection jsonObjectCollection = jsonTextParser.Parse(content) as JsonObjectCollection;
				JsonBooleanValue jsonBooleanValue = jsonObjectCollection["success"] as JsonBooleanValue;
				bool flag2 = !jsonBooleanValue.Value.Value;
				if (flag2)
				{
					JsonStringValue jsonStringValue = jsonObjectCollection["msg"] as JsonStringValue;
					result = jsonStringValue.Value;
				}
				else
				{
					JsonObjectCollection jsonObjectCollection2 = jsonObjectCollection["msg"] as JsonObjectCollection;
					JsonStringValue jsonStringValue2 = jsonObjectCollection2["server_type"] as JsonStringValue;
					string value = jsonStringValue2.Value;
					ServerType serverType = EnumString.getServerType(value);
					JsonNumericValue jsonNumericValue = jsonObjectCollection2["server_id"] as JsonNumericValue;
					int num = (int)jsonNumericValue.Value;
					JsonStringValue jsonStringValue3 = jsonObjectCollection2["account_name"] as JsonStringValue;
					string value2 = jsonStringValue3.Value;
					JsonStringValue jsonStringValue4 = jsonObjectCollection2["account_password"] as JsonStringValue;
					string value3 = jsonStringValue4.Value;
					JsonStringValue jsonStringValue5 = jsonObjectCollection2["role_name"] as JsonStringValue;
					string value4 = jsonStringValue5.Value;
					JsonStringValue jsonStringValue6 = jsonObjectCollection2["web_cookies"] as JsonStringValue;
					string text = jsonStringValue6.Value;
					text = Encoding.UTF8.GetString(Convert.FromBase64String(text));
					JsonStringValue jsonStringValue7 = jsonObjectCollection2["jsession_id"] as JsonStringValue;
					string value5 = jsonStringValue7.Value;
					JsonStringValue jsonStringValue8 = jsonObjectCollection2["game_url"] as JsonStringValue;
					string value6 = jsonStringValue8.Value;
					JsonStringValue jsonStringValue9 = jsonObjectCollection2["config"] as JsonStringValue;
					string text2 = jsonStringValue9.Value;
					text2 = Encoding.UTF8.GetString(Convert.FromBase64String(text2));
					this._account = new AccountData();
					this._account.UserName = value2;
					this._account.Password = value3;
					this._account.Server_type = serverType;
					this._account.ServerId = num;
					this._account.RoleName = value4;
					this._account.Cookies = CommonUtils.generateCookies(text);
					this._account.JsessionId = value5;
					this._account.GameUrl = value6;
					this._account.GameConf = GameConfig.generateConfig(text2);
					this._account.GameConf.setRoleName(value, num, value4, "");
					result = "";
				}
			}
			return result;
		}

		public string pushSession()
		{
			string url = string.Format("{0}/api/push_session", this._host);
			string s = CommonUtils.generateCookieString(this._account.Cookies);
			string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
			string data = string.Format("uid={0}&uauth={1}&accid={2}&web_cookies={3}&jsession_id={4}&game_url={5}", new object[]
			{
				this._userId,
				this._userAuth,
				this._accId,
				text,
				this._account.JsessionId,
				this._account.GameUrl
			});
			List<Cookie> list = new List<Cookie>();
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref list, "", null);
			string result;
			if (httpResult.StatusCode != 200)
			{
				result = "http访问失败";
			}
			else
			{
				string content = httpResult.getContent();
				JsonTextParser jsonTextParser = new JsonTextParser();
				JsonObjectCollection jsonObjectCollection = jsonTextParser.Parse(content) as JsonObjectCollection;
				JsonBooleanValue jsonBooleanValue = jsonObjectCollection["success"] as JsonBooleanValue;
				if (!jsonBooleanValue.Value.Value)
				{
					JsonStringValue jsonStringValue = jsonObjectCollection["msg"] as JsonStringValue;
					result = jsonStringValue.Value;
				}
				else
				{
					result = "";
				}
			}
			return result;
		}

		public string pushConfig()
		{
			string url = string.Format("{0}/api/push_conf", this._host);
			string rawString = this._account.GameConf.getRawString();
			string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawString));
			text = Uri.EscapeDataString(text);
			string data = string.Format("uid={0}&uauth={1}&accid={2}&config={3}", new object[]
			{
				this._userId,
				this._userAuth,
				this._accId,
				text
			});
			List<Cookie> list = new List<Cookie>();
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref list, "", null);
			string result;
			if (httpResult.StatusCode != 200)
			{
				result = "http访问失败";
			}
			else
			{
				string content = httpResult.getContent();
				JsonTextParser jsonTextParser = new JsonTextParser();
				JsonObjectCollection jsonObjectCollection = jsonTextParser.Parse(content) as JsonObjectCollection;
				JsonBooleanValue jsonBooleanValue = jsonObjectCollection["success"] as JsonBooleanValue;
				if (!jsonBooleanValue.Value.Value)
				{
					JsonStringValue jsonStringValue = jsonObjectCollection["msg"] as JsonStringValue;
					result = jsonStringValue.Value;
				}
				else
				{
					result = "";
				}
			}
			return result;
		}
	}
}
