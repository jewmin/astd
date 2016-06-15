using System;

namespace com.lover.astd.common.model.battle
{
	public class Npc : AsObject, IComparable<Npc>, IComparable
	{
		private bool _attack;

		private int _armyId;

		private int _type;

		private string _itemName = "";

		private string _itemNamePure = "";

		private int _itemRatioType;

		private string _itemColor = "#ffffff";

		private string _formation = "不变阵";

		public bool Attack
		{
			get
			{
				return this._attack;
			}
			set
			{
				this._attack = value;
			}
		}

		public int ArmyId
		{
			get
			{
				return this._armyId;
			}
			set
			{
				this._armyId = value;
			}
		}

		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				bool flag = value == null || value.Equals("");
				if (!flag)
				{
					this._itemName = value;
					int num = this._itemName.IndexOf("（");
					bool flag2 = num < 0;
					if (flag2)
					{
						num = this._itemName.IndexOf("(");
					}
					bool flag3 = num < 0;
					if (flag3)
					{
						this._itemNamePure = this._itemName;
					}
					else
					{
						this._itemNamePure = this._itemName.Substring(0, num);
					}
					bool flag4 = this._itemName.IndexOf("小概率") >= 0;
					if (flag4)
					{
						this._itemRatioType = 1;
					}
					else
					{
						bool flag5 = this._itemName.IndexOf("大概率") >= 0;
						if (flag5)
						{
							this._itemRatioType = 2;
						}
						else
						{
							this._itemRatioType = 0;
						}
					}
				}
			}
		}

		public string ItemNamePure
		{
			get
			{
				return this._itemNamePure;
			}
		}

		public int ItemRatioType
		{
			get
			{
				return this._itemRatioType;
			}
		}

		public string ItemColor
		{
			get
			{
				return this._itemColor;
			}
			set
			{
				this._itemColor = value;
			}
		}

		public string Formation
		{
			get
			{
				return this._formation;
			}
			set
			{
				this._formation = value;
			}
		}

		public string ItemDesc
		{
			get
			{
				return string.Format("{0} {1}", this._name, this._itemName);
			}
		}

		public int CompareTo(Npc other)
		{
			bool flag = base.Id > other.Id;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = base.Id < other.Id;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		public int CompareTo(object obj)
		{
			Npc npc = obj as Npc;
			bool flag = npc == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = base.Id > npc.Id;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					bool flag3 = base.Id < npc.Id;
					if (flag3)
					{
						result = 1;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}
	}
}
