using com.lover.astd.common.model;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common
{
	public class ClientCache
	{
		private static ClientCache _instance = new ClientCache();

		private ClientUser _currentUser;

		public List<AccountData> _currentAccounts = new List<AccountData>();

		public static ClientCache instance
		{
			get
			{
				return ClientCache._instance;
			}
		}

		public ClientUser CurrentUser
		{
			get
			{
				return this._currentUser;
			}
			set
			{
				this._currentUser = value;
			}
		}

		public void addAccount(AccountData acc)
		{
			AccountData accountData = this.findAccount(acc.Id);
			if (accountData == null)
			{
				this._currentAccounts.Add(acc);
			}
			else
			{
				accountData.refreshData(acc);
			}
		}

		public void removeAccount(int accId)
		{
			AccountData accountData = this.findAccount(accId);
			if (accountData != null)
			{
				this._currentAccounts.Remove(accountData);
			}
		}

		public AccountData findAccount(int accId)
		{
            foreach (AccountData acc in this._currentAccounts)
            {
                if (acc.Id == accId)
                {
                    return acc;
                }
            }
            return null;
		}

		public void log(string text)
		{
			Console.WriteLine(text);
		}
	}
}
