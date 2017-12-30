using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;

namespace com.lover.astd.common.activity
{
    public class RingEvent : MgrBase
    {
        public RingEvent(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Chocolate;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }

        /// <summary>
        /// 新年敲钟活动信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="random_ring_cost">随机敲钟花费金币上限</param>
        /// <param name="other_ring_cost">固定敲钟花费金币上限</param>
        public void getRingEventInfo(ProtocolMgr protocol, ILogger logger, User user, int random_ring_cost, int other_ring_cost)
        {
            string url = "/root/ringEvent!getRingEventInfo.action";
        }

        /// <summary>
        /// 敲钟
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        /// <param name="bellId">0随机 1福 2禄 3寿</param>
        public void ring(ProtocolMgr protocol, ILogger logger, User user, int bellId)
        {
            string url = "/root/ringEvent!ring.action";
            string data = string.Format("bellId={0}", bellId);
        }

        /// <summary>
        /// 打开对联
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public void openReel(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/ringEvent!openReel.action";
        }
    }
}
