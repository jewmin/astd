using com.lover.astd.common.config;
using com.lover.astd.common.manager;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;
using System.Net;

namespace com.lover.astd.common.model
{
	public class AccountData
	{
		private int _id;

		private int _userId;

		private DateTime _enableTime;

		private DateTime _sessionTime;

		private bool _enabled = true;

		private ServerType _serverType;

		private int _serverId;

		private string _custom_server_name = "";

		private string _userName;

		private string _password;

		private string _roleName;

		private string _customLoginUrl;

		private string _customGameUrl;

		private bool _isLastLogin;

		private AccountStatus _status;

		private GameConfig _gameConf;

        private OtherConfig _otherConf;

		private string _gameUrl;

		private string _jsessionId;

		private List<Cookie> _cookies = new List<Cookie>();

		public int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
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

		public DateTime EnableTime
		{
			get
			{
				return this._enableTime;
			}
			set
			{
				this._enableTime = value;
			}
		}

		public DateTime SessionTime
		{
			get
			{
				return this._sessionTime;
			}
			set
			{
				this._sessionTime = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		public ServerType Server_type
		{
			get
			{
				return this._serverType;
			}
			set
			{
				this._serverType = value;
			}
		}

		public int ServerId
		{
			get
			{
				return this._serverId;
			}
			set
			{
				this._serverId = value;
			}
		}

		public string UserName
		{
			get
			{
				return this._userName;
			}
			set
			{
				this._userName = value;
			}
		}

		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}

		public string RoleName
		{
			get
			{
				return this._roleName;
			}
			set
			{
				this._roleName = value;
			}
		}

		public string CustomLoginUrl
		{
			get
			{
				return this._customLoginUrl;
			}
			set
			{
				this._customLoginUrl = value;
			}
		}

		public string CustomGameUrl
		{
			get
			{
				return this._customGameUrl;
			}
			set
			{
				this._customGameUrl = value;
			}
		}

		public bool IsLastLogin
		{
			get
			{
				return this._isLastLogin;
			}
			set
			{
				this._isLastLogin = value;
			}
		}

		public AccountStatus Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
			}
		}

		public GameConfig GameConf
		{
			get
			{
				return this._gameConf;
			}
			set
			{
				this._gameConf = value;
			}
		}

        public OtherConfig OtherConf
        {
            get
            {
                return _otherConf;
            }
            set
            {
                _otherConf = value;
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
				this._gameUrl = value;
			}
		}

		public string JsessionId
		{
			get
			{
				return this._jsessionId;
			}
			set
			{
				this._jsessionId = value;
			}
		}

		public List<Cookie> Cookies
		{
			get
			{
				return this._cookies;
			}
			set
			{
				this._cookies = value;
			}
		}

		public string AccDesc
		{
			get
			{
				return string.Format("{0}-{1}-[{2}]:ID:{3}", new object[]
				{
					EnumString.getString(this.Server_type),
					this.ServerId,
					this.RoleName,
					this.Id
				});
			}
		}

		public void setServerName(string name)
		{
			this._custom_server_name = name;
		}

		public string getServerName()
		{
			bool flag = this._custom_server_name != "";
			string result;
			if (flag)
			{
				result = this._custom_server_name;
			}
			else
			{
				result = string.Format("{0}_{1}区", EnumString.getString(this.Server_type), this._serverId);
			}
			return result;
		}

		public string Desc()
		{
			bool flag = this._serverType == ServerType.Custom;
			string result;
			if (flag)
			{
				result = string.Format("[{0}] {1}", EnumString.getString(this._status), EnumString.getString(this._serverType));
			}
			else
			{
				result = string.Format("[{0}] {1}({2}区):{3}{4}", new object[]
				{
					EnumString.getString(this._status),
					EnumString.getString(this._serverType),
					this._serverId.ToString(),
					this._userName,
					(this._roleName == "") ? "" : ("|" + this._roleName)
				});
			}
			return result;
		}

		public override string ToString()
		{
			return this._userName;
		}

		public void setServerType(string server_typestr)
		{
			foreach (ServerType serverType in Enum.GetValues(typeof(ServerType)))
			{
				bool flag = server_typestr.Equals(serverType.ToString());
				if (flag)
				{
					this._serverType = serverType;
					break;
				}
			}
		}

		public override bool Equals(object accdt)
		{
			bool flag = !(accdt is AccountData);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				AccountData accountData = accdt as AccountData;
				bool flag2 = this.Server_type == ServerType.Custom;
				if (flag2)
				{
					result = (accountData.Server_type == ServerType.Custom);
				}
				else
				{
					result = (this.Server_type == accountData.Server_type && this.UserName == accountData.UserName && this.ServerId == accountData.ServerId && this.RoleName == accountData.RoleName);
				}
			}
			return result;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public string generateGameId()
		{
			return string.Format("{0}_{1}_{2}_{3}", new object[]
			{
				EnumString.getString(this._serverType),
				this._serverId,
				this._userName,
				this._roleName
			});
		}

		public void refreshData(AccountData src)
		{
			this.UserId = src.UserId;
			this.Enabled = src.Enabled;
			this.Server_type = src.Server_type;
			this.ServerId = src.ServerId;
			this.RoleName = src.RoleName;
			this.UserName = src.UserName;
			this.Password = src.Password;
			this.EnableTime = src.EnableTime;
			this.GameConf = src.GameConf;
            this.OtherConf = src.OtherConf;
			this.Cookies = src.Cookies;
			this.JsessionId = src.JsessionId;
			this.GameUrl = src.GameUrl;
			this.SessionTime = src.SessionTime;
			this.Status = src.Status;
		}
	}
}
