using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using System;
using System.Drawing;
using System.Xml;

namespace com.lover.astd.common.logic
{
	public class TroopMgr : MgrBase
	{
		private long _freeArmyCd;

		public TroopMgr(TimeMgr tmrMgr, ServiceFactory factory)
		{
			this._logColor = Color.DarkSalmon;
			this._tmrMgr = tmrMgr;
			this._factory = factory;
		}

        /// <summary>
        /// 确保兵力
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="min_troop_percent"></param>
        /// <returns></returns>
		public int makeSureForce(ProtocolMgr protocol, ILogger logger, double min_troop_percent = 0.5)
		{
			User user = protocol.getUser();
            if ((double)user.Forces * 1.0 / (double)user.MaxForces < min_troop_percent && (this._freeArmyCd == 0 || (this._freeArmyCd > 0 && this._tmrMgr.TimeStamp > this._freeArmyCd)))
			{
				int freeArmyCount = 0;
				int freeArmy = this.getFreeArmy(protocol, logger, out freeArmyCount);
				if (freeArmy == 0)
				{
					if (freeArmyCount == 0)
					{
						this._freeArmyCd = this._tmrMgr.TimeStamp + base.next_day();
					}
					else
					{
						this._freeArmyCd = this._tmrMgr.TimeStamp + 1800000;
					}
				}
                else if (freeArmy != 1)
                {
                    if (freeArmy == 2)
                    {
                        this._freeArmyCd = this._tmrMgr.TimeStamp + 1800000;
                    }
                    else if (freeArmy == 3)
                    {
                        this._freeArmyCd = this._tmrMgr.TimeStamp + base.next_day();
                    }
                }
			}
			int forces = (int)((double)user.MaxForces * min_troop_percent);
            int needcopper = (int)((forces - user.Forces) * 0.5);
            if (user.Silver < needcopper)
            {
                int times = needcopper / 10000000;
                this._factory.getMiscManager().ticketExchangeMoney(protocol, logger, times);
            }
			if (user.Forces < forces && !this.draught(protocol, logger, forces - user.Forces))
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

        /// <summary>
        /// 义兵
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="freeArmyCount"></param>
        /// <returns></returns>
		private int getFreeArmy(ProtocolMgr protocol, ILogger logger, out int freeArmyCount)
		{
			freeArmyCount = 0;
			string url = "/root/mainCity!rightArmy.action";
			ServerResult xml = protocol.getXml(url, "征义兵");
			if (xml == null)
			{
				return 1;
			}
            else if (!xml.CmdSucceed)
            {
                if (xml.CmdError.Contains("征义兵CD中") || xml.CmdError.Contains("征義兵CD中"))
                {
                    return 2;
                }
                if (xml.CmdError.Contains("已用完") || xml.CmdError.Contains("已用完"))
                {
                    return 3;
                }
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/rightnum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out freeArmyCount);
            }
            else
            {
                freeArmyCount = 0;
            }
            int forces = 0;
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/forces");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out forces);
            }
            base.logInfo(logger, string.Format("自动征义兵, 兵力+{0}, 义兵次数还剩{1}次", forces, freeArmyCount));
            return 0;
		}

        /// <summary>
        /// 征兵
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="force_number"></param>
        /// <returns></returns>
		protected bool draught(ProtocolMgr protocol, ILogger logger, int force_number)
		{
			string url = "/root/mainCity!draught.action";
			string data = "forceNum=" + force_number;
			ServerResult serverResult = protocol.postXml(url, data, "征兵");
			if (serverResult == null || !serverResult.CmdSucceed)
			{
				return false;
			}
			else
			{
				XmlDocument cmdResult = serverResult.CmdResult;
				return true;
			}
		}
	}
}
