using System;
using System.Xml;

namespace com.lover.astd.common.model.building
{
	public class Building : AsObject
	{
		private int _buildingId;

		private bool _isOutCity;

		private int _level;

		private int _upgradeCost;

		private int _cdtime;

        private int _state;

		public int BuildingId
		{
			get
			{
				return this._buildingId;
			}
			set
			{
				this._buildingId = value;
			}
		}

		public bool IsOutCity
		{
			get
			{
				return this._isOutCity;
			}
			set
			{
				this._isOutCity = value;
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

		public int Cdtime
		{
			get
			{
				return this._cdtime;
			}
			set
			{
				this._cdtime = value;
			}
		}

        public int State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

		public string getCost()
		{
			bool isOutCity = this.IsOutCity;
			string result;
			if (isOutCity)
			{
				result = string.Format("{0}玉石", this.UpgradeCost);
			}
			else
			{
				result = string.Format("{0}银币", this.UpgradeCost);
			}
			return result;
		}

        //<id>1182886</id>
        //<buildid>29</buildid>
        //<buildname>城墙</buildname>
        //<intro>升级可以增加城防上限。</intro>
        //<cityid>null</cityid>
        //<playerid>227393</playerid>
        //<buildlevel>338</buildlevel>
        //<nextcopper>1404000</nextcopper>
        //<cdtime>522</cdtime>
        //<lastcdtime>0</lastcdtime>
        //<lastupdatetime>0</lastupdatetime>
		public void fillValues(XmlNodeList nodes)
		{
			foreach (XmlNode xmlNode in nodes)
			{
				if (xmlNode.Name == "id")
				{
					base.Id = int.Parse(xmlNode.InnerText);
				}
                else if (xmlNode.Name == "buildid")
                {
                    this.BuildingId = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "buildname")
                {
                    base.Name = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "buildlevel")
                {
                    this.Level = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "nextcopper")
                {
                    this.UpgradeCost = int.Parse(xmlNode.InnerText);
                    this.IsOutCity = false;
                }
                else if (xmlNode.Name == "nextstone")
                {
                    this.UpgradeCost = int.Parse(xmlNode.InnerText);
                    this.IsOutCity = true;
                }
                else if (xmlNode.Name == "cdtime")
                {
                    this.Cdtime = int.Parse(xmlNode.InnerText);
                }
			}
		}

        //<id>32</id>
        //<slaves>0</slaves>
        //<process>20</process>
        //<state>1</state>
        //<totalprocess>20</totalprocess>
        public void fillMoziValues(XmlNodeList nodes)
        {
            foreach (XmlNode xmlNode in nodes)
            {
                if (xmlNode.Name == "state")
                {
                    this.State = int.Parse(xmlNode.InnerText);
                }
            }
        }
	}
}
