using System;

namespace com.lover.astd.common.model.attack
{
    /// <summary>
    /// 个人令
    /// </summary>
	public class UserToken
	{
        /// <summary>
        /// Id,协议用
        /// </summary>
		public int id;
        /// <summary>
        /// 类型,-1:未解锁,1:建造令,2:破坏令,4:鼓舞令,5:诽谤令,7:窃取令,8:战绩令,9:横扫令
        /// </summary>
		public int tokenid;
        /// <summary>
        /// 名称
        /// </summary>
		public string name;
        /// <summary>
        /// 等级
        /// </summary>
		public int level;
        /// <summary>
        /// 效果
        /// </summary>
        public float effect;
	}
}
