using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.model.enumer
{
    public enum ExeCode
    {
        instant             = -1,   //瞬间 100ms
        immediate           = -2,   //马上 2s
        one_minute          = -3,   //一分钟 1min
        next_dash           = -4,   //下一个 10s, 40s
        next_halfhour       = -5,   //半小时后 [0, 30min]
        next_hour           = -6,   //一小时后 [0, 1h]
        an_hour_later       = -7,   //一个小时 1h
        next_boat           = -8,   //下一个龙舟比赛时间 10点, 15点, 21点
        next_dinner         = -9,   //下一个宴会时间 10点, 19点
        next_gongjian_time  = -10,  //下一个攻坚时间 10点, 15点, 20点30分
        next_day            = -11,  //第二天5点
        next_day_eight      = -12   //第二天8点
    }
}
