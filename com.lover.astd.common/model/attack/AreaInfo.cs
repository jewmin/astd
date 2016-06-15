using System;

namespace com.lover.astd.common.model.attack
{
    /// <summary>
    /// 地区
    /// </summary>
	public class AreaInfo
	{
        /// <summary>
        /// 地区Id
        /// </summary>
		public int areaid;
        /// <summary>
        /// 所属国家
        /// </summary>
		public int nation;
        /// <summary>
        /// 地区名称
        /// </summary>
		public string areaname;
        /// <summary>
        /// 可进入
        /// </summary>
		public bool enterable;
        /// <summary>
        /// 可移动
        /// </summary>
		public bool transferable;
        /// <summary>
        /// 本人所在地
        /// </summary>
		public bool isselfarea;
        /// <summary>
        /// 区域数量
        /// </summary>
		public int scopecount;
        /// <summary>
        /// 资源 100:间谍
        /// </summary>
        public int ziyuan;
	}
}
