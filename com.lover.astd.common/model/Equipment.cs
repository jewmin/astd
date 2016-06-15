using System;
using System.Xml;

namespace com.lover.astd.common.model
{
	public class Equipment : AsObject
	{
		private GoodsType _goodstype;

		private string _generalName;

		private EquipmentType _type;

		private EquipmentQuality _quality;

		private bool _isFragment;

		private int _fragmentCount;

		private int _totalFragmentCount;

		private int _valueNow;

		private int _level;

		private int _enchantLevel;

		private bool _canUpgrade;

		private int _upgradeCost;

		private int _remainTime;

		private bool _weaponEquiped;

		private int _zeroStarNum;

		private int _onceNum;

		private int _curWeaponLevelLimit;

		public bool _isSuperWeapon;

		public int _curCuilianLevel;

		public int _curCuilianCount;

		public int _cuilianNeedCount;

		public int _curCuilianLevelLimit;

		private bool _canEnchant;

		private int _enchantCost1;

		private int _enchantCost2;

		private int _enchantCost3;

        private int _baowulea;

        public int Baowulea
        {
            get { return _baowulea; }
            set { _baowulea = value; }
        }

        private int _baowustr;

        public int Baowustr
        {
            get { return _baowustr; }
            set { _baowustr = value; }
        }

        private int _baowuint;

        public int Baowuint
        {
            get { return _baowuint; }
            set { _baowuint = value; }
        }

		public GoodsType goodstype
		{
			get
			{
				return this._goodstype;
			}
			set
			{
				this._goodstype = value;
			}
		}

		public string GeneralName
		{
			get
			{
				return this._generalName;
			}
			set
			{
				this._generalName = value;
			}
		}

		public string EquipNameWithGeneral
		{
			get
			{
				bool flag = this._generalName != null && this._generalName != "";
				string result;
				if (flag)
				{
					result = string.Format("{0}({1})", this._name, this._generalName);
				}
				else
				{
					result = this._name;
				}
				return result;
			}
		}

		public EquipmentType Type
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

		public EquipmentQuality Quality
		{
			get
			{
				return this._quality;
			}
			set
			{
				this._quality = value;
			}
		}

		public bool IsFragment
		{
			get
			{
				return this._isFragment;
			}
			set
			{
				this._isFragment = value;
			}
		}

		public int FragmentCount
		{
			get
			{
				return this._fragmentCount;
			}
			set
			{
				this._fragmentCount = value;
			}
		}

		public int TotalFragmentCount
		{
			get
			{
				return this._totalFragmentCount;
			}
			set
			{
				this._totalFragmentCount = value;
			}
		}

		public int ValueNow
		{
			get
			{
				return this._valueNow;
			}
			set
			{
				this._valueNow = value;
			}
		}

		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
			}
		}

		public int EnchantLevel
		{
			get
			{
				return this._enchantLevel;
			}
			set
			{
				this._enchantLevel = value;
			}
		}

		public bool CanUpgrade
		{
			get
			{
				return this._canUpgrade;
			}
			set
			{
				this._canUpgrade = value;
			}
		}

		public int UpgradeCost
		{
			get
			{
				return this._upgradeCost;
			}
			set
			{
				this._upgradeCost = value;
			}
		}

		public int RemainTime
		{
			get
			{
				return this._remainTime;
			}
			set
			{
				this._remainTime = value;
			}
		}

		public bool WeaponEquiped
		{
			get
			{
				return this._weaponEquiped;
			}
			set
			{
				this._weaponEquiped = value;
			}
		}

		public int ZeroStarNum
		{
			get
			{
				return this._zeroStarNum;
			}
			set
			{
				this._zeroStarNum = value;
			}
		}

		public int OnceNum
		{
			get
			{
				return this._onceNum;
			}
			set
			{
				this._onceNum = value;
			}
		}

		public int CurWeaponLevelLimit
		{
			get
			{
				return this._curWeaponLevelLimit;
			}
			set
			{
				this._curWeaponLevelLimit = value;
			}
		}

		public bool CanEnchant
		{
			get
			{
				return this._canEnchant;
			}
			set
			{
				this._canEnchant = value;
			}
		}

		public int EnchantCost1
		{
			get
			{
				return this._enchantCost1;
			}
			set
			{
				this._enchantCost1 = value;
			}
		}

		public int EnchantCost2
		{
			get
			{
				return this._enchantCost2;
			}
			set
			{
				this._enchantCost2 = value;
			}
		}

		public int EnchantCost3
		{
			get
			{
				return this._enchantCost3;
			}
			set
			{
				this._enchantCost3 = value;
			}
		}

		public void fillValues(XmlNodeList nodes)
		{
			foreach (XmlNode xmlNode in nodes)
			{
				if (xmlNode.Name == "storeid" || xmlNode.Name == "goodsid")
				{
					if (this._id <= 0)
					{
						base.Id = int.Parse(xmlNode.InnerText);
					}
				}
                else if (xmlNode.Name == "id")
                {
                    base.Id = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "cuilianlv")
                {
                    this._isSuperWeapon = true;
                    int.TryParse(xmlNode.InnerText, out this._curCuilianLevel);
                }
                else if (xmlNode.Name == "curnum")
                {
                    int.TryParse(xmlNode.InnerText, out this._curCuilianCount);
                }
                else if (xmlNode.Name == "curmaxnum")
                {
                    int.TryParse(xmlNode.InnerText, out this._cuilianNeedCount);
                }
                else if (xmlNode.Name == "equipname" || xmlNode.Name == "name")
                {
                    base.Name = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "type")
                {
                    this.goodstype = (GoodsType)Enum.Parse(typeof(GoodsType), xmlNode.InnerText);
                }
                else if (xmlNode.Name == "equiptype")
                {
                    this.Type = (EquipmentType)Enum.Parse(typeof(EquipmentType), xmlNode.InnerText);
                }
                else if (xmlNode.Name == "quality")
                {
                    this.Quality = (EquipmentQuality)Enum.Parse(typeof(EquipmentQuality), xmlNode.InnerText);
                }
                else if (xmlNode.Name == "attribute")
                {
                    this.ValueNow = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "coppercost")
                {
                    this.UpgradeCost = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "reallevel")
                {
                    this.Level = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "equiplv")
                {
                    this.Level = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "equiplevel")
                {
                    if (this.Level == 0)
                    {
                        this.Level = int.Parse(xmlNode.InnerText);
                    }
                }
                else if (xmlNode.Name == "enchanting")
                {
                    this.EnchantLevel = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "remaintime")
                {
                    this.RemainTime = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "generalname")
                {
                    this.GeneralName = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "equiped")
                {
                    this.WeaponEquiped = (xmlNode.InnerText == "1");
                }
                else if (xmlNode.Name == "upgradeable")
                {
                    this.CanUpgrade = (xmlNode.InnerText == "1");
                }
                else if (xmlNode.Name == "enchantable")
                {
                    this.CanEnchant = (xmlNode.InnerText == "true");
                }
                else if (xmlNode.Name == "cost1")
                {
                    this.EnchantCost1 = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "cost2")
                {
                    this.EnchantCost2 = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "cost3")
                {
                    this.EnchantCost3 = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "chip")
                {
                    this.IsFragment = (xmlNode.InnerText == "1");
                }
                else if (xmlNode.Name == "goodsnum")
                {
                    this.FragmentCount = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "fnum")
                {
                    this.TotalFragmentCount = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "zerostarnum")
                {
                    this.ZeroStarNum = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "curlimit")
                {
                    this.CurWeaponLevelLimit = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "oncenum")
                {
                    this.OnceNum = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "baowulea" || xmlNode.Name == "property_lea")
                {
                    this.Baowulea = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "baowustr" || xmlNode.Name == "property_str")
                {
                    this.Baowustr = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "baowuint" || xmlNode.Name == "property_int")
                {
                    this.Baowuint = int.Parse(xmlNode.InnerText);
                }
			}
		}
	}
}
