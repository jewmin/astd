using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.model.attack
{
    /// <summary>
    /// 决斗信息
    /// </summary>
    public class PkInfo
    {
        /// <summary>
        /// 请求结果,-1:空值,-2:错误,0:正常,1:不在决斗中
        /// </summary>
        public int result;
        /// <summary>
        /// 阶段
        /// </summary>
        public int stage;
        /// <summary>
        /// 结果,0:正在决斗,1:胜利,2:平局,3:失败
        /// </summary>
        public int pkresult;
        /// <summary>
        /// 战绩
        /// </summary>
        public int score;
        /// <summary>
        /// 宝石
        /// </summary>
        public int baoshi;
        /// <summary>
        /// 攻方城防值
        /// </summary>
        public int gong_cityhp;
        /// <summary>
        /// 攻方最大城防值
        /// </summary>
        public int gong_maxcityhp;
        /// <summary>
        /// 攻方玩家名称
        /// </summary>
        public string gong_name;
        /// <summary>
        /// 守方城防值
        /// </summary>
        public int fang_cityhp;
        /// <summary>
        /// 守方最大城防值
        /// </summary>
        public int fang_maxcityhp;
        /// <summary>
        /// 守方玩家名称
        /// </summary>
        public string fang_name;
        /// <summary>
        /// 守方所在地区
        /// </summary>
        public int areaId;
        /// <summary>
        /// 守方所在区域
        /// </summary>
        public int scopeid;
        /// <summary>
        /// 守方所在城池
        /// </summary>
        public int cityid;
    }
}
