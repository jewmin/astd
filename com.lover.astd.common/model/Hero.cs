using System;
using System.Xml;

namespace com.lover.astd.common.model
{
	public class Hero : AsObject
	{
		private int _tong;

		private int _yong;

		private int _zhi;

		private int _star;

		private int _level;

		private int _shiftLevel;

		private int _experience;

		private int _trainPositionId;

		public int Tong
		{
			get
			{
				return this._tong;
			}
			set
			{
				this._tong = value;
			}
		}

		public int Yong
		{
			get
			{
				return this._yong;
			}
			set
			{
				this._yong = value;
			}
		}

		public int Zhi
		{
			get
			{
				return this._zhi;
			}
			set
			{
				this._zhi = value;
			}
		}

		public int Star
		{
			get
			{
				return this._star;
			}
			set
			{
				this._star = value;
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

		public int ShiftLevel
		{
			get
			{
				return this._shiftLevel;
			}
			set
			{
				this._shiftLevel = value;
			}
		}

		public int Experience
		{
			get
			{
				return this._experience;
			}
			set
			{
				this._experience = value;
			}
		}

		public int TrainPositionId
		{
			get
			{
				return this._trainPositionId;
			}
			set
			{
				this._trainPositionId = value;
			}
		}

		public string Desc
		{
			get
			{
				return string.Format("{0}  {1}级", this._name, this._level);
			}
		}

		public void fillValues(XmlNodeList nodes)
		{
			foreach (XmlNode xmlNode in nodes)
			{
				if (xmlNode.Name == "generalid")
				{
					base.Id = int.Parse(xmlNode.InnerText);
				}
                else if (xmlNode.Name == "generalname")
                {
                    base.Name = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "generallevel")
                {
                    this.Level = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "generalexp")
                {
                    this.Experience = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "trainposid")
                {
                    this.TrainPositionId = int.Parse(xmlNode.InnerText);
                }
                else if (xmlNode.Name == "shiftlv")
                {
                    this.ShiftLevel = int.Parse(xmlNode.InnerText);
                }
			}
		}
	}
}
