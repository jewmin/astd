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

		public void fillValues(XmlNodeList nodes)
		{
			foreach (XmlNode xmlNode in nodes)
			{
				bool flag = xmlNode.Name == "id";
				if (flag)
				{
					base.Id = int.Parse(xmlNode.InnerText);
				}
				else
				{
					bool flag2 = xmlNode.Name == "buildid";
					if (flag2)
					{
						this.BuildingId = int.Parse(xmlNode.InnerText);
					}
					else
					{
						bool flag3 = xmlNode.Name == "buildname";
						if (flag3)
						{
							base.Name = xmlNode.InnerText;
						}
						else
						{
							bool flag4 = xmlNode.Name == "buildlevel";
							if (flag4)
							{
								this.Level = int.Parse(xmlNode.InnerText);
							}
							else
							{
								bool flag5 = xmlNode.Name == "nextcopper";
								if (flag5)
								{
									this.UpgradeCost = int.Parse(xmlNode.InnerText);
									this.IsOutCity = false;
								}
								else
								{
									bool flag6 = xmlNode.Name == "nextstone";
									if (flag6)
									{
										this.UpgradeCost = int.Parse(xmlNode.InnerText);
										this.IsOutCity = true;
									}
									else
									{
										bool flag7 = xmlNode.Name == "cdtime";
										if (flag7)
										{
											this.Cdtime = int.Parse(xmlNode.InnerText);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
