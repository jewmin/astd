using System;

namespace com.lover.astd.common.model
{
	public class AsObject
	{
		protected int _id;

		protected bool _isChecked;

		protected string _name;

		protected bool _isChecked2;

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

		public bool IsChecked
		{
			get
			{
				return this._isChecked;
			}
			set
			{
				this._isChecked = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public bool IsChecked2
		{
			get
			{
				return this._isChecked2;
			}
			set
			{
				this._isChecked2 = value;
			}
		}
	}
}
