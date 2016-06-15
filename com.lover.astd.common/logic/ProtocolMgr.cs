using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using com.lover.common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace com.lover.astd.common.logic
{
	public class ProtocolMgr
	{
		private ServiceFactory _factory;

		private User _user;

		private ILogger _logger;

		private IServer _logic;

		private string _gameUrl;

		private string _jsessionId;

		private List<Cookie> _cookies = new List<Cookie>();

		public ILogger logger
		{
			get
			{
				return this._logger;
			}
		}

		public User getUser()
		{
			return this._user;
		}

		public ProtocolMgr(User u, ILogger logger, IServer logic, string gameUrl, string jsessionId, ServiceFactory factory)
		{
			this._user = u;
			this._logger = logger;
			this._logic = logic;
			this._gameUrl = gameUrl;
			this._factory = factory;
			Uri uri = new Uri(this._gameUrl);
			bool flag = uri.Port == 80 || uri.Port == 0;
			if (flag)
			{
				this._gameUrl = string.Format("{0}://{1}", uri.Scheme, uri.Host);
			}
			else
			{
				this._gameUrl = string.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port);
			}
			this._jsessionId = jsessionId;
			this._cookies.Clear();
			Uri uri2 = new Uri(gameUrl);
			Cookie cookie = new Cookie();
			cookie.Path = "/root";
			cookie.Domain = uri2.Host;
			cookie.Name = "JSESSIONID";
			cookie.Value = jsessionId;
			this._cookies.Add(cookie);
		}

		public ServerResult getXml(string url, string desc)
		{
			this._logger.logSingle(desc);
			ServerResult serverResult = this.doGet(url);
			if (serverResult == null)
			{
				return null;
			}
            this._logger.logDebug(serverResult.getDebugInfo());
            if (!serverResult.HttpSucceed)
            {
                return null;
            }
            if (!serverResult.CmdSucceed)
            {
                if (serverResult.CmdError.Contains("验证码") || serverResult.CmdError.Contains("驗證碼"))
                {
                    this._logic.startReLoginTimer();
                    this._logic.stopServer(false);
                    this._logic.notifyState(AccountStatus.STA_stopped_game_verify);
                    Exception ex = new Exception("需要验证码");
                    throw ex;
                }
                if (serverResult.CmdError.Contains("连接已超时") || serverResult.CmdError.Contains("用户已在别处登陆") || serverResult.CmdError.Contains("連接已超時") || serverResult.CmdError.Contains("用戶已在別處登錄"))
                {
                    this._logic.startReLoginTimer();
                    this._logic.stopServer(false);
                    Exception ex2 = new Exception("需要重新登录");
                    throw ex2;
                }
            }
            XmlNode xmlNode = serverResult.CmdResult.SelectSingleNode("/results/playerupdateinfo");
            if (xmlNode != null)
            {
                this._user.refreshPlayerInfo(xmlNode);
                if (this._logic != null)
                {
                    this._logic.refreshPlayerSafe();
                }
            }
            XmlNode xmlNode2 = serverResult.CmdResult.SelectSingleNode("/results/playerbattleinfo");
            if (xmlNode2 != null)
            {
                this._user.refreshPlayerInfo(xmlNode2);
                if (this._logic != null)
                {
                    this._logic.refreshPlayerSafe();
                }
            }
            return serverResult;
		}

		public ServerResult postXml(string url, string data, string desc)
		{
			this._logger.logSingle(desc);
			ServerResult serverResult = this.doPost(url, data);
			if (serverResult == null)
			{
				return null;
			}
            this._logger.logDebug(serverResult.getDebugInfo());
            if (!serverResult.HttpSucceed)
            {
                return null;
            }
            if (!serverResult.CmdSucceed)
            {
                if (serverResult.CmdError.Contains("验证码") || serverResult.CmdError.Contains("驗證碼"))
                {
                    this._logic.startReLoginTimer();
                    this._logic.stopServer(false);
                    this._logic.notifyState(AccountStatus.STA_stopped_game_verify);
                    Exception ex = new Exception("需要验证码");
                    throw ex;
                }
                if (serverResult.CmdError.Contains("连接已超时") || serverResult.CmdError.Contains("用户已在别处登陆") || serverResult.CmdError.Contains("連接已超時") || serverResult.CmdError.Contains("用戶已在別處登錄"))
                {
                    this._logic.startReLoginTimer();
                    this._logic.stopServer(false);
                    Exception ex2 = new Exception("需要重新登录");
                    throw ex2;
                }
            }
            XmlNode xmlNode = serverResult.CmdResult.SelectSingleNode("/results/playerupdateinfo");
            if (xmlNode != null)
            {
                this._user.refreshPlayerInfo(xmlNode);
                if (this._logic != null)
                {
                    this._logic.refreshPlayerSafe();
                }
            }
            XmlNode xmlNode2 = serverResult.CmdResult.SelectSingleNode("/results/playerbattleinfo");
            if (xmlNode2 != null)
            {
                this._user.refreshPlayerInfo(xmlNode2);
                if (this._logic != null)
                {
                    this._logic.refreshPlayerSafe();
                }
            }
            return serverResult;
		}

		protected bool ifSuccess(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("/results/state");
			return xmlNode.InnerText == "1";
		}

		private ServerResult doGet(string url)
		{
			string text = string.Format("{0}{1}?{2}", this._gameUrl, url, this._factory.TmrMgr.TimeStamp);
			this._logger.logDebug(string.Format("get url={0}", text));
			ServerResult result = null;
			try
			{
				string server_return = TransferMgr.doGet(text, ref this._cookies);
				result = new ServerResult(server_return);
			}
			catch (Exception ex)
			{
				this._logger.logError(ex.Message);
			}
			return result;
		}

		private ServerResult doPost(string url, string data)
		{
			string text = string.Format("{0}{1}?{2}", this._gameUrl, url, this._factory.TmrMgr.TimeStamp);
			this._logger.logDebug(string.Format("post url={0}, data={1}", text, data));
			ServerResult result = null;
			try
			{
				string server_return = TransferMgr.doPost(text, data, ref this._cookies);
				result = new ServerResult(server_return);
			}
			catch (Exception ex)
			{
				this._logger.logError(ex.Message);
			}
			return result;
		}
	}
}
