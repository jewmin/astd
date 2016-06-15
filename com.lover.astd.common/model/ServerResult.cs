using System;
using System.Xml;

namespace com.lover.astd.common.model
{
	public class ServerResult
	{
		private string _innerXml;

		private int _httpCode;

		private bool _cmdSucceed;

		private string _cmdError;

		private XmlDocument _cmdResult;

		public bool HttpSucceed
		{
			get
			{
				return this._httpCode == 200;
			}
		}

		public int HttpCode
		{
			get
			{
				return this._httpCode;
			}
			set
			{
				this._httpCode = value;
			}
		}

		public bool CmdSucceed
		{
			get
			{
				return this._cmdSucceed;
			}
		}

		public string CmdError
		{
			get
			{
				return this._cmdError;
			}
		}

		public XmlDocument CmdResult
		{
			get
			{
				return this._cmdResult;
			}
			set
			{
				this._cmdResult = value;
			}
		}

		public string getHttpErrorInfo()
		{
			string result = "";
			int httpCode = this._httpCode;
			if (httpCode != -1)
			{
				if (httpCode != 404)
				{
					if (httpCode == 500)
					{
						result = "500 服务器错误";
					}
				}
				else
				{
					result = "404 未找到网页";
				}
			}
			else
			{
				result = "网断了";
			}
			return result;
		}

		public string getDebugInfo()
		{
			if (!this.HttpSucceed)
			{
				return this.getHttpErrorInfo();
			}
            else if (!this.CmdSucceed)
            {
                return this.CmdError;
            }
            else
            {
                return this._innerXml;
            }
		}

		public ServerResult(string server_return)
		{
			if (server_return == null || server_return == "")
			{
				this._cmdSucceed = false;
				this._cmdError = "";
				this._cmdResult = null;
			}
            else if (server_return.StartsWith("code:"))
            {
                string s = server_return.Replace("code:", "");
                int httpCode = int.Parse(s);
                this._httpCode = httpCode;
            }
            else
            {
                this._httpCode = 200;
                this._cmdResult = new XmlDocument();
                server_return = server_return.Replace("&", "");
                this._cmdResult.LoadXml(server_return);
                XmlNode xmlNode = this._cmdResult.SelectSingleNode("/results/state");
                if (xmlNode == null || xmlNode.InnerText != "1")
                {
                    if (xmlNode.InnerText == "3")
                    {
                        this._cmdError = "出现验证码了";
                    }
                    else
                    {
                        XmlNode xmlNode2 = this._cmdResult.SelectSingleNode("/results/message");
                        if (xmlNode2 != null)
                        {
                            this._cmdError = xmlNode2.InnerText;
                        }
                        else
                        {
                            xmlNode2 = this._cmdResult.SelectSingleNode("/results/exception");
                            if (xmlNode2 != null)
                            {
                                this._cmdError = xmlNode2.InnerText;
                            }
                        }
                    }
                    if (this._cmdError == null)
                    {
                        this._cmdError = server_return;
                    }
                    this._cmdSucceed = false;
                }
                else
                {
                    this._innerXml = server_return;
                    this._cmdError = "";
                    this._cmdSucceed = true;
                }
            }
		}
	}
}
