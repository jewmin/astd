using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace com.lover.astd.common.partner
{
	public abstract class LoginImplBase
	{
		private object _file_cache_lock = new object();

		protected LoginMgr _loginMgr;

		protected string _username;

		protected string _password;

		protected string _md5_password;

		protected string _md5_password_lower;

		protected Random _rand = new Random();

		protected AccountData _acc;

		protected LoginImplBase(LoginMgr loginMgr)
		{
			this._loginMgr = loginMgr;
		}

		public void setAccount(AccountData dt)
		{
			this._acc = dt;
			this._username = Uri.EscapeDataString(this._acc.UserName);
			this._password = Uri.EscapeDataString(this._acc.Password);
			this._md5_password = CommonUtils.generateStringMd5(this._acc.Password, null);
			this._md5_password_lower = this._md5_password.ToLower();
		}

		protected void saveCacheFile(string filename, string filecontent)
		{
			Monitor.Enter(this._file_cache_lock);
			string text = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\cache";
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			FileInfo fileInfo = new FileInfo(text + "\\" + filename);
			bool flag2 = !fileInfo.Exists;
			FileStream fileStream;
			if (flag2)
			{
				fileStream = fileInfo.Create();
			}
			else
			{
				fileStream = fileInfo.OpenWrite();
			}
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(filecontent);
			streamWriter.Close();
			fileStream.Close();
			Monitor.Exit(this._file_cache_lock);
		}

		protected string getCacheFile(string filename)
		{
			Monitor.Enter(this._file_cache_lock);
			string text = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\cache";
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			FileInfo fileInfo = new FileInfo(text + "\\" + filename);
			bool flag2 = !fileInfo.Exists;
			string result;
			if (flag2)
			{
				result = "";
			}
			else
			{
				FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
				StreamReader streamReader = new StreamReader(fileStream);
				string text2 = streamReader.ReadToEnd();
				streamReader.Close();
				fileStream.Close();
				Monitor.Exit(this._file_cache_lock);
				result = text2;
			}
			return result;
		}

		public abstract LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null);

		public virtual bool alwaysNeedVerifyCode()
		{
			return false;
		}

		public virtual string getAlwaysCaptchaImageUrl()
		{
			return "";
		}

		protected void buildAlwaysVerifyResult(ref LoginResult result, ref List<Cookie> cookies)
		{
			result.StatusCode = LoginStatusCode.NeedVerifyCode;
			result.ErrMessage = "需要输入验证码";
			result.CaptchaUrl = this.getAlwaysCaptchaImageUrl();
			result.CaptchaImage = this._loginMgr.getCaptchaImage(result.CaptchaUrl, ref cookies);
			result.WebCookies = cookies;
		}

		protected string getGBKEscape(string name)
		{
			byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(name);
			string text = "";
			byte[] array = bytes;
			int num;
			for (int i = 0; i < array.Length; i = num + 1)
			{
				byte b = array[i];
				text += string.Format("%{0:X2}", b);
				num = i;
			}
			return text.ToUpper();
		}

		protected void processRedirect(string redirecturl, LoginResult result, ref List<Cookie> cookies)
		{
			this.goingToGameUrl();
			HttpResult httpResult = TransferMgr.doGetPure(redirecturl, ref cookies, "", null);
			bool flag = httpResult == null;
			if (flag)
			{
				result.StatusCode = LoginStatusCode.FailInGotoGameUrl;
			}
			else
			{
				string extraHeader = httpResult.getExtraHeader("Location");
				bool flag2 = extraHeader == null;
				if (flag2)
				{
					result.StatusCode = LoginStatusCode.FailInGetSession;
				}
				else
				{
					this.makeSureValidUri(redirecturl, ref extraHeader);
					bool flag3 = !extraHeader.Contains("start.action");
					if (flag3)
					{
						this.processRedirect(extraHeader, result, ref cookies);
					}
					else
					{
						this.processStartGame(extraHeader, result, ref cookies, "");
					}
				}
			}
		}

		protected void processStartGame(string starturl, LoginResult result, ref List<Cookie> cookies, string referer = "")
		{
			this.gettingSession();
			HttpResult httpResult = TransferMgr.doGetPure(starturl, ref cookies, referer, null);
			bool flag = httpResult == null;
			if (flag)
			{
				result.StatusCode = LoginStatusCode.FailInGetSession;
			}
			else
			{
				result.GameUrl = httpResult.getExtraHeader("Location");
				List<Cookie> cookies2 = httpResult.getCookies();
				foreach (Cookie current in cookies2)
				{
					bool flag2 = current.Name == "JSESSIONID";
					if (flag2)
					{
						result.JSessionID = current.Value;
						break;
					}
				}
				result.WebCookies = cookies;
				bool flag3 = result.JSessionID == null || result.JSessionID == "";
				if (flag3)
				{
					result.StatusCode = LoginStatusCode.FailInGetSession;
				}
				else
				{
					result.StatusCode = LoginStatusCode.Success;
				}
				this.succeed();
			}
		}

		protected void postStartGame(string starturl, string data, LoginResult result, ref List<Cookie> cookies, string referer = "")
		{
			this.gettingSession();
			HttpResult httpResult = TransferMgr.doPostPure(starturl, data, ref cookies, referer, null);
			bool flag = httpResult == null;
			if (flag)
			{
				result.StatusCode = LoginStatusCode.FailInGetSession;
			}
			else
			{
				result.GameUrl = httpResult.getExtraHeader("Location");
				List<Cookie> cookies2 = httpResult.getCookies();
				foreach (Cookie current in cookies2)
				{
					bool flag2 = current.Name == "JSESSIONID";
					if (flag2)
					{
						result.JSessionID = current.Value;
						break;
					}
				}
				result.WebCookies = cookies;
				bool flag3 = result.JSessionID == null || result.JSessionID == "";
				if (flag3)
				{
					result.StatusCode = LoginStatusCode.FailInGetSession;
				}
				else
				{
					result.StatusCode = LoginStatusCode.Success;
				}
				this.succeed();
			}
		}

		protected void makeSureValidUri(string base_url, ref string now_url)
		{
			bool flag = now_url.StartsWith("http://") || now_url.StartsWith("https://");
			if (!flag)
			{
				Uri uri = new Uri(base_url);
				string scheme = uri.Scheme;
				string host = uri.Host;
				int port = uri.Port;
				string text = "";
				bool flag2 = port != 0 && port != 80;
				if (flag2)
				{
					text = ":" + port;
				}
				string text2 = uri.AbsolutePath;
				int num = text2.LastIndexOf('/');
				bool flag3 = num >= 0;
				if (flag3)
				{
					text2 = text2.Substring(0, num);
				}
				bool flag4 = now_url.StartsWith("./");
				if (flag4)
				{
					now_url = now_url.Substring(1);
				}
				else
				{
					bool flag5 = now_url.StartsWith("../");
					if (flag5)
					{
						now_url = now_url.Substring(2);
						num = text2.LastIndexOf('/');
						bool flag6 = num > 0;
						if (flag6)
						{
							text2 = text2.Substring(0, num);
						}
					}
					else
					{
						bool flag7 = now_url.StartsWith("/");
						if (flag7)
						{
							text2 = "";
						}
						else
						{
							now_url = "/" + now_url;
						}
					}
				}
				now_url = string.Format("{0}://{1}{2}{3}{4}", new object[]
				{
					scheme,
					host,
					text,
					text2,
					now_url
				});
			}
		}

		protected void logging()
		{
			this.sendStatus("正在登录...");
		}

		protected void findingServerUrl()
		{
			this.sendStatus("正在获取所在区的地址...");
		}

		protected void goingToGameUrl()
		{
			this.sendStatus("正在跳转到所在区地址...");
		}

		protected void gettingSession()
		{
			this.sendStatus("正在获取会话...");
		}

		protected void succeed()
		{
			this.sendStatus("登录完成~~");
		}

		private void sendStatus(string status)
		{
			this._loginMgr.sendStatus(status);
		}
	}
}
