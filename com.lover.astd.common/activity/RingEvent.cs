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
        class RingState : XmlObject
        {
            public int id { get; set; }
            public int state { get; set; }
            public int need { get; set; }

            public override void Parse(System.Xml.XmlNode node)
            {
                throw new NotImplementedException();
            }

            public override bool CanAdd()
            {
                throw new NotImplementedException();
            }
        }

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
            ServerResult xml = protocol.getXml(url, "新年敲钟 - 活动面板");
            if (xml == null || !xml.CmdSucceed) return;
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
            ServerResult xml = protocol.postXml(url, data, "新年敲钟 - 敲钟");
            if (xml == null || !xml.CmdSucceed) return;
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
            ServerResult xml = protocol.getXml(url, "新年敲钟 - 打开对联");
            if (xml == null || !xml.CmdSucceed) return;
        }
    }
}
