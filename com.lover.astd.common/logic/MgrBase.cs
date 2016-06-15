using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace com.lover.astd.common.logic
{
	public class MgrBase
	{
		protected TimeMgr _tmrMgr;

		protected ServiceFactory _factory;

		protected Color _logColor = Color.Black;

		protected void logInfo(ILogger logger, string text)
		{
			logger.log(text, this._logColor);
		}

        protected void logInfo(ILogger logger, string text, Color color)
        {
            logger.log(text, color);
        }

		protected long immediate()
		{
			return 2000L;
		}

		protected long an_hour_later()
		{
			return 3600000L;
		}

		protected long next_halfhour()
		{
			DateTime dateTimeNow = this._tmrMgr.DateTimeNow;
			bool flag = dateTimeNow.Minute < 30;
			int num;
			if (flag)
			{
				num = 30 - dateTimeNow.Minute;
			}
			else
			{
				num = 60 - dateTimeNow.Minute;
			}
			return (long)(num * 60 * 1000);
		}

		protected long next_hour()
		{
			int num = 60 - this._tmrMgr.DateTimeNow.Minute;
			return (long)(num * 60 * 1000);
		}

		protected long next_day()
		{
			DateTime dateTimeNow = this._tmrMgr.DateTimeNow;
			bool flag = dateTimeNow.Hour < 5;
			int num;
			if (flag)
			{
				num = 5 - dateTimeNow.Hour - 1;
			}
			else
			{
				num = 29 - dateTimeNow.Hour - 1;
			}
			int num2 = 60 - dateTimeNow.Minute;
			return (long)((num * 60 * 60 + (num2 + 30) * 60) * 1000);
		}

        /// <summary>
        /// 第二天早上8点
        /// </summary>
        /// <returns></returns>
        protected long next_day_eight()
        {
            DateTime dateTimeNow = this._tmrMgr.DateTimeNow;
            int hour;
            if (dateTimeNow.Hour < 8)
            {
                hour = 8 - dateTimeNow.Hour - 1;
            }
            else
            {
                hour = 24 + 8 - dateTimeNow.Hour - 1;
            }
            int minute = 60 - dateTimeNow.Minute;
            return (long)((hour * 3600 + (minute + 30) * 60) * 1000);
        }

		protected long next_dinner()
		{
			DateTime dateTimeNow = this._tmrMgr.DateTimeNow;
			bool flag = dateTimeNow.Hour < 10;
			int num;
			if (flag)
			{
				num = 10 - dateTimeNow.Hour - 1;
			}
			else
			{
				bool flag2 = dateTimeNow.Hour < 19;
				if (flag2)
				{
					num = 19 - dateTimeNow.Hour - 1;
				}
				else
				{
					num = 34 - dateTimeNow.Hour - 1;
				}
			}
			int num2 = 60 - dateTimeNow.Minute;
			return (long)((num * 60 * 60 + (num2 + 1) * 60) * 1000);
		}

		protected long next_gongjian()
		{
			DateTime dateTimeNow = this._tmrMgr.DateTimeNow;
			bool flag = dateTimeNow.Hour < 10;
			int num;
			if (flag)
			{
				num = 10 - dateTimeNow.Hour - 1;
			}
			else
			{
				bool flag2 = dateTimeNow.Hour < 19;
				if (flag2)
				{
					num = 19 - dateTimeNow.Hour - 1;
				}
				else
				{
					num = 34 - dateTimeNow.Hour - 1;
				}
			}
			int num2 = 60 - dateTimeNow.Minute;
			return (long)((num * 60 * 60 + (num2 + 1) * 60) * 1000);
		}

		protected bool in_gongjian_time()
		{
			DateTime dateTimeNow = this._tmrMgr.DateTimeNow;
			int hour = dateTimeNow.Hour;
			int minute = dateTimeNow.Minute;
			return (hour == 10 && minute < 30) || (hour == 15 && minute < 30) || (hour == 20 && minute > 30);
		}

		protected long smaller_time(long time1, long time2)
		{
			bool flag = time1 < time2;
			long result;
			if (flag)
			{
				result = time1;
			}
			else
			{
				result = time2;
			}
			return result;
		}

		protected string formatTime(long milliseconds)
		{
			long num = milliseconds / 60000L;
			long num2 = milliseconds / 1000L - num * 60L;
			return string.Format("{0:d2}:{1:d2}", num, num2);
		}

		protected void tryUseCard(ProtocolMgr protocol, ILogger logger, XmlDocument xml)
		{
			XmlNodeList xmlNodeList = xml.SelectNodes("/results/card");
			bool flag = xmlNodeList == null;
			if (!flag)
			{
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					bool flag2 = xmlNode != null && xmlNode.HasChildNodes;
					if (flag2)
					{
						int num = -1;
						int num2 = 0;
						int num3 = 0;
						XmlNode xmlNode2 = xmlNode.SelectSingleNode("cardtype");
						bool flag3 = xmlNode2 != null;
						if (flag3)
						{
							int.TryParse(xmlNode2.InnerText, out num);
						}
						XmlNode xmlNode3 = xmlNode.SelectSingleNode("cardnum");
						bool flag4 = xmlNode3 != null;
						if (flag4)
						{
							int.TryParse(xmlNode3.InnerText, out num2);
						}
						XmlNode xmlNode4 = xmlNode.SelectSingleNode("active");
						bool flag5 = xmlNode4 != null;
						if (flag5)
						{
							int.TryParse(xmlNode4.InnerText, out num3);
						}
						XmlNode xmlNode5 = xmlNode.SelectSingleNode("cardname");
						bool flag6 = xmlNode5 != null;
						if (flag6)
						{
							string innerText = xmlNode5.InnerText;
						}
						bool flag7 = num >= 0 && num2 > 0 && num3 == 0;
						if (flag7)
						{
							this.useCardByType(protocol, logger, num);
						}
					}
				}
			}
		}

		protected void tryCancelCard(ProtocolMgr protocol, ILogger logger, XmlDocument xml)
		{
			XmlNodeList xmlNodeList = xml.SelectNodes("/results/card");
			bool flag = xmlNodeList == null;
			if (!flag)
			{
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					bool flag2 = xmlNode != null && xmlNode.HasChildNodes;
					if (flag2)
					{
						int num = -1;
						int num2 = 0;
						int num3 = 0;
						XmlNode xmlNode2 = xmlNode.SelectSingleNode("cardtype");
						bool flag3 = xmlNode2 != null;
						if (flag3)
						{
							int.TryParse(xmlNode2.InnerText, out num);
						}
						XmlNode xmlNode3 = xmlNode.SelectSingleNode("cardnum");
						bool flag4 = xmlNode3 != null;
						if (flag4)
						{
							int.TryParse(xmlNode3.InnerText, out num2);
						}
						XmlNode xmlNode4 = xmlNode.SelectSingleNode("active");
						bool flag5 = xmlNode4 != null;
						if (flag5)
						{
							int.TryParse(xmlNode4.InnerText, out num3);
						}
						XmlNode xmlNode5 = xmlNode.SelectSingleNode("cardname");
						bool flag6 = xmlNode5 != null;
						if (flag6)
						{
							string innerText = xmlNode5.InnerText;
						}
						bool flag7 = num >= 0 && num3 != 0;
						if (flag7)
						{
							this.useCardByType(protocol, logger, num);
						}
					}
				}
			}
		}

		protected void useCardByType(ProtocolMgr protocol, ILogger logger, int cardType)
		{
			string url = "/root/goods!useCardByTypes.action";
			string data = "cardTypes=" + cardType;
			ServerResult serverResult = protocol.postXml(url, data, "使用或取消卡");
			bool flag = serverResult == null;
			if (!flag)
			{
				bool flag2 = !serverResult.CmdSucceed;
				if (!flag2)
				{
					XmlDocument cmdResult = serverResult.CmdResult;
					string[] array = new string[]
					{
						"",
						"商人召唤卡",
						"强化暴击卡",
						"强化打折卡",
						"兵器升级卡",
						"兵器暴击卡",
						"政绩翻倍卡",
						"征收翻倍卡",
						"纺织翻倍卡"
					};
					bool flag3 = cardType > 0 && cardType < array.Length;
					if (flag3)
					{
						this.logInfo(logger, string.Format("使用[{0}]", array[cardType]));
					}
					else
					{
						this.logInfo(logger, string.Format("使用[卡type={0}]", cardType));
					}
				}
			}
		}

		protected List<int> generateIds(string id_string)
		{
			List<int> list = new List<int>();
			bool flag = id_string == null || id_string == "";
			List<int> result;
			if (flag)
			{
				result = list;
			}
			else
			{
				string[] array = id_string.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				int num2;
				for (int i = 0; i < array2.Length; i = num2 + 1)
				{
					string text = array2[i];
					bool flag2 = text != null && !(text == "");
					if (flag2)
					{
						int num = 0;
						int.TryParse(text, out num);
						bool flag3 = num != 0;
						if (flag3)
						{
							list.Add(num);
						}
					}
					num2 = i;
				}
				result = list;
			}
			return result;
		}
	}
}
