using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using com.lover.astd.common.partner;
using com.lover.astd.common.partner.impl;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace com.lover.astd.common.manager
{
	public class LoginMgr
	{
		private ILoginReceiver _client;

		private LoginImplBase _partner;

		public LoginResult doLogin(ILoginReceiver loginClient, ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			bool flag = loginClient == null;
			LoginResult result;
			if (flag)
			{
				result = null;
			}
			else
			{
				this._client = loginClient;
				AccountData account = loginClient.getAccount();
				this._partner = this.getLoginImpl(account.Server_type);
				bool flag2 = this._partner == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					this._partner.setAccount(account);
					LoginResult loginResult = null;
					try
					{
						loginResult = this._partner.login(ref cookies, verify_code, extra);
					}
					catch (Exception ex)
					{
						loginResult = new LoginResult();
						loginResult.StatusCode = LoginStatusCode.FailInLogin;
						loginResult.ErrMessage = "登录失败, 请重试, " + ex.StackTrace;
					}
					result = loginResult;
				}
			}
			return result;
		}

		public Image getCaptchaImage(string url, ref List<Cookie> cookies)
		{
			Image result;
			try
			{
				HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
				bool flag = httpResult == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					Stream decodedStream = httpResult.getDecodedStream();
					Image image = Image.FromStream(decodedStream);
					decodedStream.Close();
					result = image;
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public void sendStatus(string status)
		{
			bool flag = this._client != null;
			if (flag)
			{
				this._client.setLoginStatus(status);
			}
		}

		private LoginImplBase getLoginImpl(ServerType stype)
		{
			LoginImplBase result = null;
			try
			{
				switch (stype)
				{
				case ServerType.YaoWan:
					result = new YaoWanLogin(this);
					break;
				case ServerType._360:
					result = new _360Login(this);
					break;
				case ServerType.PPS:
					result = new PpsLogin(this);
					break;
				case ServerType.DuoWan:
					result = new DuowanLogin(this);
					break;
				case ServerType._37Wan:
					result = new _37WanLogin(this);
					break;
				case ServerType.ZhuLang:
					result = new ZhulangLogin(this);
					break;
				case ServerType.HuanLang:
					result = new HuanlangLogin(this);
					break;
				case ServerType.Baidu:
					result = new BaiduLogin(this);
					break;
				case ServerType._178:
					result = new _178Login(this);
					break;
				case ServerType.Aoshitang:
					result = new AoshitangLogin(this);
					break;
				case ServerType._51Wan:
					result = new _51wanLogin(this);
					break;
				case ServerType.ISpeak:
					result = new IspeakLogin(this);
					break;
				case ServerType.IFeng:
					result = new IfengLogin(this);
					break;
				case ServerType._8ZY:
					result = new _8zyLogin(this);
					break;
				case ServerType.XdWan:
					result = new XdwanLogin(this);
					break;
				case ServerType._4399:
					result = new _4399Login(this);
					break;
				case ServerType._6711:
					result = new _6711Login(this);
					break;
				case ServerType._6998:
					result = new _6998Login(this);
					break;
				case ServerType.KuWan8:
					result = new Kuwan8Login(this);
					break;
				case ServerType.PeiYou:
					result = new PeiyouLogin(this);
					break;
				case ServerType.VeryCd:
					result = new VerycdLogin(this);
					break;
				case ServerType.WebXGame:
					result = new WebxgameLogin(this);
					break;
				case ServerType.TgBus:
					result = new TgbusLogin(this);
					break;
				case ServerType._56uu:
					result = new _56uuLogin(this);
					break;
				case ServerType.Jinjuzi:
					result = new JinjuziLogin(this);
					break;
				case ServerType._3896:
					result = new _3896Login(this);
					break;
				case ServerType.Uz73:
					result = new Uz73Login(this);
					break;
				case ServerType.MiYou:
					result = new MiyouLogin(this);
					break;
				case ServerType.RenRen:
					result = new RenrenLogin(this);
					break;
				case ServerType.Kunlun:
					result = new KunlunLogin(this);
					break;
				case ServerType.NiuA:
					result = new NiuaLogin(this);
					break;
				case ServerType.Game2:
					result = new Game2Login(this);
					break;
				case ServerType._91:
					result = new _91Login(this);
					break;
				case ServerType.Cga:
					result = new CgaLogin(this);
					break;
				case ServerType.Plu:
					result = new PluLogin(this);
					break;
				case ServerType._53Wan:
					result = new _53wanLogin(this);
					break;
				case ServerType._789hi:
					result = new _789hiLogin(this);
					break;
				case ServerType.Lequ:
					result = new LequLogin(this);
					break;
				case ServerType.Letou8:
					result = new Letou8Login(this);
					break;
				case ServerType.SnsTele:
					result = new SnsteleLogin(this);
					break;
				case ServerType.Kuwo:
					result = new KuwoLogin(this);
					break;
				case ServerType._96Pk:
					result = new _96PkLogin(this);
					break;
				case ServerType._51:
					result = new _51Login(this);
					break;
				case ServerType._29ww:
					result = new _29wwLogin(this);
					break;
				case ServerType._91wan:
					result = new _91wanLogin(this);
					break;
				case ServerType.Tianya:
					result = new TianyaLogin(this);
					break;
				case ServerType.Funshion:
					result = new FunshionLogin(this);
					break;
				case ServerType.Pptv:
					result = new PptvLogin(this);
					break;
				case ServerType.Kuaiwan:
					result = new KuaiwanLogin(this);
					break;
				case ServerType.Huolawan:
					result = new HuolawanLogin(this);
					break;
				case ServerType.Youwo:
					result = new YouwoLogin(this);
					break;
				case ServerType.Kugou:
					result = new KugouLogin(this);
					break;
				}
			}
			catch (Exception ex)
			{
				UiUtils.getInstance().error("Error:::" + ex.StackTrace);
			}
			return result;
		}

		public bool ifSupportServer(ServerType stype)
		{
			return this.getLoginImpl(stype) != null;
		}
	}
}
