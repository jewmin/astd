using System;

namespace com.lover.astd.common.model
{
	public class ClientUser
	{
		private int _id;

		private string _username;

		private string _password;

		private string _nickname;

		private string _email;

		private string _phone;

		private DateTime _created;

		private string _createIp;

		private DateTime _lastLogin;

		private string _lastLoginIp;

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

		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
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

		public string Nickname
		{
			get
			{
				return this._nickname;
			}
			set
			{
				this._nickname = value;
			}
		}

		public string Email
		{
			get
			{
				return this._email;
			}
			set
			{
				this._email = value;
			}
		}

		public string Phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				this._phone = value;
			}
		}

		public DateTime Created
		{
			get
			{
				return this._created;
			}
			set
			{
				this._created = value;
			}
		}

		public string CreateIp
		{
			get
			{
				return this._createIp;
			}
			set
			{
				this._createIp = value;
			}
		}

		public DateTime LastLogin
		{
			get
			{
				return this._lastLogin;
			}
			set
			{
				this._lastLogin = value;
			}
		}

		public string LastLoginIp
		{
			get
			{
				return this._lastLoginIp;
			}
			set
			{
				this._lastLoginIp = value;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is ClientUser && this.Id == (obj as ClientUser).Id;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
