using System;

namespace com.lover.astd.common.model.attack
{
    /// <summary>
    /// 地区城池
    /// </summary>
	public class ScopeCity
	{
        /// <summary>
        /// 城池Id
        /// </summary>
		public int cityid;
        /// <summary>
        /// 区域Id
        /// </summary>
		public int scopeid;
        /// <summary>
        /// 玩家Id
        /// </summary>
		public int playerid;
        /// <summary>
        /// 玩家名称
        /// </summary>
		public string playername;
        /// <summary>
        /// 城池等级
        /// </summary>
		public int citylevel;
        /// <summary>
        /// 城池类型,1:玩家,3:守备军,4:禁卫军,6\7\8\11:间谍
        /// </summary>
		public int citytype;
        /// <summary>
        /// 状态,0:正常,1:劳改
        /// </summary>
		public int arreststate;
        /// <summary>
        /// 所属国家
        /// </summary>
		public int nation;
        /// <summary>
        /// 保护CD
        /// </summary>
		public int protectcd;
        /// <summary>
        /// 敌对
        /// </summary>
		public int hostility;
        /// <summary>
        /// 间谍id
        /// </summary>
        public int myspy;
	}
}
