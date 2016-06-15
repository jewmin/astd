using com.lover.astd.common.logic;
using com.lover.astd.common.model.attack;
using com.lover.astd.common.model.battle;
using com.lover.astd.common.model.building;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace com.lover.astd.common.model
{
	public class User
	{
		public static int Level_Trade = 40;

		public static int Level_Weave = 82;

		public static int Level_Gem_Events = 130;

		public static int Level_Jail = 180;

		private List<string> _ui_render_queue = new List<string>();

		public bool _version_gift;

		private bool _gotLoginReward = true;

		private int _id;

		private string _username;

		private int _year;

		private int _season;

		private string[] _seasonNames = new string[]
		{
			"春",
			"夏",
			"秋",
			"冬"
		};

		private int _nationInt;

		private string[] _nations = new string[]
		{
			"魏国",
			"蜀国",
			"吴国"
		};

		private string _nation;

		private string _group;

		private float _grainPrice;

		private int _price;

		private int _magic;

		private bool _canUseGoldQh;

		private int _level;

		private int _gold;

		public int _sysgold;

		public int _usergold;

		private int _silver;

		private int _maxSilver;

		private int _food;

		private int _maxFood;

		private int _forces;

		private int _maxForces;

		private int _credit;

		private int _prestige;

		private int _token;

		private long _tokenCdTime;

		private bool _tokenCdFlag;

		private int _attackOrders;

		private int _stone;

		private int _gems;

		private List<ActivityType> _activities = new List<ActivityType>();

		private int _curMovable;

		public int _stock_cd;

		public bool _can_get_nation_task_reward;

		public bool _is_new_trade;

		public int _max_active = 420;

		public int _can_buy_active_count;

		public bool _arch_have_got_all_pieces;

		public int _impose_count;

		public long _impose_cdtime;

		public bool _impose_hasCd;

		public int _impose_now_loyalty;

		public int _impose_force_gold;

		public bool _impose_isImposeEvent;

		public int _impose_imposeEvent_numNow;

		public int _impose_imposeEvent_numMax = 80;

		public string _impose_imposeEvent_rewardStatus = "";

		public bool _impose_isRepayEvent;

		public int _impose_repayEvent_numNow;

		public int _impose_repayEvent_numMax = 80;

		public string _impose_repayEvent_rewardStatus = "";

		public int[] _fete_min_levels;

		public string[] _fete_names;

		public int[] _fete_gold;

		public int[] _fete_now_gold;

		public int[] _fete_now_free_times;

        public bool _is_fete_activity;

		public int _dinner_count;

		public int _dinner_in_time;

		public bool _dinner_joined;

		public string _dinner_team_id;

		public string _dinner_team_creator;

		public List<Building> _buildings;

		public Dictionary<int, BuildingLine> _buildingLines;

		private long _tuFeiCd;

		private bool _tuFeiCdFlag;

		private int _tufeiNumber;

		private int _tufeiCredit;

		private List<Hero> _heroes;

		public bool _battle_got_weapon_event_free_token;

		public int _battle_free_force_token;

		public string _battle_current_army_id;

		public int _battle_max_army_id;

		public List<Npc> _all_armys;

		public List<Npc> _all_npcs;
        /// <summary>
        /// 当前城防
        /// </summary>
		public int _attack_cityHp;
        /// <summary>
        /// 最大城防
        /// </summary>
		public int _attack_maxCityHp;
        /// <summary>
        /// 城防回复时间
        /// </summary>
        public int _attack_cityHpRecoverCd;
        /// <summary>
        /// 战绩
        /// </summary>
		public int _attack_battleScore;
        /// <summary>
        /// 战绩宝箱
        /// </summary>
        public int _attack_battleScore_box;
        /// <summary>
        /// 决斗战旗
        /// </summary>
        public int _attack_daojuFlag;
        /// <summary>
        /// 诱敌锦囊
        /// </summary>
        public int _attack_daojuJin;
        /// <summary>
        /// 战绩奖励,0:没有,1:可领取,2:已领取
        /// </summary>
		public int[] _attack_battleScore_awardGot;
        /// <summary>
        /// 上轮排名奖励,0:没有,1:可领取,2:已领取
        /// </summary>
        public int _attack_last_awardGot;
        /// <summary>
        /// 鼓舞状态,0:没有,1:有
        /// </summary>
		public int _attack_inspiredState;
        /// <summary>
        /// 鼓舞等级
        /// </summary>
		public float _attack_inspiredEffect;
        /// <summary>
        /// 战绩令有效时间
        /// </summary>
		public int _attack_scoreTokencd;

		public bool _jail_can_tech;
        /// <summary>
        /// 玩家状态,>10:逃跑CD,=100:被抓
        /// </summary>
		public int _arrest_state;
        /// <summary>
        /// 在新世界
        /// </summary>
		public bool _inNewArea;
        /// <summary>
        /// 移动CD
        /// </summary>
		public int _attack_transfer_cd;
        /// <summary>
        /// 本人所在地区Id
        /// </summary>
		public int _attack_selfCityId;
        /// <summary>
        /// 间谍所在地区
        /// </summary>
        public string _attack_spy_city;

		public int _attack_remain_cityevent_num;

		public string _attack_current_cityevent_name;

		public int _attack_current_cityevent_cdtime;

		public int _attack_can_reward_cityevent;
        /// <summary>
        /// 有监狱
        /// </summary>
		public bool _attack_have_jail;
        /// <summary>
        /// 集结地区
        /// </summary>
		public string _attack_nation_battle_city;
        /// <summary>
        /// 攻坚状态,0:没到攻坚,3:完成攻坚
        /// </summary>
		public int _attack_gongjian_status;
        /// <summary>
        /// 攻坚剩余时间
        /// </summary>
		public long _attack_nationBattleRemainTime;
        /// <summary>
        /// 所有地区
        /// </summary>
		public List<AreaInfo> _attack_all_areas;
        /// <summary>
        /// 个人令
        /// </summary>
		public List<UserToken> _attack_user_tokens;
        /// <summary>
        /// 所有地区所属国家
        /// </summary>
		private Dictionary<int, int> _newAreaCityInfo;
        /// <summary>
        /// 途经地区
        /// </summary>
		public List<AreaInfo> _attack_move_path;
        /// <summary>
        /// 悬赏目标Id
        /// </summary>
		public int _attack_cityevent_targetid;
        /// <summary>
        /// 悬赏目标所在地区Id
        /// </summary>
		public int _attack_cityevent_target_areaid;
        /// <summary>
        /// 悬赏目标所在区域Id
        /// </summary>
		public int _attack_cityevent_target_scopeid;
        /// <summary>
        /// 天降奇兵目标Id
        /// </summary>
		public int _attack_nationevent_targetid;
        /// <summary>
        /// 天降奇兵目标所在地区Id
        /// </summary>
		public int _attack_nationevent_target_areaid;
        /// <summary>
        /// 天降奇兵目标所在区域Id
        /// </summary>
		public int _attack_nationevent_target_scopeid;
        /// <summary>
        /// 诱敌目标所在地区Id
        /// </summary>
        public int _attack_xiudi_target_areaid;
        /// <summary>
        /// 诱敌目标所在区域Id
        /// </summary>
        public int _attack_xiudi_target_scopeid;
        /// <summary>
        /// 诱敌目标玩家Id
        /// </summary>
        public int _attack_xiudi_target_playerid;
        /// <summary>
        /// 决斗目标所在地区Id
        /// </summary>
        public int _attack_juedou_target_areaid;
        /// <summary>
        /// 决斗目标所在区域Id
        /// </summary>
        public int _attack_juedou_target_scopeid;
        /// <summary>
        /// 决斗目标城池Id
        /// </summary>
        public int _attack_juedou_target_cityid;
        
		public int _storeGems;

		public int _equipGems;

		public int _storeUsedSize;

		public int _storeTotalSize;

		public List<Equipment> _qhEquips;

		public List<Equipment> _mhEquips;

		public List<Equipment> _storeEquips;

		public string _weapon_info;

		public string _army_reach;

		private ServiceFactory _factory;
        /// <summary>
        /// 登录奖励,false:未领取,true:已领取
        /// </summary>
		public bool GotLoginReward
		{
			get
			{
				return this._gotLoginReward;
			}
			set
			{
				this._gotLoginReward = value;
			}
		}
        /// <summary>
        /// 角色Id
        /// </summary>
		public int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
        /// <summary>
        /// 角色名
        /// </summary>
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
			}
		}
        /// <summary>
        /// 年代
        /// </summary>
		public int Year
		{
			get
			{
				return this._year;
			}
			set
			{
				this._year = value;
			}
		}
        /// <summary>
        /// 季节,1:春,2:夏,3:秋,4:冬
        /// </summary>
		public int Season
		{
			get
			{
				return this._season;
			}
			set
			{
				this._season = value;
			}
		}
        /// <summary>
        /// 季节
        /// </summary>
		public string SeasonName
		{
			get
			{
				if (this._season < 1)
				{
					return "";
				}
				else
				{
					return this._seasonNames[this._season - 1];
				}
			}
		}
        /// <summary>
        /// 所属国家,0:中立,1:魏,2:蜀,3:吴
        /// </summary>
		public int NationInt
		{
			get
			{
				return this._nationInt;
			}
			set
			{
				this._nationInt = value;
			}
		}
        /// <summary>
        /// 所属国家
        /// </summary>
		public string Nation
		{
			get
			{
				return this._nation;
			}
			set
			{
				this._nation = value;
			}
		}

		public string Group
		{
			get
			{
				return this._group;
			}
			set
			{
				this._group = value;
			}
		}

		public float GrainPrice
		{
			get
			{
				return this._grainPrice;
			}
			set
			{
				this._grainPrice = value;
			}
		}

		public int Price
		{
			get
			{
				return this._price;
			}
			set
			{
				this._price = value;
			}
		}
        /// <summary>
        /// 魔力值
        /// </summary>
		public int Magic
		{
			get
			{
				return this._magic;
			}
			set
			{
				this._magic = value;
			}
		}

		public bool CanUseGoldQh
		{
			get
			{
				return this._canUseGoldQh;
			}
			set
			{
				this._canUseGoldQh = value;
			}
		}
        /// <summary>
        /// 当前等级
        /// </summary>
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
			}
		}
        /// <summary>
        /// 当前金币
        /// </summary>
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				this._gold = value;
			}
		}
        /// <summary>
        /// 当前银币
        /// </summary>
		public int Silver
		{
			get
			{
				return this._silver;
			}
			set
			{
				this._silver = value;
			}
		}
        /// <summary>
        /// 最大银币
        /// </summary>
		public int MaxSilver
		{
			get
			{
				return this._maxSilver;
			}
			set
			{
				this._maxSilver = value;
			}
		}

		public int Food
		{
			get
			{
				return this._food;
			}
			set
			{
				this._food = value;
			}
		}

		public int MaxFood
		{
			get
			{
				return this._maxFood;
			}
			set
			{
				this._maxFood = value;
			}
		}
        /// <summary>
        /// 当前兵力
        /// </summary>
		public int Forces
		{
			get
			{
				return this._forces;
			}
			set
			{
				this._forces = value;
			}
		}
        /// <summary>
        /// 最大兵力
        /// </summary>
		public int MaxForces
		{
			get
			{
				return this._maxForces;
			}
			set
			{
				this._maxForces = value;
			}
		}
        /// <summary>
        /// 军功
        /// </summary>
        public int Credit
		{
			get
			{
				return this._credit;
			}
			set
			{
				this._credit = value;
			}
		}

		public int Prestige
		{
			get
			{
				return this._prestige;
			}
			set
			{
				this._prestige = value;
			}
		}
        /// <summary>
        /// 军令
        /// </summary>
		public int Token
		{
			get
			{
				return this._token;
			}
			set
			{
				this._token = value;
			}
		}
        /// <summary>
        /// 攻击CD时间
        /// </summary>
		public long TokenCdTime
		{
			get
			{
				return this._tokenCdTime;
			}
			set
			{
				this._tokenCdTime = value;
			}
		}

		public bool TokenCdFlag
		{
			get
			{
				return this._tokenCdFlag && this._tokenCdTime > 0L;
			}
			set
			{
				this._tokenCdFlag = value;
			}
		}
        /// <summary>
        /// 攻击令
        /// </summary>
		public int AttackOrders
		{
			get
			{
				return this._attackOrders;
			}
			set
			{
				this._attackOrders = value;
			}
		}
        /// <summary>
        /// 玉石
        /// </summary>
		public int Stone
		{
			get
			{
				return this._stone;
			}
			set
			{
				this._stone = value;
			}
		}
        /// <summary>
        /// 宝石
        /// </summary>
		public int Gems
		{
			get
			{
				return this._gems;
			}
			set
			{
				this._gems = value;
			}
		}
        /// <summary>
        /// 行动力
        /// </summary>
		public int CurMovable
		{
			get
			{
				return this._curMovable;
			}
			set
			{
				this._curMovable = value;
			}
		}
        /// <summary>
        /// 突飞CD时间
        /// </summary>
		public long TuFeiCd
		{
			get
			{
				return this._tuFeiCd;
			}
			set
			{
				this._tuFeiCd = value;
			}
		}
        /// <summary>
        /// 突飞CD
        /// </summary>
		public bool TuFeiCdFlag
		{
			get
			{
				return this._tuFeiCdFlag;
			}
			set
			{
				this._tuFeiCdFlag = value;
			}
		}
        /// <summary>
        /// 突飞令
        /// </summary>
		public int TufeiNumber
		{
			get
			{
				return this._tufeiNumber;
			}
			set
			{
				this._tufeiNumber = value;
			}
		}
        /// <summary>
        /// 突飞所需军功
        /// </summary>
		public int TufeiCredit
		{
			get
			{
				return this._tufeiCredit;
			}
			set
			{
				this._tufeiCredit = value;
			}
		}
        /// <summary>
        /// 武将
        /// </summary>
		public List<Hero> Heroes
		{
			get
			{
				return this._heroes;
			}
			set
			{
				this._heroes = value;
			}
		}

		public void addUiToQueue(string ui_key)
		{
			bool flag = !this._ui_render_queue.Contains(ui_key);
			if (flag)
			{
				this._ui_render_queue.Add(ui_key);
			}
		}

		public string popUiFromQueue()
		{
			bool flag = this._ui_render_queue.Count > 0;
			string result;
			if (flag)
			{
				string text = this._ui_render_queue[0];
				this._ui_render_queue.RemoveAt(0);
				result = text;
			}
			else
			{
				result = "";
			}
			return result;
		}

		public bool isActivityRunning(ActivityType tp)
		{
			foreach (ActivityType current in this._activities)
			{
				if (tp == current)
				{
					return true;
				}
			}
            return false;
		}

		public void clearActivities()
		{
			this._activities.Clear();
		}

		public void removeActivity(ActivityType tp)
		{
			this._activities.Remove(tp);
		}

		public void addActivity(ActivityType tp)
		{
			bool flag = false;
			foreach (ActivityType current in this._activities)
			{
				if (tp == current)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._activities.Add(tp);
			}
		}

		public AreaInfo getAreaById(int areaId)
		{
			foreach (AreaInfo current in this._attack_all_areas)
			{
				if (current.areaid == areaId)
				{
                    return current;
				}
			}
            return null;
		}

		public AreaInfo getAreaByName(string areaName)
		{
			if (areaName == null || areaName == "")
			{
				return null;
			}
            foreach (AreaInfo current in this._attack_all_areas)
            {
                if (current.areaname == areaName)
                {
                    return current;
                }
            }
            return null;
		}

		public Dictionary<int, int> getNewAreaCityInfo()
		{
			return this._newAreaCityInfo;
		}

		public void setNewAreaCityNation(int cityid, int nation)
		{
			if (cityid >= 101 && cityid <= 136)
			{
				this._newAreaCityInfo[cityid] = nation;
			}
		}

		public AreaInfo _get_attack_move_target()
		{
			if (this._attack_move_path.Count == 0)
			{
				return null;
			}
			else
			{
				return this._attack_move_path[this._attack_move_path.Count - 1];
			}
		}

		public void _remove_attack_move_now(int area_id)
		{
			bool flag = this._attack_move_path.Count == 0;
			if (!flag)
			{
				bool flag2 = area_id == this._attack_move_path[0].areaid;
				if (flag2)
				{
					this._attack_move_path.RemoveAt(0);
				}
			}
		}

		public string _hero_info()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (int i = 0; i < this._heroes.Count; i = num + 1)
			{
				Hero hero = this._heroes[i];
				stringBuilder.AppendFormat("{0}|{1}|{2};", hero.Id, hero.Name, hero.Level);
				num = i;
			}
			return stringBuilder.ToString();
		}

		public string _build_info()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (int i = 0; i < this._buildings.Count; i = num + 1)
			{
				Building building = this._buildings[i];
				stringBuilder.AppendFormat("{0}|{1}|{2};", building.Id, building.Name, building.Level);
				num = i;
			}
			return stringBuilder.ToString();
		}

		public string _qh_info()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (int i = 0; i < this._qhEquips.Count; i = num + 1)
			{
				Equipment equipment = this._qhEquips[i];
				bool flag = equipment.Quality == EquipmentQuality.White;
				string text;
				if (flag)
				{
					text = "gray";
				}
				else
				{
					bool flag2 = equipment.Quality == EquipmentQuality.Blue;
					if (flag2)
					{
						text = "blue";
					}
					else
					{
						bool flag3 = equipment.Quality == EquipmentQuality.Green;
						if (flag3)
						{
							text = "green";
						}
						else
						{
							bool flag4 = equipment.Quality == EquipmentQuality.Yellow;
							if (flag4)
							{
								text = "yellow";
							}
							else
							{
								bool flag5 = equipment.Quality == EquipmentQuality.Red;
								if (flag5)
								{
									text = "red";
								}
								else
								{
									text = "purple";
								}
							}
						}
					}
				}
				stringBuilder.AppendFormat("{0}|{1}|{2}|{3};", new object[]
				{
					equipment.Id,
					equipment.EquipNameWithGeneral,
					equipment.Level,
					text
				});
				num = i;
			}
			return stringBuilder.ToString();
		}

		public string _mh_info()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (int i = 0; i < this._mhEquips.Count; i = num + 1)
			{
				Equipment equipment = this._mhEquips[i];
				bool flag = equipment.Quality == EquipmentQuality.White;
				string text;
				if (flag)
				{
					text = "gray";
				}
				else
				{
					bool flag2 = equipment.Quality == EquipmentQuality.Blue;
					if (flag2)
					{
						text = "blue";
					}
					else
					{
						bool flag3 = equipment.Quality == EquipmentQuality.Green;
						if (flag3)
						{
							text = "green";
						}
						else
						{
							bool flag4 = equipment.Quality == EquipmentQuality.Yellow;
							if (flag4)
							{
								text = "yellow";
							}
							else
							{
								bool flag5 = equipment.Quality == EquipmentQuality.Red;
								if (flag5)
								{
									text = "red";
								}
								else
								{
									text = "purple";
								}
							}
						}
					}
				}
				stringBuilder.AppendFormat("{0}|{1}|{2}|{3};", new object[]
				{
					equipment.Id,
					equipment.EquipNameWithGeneral,
					equipment.EnchantLevel,
					text
				});
				num = i;
			}
			return stringBuilder.ToString();
		}

		public void refreshPlayerInfo(XmlNode updates)
		{
            if (updates == null || !updates.HasChildNodes)
            {
                return;
            }
            XmlNodeList childNodes = updates.ChildNodes;
            int num = -1;
            int num2 = -1;
            try
            {
                foreach (XmlNode xmlNode in childNodes)
                {
                    if (xmlNode.Name == "perdayreward")
                    {
                        this._gotLoginReward = false;
                    }
                    else if (xmlNode.Name == "version_gift")
                    {
                        this._version_gift = (xmlNode.InnerText == "1");
                    }
                    else if (xmlNode.Name == "playerid")
                    {
                        this._id = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "playername")
                    {
                        this._username = xmlNode.InnerText;
                    }
                    else if (xmlNode.Name == "playerlevel")
                    {
                        this._level = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "copper")
                    {
                        this._silver = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "food")
                    {
                        this._food = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "forces")
                    {
                        this._forces = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "sys_gold")
                    {
                        num = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "user_gold")
                    {
                        num2 = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "jyungong")
                    {
                        this._credit = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "prestige")
                    {
                        this._prestige = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "year")
                    {
                        this._year = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "season")
                    {
                        this._season = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "token")
                    {
                        this._token = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "tokencd")
                    {
                        this._tokenCdTime = long.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "tokencdflag")
                    {
                        this._tokenCdFlag = (xmlNode.InnerText == "1");
                    }
                    else if (xmlNode.Name == "innewarea")
                    {
                        this._inNewArea = (xmlNode.InnerText == "1");
                    }
                    else if (xmlNode.Name == "nation")
                    {
                        this._nationInt = int.Parse(xmlNode.InnerText);
                        if (this._nationInt <= 0)
                        {
                            this._nation = "中立";
                        }
                        else
                        {
                            this._nation = this._nations[this._nationInt - 1];
                        }
                    }
                    else if (xmlNode.Name == "atttoken" || xmlNode.Name == "attacktoken")
                    {
                        this._attackOrders = int.Parse(xmlNode.InnerText);
                    }
                    else if (xmlNode.Name == "cityhp")
                    {
                        this._attack_cityHp = int.Parse(xmlNode.InnerText);
                    }
                    else
                    {
                        if (xmlNode.Name == "maxcityhp")
                        {
                            this._attack_maxCityHp = int.Parse(xmlNode.InnerText);
                        }
                        else if (xmlNode.Name == "cityhprecovercd")
                        {
                            this._attack_cityHpRecoverCd = int.Parse(xmlNode.InnerText);
                        }
                        else if (xmlNode.Name == "inspirestate")
                        {
                            this._attack_inspiredState = int.Parse(xmlNode.InnerText);
                        }
                        else if (xmlNode.Name == "inspireeffect")
                        {
                            this._attack_inspiredEffect = float.Parse(xmlNode.InnerText);
                        }
                        else if (xmlNode.Name == "battlescore")
                        {
                            this._attack_battleScore = int.Parse(xmlNode.InnerText);
                        }
                        else if (xmlNode.Name == "curactive")
                        {
                            this._curMovable = int.Parse(xmlNode.InnerText);
                        }
                        else if (xmlNode.Name == "bowlder")
                        {
                            string innerText = xmlNode.InnerText;
                            this._stone = int.Parse(innerText.Substring(0, innerText.Length - 3));
                        }
                        else if (xmlNode.Name == "arreststate")
                        {
                            if (xmlNode.InnerText == "null")
                            {
                                this._arrest_state = 0;
                            }
                            else
                            {
                                this._arrest_state = int.Parse(xmlNode.InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            if (num >= 0 && num2 >= 0)
            {
                this._gold = num + num2;
            }
            else if (num >= 0)
            {
                this._gold = num;
            }
            else if (num2 >= 0)
            {
                this._gold = num2;
            }
            if (num >= 0)
            {
                this._sysgold = num;
            }
            if (num2 >= 0)
            {
                this._usergold = num2;
            }
            XmlNode xmlNode2 = updates.SelectSingleNode("nian");
            if (xmlNode2 != null)
            {
                int num3 = 0;
                int.TryParse(xmlNode2.InnerText, out num3);
                if (num3 < 3)
                {
                    this.addActivity(ActivityType.WorldArmy);
                }
                else if (num3 >= 3)
                {
                    this.addActivity(ActivityType.WorldArmyZongzi);
                }
            }
            XmlNode xmlNode3 = updates.SelectSingleNode("gifteventcopper");
            if (xmlNode3 != null)
            {
                this.addActivity(ActivityType.SilverFlop);
            }
            XmlNode xmlNode4 = updates.SelectSingleNode("gifteventbaoshi4");
            if (xmlNode4 != null)
            {
                this.addActivity(ActivityType.GemFlop);
            }
            XmlNode xmlNode5 = updates.SelectSingleNode("eventtime");
            if (xmlNode5 != null && xmlNode5.InnerText.Equals("1"))
            {
                this.addActivity(ActivityType.ImposeEvent);
            }
            XmlNode xmlNode6 = updates.SelectSingleNode("isgeneraleventtime");
            if (xmlNode6 != null && xmlNode6.InnerText.Equals("1"))
            {
                this.addActivity(ActivityType.HeroTrainEvent);
            }
            XmlNode xmlNode7 = updates.SelectSingleNode("showkfwd");
            if (xmlNode7 != null && !xmlNode7.InnerText.Equals("0"))
            {
                this.addActivity(ActivityType.PlayerCompete);
            }
            XmlNode xmlNode8 = updates.SelectSingleNode("kfwdeventreward");
            if (xmlNode8 != null && !xmlNode8.InnerText.Equals("0"))
            {
                this.addActivity(ActivityType.PlayerCompeteEvent);
            }
            XmlNode xmlNode9 = updates.SelectSingleNode("showkfpvp");
            if (xmlNode9 != null && !xmlNode9.InnerText.Equals("0"))
            {
                this.addActivity(ActivityType.KfPvp);
            }
            XmlNode xmlNode10 = updates.SelectSingleNode("kfpvpshow");
            if (xmlNode10 != null && !xmlNode10.InnerText.Equals("0"))
            {
                this.addActivity(ActivityType.KfPvp);
            }
            XmlNode xmlNode11 = updates.SelectSingleNode("showkfzb");
            if (xmlNode11 != null && !xmlNode11.InnerText.Equals("0"))
            {
                this.addActivity(ActivityType.CrossplatformCompete);
            }
            XmlNode xmlNode12 = updates.SelectSingleNode("banquet");
            if (xmlNode12 != null)
            {
                this.addActivity(ActivityType.KfBanquet);
            }
            XmlNodeList xmlNodeList = updates.SelectNodes("boxmodels/boxmodel/list/value/mode");
            foreach (XmlNode xmlNode13 in xmlNodeList)
            {
                if (xmlNode13.InnerText == "1")
                {
                    this.addActivity(ActivityType.GiftEvent);
                }
            }
            XmlNode xmlNode14 = updates.SelectSingleNode("feteevent");
            if (xmlNode14 != null)
            {
                this.addActivity(ActivityType.FeteEvent);
            }
            XmlNodeList xmlNodeList2 = updates.SelectNodes("notice/property/content");
            foreach (XmlNode xmlNode15 in xmlNodeList2)
            {
                if (xmlNode15 != null)
                {
                    string innerText2 = xmlNode15.InnerText;
                    string[] array = innerText2.Split(new char[] { ' ' });
                    string text = "";
                    string[] array2 = array;
                    int num4;
                    for (int i = 0; i < array2.Length; i = num4 + 1)
                    {
                        string text2 = array2[i];
                        if (text2 != null && text2.Trim().Length != 0)
                        {
                            text += text2.Trim();
                            if (text.StartsWith("【") && (text.EndsWith(":00") || text.EndsWith(":30") || text.EndsWith("点")))
                            {
                                List<ActivityType> list = this.generateEventFromNotice(text);
                                foreach (ActivityType current in list)
                                {
                                    this.addActivity(current);
                                }
                                text = "";
                            }
                        }
                        num4 = i;
                    }
                }
            }
		}

		private List<ActivityType> generateEventFromNotice(string notice_content)
		{
			DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
			List<ActivityType> list = new List<ActivityType>();
			Regex regex = new Regex("【(.*)】([^-—]*)[-—至到 ]+([^-—]*)", RegexOptions.None);
			MatchCollection matchCollection = regex.Matches(notice_content);
			bool flag = matchCollection != null && matchCollection.Count > 0 && matchCollection[0] != null && matchCollection[0].Groups != null && matchCollection[0].Groups.Count >= 4;
			if (flag)
			{
				string value = matchCollection[0].Groups[1].Value;
				string value2 = matchCollection[0].Groups[2].Value;
				string value3 = matchCollection[0].Groups[3].Value;
				long num = this.parseDateTimeMSeconds(value2);
				long num2 = this.parseDateTimeMSeconds(value3);
				bool flag2 = num > num2;
				if (flag2)
				{
					num2 += 86400000L;
					bool flag3 = num > num2;
					if (flag3)
					{
						bool flag4 = dateTimeNow.Month == 12;
						if (flag4)
						{
							num2 += 31536000000L;
						}
						else
						{
							bool flag5 = dateTimeNow.Month == 1;
							if (flag5)
							{
								num -= 31536000000L;
							}
						}
					}
				}
				bool flag6 = num < this._factory.TmrMgr.TimeStamp && num2 > this._factory.TmrMgr.TimeStamp;
				if (flag6)
				{
					bool flag7 = value.IndexOf("典狱活动") >= 0;
					if (flag7)
					{
						list.Add(ActivityType.JailEvent);
					}
					bool flag8 = value.IndexOf("宝石翻牌") >= 0;
					if (flag8)
					{
						list.Add(ActivityType.GemFlop);
					}
					bool flag9 = value.IndexOf("银币翻牌") >= 0;
					if (flag9)
					{
						list.Add(ActivityType.SilverFlop);
					}
					bool flag10 = value.IndexOf("军资回馈") >= 0;
					if (flag10)
					{
						list.Add(ActivityType.RepayEvent);
					}
					bool flag11 = value.IndexOf("考古活动") >= 0;
					if (flag11)
					{
						list.Add(ActivityType.ArchEvent);
					}
					bool flag12 = value.IndexOf("盛宴活动") >= 0;
					if (flag12)
					{
						list.Add(ActivityType.KfBanquet);
					}
					bool flag13 = value.IndexOf("犒赏活动") >= 0;
					if (flag13)
					{
						list.Add(ActivityType.GiftEvent);
					}
					bool flag14 = value.IndexOf("世界军团") >= 0;
					if (flag14)
					{
						list.Add(ActivityType.WorldArmy);
					}
					bool flag15 = value.IndexOf("古城探宝") >= 0;
					if (flag15)
					{
						list.Add(ActivityType.TreasureGame);
					}
					bool flag16 = value.IndexOf("元旦") >= 0;
					if (flag16)
					{
						list.Add(ActivityType.NewYearEvent);
					}
				}
			}
			return list;
		}

		private long parseDateTimeMSeconds(string dtstr)
		{
			dtstr = dtstr.Replace("凌晨", "");
			dtstr = dtstr.Replace("点", ":00");
			dtstr = dtstr.Replace("24:00", "23:59");
			DateTime dateTime;
			DateTime.TryParse(dtstr, out dateTime);
			return decimal.ToInt64(decimal.Divide(dateTime.Ticks - 621355968000000000L, 10000m));
		}

		public void updateLimits(XmlNode updates)
		{
			bool flag = updates == null || !updates.HasChildNodes;
			if (!flag)
			{
				XmlNodeList childNodes = updates.ChildNodes;
				foreach (XmlNode xmlNode in childNodes)
				{
					bool flag2 = xmlNode.Name == "maxfood";
					if (flag2)
					{
						this._maxFood = int.Parse(xmlNode.InnerText);
					}
					else
					{
						bool flag3 = xmlNode.Name == "maxcoin";
						if (flag3)
						{
							this._maxSilver = int.Parse(xmlNode.InnerText);
						}
						else
						{
							bool flag4 = xmlNode.Name == "maxforce";
							if (flag4)
							{
								this._maxForces = int.Parse(xmlNode.InnerText);
							}
						}
					}
				}
			}
		}

		public User(ServiceFactory factory)
		{
            this._fete_min_levels = new int[6];
            this._fete_names = new string[6];
			this._fete_gold = new int[6];
			this._fete_now_gold = new int[6];
			this._fete_now_free_times = new int[6];
            this._is_fete_activity = false;
			this._dinner_team_id = "";
			this._dinner_team_creator = "";
			this._buildings = new List<Building>();
			this._buildingLines = new Dictionary<int, BuildingLine>();
			this._heroes = new List<Hero>();
			this._battle_current_army_id = "";
			this._all_armys = new List<Npc>();
			this._all_npcs = new List<Npc>();
			this._attack_battleScore_awardGot = new int[5];
			this._attack_current_cityevent_name = "";
			this._attack_nation_battle_city = "";
			this._attack_all_areas = new List<AreaInfo>();
			this._attack_user_tokens = new List<UserToken>();
			this._newAreaCityInfo = new Dictionary<int, int>();
			this._attack_move_path = new List<AreaInfo>();
			this._qhEquips = new List<Equipment>();
			this._mhEquips = new List<Equipment>();
			this._storeEquips = new List<Equipment>();
			this._weapon_info = "";
			this._army_reach = "";
			this._factory = factory;
		}
	}
}
