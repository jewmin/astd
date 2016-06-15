using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace com.lover.astd.common.partner
{
	public class LoginResult
	{
		private string _errMessage;

		private LoginStatusCode _statusCode;

		private List<Cookie> _webCookies;

		private string _jSessionID = "";

		private string _gameUrl = "";

		private string _captchaUrl;

		private Image _captchaImage;

		private string _captchaExtra;

		public string ErrMessage
		{
			get
			{
				return this._errMessage;
			}
			set
			{
				this._errMessage = value;
			}
		}

		public LoginStatusCode StatusCode
		{
			get
			{
				return this._statusCode;
			}
			set
			{
				this._statusCode = value;
			}
		}

		public string StatusDesc
		{
			get
			{
				string text = "";
				switch (this._statusCode)
				{
				case LoginStatusCode.NeedVerifyCode:
					text = this._errMessage;
					break;
				case LoginStatusCode.FailInGetToken:
					text = "获取登录token失败, 请重试";
					break;
				case LoginStatusCode.FailInLogin:
					text = "登录失败, 请检查是否用户名/密码/验证码错误, 如确认无误, 请联系作者";
					break;
				case LoginStatusCode.FailInGetServerList:
					text = "获取服务器列表失败";
					break;
				case LoginStatusCode.FailInFindingGameUrl:
					text = "获取游戏地址失败";
					break;
				case LoginStatusCode.FailInGotoGameUrl:
					text = "跳转到游戏地址失败, 请查看是否已经合区";
					break;
				case LoginStatusCode.FailInGetSession:
					text = "获取会话失败";
					break;
				}
				bool flag = this._errMessage != null && this._errMessage != "";
				if (flag)
				{
					text = text + ";" + this._errMessage;
				}
				return text;
			}
		}

		public List<Cookie> WebCookies
		{
			get
			{
				return this._webCookies;
			}
			set
			{
				this._webCookies = value;
			}
		}

		public string JSessionID
		{
			get
			{
				return this._jSessionID;
			}
			set
			{
				this._jSessionID = value;
			}
		}

		public string GameUrl
		{
			get
			{
				return this._gameUrl;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this._gameUrl = value;
				}
			}
		}

		public string CaptchaUrl
		{
			get
			{
				return this._captchaUrl;
			}
			set
			{
				this._captchaUrl = value;
			}
		}

		public Image CaptchaImage
		{
			get
			{
				return this._captchaImage;
			}
			set
			{
				this._captchaImage = value;
			}
		}

		public string CaptchaExtra
		{
			get
			{
				return this._captchaExtra;
			}
			set
			{
				this._captchaExtra = value;
			}
		}
	}
}
