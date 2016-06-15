using com.lover.astd.common.model.enumer;
using System;

namespace com.lover.astd.common.manager
{
	public class EnumString
	{
		public static ServerType getServerType(string server_typestr)
		{
			foreach (ServerType serverType in Enum.GetValues(typeof(ServerType)))
			{
				if (server_typestr.Equals(serverType.ToString()))
				{
					return serverType;
				}
			}
			return ServerType.Custom;
		}

        public static string getString(ServerType tp)
        {
            switch (tp)
            {
                case ServerType.Custom:
                    return "自定义";
                case ServerType.YaoWan:
                    return "要玩";
                case ServerType._360:
                    return "360";
                case ServerType.PPS:
                    return "PPS";
                case ServerType.DuoWan:
                    return "多玩";
                case ServerType._37Wan:
                    return "37玩";
                case ServerType.ZhuLang:
                    return "逐浪";
                case ServerType.HuanLang:
                    return "幻浪";
                case ServerType.Baidu:
                    return "百度";
                case ServerType._178:
                    return "178";
                case ServerType.Aoshitang:
                    return "傲世堂";
                case ServerType._51Wan:
                    return "51玩";
                case ServerType.ISpeak:
                    return "ISpeak";
                case ServerType.IFeng:
                    return "凤凰";
                case ServerType._8ZY:
                    return "八爪鱼";
                case ServerType.XdWan:
                    return "兄弟玩";
                case ServerType._4399:
                    return "4399";
                case ServerType._6711:
                    return "6711";
                case ServerType._6998:
                    return "6998";
                case ServerType.KuWan8:
                    return "酷玩吧";
                case ServerType.PeiYou:
                    return "陪游";
                case ServerType.VeryCd:
                    return "电驴";
                case ServerType.WebXGame:
                    return "E侠";
                case ServerType.TgBus:
                    return "电玩巴士";
                case ServerType._56uu:
                    return "56uu";
                case ServerType.Jinjuzi:
                    return "金桔网";
                case ServerType._3896:
                    return "3896";
                case ServerType.Uz73:
                    return "悠哉游戏";
                case ServerType.MiYou:
                    return "蜜柚游戏";
                case ServerType.RenRen:
                    return "人人网";
                case ServerType.Kunlun:
                    return "昆仑";
                case ServerType.NiuA:
                    return "牛A";
                case ServerType.Game2:
                    return "哥们";
                case ServerType._91:
                    return "91游戏";
                case ServerType.Cga:
                    return "浩方游戏";
                case ServerType.Plu:
                    return "PLU";
                case ServerType._53Wan:
                    return "53玩";
                case ServerType._789hi:
                    return "玉米游戏";
                case ServerType.Lequ:
                    return "乐趣网";
                case ServerType.Letou8:
                    return "乐透游戏";
                case ServerType.SnsTele:
                    return "泰勒互动";
                case ServerType.Kuwo:
                    return "酷我游戏";
                case ServerType._96Pk:
                    return "96PK";
                case ServerType._51:
                    return "51游戏";
                case ServerType._29ww:
                    return "29ww";
                case ServerType._91wan:
                    return "91wan";
                case ServerType.Tianya:
                    return "天涯";
                case ServerType.Funshion:
                    return "风行";
                case ServerType.Pptv:
                    return "PPTV";
                case ServerType.Kuaiwan:
                    return "快玩";
                case ServerType.Huolawan:
                    return "火辣玩";
                case ServerType.Youwo:
                    return "游窝游戏";
                case ServerType.Kugou:
                    return "酷狗游戏";
                default:
                    return tp.ToString();
            }
        }

        public static string getString(AccountStatus status)
        {
            switch (status)
            {
                case AccountStatus.STA_not_start:
                    return "挂机未运行";
                case AccountStatus.STA_initial:
                    return "初始化";
                case AccountStatus.STA_running:
                    return "挂机运行中";
                case AccountStatus.STA_relogin:
                    return "等待重登录";
                case AccountStatus.STA_stopped:
                    return "挂机停止中";
                case AccountStatus.STA_stopped_login_verify:
                    return "等待登录验证码";
                case AccountStatus.STA_login_failed:
                    return "登录失败";
                case AccountStatus.STA_login_failed_max:
                    return "达到最大登录次数";
                case AccountStatus.STA_stopped_game_verify:
                    return "等待游戏验证码";
                case AccountStatus.STA_due_time:
                    return "已过期";
                default:
                    return "";
            }
        }
	}
}
