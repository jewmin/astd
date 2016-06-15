using AxSHDocVw;
using com.lover.astd.common;
using com.lover.astd.common.config;
using com.lover.astd.common.logic;
using com.lover.astd.common.logicexe;
using com.lover.astd.common.logicexe.activities;
using com.lover.astd.common.logicexe.battle;
using com.lover.astd.common.logicexe.building;
using com.lover.astd.common.logicexe.economy;
using com.lover.astd.common.logicexe.equip;
using com.lover.astd.common.logicexe.hero;
using com.lover.astd.common.logicexe.secretary;
using com.lover.astd.common.logicexe.temp;
using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using com.lover.astd.common.model.battle;
using com.lover.astd.common.model.building;
using com.lover.astd.common.model.enumer;
using com.lover.astd.common.model.misc;
using com.lover.astd.common.partner;
using com.lover.astd.game.ui.server.impl;
using com.lover.astd.game.ui.server.impl.activities;
using com.lover.astd.game.ui.server.impl.battle;
using com.lover.astd.game.ui.server.impl.economy;
using com.lover.astd.game.ui.server.impl.equipment;
using com.lover.astd.game.ui.server.impl.secretary;
using com.lover.astd.game.ui.server.impl.troop;
using com.lover.astd.game.ui.ui;
using com.lover.astd.weaponcalc;
using com.lover.common;
using com.lover.common.hook;
using Microsoft.Win32;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace com.lover.astd.game.ui
{
	public class MainForm : Form, ILogger, IServer
	{
		private delegate void SetLablePlayerInfoDelegate();

		private class TempLogger : ILogger
		{
			private MainForm _frm;

			public TempLogger(MainForm frm)
			{
				this._frm = frm;
			}

			public void logDebug(string text)
			{
				this._frm.logTempSafe(text, LogLevel.Debug, Color.Red);
			}

			public void logError(string text)
			{
				this._frm.logTempSafe(text, LogLevel.Error, Color.Red);
			}

			public void log(string text, Color color)
			{
				this._frm.logTempSafe(text, LogLevel.Info, color);
			}

			public void logSingle(string text)
			{
			}

			public void logSurprise(string text)
			{
			}
		}

		private delegate void logMeDelegate(string logtext);

		private delegate void LogSingleDelegate(string logtext);

		private delegate void LogDelegate(string logtext, LogLevel level, Color logColor);

		private delegate void LogSurpriseDelegate(string logtext, Color logColor);

		private delegate void logTempDelegate(string text, LogLevel level, Color logColor);

		private string _initialUrl = GlobalConfig.HomeUrl + "/astd/update";

		protected LogHelper _logger;

		private ServiceFactory _factory;

		private LoginMgr _loginMgr;

		private bool _accountFromArgs;

		private DoKeyHook _keyHook;

		private LogLevel _logLevel = LogLevel.Info;

		private bool _intentExit;

		private System.Windows.Forms.Timer _serverTimeTimer;

		private System.Windows.Forms.Timer _refreshUiTimer;

		private ExeMgr _exeMgr;

		private SettingsMgr _settingMgr;

		private System.Timers.Timer _relogin_timer;

		private Thread _init_thread;

		private Thread _exe_thread;

		private int _serverStatus;

		private string _custome_session = "";

		private AccountData _account;

		private bool _auto_hide_flash;

		private User _gameUser;

		private Dictionary<string, int> _selectIndexes = new Dictionary<string, int>();

		private Thread _tempThread;

		private bool _doingTemp;

		private System.Windows.Forms.Timer _refreshTempTimer;

		private ITempExe _tempExe;

		private bool _isRelogin;

		private LoginResult _login_result;

		private string _jsessionid = "";

		private string _gameurl = "";

        private List<Cookie> _cookies = new List<Cookie>();

		private MainForm.LogSingleDelegate _logSingleDelegate;

		private MainForm.LogDelegate _logDelegate;

		private MainForm.LogSurpriseDelegate _logSurpriseDelegate;

		private MainForm.logTempDelegate _logTempSafe;

		private bool _server_running;

		private IContainer components;

		public TabControl mainTab;

		public TabPage logPage;

		public TabPage gamePage;

		public TabPage setPage;

		public MenuStrip mainMenu;

		public ToolStripMenuItem menu_init;

		public RichTextBox logText;

		public ToolStripMenuItem menu_startServer;

		public ToolStripMenuItem menu_stopServer;

		public ToolStripMenuItem menu_test;

		public ToolStripMenuItem menu_refresh;

        public Label lblStatus;

		public ToolStripMenuItem menuWeaponCalc;

		public ToolStripMenuItem menu_trainCalc;

		public ToolStripMenuItem menu_saveSettings;

		public GroupBox groupBox1;

		public CheckBox chk_impose_enable;

		public NumericUpDown num_force_impose_gold;

		public Label label1;

		public NumericUpDown num_impose_reserve;

		public CheckBox chk_stock_enable;

		public CheckBox chk_stock_1;

		public CheckBox chk_stock_6;

		public CheckBox chk_stock_5;

		public CheckBox chk_stock_4;

		public CheckBox chk_stock_3;

		public CheckBox chk_stock_2;

		public CheckBox chk_stock_7;

		public GroupBox groupBox3;

		public CheckBox chk_global_logonAward;

		public CheckBox chk_global_reserve_silver;

		public NumericUpDown num_global_reserve_silver;

		public NumericUpDown num_global_reserve_gold;

		public CheckBox chk_global_reserve_gold;

		public GroupBox groupBox4;

		public GroupBox groupBox5;

		public GroupBox groupBox6;

		public CheckBox chk_building_enable;

		public CheckBox chk_hero_tufei_enable;

		public NumericUpDown num_global_reserve_stone;

		public CheckBox chk_global_reserve_stone;

		public NumericUpDown num_global_reserve_credit;

		public CheckBox chk_global_reserve_credit;

		public CheckBox chk_hero_wash_enable;

		public ComboBox combo_hero_wash;

		public CheckBox chk_hero_wash_zhi;

		public CheckBox chk_hero_wash_yong;

		public CheckBox chk_hero_wash_tong;

		public Label label3;

		public Label label2;

		public DataGridView dg_buildings;

		public DataGridView dg_heros;

		public GroupBox groupBox8;

		public TabControl tabControl2;

		public TabPage tabPage5;

		public NumericUpDown num_attack_npc_reserve_order;

		public CheckBox chk_attack_npc_enable;

		public TabPage tabPage6;

		public DataGridView dg_attack_army;

		public CheckBox chk_attack_army;

		public TabPage setPage2;

		public GroupBox groupBox9;

		public CheckBox chk_market_enable;

		public CheckBox chk_market_gem;

		public CheckBox chk_market_stone;

		public CheckBox chk_market_silver;

		public CheckBox chk_market_purpleweapon;

		public CheckBox chk_market_redweapon;

		public ComboBox combo_market_gem_type;

		public ComboBox combo_market_purpleweapon_type;

		public ComboBox combo_market_redweapon_type;

		public ComboBox combo_market_stone_type;

		public ComboBox combo_market_silver_type;

		public CheckBox chk_market_gem_gold;

		public CheckBox chk_market_purpleweapon_gold;

		public CheckBox chk_market_redweapon_gold;

		public CheckBox chk_market_stone_gold;

		public CheckBox chk_market_silver_gold;

		public TabPage tabPage7;

		public CheckBox chk_attack_player;

		public CheckBox chk_attack_player_use_token;

		public CheckBox chk_attack_player_move_tokenfull;

		public NumericUpDown num_attack_player_level_max;

		public NumericUpDown num_attack_player_level_min;

		public Label label5;

		public Label label4;

		public TabPage tabPage8;

		public NumericUpDown num_attack_army_reserve_order;

		public CheckBox chk_res_campaign_enable;

		public CheckBox chk_dinner;

		public CheckBox chk_get_stone;

		public NumericUpDown num_get_stone;

		public Label lbl_playerinfo;

		public Label lbl_time;

		public ToolStripMenuItem menu_translator;

		public ToolStripMenuItem menu_report;

		public CheckBox chk_merchant;

		public ComboBox combo_merchant_sell_level;

		public ComboBox combo_merchant_type;

		public CheckBox chk_merchant_onlyfree;

		public CheckBox chk_defense_format;

		public ComboBox combo_defense_formation;

		private GroupBox groupBox10;

		public CheckBox chk_fete_enable;

		private Label lbl_fete_silver;

		public NumericUpDown num_fete_1;

		private Label label8;

		public NumericUpDown num_fete_3;

		private Label label7;

		public NumericUpDown num_fete_2;

		private Label label9;

		public NumericUpDown num_fete_6;

		private Label label10;

		public NumericUpDown num_fete_5;

		private Label label11;

		public NumericUpDown num_fete_4;

		private Label label13;

		private Label label12;

		public GroupBox groupBox7;

		public TabControl tabControl1;

		public TabPage tabPage1;

		public NumericUpDown num_equip_qh_use_gold_limit;

		public CheckBox chk_equip_qh_use_gold;

		public DataGridView dg_equipQh;

		public CheckBox chk_equip_qh_enable;

		public TabPage tabPage2;

		public ComboBox combo_mh_type;

		public DataGridView dg_equipMh;

		public CheckBox chk_equip_mh_enable;

		public TabPage tabPage3;

		public RichTextBox txt_weaponinfo;

		public Label label6;

		public CheckBox chk_upgrade_weapone_enable;

		public TabPage tabPage4;

		public NumericUpDown num_upgrade_gem;

		public NumericUpDown num_polish_goon;

		public NumericUpDown num_polish_reserve;

		public CheckBox chk_upgrade_gem;

		public CheckBox chk_polish_enable;

		public CheckBox chk_attack_npc_enemy;

		public NumericUpDown num_polish_item_reserve;

		private Label label15;

		private Label label14;

		private Label label16;

		private Label label17;

		private ToolStripMenuItem menu_relogin;

		private DataGridViewCheckBoxColumn col_chk_equipEnchant;

		private DataGridViewTextBoxColumn col_name_equipEnchant;

		private DataGridViewTextBoxColumn col_level_equipEnchant;

		private DataGridViewCheckBoxColumn col_chk_equipMagic;

		private DataGridViewTextBoxColumn col_name_equipMagic;

		private DataGridViewTextBoxColumn col_level_equipMagic;

		private DataGridViewCheckBoxColumn col_chk_hero;

		private DataGridViewTextBoxColumn col_name_hero;

		private DataGridViewTextBoxColumn col_level_hero;

		private DataGridViewCheckBoxColumn col_chk_building;

		private DataGridViewTextBoxColumn col_name_building;

		private DataGridViewTextBoxColumn col_level_building;

		public TextBox txt_servers;

		private ToolStripMenuItem menu_clearLog;

		public CheckBox chk_giftevent_enable;

		public CheckBox chk_crossplatm_compete;

		public RichTextBox logSurpise;

		public CheckBox chk_arch_enable;

		public CheckBox chk_kf_banquet_enabled;

		public CheckBox chk_selAll_building;

		private Label label18;

		private Label label19;

		public CheckBox chk_hero_nocd;

		public NumericUpDown num_impose_loyalty;

		private Label label20;

		public NumericUpDown num_mh_failcount;

		private Label label21;

		private Label label22;

		public NumericUpDown num_polish_melt_failcount;

		public CheckBox chk_silver_flop_enable;

		public CheckBox chk_dailytask_enable;

		public NumericUpDown num_global_gem_price;

		private Label label23;

		public CheckBox chk_gem_flop_enable;

		public CheckBox chk_world_army_enable;

		public NumericUpDown num_gem_flop_upgrade_count;

		public CheckBox chk_attack_not_injail;

		public Label label24;

		public NumericUpDown num_attack_score;

		public CheckBox chk_kfwd_enable;

		public CheckBox chk_kfwd_buyreward;

		public CheckBox chk_market_force_attack;

		public ComboBox combo_world_army_boxtype;

		private GroupBox groupBox15;

		private GroupBox groupBox12;

		private GroupBox groupBox11;

		private GroupBox groupBox16;

		private GroupBox groupBox18;

		public NumericUpDown num_treasure_game_goldstep;

		public NumericUpDown num_daily_treasure_game_goldstep;

		public CheckBox chk_treasure_game_enable;

		public CheckBox chk_daily_treasure_game;

		private Label label27;

		private Label label26;

		public TextBox txt_giftevent_serial;

		public CheckBox chk_building_protect;

		public CheckBox chk_kf_banquet_nation;

		public CheckBox chk_world_army_buybox2;

		public CheckBox chk_world_army_buybox1;

		private ToolStripMenuItem menu_login;

		private ToolStripMenuItem menu_exit;

		private ToolTip tip_msg;

		private NotifyIcon notify_icon;

		public AxWebBrowser gameBrowser;

		public AxWebBrowser subBrowser;

		public NumericUpDown num_kf_banquet_buygold;

		public CheckBox chk_hero_giftevent;

		public RichTextBox logTemp;

		private Button btn_tempAdd;

		private Button btn_tempStop;

		private Label lbl_tempStatus;

		private Button btn_attack_target;

		public Label lbl_attack_target;

		public ComboBox combo_world_army_refresh_type;

		private Button btn_weaponCalc;

		public CheckBox chk_arch_create;

		public CheckBox chk_market_notbuy_iffail;

		public CheckBox chk_attack_player_cityevent;

		public CheckBox chk_attack_player_gongjian;

		private Button btn_selNpc;

		private Button btn_selForceNpc;

		public Label lbl_attackNpc;

		public Label lbl_attackForceNpc;

		public NumericUpDown num_attack_army_first_interval;

		public CheckBox chk_attack_army_only_jingyingtime;

		private ToolStripMenuItem menu_more;

		private ToolStripMenuItem menu_default;

		public CheckBox chk_festival_event;

		private BackgroundWorker login_worker;

		private Label label33;

		public TextBox txt_daily_treasure_game_boxtype;

		private Label label34;

		public TextBox txt_daily_treasure_game_goldmove_boxtype;

		private Label label35;

		public TextBox txt_treasure_game_goldmove_boxtype;

		private Label label36;

		public TextBox txt_treasure_game_boxtype;

		public CheckBox chk_attack_jail_tech;

		public CheckBox chk_attack_get_extra_order;

		public CheckBox chk_attack_enable_attack;

		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;

		private DataGridViewCheckBoxColumn column_firstbattle;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		public CheckBox chk_attack_army_first;

		private Label label25;

		public TextBox txt_arch_upgrade;

		public CheckBox chk_attack_reserve_token;

		public NumericUpDown num_attack_reserve_token;

		private Label label28;

		private ToolStripMenuItem menu_about;

		public GroupBox groupBox2;

		public CheckBox chk_movable_arch_trade_max;

		private Label label32;

		public NumericUpDown num_movable_refine_reserve;

		public CheckBox chk_movable_arch_trade;

		public CheckBox chk_movable_winter_trade;

		private Label label30;

		public ComboBox combo_movable_refine_item;

		private Label label29;

		public TextBox txt_movable_order;

		public NumericUpDown num_movable_weave_price;

		public CheckBox chk_movable_enable;

		public TextBox txt_movable_visit;

		private Label label37;

		public NumericUpDown num_movable_visit_fail;

		private Label label38;

		private ToolStripMenuItem menu_hideGame;

		public CheckBox chk_treasure_game_use_ticket;

		public ComboBox combo_treasure_game_use_ticket_type;

		public CheckBox chk_treasure_game_ticket_gold_end;

		private GroupBox groupBox13;

		public ComboBox combo_global_boss_key_key;

		public CheckBox chk_global_boss_key_shift;

		public CheckBox chk_global_boss_key_alt;

		public CheckBox chk_global_boss_key_ctrl;

		public CheckBox chk_global_boss_key_enable;

		public NumericUpDown num_crossplatm_compete_gold;

		private Button btn_sel_resCampaign;

		public Label lbl_attack_resCampaign;

		private ToolStripMenuItem menu_reportViewer;

        private ToolStripMenuItem menu_protocol_viewer;

		public CheckBox chk_world_army_openbox;

		public CheckBox chk_auto_hide_flash;

		public CheckBox chk_movable_arch_refill;

		public CheckBox chk_market_drop_notbuy;

		public CheckBox chk_daily_treasure_game_super_gold_end;

		public Label lbl_day_treasure_game_sel;

		private Button btn_day_treasure_game_select;

		public CheckBox chk_market_super;

		public NumericUpDown num_secretary_open_treasure;

		public CheckBox chk_secretary_open_treasure;

		public CheckBox chk_openbox_for_yupei;

		public CheckBox chk_troop_feedback_enable;

		public CheckBox chk_troop_feedback_doubleweapon;

		public CheckBox chk_troop_feedback_refine_notired;

		public CheckBox chk_troop_feedback_opentreasure;

		public ComboBox combo_troop_feedback_opentype;

		public CheckBox chk_troop_feedback_openbox;

		private Label label31;

		private Label label39;

		private Label label40;

		public CheckBox chk_super_fanpai;

		private Label label41;

		public CheckBox chk_daily_weapon;

		public ComboBox combo_daily_weapon_type;

		public CheckBox chk_kfzb_market;

		public CheckBox chk_kfzb_market_gem;

		public CheckBox chk_kfzb_market_stone;

		public CheckBox chk_kfzb_market_silver;

		public ComboBox combo_kfzb_market_weapon_pos_1;

		public ComboBox combo_kfzb_market_gem_pos;

		public ComboBox combo_kfzb_market_stone_pos;

		public ComboBox combo_kfzb_market_silver_pos;

		public CheckBox chk_kfzb_market_weapon_1;

		public CheckBox chk_kfzb_market_ignore;

		public ComboBox combo_kfzb_market_weapon_pos_2;

		public CheckBox chk_kfzb_market_weapon_2;

		public CheckBox chk_secretary_giftbox;

		public NumericUpDown num_movable_weave_count;

		private Label label42;

		private Label label43;

		private Label label44;

		public NumericUpDown num_movable_refine_factory_gold;

		public NumericUpDown num_movable_refine_factory_stone;

		public NumericUpDown num_movable_reserve;

		private Label label45;

		public ComboBox combo_kfwd_openbox_type;

		public CheckBox chk_kfwd_openbox;

		public CheckBox chk_tower_enable;

		public ComboBox combo_tower_type;

		public CheckBox chk_battle_event_enabled;

		public CheckBox chk_troop_turntable;

		public CheckBox chk_cake_event;

		public CheckBox chk_troop_turntable_goldratio;

		public ComboBox combo_troop_turntable_type;

		public NumericUpDown num_troop_turntable_buygold;

		private Label label46;

		public CheckBox chk_upgrade_crystal;

		public CheckBox chk_battle_event_gem;

		public CheckBox chk_battle_event_weapon;

		public CheckBox chk_battle_event_stone;

		public CheckBox chk_battle_event_stone_gold;

		public CheckBox chk_battle_event_weapon_gold;

		public CheckBox chk_battle_event_gem_gold;

		private Label label47;

		public CheckBox chk_market_usetoken;

		public CheckBox chk_gemdump_enable;

		public NumericUpDown num_attack_cityevent_maxstar;

		public Label label48;

		public ComboBox combo_attack_filter_type;

		public TextBox txt_attack_filter_content;

		public CheckBox chk_Nation;

		private GroupBox groupBox14;

		public NumericUpDown downUp_QiFuCoin;

		private Label label49;

		public CheckBox checkBox_QiFu;

		private Label label51;

		public NumericUpDown downUp_QiFuDQ;

		private Label label50;

		public CheckBox chk_again;

		public CheckBox chk_Cai;

		public NumericUpDown nUD_toukui_ticket;

		private Label label53;

		private Label label52;

		public CheckBox chk_AutoBuyCredit;

		private GroupBox groupBox17;

		public CheckBox chk_SuperUseFree;

		private Label label56;

		private Label label60;

		private Label label61;

		public NumericUpDown nUD_DianQuan;

		private Label label59;

		private Label label58;

		private Label label57;

		public NumericUpDown nUD_CiShu;

		private Label label55;

		public NumericUpDown nUD_BaoShi;

		public CheckBox chk_AutoBaiShen;

		public CheckBox chk_ShenHuo;

		private GroupBox groupBox19;

		private Label label63;

		public NumericUpDown nUD_boat_up_coin;

		private Label label62;

		public CheckBox chk_AutoSelectTime;

		public TextBox txt_boat_upgrade;

		private Label label54;

		public CheckBox chk_boat_creat;

		public CheckBox chk_AutoBoat;

		private Label label65;

		private Label label64;

		public NumericUpDown nm_Out;

		private Label label67;

		public NumericUpDown nUD_reserved_num;

		private Label label66;

		private GroupBox groupBox20;

		public CheckBox chk_AutoYuebing;

		public CheckBox chk_GoldTrain;

		public CheckBox chk_AutoYuebingHongbao;

        public ComboBox cb_OpenYuebingHongbaoType;
        public TextBox txt_treasure_game_dice_type;
        private Label label68;
        public CheckBox chk_new_day_treasure_game;
        private Label label69;
        public TextBox txt_new_day_treasure_game_boxtype;
        public CheckBox chk_new_day_treasure_game_shake_tree;
        public NumericUpDown num_get_baoshi_stone;
        public CheckBox chk_get_baoshi_stone;
        public CheckBox chk_upgrade_war_chariot;
        public NumericUpDown num_upgrade_crystal;
        public NumericUpDown num_yupei_attr;
        public CheckBox chk_auto_melt_yupei;
        public CheckBox chk_juedou;

		public CheckBox chk_upgrade_toukui;

		public ServiceFactory Factory
		{
			get
			{
				return this._factory;
			}
			set
			{
				this._factory = value;
			}
		}

		public bool AccountFromArgs
		{
			get
			{
				return this._accountFromArgs;
			}
			set
			{
				this._accountFromArgs = value;
			}
		}

		public AccountData Account
		{
			get
			{
				return this._account;
			}
		}

		public bool Auto_hide_flash
		{
			get
			{
				return this._auto_hide_flash;
			}
			set
			{
				this._auto_hide_flash = value;
			}
		}

		public User GameUser
		{
			get
			{
				return this._gameUser;
			}
			set
			{
				this._gameUser = value;
			}
		}

		public string JSessionId
		{
			get
			{
				return this._jsessionid;
			}
		}

		public string Gameurl
		{
			get
			{
				return this._gameurl;
			}
		}

		public GameConfig getConfig()
		{
			if (this._account != null)
			{
				return this._account.GameConf;
			}
			return null;
		}

		public MainForm()
		{
			this.InitializeComponent();
		}

		public void removeKeyHook()
		{
			HookUtils.removeKeyHook();
		}

		public void setKeyHook(bool shift, bool ctrl, bool alt, Keys key)
		{
			HookUtils.setKeyHook(shift, ctrl, alt, key, this._keyHook);
		}

		private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			SystemEvents.SessionEnding -= new SessionEndingEventHandler(this.SystemEvents_SessionEnding);
			this.closeMe();
		}

		private void toggleHide()
		{
			if (this.notify_icon.Visible)
			{
				this.hideMe();
				this.notify_icon.Visible = false;
				return;
			}
			this.showMe();
			this.notify_icon.Visible = true;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this._keyHook = new DoKeyHook(this.toggleHide);
			if (GlobalConfig.isDebug)
			{
				this._logLevel = LogLevel.Debug;
			}
			this._factory = new ServiceFactory();
			this._gameUser = new User(this._factory);
			this._loginMgr = new LoginMgr();
			this.Text = GlobalConfig.Version;
			this.gameBrowser.Navigate(this._initialUrl);
            Console.WriteLine("this._initialUrl: {0}", this._initialUrl);
			if (!GlobalConfig.isDebug)
			{
				this.menu_test.Visible = false;
			}
			if (this._accountFromArgs && !UserSessionMgr.instance.isSf)
			{
				this.menu_login.Visible = false;
				this.menu_about.Visible = false;
				this.menu_more.Visible = false;
				this.menu_relogin.Visible = false;
				this.menu_exit.Visible = false;
				this.notify_icon.Visible = false;
			}
			if (GlobalConfig.isManager)
			{
				this.menu_login.Visible = false;
				this.menu_about.Visible = false;
				this.menu_more.Visible = false;
				this.menu_relogin.Visible = false;
				this.menu_exit.Visible = false;
				this.menu_startServer.Visible = false;
				this.menu_stopServer.Visible = false;
				this.menu_init.Visible = false;
				this.setPage.Parent = null;
				this.setPage2.Parent = null;
				this.notify_icon.Visible = false;
				this.txt_servers.Visible = false;
			}
			this.menu_refresh.Enabled = false;
			this.menu_startServer.Enabled = false;
			this.menu_stopServer.Enabled = false;
			this.menu_default.Enabled = false;
			SystemEvents.SessionEnding += new SessionEndingEventHandler(this.SystemEvents_SessionEnding);
			this._serverTimeTimer = new System.Windows.Forms.Timer();
			this._serverTimeTimer.Interval = 1000;
			this._serverTimeTimer.Tick += new EventHandler(this._serverTimeTimer_Tick);
			this._serverTimeTimer.Start();
			this._refreshUiTimer = new System.Windows.Forms.Timer();
			this._refreshUiTimer.Interval = 500;
			this._refreshUiTimer.Tick += new EventHandler(this._refreshUiTimer_Tick);
			this._refreshUiTimer.Start();
			if (this._accountFromArgs)
			{
				this.initGameFromArgs(UserSessionMgr.instance.Account);
			}
		}

		private void _refreshUiTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (this._serverStatus == 0)
				{
					this.menu_startServer.Enabled = false;
					this.menu_stopServer.Enabled = false;
					if (this._account != null && this._account.Server_type == ServerType.Custom)
					{
						this.menu_init.Enabled = true;
					}
					else
					{
						this.menu_init.Enabled = false;
					}
					this.menu_refresh.Enabled = false;
					this.menu_default.Enabled = false;
				}
				else if (this._serverStatus == 1)
				{
					this.menu_startServer.Enabled = true;
					this.menu_stopServer.Enabled = false;
					this.menu_init.Enabled = true;
					this.menu_refresh.Enabled = true;
					this.menu_default.Enabled = true;
				}
				else if (this._serverStatus == 2)
				{
					this.menu_startServer.Enabled = false;
					this.menu_stopServer.Enabled = true;
					this.menu_init.Enabled = true;
					this.menu_refresh.Enabled = true;
					this.menu_default.Enabled = true;
				}
				else if (this._serverStatus == 3)
				{
					this.menu_startServer.Enabled = true;
					this.menu_stopServer.Enabled = false;
					this.menu_init.Enabled = true;
					this.menu_refresh.Enabled = true;
					this.menu_default.Enabled = true;
				}
				if (this._settingMgr != null)
				{
					this._refreshUiTimer.Stop();
					this._settingMgr.renderSettings();
					if (this._exeMgr != null)
					{
						this.txt_servers.Text = this._exeMgr.Status;
					}
					this._refreshUiTimer.Start();
				}
			}
			catch (Exception ex)
			{
				this.logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
			}
		}

		private void _serverTimeTimer_Tick(object sender, EventArgs e)
		{
            try
            {
                if (this._factory == null)
                {
                    return;
                }
                DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
                this.lbl_time.Text = dateTimeNow.ToString("G");
            }
            catch (Exception ex)
            {
                this.logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
		}

		public void refreshPlayerInfo()
		{
			User gameUser = this._gameUser;
			this.Text = string.Format("[{0}] <{1}>", string.Format("{0:s}({1:d}) - {2}", gameUser.Username, gameUser.Level, this._account.getServerName()), GlobalConfig.Version);
			this.setPlayerInfoLabelSafe();
			this.setPlayerInfoNotify();
		}

		private void setPlayerInfoLabelSafe()
		{
			if (this.lbl_playerinfo.InvokeRequired)
			{
				this.lbl_playerinfo.Invoke(new MainForm.SetLablePlayerInfoDelegate(this.setPlayerInfoLabel));
				return;
			}
			this.setPlayerInfoLabel();
		}

		private void setPlayerInfoLabel()
		{
			User gameUser = this._gameUser;
			this.lbl_playerinfo.Text = string.Format("{0}[{1}级 {2} {3}{4}] 金币[{5}] 银币[{6}] 玉石[{7}] 军功[{8}] 行动力[{9}] 军令[{10}/50] 攻击令[{11}/50] 城防[{12}] 战绩[{13}]", new object[]
			{
				gameUser.Username,
				gameUser.Level,
				gameUser.Nation,
				gameUser.Year,
				gameUser.SeasonName,
				gameUser.Gold,
				CommonUtils.getReadable((long)gameUser.Silver),
				CommonUtils.getReadable((long)gameUser.Stone),
				CommonUtils.getReadable((long)gameUser.Credit),
				CommonUtils.getReadable((long)gameUser.CurMovable),
				gameUser.Token,
				gameUser.AttackOrders,
				gameUser._attack_cityHp,
				gameUser._attack_battleScore
			});
		}

		private void setPlayerInfoNotify()
		{
			User gameUser = this._gameUser;
			this.notify_icon.Text = string.Format("{0}{1}区-{2}", EnumString.getString(this._account.Server_type), this._account.ServerId, gameUser.Username);
		}

		private void gameBrowser_NewWindow3(object sender, DWebBrowserEvents2_NewWindow3Event e)
		{
			this.gameBrowser.Navigate(e.bstrUrl);
            Console.WriteLine("e.bstrUrl: {0}", e.bstrUrl);
			e.cancel = true;
		}

		private void gameBrowser_NavigateComplete2(object sender, DWebBrowserEvents2_NavigateComplete2Event e)
		{
			if (this._account == null || this._account.Server_type != ServerType.Custom)
			{
				return;
			}
			HTMLDocument hTMLDocument = (HTMLDocument)this.gameBrowser.Document;
			if (hTMLDocument == null)
			{
				return;
			}
			Uri uri = new Uri(hTMLDocument.url);
			Uri uri2 = new Uri(this._account.CustomGameUrl);
			if (uri.Host.Equals(uri2.Host) && uri.AbsolutePath.Equals(uri2.AbsolutePath))
			{
				this.getCustomSession();
			}
		}

		private void subBrowser_NavigateComplete2(object sender, DWebBrowserEvents2_NavigateComplete2Event e)
		{
			if (this._account == null || this._account.Server_type != ServerType.Custom)
			{
				return;
			}
			HTMLDocument hTMLDocument = (HTMLDocument)this.subBrowser.Document;
			if (hTMLDocument == null)
			{
				return;
			}
			string cookie = hTMLDocument.cookie;
			if (cookie == null)
			{
				return;
			}
			string[] array = cookie.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text != null)
				{
					string[] array2 = text.Split(new char[]
					{
						'='
					});
					if (array2.Length >= 2 && array2[0].Equals("JSESSIONID"))
					{
						this._custome_session = array2[1];
						this._jsessionid = this._custome_session;
						this._gameurl = this._account.CustomGameUrl;
						this.start_delay_init(false);
						return;
					}
				}
			}
		}

		private void menu_login_Click(object sender, EventArgs e)
		{
			if (_account != null)
			{
				MessageBox.Show("小伙伴, 你已经登录了, 如果要更换账号的话, 还是关了再开吧, 要不就重新打开一个好了");
				return;
			}
			try
			{
				new NewLoginForm(this).ShowDialog();
			}
			catch (Exception ex)
			{
				logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
			}
		}

		private void menu_init_Click(object sender, EventArgs e)
		{
			if (this._account == null)
			{
				return;
			}
			if (this._account.Server_type == ServerType.Custom && (this._custome_session == null || this._custome_session.Length == 0))
			{
				this.getCustomSession();
				return;
			}
			this.start_delay_init(true);
		}

		private void menu_startServer_Click(object sender, EventArgs e)
		{
			if (this._account == null || this._account.GameConf == null)
			{
				UiUtils.getInstance().info("还未初始化成功");
				return;
			}
			this.menu_startServer.Enabled = false;
			this.menu_stopServer.Enabled = true;
			this.startServer();
		}

		private void menu_stopServer_Click(object sender, EventArgs e)
		{
			if (this._account == null || this._account.GameConf == null)
			{
				UiUtils.getInstance().info("还未初始化成功");
				return;
			}
			this.menu_startServer.Enabled = true;
			this.menu_stopServer.Enabled = false;
			this.stopServer(true);
		}

		private void menu_refresh_Click(object sender, EventArgs e)
		{
			if (this._account == null || this._account.GameConf == null)
			{
				UiUtils.getInstance().info("还未初始化成功");
				return;
			}
			this.gameBrowser.Navigate(this._gameurl);
            Console.WriteLine("this._gameurl: {0}", this._gameurl);
		}

		private void menu_saveSettings_Click(object sender, EventArgs e)
		{
			base.Validate();
			if (this._settingMgr == null || this._account == null || this._account.GameConf == null)
			{
				UiUtils.getInstance().info("还未初始化成功");
				return;
			}
			this._account.GameConf.clearSettingDict();
			this._settingMgr.saveSettings();
			this._account.GameConf.saveSettings();
			this._account.GameConf.loadSettings();
			if (UserSessionMgr.instance.is_init_from_args)
			{
				string text = UserSessionMgr.instance.pushConfig();
				if (text.Length > 0)
				{
					UiUtils.getInstance().info("上传配置到服务器失败, 请重试, " + text);
				}
			}
			UiUtils.getInstance().info("保存设置成功");
		}

		private void menu_default_Click(object sender, EventArgs e)
		{
			if (this._settingMgr == null || this._account == null || this._account.GameConf == null)
			{
				UiUtils.getInstance().info("还未初始化成功");
				return;
			}
			if (MessageBox.Show("你确定要恢复默认设置吗?", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				this._settingMgr.loadDefaultSettings();
				this._account.GameConf.saveSettings();
				this._account.GameConf.loadSettings();
				UiUtils.getInstance().info("保存设置成功");
			}
		}

		private void menu_relogin_Click(object sender, EventArgs e)
		{
			if (this._settingMgr == null || this._account == null || this._account.GameConf == null)
			{
				UiUtils.getInstance().info("还未初始化成功");
				return;
			}
			if (MessageBox.Show("你确定要刷新立即重新登录吗?", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				this.startRelogin();
			}
		}

		private void menu_clearLog_Click(object sender, EventArgs e)
		{
			this.logText.Text = "";
			this.logSurpise.Text = "";
			this.logTemp.Text = "";
		}

		private void dg_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Clicks < 2 && e.Button == MouseButtons.Left && e.ColumnIndex == 0 && e.RowIndex > -1)
			{
				DataGridView expr_2D = sender as DataGridView;
				expr_2D.DoDragDrop(expr_2D.Rows[e.RowIndex], DragDropEffects.Move);
			}
		}

		private void dg_DragDrop(object sender, DragEventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			int num = -1;
			if (this._selectIndexes.ContainsKey(dataGridView.Name))
			{
				num = this._selectIndexes[dataGridView.Name];
			}
			if (num == -1)
			{
				return;
			}
			int dataGridRowIndexFromPoint = this.getDataGridRowIndexFromPoint(dataGridView, e.X, e.Y);
			if (dataGridRowIndexFromPoint == -1)
			{
				return;
			}
			BindingSource expr_57 = dataGridView.DataSource as BindingSource;
			object value = expr_57[num];
			expr_57.RemoveAt(num);
			expr_57.Insert(dataGridRowIndexFromPoint, value);
		}

		private void dg_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void dg_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex > -1)
			{
				DataGridView dataGridView = sender as DataGridView;
				if (!this._selectIndexes.ContainsKey(dataGridView.Name))
				{
					this._selectIndexes.Add(dataGridView.Name, e.RowIndex);
					return;
				}
				this._selectIndexes[dataGridView.Name] = e.RowIndex;
			}
		}

		private void dg_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			int num = -1;
			if (this._selectIndexes.ContainsKey(dataGridView.Name))
			{
				num = this._selectIndexes[dataGridView.Name];
			}
			if (dataGridView.Rows.Count > 0 && dataGridView.SelectedRows.Count > 0 && dataGridView.SelectedRows[0].Index != num)
			{
				if (dataGridView.Rows.Count <= num)
				{
					num = dataGridView.Rows.Count - 1;
				}
				dataGridView.Rows[num].Selected = true;
				DataGridView expr_8D = dataGridView;
				expr_8D.CurrentCell = expr_8D.Rows[num].Cells[0];
			}
		}

		private int getDataGridRowIndexFromPoint(DataGridView dg, int px, int py)
		{
			int i = 0;
			int count = dg.Rows.Count;
			while (i < count)
			{
				Rectangle rowDisplayRectangle = dg.GetRowDisplayRectangle(i, false);
				if (dg.RectangleToScreen(rowDisplayRectangle).Contains(px, py))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		private void dg_rowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			if (dataGridView == null)
			{
				return;
			}
			BindingSource bindingSource = dataGridView.DataSource as BindingSource;
			if (bindingSource == null)
			{
				return;
			}
			List<Equipment> list = bindingSource.DataSource as List<Equipment>;
			List<Npc> list2 = bindingSource.DataSource as List<Npc>;
			if (list == null && list2 == null)
			{
				return;
			}
			if (list != null && list.Count <= e.RowIndex)
			{
				return;
			}
			if (list2 != null && list2.Count <= e.RowIndex)
			{
				return;
			}
			DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
			if (list == null)
			{
				if (list2 != null)
				{
					string text = list2[e.RowIndex].ItemColor.TrimStart(new char[]
					{
						'#'
					});
					if (text != "")
					{
						Color foreColor = Color.FromArgb(int.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier), int.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier), int.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier));
						dataGridViewRow.DefaultCellStyle.ForeColor = foreColor;
					}
				}
				return;
			}
			Equipment equipment = list[e.RowIndex];
			if (equipment.Quality == EquipmentQuality.White)
			{
				dataGridViewRow.DefaultCellStyle.ForeColor = Color.Black;
				return;
			}
			if (equipment.Quality == EquipmentQuality.Blue)
			{
				dataGridViewRow.DefaultCellStyle.ForeColor = Color.Blue;
				return;
			}
			if (equipment.Quality == EquipmentQuality.Green)
			{
				dataGridViewRow.DefaultCellStyle.ForeColor = Color.Green;
				return;
			}
			if (equipment.Quality == EquipmentQuality.Yellow)
			{
				dataGridViewRow.DefaultCellStyle.ForeColor = Color.Yellow;
				return;
			}
			if (equipment.Quality == EquipmentQuality.Red)
			{
				dataGridViewRow.DefaultCellStyle.ForeColor = Color.Red;
				return;
			}
			dataGridViewRow.DefaultCellStyle.ForeColor = Color.Purple;
		}

		private void dg_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ((this._accountFromArgs && !UserSessionMgr.instance.isSf) || GlobalConfig.isManager)
			{
				if (MessageBox.Show("确认退出吗?", "确认退出", MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					e.Cancel = true;
					return;
				}
			}
			else if (!this._intentExit)
			{
				this.hideMe();
				e.Cancel = true;
			}
		}

		private void menu_exit_Click(object sender, EventArgs e)
		{
			if ((this._accountFromArgs && !UserSessionMgr.instance.isSf) || GlobalConfig.isManager)
			{
				base.Close();
				return;
			}
			if (MessageBox.Show("确认退出吗?", "确认退出", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				this.closeMe();
			}
		}

		private void menu_about_Click(object sender, EventArgs e)
		{
			UiUtils.getInstance().info("本程序仅仅是为了个人测试代码架构设计, 如果冒犯了您的利益, 请立即与我联系, QQ群: 171935400");
		}

		private void menu_hideGame_Click(object sender, EventArgs e)
		{
			if (this._gameurl == null || this._gameurl == "")
			{
				UiUtils.getInstance().info("还未初始化");
				return;
			}
			if (this.gameBrowser.LocationURL.Equals(this._initialUrl))
			{
				this.gameBrowser.Navigate(this._gameurl);
                Console.WriteLine("this._gameurl: {0}", this._gameurl);
				return;
			}
			this.gameBrowser.Navigate(this._initialUrl);
            Console.WriteLine("this._initialUrl: {0}", this._initialUrl);
		}

		private void context_menu_exit_Click(object sender, EventArgs e)
		{
			this.closeMe();
		}

		private void notify_icon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (base.Visible)
			{
				this.hideMe();
				return;
			}
			this.showMe();
		}

		private void chk_selAll_building_CheckedChanged(object sender, EventArgs e)
		{
			object dataSource = this.dg_buildings.DataSource;
			if (dataSource == null)
			{
				return;
			}
			BindingSource bindingSource = dataSource as BindingSource;
			if (bindingSource == null)
			{
				return;
			}
			List<Building> list = bindingSource.DataSource as List<Building>;
			if (list == null)
			{
				return;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Building building = list[i];
				if (building != null)
				{
					building.IsChecked = this.chk_selAll_building.Checked;
				}
				i++;
			}
			this.dg_buildings.Refresh();
		}

		private void menu_reportViewer_Click(object sender, EventArgs e)
		{
			NewReportViewer viewer = new NewReportViewer(this._gameurl);
			viewer.Show();
			viewer.BringToFront();
		}

		private void menu_protocol_viewer_Click(object sender, EventArgs e)
		{
            if (_gameurl == null || _gameurl == "" || _jsessionid == null || _jsessionid == "")
            {
                UiUtils.getInstance().info("请先登录");
                return;
            }
            NewTranslator translator = new NewTranslator(_gameurl, _jsessionid);
			translator.Show();
			translator.BringToFront();
		}

		private void logText_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			string linkText = e.LinkText;
			NewReportViewer viewer = new NewReportViewer(_gameurl);
			viewer.Show();
			viewer.BringToFront();
			viewer.setReport(linkText);
		}

		private void btn_attack_target_Click(object sender, EventArgs e)
		{
            NewAttackTargetSelector selector = new NewAttackTargetSelector();
			selector.StartPosition = FormStartPosition.Manual;
			selector.Location = new Point(500, 400);
			selector.setCityInfo(_gameUser.getNewAreaCityInfo(), _gameUser._attack_selfCityId);
			selector.ShowDialog();
			string cityname = selector.getCityname();
			this.lbl_attack_target.Text = cityname;
		}

		private void btn_weaponCalc_Click(object sender, EventArgs e)
		{
			new WeaponCalc
			{
				StartPosition = FormStartPosition.CenterParent
			}.ShowDialog();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			this.lblStatus.Location = new Point(5, base.Height - 60);
			this.lbl_playerinfo.Location = new Point(200, base.Height - 60);
			this.mainTab.Width = base.Width - 20;
			this.mainTab.Height = this.lblStatus.Location.Y - 35;
			this.gameBrowser.Width = this.mainTab.Width - 15;
			this.gameBrowser.Height = this.mainTab.Height - 20;
		}

		private void btn_selNpc_Click(object sender, EventArgs e)
		{
			NewNpcSelector selector = new NewNpcSelector();
			selector.StartPosition = FormStartPosition.CenterParent;
			selector.setNpcs(_gameUser._all_npcs);
			selector.ShowDialog();
			Npc selectedNpc = selector.getSelectedNpc();
			string formation = selector.getFormation();
			if (selectedNpc == null)
			{
				return;
			}
			lbl_attackNpc.Text = string.Format("{0}:{1}:{2}:{3}", selectedNpc.Name, selectedNpc.ItemName, formation, selectedNpc.Id);
		}

		private void btn_selForceNpc_Click(object sender, EventArgs e)
		{
			NewNpcSelector selector = new NewNpcSelector();
			selector.StartPosition = FormStartPosition.CenterParent;
			selector.setNpcs(_gameUser._all_npcs);
			selector.ShowDialog();
			Npc selectedNpc = selector.getSelectedNpc();
			string formation = selector.getFormation();
			if (selectedNpc == null)
			{
				return;
			}
			lbl_attackForceNpc.Text = string.Format("{0}:{1}:{2}:{3}", selectedNpc.Name, selectedNpc.ItemName, formation, selectedNpc.Id);
		}

		private void btn_sel_resCampaign_Click(object sender, EventArgs e)
		{
			if (_gameurl == null || _gameurl == "")
			{
				UiUtils.getInstance().info("还未初始化");
				return;
			}
			NewResCampaignSelector selector = new NewResCampaignSelector();
			selector.StartPosition = FormStartPosition.Manual;
			selector.Location = new Point(500, 400);
			selector.setMainForm(this);
			selector.ShowDialog();
			ResCampaign selectCampaign = selector.getSelectCampaign();
			if (selectCampaign == null)
			{
				return;
			}
			lbl_attack_resCampaign.Text = string.Format("{0}:{1}", selectCampaign.id, selectCampaign.name);
		}

		private void btn_day_treasure_game_select_Click(object sender, EventArgs e)
		{
			if (_gameurl == null || _gameurl == "")
			{
				UiUtils.getInstance().info("还未初始化");
				return;
			}
			NewDailyTreasureGameSelector selector = new NewDailyTreasureGameSelector();
			selector.StartPosition = FormStartPosition.Manual;
			selector.Location = new Point(430, 350);
			selector.setMainForm(this);
			selector.ShowDialog();
			TGame selectCampaign = selector.getSelectCampaign();
			if (selectCampaign == null)
			{
				return;
			}
			lbl_day_treasure_game_sel.Text = string.Format("{0}:{1}", selectCampaign.tid, selectCampaign.powername);
		}

		public void addTempServer(string type, Dictionary<string, string> conf)
		{
			if (this._tempExe != null)
			{
				UiUtils.getInstance().info("还有临时任务没有完成, 不能再开始新的临时任务");
				return;
			}
			if (type.Equals("campaign"))
			{
				this._tempExe = new CampaignExe();
			}
			else if (type.Equals("equip_melt"))
			{
				this._tempExe = new EquipMeltExe();
			}
			else if (type.Equals("wash"))
			{
				this._tempExe = new CreditWashExe();
			}
			else if (type.Equals("ticket_exchange"))
			{
				this._tempExe = new TicketExchangeExe();
			}
			else if (type.Equals("cut_rawstone"))
			{
				this._tempExe = new RawStoneExe();
			}
			else if (type.Equals("dump_store"))
			{
				this._tempExe = new DumpStoreExe();
			}
            else if (type.Equals("weave"))
            {
                this._tempExe = new WeaveExe();
            }
			if (this._tempExe == null)
			{
				return;
			}
			this._tempExe.setTarget(conf);
			this.lbl_tempStatus.Text = this._tempExe.getStatus();
			ILogger logger = new MainForm.TempLogger(this);
			ProtocolMgr proto = new ProtocolMgr(this._gameUser, logger, this, this._gameurl, this._jsessionid, this._factory);
			this._tempExe.setVariables(proto, logger, this, this._gameUser, this._account.GameConf, this._factory);
			this.startTempServer();
		}

		private void btn_tempAdd_Click(object sender, EventArgs e)
		{
			if (_settingMgr == null)
			{
				UiUtils.getInstance().error("还未初始化服务");
				return;
			}
			new NewTempServerForm(this).ShowDialog();
		}

		private void btn_tempStop_Click(object sender, EventArgs e)
		{
			if (this._settingMgr == null)
			{
				UiUtils.getInstance().error("还未初始化服务");
				return;
			}
			this.stopTempServer();
			this._tempExe = null;
		}

		private void startTempServer()
		{
			this.stopTempServer();
			if (this._tempExe == null)
			{
				return;
			}
			this._refreshTempTimer = new System.Windows.Forms.Timer();
			this._refreshTempTimer.Interval = 100;
			this._refreshTempTimer.Tick += new EventHandler(this._refreshTempTimer_Tick);
			this._refreshTempTimer.Start();
			this._doingTemp = true;
			this._tempThread = new Thread(new ThreadStart(this._temp_thread_proc));
			this._tempThread.Start();
			this.btn_tempAdd.Enabled = false;
			this.btn_tempStop.Enabled = true;
		}

		private void _temp_thread_proc()
		{
			try
			{
				int num = 0;
				if (this._tempExe != null)
				{
					long num2 = this._tempExe.execute();
					num2 += this._factory.TmrMgr.TimeStamp;
					while (this._doingTemp && !this._tempExe.isFinished())
					{
						if (num < 20)
						{
							Thread.Sleep(100);
							num++;
						}
						else
						{
							num = 0;
							if (num2 < this._factory.TmrMgr.TimeStamp)
							{
								num2 = this._tempExe.execute();
								num2 += this._factory.TmrMgr.TimeStamp;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
			}
		}

		private void _refreshTempTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (this._tempExe == null)
				{
					this.stopTempServer();
					return;
				}
				this.lbl_tempStatus.Text = this._tempExe.getStatus();
				if (this._tempExe.isFinished())
				{
					this.stopTempServer();
					this._tempExe = null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("_refreshTempTimer_Tick:{0}", ex.Message);
			}
		}

		private void stopTempServer()
		{
			this._doingTemp = false;
			if (this._refreshTempTimer != null && this._refreshTempTimer.Enabled)
			{
				this._refreshTempTimer.Stop();
				this._refreshTempTimer.Dispose();
				this._refreshTempTimer = null;
			}
			this._tempThread = null;
			this.lbl_tempStatus.Text = "";
			this.btn_tempAdd.Enabled = true;
			this.btn_tempStop.Enabled = false;
		}

		public void initializeComplete()
		{
		}

		public void hideMe()
		{
			base.Hide();
			if (this._auto_hide_flash)
			{
				this.gameBrowser.Navigate(this._initialUrl);
                Console.WriteLine("this._initialUrl: {0}", this._initialUrl);
			}
		}

		public void showMe()
		{
			base.Show();
			base.BringToFront();
		}

		public void closeMe()
		{
			this._intentExit = true;
			this.stopServer(true);
			this._account = null;
			base.Close();
		}

		public void startRelogin()
		{
			this._isRelogin = true;
			this.stop_relogin_timer();
			this.login_worker.CancelAsync();
			this.login_worker.RunWorkerAsync();
		}

		private void getCustomSession()
		{
			if (this._account == null || this._account.Server_type != ServerType.Custom)
			{
				return;
			}
			string text = this._account.CustomGameUrl;
			if (text == null)
			{
				return;
			}
			if (text.EndsWith("/"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			string uRL = text + "/root/";
			this.subBrowser.Navigate(uRL);
            Console.WriteLine("uRL: {0}", uRL);
		}

		private void logMeSafe(string logtext)
		{
			if (this.logText.InvokeRequired)
			{
				this.logText.Invoke(new MainForm.logMeDelegate(this.logMe), new object[]
				{
					logtext
				});
				return;
			}
			this.logMe(logtext);
		}

		private void logMe(string logtext)
		{
			string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", this._factory.TmrMgr.DateTimeNow, logtext);
			this.logText.AppendText(text);
			this.logText.Select(this.logText.Text.Length - text.Length, text.Length);
			this.logText.SelectionColor = Color.Red;
			this.logText.Select(this.logText.Text.Length, 0);
			this.logText.AppendText("\n");
			this.logText.ScrollToCaret();
		}

		private void login_worker_DoWork(object sender, DoWorkEventArgs e)
		{
			CommonReceiver loginClient = new CommonReceiver(this._account, this.login_worker);
			if (this._login_result != null && this._login_result.StatusCode == LoginStatusCode.NeedVerifyCode)
			{
				this.logMeSafe("重新登录需要验证码呀大哥, 貌似你还是关了再重新打开才行");
				return;
			}
			//List<Cookie> list = new List<Cookie>();
            _cookies = new List<Cookie>();
            this._login_result = this._loginMgr.doLogin(loginClient, ref _cookies, null, null);
			e.Result = this._login_result;
		}

		private void login_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			string text = e.UserState as string;
			if (text == null)
			{
				return;
			}
			this.logMeSafe(text);
		}

		private void login_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LoginResult loginResult = e.Result as LoginResult;
			if (loginResult == null)
			{
				this.logMeSafe("重新登录失败");
				this.startReLoginTimerImpl(30);
				return;
			}
			string arg_29_0 = loginResult.StatusDesc;
			bool flag = loginResult.StatusCode == LoginStatusCode.Success;
			if (loginResult.StatusCode == LoginStatusCode.NeedVerifyCode)
			{
				this.logMeSafe("重新登录需要验证码呀大哥, 貌似你还是关了再重新打开才行");
				return;
			}
			if (!flag)
			{
				this.logMeSafe("重新登录失败");
				this.startReLoginTimerImpl(30);
				return;
			}
			this.initGame(this._account, loginResult);
		}

		public void initGameFromArgs(AccountData acc)
		{
			this._account = acc;
			this._exeMgr = new ExeMgr(this._account.GameConf, this._gameUser);
			this._settingMgr = new SettingsMgr(this._account.GameConf, this._gameUser);
			this._account.Status = AccountStatus.STA_not_start;
			if (this._account.Server_type == ServerType.Custom)
			{
				this.gameBrowser.Navigate(this._account.CustomLoginUrl);
                Console.WriteLine("this._account.CustomLoginUrl: {0}", this._account.CustomLoginUrl);
				return;
			}
			string gameUrl = acc.GameUrl;
			foreach (Cookie current in CommonUtils.getCookieByUrl(acc.Cookies, gameUrl))
			{
				if (current != null)
				{
					CommonUtils.setCookie(gameUrl, current.Name, current.Value, current.Path, current.Domain, current.Expires);
				}
			}
			Uri uri = new Uri(gameUrl);
			CommonUtils.setCookie(string.Format("{0}://{1}/", uri.Scheme, uri.Host), "JSESSIONID", acc.JsessionId, "/root/", uri.Host);
			this._jsessionid = acc.JsessionId;
			this._gameurl = acc.GameUrl;
			this.gameBrowser.Navigate(gameUrl);
            Console.WriteLine("gameUrl: {0}", gameUrl);
			this.start_delay_init(false);
		}

		public void initGame(AccountData acc, LoginResult state)
		{
			this._account = acc;
			this._exeMgr = new ExeMgr(this._account.GameConf, this._gameUser);
			this._settingMgr = new SettingsMgr(this._account.GameConf, this._gameUser);
			this._account.Status = AccountStatus.STA_not_start;
			if (this._account.Server_type == ServerType.Custom)
			{
				this.gameBrowser.Navigate(this._account.CustomLoginUrl);
                Console.WriteLine("this._account.CustomLoginUrl: {0}", this._account.CustomLoginUrl);
				return;
			}
			string gameUrl = state.GameUrl;
			foreach (Cookie current in CommonUtils.getCookieByUrl(state.WebCookies, gameUrl))
			{
				if (current != null)
				{
					CommonUtils.setCookie(gameUrl, current.Name, current.Value, current.Path, current.Domain, current.Expires);
				}
			}
			Uri uri = new Uri(gameUrl);
			CommonUtils.setCookie(string.Format("{0}://{1}/", uri.Scheme, uri.Host), "JSESSIONID", state.JSessionID, "/root/", uri.Host);
			this._account.Cookies = state.WebCookies;
			this._account.JsessionId = state.JSessionID;
			this._account.GameUrl = state.GameUrl;
			this._jsessionid = state.JSessionID;
			this._gameurl = state.GameUrl;
			this.gameBrowser.Navigate(gameUrl);
            Console.WriteLine("gameUrl: {0}", gameUrl);
			this.start_delay_init(false);
		}

		private void init_session()
		{
			try
			{
				ProtocolMgr protocolMgr = new ProtocolMgr(this._gameUser, this, this, this._gameurl, this._jsessionid, this._factory);
				this._factory.getMiscManager().getServerTime(protocolMgr, this);
				int playerInfo = this._factory.getMiscManager().getPlayerInfo(protocolMgr, this, this._account.RoleName, this._gameUser);
				if (playerInfo == 2)
				{
					UiUtils.getInstance().error("您选择的角色不存在");
				}
				else if (playerInfo == 3)
				{
					UiUtils.getInstance().error("切换角色失败");
				}
				else if (playerInfo == 4)
				{
					UiUtils.getInstance().error("角色被封号");
				}
				else if (playerInfo == 1 || playerInfo == 10)
				{
					UiUtils.getInstance().error("获取用户信息失败, 请重试");
				}
				else
				{
					if (!this._accountFromArgs || UserSessionMgr.instance.isSf)
					{
						this._account.GameConf = new GameConfig(EnumString.getString(this._account.Server_type), this._account.ServerId, this._gameUser.Username);
						this._account.GameConf.loadSettings();
					}
					this.buildServers();
					this._exeMgr.setExeVariables(protocolMgr, this, this, this._gameUser, this._account.GameConf, this._factory);
					if (this._logger == null)
					{
						this._logger = new LogHelper(EnumString.getString(this._account.Server_type), this._account.ServerId, this._gameUser.Username, "");
					}
					this._exeMgr.init_data();
					this.init_completed();
				}
			}
			catch (Exception ex)
			{
				this.logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
			}
			finally
			{
				this._init_thread = null;
			}
		}

		private void init_data()
		{
			try
			{
				if (this._exeMgr != null)
				{
					this._exeMgr.init_data();
				}
			}
			catch (Exception ex)
			{
				this.logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
			}
			finally
			{
				this._init_thread = null;
			}
		}

		private void start_delay_init(bool is_only_init_data = false)
		{
			if (this._init_thread == null)
			{
				if (is_only_init_data)
				{
					this._init_thread = new Thread(new ThreadStart(this.init_data));
				}
				else
				{
					this._init_thread = new Thread(new ThreadStart(this.init_session));
				}
				this._init_thread.Start();
			}
		}

		private void menu_test_Click(object sender, EventArgs e)
		{
            ProtocolMgr protocolMgr = new ProtocolMgr(this._gameUser, this, this, this._gameurl, this._jsessionid, this._factory);
            AttackExe exe = new AttackExe();
            exe.setVariables(protocolMgr, this, this, this._gameUser, this._account.GameConf, this._factory);
            exe.do_juedou_attack(288, 310);
		}

		private void buildServers()
		{
			if (GlobalConfig.isDebug)
			{
				this._settingMgr.addSetting(new CommonServer(this));
				this._settingMgr.addSetting(new BuildingServer(this));
				this._settingMgr.addSetting(new HeroTrainServer(this));
				this._settingMgr.addSetting(new HeroWashServer(this));
				this._settingMgr.addSetting(new SecretaryServer(this));
				this._settingMgr.addSetting(new QhServer(this));
				this._settingMgr.addSetting(new QiFuServer(this));
				this._settingMgr.addSetting(new BaiShenServer(this));
				this._settingMgr.addSetting(new ShenHuoServer(this));
				this._settingMgr.addSetting(new MhServer(this));
				this._settingMgr.addSetting(new MerchantServer(this));
				this._settingMgr.addSetting(new DailyWeaponServer(this));
				this._settingMgr.addSetting(new WeaponServer(this));
				this._settingMgr.addSetting(new StoreServer(this));
				this._settingMgr.addSetting(new PolishServer(this));
				this._settingMgr.addSetting(new DinnerServer(this));
				this._settingMgr.addSetting(new FeteServer(this));
				this._settingMgr.addSetting(new ImposeServer(this));
				this._settingMgr.addSetting(new MarketServer(this));
				this._settingMgr.addSetting(new MovableServer(this));
				this._settingMgr.addSetting(new StockServer(this));
				this._settingMgr.addSetting(new StoneServer(this));
				this._settingMgr.addSetting(new BattleServer(this));
				this._settingMgr.addSetting(new AttackServer(this));
				this._settingMgr.addSetting(new ResCampaignServer(this));
				this._settingMgr.addSetting(new ArchServer(this));
				this._settingMgr.addSetting(new BoatServer(this));
				this._settingMgr.addSetting(new YueBingServer(this));
				this._settingMgr.addSetting(new CrossPlatformCompeteServer(this));
				this._settingMgr.addSetting(new DailyTreasureGameServer(this));
				this._settingMgr.addSetting(new GemFlopServer(this));
				this._settingMgr.addSetting(new SilverFlopServer(this));
				this._settingMgr.addSetting(new JailEventServer(this));
				this._settingMgr.addSetting(new KfBanquetServer(this));
				this._settingMgr.addSetting(new GiftEventServer(this));
				this._settingMgr.addSetting(new PlayerCompeteServer(this));
				this._settingMgr.addSetting(new TreasureGameServer(this));
				this._settingMgr.addSetting(new WorldArmyServer(this));
				this._settingMgr.addSetting(new FestivalEventServer(this));
				this._settingMgr.addSetting(new TroopFeedbackServer(this));
				this._settingMgr.addSetting(new SuperFanpaiServer(this));
				this._settingMgr.addSetting(new TowerServer(this));
				this._settingMgr.addSetting(new TroopTurntableServer(this));
				this._settingMgr.addSetting(new CakeServer(this));
				this._settingMgr.addSetting(new GemDumpServer(this));
				this._settingMgr.addSetting(new NewDayTreasureGameServer(this));
				this._settingMgr.addSetting(new BaoshiStoneServer(this));
                this._settingMgr.addSetting(new WarChariotServer(this));
				this._exeMgr.addExe(new CommonExe());
				this._exeMgr.addExe(new PingExe());
				this._exeMgr.addExe(new BuildingExe());
				this._exeMgr.addExe(new HeroTrainExe());
				this._exeMgr.addExe(new HeroWashExe());
                this._exeMgr.addExe(new SecretaryExe());
                this._exeMgr.addExe(new MovableExe());
				this._exeMgr.addExe(new QhExe());
				this._exeMgr.addExe(new MhExe());
				this._exeMgr.addExe(new QiFuExe());
				this._exeMgr.addExe(new BaiShenExe());
				this._exeMgr.addExe(new ShenHuoExe());
				this._exeMgr.addExe(new MerchantExe());
				this._exeMgr.addExe(new DailyWeaponExe());
				this._exeMgr.addExe(new WeaponExe());
				this._exeMgr.addExe(new StoreExe());
				this._exeMgr.addExe(new PolishExe());
				this._exeMgr.addExe(new DinnerExe());
				this._exeMgr.addExe(new FeteExe());
				this._exeMgr.addExe(new ImposeExe());
				this._exeMgr.addExe(new MarketExe());
				this._exeMgr.addExe(new StockExe());
				this._exeMgr.addExe(new StoneExe());
				this._exeMgr.addExe(new BattleExe());
				this._exeMgr.addExe(new AttackExe());
				this._exeMgr.addExe(new ResCampaignExe());
				this._exeMgr.addExe(new ArchExe());
				this._exeMgr.addExe(new BoatExe());
				this._exeMgr.addExe(new YueBingExe());
				this._exeMgr.addExe(new CrossplatformCompeteExe());
				this._exeMgr.addExe(new DailyTreasureGameExe());
				this._exeMgr.addExe(new GemFlopExe());
				this._exeMgr.addExe(new SilverFlopExe());
				this._exeMgr.addExe(new JailEventExe());
				this._exeMgr.addExe(new KfBanquetExe());
				this._exeMgr.addExe(new GiftEventExe());
				this._exeMgr.addExe(new PlayerCompeteExe());
				this._exeMgr.addExe(new TreasureGameExe());
				this._exeMgr.addExe(new WorldArmyExe());
				this._exeMgr.addExe(new FestivalEventExe());
				this._exeMgr.addExe(new TroopFeedbackExe());
				this._exeMgr.addExe(new SuperFanpaiExe());
				this._exeMgr.addExe(new TowerExe());
				this._exeMgr.addExe(new TroopTurntableExe());
				this._exeMgr.addExe(new CakeExe());
				this._exeMgr.addExe(new GemDumpExe());
				this._exeMgr.addExe(new NewDayTreasureGameExe());
				this._exeMgr.addExe(new BaoshiStoneExe());
                this._exeMgr.addExe(new WarChariotExe());
				return;
			}
			this._settingMgr.addSetting(new CommonServer(this));
			this._settingMgr.addSetting(new BuildingServer(this));
			this._settingMgr.addSetting(new HeroTrainServer(this));
			this._settingMgr.addSetting(new HeroWashServer(this));
			this._settingMgr.addSetting(new SecretaryServer(this));
			this._settingMgr.addSetting(new QhServer(this));
			this._settingMgr.addSetting(new QiFuServer(this));
			this._settingMgr.addSetting(new BaiShenServer(this));
			this._settingMgr.addSetting(new ShenHuoServer(this));
			this._settingMgr.addSetting(new MhServer(this));
			this._settingMgr.addSetting(new MerchantServer(this));
			this._settingMgr.addSetting(new DailyWeaponServer(this));
			this._settingMgr.addSetting(new WeaponServer(this));
			this._settingMgr.addSetting(new StoreServer(this));
			this._settingMgr.addSetting(new PolishServer(this));
			this._settingMgr.addSetting(new DinnerServer(this));
			this._settingMgr.addSetting(new FeteServer(this));
			this._settingMgr.addSetting(new ImposeServer(this));
			this._settingMgr.addSetting(new MarketServer(this));
			this._settingMgr.addSetting(new MovableServer(this));
			this._settingMgr.addSetting(new StockServer(this));
			this._settingMgr.addSetting(new StoneServer(this));
			this._settingMgr.addSetting(new BattleServer(this));
			this._settingMgr.addSetting(new AttackServer(this));
			this._settingMgr.addSetting(new ResCampaignServer(this));
			this._settingMgr.addSetting(new ArchServer(this));
			this._settingMgr.addSetting(new BoatServer(this));
			this._settingMgr.addSetting(new YueBingServer(this));
			this._settingMgr.addSetting(new CrossPlatformCompeteServer(this));
			this._settingMgr.addSetting(new DailyTreasureGameServer(this));
			this._settingMgr.addSetting(new GemFlopServer(this));
			this._settingMgr.addSetting(new SilverFlopServer(this));
			this._settingMgr.addSetting(new JailEventServer(this));
			this._settingMgr.addSetting(new KfBanquetServer(this));
			this._settingMgr.addSetting(new GiftEventServer(this));
			this._settingMgr.addSetting(new PlayerCompeteServer(this));
			this._settingMgr.addSetting(new TreasureGameServer(this));
			this._settingMgr.addSetting(new WorldArmyServer(this));
			this._settingMgr.addSetting(new FestivalEventServer(this));
			this._settingMgr.addSetting(new TroopFeedbackServer(this));
			this._settingMgr.addSetting(new SuperFanpaiServer(this));
			this._settingMgr.addSetting(new TowerServer(this));
			this._settingMgr.addSetting(new TroopTurntableServer(this));
			this._settingMgr.addSetting(new CakeServer(this));
			this._settingMgr.addSetting(new GemDumpServer(this));
			this._settingMgr.addSetting(new NewDayTreasureGameServer(this));
			this._settingMgr.addSetting(new BaoshiStoneServer(this));
            this._settingMgr.addSetting(new WarChariotServer(this));
			this._exeMgr.addExe(new PingExe());
			this._exeMgr.addExe(new CommonExe());
			this._exeMgr.addExe(new BuildingExe());
			this._exeMgr.addExe(new HeroTrainExe());
			this._exeMgr.addExe(new HeroWashExe());
			this._exeMgr.addExe(new SecretaryExe());
            this._exeMgr.addExe(new MovableExe());
			this._exeMgr.addExe(new QhExe());
			this._exeMgr.addExe(new MhExe());
			this._exeMgr.addExe(new QiFuExe());
			this._exeMgr.addExe(new BaiShenExe());
			this._exeMgr.addExe(new ShenHuoExe());
			this._exeMgr.addExe(new MerchantExe());
			this._exeMgr.addExe(new DailyWeaponExe());
			this._exeMgr.addExe(new WeaponExe());
			this._exeMgr.addExe(new StoreExe());
			this._exeMgr.addExe(new PolishExe());
			this._exeMgr.addExe(new DinnerExe());
			this._exeMgr.addExe(new FeteExe());
			this._exeMgr.addExe(new ImposeExe());
			this._exeMgr.addExe(new MarketExe());
			this._exeMgr.addExe(new StockExe());
			this._exeMgr.addExe(new StoneExe());
			this._exeMgr.addExe(new BattleExe());
			this._exeMgr.addExe(new AttackExe());
			this._exeMgr.addExe(new ResCampaignExe());
			this._exeMgr.addExe(new ArchExe());
			this._exeMgr.addExe(new BoatExe());
			this._exeMgr.addExe(new YueBingExe());
			this._exeMgr.addExe(new CrossplatformCompeteExe());
			this._exeMgr.addExe(new DailyTreasureGameExe());
			this._exeMgr.addExe(new GemFlopExe());
			this._exeMgr.addExe(new SilverFlopExe());
			this._exeMgr.addExe(new JailEventExe());
			this._exeMgr.addExe(new KfBanquetExe());
			this._exeMgr.addExe(new GiftEventExe());
			this._exeMgr.addExe(new PlayerCompeteExe());
			this._exeMgr.addExe(new TreasureGameExe());
			this._exeMgr.addExe(new WorldArmyExe());
			this._exeMgr.addExe(new FestivalEventExe());
			this._exeMgr.addExe(new TroopFeedbackExe());
			this._exeMgr.addExe(new SuperFanpaiExe());
			this._exeMgr.addExe(new TowerExe());
			this._exeMgr.addExe(new TroopTurntableExe());
			this._exeMgr.addExe(new CakeExe());
			this._exeMgr.addExe(new GemDumpExe());
			this._exeMgr.addExe(new NewDayTreasureGameExe());
			this._exeMgr.addExe(new BaoshiStoneExe());
            this._exeMgr.addExe(new WarChariotExe());
		}

		public void startReLoginTimerImpl(int waitSeconds = 1800)
		{
			if (waitSeconds == 0)
			{
				this.startRelogin();
				return;
			}
			if (this._relogin_timer != null)
			{
				return;
			}
			this.logMeSafe(string.Format("将在{0}后开始重新登录", (waitSeconds >= 60) ? string.Format("{0}分钟", waitSeconds / 60) : string.Format("{0}秒", waitSeconds)));
			this.start_relogin_timer(waitSeconds);
		}

		private void tmr_relogin_Tick(object sender, EventArgs e)
		{
            try
            {
                this.stop_relogin_timer();
                this.startRelogin();
            }
            catch (Exception ex)
            {
                Console.WriteLine("tmr_relogin_Tick:{0}", ex.Message);
            }
		}

		private void start_relogin_timer(int waitSeconds)
		{
			this.stop_relogin_timer();
			this._relogin_timer = new System.Timers.Timer();
			this._relogin_timer.Interval = (double)(waitSeconds * 1000);
			this._relogin_timer.Elapsed += new ElapsedEventHandler(this.tmr_relogin_Tick);
			this._relogin_timer.Start();
		}

		private void stop_relogin_timer()
		{
			if (this._relogin_timer != null)
			{
				this._relogin_timer.Stop();
				this._relogin_timer = null;
			}
		}

		public void logDebug(string text)
		{
			this.LogSafe(text, LogLevel.Debug, Color.Blue);
		}

		public void logError(string text)
		{
			this.LogSafe(text, LogLevel.Error, Color.Red);
		}

		public void log(string text, Color color)
		{
			this.LogSafe(text, LogLevel.Info, color);
		}

		public void logSingle(string text)
		{
			this.LogSingleSafe(text);
		}

		public void logSurprise(string text)
		{
			this.logSurpriseSafe(text, Color.Black);
		}

		public void LogSingleSafe(string logtext)
		{
			if (this._logSingleDelegate == null)
			{
				this._logSingleDelegate = new MainForm.LogSingleDelegate(this.LogSingle);
			}
			if (this.lblStatus.InvokeRequired)
			{
				this.lblStatus.Invoke(this._logSingleDelegate, new object[]
				{
					logtext
				});
				return;
			}
			this.LogSingle(logtext);
		}

		internal void LogSingle(string logtext)
		{
			this.lblStatus.Text = logtext;
			if (logtext == "挂机就绪")
			{
				this.menu_startServer.Enabled = true;
				this.menu_stopServer.Enabled = false;
			}
		}

		public void LogDebug(string logtext)
		{
			this.LogSafe(logtext, LogLevel.Debug, Color.Gray);
		}

		public void LogInfo(string logtext, Color logColor)
		{
			this.LogSafe(logtext, LogLevel.Info, logColor);
		}

		public void LogError(string logtext)
		{
			this.LogSafe(logtext, LogLevel.Error, Color.Red);
		}

		protected void LogSafe(string logtext, LogLevel level, Color logColor)
		{
			if (this._logDelegate == null)
			{
				this._logDelegate = new MainForm.LogDelegate(this.Log);
			}
			if (this.logText.InvokeRequired)
			{
				this.logText.Invoke(this._logDelegate, new object[]
				{
					logtext,
					level,
					logColor
				});
				return;
			}
			this.Log(logtext, level, logColor);
		}

		protected void Log(string logtext, LogLevel level, Color logColor)
		{
			if (level < this._logLevel)
			{
				return;
			}
			string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", this._factory.TmrMgr.DateTimeNow, logtext);
			if (this._logger != null)
			{
				this._logger.log(text);
			}
			if (level >= LogLevel.Debug)
			{
				if (this.logText == null)
				{
					return;
				}
				this.logText.AppendText(text);
				this.logText.Select(this.logText.Text.Length - text.Length, text.Length);
				this.logText.SelectionColor = logColor;
				this.logText.Select(this.logText.Text.Length, 0);
				this.logText.AppendText("\n");
				this.logText.ScrollToCaret();
			}
		}

		protected void LogFile(string logtext)
		{
			string logtext2 = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", this._factory.TmrMgr.DateTimeNow, logtext);
			if (this._logger != null)
			{
				this._logger.log(logtext2);
			}
		}

		protected void logSurpriseSafe(string logtext, Color logColor)
		{
			if (this._logSurpriseDelegate == null)
			{
				this._logSurpriseDelegate = new MainForm.LogSurpriseDelegate(this.LogSurprise);
			}
			if (this.logSurpise.InvokeRequired)
			{
				this.logSurpise.Invoke(this._logSurpriseDelegate, new object[]
				{
					logtext,
					logColor
				});
				return;
			}
			this.LogSurprise(logtext, logColor);
		}

		public void LogSurprise(string logtext, Color logColor)
		{
			if (this.logSurpise == null)
			{
				return;
			}
			string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", DateTime.Now, logtext);
			this.logSurpise.AppendText(text);
			this.logSurpise.Select(this.logSurpise.Text.Length - text.Length, text.Length);
			this.logSurpise.SelectionColor = logColor;
			this.logSurpise.Select(this.logSurpise.Text.Length, 0);
			this.logSurpise.AppendText("\n");
			this.logSurpise.ScrollToCaret();
		}

		public void LogTemp(string logtext, LogLevel level, Color logColor)
		{
			this.logTempSafe(logtext, level, logColor);
		}

		private void logTempSafe(string text, LogLevel level, Color logColor)
		{
			if (this._logTempSafe == null)
			{
				this._logTempSafe = new MainForm.logTempDelegate(this.logTemperory);
			}
			if (this.logTemp.InvokeRequired)
			{
				this.logTemp.Invoke(this._logTempSafe, new object[]
				{
					text,
					level,
					logColor
				});
				return;
			}
			this.logTemperory(text, level, logColor);
		}

		private void logTemperory(string text, LogLevel level, Color logColor)
		{
            if (level < this._logLevel)
            {
                return;
            }
			string text2 = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", this._factory.TmrMgr.DateTimeNow, text);
			if (this._logger != null)
			{
				this._logger.log(text2);
			}
			if (level >= LogLevel.Debug)
			{
				if (this.logTemp == null)
				{
					return;
				}
				this.logTemp.AppendText(text2);
				this.logTemp.Select(this.logTemp.Text.Length - text2.Length, text2.Length);
				this.logTemp.SelectionColor = logColor;
				this.logTemp.Select(this.logText.Text.Length, 0);
				this.logTemp.AppendText("\n");
				this.logTemp.ScrollToCaret();
			}
		}

		public void notifyState(AccountStatus status)
		{
			this._account.Status = status;
		}

		public void init_completed()
		{
			if (this._isRelogin)
			{
				this.startServer();
				return;
			}
			this.LogSingleSafe("挂机就绪");
			this._serverStatus = 1;
		}

		private void _exe_proc()
		{
			int num = 0;
			while (this._server_running)
			{
				if (this._exeMgr == null)
				{
					Thread.Sleep(100);
				}
				else
				{
					try
					{
						if (num < 20)
						{
							num++;
							Thread.Sleep(100);
						}
						else
						{
							num = 0;
							this._exeMgr.fire(false);
							this.LogSingleSafe("挂机中...");
						}
					}
					catch (ThreadInterruptedException arg_44_0)
					{
						Console.WriteLine(arg_44_0);
					}
					catch (Exception ex)
					{
						this.logError(string.Format("error:{0}:{1}", ex.Message, ex.StackTrace));
					}
				}
			}
		}

		public void startServer()
		{
			this.stopServer(false);
			this._server_running = true;
			this._exeMgr.clear_runtime();
			this._exe_thread = new Thread(new ThreadStart(this._exe_proc));
			this._exe_thread.Start();
			this._serverStatus = 2;
		}

		public void stopServer(bool is_user_operate = false)
		{
			this._server_running = false;
			if (this._exe_thread != null)
			{
				try
				{
					this._exe_thread.Join(0);
					this._exe_thread.Interrupt();
					this._exe_thread = null;
				}
				catch
				{
				}
			}
			if (is_user_operate)
			{
				this._serverStatus = 3;
				this._isRelogin = false;
				return;
			}
			this._serverStatus = 3;
			this._isRelogin = true;
		}

		public void startReLoginTimer()
		{
			this.startReLoginTimerImpl(1800);
		}

		public void refreshPlayerSafe()
		{
			this._gameUser.addUiToQueue("global");
		}

		public void notifySingleExe(string exe_name)
		{
			if (this._exeMgr == null)
			{
				return;
			}
			this._exeMgr.fireSingleForce(exe_name);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainTab = new System.Windows.Forms.TabControl();
            this.gamePage = new System.Windows.Forms.TabPage();
            this.gameBrowser = new AxSHDocVw.AxWebBrowser();
            this.setPage = new System.Windows.Forms.TabPage();
            this.chk_AutoBuyCredit = new System.Windows.Forms.CheckBox();
            this.chk_Cai = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.combo_daily_weapon_type = new System.Windows.Forms.ComboBox();
            this.chk_daily_weapon = new System.Windows.Forms.CheckBox();
            this.btn_weaponCalc = new System.Windows.Forms.Button();
            this.txt_weaponinfo = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chk_upgrade_weapone_enable = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.nUD_toukui_ticket = new System.Windows.Forms.NumericUpDown();
            this.chk_upgrade_toukui = new System.Windows.Forms.CheckBox();
            this.num_equip_qh_use_gold_limit = new System.Windows.Forms.NumericUpDown();
            this.chk_equip_qh_use_gold = new System.Windows.Forms.CheckBox();
            this.dg_equipQh = new System.Windows.Forms.DataGridView();
            this.col_chk_equipEnchant = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_name_equipEnchant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_level_equipEnchant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk_equip_qh_enable = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.num_mh_failcount = new System.Windows.Forms.NumericUpDown();
            this.combo_mh_type = new System.Windows.Forms.ComboBox();
            this.dg_equipMh = new System.Windows.Forms.DataGridView();
            this.col_chk_equipMagic = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_name_equipMagic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_level_equipMagic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk_equip_mh_enable = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.num_yupei_attr = new System.Windows.Forms.NumericUpDown();
            this.chk_auto_melt_yupei = new System.Windows.Forms.CheckBox();
            this.num_upgrade_crystal = new System.Windows.Forms.NumericUpDown();
            this.chk_upgrade_war_chariot = new System.Windows.Forms.CheckBox();
            this.chk_upgrade_crystal = new System.Windows.Forms.CheckBox();
            this.chk_openbox_for_yupei = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.num_polish_melt_failcount = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.num_polish_item_reserve = new System.Windows.Forms.NumericUpDown();
            this.num_upgrade_gem = new System.Windows.Forms.NumericUpDown();
            this.num_polish_goon = new System.Windows.Forms.NumericUpDown();
            this.num_polish_reserve = new System.Windows.Forms.NumericUpDown();
            this.chk_upgrade_gem = new System.Windows.Forms.CheckBox();
            this.chk_polish_enable = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.num_fete_6 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.num_fete_5 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.num_fete_4 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.num_fete_3 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.num_fete_2 = new System.Windows.Forms.NumericUpDown();
            this.lbl_fete_silver = new System.Windows.Forms.Label();
            this.num_fete_1 = new System.Windows.Forms.NumericUpDown();
            this.chk_fete_enable = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.chk_market_usetoken = new System.Windows.Forms.CheckBox();
            this.chk_market_super = new System.Windows.Forms.CheckBox();
            this.chk_market_drop_notbuy = new System.Windows.Forms.CheckBox();
            this.chk_market_notbuy_iffail = new System.Windows.Forms.CheckBox();
            this.chk_market_force_attack = new System.Windows.Forms.CheckBox();
            this.chk_market_purpleweapon_gold = new System.Windows.Forms.CheckBox();
            this.chk_market_redweapon_gold = new System.Windows.Forms.CheckBox();
            this.chk_market_stone_gold = new System.Windows.Forms.CheckBox();
            this.chk_market_silver_gold = new System.Windows.Forms.CheckBox();
            this.chk_market_gem_gold = new System.Windows.Forms.CheckBox();
            this.combo_market_purpleweapon_type = new System.Windows.Forms.ComboBox();
            this.combo_market_redweapon_type = new System.Windows.Forms.ComboBox();
            this.combo_market_stone_type = new System.Windows.Forms.ComboBox();
            this.combo_market_silver_type = new System.Windows.Forms.ComboBox();
            this.combo_market_gem_type = new System.Windows.Forms.ComboBox();
            this.chk_market_purpleweapon = new System.Windows.Forms.CheckBox();
            this.chk_market_redweapon = new System.Windows.Forms.CheckBox();
            this.chk_market_stone = new System.Windows.Forms.CheckBox();
            this.chk_market_silver = new System.Windows.Forms.CheckBox();
            this.chk_market_gem = new System.Windows.Forms.CheckBox();
            this.chk_market_enable = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.combo_defense_formation = new System.Windows.Forms.ComboBox();
            this.chk_defense_format = new System.Windows.Forms.CheckBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label47 = new System.Windows.Forms.Label();
            this.chk_battle_event_stone_gold = new System.Windows.Forms.CheckBox();
            this.chk_battle_event_weapon_gold = new System.Windows.Forms.CheckBox();
            this.chk_battle_event_gem_gold = new System.Windows.Forms.CheckBox();
            this.chk_battle_event_weapon = new System.Windows.Forms.CheckBox();
            this.chk_battle_event_stone = new System.Windows.Forms.CheckBox();
            this.chk_battle_event_gem = new System.Windows.Forms.CheckBox();
            this.chk_battle_event_enabled = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.btn_selForceNpc = new System.Windows.Forms.Button();
            this.lbl_attackForceNpc = new System.Windows.Forms.Label();
            this.btn_selNpc = new System.Windows.Forms.Button();
            this.lbl_attackNpc = new System.Windows.Forms.Label();
            this.num_attack_npc_reserve_order = new System.Windows.Forms.NumericUpDown();
            this.chk_attack_npc_enable = new System.Windows.Forms.CheckBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.chk_attack_army_first = new System.Windows.Forms.CheckBox();
            this.chk_attack_army_only_jingyingtime = new System.Windows.Forms.CheckBox();
            this.num_attack_army_first_interval = new System.Windows.Forms.NumericUpDown();
            this.num_attack_army_reserve_order = new System.Windows.Forms.NumericUpDown();
            this.dg_attack_army = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.column_firstbattle = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk_attack_army = new System.Windows.Forms.CheckBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.chk_juedou = new System.Windows.Forms.CheckBox();
            this.label67 = new System.Windows.Forms.Label();
            this.nUD_reserved_num = new System.Windows.Forms.NumericUpDown();
            this.label66 = new System.Windows.Forms.Label();
            this.chk_Nation = new System.Windows.Forms.CheckBox();
            this.txt_attack_filter_content = new System.Windows.Forms.TextBox();
            this.combo_attack_filter_type = new System.Windows.Forms.ComboBox();
            this.label48 = new System.Windows.Forms.Label();
            this.num_attack_cityevent_maxstar = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.num_attack_reserve_token = new System.Windows.Forms.NumericUpDown();
            this.chk_attack_reserve_token = new System.Windows.Forms.CheckBox();
            this.chk_attack_enable_attack = new System.Windows.Forms.CheckBox();
            this.chk_attack_get_extra_order = new System.Windows.Forms.CheckBox();
            this.chk_attack_jail_tech = new System.Windows.Forms.CheckBox();
            this.chk_attack_player_gongjian = new System.Windows.Forms.CheckBox();
            this.chk_attack_player_cityevent = new System.Windows.Forms.CheckBox();
            this.lbl_attack_target = new System.Windows.Forms.Label();
            this.btn_attack_target = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.num_attack_score = new System.Windows.Forms.NumericUpDown();
            this.chk_attack_not_injail = new System.Windows.Forms.CheckBox();
            this.chk_attack_npc_enemy = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.num_attack_player_level_max = new System.Windows.Forms.NumericUpDown();
            this.num_attack_player_level_min = new System.Windows.Forms.NumericUpDown();
            this.chk_attack_player_move_tokenfull = new System.Windows.Forms.CheckBox();
            this.chk_attack_player_use_token = new System.Windows.Forms.CheckBox();
            this.chk_attack_player = new System.Windows.Forms.CheckBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.lbl_attack_resCampaign = new System.Windows.Forms.Label();
            this.btn_sel_resCampaign = new System.Windows.Forms.Button();
            this.chk_res_campaign_enable = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chk_hero_giftevent = new System.Windows.Forms.CheckBox();
            this.chk_hero_nocd = new System.Windows.Forms.CheckBox();
            this.dg_heros = new System.Windows.Forms.DataGridView();
            this.col_chk_hero = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_name_hero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_level_hero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chk_hero_wash_zhi = new System.Windows.Forms.CheckBox();
            this.chk_hero_wash_yong = new System.Windows.Forms.CheckBox();
            this.chk_hero_wash_tong = new System.Windows.Forms.CheckBox();
            this.combo_hero_wash = new System.Windows.Forms.ComboBox();
            this.chk_hero_wash_enable = new System.Windows.Forms.CheckBox();
            this.chk_hero_tufei_enable = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chk_building_protect = new System.Windows.Forms.CheckBox();
            this.chk_selAll_building = new System.Windows.Forms.CheckBox();
            this.dg_buildings = new System.Windows.Forms.DataGridView();
            this.col_chk_building = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_name_building = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_level_building = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk_building_enable = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.num_get_baoshi_stone = new System.Windows.Forms.NumericUpDown();
            this.chk_get_baoshi_stone = new System.Windows.Forms.CheckBox();
            this.chk_secretary_giftbox = new System.Windows.Forms.CheckBox();
            this.num_secretary_open_treasure = new System.Windows.Forms.NumericUpDown();
            this.chk_secretary_open_treasure = new System.Windows.Forms.CheckBox();
            this.chk_dailytask_enable = new System.Windows.Forms.CheckBox();
            this.chk_stock_7 = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.chk_stock_6 = new System.Windows.Forms.CheckBox();
            this.chk_merchant_onlyfree = new System.Windows.Forms.CheckBox();
            this.chk_stock_5 = new System.Windows.Forms.CheckBox();
            this.combo_merchant_sell_level = new System.Windows.Forms.ComboBox();
            this.chk_stock_4 = new System.Windows.Forms.CheckBox();
            this.combo_merchant_type = new System.Windows.Forms.ComboBox();
            this.chk_stock_3 = new System.Windows.Forms.CheckBox();
            this.chk_merchant = new System.Windows.Forms.CheckBox();
            this.chk_stock_2 = new System.Windows.Forms.CheckBox();
            this.chk_stock_1 = new System.Windows.Forms.CheckBox();
            this.num_get_stone = new System.Windows.Forms.NumericUpDown();
            this.chk_stock_enable = new System.Windows.Forms.CheckBox();
            this.chk_get_stone = new System.Windows.Forms.CheckBox();
            this.chk_dinner = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.num_global_gem_price = new System.Windows.Forms.NumericUpDown();
            this.num_global_reserve_credit = new System.Windows.Forms.NumericUpDown();
            this.chk_global_reserve_credit = new System.Windows.Forms.CheckBox();
            this.num_global_reserve_stone = new System.Windows.Forms.NumericUpDown();
            this.chk_global_reserve_stone = new System.Windows.Forms.CheckBox();
            this.num_global_reserve_gold = new System.Windows.Forms.NumericUpDown();
            this.chk_global_reserve_gold = new System.Windows.Forms.CheckBox();
            this.num_global_reserve_silver = new System.Windows.Forms.NumericUpDown();
            this.chk_global_reserve_silver = new System.Windows.Forms.CheckBox();
            this.chk_global_logonAward = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.num_impose_loyalty = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.num_impose_reserve = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.num_force_impose_gold = new System.Windows.Forms.NumericUpDown();
            this.chk_impose_enable = new System.Windows.Forms.CheckBox();
            this.setPage2 = new System.Windows.Forms.TabPage();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.cb_OpenYuebingHongbaoType = new System.Windows.Forms.ComboBox();
            this.chk_AutoYuebingHongbao = new System.Windows.Forms.CheckBox();
            this.chk_GoldTrain = new System.Windows.Forms.CheckBox();
            this.chk_AutoYuebing = new System.Windows.Forms.CheckBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.label63 = new System.Windows.Forms.Label();
            this.nUD_boat_up_coin = new System.Windows.Forms.NumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.chk_AutoSelectTime = new System.Windows.Forms.CheckBox();
            this.txt_boat_upgrade = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.chk_boat_creat = new System.Windows.Forms.CheckBox();
            this.chk_AutoBoat = new System.Windows.Forms.CheckBox();
            this.chk_ShenHuo = new System.Windows.Forms.CheckBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.chk_SuperUseFree = new System.Windows.Forms.CheckBox();
            this.label56 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.nUD_DianQuan = new System.Windows.Forms.NumericUpDown();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.nUD_CiShu = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.nUD_BaoShi = new System.Windows.Forms.NumericUpDown();
            this.chk_AutoBaiShen = new System.Windows.Forms.CheckBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label51 = new System.Windows.Forms.Label();
            this.downUp_QiFuDQ = new System.Windows.Forms.NumericUpDown();
            this.label50 = new System.Windows.Forms.Label();
            this.downUp_QiFuCoin = new System.Windows.Forms.NumericUpDown();
            this.label49 = new System.Windows.Forms.Label();
            this.checkBox_QiFu = new System.Windows.Forms.CheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.chk_auto_hide_flash = new System.Windows.Forms.CheckBox();
            this.chk_global_boss_key_enable = new System.Windows.Forms.CheckBox();
            this.combo_global_boss_key_key = new System.Windows.Forms.ComboBox();
            this.chk_global_boss_key_shift = new System.Windows.Forms.CheckBox();
            this.chk_global_boss_key_alt = new System.Windows.Forms.CheckBox();
            this.chk_global_boss_key_ctrl = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.num_movable_reserve = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.num_movable_refine_factory_gold = new System.Windows.Forms.NumericUpDown();
            this.num_movable_refine_factory_stone = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.num_movable_weave_count = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.chk_movable_arch_refill = new System.Windows.Forms.CheckBox();
            this.label38 = new System.Windows.Forms.Label();
            this.num_movable_visit_fail = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.txt_movable_visit = new System.Windows.Forms.TextBox();
            this.chk_movable_arch_trade_max = new System.Windows.Forms.CheckBox();
            this.label32 = new System.Windows.Forms.Label();
            this.num_movable_refine_reserve = new System.Windows.Forms.NumericUpDown();
            this.chk_movable_arch_trade = new System.Windows.Forms.CheckBox();
            this.chk_movable_winter_trade = new System.Windows.Forms.CheckBox();
            this.label30 = new System.Windows.Forms.Label();
            this.combo_movable_refine_item = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txt_movable_order = new System.Windows.Forms.TextBox();
            this.num_movable_weave_price = new System.Windows.Forms.NumericUpDown();
            this.chk_movable_enable = new System.Windows.Forms.CheckBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.chk_new_day_treasure_game_shake_tree = new System.Windows.Forms.CheckBox();
            this.label69 = new System.Windows.Forms.Label();
            this.txt_new_day_treasure_game_boxtype = new System.Windows.Forms.TextBox();
            this.chk_new_day_treasure_game = new System.Windows.Forms.CheckBox();
            this.txt_treasure_game_dice_type = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.lbl_day_treasure_game_sel = new System.Windows.Forms.Label();
            this.btn_day_treasure_game_select = new System.Windows.Forms.Button();
            this.chk_daily_treasure_game_super_gold_end = new System.Windows.Forms.CheckBox();
            this.chk_treasure_game_ticket_gold_end = new System.Windows.Forms.CheckBox();
            this.combo_treasure_game_use_ticket_type = new System.Windows.Forms.ComboBox();
            this.chk_treasure_game_use_ticket = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txt_treasure_game_goldmove_boxtype = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.txt_treasure_game_boxtype = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.txt_daily_treasure_game_goldmove_boxtype = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txt_daily_treasure_game_boxtype = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.num_treasure_game_goldstep = new System.Windows.Forms.NumericUpDown();
            this.num_daily_treasure_game_goldstep = new System.Windows.Forms.NumericUpDown();
            this.chk_treasure_game_enable = new System.Windows.Forms.CheckBox();
            this.chk_daily_treasure_game = new System.Windows.Forms.CheckBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.combo_tower_type = new System.Windows.Forms.ComboBox();
            this.chk_tower_enable = new System.Windows.Forms.CheckBox();
            this.chk_world_army_openbox = new System.Windows.Forms.CheckBox();
            this.chk_festival_event = new System.Windows.Forms.CheckBox();
            this.combo_world_army_refresh_type = new System.Windows.Forms.ComboBox();
            this.chk_world_army_buybox2 = new System.Windows.Forms.CheckBox();
            this.chk_world_army_buybox1 = new System.Windows.Forms.CheckBox();
            this.chk_world_army_enable = new System.Windows.Forms.CheckBox();
            this.combo_world_army_boxtype = new System.Windows.Forms.ComboBox();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label65 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.nm_Out = new System.Windows.Forms.NumericUpDown();
            this.chk_gemdump_enable = new System.Windows.Forms.CheckBox();
            this.num_troop_turntable_buygold = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.combo_troop_turntable_type = new System.Windows.Forms.ComboBox();
            this.chk_troop_turntable_goldratio = new System.Windows.Forms.CheckBox();
            this.chk_cake_event = new System.Windows.Forms.CheckBox();
            this.chk_troop_turntable = new System.Windows.Forms.CheckBox();
            this.chk_super_fanpai = new System.Windows.Forms.CheckBox();
            this.label39 = new System.Windows.Forms.Label();
            this.chk_troop_feedback_openbox = new System.Windows.Forms.CheckBox();
            this.combo_troop_feedback_opentype = new System.Windows.Forms.ComboBox();
            this.chk_troop_feedback_opentreasure = new System.Windows.Forms.CheckBox();
            this.chk_troop_feedback_doubleweapon = new System.Windows.Forms.CheckBox();
            this.chk_troop_feedback_refine_notired = new System.Windows.Forms.CheckBox();
            this.chk_troop_feedback_enable = new System.Windows.Forms.CheckBox();
            this.chk_gem_flop_enable = new System.Windows.Forms.CheckBox();
            this.num_gem_flop_upgrade_count = new System.Windows.Forms.NumericUpDown();
            this.chk_silver_flop_enable = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.txt_arch_upgrade = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.chk_arch_create = new System.Windows.Forms.CheckBox();
            this.txt_giftevent_serial = new System.Windows.Forms.TextBox();
            this.chk_giftevent_enable = new System.Windows.Forms.CheckBox();
            this.chk_arch_enable = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.chk_again = new System.Windows.Forms.CheckBox();
            this.combo_kfwd_openbox_type = new System.Windows.Forms.ComboBox();
            this.chk_kfwd_openbox = new System.Windows.Forms.CheckBox();
            this.combo_kfzb_market_weapon_pos_2 = new System.Windows.Forms.ComboBox();
            this.chk_kfzb_market_weapon_2 = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_ignore = new System.Windows.Forms.CheckBox();
            this.combo_kfzb_market_weapon_pos_1 = new System.Windows.Forms.ComboBox();
            this.combo_kfzb_market_gem_pos = new System.Windows.Forms.ComboBox();
            this.combo_kfzb_market_stone_pos = new System.Windows.Forms.ComboBox();
            this.combo_kfzb_market_silver_pos = new System.Windows.Forms.ComboBox();
            this.chk_kfzb_market_weapon_1 = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_gem = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_stone = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_silver = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market = new System.Windows.Forms.CheckBox();
            this.num_crossplatm_compete_gold = new System.Windows.Forms.NumericUpDown();
            this.num_kf_banquet_buygold = new System.Windows.Forms.NumericUpDown();
            this.chk_kf_banquet_nation = new System.Windows.Forms.CheckBox();
            this.chk_crossplatm_compete = new System.Windows.Forms.CheckBox();
            this.chk_kf_banquet_enabled = new System.Windows.Forms.CheckBox();
            this.chk_kfwd_enable = new System.Windows.Forms.CheckBox();
            this.chk_kfwd_buyreward = new System.Windows.Forms.CheckBox();
            this.logPage = new System.Windows.Forms.TabPage();
            this.lbl_tempStatus = new System.Windows.Forms.Label();
            this.btn_tempStop = new System.Windows.Forms.Button();
            this.btn_tempAdd = new System.Windows.Forms.Button();
            this.logTemp = new System.Windows.Forms.RichTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.logSurpise = new System.Windows.Forms.RichTextBox();
            this.txt_servers = new System.Windows.Forms.TextBox();
            this.logText = new System.Windows.Forms.RichTextBox();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menu_login = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_init = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_saveSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_relogin = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_startServer = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_stopServer = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_clearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_test = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_more = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_default = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_hideGame = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_reportViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_protocol_viewer = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_about = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWeaponCalc = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_trainCalc = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_translator = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_report = new System.Windows.Forms.ToolStripMenuItem();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbl_playerinfo = new System.Windows.Forms.Label();
            this.lbl_time = new System.Windows.Forms.Label();
            this.tip_msg = new System.Windows.Forms.ToolTip(this.components);
            this.notify_icon = new System.Windows.Forms.NotifyIcon(this.components);
            this.subBrowser = new AxSHDocVw.AxWebBrowser();
            this.login_worker = new System.ComponentModel.BackgroundWorker();
            this.mainTab.SuspendLayout();
            this.gamePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameBrowser)).BeginInit();
            this.setPage.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_toukui_ticket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_equip_qh_use_gold_limit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_equipQh)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_mh_failcount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_equipMh)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_yupei_attr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_upgrade_crystal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_melt_failcount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_item_reserve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_upgrade_gem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_goon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_reserve)).BeginInit();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_1)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_npc_reserve_order)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_army_first_interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_army_reserve_order)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_attack_army)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_reserved_num)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_cityevent_maxstar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_reserve_token)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_score)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_player_level_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_player_level_min)).BeginInit();
            this.tabPage8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_heros)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_buildings)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_get_baoshi_stone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_secretary_open_treasure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_get_stone)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_gem_price)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_credit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_stone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_gold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_silver)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_impose_loyalty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_impose_reserve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_force_impose_gold)).BeginInit();
            this.setPage2.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_boat_up_coin)).BeginInit();
            this.groupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_DianQuan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_CiShu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_BaoShi)).BeginInit();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downUp_QiFuDQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downUp_QiFuCoin)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_reserve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_refine_factory_gold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_refine_factory_stone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_weave_count)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_visit_fail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_refine_reserve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_weave_price)).BeginInit();
            this.groupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_treasure_game_goldstep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_daily_treasure_game_goldstep)).BeginInit();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_troop_turntable_buygold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_gem_flop_upgrade_count)).BeginInit();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_crossplatm_compete_gold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_kf_banquet_buygold)).BeginInit();
            this.logPage.SuspendLayout();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.gamePage);
            this.mainTab.Controls.Add(this.setPage);
            this.mainTab.Controls.Add(this.setPage2);
            this.mainTab.Controls.Add(this.logPage);
            this.mainTab.Location = new System.Drawing.Point(0, 30);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(1060, 633);
            this.mainTab.TabIndex = 0;
            // 
            // gamePage
            // 
            this.gamePage.Controls.Add(this.gameBrowser);
            this.gamePage.Location = new System.Drawing.Point(4, 22);
            this.gamePage.Name = "gamePage";
            this.gamePage.Padding = new System.Windows.Forms.Padding(3);
            this.gamePage.Size = new System.Drawing.Size(1052, 607);
            this.gamePage.TabIndex = 0;
            this.gamePage.Text = "游戏";
            this.gamePage.UseVisualStyleBackColor = true;
            // 
            // gameBrowser
            // 
            this.gameBrowser.Enabled = true;
            this.gameBrowser.Location = new System.Drawing.Point(3, 3);
            this.gameBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gameBrowser.OcxState")));
            this.gameBrowser.Size = new System.Drawing.Size(1050, 600);
            this.gameBrowser.TabIndex = 0;
            this.gameBrowser.NewWindow3 += new AxSHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(this.gameBrowser_NewWindow3);
            this.gameBrowser.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.gameBrowser_NavigateComplete2);
            // 
            // setPage
            // 
            this.setPage.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.setPage.Controls.Add(this.chk_AutoBuyCredit);
            this.setPage.Controls.Add(this.chk_Cai);
            this.setPage.Controls.Add(this.groupBox7);
            this.setPage.Controls.Add(this.groupBox10);
            this.setPage.Controls.Add(this.groupBox9);
            this.setPage.Controls.Add(this.groupBox8);
            this.setPage.Controls.Add(this.groupBox6);
            this.setPage.Controls.Add(this.groupBox5);
            this.setPage.Controls.Add(this.groupBox4);
            this.setPage.Controls.Add(this.groupBox3);
            this.setPage.Controls.Add(this.groupBox1);
            this.setPage.Location = new System.Drawing.Point(4, 22);
            this.setPage.Name = "setPage";
            this.setPage.Padding = new System.Windows.Forms.Padding(3);
            this.setPage.Size = new System.Drawing.Size(1052, 607);
            this.setPage.TabIndex = 2;
            this.setPage.Text = "设置1";
            // 
            // chk_AutoBuyCredit
            // 
            this.chk_AutoBuyCredit.AutoSize = true;
            this.chk_AutoBuyCredit.Location = new System.Drawing.Point(94, 139);
            this.chk_AutoBuyCredit.Name = "chk_AutoBuyCredit";
            this.chk_AutoBuyCredit.Size = new System.Drawing.Size(108, 16);
            this.chk_AutoBuyCredit.TabIndex = 11;
            this.chk_AutoBuyCredit.Text = "自动点券换军功";
            this.chk_AutoBuyCredit.UseVisualStyleBackColor = true;
            // 
            // chk_Cai
            // 
            this.chk_Cai.AutoSize = true;
            this.chk_Cai.Location = new System.Drawing.Point(15, 139);
            this.chk_Cai.Name = "chk_Cai";
            this.chk_Cai.Size = new System.Drawing.Size(72, 16);
            this.chk_Cai.TabIndex = 9;
            this.chk_Cai.Text = "自动采集";
            this.chk_Cai.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tabControl1);
            this.groupBox7.Location = new System.Drawing.Point(654, 57);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(392, 273);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "装备管理";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(5, 17);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(383, 247);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage3.Controls.Add(this.combo_daily_weapon_type);
            this.tabPage3.Controls.Add(this.chk_daily_weapon);
            this.tabPage3.Controls.Add(this.btn_weaponCalc);
            this.tabPage3.Controls.Add(this.txt_weaponinfo);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.chk_upgrade_weapone_enable);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(375, 221);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "兵器";
            // 
            // combo_daily_weapon_type
            // 
            this.combo_daily_weapon_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_daily_weapon_type.FormattingEnabled = true;
            this.combo_daily_weapon_type.Items.AddRange(new object[] {
            "无敌将军炮",
            "五毒问心钉",
            "玄霆角",
            "七戮锋",
            "蟠龙华盖",
            "轩辕指南车",
            "落魂冥灯",
            "红衣将军炮",
            "化骨丛棘",
            "彤云角",
            "弑履岩",
            "玉麟旌麾",
            "化骨悬灯",
            "铜人计里车"});
            this.combo_daily_weapon_type.Location = new System.Drawing.Point(186, 3);
            this.combo_daily_weapon_type.Name = "combo_daily_weapon_type";
            this.combo_daily_weapon_type.Size = new System.Drawing.Size(100, 20);
            this.combo_daily_weapon_type.TabIndex = 27;
            // 
            // chk_daily_weapon
            // 
            this.chk_daily_weapon.AutoSize = true;
            this.chk_daily_weapon.Location = new System.Drawing.Point(6, 5);
            this.chk_daily_weapon.Name = "chk_daily_weapon";
            this.chk_daily_weapon.Size = new System.Drawing.Size(174, 16);
            this.chk_daily_weapon.TabIndex = 26;
            this.chk_daily_weapon.Text = "领取每日兵器(182级前有效)";
            this.chk_daily_weapon.UseVisualStyleBackColor = true;
            // 
            // btn_weaponCalc
            // 
            this.btn_weaponCalc.Location = new System.Drawing.Point(102, 40);
            this.btn_weaponCalc.Name = "btn_weaponCalc";
            this.btn_weaponCalc.Size = new System.Drawing.Size(119, 23);
            this.btn_weaponCalc.TabIndex = 25;
            this.btn_weaponCalc.Text = "打开兵器计算器";
            this.btn_weaponCalc.UseVisualStyleBackColor = true;
            this.btn_weaponCalc.Click += new System.EventHandler(this.btn_weaponCalc_Click);
            // 
            // txt_weaponinfo
            // 
            this.txt_weaponinfo.Location = new System.Drawing.Point(1, 69);
            this.txt_weaponinfo.Name = "txt_weaponinfo";
            this.txt_weaponinfo.ReadOnly = true;
            this.txt_weaponinfo.Size = new System.Drawing.Size(371, 146);
            this.txt_weaponinfo.TabIndex = 24;
            this.txt_weaponinfo.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "当前兵器概况：";
            // 
            // chk_upgrade_weapone_enable
            // 
            this.chk_upgrade_weapone_enable.AutoSize = true;
            this.chk_upgrade_weapone_enable.Location = new System.Drawing.Point(6, 28);
            this.chk_upgrade_weapone_enable.Name = "chk_upgrade_weapone_enable";
            this.chk_upgrade_weapone_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_upgrade_weapone_enable.TabIndex = 19;
            this.chk_upgrade_weapone_enable.Text = "升级兵器";
            this.chk_upgrade_weapone_enable.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage1.Controls.Add(this.label53);
            this.tabPage1.Controls.Add(this.label52);
            this.tabPage1.Controls.Add(this.nUD_toukui_ticket);
            this.tabPage1.Controls.Add(this.chk_upgrade_toukui);
            this.tabPage1.Controls.Add(this.num_equip_qh_use_gold_limit);
            this.tabPage1.Controls.Add(this.chk_equip_qh_use_gold);
            this.tabPage1.Controls.Add(this.dg_equipQh);
            this.tabPage1.Controls.Add(this.chk_equip_qh_enable);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(375, 221);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "强化";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(253, 203);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(29, 12);
            this.label53.TabIndex = 26;
            this.label53.Text = "点券";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(126, 203);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(53, 12);
            this.label52.TabIndex = 25;
            this.label52.Text = "强化到：";
            // 
            // nUD_toukui_ticket
            // 
            this.nUD_toukui_ticket.Location = new System.Drawing.Point(182, 197);
            this.nUD_toukui_ticket.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.nUD_toukui_ticket.Name = "nUD_toukui_ticket";
            this.nUD_toukui_ticket.Size = new System.Drawing.Size(63, 21);
            this.nUD_toukui_ticket.TabIndex = 24;
            this.nUD_toukui_ticket.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // chk_upgrade_toukui
            // 
            this.chk_upgrade_toukui.AutoSize = true;
            this.chk_upgrade_toukui.Location = new System.Drawing.Point(9, 203);
            this.chk_upgrade_toukui.Name = "chk_upgrade_toukui";
            this.chk_upgrade_toukui.Size = new System.Drawing.Size(96, 16);
            this.chk_upgrade_toukui.TabIndex = 23;
            this.chk_upgrade_toukui.Text = "免费强化头盔";
            this.chk_upgrade_toukui.UseVisualStyleBackColor = true;
            // 
            // num_equip_qh_use_gold_limit
            // 
            this.num_equip_qh_use_gold_limit.Location = new System.Drawing.Point(135, 173);
            this.num_equip_qh_use_gold_limit.Name = "num_equip_qh_use_gold_limit";
            this.num_equip_qh_use_gold_limit.Size = new System.Drawing.Size(146, 21);
            this.num_equip_qh_use_gold_limit.TabIndex = 20;
            // 
            // chk_equip_qh_use_gold
            // 
            this.chk_equip_qh_use_gold.AutoSize = true;
            this.chk_equip_qh_use_gold.Location = new System.Drawing.Point(9, 181);
            this.chk_equip_qh_use_gold.Name = "chk_equip_qh_use_gold";
            this.chk_equip_qh_use_gold.Size = new System.Drawing.Size(120, 16);
            this.chk_equip_qh_use_gold.TabIndex = 19;
            this.chk_equip_qh_use_gold.Text = "大于多少使用金币";
            this.chk_equip_qh_use_gold.UseVisualStyleBackColor = true;
            // 
            // dg_equipQh
            // 
            this.dg_equipQh.AllowDrop = true;
            this.dg_equipQh.AllowUserToAddRows = false;
            this.dg_equipQh.AllowUserToDeleteRows = false;
            this.dg_equipQh.AllowUserToOrderColumns = true;
            this.dg_equipQh.AllowUserToResizeRows = false;
            this.dg_equipQh.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dg_equipQh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_equipQh.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_chk_equipEnchant,
            this.col_name_equipEnchant,
            this.col_level_equipEnchant});
            this.dg_equipQh.Location = new System.Drawing.Point(5, 27);
            this.dg_equipQh.Name = "dg_equipQh";
            this.dg_equipQh.RowHeadersWidth = 30;
            this.dg_equipQh.RowTemplate.Height = 23;
            this.dg_equipQh.Size = new System.Drawing.Size(365, 140);
            this.dg_equipQh.TabIndex = 18;
            this.dg_equipQh.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_equipQh.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_equipQh.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_equipQh.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dg_rowPrePaint);
            this.dg_equipQh.SelectionChanged += new System.EventHandler(this.dg_SelectionChanged);
            this.dg_equipQh.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_DragDrop);
            this.dg_equipQh.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_DragEnter);
            // 
            // col_chk_equipEnchant
            // 
            this.col_chk_equipEnchant.DataPropertyName = "IsChecked";
            this.col_chk_equipEnchant.HeaderText = "";
            this.col_chk_equipEnchant.Name = "col_chk_equipEnchant";
            this.col_chk_equipEnchant.Width = 30;
            // 
            // col_name_equipEnchant
            // 
            this.col_name_equipEnchant.DataPropertyName = "EquipNameWithGeneral";
            this.col_name_equipEnchant.HeaderText = "名字";
            this.col_name_equipEnchant.Name = "col_name_equipEnchant";
            this.col_name_equipEnchant.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_name_equipEnchant.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_name_equipEnchant.Width = 225;
            // 
            // col_level_equipEnchant
            // 
            this.col_level_equipEnchant.DataPropertyName = "Level";
            this.col_level_equipEnchant.HeaderText = "等级";
            this.col_level_equipEnchant.Name = "col_level_equipEnchant";
            this.col_level_equipEnchant.Width = 60;
            // 
            // chk_equip_qh_enable
            // 
            this.chk_equip_qh_enable.AutoSize = true;
            this.chk_equip_qh_enable.Location = new System.Drawing.Point(5, 5);
            this.chk_equip_qh_enable.Name = "chk_equip_qh_enable";
            this.chk_equip_qh_enable.Size = new System.Drawing.Size(168, 16);
            this.chk_equip_qh_enable.TabIndex = 3;
            this.chk_equip_qh_enable.Text = "开启强化(下面功能总开关)";
            this.chk_equip_qh_enable.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage2.Controls.Add(this.label21);
            this.tabPage2.Controls.Add(this.label20);
            this.tabPage2.Controls.Add(this.num_mh_failcount);
            this.tabPage2.Controls.Add(this.combo_mh_type);
            this.tabPage2.Controls.Add(this.dg_equipMh);
            this.tabPage2.Controls.Add(this.chk_equip_mh_enable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(375, 221);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "魔化";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(284, 8);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(77, 12);
            this.label21.TabIndex = 24;
            this.label21.Text = "次休息后继续";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(212, 7);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(29, 12);
            this.label20.TabIndex = 23;
            this.label20.Text = "失败";
            // 
            // num_mh_failcount
            // 
            this.num_mh_failcount.Location = new System.Drawing.Point(245, 3);
            this.num_mh_failcount.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_mh_failcount.Name = "num_mh_failcount";
            this.num_mh_failcount.Size = new System.Drawing.Size(36, 21);
            this.num_mh_failcount.TabIndex = 22;
            this.num_mh_failcount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // combo_mh_type
            // 
            this.combo_mh_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_mh_type.FormattingEnabled = true;
            this.combo_mh_type.Items.AddRange(new object[] {
            "少量",
            "大量",
            "海量"});
            this.combo_mh_type.Location = new System.Drawing.Point(83, 3);
            this.combo_mh_type.Name = "combo_mh_type";
            this.combo_mh_type.Size = new System.Drawing.Size(71, 20);
            this.combo_mh_type.TabIndex = 20;
            // 
            // dg_equipMh
            // 
            this.dg_equipMh.AllowDrop = true;
            this.dg_equipMh.AllowUserToAddRows = false;
            this.dg_equipMh.AllowUserToDeleteRows = false;
            this.dg_equipMh.AllowUserToOrderColumns = true;
            this.dg_equipMh.AllowUserToResizeRows = false;
            this.dg_equipMh.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dg_equipMh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_equipMh.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_chk_equipMagic,
            this.col_name_equipMagic,
            this.col_level_equipMagic});
            this.dg_equipMh.Location = new System.Drawing.Point(5, 27);
            this.dg_equipMh.Name = "dg_equipMh";
            this.dg_equipMh.RowHeadersWidth = 30;
            this.dg_equipMh.RowTemplate.Height = 23;
            this.dg_equipMh.Size = new System.Drawing.Size(367, 188);
            this.dg_equipMh.TabIndex = 19;
            this.dg_equipMh.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_equipMh.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_equipMh.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_equipMh.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dg_rowPrePaint);
            this.dg_equipMh.SelectionChanged += new System.EventHandler(this.dg_SelectionChanged);
            this.dg_equipMh.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_DragDrop);
            this.dg_equipMh.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_DragEnter);
            // 
            // col_chk_equipMagic
            // 
            this.col_chk_equipMagic.DataPropertyName = "IsChecked";
            this.col_chk_equipMagic.HeaderText = "";
            this.col_chk_equipMagic.Name = "col_chk_equipMagic";
            this.col_chk_equipMagic.Width = 30;
            // 
            // col_name_equipMagic
            // 
            this.col_name_equipMagic.DataPropertyName = "EquipNameWithGeneral";
            this.col_name_equipMagic.HeaderText = "名字";
            this.col_name_equipMagic.Name = "col_name_equipMagic";
            this.col_name_equipMagic.Width = 225;
            // 
            // col_level_equipMagic
            // 
            this.col_level_equipMagic.DataPropertyName = "EnchantLevel";
            this.col_level_equipMagic.HeaderText = "魔化";
            this.col_level_equipMagic.Name = "col_level_equipMagic";
            this.col_level_equipMagic.Width = 60;
            // 
            // chk_equip_mh_enable
            // 
            this.chk_equip_mh_enable.AutoSize = true;
            this.chk_equip_mh_enable.Location = new System.Drawing.Point(5, 5);
            this.chk_equip_mh_enable.Name = "chk_equip_mh_enable";
            this.chk_equip_mh_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_equip_mh_enable.TabIndex = 5;
            this.chk_equip_mh_enable.Text = "开启魔化";
            this.chk_equip_mh_enable.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage4.Controls.Add(this.num_yupei_attr);
            this.tabPage4.Controls.Add(this.chk_auto_melt_yupei);
            this.tabPage4.Controls.Add(this.num_upgrade_crystal);
            this.tabPage4.Controls.Add(this.chk_upgrade_war_chariot);
            this.tabPage4.Controls.Add(this.chk_upgrade_crystal);
            this.tabPage4.Controls.Add(this.chk_openbox_for_yupei);
            this.tabPage4.Controls.Add(this.label22);
            this.tabPage4.Controls.Add(this.num_polish_melt_failcount);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.num_polish_item_reserve);
            this.tabPage4.Controls.Add(this.num_upgrade_gem);
            this.tabPage4.Controls.Add(this.num_polish_goon);
            this.tabPage4.Controls.Add(this.num_polish_reserve);
            this.tabPage4.Controls.Add(this.chk_upgrade_gem);
            this.tabPage4.Controls.Add(this.chk_polish_enable);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(375, 221);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "其他";
            // 
            // num_yupei_attr
            // 
            this.num_yupei_attr.Location = new System.Drawing.Point(186, 172);
            this.num_yupei_attr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_yupei_attr.Name = "num_yupei_attr";
            this.num_yupei_attr.Size = new System.Drawing.Size(46, 21);
            this.num_yupei_attr.TabIndex = 23;
            this.num_yupei_attr.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // chk_auto_melt_yupei
            // 
            this.chk_auto_melt_yupei.AutoSize = true;
            this.chk_auto_melt_yupei.Location = new System.Drawing.Point(6, 175);
            this.chk_auto_melt_yupei.Name = "chk_auto_melt_yupei";
            this.chk_auto_melt_yupei.Size = new System.Drawing.Size(180, 16);
            this.chk_auto_melt_yupei.TabIndex = 22;
            this.chk_auto_melt_yupei.Text = "自动熔化家传玉佩，若级别≤";
            this.chk_auto_melt_yupei.UseVisualStyleBackColor = true;
            // 
            // num_upgrade_crystal
            // 
            this.num_upgrade_crystal.Location = new System.Drawing.Point(179, 126);
            this.num_upgrade_crystal.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_upgrade_crystal.Name = "num_upgrade_crystal";
            this.num_upgrade_crystal.Size = new System.Drawing.Size(46, 21);
            this.num_upgrade_crystal.TabIndex = 21;
            this.num_upgrade_crystal.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // chk_upgrade_war_chariot
            // 
            this.chk_upgrade_war_chariot.AutoSize = true;
            this.chk_upgrade_war_chariot.Location = new System.Drawing.Point(6, 152);
            this.chk_upgrade_war_chariot.Name = "chk_upgrade_war_chariot";
            this.chk_upgrade_war_chariot.Size = new System.Drawing.Size(180, 16);
            this.chk_upgrade_war_chariot.TabIndex = 20;
            this.chk_upgrade_war_chariot.Text = "自动强化战车(有铁锤情况下)";
            this.chk_upgrade_war_chariot.UseVisualStyleBackColor = true;
            // 
            // chk_upgrade_crystal
            // 
            this.chk_upgrade_crystal.AutoSize = true;
            this.chk_upgrade_crystal.Location = new System.Drawing.Point(6, 129);
            this.chk_upgrade_crystal.Name = "chk_upgrade_crystal";
            this.chk_upgrade_crystal.Size = new System.Drawing.Size(168, 16);
            this.chk_upgrade_crystal.TabIndex = 19;
            this.chk_upgrade_crystal.Text = "自动进阶水晶石，若级别≥";
            this.chk_upgrade_crystal.UseVisualStyleBackColor = true;
            // 
            // chk_openbox_for_yupei
            // 
            this.chk_openbox_for_yupei.AutoSize = true;
            this.chk_openbox_for_yupei.Location = new System.Drawing.Point(5, 107);
            this.chk_openbox_for_yupei.Name = "chk_openbox_for_yupei";
            this.chk_openbox_for_yupei.Size = new System.Drawing.Size(168, 16);
            this.chk_openbox_for_yupei.TabIndex = 18;
            this.chk_openbox_for_yupei.Text = "玉佩用完自动开宝箱找玉佩";
            this.chk_openbox_for_yupei.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(19, 33);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(125, 12);
            this.label22.TabIndex = 17;
            this.label22.Text = "多少次炼化不升点融化";
            // 
            // num_polish_melt_failcount
            // 
            this.num_polish_melt_failcount.Location = new System.Drawing.Point(156, 28);
            this.num_polish_melt_failcount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_polish_melt_failcount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_polish_melt_failcount.Name = "num_polish_melt_failcount";
            this.num_polish_melt_failcount.Size = new System.Drawing.Size(46, 21);
            this.num_polish_melt_failcount.TabIndex = 16;
            this.num_polish_melt_failcount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(19, 57);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(131, 12);
            this.label16.TabIndex = 15;
            this.label16.Text = "金币炼化, 若5次属性≥";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(222, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 12);
            this.label15.TabIndex = 14;
            this.label15.Text = "仓库最多保留个数";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(78, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 13;
            this.label14.Text = "保留炼化次数";
            // 
            // num_polish_item_reserve
            // 
            this.num_polish_item_reserve.Location = new System.Drawing.Point(326, 4);
            this.num_polish_item_reserve.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_polish_item_reserve.Name = "num_polish_item_reserve";
            this.num_polish_item_reserve.Size = new System.Drawing.Size(37, 21);
            this.num_polish_item_reserve.TabIndex = 12;
            this.num_polish_item_reserve.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // num_upgrade_gem
            // 
            this.num_upgrade_gem.Location = new System.Drawing.Point(159, 81);
            this.num_upgrade_gem.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_upgrade_gem.Name = "num_upgrade_gem";
            this.num_upgrade_gem.Size = new System.Drawing.Size(46, 21);
            this.num_upgrade_gem.TabIndex = 10;
            this.num_upgrade_gem.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // num_polish_goon
            // 
            this.num_polish_goon.Location = new System.Drawing.Point(156, 53);
            this.num_polish_goon.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.num_polish_goon.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_polish_goon.Name = "num_polish_goon";
            this.num_polish_goon.Size = new System.Drawing.Size(46, 21);
            this.num_polish_goon.TabIndex = 9;
            this.num_polish_goon.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // num_polish_reserve
            // 
            this.num_polish_reserve.Location = new System.Drawing.Point(157, 5);
            this.num_polish_reserve.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_polish_reserve.Name = "num_polish_reserve";
            this.num_polish_reserve.Size = new System.Drawing.Size(46, 21);
            this.num_polish_reserve.TabIndex = 8;
            this.num_polish_reserve.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chk_upgrade_gem
            // 
            this.chk_upgrade_gem.AutoSize = true;
            this.chk_upgrade_gem.Location = new System.Drawing.Point(5, 83);
            this.chk_upgrade_gem.Name = "chk_upgrade_gem";
            this.chk_upgrade_gem.Size = new System.Drawing.Size(150, 16);
            this.chk_upgrade_gem.TabIndex = 6;
            this.chk_upgrade_gem.Text = "自动合成宝石,若级别≤";
            this.chk_upgrade_gem.UseVisualStyleBackColor = true;
            // 
            // chk_polish_enable
            // 
            this.chk_polish_enable.AutoSize = true;
            this.chk_polish_enable.Location = new System.Drawing.Point(5, 10);
            this.chk_polish_enable.Name = "chk_polish_enable";
            this.chk_polish_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_polish_enable.TabIndex = 3;
            this.chk_polish_enable.Text = "开启炼化";
            this.chk_polish_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label9);
            this.groupBox10.Controls.Add(this.num_fete_6);
            this.groupBox10.Controls.Add(this.label10);
            this.groupBox10.Controls.Add(this.num_fete_5);
            this.groupBox10.Controls.Add(this.label11);
            this.groupBox10.Controls.Add(this.num_fete_4);
            this.groupBox10.Controls.Add(this.label8);
            this.groupBox10.Controls.Add(this.num_fete_3);
            this.groupBox10.Controls.Add(this.label7);
            this.groupBox10.Controls.Add(this.num_fete_2);
            this.groupBox10.Controls.Add(this.lbl_fete_silver);
            this.groupBox10.Controls.Add(this.num_fete_1);
            this.groupBox10.Controls.Add(this.chk_fete_enable);
            this.groupBox10.Location = new System.Drawing.Point(3, 160);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(205, 125);
            this.groupBox10.TabIndex = 9;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "祭祀";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(102, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 29;
            this.label9.Text = "大祭祀<";
            // 
            // num_fete_6
            // 
            this.num_fete_6.Location = new System.Drawing.Point(157, 86);
            this.num_fete_6.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_fete_6.Name = "num_fete_6";
            this.num_fete_6.Size = new System.Drawing.Size(36, 21);
            this.num_fete_6.TabIndex = 28;
            this.tip_msg.SetToolTip(this.num_fete_6, "0为不大祭祀");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 27;
            this.label10.Text = "宝石≤";
            // 
            // num_fete_5
            // 
            this.num_fete_5.Location = new System.Drawing.Point(63, 86);
            this.num_fete_5.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_fete_5.Name = "num_fete_5";
            this.num_fete_5.Size = new System.Drawing.Size(36, 21);
            this.num_fete_5.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(113, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "玉石≤";
            // 
            // num_fete_4
            // 
            this.num_fete_4.Location = new System.Drawing.Point(157, 61);
            this.num_fete_4.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_fete_4.Name = "num_fete_4";
            this.num_fete_4.Size = new System.Drawing.Size(36, 21);
            this.num_fete_4.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "威望≤";
            // 
            // num_fete_3
            // 
            this.num_fete_3.Location = new System.Drawing.Point(63, 61);
            this.num_fete_3.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_fete_3.Name = "num_fete_3";
            this.num_fete_3.Size = new System.Drawing.Size(36, 21);
            this.num_fete_3.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(114, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "军功≤";
            // 
            // num_fete_2
            // 
            this.num_fete_2.Location = new System.Drawing.Point(158, 38);
            this.num_fete_2.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_fete_2.Name = "num_fete_2";
            this.num_fete_2.Size = new System.Drawing.Size(36, 21);
            this.num_fete_2.TabIndex = 20;
            // 
            // lbl_fete_silver
            // 
            this.lbl_fete_silver.AutoSize = true;
            this.lbl_fete_silver.Location = new System.Drawing.Point(19, 42);
            this.lbl_fete_silver.Name = "lbl_fete_silver";
            this.lbl_fete_silver.Size = new System.Drawing.Size(41, 12);
            this.lbl_fete_silver.TabIndex = 19;
            this.lbl_fete_silver.Text = "银币≤";
            // 
            // num_fete_1
            // 
            this.num_fete_1.Location = new System.Drawing.Point(63, 38);
            this.num_fete_1.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_fete_1.Name = "num_fete_1";
            this.num_fete_1.Size = new System.Drawing.Size(36, 21);
            this.num_fete_1.TabIndex = 18;
            // 
            // chk_fete_enable
            // 
            this.chk_fete_enable.AutoSize = true;
            this.chk_fete_enable.Location = new System.Drawing.Point(10, 20);
            this.chk_fete_enable.Name = "chk_fete_enable";
            this.chk_fete_enable.Size = new System.Drawing.Size(156, 16);
            this.chk_fete_enable.TabIndex = 17;
            this.chk_fete_enable.Text = "开启(输入框代表金币数)";
            this.chk_fete_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.chk_market_usetoken);
            this.groupBox9.Controls.Add(this.chk_market_super);
            this.groupBox9.Controls.Add(this.chk_market_drop_notbuy);
            this.groupBox9.Controls.Add(this.chk_market_notbuy_iffail);
            this.groupBox9.Controls.Add(this.chk_market_force_attack);
            this.groupBox9.Controls.Add(this.chk_market_purpleweapon_gold);
            this.groupBox9.Controls.Add(this.chk_market_redweapon_gold);
            this.groupBox9.Controls.Add(this.chk_market_stone_gold);
            this.groupBox9.Controls.Add(this.chk_market_silver_gold);
            this.groupBox9.Controls.Add(this.chk_market_gem_gold);
            this.groupBox9.Controls.Add(this.combo_market_purpleweapon_type);
            this.groupBox9.Controls.Add(this.combo_market_redweapon_type);
            this.groupBox9.Controls.Add(this.combo_market_stone_type);
            this.groupBox9.Controls.Add(this.combo_market_silver_type);
            this.groupBox9.Controls.Add(this.combo_market_gem_type);
            this.groupBox9.Controls.Add(this.chk_market_purpleweapon);
            this.groupBox9.Controls.Add(this.chk_market_redweapon);
            this.groupBox9.Controls.Add(this.chk_market_stone);
            this.groupBox9.Controls.Add(this.chk_market_silver);
            this.groupBox9.Controls.Add(this.chk_market_gem);
            this.groupBox9.Controls.Add(this.chk_market_enable);
            this.groupBox9.Location = new System.Drawing.Point(420, 57);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(229, 273);
            this.groupBox9.TabIndex = 8;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "集市";
            // 
            // chk_market_usetoken
            // 
            this.chk_market_usetoken.AutoSize = true;
            this.chk_market_usetoken.Location = new System.Drawing.Point(28, 223);
            this.chk_market_usetoken.Name = "chk_market_usetoken";
            this.chk_market_usetoken.Size = new System.Drawing.Size(84, 16);
            this.chk_market_usetoken.TabIndex = 38;
            this.chk_market_usetoken.Text = "使用进货令";
            this.chk_market_usetoken.UseVisualStyleBackColor = true;
            // 
            // chk_market_super
            // 
            this.chk_market_super.AutoSize = true;
            this.chk_market_super.Checked = true;
            this.chk_market_super.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_market_super.Location = new System.Drawing.Point(113, 43);
            this.chk_market_super.Name = "chk_market_super";
            this.chk_market_super.Size = new System.Drawing.Size(72, 16);
            this.chk_market_super.TabIndex = 37;
            this.chk_market_super.Text = "购买特供";
            this.chk_market_super.UseVisualStyleBackColor = true;
            // 
            // chk_market_drop_notbuy
            // 
            this.chk_market_drop_notbuy.AutoSize = true;
            this.chk_market_drop_notbuy.Location = new System.Drawing.Point(28, 201);
            this.chk_market_drop_notbuy.Name = "chk_market_drop_notbuy";
            this.chk_market_drop_notbuy.Size = new System.Drawing.Size(96, 16);
            this.chk_market_drop_notbuy.TabIndex = 36;
            this.chk_market_drop_notbuy.Text = "下架不买物品";
            this.chk_market_drop_notbuy.UseVisualStyleBackColor = true;
            // 
            // chk_market_notbuy_iffail
            // 
            this.chk_market_notbuy_iffail.AutoSize = true;
            this.chk_market_notbuy_iffail.Location = new System.Drawing.Point(28, 179);
            this.chk_market_notbuy_iffail.Name = "chk_market_notbuy_iffail";
            this.chk_market_notbuy_iffail.Size = new System.Drawing.Size(96, 16);
            this.chk_market_notbuy_iffail.TabIndex = 35;
            this.chk_market_notbuy_iffail.Text = "还价失败不买";
            this.chk_market_notbuy_iffail.UseVisualStyleBackColor = true;
            // 
            // chk_market_force_attack
            // 
            this.chk_market_force_attack.AutoSize = true;
            this.chk_market_force_attack.Location = new System.Drawing.Point(28, 44);
            this.chk_market_force_attack.Name = "chk_market_force_attack";
            this.chk_market_force_attack.Size = new System.Drawing.Size(72, 16);
            this.chk_market_force_attack.TabIndex = 34;
            this.chk_market_force_attack.Text = "买强攻令";
            this.chk_market_force_attack.UseVisualStyleBackColor = true;
            // 
            // chk_market_purpleweapon_gold
            // 
            this.chk_market_purpleweapon_gold.AutoSize = true;
            this.chk_market_purpleweapon_gold.Location = new System.Drawing.Point(177, 155);
            this.chk_market_purpleweapon_gold.Name = "chk_market_purpleweapon_gold";
            this.chk_market_purpleweapon_gold.Size = new System.Drawing.Size(48, 16);
            this.chk_market_purpleweapon_gold.TabIndex = 33;
            this.chk_market_purpleweapon_gold.Text = "金币";
            this.chk_market_purpleweapon_gold.UseVisualStyleBackColor = true;
            // 
            // chk_market_redweapon_gold
            // 
            this.chk_market_redweapon_gold.AutoSize = true;
            this.chk_market_redweapon_gold.Location = new System.Drawing.Point(177, 133);
            this.chk_market_redweapon_gold.Name = "chk_market_redweapon_gold";
            this.chk_market_redweapon_gold.Size = new System.Drawing.Size(48, 16);
            this.chk_market_redweapon_gold.TabIndex = 32;
            this.chk_market_redweapon_gold.Text = "金币";
            this.chk_market_redweapon_gold.UseVisualStyleBackColor = true;
            // 
            // chk_market_stone_gold
            // 
            this.chk_market_stone_gold.AutoSize = true;
            this.chk_market_stone_gold.Location = new System.Drawing.Point(177, 111);
            this.chk_market_stone_gold.Name = "chk_market_stone_gold";
            this.chk_market_stone_gold.Size = new System.Drawing.Size(48, 16);
            this.chk_market_stone_gold.TabIndex = 31;
            this.chk_market_stone_gold.Text = "金币";
            this.chk_market_stone_gold.UseVisualStyleBackColor = true;
            // 
            // chk_market_silver_gold
            // 
            this.chk_market_silver_gold.AutoSize = true;
            this.chk_market_silver_gold.Checked = true;
            this.chk_market_silver_gold.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_market_silver_gold.Enabled = false;
            this.chk_market_silver_gold.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.chk_market_silver_gold.Location = new System.Drawing.Point(177, 87);
            this.chk_market_silver_gold.Name = "chk_market_silver_gold";
            this.chk_market_silver_gold.Size = new System.Drawing.Size(48, 16);
            this.chk_market_silver_gold.TabIndex = 30;
            this.chk_market_silver_gold.Text = "金币";
            this.chk_market_silver_gold.UseVisualStyleBackColor = true;
            // 
            // chk_market_gem_gold
            // 
            this.chk_market_gem_gold.AutoSize = true;
            this.chk_market_gem_gold.Location = new System.Drawing.Point(176, 65);
            this.chk_market_gem_gold.Name = "chk_market_gem_gold";
            this.chk_market_gem_gold.Size = new System.Drawing.Size(48, 16);
            this.chk_market_gem_gold.TabIndex = 29;
            this.chk_market_gem_gold.Text = "金币";
            this.chk_market_gem_gold.UseVisualStyleBackColor = true;
            // 
            // combo_market_purpleweapon_type
            // 
            this.combo_market_purpleweapon_type.FormattingEnabled = true;
            this.combo_market_purpleweapon_type.Items.AddRange(new object[] {
            "白色",
            "蓝色",
            "绿色",
            "黄色"});
            this.combo_market_purpleweapon_type.Location = new System.Drawing.Point(92, 153);
            this.combo_market_purpleweapon_type.Name = "combo_market_purpleweapon_type";
            this.combo_market_purpleweapon_type.Size = new System.Drawing.Size(79, 20);
            this.combo_market_purpleweapon_type.TabIndex = 27;
            this.combo_market_purpleweapon_type.Text = "选择品质";
            // 
            // combo_market_redweapon_type
            // 
            this.combo_market_redweapon_type.FormattingEnabled = true;
            this.combo_market_redweapon_type.Items.AddRange(new object[] {
            "白色",
            "蓝色",
            "绿色",
            "黄色"});
            this.combo_market_redweapon_type.Location = new System.Drawing.Point(92, 131);
            this.combo_market_redweapon_type.Name = "combo_market_redweapon_type";
            this.combo_market_redweapon_type.Size = new System.Drawing.Size(79, 20);
            this.combo_market_redweapon_type.TabIndex = 26;
            this.combo_market_redweapon_type.Text = "选择品质";
            // 
            // combo_market_stone_type
            // 
            this.combo_market_stone_type.FormattingEnabled = true;
            this.combo_market_stone_type.Items.AddRange(new object[] {
            "白色",
            "蓝色",
            "绿色",
            "黄色"});
            this.combo_market_stone_type.Location = new System.Drawing.Point(92, 109);
            this.combo_market_stone_type.Name = "combo_market_stone_type";
            this.combo_market_stone_type.Size = new System.Drawing.Size(79, 20);
            this.combo_market_stone_type.TabIndex = 25;
            this.combo_market_stone_type.Text = "选择品质";
            // 
            // combo_market_silver_type
            // 
            this.combo_market_silver_type.FormattingEnabled = true;
            this.combo_market_silver_type.Items.AddRange(new object[] {
            "白色",
            "蓝色",
            "绿色",
            "黄色"});
            this.combo_market_silver_type.Location = new System.Drawing.Point(92, 86);
            this.combo_market_silver_type.Name = "combo_market_silver_type";
            this.combo_market_silver_type.Size = new System.Drawing.Size(79, 20);
            this.combo_market_silver_type.TabIndex = 24;
            this.combo_market_silver_type.Text = "选择品质";
            // 
            // combo_market_gem_type
            // 
            this.combo_market_gem_type.FormattingEnabled = true;
            this.combo_market_gem_type.Items.AddRange(new object[] {
            "白色",
            "蓝色",
            "绿色",
            "黄色"});
            this.combo_market_gem_type.Location = new System.Drawing.Point(92, 63);
            this.combo_market_gem_type.Name = "combo_market_gem_type";
            this.combo_market_gem_type.Size = new System.Drawing.Size(79, 20);
            this.combo_market_gem_type.TabIndex = 23;
            this.combo_market_gem_type.Text = "选择品质";
            // 
            // chk_market_purpleweapon
            // 
            this.chk_market_purpleweapon.AutoSize = true;
            this.chk_market_purpleweapon.Location = new System.Drawing.Point(28, 155);
            this.chk_market_purpleweapon.Name = "chk_market_purpleweapon";
            this.chk_market_purpleweapon.Size = new System.Drawing.Size(60, 16);
            this.chk_market_purpleweapon.TabIndex = 21;
            this.chk_market_purpleweapon.Text = "紫兵器";
            this.chk_market_purpleweapon.UseVisualStyleBackColor = true;
            // 
            // chk_market_redweapon
            // 
            this.chk_market_redweapon.AutoSize = true;
            this.chk_market_redweapon.Location = new System.Drawing.Point(28, 133);
            this.chk_market_redweapon.Name = "chk_market_redweapon";
            this.chk_market_redweapon.Size = new System.Drawing.Size(60, 16);
            this.chk_market_redweapon.TabIndex = 20;
            this.chk_market_redweapon.Text = "红兵器";
            this.chk_market_redweapon.UseVisualStyleBackColor = true;
            // 
            // chk_market_stone
            // 
            this.chk_market_stone.AutoSize = true;
            this.chk_market_stone.Location = new System.Drawing.Point(28, 111);
            this.chk_market_stone.Name = "chk_market_stone";
            this.chk_market_stone.Size = new System.Drawing.Size(48, 16);
            this.chk_market_stone.TabIndex = 19;
            this.chk_market_stone.Text = "玉石";
            this.chk_market_stone.UseVisualStyleBackColor = true;
            // 
            // chk_market_silver
            // 
            this.chk_market_silver.AutoSize = true;
            this.chk_market_silver.Location = new System.Drawing.Point(28, 89);
            this.chk_market_silver.Name = "chk_market_silver";
            this.chk_market_silver.Size = new System.Drawing.Size(48, 16);
            this.chk_market_silver.TabIndex = 18;
            this.chk_market_silver.Text = "银币";
            this.chk_market_silver.UseVisualStyleBackColor = true;
            // 
            // chk_market_gem
            // 
            this.chk_market_gem.AutoSize = true;
            this.chk_market_gem.Location = new System.Drawing.Point(28, 67);
            this.chk_market_gem.Name = "chk_market_gem";
            this.chk_market_gem.Size = new System.Drawing.Size(48, 16);
            this.chk_market_gem.TabIndex = 17;
            this.chk_market_gem.Text = "宝石";
            this.chk_market_gem.UseVisualStyleBackColor = true;
            // 
            // chk_market_enable
            // 
            this.chk_market_enable.AutoSize = true;
            this.chk_market_enable.Location = new System.Drawing.Point(7, 20);
            this.chk_market_enable.Name = "chk_market_enable";
            this.chk_market_enable.Size = new System.Drawing.Size(48, 16);
            this.chk_market_enable.TabIndex = 16;
            this.chk_market_enable.Text = "开启";
            this.chk_market_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.combo_defense_formation);
            this.groupBox8.Controls.Add(this.chk_defense_format);
            this.groupBox8.Controls.Add(this.tabControl2);
            this.groupBox8.Location = new System.Drawing.Point(420, 336);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(626, 265);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "军事管理";
            // 
            // combo_defense_formation
            // 
            this.combo_defense_formation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_defense_formation.FormattingEnabled = true;
            this.combo_defense_formation.Items.AddRange(new object[] {
            "不变阵",
            "格挡阵",
            "长蛇阵",
            "锋矢阵",
            "偃月阵",
            "锥形阵",
            "八卦阵",
            "七星阵",
            "雁行阵"});
            this.combo_defense_formation.Location = new System.Drawing.Point(92, 12);
            this.combo_defense_formation.Name = "combo_defense_formation";
            this.combo_defense_formation.Size = new System.Drawing.Size(81, 20);
            this.combo_defense_formation.TabIndex = 28;
            // 
            // chk_defense_format
            // 
            this.chk_defense_format.AutoSize = true;
            this.chk_defense_format.Location = new System.Drawing.Point(14, 16);
            this.chk_defense_format.Name = "chk_defense_format";
            this.chk_defense_format.Size = new System.Drawing.Size(72, 16);
            this.chk_defense_format.TabIndex = 13;
            this.chk_defense_format.Text = "平时阵型";
            this.chk_defense_format.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Location = new System.Drawing.Point(5, 32);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.Padding = new System.Drawing.Point(0, 0);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(618, 226);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage5.Controls.Add(this.label47);
            this.tabPage5.Controls.Add(this.chk_battle_event_stone_gold);
            this.tabPage5.Controls.Add(this.chk_battle_event_weapon_gold);
            this.tabPage5.Controls.Add(this.chk_battle_event_gem_gold);
            this.tabPage5.Controls.Add(this.chk_battle_event_weapon);
            this.tabPage5.Controls.Add(this.chk_battle_event_stone);
            this.tabPage5.Controls.Add(this.chk_battle_event_gem);
            this.tabPage5.Controls.Add(this.chk_battle_event_enabled);
            this.tabPage5.Controls.Add(this.label41);
            this.tabPage5.Controls.Add(this.btn_selForceNpc);
            this.tabPage5.Controls.Add(this.lbl_attackForceNpc);
            this.tabPage5.Controls.Add(this.btn_selNpc);
            this.tabPage5.Controls.Add(this.lbl_attackNpc);
            this.tabPage5.Controls.Add(this.num_attack_npc_reserve_order);
            this.tabPage5.Controls.Add(this.chk_attack_npc_enable);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(610, 200);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "精英";
            // 
            // label47
            // 
            this.label47.ForeColor = System.Drawing.Color.Brown;
            this.label47.Location = new System.Drawing.Point(199, 51);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(388, 59);
            this.label47.TabIndex = 62;
            this.label47.Text = "注意：使用金币选项只能选一个, 如果多选, 后面的将不起作用";
            // 
            // chk_battle_event_stone_gold
            // 
            this.chk_battle_event_stone_gold.AutoSize = true;
            this.chk_battle_event_stone_gold.Location = new System.Drawing.Point(121, 72);
            this.chk_battle_event_stone_gold.Name = "chk_battle_event_stone_gold";
            this.chk_battle_event_stone_gold.Size = new System.Drawing.Size(72, 16);
            this.chk_battle_event_stone_gold.TabIndex = 61;
            this.chk_battle_event_stone_gold.Text = "使用金币";
            this.chk_battle_event_stone_gold.UseVisualStyleBackColor = true;
            // 
            // chk_battle_event_weapon_gold
            // 
            this.chk_battle_event_weapon_gold.AutoSize = true;
            this.chk_battle_event_weapon_gold.Location = new System.Drawing.Point(121, 94);
            this.chk_battle_event_weapon_gold.Name = "chk_battle_event_weapon_gold";
            this.chk_battle_event_weapon_gold.Size = new System.Drawing.Size(72, 16);
            this.chk_battle_event_weapon_gold.TabIndex = 60;
            this.chk_battle_event_weapon_gold.Text = "使用金币";
            this.chk_battle_event_weapon_gold.UseVisualStyleBackColor = true;
            // 
            // chk_battle_event_gem_gold
            // 
            this.chk_battle_event_gem_gold.AutoSize = true;
            this.chk_battle_event_gem_gold.Location = new System.Drawing.Point(121, 50);
            this.chk_battle_event_gem_gold.Name = "chk_battle_event_gem_gold";
            this.chk_battle_event_gem_gold.Size = new System.Drawing.Size(72, 16);
            this.chk_battle_event_gem_gold.TabIndex = 59;
            this.chk_battle_event_gem_gold.Text = "使用金币";
            this.chk_battle_event_gem_gold.UseVisualStyleBackColor = true;
            // 
            // chk_battle_event_weapon
            // 
            this.chk_battle_event_weapon.AutoSize = true;
            this.chk_battle_event_weapon.Location = new System.Drawing.Point(41, 94);
            this.chk_battle_event_weapon.Name = "chk_battle_event_weapon";
            this.chk_battle_event_weapon.Size = new System.Drawing.Size(72, 16);
            this.chk_battle_event_weapon.TabIndex = 58;
            this.chk_battle_event_weapon.Text = "兵器事件";
            this.chk_battle_event_weapon.UseVisualStyleBackColor = true;
            // 
            // chk_battle_event_stone
            // 
            this.chk_battle_event_stone.AutoSize = true;
            this.chk_battle_event_stone.Location = new System.Drawing.Point(41, 72);
            this.chk_battle_event_stone.Name = "chk_battle_event_stone";
            this.chk_battle_event_stone.Size = new System.Drawing.Size(72, 16);
            this.chk_battle_event_stone.TabIndex = 57;
            this.chk_battle_event_stone.Text = "玉石事件";
            this.chk_battle_event_stone.UseVisualStyleBackColor = true;
            // 
            // chk_battle_event_gem
            // 
            this.chk_battle_event_gem.AutoSize = true;
            this.chk_battle_event_gem.Location = new System.Drawing.Point(41, 50);
            this.chk_battle_event_gem.Name = "chk_battle_event_gem";
            this.chk_battle_event_gem.Size = new System.Drawing.Size(72, 16);
            this.chk_battle_event_gem.TabIndex = 56;
            this.chk_battle_event_gem.Text = "宝石事件";
            this.chk_battle_event_gem.UseVisualStyleBackColor = true;
            // 
            // chk_battle_event_enabled
            // 
            this.chk_battle_event_enabled.AutoSize = true;
            this.chk_battle_event_enabled.Location = new System.Drawing.Point(20, 28);
            this.chk_battle_event_enabled.Name = "chk_battle_event_enabled";
            this.chk_battle_event_enabled.Size = new System.Drawing.Size(96, 16);
            this.chk_battle_event_enabled.TabIndex = 54;
            this.chk_battle_event_enabled.Text = "启用征战事件";
            this.chk_battle_event_enabled.UseVisualStyleBackColor = true;
            // 
            // label41
            // 
            this.label41.ForeColor = System.Drawing.Color.Brown;
            this.label41.Location = new System.Drawing.Point(199, 6);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(388, 20);
            this.label41.TabIndex = 53;
            this.label41.Text = "注意：有征战事件会忽略此设置, 为保证征战事件, 请至少保留10+军令";
            // 
            // btn_selForceNpc
            // 
            this.btn_selForceNpc.Location = new System.Drawing.Point(19, 156);
            this.btn_selForceNpc.Name = "btn_selForceNpc";
            this.btn_selForceNpc.Size = new System.Drawing.Size(85, 23);
            this.btn_selForceNpc.TabIndex = 24;
            this.btn_selForceNpc.Text = "选择强攻NPC";
            this.btn_selForceNpc.UseVisualStyleBackColor = true;
            this.btn_selForceNpc.Click += new System.EventHandler(this.btn_selForceNpc_Click);
            // 
            // lbl_attackForceNpc
            // 
            this.lbl_attackForceNpc.AutoSize = true;
            this.lbl_attackForceNpc.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_attackForceNpc.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbl_attackForceNpc.Location = new System.Drawing.Point(116, 160);
            this.lbl_attackForceNpc.Name = "lbl_attackForceNpc";
            this.lbl_attackForceNpc.Size = new System.Drawing.Size(23, 15);
            this.lbl_attackForceNpc.TabIndex = 23;
            this.lbl_attackForceNpc.Text = "无";
            // 
            // btn_selNpc
            // 
            this.btn_selNpc.Location = new System.Drawing.Point(19, 127);
            this.btn_selNpc.Name = "btn_selNpc";
            this.btn_selNpc.Size = new System.Drawing.Size(85, 23);
            this.btn_selNpc.TabIndex = 22;
            this.btn_selNpc.Text = "选择NPC";
            this.btn_selNpc.UseVisualStyleBackColor = true;
            this.btn_selNpc.Click += new System.EventHandler(this.btn_selNpc_Click);
            // 
            // lbl_attackNpc
            // 
            this.lbl_attackNpc.AutoSize = true;
            this.lbl_attackNpc.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_attackNpc.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbl_attackNpc.Location = new System.Drawing.Point(116, 131);
            this.lbl_attackNpc.Name = "lbl_attackNpc";
            this.lbl_attackNpc.Size = new System.Drawing.Size(23, 15);
            this.lbl_attackNpc.TabIndex = 21;
            this.lbl_attackNpc.Text = "无";
            // 
            // num_attack_npc_reserve_order
            // 
            this.num_attack_npc_reserve_order.Location = new System.Drawing.Point(114, 2);
            this.num_attack_npc_reserve_order.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_attack_npc_reserve_order.Name = "num_attack_npc_reserve_order";
            this.num_attack_npc_reserve_order.Size = new System.Drawing.Size(79, 21);
            this.num_attack_npc_reserve_order.TabIndex = 20;
            // 
            // chk_attack_npc_enable
            // 
            this.chk_attack_npc_enable.AutoSize = true;
            this.chk_attack_npc_enable.Location = new System.Drawing.Point(5, 5);
            this.chk_attack_npc_enable.Name = "chk_attack_npc_enable";
            this.chk_attack_npc_enable.Size = new System.Drawing.Size(108, 16);
            this.chk_attack_npc_enable.TabIndex = 3;
            this.chk_attack_npc_enable.Text = "开启, 军令大于";
            this.chk_attack_npc_enable.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage6.Controls.Add(this.chk_attack_army_first);
            this.tabPage6.Controls.Add(this.chk_attack_army_only_jingyingtime);
            this.tabPage6.Controls.Add(this.num_attack_army_first_interval);
            this.tabPage6.Controls.Add(this.num_attack_army_reserve_order);
            this.tabPage6.Controls.Add(this.dg_attack_army);
            this.tabPage6.Controls.Add(this.chk_attack_army);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(610, 200);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "军团";
            // 
            // chk_attack_army_first
            // 
            this.chk_attack_army_first.AutoSize = true;
            this.chk_attack_army_first.Checked = true;
            this.chk_attack_army_first.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_attack_army_first.Location = new System.Drawing.Point(182, 5);
            this.chk_attack_army_first.Name = "chk_attack_army_first";
            this.chk_attack_army_first.Size = new System.Drawing.Size(204, 16);
            this.chk_attack_army_first.TabIndex = 26;
            this.chk_attack_army_first.Text = "打军团首次, 扫描时间间隔多少秒";
            this.chk_attack_army_first.UseVisualStyleBackColor = true;
            // 
            // chk_attack_army_only_jingyingtime
            // 
            this.chk_attack_army_only_jingyingtime.AutoSize = true;
            this.chk_attack_army_only_jingyingtime.Checked = true;
            this.chk_attack_army_only_jingyingtime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_attack_army_only_jingyingtime.Location = new System.Drawing.Point(467, 5);
            this.chk_attack_army_only_jingyingtime.Name = "chk_attack_army_only_jingyingtime";
            this.chk_attack_army_only_jingyingtime.Size = new System.Drawing.Size(132, 16);
            this.chk_attack_army_only_jingyingtime.TabIndex = 25;
            this.chk_attack_army_only_jingyingtime.Text = "仅精英时间打非首次";
            this.chk_attack_army_only_jingyingtime.UseVisualStyleBackColor = true;
            // 
            // num_attack_army_first_interval
            // 
            this.num_attack_army_first_interval.Location = new System.Drawing.Point(389, 3);
            this.num_attack_army_first_interval.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.num_attack_army_first_interval.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.num_attack_army_first_interval.Name = "num_attack_army_first_interval";
            this.num_attack_army_first_interval.Size = new System.Drawing.Size(67, 21);
            this.num_attack_army_first_interval.TabIndex = 24;
            this.num_attack_army_first_interval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // num_attack_army_reserve_order
            // 
            this.num_attack_army_reserve_order.Location = new System.Drawing.Point(115, 2);
            this.num_attack_army_reserve_order.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.num_attack_army_reserve_order.Name = "num_attack_army_reserve_order";
            this.num_attack_army_reserve_order.Size = new System.Drawing.Size(53, 21);
            this.num_attack_army_reserve_order.TabIndex = 22;
            // 
            // dg_attack_army
            // 
            this.dg_attack_army.AllowDrop = true;
            this.dg_attack_army.AllowUserToAddRows = false;
            this.dg_attack_army.AllowUserToDeleteRows = false;
            this.dg_attack_army.AllowUserToOrderColumns = true;
            this.dg_attack_army.AllowUserToResizeRows = false;
            this.dg_attack_army.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dg_attack_army.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_attack_army.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn2,
            this.column_firstbattle,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dg_attack_army.Location = new System.Drawing.Point(5, 27);
            this.dg_attack_army.Name = "dg_attack_army";
            this.dg_attack_army.RowHeadersWidth = 30;
            this.dg_attack_army.RowTemplate.Height = 23;
            this.dg_attack_army.Size = new System.Drawing.Size(602, 161);
            this.dg_attack_army.TabIndex = 19;
            this.dg_attack_army.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_attack_army.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_attack_army.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_attack_army.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dg_rowPrePaint);
            this.dg_attack_army.SelectionChanged += new System.EventHandler(this.dg_SelectionChanged);
            this.dg_attack_army.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_DragDrop);
            this.dg_attack_army.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_DragEnter);
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "IsChecked";
            this.dataGridViewCheckBoxColumn2.HeaderText = "";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Width = 30;
            // 
            // column_firstbattle
            // 
            this.column_firstbattle.DataPropertyName = "IsChecked2";
            this.column_firstbattle.HeaderText = "只首次";
            this.column_firstbattle.Name = "column_firstbattle";
            this.column_firstbattle.ToolTipText = "选了之后只打首次";
            this.column_firstbattle.Width = 50;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn3.HeaderText = "名字";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 180;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ItemName";
            this.dataGridViewTextBoxColumn4.HeaderText = "装备";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 220;
            // 
            // chk_attack_army
            // 
            this.chk_attack_army.AutoSize = true;
            this.chk_attack_army.Location = new System.Drawing.Point(5, 5);
            this.chk_attack_army.Name = "chk_attack_army";
            this.chk_attack_army.Size = new System.Drawing.Size(108, 16);
            this.chk_attack_army.TabIndex = 5;
            this.chk_attack_army.Text = "开启, 军令大于";
            this.chk_attack_army.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage7.Controls.Add(this.chk_juedou);
            this.tabPage7.Controls.Add(this.label67);
            this.tabPage7.Controls.Add(this.nUD_reserved_num);
            this.tabPage7.Controls.Add(this.label66);
            this.tabPage7.Controls.Add(this.chk_Nation);
            this.tabPage7.Controls.Add(this.txt_attack_filter_content);
            this.tabPage7.Controls.Add(this.combo_attack_filter_type);
            this.tabPage7.Controls.Add(this.label48);
            this.tabPage7.Controls.Add(this.num_attack_cityevent_maxstar);
            this.tabPage7.Controls.Add(this.label31);
            this.tabPage7.Controls.Add(this.label28);
            this.tabPage7.Controls.Add(this.num_attack_reserve_token);
            this.tabPage7.Controls.Add(this.chk_attack_reserve_token);
            this.tabPage7.Controls.Add(this.chk_attack_enable_attack);
            this.tabPage7.Controls.Add(this.chk_attack_get_extra_order);
            this.tabPage7.Controls.Add(this.chk_attack_jail_tech);
            this.tabPage7.Controls.Add(this.chk_attack_player_gongjian);
            this.tabPage7.Controls.Add(this.chk_attack_player_cityevent);
            this.tabPage7.Controls.Add(this.lbl_attack_target);
            this.tabPage7.Controls.Add(this.btn_attack_target);
            this.tabPage7.Controls.Add(this.label24);
            this.tabPage7.Controls.Add(this.num_attack_score);
            this.tabPage7.Controls.Add(this.chk_attack_not_injail);
            this.tabPage7.Controls.Add(this.chk_attack_npc_enemy);
            this.tabPage7.Controls.Add(this.label5);
            this.tabPage7.Controls.Add(this.label4);
            this.tabPage7.Controls.Add(this.num_attack_player_level_max);
            this.tabPage7.Controls.Add(this.num_attack_player_level_min);
            this.tabPage7.Controls.Add(this.chk_attack_player_move_tokenfull);
            this.tabPage7.Controls.Add(this.chk_attack_player_use_token);
            this.tabPage7.Controls.Add(this.chk_attack_player);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(610, 200);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "敌国";
            // 
            // chk_juedou
            // 
            this.chk_juedou.AutoSize = true;
            this.chk_juedou.Location = new System.Drawing.Point(525, 31);
            this.chk_juedou.Name = "chk_juedou";
            this.chk_juedou.Size = new System.Drawing.Size(72, 16);
            this.chk_juedou.TabIndex = 61;
            this.chk_juedou.Text = "自动决斗";
            this.chk_juedou.UseVisualStyleBackColor = true;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(585, 9);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(17, 12);
            this.label67.TabIndex = 60;
            this.label67.Text = "次";
            // 
            // nUD_reserved_num
            // 
            this.nUD_reserved_num.Location = new System.Drawing.Point(548, 3);
            this.nUD_reserved_num.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nUD_reserved_num.Name = "nUD_reserved_num";
            this.nUD_reserved_num.Size = new System.Drawing.Size(31, 21);
            this.nUD_reserved_num.TabIndex = 59;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(522, 9);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(29, 12);
            this.label66.TabIndex = 58;
            this.label66.Text = "保留";
            // 
            // chk_Nation
            // 
            this.chk_Nation.AutoSize = true;
            this.chk_Nation.Location = new System.Drawing.Point(424, 31);
            this.chk_Nation.Name = "chk_Nation";
            this.chk_Nation.Size = new System.Drawing.Size(96, 16);
            this.chk_Nation.TabIndex = 57;
            this.chk_Nation.Text = "自动天降奇兵";
            this.chk_Nation.UseVisualStyleBackColor = true;
            // 
            // txt_attack_filter_content
            // 
            this.txt_attack_filter_content.Location = new System.Drawing.Point(219, 135);
            this.txt_attack_filter_content.Name = "txt_attack_filter_content";
            this.txt_attack_filter_content.Size = new System.Drawing.Size(75, 21);
            this.txt_attack_filter_content.TabIndex = 56;
            this.tip_msg.SetToolTip(this.txt_attack_filter_content, "2=2星,3=3星,哪项不要就不要填, 兵器和1星不要钱,自动开");
            // 
            // combo_attack_filter_type
            // 
            this.combo_attack_filter_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_attack_filter_type.FormattingEnabled = true;
            this.combo_attack_filter_type.Items.AddRange(new object[] {
            "不定向",
            "国家",
            "个人"});
            this.combo_attack_filter_type.Location = new System.Drawing.Point(155, 136);
            this.combo_attack_filter_type.Name = "combo_attack_filter_type";
            this.combo_attack_filter_type.Size = new System.Drawing.Size(58, 20);
            this.combo_attack_filter_type.TabIndex = 55;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(86, 142);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(65, 12);
            this.label48.TabIndex = 54;
            this.label48.Text = "定向攻击：";
            // 
            // num_attack_cityevent_maxstar
            // 
            this.num_attack_cityevent_maxstar.Location = new System.Drawing.Point(475, 3);
            this.num_attack_cityevent_maxstar.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_attack_cityevent_maxstar.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_attack_cityevent_maxstar.Name = "num_attack_cityevent_maxstar";
            this.num_attack_cityevent_maxstar.Size = new System.Drawing.Size(46, 21);
            this.num_attack_cityevent_maxstar.TabIndex = 53;
            this.num_attack_cityevent_maxstar.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label31
            // 
            this.label31.ForeColor = System.Drawing.Color.Brown;
            this.label31.Location = new System.Drawing.Point(67, 53);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(499, 28);
            this.label31.TabIndex = 52;
            this.label31.Text = "注意：选中自动攻坚战必须选择自动移动, 否则不能移动自然无法攻坚, 另外, 悬赏事件如果不能打过80%以上的同级玩家, 最好选择低星级, 否则完成不了";
            // 
            // label28
            // 
            this.label28.ForeColor = System.Drawing.Color.Brown;
            this.label28.Location = new System.Drawing.Point(69, 159);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(535, 34);
            this.label28.TabIndex = 51;
            this.label28.Text = "注意：战绩值不达到设定值之前会忽略保留军令数量, 另外, 级别必须选择, 否则找不到0级的敌人, 会不打人只打NPC, 定向攻击中, 类型选国家的话后面写国家(如" +
                "魏), 如果选个人后面写玩家名字";
            // 
            // num_attack_reserve_token
            // 
            this.num_attack_reserve_token.Location = new System.Drawing.Point(145, 97);
            this.num_attack_reserve_token.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_attack_reserve_token.Name = "num_attack_reserve_token";
            this.num_attack_reserve_token.Size = new System.Drawing.Size(60, 21);
            this.num_attack_reserve_token.TabIndex = 50;
            // 
            // chk_attack_reserve_token
            // 
            this.chk_attack_reserve_token.AutoSize = true;
            this.chk_attack_reserve_token.Location = new System.Drawing.Point(69, 99);
            this.chk_attack_reserve_token.Name = "chk_attack_reserve_token";
            this.chk_attack_reserve_token.Size = new System.Drawing.Size(72, 16);
            this.chk_attack_reserve_token.TabIndex = 49;
            this.chk_attack_reserve_token.Text = "保留军令";
            this.chk_attack_reserve_token.UseVisualStyleBackColor = true;
            // 
            // chk_attack_enable_attack
            // 
            this.chk_attack_enable_attack.AutoSize = true;
            this.chk_attack_enable_attack.Location = new System.Drawing.Point(21, 82);
            this.chk_attack_enable_attack.Name = "chk_attack_enable_attack";
            this.chk_attack_enable_attack.Size = new System.Drawing.Size(96, 16);
            this.chk_attack_enable_attack.TabIndex = 48;
            this.chk_attack_enable_attack.Text = "开启攻击玩家";
            this.chk_attack_enable_attack.UseVisualStyleBackColor = true;
            // 
            // chk_attack_get_extra_order
            // 
            this.chk_attack_get_extra_order.AutoSize = true;
            this.chk_attack_get_extra_order.Location = new System.Drawing.Point(182, 6);
            this.chk_attack_get_extra_order.Name = "chk_attack_get_extra_order";
            this.chk_attack_get_extra_order.Size = new System.Drawing.Size(108, 16);
            this.chk_attack_get_extra_order.TabIndex = 47;
            this.chk_attack_get_extra_order.Text = "领取马车攻击令";
            this.chk_attack_get_extra_order.UseVisualStyleBackColor = true;
            // 
            // chk_attack_jail_tech
            // 
            this.chk_attack_jail_tech.AutoSize = true;
            this.chk_attack_jail_tech.Location = new System.Drawing.Point(69, 6);
            this.chk_attack_jail_tech.Name = "chk_attack_jail_tech";
            this.chk_attack_jail_tech.Size = new System.Drawing.Size(96, 16);
            this.chk_attack_jail_tech.TabIndex = 46;
            this.chk_attack_jail_tech.Text = "劳作技术研究";
            this.chk_attack_jail_tech.UseVisualStyleBackColor = true;
            // 
            // chk_attack_player_gongjian
            // 
            this.chk_attack_player_gongjian.AutoSize = true;
            this.chk_attack_player_gongjian.Location = new System.Drawing.Point(182, 31);
            this.chk_attack_player_gongjian.Name = "chk_attack_player_gongjian";
            this.chk_attack_player_gongjian.Size = new System.Drawing.Size(84, 16);
            this.chk_attack_player_gongjian.TabIndex = 24;
            this.chk_attack_player_gongjian.Text = "自动攻坚战";
            this.tip_msg.SetToolTip(this.chk_attack_player_gongjian, "攻坚战期间将忽略移动目标城市");
            this.chk_attack_player_gongjian.UseVisualStyleBackColor = true;
            // 
            // chk_attack_player_cityevent
            // 
            this.chk_attack_player_cityevent.AutoSize = true;
            this.chk_attack_player_cityevent.Location = new System.Drawing.Point(299, 6);
            this.chk_attack_player_cityevent.Name = "chk_attack_player_cityevent";
            this.chk_attack_player_cityevent.Size = new System.Drawing.Size(180, 16);
            this.chk_attack_player_cityevent.TabIndex = 23;
            this.chk_attack_player_cityevent.Text = "自动悬赏事件, 最高选择星级";
            this.chk_attack_player_cityevent.UseVisualStyleBackColor = true;
            // 
            // lbl_attack_target
            // 
            this.lbl_attack_target.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_attack_target.ForeColor = System.Drawing.Color.IndianRed;
            this.lbl_attack_target.Location = new System.Drawing.Point(421, 29);
            this.lbl_attack_target.Name = "lbl_attack_target";
            this.lbl_attack_target.Size = new System.Drawing.Size(100, 20);
            this.lbl_attack_target.TabIndex = 22;
            // 
            // btn_attack_target
            // 
            this.btn_attack_target.Location = new System.Drawing.Point(299, 28);
            this.btn_attack_target.Name = "btn_attack_target";
            this.btn_attack_target.Size = new System.Drawing.Size(116, 23);
            this.btn_attack_target.TabIndex = 21;
            this.btn_attack_target.Text = "选择移动目标";
            this.btn_attack_target.UseVisualStyleBackColor = true;
            this.btn_attack_target.Click += new System.EventHandler(this.btn_attack_target_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(385, 101);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(113, 12);
            this.label24.TabIndex = 20;
            this.label24.Text = "多少战绩停止攻击：";
            // 
            // num_attack_score
            // 
            this.num_attack_score.Location = new System.Drawing.Point(504, 97);
            this.num_attack_score.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.num_attack_score.Name = "num_attack_score";
            this.num_attack_score.Size = new System.Drawing.Size(84, 21);
            this.num_attack_score.TabIndex = 19;
            this.num_attack_score.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // chk_attack_not_injail
            // 
            this.chk_attack_not_injail.AutoSize = true;
            this.chk_attack_not_injail.Location = new System.Drawing.Point(254, 121);
            this.chk_attack_not_injail.Name = "chk_attack_not_injail";
            this.chk_attack_not_injail.Size = new System.Drawing.Size(84, 16);
            this.chk_attack_not_injail.TabIndex = 18;
            this.chk_attack_not_injail.Text = "不打被抓的";
            this.chk_attack_not_injail.UseVisualStyleBackColor = true;
            // 
            // chk_attack_npc_enemy
            // 
            this.chk_attack_npc_enemy.AutoSize = true;
            this.chk_attack_npc_enemy.Location = new System.Drawing.Point(155, 121);
            this.chk_attack_npc_enemy.Name = "chk_attack_npc_enemy";
            this.chk_attack_npc_enemy.Size = new System.Drawing.Size(84, 16);
            this.chk_attack_npc_enemy.TabIndex = 17;
            this.chk_attack_npc_enemy.Text = "攻打守备军";
            this.chk_attack_npc_enemy.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(316, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "级别：";
            // 
            // num_attack_player_level_max
            // 
            this.num_attack_player_level_max.Location = new System.Drawing.Point(329, 97);
            this.num_attack_player_level_max.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_attack_player_level_max.Name = "num_attack_player_level_max";
            this.num_attack_player_level_max.Size = new System.Drawing.Size(46, 21);
            this.num_attack_player_level_max.TabIndex = 11;
            // 
            // num_attack_player_level_min
            // 
            this.num_attack_player_level_min.Location = new System.Drawing.Point(267, 97);
            this.num_attack_player_level_min.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_attack_player_level_min.Name = "num_attack_player_level_min";
            this.num_attack_player_level_min.Size = new System.Drawing.Size(46, 21);
            this.num_attack_player_level_min.TabIndex = 10;
            // 
            // chk_attack_player_move_tokenfull
            // 
            this.chk_attack_player_move_tokenfull.AutoSize = true;
            this.chk_attack_player_move_tokenfull.Location = new System.Drawing.Point(69, 33);
            this.chk_attack_player_move_tokenfull.Name = "chk_attack_player_move_tokenfull";
            this.chk_attack_player_move_tokenfull.Size = new System.Drawing.Size(72, 16);
            this.chk_attack_player_move_tokenfull.TabIndex = 9;
            this.chk_attack_player_move_tokenfull.Text = "自动移动";
            this.chk_attack_player_move_tokenfull.UseVisualStyleBackColor = true;
            // 
            // chk_attack_player_use_token
            // 
            this.chk_attack_player_use_token.AutoSize = true;
            this.chk_attack_player_use_token.Location = new System.Drawing.Point(69, 121);
            this.chk_attack_player_use_token.Name = "chk_attack_player_use_token";
            this.chk_attack_player_use_token.Size = new System.Drawing.Size(72, 16);
            this.chk_attack_player_use_token.TabIndex = 7;
            this.chk_attack_player_use_token.Text = "使用令牌";
            this.chk_attack_player_use_token.UseVisualStyleBackColor = true;
            // 
            // chk_attack_player
            // 
            this.chk_attack_player.AutoSize = true;
            this.chk_attack_player.Location = new System.Drawing.Point(6, 6);
            this.chk_attack_player.Name = "chk_attack_player";
            this.chk_attack_player.Size = new System.Drawing.Size(48, 16);
            this.chk_attack_player.TabIndex = 6;
            this.chk_attack_player.Text = "开启";
            this.chk_attack_player.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tabPage8.Controls.Add(this.lbl_attack_resCampaign);
            this.tabPage8.Controls.Add(this.btn_sel_resCampaign);
            this.tabPage8.Controls.Add(this.chk_res_campaign_enable);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(610, 200);
            this.tabPage8.TabIndex = 3;
            this.tabPage8.Text = "其他";
            // 
            // lbl_attack_resCampaign
            // 
            this.lbl_attack_resCampaign.AutoSize = true;
            this.lbl_attack_resCampaign.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_attack_resCampaign.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbl_attack_resCampaign.Location = new System.Drawing.Point(119, 32);
            this.lbl_attack_resCampaign.Name = "lbl_attack_resCampaign";
            this.lbl_attack_resCampaign.Size = new System.Drawing.Size(23, 15);
            this.lbl_attack_resCampaign.TabIndex = 24;
            this.lbl_attack_resCampaign.Text = "无";
            // 
            // btn_sel_resCampaign
            // 
            this.btn_sel_resCampaign.Location = new System.Drawing.Point(24, 28);
            this.btn_sel_resCampaign.Name = "btn_sel_resCampaign";
            this.btn_sel_resCampaign.Size = new System.Drawing.Size(85, 23);
            this.btn_sel_resCampaign.TabIndex = 23;
            this.btn_sel_resCampaign.Text = "选择副本";
            this.btn_sel_resCampaign.UseVisualStyleBackColor = true;
            this.btn_sel_resCampaign.Click += new System.EventHandler(this.btn_sel_resCampaign_Click);
            // 
            // chk_res_campaign_enable
            // 
            this.chk_res_campaign_enable.AutoSize = true;
            this.chk_res_campaign_enable.Location = new System.Drawing.Point(6, 8);
            this.chk_res_campaign_enable.Name = "chk_res_campaign_enable";
            this.chk_res_campaign_enable.Size = new System.Drawing.Size(156, 16);
            this.chk_res_campaign_enable.TabIndex = 19;
            this.chk_res_campaign_enable.Text = "自动重置并攻打资源副本";
            this.chk_res_campaign_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chk_hero_giftevent);
            this.groupBox6.Controls.Add(this.chk_hero_nocd);
            this.groupBox6.Controls.Add(this.dg_heros);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.chk_hero_wash_zhi);
            this.groupBox6.Controls.Add(this.chk_hero_wash_yong);
            this.groupBox6.Controls.Add(this.chk_hero_wash_tong);
            this.groupBox6.Controls.Add(this.combo_hero_wash);
            this.groupBox6.Controls.Add(this.chk_hero_wash_enable);
            this.groupBox6.Controls.Add(this.chk_hero_tufei_enable);
            this.groupBox6.Location = new System.Drawing.Point(214, 292);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 309);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "武将管理";
            // 
            // chk_hero_giftevent
            // 
            this.chk_hero_giftevent.AutoSize = true;
            this.chk_hero_giftevent.Location = new System.Drawing.Point(77, 19);
            this.chk_hero_giftevent.Name = "chk_hero_giftevent";
            this.chk_hero_giftevent.Size = new System.Drawing.Size(120, 16);
            this.chk_hero_giftevent.TabIndex = 19;
            this.chk_hero_giftevent.Text = "犒赏活动自动突飞";
            this.chk_hero_giftevent.UseVisualStyleBackColor = true;
            // 
            // chk_hero_nocd
            // 
            this.chk_hero_nocd.AutoSize = true;
            this.chk_hero_nocd.Location = new System.Drawing.Point(6, 39);
            this.chk_hero_nocd.Name = "chk_hero_nocd";
            this.chk_hero_nocd.Size = new System.Drawing.Size(48, 16);
            this.chk_hero_nocd.TabIndex = 18;
            this.chk_hero_nocd.Text = "秒CD";
            this.chk_hero_nocd.UseVisualStyleBackColor = true;
            // 
            // dg_heros
            // 
            this.dg_heros.AllowDrop = true;
            this.dg_heros.AllowUserToAddRows = false;
            this.dg_heros.AllowUserToDeleteRows = false;
            this.dg_heros.AllowUserToOrderColumns = true;
            this.dg_heros.AllowUserToResizeRows = false;
            this.dg_heros.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dg_heros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_heros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_chk_hero,
            this.col_name_hero,
            this.col_level_hero});
            this.dg_heros.Location = new System.Drawing.Point(6, 57);
            this.dg_heros.Name = "dg_heros";
            this.dg_heros.RowHeadersWidth = 30;
            this.dg_heros.RowTemplate.Height = 23;
            this.dg_heros.Size = new System.Drawing.Size(190, 170);
            this.dg_heros.TabIndex = 17;
            this.dg_heros.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_heros.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_heros.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_heros.SelectionChanged += new System.EventHandler(this.dg_SelectionChanged);
            this.dg_heros.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_DragDrop);
            this.dg_heros.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_DragEnter);
            // 
            // col_chk_hero
            // 
            this.col_chk_hero.DataPropertyName = "IsChecked";
            this.col_chk_hero.HeaderText = "";
            this.col_chk_hero.Name = "col_chk_hero";
            this.col_chk_hero.Width = 30;
            // 
            // col_name_hero
            // 
            this.col_name_hero.DataPropertyName = "Name";
            this.col_name_hero.HeaderText = "武将";
            this.col_name_hero.Name = "col_name_hero";
            this.col_name_hero.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_name_hero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_name_hero.Width = 55;
            // 
            // col_level_hero
            // 
            this.col_level_hero.DataPropertyName = "Level";
            this.col_level_hero.HeaderText = "级别";
            this.col_level_hero.Name = "col_level_hero";
            this.col_level_hero.Width = 55;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "洗什么：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "洗哪位：";
            // 
            // chk_hero_wash_zhi
            // 
            this.chk_hero_wash_zhi.AutoSize = true;
            this.chk_hero_wash_zhi.Location = new System.Drawing.Point(155, 277);
            this.chk_hero_wash_zhi.Name = "chk_hero_wash_zhi";
            this.chk_hero_wash_zhi.Size = new System.Drawing.Size(36, 16);
            this.chk_hero_wash_zhi.TabIndex = 14;
            this.chk_hero_wash_zhi.Text = "智";
            this.chk_hero_wash_zhi.UseVisualStyleBackColor = true;
            // 
            // chk_hero_wash_yong
            // 
            this.chk_hero_wash_yong.AutoSize = true;
            this.chk_hero_wash_yong.Location = new System.Drawing.Point(113, 277);
            this.chk_hero_wash_yong.Name = "chk_hero_wash_yong";
            this.chk_hero_wash_yong.Size = new System.Drawing.Size(36, 16);
            this.chk_hero_wash_yong.TabIndex = 13;
            this.chk_hero_wash_yong.Text = "勇";
            this.chk_hero_wash_yong.UseVisualStyleBackColor = true;
            // 
            // chk_hero_wash_tong
            // 
            this.chk_hero_wash_tong.AutoSize = true;
            this.chk_hero_wash_tong.Location = new System.Drawing.Point(71, 277);
            this.chk_hero_wash_tong.Name = "chk_hero_wash_tong";
            this.chk_hero_wash_tong.Size = new System.Drawing.Size(36, 16);
            this.chk_hero_wash_tong.TabIndex = 12;
            this.chk_hero_wash_tong.Text = "统";
            this.chk_hero_wash_tong.UseVisualStyleBackColor = true;
            // 
            // combo_hero_wash
            // 
            this.combo_hero_wash.DisplayMember = "Desc";
            this.combo_hero_wash.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_hero_wash.FormattingEnabled = true;
            this.combo_hero_wash.Location = new System.Drawing.Point(70, 251);
            this.combo_hero_wash.Name = "combo_hero_wash";
            this.combo_hero_wash.Size = new System.Drawing.Size(121, 20);
            this.combo_hero_wash.TabIndex = 6;
            this.combo_hero_wash.ValueMember = "Id";
            // 
            // chk_hero_wash_enable
            // 
            this.chk_hero_wash_enable.AutoSize = true;
            this.chk_hero_wash_enable.Location = new System.Drawing.Point(7, 232);
            this.chk_hero_wash_enable.Name = "chk_hero_wash_enable";
            this.chk_hero_wash_enable.Size = new System.Drawing.Size(192, 16);
            this.chk_hero_wash_enable.TabIndex = 4;
            this.chk_hero_wash_enable.Text = "免费洗属性(军功洗在临时任务)";
            this.chk_hero_wash_enable.UseVisualStyleBackColor = true;
            // 
            // chk_hero_tufei_enable
            // 
            this.chk_hero_tufei_enable.AutoSize = true;
            this.chk_hero_tufei_enable.Location = new System.Drawing.Point(6, 20);
            this.chk_hero_tufei_enable.Name = "chk_hero_tufei_enable";
            this.chk_hero_tufei_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_hero_tufei_enable.TabIndex = 2;
            this.chk_hero_tufei_enable.Text = "开启突飞";
            this.chk_hero_tufei_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chk_building_protect);
            this.groupBox5.Controls.Add(this.chk_selAll_building);
            this.groupBox5.Controls.Add(this.dg_buildings);
            this.groupBox5.Controls.Add(this.chk_building_enable);
            this.groupBox5.Location = new System.Drawing.Point(8, 292);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 309);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "升级建筑";
            // 
            // chk_building_protect
            // 
            this.chk_building_protect.AutoSize = true;
            this.chk_building_protect.Location = new System.Drawing.Point(7, 39);
            this.chk_building_protect.Name = "chk_building_protect";
            this.chk_building_protect.Size = new System.Drawing.Size(168, 16);
            this.chk_building_protect.TabIndex = 5;
            this.chk_building_protect.Text = "民居比内城其他建筑高一级";
            this.chk_building_protect.UseVisualStyleBackColor = true;
            // 
            // chk_selAll_building
            // 
            this.chk_selAll_building.AutoSize = true;
            this.chk_selAll_building.Location = new System.Drawing.Point(78, 15);
            this.chk_selAll_building.Name = "chk_selAll_building";
            this.chk_selAll_building.Size = new System.Drawing.Size(78, 16);
            this.chk_selAll_building.TabIndex = 4;
            this.chk_selAll_building.Text = "全选/不选";
            this.chk_selAll_building.UseVisualStyleBackColor = true;
            this.chk_selAll_building.CheckedChanged += new System.EventHandler(this.chk_selAll_building_CheckedChanged);
            // 
            // dg_buildings
            // 
            this.dg_buildings.AllowDrop = true;
            this.dg_buildings.AllowUserToAddRows = false;
            this.dg_buildings.AllowUserToDeleteRows = false;
            this.dg_buildings.AllowUserToResizeRows = false;
            this.dg_buildings.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dg_buildings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_buildings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_chk_building,
            this.col_name_building,
            this.col_level_building});
            this.dg_buildings.Location = new System.Drawing.Point(7, 61);
            this.dg_buildings.Name = "dg_buildings";
            this.dg_buildings.RowHeadersWidth = 30;
            this.dg_buildings.RowTemplate.Height = 23;
            this.dg_buildings.Size = new System.Drawing.Size(190, 241);
            this.dg_buildings.TabIndex = 3;
            this.dg_buildings.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_buildings.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_buildings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_buildings.SelectionChanged += new System.EventHandler(this.dg_SelectionChanged);
            this.dg_buildings.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_DragDrop);
            this.dg_buildings.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_DragEnter);
            // 
            // col_chk_building
            // 
            this.col_chk_building.DataPropertyName = "IsChecked";
            this.col_chk_building.HeaderText = "";
            this.col_chk_building.Name = "col_chk_building";
            this.col_chk_building.Width = 30;
            // 
            // col_name_building
            // 
            this.col_name_building.DataPropertyName = "Name";
            this.col_name_building.HeaderText = "名称";
            this.col_name_building.Name = "col_name_building";
            this.col_name_building.Width = 55;
            // 
            // col_level_building
            // 
            this.col_level_building.DataPropertyName = "Level";
            this.col_level_building.HeaderText = "级别";
            this.col_level_building.Name = "col_level_building";
            this.col_level_building.Width = 55;
            // 
            // chk_building_enable
            // 
            this.chk_building_enable.AutoSize = true;
            this.chk_building_enable.Location = new System.Drawing.Point(7, 15);
            this.chk_building_enable.Name = "chk_building_enable";
            this.chk_building_enable.Size = new System.Drawing.Size(48, 16);
            this.chk_building_enable.TabIndex = 1;
            this.chk_building_enable.Text = "开启";
            this.chk_building_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.num_get_baoshi_stone);
            this.groupBox4.Controls.Add(this.chk_get_baoshi_stone);
            this.groupBox4.Controls.Add(this.chk_secretary_giftbox);
            this.groupBox4.Controls.Add(this.num_secretary_open_treasure);
            this.groupBox4.Controls.Add(this.chk_secretary_open_treasure);
            this.groupBox4.Controls.Add(this.chk_dailytask_enable);
            this.groupBox4.Controls.Add(this.chk_stock_7);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.chk_stock_6);
            this.groupBox4.Controls.Add(this.chk_merchant_onlyfree);
            this.groupBox4.Controls.Add(this.chk_stock_5);
            this.groupBox4.Controls.Add(this.combo_merchant_sell_level);
            this.groupBox4.Controls.Add(this.chk_stock_4);
            this.groupBox4.Controls.Add(this.combo_merchant_type);
            this.groupBox4.Controls.Add(this.chk_stock_3);
            this.groupBox4.Controls.Add(this.chk_merchant);
            this.groupBox4.Controls.Add(this.chk_stock_2);
            this.groupBox4.Controls.Add(this.chk_stock_1);
            this.groupBox4.Controls.Add(this.num_get_stone);
            this.groupBox4.Controls.Add(this.chk_stock_enable);
            this.groupBox4.Controls.Add(this.chk_get_stone);
            this.groupBox4.Controls.Add(this.chk_dinner);
            this.groupBox4.Location = new System.Drawing.Point(214, 57);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 229);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "小秘书";
            // 
            // num_get_baoshi_stone
            // 
            this.num_get_baoshi_stone.Location = new System.Drawing.Point(151, 83);
            this.num_get_baoshi_stone.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_get_baoshi_stone.Name = "num_get_baoshi_stone";
            this.num_get_baoshi_stone.Size = new System.Drawing.Size(46, 21);
            this.num_get_baoshi_stone.TabIndex = 49;
            this.num_get_baoshi_stone.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chk_get_baoshi_stone
            // 
            this.chk_get_baoshi_stone.AutoSize = true;
            this.chk_get_baoshi_stone.Location = new System.Drawing.Point(6, 88);
            this.chk_get_baoshi_stone.Name = "chk_get_baoshi_stone";
            this.chk_get_baoshi_stone.Size = new System.Drawing.Size(144, 16);
            this.chk_get_baoshi_stone.TabIndex = 48;
            this.chk_get_baoshi_stone.Text = "自动采集宝石百分比≥";
            this.chk_get_baoshi_stone.UseVisualStyleBackColor = true;
            // 
            // chk_secretary_giftbox
            // 
            this.chk_secretary_giftbox.AutoSize = true;
            this.chk_secretary_giftbox.Location = new System.Drawing.Point(6, 153);
            this.chk_secretary_giftbox.Name = "chk_secretary_giftbox";
            this.chk_secretary_giftbox.Size = new System.Drawing.Size(168, 16);
            this.chk_secretary_giftbox.TabIndex = 47;
            this.chk_secretary_giftbox.Text = "自动充值赠礼(仅在线宝箱)";
            this.chk_secretary_giftbox.UseVisualStyleBackColor = true;
            // 
            // num_secretary_open_treasure
            // 
            this.num_secretary_open_treasure.Location = new System.Drawing.Point(151, 33);
            this.num_secretary_open_treasure.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_secretary_open_treasure.Name = "num_secretary_open_treasure";
            this.num_secretary_open_treasure.Size = new System.Drawing.Size(46, 21);
            this.num_secretary_open_treasure.TabIndex = 46;
            this.num_secretary_open_treasure.Value = new decimal(new int[] {
            450,
            0,
            0,
            0});
            // 
            // chk_secretary_open_treasure
            // 
            this.chk_secretary_open_treasure.AutoSize = true;
            this.chk_secretary_open_treasure.Location = new System.Drawing.Point(6, 38);
            this.chk_secretary_open_treasure.Name = "chk_secretary_open_treasure";
            this.chk_secretary_open_treasure.Size = new System.Drawing.Size(144, 16);
            this.chk_secretary_open_treasure.TabIndex = 45;
            this.chk_secretary_open_treasure.Text = "开国家宝箱如果个数≥";
            this.chk_secretary_open_treasure.UseVisualStyleBackColor = true;
            // 
            // chk_dailytask_enable
            // 
            this.chk_dailytask_enable.AutoSize = true;
            this.chk_dailytask_enable.Location = new System.Drawing.Point(98, 17);
            this.chk_dailytask_enable.Name = "chk_dailytask_enable";
            this.chk_dailytask_enable.Size = new System.Drawing.Size(96, 16);
            this.chk_dailytask_enable.TabIndex = 44;
            this.chk_dailytask_enable.Text = "自动每日任务";
            this.chk_dailytask_enable.UseVisualStyleBackColor = true;
            // 
            // chk_stock_7
            // 
            this.chk_stock_7.AutoSize = true;
            this.chk_stock_7.Location = new System.Drawing.Point(6, 209);
            this.chk_stock_7.Name = "chk_stock_7";
            this.chk_stock_7.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_7.TabIndex = 15;
            this.chk_stock_7.Text = "钻石";
            this.chk_stock_7.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(22, 135);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(101, 12);
            this.label17.TabIndex = 31;
            this.label17.Text = "卖掉委派物品级别";
            // 
            // chk_stock_6
            // 
            this.chk_stock_6.AutoSize = true;
            this.chk_stock_6.Location = new System.Drawing.Point(150, 191);
            this.chk_stock_6.Name = "chk_stock_6";
            this.chk_stock_6.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_6.TabIndex = 14;
            this.chk_stock_6.Text = "珍珠";
            this.chk_stock_6.UseVisualStyleBackColor = true;
            // 
            // chk_merchant_onlyfree
            // 
            this.chk_merchant_onlyfree.AutoSize = true;
            this.chk_merchant_onlyfree.Location = new System.Drawing.Point(132, 112);
            this.chk_merchant_onlyfree.Name = "chk_merchant_onlyfree";
            this.chk_merchant_onlyfree.Size = new System.Drawing.Size(60, 16);
            this.chk_merchant_onlyfree.TabIndex = 28;
            this.chk_merchant_onlyfree.Text = "仅免费";
            this.chk_merchant_onlyfree.UseVisualStyleBackColor = true;
            // 
            // chk_stock_5
            // 
            this.chk_stock_5.AutoSize = true;
            this.chk_stock_5.Location = new System.Drawing.Point(102, 209);
            this.chk_stock_5.Name = "chk_stock_5";
            this.chk_stock_5.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_5.TabIndex = 13;
            this.chk_stock_5.Text = "黄金";
            this.chk_stock_5.UseVisualStyleBackColor = true;
            // 
            // combo_merchant_sell_level
            // 
            this.combo_merchant_sell_level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_merchant_sell_level.FormattingEnabled = true;
            this.combo_merchant_sell_level.Items.AddRange(new object[] {
            "白色",
            "蓝色",
            "绿色",
            "黄色",
            "红色",
            "紫色"});
            this.combo_merchant_sell_level.Location = new System.Drawing.Point(132, 130);
            this.combo_merchant_sell_level.Name = "combo_merchant_sell_level";
            this.combo_merchant_sell_level.Size = new System.Drawing.Size(58, 20);
            this.combo_merchant_sell_level.TabIndex = 27;
            // 
            // chk_stock_4
            // 
            this.chk_stock_4.AutoSize = true;
            this.chk_stock_4.Location = new System.Drawing.Point(54, 209);
            this.chk_stock_4.Name = "chk_stock_4";
            this.chk_stock_4.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_4.TabIndex = 12;
            this.chk_stock_4.Text = "铁矿";
            this.chk_stock_4.UseVisualStyleBackColor = true;
            // 
            // combo_merchant_type
            // 
            this.combo_merchant_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_merchant_type.FormattingEnabled = true;
            this.combo_merchant_type.Items.AddRange(new object[] {
            "披风",
            "马"});
            this.combo_merchant_type.Location = new System.Drawing.Point(77, 109);
            this.combo_merchant_type.Name = "combo_merchant_type";
            this.combo_merchant_type.Size = new System.Drawing.Size(43, 20);
            this.combo_merchant_type.TabIndex = 26;
            // 
            // chk_stock_3
            // 
            this.chk_stock_3.AutoSize = true;
            this.chk_stock_3.Location = new System.Drawing.Point(102, 191);
            this.chk_stock_3.Name = "chk_stock_3";
            this.chk_stock_3.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_3.TabIndex = 11;
            this.chk_stock_3.Text = "木材";
            this.chk_stock_3.UseVisualStyleBackColor = true;
            // 
            // chk_merchant
            // 
            this.chk_merchant.AutoSize = true;
            this.chk_merchant.Location = new System.Drawing.Point(6, 112);
            this.chk_merchant.Name = "chk_merchant";
            this.chk_merchant.Size = new System.Drawing.Size(72, 16);
            this.chk_merchant.TabIndex = 25;
            this.chk_merchant.Text = "自动委派";
            this.chk_merchant.UseVisualStyleBackColor = true;
            // 
            // chk_stock_2
            // 
            this.chk_stock_2.AutoSize = true;
            this.chk_stock_2.Location = new System.Drawing.Point(54, 191);
            this.chk_stock_2.Name = "chk_stock_2";
            this.chk_stock_2.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_2.TabIndex = 10;
            this.chk_stock_2.Text = "苹果";
            this.chk_stock_2.UseVisualStyleBackColor = true;
            // 
            // chk_stock_1
            // 
            this.chk_stock_1.AutoSize = true;
            this.chk_stock_1.Location = new System.Drawing.Point(6, 191);
            this.chk_stock_1.Name = "chk_stock_1";
            this.chk_stock_1.Size = new System.Drawing.Size(48, 16);
            this.chk_stock_1.TabIndex = 9;
            this.chk_stock_1.Text = "绿豆";
            this.chk_stock_1.UseVisualStyleBackColor = true;
            // 
            // num_get_stone
            // 
            this.num_get_stone.Location = new System.Drawing.Point(151, 57);
            this.num_get_stone.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_get_stone.Name = "num_get_stone";
            this.num_get_stone.Size = new System.Drawing.Size(46, 21);
            this.num_get_stone.TabIndex = 21;
            this.num_get_stone.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chk_stock_enable
            // 
            this.chk_stock_enable.AutoSize = true;
            this.chk_stock_enable.Location = new System.Drawing.Point(6, 172);
            this.chk_stock_enable.Name = "chk_stock_enable";
            this.chk_stock_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_stock_enable.TabIndex = 8;
            this.chk_stock_enable.Text = "开启贸易";
            this.chk_stock_enable.UseVisualStyleBackColor = true;
            // 
            // chk_get_stone
            // 
            this.chk_get_stone.AutoSize = true;
            this.chk_get_stone.Location = new System.Drawing.Point(6, 62);
            this.chk_get_stone.Name = "chk_get_stone";
            this.chk_get_stone.Size = new System.Drawing.Size(144, 16);
            this.chk_get_stone.TabIndex = 20;
            this.chk_get_stone.Text = "自动采集玉石百分比≥";
            this.chk_get_stone.UseVisualStyleBackColor = true;
            // 
            // chk_dinner
            // 
            this.chk_dinner.AutoSize = true;
            this.chk_dinner.Location = new System.Drawing.Point(6, 17);
            this.chk_dinner.Name = "chk_dinner";
            this.chk_dinner.Size = new System.Drawing.Size(72, 16);
            this.chk_dinner.TabIndex = 19;
            this.chk_dinner.Text = "自动宴会";
            this.chk_dinner.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.num_global_gem_price);
            this.groupBox3.Controls.Add(this.num_global_reserve_credit);
            this.groupBox3.Controls.Add(this.chk_global_reserve_credit);
            this.groupBox3.Controls.Add(this.num_global_reserve_stone);
            this.groupBox3.Controls.Add(this.chk_global_reserve_stone);
            this.groupBox3.Controls.Add(this.num_global_reserve_gold);
            this.groupBox3.Controls.Add(this.chk_global_reserve_gold);
            this.groupBox3.Controls.Add(this.num_global_reserve_silver);
            this.groupBox3.Controls.Add(this.chk_global_reserve_silver);
            this.groupBox3.Controls.Add(this.chk_global_logonAward);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1038, 45);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "全局";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(790, 24);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(167, 12);
            this.label23.TabIndex = 14;
            this.label23.Text = "宝石性价比: 10金币=多少宝石";
            // 
            // num_global_gem_price
            // 
            this.num_global_gem_price.Location = new System.Drawing.Point(960, 19);
            this.num_global_gem_price.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_global_gem_price.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_global_gem_price.Name = "num_global_gem_price";
            this.num_global_gem_price.Size = new System.Drawing.Size(72, 21);
            this.num_global_gem_price.TabIndex = 13;
            this.num_global_gem_price.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // num_global_reserve_credit
            // 
            this.num_global_reserve_credit.Location = new System.Drawing.Point(675, 16);
            this.num_global_reserve_credit.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_global_reserve_credit.Name = "num_global_reserve_credit";
            this.num_global_reserve_credit.Size = new System.Drawing.Size(98, 21);
            this.num_global_reserve_credit.TabIndex = 12;
            // 
            // chk_global_reserve_credit
            // 
            this.chk_global_reserve_credit.AutoSize = true;
            this.chk_global_reserve_credit.Location = new System.Drawing.Point(603, 20);
            this.chk_global_reserve_credit.Name = "chk_global_reserve_credit";
            this.chk_global_reserve_credit.Size = new System.Drawing.Size(72, 16);
            this.chk_global_reserve_credit.TabIndex = 11;
            this.chk_global_reserve_credit.Text = "保留军工";
            this.chk_global_reserve_credit.UseVisualStyleBackColor = true;
            // 
            // num_global_reserve_stone
            // 
            this.num_global_reserve_stone.Location = new System.Drawing.Point(531, 15);
            this.num_global_reserve_stone.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_global_reserve_stone.Name = "num_global_reserve_stone";
            this.num_global_reserve_stone.Size = new System.Drawing.Size(66, 21);
            this.num_global_reserve_stone.TabIndex = 10;
            // 
            // chk_global_reserve_stone
            // 
            this.chk_global_reserve_stone.AutoSize = true;
            this.chk_global_reserve_stone.Location = new System.Drawing.Point(453, 20);
            this.chk_global_reserve_stone.Name = "chk_global_reserve_stone";
            this.chk_global_reserve_stone.Size = new System.Drawing.Size(72, 16);
            this.chk_global_reserve_stone.TabIndex = 9;
            this.chk_global_reserve_stone.Text = "保留玉石";
            this.chk_global_reserve_stone.UseVisualStyleBackColor = true;
            // 
            // num_global_reserve_gold
            // 
            this.num_global_reserve_gold.Location = new System.Drawing.Point(367, 15);
            this.num_global_reserve_gold.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_global_reserve_gold.Name = "num_global_reserve_gold";
            this.num_global_reserve_gold.Size = new System.Drawing.Size(79, 21);
            this.num_global_reserve_gold.TabIndex = 8;
            // 
            // chk_global_reserve_gold
            // 
            this.chk_global_reserve_gold.AutoSize = true;
            this.chk_global_reserve_gold.Location = new System.Drawing.Point(295, 18);
            this.chk_global_reserve_gold.Name = "chk_global_reserve_gold";
            this.chk_global_reserve_gold.Size = new System.Drawing.Size(72, 16);
            this.chk_global_reserve_gold.TabIndex = 7;
            this.chk_global_reserve_gold.Text = "保留金币";
            this.chk_global_reserve_gold.UseVisualStyleBackColor = true;
            // 
            // num_global_reserve_silver
            // 
            this.num_global_reserve_silver.Location = new System.Drawing.Point(187, 15);
            this.num_global_reserve_silver.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_global_reserve_silver.Name = "num_global_reserve_silver";
            this.num_global_reserve_silver.Size = new System.Drawing.Size(98, 21);
            this.num_global_reserve_silver.TabIndex = 6;
            // 
            // chk_global_reserve_silver
            // 
            this.chk_global_reserve_silver.AutoSize = true;
            this.chk_global_reserve_silver.Location = new System.Drawing.Point(109, 20);
            this.chk_global_reserve_silver.Name = "chk_global_reserve_silver";
            this.chk_global_reserve_silver.Size = new System.Drawing.Size(72, 16);
            this.chk_global_reserve_silver.TabIndex = 2;
            this.chk_global_reserve_silver.Text = "保留银币";
            this.chk_global_reserve_silver.UseVisualStyleBackColor = true;
            // 
            // chk_global_logonAward
            // 
            this.chk_global_logonAward.AutoSize = true;
            this.chk_global_logonAward.Location = new System.Drawing.Point(7, 20);
            this.chk_global_logonAward.Name = "chk_global_logonAward";
            this.chk_global_logonAward.Size = new System.Drawing.Size(96, 16);
            this.chk_global_logonAward.TabIndex = 1;
            this.chk_global_logonAward.Text = "领取登录奖励";
            this.chk_global_logonAward.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.num_impose_loyalty);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.num_impose_reserve);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.num_force_impose_gold);
            this.groupBox1.Controls.Add(this.chk_impose_enable);
            this.groupBox1.Location = new System.Drawing.Point(8, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "征收";
            // 
            // num_impose_loyalty
            // 
            this.num_impose_loyalty.Location = new System.Drawing.Point(159, 42);
            this.num_impose_loyalty.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_impose_loyalty.Name = "num_impose_loyalty";
            this.num_impose_loyalty.Size = new System.Drawing.Size(38, 21);
            this.num_impose_loyalty.TabIndex = 8;
            this.num_impose_loyalty.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 7;
            this.label13.Text = "强征到";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(105, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 6;
            this.label12.Text = "保留次数";
            // 
            // num_impose_reserve
            // 
            this.num_impose_reserve.Location = new System.Drawing.Point(157, 17);
            this.num_impose_reserve.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.num_impose_reserve.Name = "num_impose_reserve";
            this.num_impose_reserve.Size = new System.Drawing.Size(40, 21);
            this.num_impose_reserve.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "金币 选民忠≤";
            // 
            // num_force_impose_gold
            // 
            this.num_force_impose_gold.Location = new System.Drawing.Point(46, 39);
            this.num_force_impose_gold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_force_impose_gold.Name = "num_force_impose_gold";
            this.num_force_impose_gold.Size = new System.Drawing.Size(35, 21);
            this.num_force_impose_gold.TabIndex = 2;
            // 
            // chk_impose_enable
            // 
            this.chk_impose_enable.AutoSize = true;
            this.chk_impose_enable.Location = new System.Drawing.Point(7, 21);
            this.chk_impose_enable.Name = "chk_impose_enable";
            this.chk_impose_enable.Size = new System.Drawing.Size(48, 16);
            this.chk_impose_enable.TabIndex = 0;
            this.chk_impose_enable.Text = "开启";
            this.chk_impose_enable.UseVisualStyleBackColor = true;
            // 
            // setPage2
            // 
            this.setPage2.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.setPage2.Controls.Add(this.groupBox20);
            this.setPage2.Controls.Add(this.groupBox19);
            this.setPage2.Controls.Add(this.chk_ShenHuo);
            this.setPage2.Controls.Add(this.groupBox17);
            this.setPage2.Controls.Add(this.groupBox14);
            this.setPage2.Controls.Add(this.groupBox13);
            this.setPage2.Controls.Add(this.groupBox2);
            this.setPage2.Controls.Add(this.groupBox18);
            this.setPage2.Controls.Add(this.groupBox16);
            this.setPage2.Controls.Add(this.groupBox15);
            this.setPage2.Controls.Add(this.groupBox12);
            this.setPage2.Controls.Add(this.groupBox11);
            this.setPage2.Location = new System.Drawing.Point(4, 22);
            this.setPage2.Name = "setPage2";
            this.setPage2.Padding = new System.Windows.Forms.Padding(3);
            this.setPage2.Size = new System.Drawing.Size(1052, 607);
            this.setPage2.TabIndex = 3;
            this.setPage2.Text = "设置2";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.cb_OpenYuebingHongbaoType);
            this.groupBox20.Controls.Add(this.chk_AutoYuebingHongbao);
            this.groupBox20.Controls.Add(this.chk_GoldTrain);
            this.groupBox20.Controls.Add(this.chk_AutoYuebing);
            this.groupBox20.Location = new System.Drawing.Point(843, 465);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(204, 139);
            this.groupBox20.TabIndex = 56;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "国庆阅兵";
            // 
            // cb_OpenYuebingHongbaoType
            // 
            this.cb_OpenYuebingHongbaoType.FormattingEnabled = true;
            this.cb_OpenYuebingHongbaoType.Items.AddRange(new object[] {
            "免费",
            "2倍",
            "4倍",
            "10倍"});
            this.cb_OpenYuebingHongbaoType.Location = new System.Drawing.Point(127, 92);
            this.cb_OpenYuebingHongbaoType.Name = "cb_OpenYuebingHongbaoType";
            this.cb_OpenYuebingHongbaoType.Size = new System.Drawing.Size(61, 20);
            this.cb_OpenYuebingHongbaoType.TabIndex = 3;
            // 
            // chk_AutoYuebingHongbao
            // 
            this.chk_AutoYuebingHongbao.AutoSize = true;
            this.chk_AutoYuebingHongbao.Location = new System.Drawing.Point(20, 94);
            this.chk_AutoYuebingHongbao.Name = "chk_AutoYuebingHongbao";
            this.chk_AutoYuebingHongbao.Size = new System.Drawing.Size(108, 16);
            this.chk_AutoYuebingHongbao.TabIndex = 2;
            this.chk_AutoYuebingHongbao.Text = "自动开阅兵红包";
            this.chk_AutoYuebingHongbao.UseVisualStyleBackColor = true;
            // 
            // chk_GoldTrain
            // 
            this.chk_GoldTrain.AutoSize = true;
            this.chk_GoldTrain.Location = new System.Drawing.Point(20, 57);
            this.chk_GoldTrain.Name = "chk_GoldTrain";
            this.chk_GoldTrain.Size = new System.Drawing.Size(72, 16);
            this.chk_GoldTrain.TabIndex = 1;
            this.chk_GoldTrain.Text = "金币训练";
            this.chk_GoldTrain.UseVisualStyleBackColor = true;
            // 
            // chk_AutoYuebing
            // 
            this.chk_AutoYuebing.AutoSize = true;
            this.chk_AutoYuebing.Location = new System.Drawing.Point(20, 22);
            this.chk_AutoYuebing.Name = "chk_AutoYuebing";
            this.chk_AutoYuebing.Size = new System.Drawing.Size(96, 16);
            this.chk_AutoYuebing.TabIndex = 0;
            this.chk_AutoYuebing.Text = "自动阅兵活动";
            this.chk_AutoYuebing.UseVisualStyleBackColor = true;
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.label63);
            this.groupBox19.Controls.Add(this.nUD_boat_up_coin);
            this.groupBox19.Controls.Add(this.label62);
            this.groupBox19.Controls.Add(this.chk_AutoSelectTime);
            this.groupBox19.Controls.Add(this.txt_boat_upgrade);
            this.groupBox19.Controls.Add(this.label54);
            this.groupBox19.Controls.Add(this.chk_boat_creat);
            this.groupBox19.Controls.Add(this.chk_AutoBoat);
            this.groupBox19.Location = new System.Drawing.Point(843, 303);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(204, 150);
            this.groupBox19.TabIndex = 55;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "龙舟活动";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(167, 113);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(29, 12);
            this.label63.TabIndex = 7;
            this.label63.Text = "金币";
            // 
            // nUD_boat_up_coin
            // 
            this.nUD_boat_up_coin.Location = new System.Drawing.Point(106, 108);
            this.nUD_boat_up_coin.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUD_boat_up_coin.Name = "nUD_boat_up_coin";
            this.nUD_boat_up_coin.Size = new System.Drawing.Size(56, 21);
            this.nUD_boat_up_coin.TabIndex = 6;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(9, 112);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(101, 12);
            this.label62.TabIndex = 5;
            this.label62.Text = "龙舟比赛加速到：";
            // 
            // chk_AutoSelectTime
            // 
            this.chk_AutoSelectTime.AutoSize = true;
            this.chk_AutoSelectTime.Location = new System.Drawing.Point(9, 90);
            this.chk_AutoSelectTime.Name = "chk_AutoSelectTime";
            this.chk_AutoSelectTime.Size = new System.Drawing.Size(120, 16);
            this.chk_AutoSelectTime.TabIndex = 4;
            this.chk_AutoSelectTime.Text = "自动选择比赛时间";
            this.chk_AutoSelectTime.UseVisualStyleBackColor = true;
            // 
            // txt_boat_upgrade
            // 
            this.txt_boat_upgrade.Location = new System.Drawing.Point(98, 66);
            this.txt_boat_upgrade.Name = "txt_boat_upgrade";
            this.txt_boat_upgrade.Size = new System.Drawing.Size(100, 21);
            this.txt_boat_upgrade.TabIndex = 3;
            this.tip_msg.SetToolTip(this.txt_boat_upgrade, "1=蓝色,2=绿色,3=黄色,4=红色, 如果什么都不升级就不要填");
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(9, 69);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(89, 12);
            this.label54.TabIndex = 2;
            this.label54.Text = "升级紫色类型：";
            // 
            // chk_boat_creat
            // 
            this.chk_boat_creat.AutoSize = true;
            this.chk_boat_creat.Location = new System.Drawing.Point(10, 46);
            this.chk_boat_creat.Name = "chk_boat_creat";
            this.chk_boat_creat.Size = new System.Drawing.Size(144, 16);
            this.chk_boat_creat.TabIndex = 1;
            this.chk_boat_creat.Text = "我要自己创建龙舟队伍";
            this.chk_boat_creat.UseVisualStyleBackColor = true;
            // 
            // chk_AutoBoat
            // 
            this.chk_AutoBoat.AutoSize = true;
            this.chk_AutoBoat.Location = new System.Drawing.Point(10, 21);
            this.chk_AutoBoat.Name = "chk_AutoBoat";
            this.chk_AutoBoat.Size = new System.Drawing.Size(96, 16);
            this.chk_AutoBoat.TabIndex = 0;
            this.chk_AutoBoat.Text = "自动龙舟活动";
            this.chk_AutoBoat.UseVisualStyleBackColor = true;
            // 
            // chk_ShenHuo
            // 
            this.chk_ShenHuo.AutoSize = true;
            this.chk_ShenHuo.Location = new System.Drawing.Point(853, 275);
            this.chk_ShenHuo.Name = "chk_ShenHuo";
            this.chk_ShenHuo.Size = new System.Drawing.Size(96, 16);
            this.chk_ShenHuo.TabIndex = 54;
            this.chk_ShenHuo.Text = "自动百炼精铁";
            this.chk_ShenHuo.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.chk_SuperUseFree);
            this.groupBox17.Controls.Add(this.label56);
            this.groupBox17.Controls.Add(this.label60);
            this.groupBox17.Controls.Add(this.label61);
            this.groupBox17.Controls.Add(this.nUD_DianQuan);
            this.groupBox17.Controls.Add(this.label59);
            this.groupBox17.Controls.Add(this.label58);
            this.groupBox17.Controls.Add(this.label57);
            this.groupBox17.Controls.Add(this.nUD_CiShu);
            this.groupBox17.Controls.Add(this.label55);
            this.groupBox17.Controls.Add(this.nUD_BaoShi);
            this.groupBox17.Controls.Add(this.chk_AutoBaiShen);
            this.groupBox17.Location = new System.Drawing.Point(843, 124);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(204, 139);
            this.groupBox17.TabIndex = 53;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "拜神";
            // 
            // chk_SuperUseFree
            // 
            this.chk_SuperUseFree.AutoSize = true;
            this.chk_SuperUseFree.Location = new System.Drawing.Point(6, 89);
            this.chk_SuperUseFree.Name = "chk_SuperUseFree";
            this.chk_SuperUseFree.Size = new System.Drawing.Size(156, 16);
            this.chk_SuperUseFree.TabIndex = 68;
            this.chk_SuperUseFree.Text = "高级祭祀只使用免费贡品";
            this.chk_SuperUseFree.UseVisualStyleBackColor = true;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(121, 66);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(77, 12);
            this.label56.TabIndex = 67;
            this.label56.Text = "使用高级祭祀";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(39, 66);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(17, 12);
            this.label60.TabIndex = 66;
            this.label60.Text = "≥";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(7, 66);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(41, 12);
            this.label61.TabIndex = 65;
            this.label61.Text = "点券：";
            // 
            // nUD_DianQuan
            // 
            this.nUD_DianQuan.Increment = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nUD_DianQuan.Location = new System.Drawing.Point(62, 62);
            this.nUD_DianQuan.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.nUD_DianQuan.Name = "nUD_DianQuan";
            this.nUD_DianQuan.Size = new System.Drawing.Size(63, 21);
            this.nUD_DianQuan.TabIndex = 64;
            this.nUD_DianQuan.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(122, 44);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(77, 12);
            this.label59.TabIndex = 63;
            this.label59.Text = "使用高级祭祀";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(39, 44);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(17, 12);
            this.label58.TabIndex = 62;
            this.label58.Text = "≥";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(8, 44);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(41, 12);
            this.label57.TabIndex = 6;
            this.label57.Text = "宝石：";
            // 
            // nUD_CiShu
            // 
            this.nUD_CiShu.Location = new System.Drawing.Point(90, 111);
            this.nUD_CiShu.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUD_CiShu.Name = "nUD_CiShu";
            this.nUD_CiShu.Size = new System.Drawing.Size(56, 21);
            this.nUD_CiShu.TabIndex = 4;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(8, 116);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(89, 12);
            this.label55.TabIndex = 3;
            this.label55.Text = "购买显灵次数：";
            // 
            // nUD_BaoShi
            // 
            this.nUD_BaoShi.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nUD_BaoShi.Location = new System.Drawing.Point(62, 40);
            this.nUD_BaoShi.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nUD_BaoShi.Name = "nUD_BaoShi";
            this.nUD_BaoShi.Size = new System.Drawing.Size(63, 21);
            this.nUD_BaoShi.TabIndex = 2;
            this.nUD_BaoShi.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // chk_AutoBaiShen
            // 
            this.chk_AutoBaiShen.AutoSize = true;
            this.chk_AutoBaiShen.Location = new System.Drawing.Point(7, 21);
            this.chk_AutoBaiShen.Name = "chk_AutoBaiShen";
            this.chk_AutoBaiShen.Size = new System.Drawing.Size(72, 16);
            this.chk_AutoBaiShen.TabIndex = 0;
            this.chk_AutoBaiShen.Text = "自动拜神";
            this.chk_AutoBaiShen.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label51);
            this.groupBox14.Controls.Add(this.downUp_QiFuDQ);
            this.groupBox14.Controls.Add(this.label50);
            this.groupBox14.Controls.Add(this.downUp_QiFuCoin);
            this.groupBox14.Controls.Add(this.label49);
            this.groupBox14.Controls.Add(this.checkBox_QiFu);
            this.groupBox14.Location = new System.Drawing.Point(842, 10);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(204, 101);
            this.groupBox14.TabIndex = 52;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "神灵祈福";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(136, 73);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(53, 12);
            this.label51.TabIndex = 5;
            this.label51.Text = "金币全开";
            // 
            // downUp_QiFuDQ
            // 
            this.downUp_QiFuDQ.Location = new System.Drawing.Point(63, 67);
            this.downUp_QiFuDQ.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.downUp_QiFuDQ.Name = "downUp_QiFuDQ";
            this.downUp_QiFuDQ.Size = new System.Drawing.Size(71, 21);
            this.downUp_QiFuDQ.TabIndex = 4;
            this.downUp_QiFuDQ.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(5, 73);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(53, 12);
            this.label50.TabIndex = 3;
            this.label50.Text = "点券大于";
            // 
            // downUp_QiFuCoin
            // 
            this.downUp_QiFuCoin.Location = new System.Drawing.Point(89, 40);
            this.downUp_QiFuCoin.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.downUp_QiFuCoin.Name = "downUp_QiFuCoin";
            this.downUp_QiFuCoin.Size = new System.Drawing.Size(46, 21);
            this.downUp_QiFuCoin.TabIndex = 2;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(6, 46);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(77, 12);
            this.label49.TabIndex = 1;
            this.label49.Text = "自动到金币≤";
            // 
            // checkBox_QiFu
            // 
            this.checkBox_QiFu.AutoSize = true;
            this.checkBox_QiFu.Location = new System.Drawing.Point(7, 21);
            this.checkBox_QiFu.Name = "checkBox_QiFu";
            this.checkBox_QiFu.Size = new System.Drawing.Size(96, 16);
            this.checkBox_QiFu.TabIndex = 0;
            this.checkBox_QiFu.Text = "自动神灵祈福";
            this.checkBox_QiFu.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.chk_auto_hide_flash);
            this.groupBox13.Controls.Add(this.chk_global_boss_key_enable);
            this.groupBox13.Controls.Add(this.combo_global_boss_key_key);
            this.groupBox13.Controls.Add(this.chk_global_boss_key_shift);
            this.groupBox13.Controls.Add(this.chk_global_boss_key_alt);
            this.groupBox13.Controls.Add(this.chk_global_boss_key_ctrl);
            this.groupBox13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox13.Location = new System.Drawing.Point(545, 502);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(285, 102);
            this.groupBox13.TabIndex = 51;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "杂项";
            // 
            // chk_auto_hide_flash
            // 
            this.chk_auto_hide_flash.AutoSize = true;
            this.chk_auto_hide_flash.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_auto_hide_flash.Location = new System.Drawing.Point(6, 73);
            this.chk_auto_hide_flash.Name = "chk_auto_hide_flash";
            this.chk_auto_hide_flash.Size = new System.Drawing.Size(180, 16);
            this.chk_auto_hide_flash.TabIndex = 42;
            this.chk_auto_hide_flash.Text = "最小化自动隐藏界面节省内存";
            this.chk_auto_hide_flash.UseVisualStyleBackColor = true;
            // 
            // chk_global_boss_key_enable
            // 
            this.chk_global_boss_key_enable.AutoSize = true;
            this.chk_global_boss_key_enable.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_global_boss_key_enable.Location = new System.Drawing.Point(6, 20);
            this.chk_global_boss_key_enable.Name = "chk_global_boss_key_enable";
            this.chk_global_boss_key_enable.Size = new System.Drawing.Size(60, 16);
            this.chk_global_boss_key_enable.TabIndex = 41;
            this.chk_global_boss_key_enable.Text = "老板键";
            this.chk_global_boss_key_enable.UseVisualStyleBackColor = true;
            // 
            // combo_global_boss_key_key
            // 
            this.combo_global_boss_key_key.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_global_boss_key_key.FormattingEnabled = true;
            this.combo_global_boss_key_key.Items.AddRange(new object[] {
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.combo_global_boss_key_key.Location = new System.Drawing.Point(183, 40);
            this.combo_global_boss_key_key.Name = "combo_global_boss_key_key";
            this.combo_global_boss_key_key.Size = new System.Drawing.Size(47, 20);
            this.combo_global_boss_key_key.TabIndex = 38;
            // 
            // chk_global_boss_key_shift
            // 
            this.chk_global_boss_key_shift.AutoSize = true;
            this.chk_global_boss_key_shift.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_global_boss_key_shift.Location = new System.Drawing.Point(123, 42);
            this.chk_global_boss_key_shift.Name = "chk_global_boss_key_shift";
            this.chk_global_boss_key_shift.Size = new System.Drawing.Size(54, 16);
            this.chk_global_boss_key_shift.TabIndex = 3;
            this.chk_global_boss_key_shift.Text = "Shift";
            this.chk_global_boss_key_shift.UseVisualStyleBackColor = true;
            // 
            // chk_global_boss_key_alt
            // 
            this.chk_global_boss_key_alt.AutoSize = true;
            this.chk_global_boss_key_alt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_global_boss_key_alt.Location = new System.Drawing.Point(75, 42);
            this.chk_global_boss_key_alt.Name = "chk_global_boss_key_alt";
            this.chk_global_boss_key_alt.Size = new System.Drawing.Size(42, 16);
            this.chk_global_boss_key_alt.TabIndex = 2;
            this.chk_global_boss_key_alt.Text = "Alt";
            this.chk_global_boss_key_alt.UseVisualStyleBackColor = true;
            // 
            // chk_global_boss_key_ctrl
            // 
            this.chk_global_boss_key_ctrl.AutoSize = true;
            this.chk_global_boss_key_ctrl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_global_boss_key_ctrl.Location = new System.Drawing.Point(18, 42);
            this.chk_global_boss_key_ctrl.Name = "chk_global_boss_key_ctrl";
            this.chk_global_boss_key_ctrl.Size = new System.Drawing.Size(48, 16);
            this.chk_global_boss_key_ctrl.TabIndex = 1;
            this.chk_global_boss_key_ctrl.Text = "Ctrl";
            this.chk_global_boss_key_ctrl.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.num_movable_reserve);
            this.groupBox2.Controls.Add(this.label45);
            this.groupBox2.Controls.Add(this.num_movable_refine_factory_gold);
            this.groupBox2.Controls.Add(this.num_movable_refine_factory_stone);
            this.groupBox2.Controls.Add(this.label44);
            this.groupBox2.Controls.Add(this.label43);
            this.groupBox2.Controls.Add(this.num_movable_weave_count);
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Controls.Add(this.label40);
            this.groupBox2.Controls.Add(this.chk_movable_arch_refill);
            this.groupBox2.Controls.Add(this.label38);
            this.groupBox2.Controls.Add(this.num_movable_visit_fail);
            this.groupBox2.Controls.Add(this.label37);
            this.groupBox2.Controls.Add(this.txt_movable_visit);
            this.groupBox2.Controls.Add(this.chk_movable_arch_trade_max);
            this.groupBox2.Controls.Add(this.label32);
            this.groupBox2.Controls.Add(this.num_movable_refine_reserve);
            this.groupBox2.Controls.Add(this.chk_movable_arch_trade);
            this.groupBox2.Controls.Add(this.chk_movable_winter_trade);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.combo_movable_refine_item);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.txt_movable_order);
            this.groupBox2.Controls.Add(this.num_movable_weave_price);
            this.groupBox2.Controls.Add(this.chk_movable_enable);
            this.groupBox2.Location = new System.Drawing.Point(545, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 342);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "行动力";
            // 
            // num_movable_reserve
            // 
            this.num_movable_reserve.Location = new System.Drawing.Point(222, 15);
            this.num_movable_reserve.Maximum = new decimal(new int[] {
            420,
            0,
            0,
            0});
            this.num_movable_reserve.Name = "num_movable_reserve";
            this.num_movable_reserve.Size = new System.Drawing.Size(46, 21);
            this.num_movable_reserve.TabIndex = 61;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(154, 21);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(65, 12);
            this.label45.TabIndex = 60;
            this.label45.Text = "保留行动力";
            // 
            // num_movable_refine_factory_gold
            // 
            this.num_movable_refine_factory_gold.Location = new System.Drawing.Point(223, 99);
            this.num_movable_refine_factory_gold.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.num_movable_refine_factory_gold.Name = "num_movable_refine_factory_gold";
            this.num_movable_refine_factory_gold.Size = new System.Drawing.Size(46, 21);
            this.num_movable_refine_factory_gold.TabIndex = 59;
            // 
            // num_movable_refine_factory_stone
            // 
            this.num_movable_refine_factory_stone.Location = new System.Drawing.Point(222, 77);
            this.num_movable_refine_factory_stone.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.num_movable_refine_factory_stone.Name = "num_movable_refine_factory_stone";
            this.num_movable_refine_factory_stone.Size = new System.Drawing.Size(46, 21);
            this.num_movable_refine_factory_stone.TabIndex = 58;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(79, 106);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(137, 12);
            this.label44.TabIndex = 57;
            this.label44.Text = "使用金币炼制如果金币≤";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(79, 86);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(137, 12);
            this.label43.TabIndex = 56;
            this.label43.Text = "使用玉石炼制如果玉石≤";
            // 
            // num_movable_weave_count
            // 
            this.num_movable_weave_count.Location = new System.Drawing.Point(222, 42);
            this.num_movable_weave_count.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.num_movable_weave_count.Name = "num_movable_weave_count";
            this.num_movable_weave_count.Size = new System.Drawing.Size(46, 21);
            this.num_movable_weave_count.TabIndex = 55;
            this.num_movable_weave_count.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(155, 47);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(65, 12);
            this.label42.TabIndex = 54;
            this.label42.Text = "纺织多少次";
            // 
            // label40
            // 
            this.label40.ForeColor = System.Drawing.Color.Brown;
            this.label40.Location = new System.Drawing.Point(30, 249);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(238, 80);
            this.label40.TabIndex = 53;
            this.label40.Text = "注意：                                  1. 行动力顺序为顺序执行,前面执行完毕前后面不会执行,因此建议将精炼(4)放在第一位," +
                "把通商放最后可以避免行动力爆仓     2. 精炼是否金币升级由全局设置的宝石性价比决定";
            // 
            // chk_movable_arch_refill
            // 
            this.chk_movable_arch_refill.AutoSize = true;
            this.chk_movable_arch_refill.Location = new System.Drawing.Point(26, 166);
            this.chk_movable_arch_refill.Name = "chk_movable_arch_refill";
            this.chk_movable_arch_refill.Size = new System.Drawing.Size(132, 16);
            this.chk_movable_arch_refill.TabIndex = 49;
            this.chk_movable_arch_refill.Text = "考古自动补充行动力";
            this.chk_movable_arch_refill.UseVisualStyleBackColor = true;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(27, 125);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(53, 12);
            this.label38.TabIndex = 48;
            this.label38.Text = "拜访商盟";
            // 
            // num_movable_visit_fail
            // 
            this.num_movable_visit_fail.Location = new System.Drawing.Point(225, 121);
            this.num_movable_visit_fail.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_movable_visit_fail.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_movable_visit_fail.Name = "num_movable_visit_fail";
            this.num_movable_visit_fail.Size = new System.Drawing.Size(43, 21);
            this.num_movable_visit_fail.TabIndex = 47;
            this.num_movable_visit_fail.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(148, 125);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(77, 12);
            this.label37.TabIndex = 46;
            this.label37.Text = "允许失败次数";
            // 
            // txt_movable_visit
            // 
            this.txt_movable_visit.Location = new System.Drawing.Point(83, 121);
            this.txt_movable_visit.Name = "txt_movable_visit";
            this.txt_movable_visit.Size = new System.Drawing.Size(61, 21);
            this.txt_movable_visit.TabIndex = 45;
            this.tip_msg.SetToolTip(this.txt_movable_visit, "1=楼兰, 2=西域, 3=巴蜀, 4=大理, 5=闽南, 6=辽东, 7=关东, 8=淮南, 9=荆楚, 10=南越");
            // 
            // chk_movable_arch_trade_max
            // 
            this.chk_movable_arch_trade_max.AutoSize = true;
            this.chk_movable_arch_trade_max.Location = new System.Drawing.Point(26, 185);
            this.chk_movable_arch_trade_max.Name = "chk_movable_arch_trade_max";
            this.chk_movable_arch_trade_max.Size = new System.Drawing.Size(228, 16);
            this.chk_movable_arch_trade_max.TabIndex = 43;
            this.chk_movable_arch_trade_max.Text = "老版通商考古自动升级驿站并极限通商";
            this.chk_movable_arch_trade_max.UseVisualStyleBackColor = true;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(142, 227);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(77, 12);
            this.label32.TabIndex = 42;
            this.label32.Text = "保留精炼次数";
            // 
            // num_movable_refine_reserve
            // 
            this.num_movable_refine_reserve.Location = new System.Drawing.Point(223, 223);
            this.num_movable_refine_reserve.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.num_movable_refine_reserve.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.num_movable_refine_reserve.Name = "num_movable_refine_reserve";
            this.num_movable_refine_reserve.Size = new System.Drawing.Size(46, 21);
            this.num_movable_refine_reserve.TabIndex = 41;
            this.num_movable_refine_reserve.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chk_movable_arch_trade
            // 
            this.chk_movable_arch_trade.AutoSize = true;
            this.chk_movable_arch_trade.Location = new System.Drawing.Point(26, 149);
            this.chk_movable_arch_trade.Name = "chk_movable_arch_trade";
            this.chk_movable_arch_trade.Size = new System.Drawing.Size(96, 16);
            this.chk_movable_arch_trade.TabIndex = 40;
            this.chk_movable_arch_trade.Text = "考古优先通商";
            this.chk_movable_arch_trade.UseVisualStyleBackColor = true;
            // 
            // chk_movable_winter_trade
            // 
            this.chk_movable_winter_trade.AutoSize = true;
            this.chk_movable_winter_trade.Location = new System.Drawing.Point(26, 205);
            this.chk_movable_winter_trade.Name = "chk_movable_winter_trade";
            this.chk_movable_winter_trade.Size = new System.Drawing.Size(168, 16);
            this.chk_movable_winter_trade.TabIndex = 39;
            this.chk_movable_winter_trade.Text = "老版通商冬季自动优先通商";
            this.chk_movable_winter_trade.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(18, 68);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(53, 12);
            this.label30.TabIndex = 38;
            this.label30.Text = "炼制物品";
            // 
            // combo_movable_refine_item
            // 
            this.combo_movable_refine_item.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_movable_refine_item.FormattingEnabled = true;
            this.combo_movable_refine_item.Items.AddRange(new object[] {
            "玄霆角",
            "无敌将军炮",
            "五毒问心钉",
            "七戮锋",
            "蟠龙华盖",
            "轩辕指南车",
            "落魂冥灯",
            "彤云角",
            "红衣将军炮",
            "化骨丛棘",
            "弑履岩",
            "玉麟旌麾",
            "铜人计里车",
            "化骨悬灯"});
            this.combo_movable_refine_item.Location = new System.Drawing.Point(71, 64);
            this.combo_movable_refine_item.Name = "combo_movable_refine_item";
            this.combo_movable_refine_item.Size = new System.Drawing.Size(87, 20);
            this.combo_movable_refine_item.TabIndex = 37;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(13, 46);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(65, 12);
            this.label29.TabIndex = 36;
            this.label29.Text = "纺织布价≥";
            // 
            // txt_movable_order
            // 
            this.txt_movable_order.Location = new System.Drawing.Point(88, 16);
            this.txt_movable_order.Name = "txt_movable_order";
            this.txt_movable_order.Size = new System.Drawing.Size(46, 21);
            this.txt_movable_order.TabIndex = 35;
            this.txt_movable_order.Text = "2341";
            this.tip_msg.SetToolTip(this.txt_movable_order, "1=通商,2=纺织,3=炼制,4=精炼, 如果哪项不要就不要填");
            // 
            // num_movable_weave_price
            // 
            this.num_movable_weave_price.Location = new System.Drawing.Point(79, 42);
            this.num_movable_weave_price.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.num_movable_weave_price.Name = "num_movable_weave_price";
            this.num_movable_weave_price.Size = new System.Drawing.Size(46, 21);
            this.num_movable_weave_price.TabIndex = 16;
            this.num_movable_weave_price.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // chk_movable_enable
            // 
            this.chk_movable_enable.AutoSize = true;
            this.chk_movable_enable.Location = new System.Drawing.Point(6, 20);
            this.chk_movable_enable.Name = "chk_movable_enable";
            this.chk_movable_enable.Size = new System.Drawing.Size(84, 16);
            this.chk_movable_enable.TabIndex = 6;
            this.chk_movable_enable.Text = "行动力顺序";
            this.chk_movable_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.chk_new_day_treasure_game_shake_tree);
            this.groupBox18.Controls.Add(this.label69);
            this.groupBox18.Controls.Add(this.txt_new_day_treasure_game_boxtype);
            this.groupBox18.Controls.Add(this.chk_new_day_treasure_game);
            this.groupBox18.Controls.Add(this.txt_treasure_game_dice_type);
            this.groupBox18.Controls.Add(this.label68);
            this.groupBox18.Controls.Add(this.lbl_day_treasure_game_sel);
            this.groupBox18.Controls.Add(this.btn_day_treasure_game_select);
            this.groupBox18.Controls.Add(this.chk_daily_treasure_game_super_gold_end);
            this.groupBox18.Controls.Add(this.chk_treasure_game_ticket_gold_end);
            this.groupBox18.Controls.Add(this.combo_treasure_game_use_ticket_type);
            this.groupBox18.Controls.Add(this.chk_treasure_game_use_ticket);
            this.groupBox18.Controls.Add(this.label35);
            this.groupBox18.Controls.Add(this.txt_treasure_game_goldmove_boxtype);
            this.groupBox18.Controls.Add(this.label36);
            this.groupBox18.Controls.Add(this.txt_treasure_game_boxtype);
            this.groupBox18.Controls.Add(this.label34);
            this.groupBox18.Controls.Add(this.txt_daily_treasure_game_goldmove_boxtype);
            this.groupBox18.Controls.Add(this.label33);
            this.groupBox18.Controls.Add(this.txt_daily_treasure_game_boxtype);
            this.groupBox18.Controls.Add(this.label27);
            this.groupBox18.Controls.Add(this.label26);
            this.groupBox18.Controls.Add(this.num_treasure_game_goldstep);
            this.groupBox18.Controls.Add(this.num_daily_treasure_game_goldstep);
            this.groupBox18.Controls.Add(this.chk_treasure_game_enable);
            this.groupBox18.Controls.Add(this.chk_daily_treasure_game);
            this.groupBox18.Location = new System.Drawing.Point(293, 154);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(240, 447);
            this.groupBox18.TabIndex = 49;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "探宝";
            // 
            // chk_new_day_treasure_game_shake_tree
            // 
            this.chk_new_day_treasure_game_shake_tree.AutoSize = true;
            this.chk_new_day_treasure_game_shake_tree.Location = new System.Drawing.Point(23, 412);
            this.chk_new_day_treasure_game_shake_tree.Name = "chk_new_day_treasure_game_shake_tree";
            this.chk_new_day_treasure_game_shake_tree.Size = new System.Drawing.Size(72, 16);
            this.chk_new_day_treasure_game_shake_tree.TabIndex = 69;
            this.chk_new_day_treasure_game_shake_tree.Text = "宝石摇树";
            this.chk_new_day_treasure_game_shake_tree.UseVisualStyleBackColor = true;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(23, 387);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(89, 12);
            this.label69.TabIndex = 68;
            this.label69.Text = "打开宝箱类型：";
            // 
            // txt_new_day_treasure_game_boxtype
            // 
            this.txt_new_day_treasure_game_boxtype.Location = new System.Drawing.Point(118, 382);
            this.txt_new_day_treasure_game_boxtype.Name = "txt_new_day_treasure_game_boxtype";
            this.txt_new_day_treasure_game_boxtype.Size = new System.Drawing.Size(104, 21);
            this.txt_new_day_treasure_game_boxtype.TabIndex = 67;
            this.tip_msg.SetToolTip(this.txt_new_day_treasure_game_boxtype, "1=中级铁锤,2=高级铁锤,哪项不要就不要填");
            // 
            // chk_new_day_treasure_game
            // 
            this.chk_new_day_treasure_game.AutoSize = true;
            this.chk_new_day_treasure_game.Location = new System.Drawing.Point(6, 361);
            this.chk_new_day_treasure_game.Name = "chk_new_day_treasure_game";
            this.chk_new_day_treasure_game.Size = new System.Drawing.Size(72, 16);
            this.chk_new_day_treasure_game.TabIndex = 66;
            this.chk_new_day_treasure_game.Text = "终极探宝";
            this.chk_new_day_treasure_game.UseVisualStyleBackColor = true;
            // 
            // txt_treasure_game_dice_type
            // 
            this.txt_treasure_game_dice_type.Location = new System.Drawing.Point(147, 274);
            this.txt_treasure_game_dice_type.Name = "txt_treasure_game_dice_type";
            this.txt_treasure_game_dice_type.Size = new System.Drawing.Size(75, 21);
            this.txt_treasure_game_dice_type.TabIndex = 65;
            this.tip_msg.SetToolTip(this.txt_treasure_game_dice_type, "0:1,1:2,2:0,位置:类型,0表示开始位置,类型:1(1点),2(2点),0(免费)");
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(16, 279);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(125, 12);
            this.label68.TabIndex = 64;
            this.label68.Text = "每个格子掷骰子类型：";
            // 
            // lbl_day_treasure_game_sel
            // 
            this.lbl_day_treasure_game_sel.AutoSize = true;
            this.lbl_day_treasure_game_sel.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_day_treasure_game_sel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbl_day_treasure_game_sel.Location = new System.Drawing.Point(127, 39);
            this.lbl_day_treasure_game_sel.Name = "lbl_day_treasure_game_sel";
            this.lbl_day_treasure_game_sel.Size = new System.Drawing.Size(23, 15);
            this.lbl_day_treasure_game_sel.TabIndex = 63;
            this.lbl_day_treasure_game_sel.Text = "无";
            // 
            // btn_day_treasure_game_select
            // 
            this.btn_day_treasure_game_select.Location = new System.Drawing.Point(31, 39);
            this.btn_day_treasure_game_select.Name = "btn_day_treasure_game_select";
            this.btn_day_treasure_game_select.Size = new System.Drawing.Size(85, 23);
            this.btn_day_treasure_game_select.TabIndex = 62;
            this.btn_day_treasure_game_select.Text = "选择副本";
            this.btn_day_treasure_game_select.UseVisualStyleBackColor = true;
            this.btn_day_treasure_game_select.Click += new System.EventHandler(this.btn_day_treasure_game_select_Click);
            // 
            // chk_daily_treasure_game_super_gold_end
            // 
            this.chk_daily_treasure_game_super_gold_end.AutoSize = true;
            this.chk_daily_treasure_game_super_gold_end.Location = new System.Drawing.Point(27, 141);
            this.chk_daily_treasure_game_super_gold_end.Name = "chk_daily_treasure_game_super_gold_end";
            this.chk_daily_treasure_game_super_gold_end.Size = new System.Drawing.Size(144, 16);
            this.chk_daily_treasure_game_super_gold_end.TabIndex = 61;
            this.chk_daily_treasure_game_super_gold_end.Text = "超级探宝金币走到最后";
            this.chk_daily_treasure_game_super_gold_end.UseVisualStyleBackColor = true;
            // 
            // chk_treasure_game_ticket_gold_end
            // 
            this.chk_treasure_game_ticket_gold_end.AutoSize = true;
            this.chk_treasure_game_ticket_gold_end.Location = new System.Drawing.Point(45, 330);
            this.chk_treasure_game_ticket_gold_end.Name = "chk_treasure_game_ticket_gold_end";
            this.chk_treasure_game_ticket_gold_end.Size = new System.Drawing.Size(96, 16);
            this.chk_treasure_game_ticket_gold_end.TabIndex = 60;
            this.chk_treasure_game_ticket_gold_end.Text = "金币走到最后";
            this.chk_treasure_game_ticket_gold_end.UseVisualStyleBackColor = true;
            // 
            // combo_treasure_game_use_ticket_type
            // 
            this.combo_treasure_game_use_ticket_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_treasure_game_use_ticket_type.FormattingEnabled = true;
            this.combo_treasure_game_use_ticket_type.Items.AddRange(new object[] {
            "玉石",
            "宝石",
            "兵器"});
            this.combo_treasure_game_use_ticket_type.Location = new System.Drawing.Point(169, 303);
            this.combo_treasure_game_use_ticket_type.Name = "combo_treasure_game_use_ticket_type";
            this.combo_treasure_game_use_ticket_type.Size = new System.Drawing.Size(56, 20);
            this.combo_treasure_game_use_ticket_type.TabIndex = 59;
            // 
            // chk_treasure_game_use_ticket
            // 
            this.chk_treasure_game_use_ticket.AutoSize = true;
            this.chk_treasure_game_use_ticket.Location = new System.Drawing.Point(26, 307);
            this.chk_treasure_game_use_ticket.Name = "chk_treasure_game_use_ticket";
            this.chk_treasure_game_use_ticket.Size = new System.Drawing.Size(144, 16);
            this.chk_treasure_game_use_ticket.TabIndex = 58;
            this.chk_treasure_game_use_ticket.Text = "使用入场券, 开启类型";
            this.chk_treasure_game_use_ticket.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(28, 223);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(113, 12);
            this.label35.TabIndex = 57;
            this.label35.Text = "金币走到宝箱类型：";
            // 
            // txt_treasure_game_goldmove_boxtype
            // 
            this.txt_treasure_game_goldmove_boxtype.Location = new System.Drawing.Point(147, 218);
            this.txt_treasure_game_goldmove_boxtype.Name = "txt_treasure_game_goldmove_boxtype";
            this.txt_treasure_game_goldmove_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_treasure_game_goldmove_boxtype.TabIndex = 56;
            this.tip_msg.SetToolTip(this.txt_treasure_game_goldmove_boxtype, "0=兵器宝箱, 1=1星,2=2星,3=3星,4=摇钱树,5=超级门票,6=200级驿站后的行动力, 哪项不要就不要填");
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(28, 196);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(89, 12);
            this.label36.TabIndex = 55;
            this.label36.Text = "打开宝箱类型：";
            // 
            // txt_treasure_game_boxtype
            // 
            this.txt_treasure_game_boxtype.Location = new System.Drawing.Point(147, 191);
            this.txt_treasure_game_boxtype.Name = "txt_treasure_game_boxtype";
            this.txt_treasure_game_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_treasure_game_boxtype.TabIndex = 54;
            this.tip_msg.SetToolTip(this.txt_treasure_game_boxtype, "2=2星,3=3星,哪项不要就不要填, 兵器和1星不要钱,自动开");
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(28, 96);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(113, 12);
            this.label34.TabIndex = 53;
            this.label34.Text = "金币走到宝箱类型：";
            // 
            // txt_daily_treasure_game_goldmove_boxtype
            // 
            this.txt_daily_treasure_game_goldmove_boxtype.Location = new System.Drawing.Point(147, 88);
            this.txt_daily_treasure_game_goldmove_boxtype.Name = "txt_daily_treasure_game_goldmove_boxtype";
            this.txt_daily_treasure_game_goldmove_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_daily_treasure_game_goldmove_boxtype.TabIndex = 52;
            this.tip_msg.SetToolTip(this.txt_daily_treasure_game_goldmove_boxtype, "0=兵器宝箱, 1=1星,2=2星,3=3星,4=摇钱树,6=200级驿站后的行动力,哪项不要就不要填");
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(28, 72);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(89, 12);
            this.label33.TabIndex = 51;
            this.label33.Text = "打开宝箱类型：";
            // 
            // txt_daily_treasure_game_boxtype
            // 
            this.txt_daily_treasure_game_boxtype.Location = new System.Drawing.Point(147, 62);
            this.txt_daily_treasure_game_boxtype.Name = "txt_daily_treasure_game_boxtype";
            this.txt_daily_treasure_game_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_daily_treasure_game_boxtype.TabIndex = 50;
            this.tip_msg.SetToolTip(this.txt_daily_treasure_game_boxtype, "2=2星,3=3星,哪项不要就不要填, 兵器和1星不要钱,自动开");
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(25, 248);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(149, 12);
            this.label27.TabIndex = 49;
            this.label27.Text = "距离宝箱几步开始金币前进";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(25, 118);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(149, 12);
            this.label26.TabIndex = 48;
            this.label26.Text = "距离宝箱几步开始金币前进";
            // 
            // num_treasure_game_goldstep
            // 
            this.num_treasure_game_goldstep.Location = new System.Drawing.Point(180, 245);
            this.num_treasure_game_goldstep.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_treasure_game_goldstep.Name = "num_treasure_game_goldstep";
            this.num_treasure_game_goldstep.Size = new System.Drawing.Size(45, 21);
            this.num_treasure_game_goldstep.TabIndex = 47;
            // 
            // num_daily_treasure_game_goldstep
            // 
            this.num_daily_treasure_game_goldstep.Location = new System.Drawing.Point(180, 113);
            this.num_daily_treasure_game_goldstep.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_daily_treasure_game_goldstep.Name = "num_daily_treasure_game_goldstep";
            this.num_daily_treasure_game_goldstep.Size = new System.Drawing.Size(45, 21);
            this.num_daily_treasure_game_goldstep.TabIndex = 46;
            // 
            // chk_treasure_game_enable
            // 
            this.chk_treasure_game_enable.AutoSize = true;
            this.chk_treasure_game_enable.Location = new System.Drawing.Point(6, 171);
            this.chk_treasure_game_enable.Name = "chk_treasure_game_enable";
            this.chk_treasure_game_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_treasure_game_enable.TabIndex = 45;
            this.chk_treasure_game_enable.Text = "古城寻宝";
            this.chk_treasure_game_enable.UseVisualStyleBackColor = true;
            // 
            // chk_daily_treasure_game
            // 
            this.chk_daily_treasure_game.AutoSize = true;
            this.chk_daily_treasure_game.Location = new System.Drawing.Point(6, 20);
            this.chk_daily_treasure_game.Name = "chk_daily_treasure_game";
            this.chk_daily_treasure_game.Size = new System.Drawing.Size(72, 16);
            this.chk_daily_treasure_game.TabIndex = 44;
            this.chk_daily_treasure_game.Text = "日常探宝";
            this.chk_daily_treasure_game.UseVisualStyleBackColor = true;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.combo_tower_type);
            this.groupBox16.Controls.Add(this.chk_tower_enable);
            this.groupBox16.Controls.Add(this.chk_world_army_openbox);
            this.groupBox16.Controls.Add(this.chk_festival_event);
            this.groupBox16.Controls.Add(this.combo_world_army_refresh_type);
            this.groupBox16.Controls.Add(this.chk_world_army_buybox2);
            this.groupBox16.Controls.Add(this.chk_world_army_buybox1);
            this.groupBox16.Controls.Add(this.chk_world_army_enable);
            this.groupBox16.Controls.Add(this.combo_world_army_boxtype);
            this.groupBox16.Location = new System.Drawing.Point(539, 10);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(291, 136);
            this.groupBox16.TabIndex = 48;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "世界军团/迎军活动";
            // 
            // combo_tower_type
            // 
            this.combo_tower_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_tower_type.FormattingEnabled = true;
            this.combo_tower_type.Items.AddRange(new object[] {
            "青玉百灵塔(1万)",
            "黄玉玲珑塔(3万)",
            "墨月麒麟塔(6万)",
            "紫灵至尊塔(12万)"});
            this.combo_tower_type.Location = new System.Drawing.Point(162, 104);
            this.combo_tower_type.Name = "combo_tower_type";
            this.combo_tower_type.Size = new System.Drawing.Size(113, 20);
            this.combo_tower_type.TabIndex = 50;
            // 
            // chk_tower_enable
            // 
            this.chk_tower_enable.AutoSize = true;
            this.chk_tower_enable.Location = new System.Drawing.Point(6, 110);
            this.chk_tower_enable.Name = "chk_tower_enable";
            this.chk_tower_enable.Size = new System.Drawing.Size(156, 16);
            this.chk_tower_enable.TabIndex = 49;
            this.chk_tower_enable.Text = "自动宝塔活动, 选择宝塔";
            this.chk_tower_enable.UseVisualStyleBackColor = true;
            // 
            // chk_world_army_openbox
            // 
            this.chk_world_army_openbox.AutoSize = true;
            this.chk_world_army_openbox.Location = new System.Drawing.Point(28, 42);
            this.chk_world_army_openbox.Name = "chk_world_army_openbox";
            this.chk_world_army_openbox.Size = new System.Drawing.Size(84, 16);
            this.chk_world_army_openbox.TabIndex = 48;
            this.chk_world_army_openbox.Text = "自动开红包";
            this.chk_world_army_openbox.UseVisualStyleBackColor = true;
            // 
            // chk_festival_event
            // 
            this.chk_festival_event.AutoSize = true;
            this.chk_festival_event.Location = new System.Drawing.Point(6, 84);
            this.chk_festival_event.Name = "chk_festival_event";
            this.chk_festival_event.Size = new System.Drawing.Size(96, 16);
            this.chk_festival_event.TabIndex = 47;
            this.chk_festival_event.Text = "自动迎军活动";
            this.chk_festival_event.UseVisualStyleBackColor = true;
            // 
            // combo_world_army_refresh_type
            // 
            this.combo_world_army_refresh_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_world_army_refresh_type.FormattingEnabled = true;
            this.combo_world_army_refresh_type.Items.AddRange(new object[] {
            "啥都不升级",
            "精英升到首领",
            "普通升到首领"});
            this.combo_world_army_refresh_type.Location = new System.Drawing.Point(196, 16);
            this.combo_world_army_refresh_type.Name = "combo_world_army_refresh_type";
            this.combo_world_army_refresh_type.Size = new System.Drawing.Size(85, 20);
            this.combo_world_army_refresh_type.TabIndex = 46;
            // 
            // chk_world_army_buybox2
            // 
            this.chk_world_army_buybox2.AutoSize = true;
            this.chk_world_army_buybox2.Location = new System.Drawing.Point(130, 63);
            this.chk_world_army_buybox2.Name = "chk_world_army_buybox2";
            this.chk_world_army_buybox2.Size = new System.Drawing.Size(96, 16);
            this.chk_world_army_buybox2.TabIndex = 44;
            this.chk_world_army_buybox2.Text = "购买打败奖励";
            this.chk_world_army_buybox2.UseVisualStyleBackColor = true;
            // 
            // chk_world_army_buybox1
            // 
            this.chk_world_army_buybox1.AutoSize = true;
            this.chk_world_army_buybox1.Location = new System.Drawing.Point(28, 63);
            this.chk_world_army_buybox1.Name = "chk_world_army_buybox1";
            this.chk_world_army_buybox1.Size = new System.Drawing.Size(96, 16);
            this.chk_world_army_buybox1.TabIndex = 43;
            this.chk_world_army_buybox1.Text = "购买排名奖励";
            this.chk_world_army_buybox1.UseVisualStyleBackColor = true;
            // 
            // chk_world_army_enable
            // 
            this.chk_world_army_enable.AutoSize = true;
            this.chk_world_army_enable.Location = new System.Drawing.Point(6, 20);
            this.chk_world_army_enable.Name = "chk_world_army_enable";
            this.chk_world_army_enable.Size = new System.Drawing.Size(192, 16);
            this.chk_world_army_enable.TabIndex = 39;
            this.chk_world_army_enable.Text = "自动世界军团, 升级部队类型：";
            this.chk_world_army_enable.UseVisualStyleBackColor = true;
            // 
            // combo_world_army_boxtype
            // 
            this.combo_world_army_boxtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_world_army_boxtype.FormattingEnabled = true;
            this.combo_world_army_boxtype.Items.AddRange(new object[] {
            "免费",
            "2倍",
            "4倍",
            "10倍"});
            this.combo_world_army_boxtype.Location = new System.Drawing.Point(115, 38);
            this.combo_world_army_boxtype.Name = "combo_world_army_boxtype";
            this.combo_world_army_boxtype.Size = new System.Drawing.Size(56, 20);
            this.combo_world_army_boxtype.TabIndex = 42;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label65);
            this.groupBox15.Controls.Add(this.label64);
            this.groupBox15.Controls.Add(this.nm_Out);
            this.groupBox15.Controls.Add(this.chk_gemdump_enable);
            this.groupBox15.Controls.Add(this.num_troop_turntable_buygold);
            this.groupBox15.Controls.Add(this.label46);
            this.groupBox15.Controls.Add(this.combo_troop_turntable_type);
            this.groupBox15.Controls.Add(this.chk_troop_turntable_goldratio);
            this.groupBox15.Controls.Add(this.chk_cake_event);
            this.groupBox15.Controls.Add(this.chk_troop_turntable);
            this.groupBox15.Controls.Add(this.chk_super_fanpai);
            this.groupBox15.Controls.Add(this.label39);
            this.groupBox15.Controls.Add(this.chk_troop_feedback_openbox);
            this.groupBox15.Controls.Add(this.combo_troop_feedback_opentype);
            this.groupBox15.Controls.Add(this.chk_troop_feedback_opentreasure);
            this.groupBox15.Controls.Add(this.chk_troop_feedback_doubleweapon);
            this.groupBox15.Controls.Add(this.chk_troop_feedback_refine_notired);
            this.groupBox15.Controls.Add(this.chk_troop_feedback_enable);
            this.groupBox15.Controls.Add(this.chk_gem_flop_enable);
            this.groupBox15.Controls.Add(this.num_gem_flop_upgrade_count);
            this.groupBox15.Controls.Add(this.chk_silver_flop_enable);
            this.groupBox15.Location = new System.Drawing.Point(8, 279);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(281, 322);
            this.groupBox15.TabIndex = 47;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "各种翻牌/兵器回馈/兵器转盘";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(142, 253);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(77, 12);
            this.label65.TabIndex = 68;
            this.label65.Text = "使用免费加倍";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(23, 253);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(65, 12);
            this.label64.TabIndex = 67;
            this.label64.Text = "外圈个数≥";
            // 
            // nm_Out
            // 
            this.nm_Out.Location = new System.Drawing.Point(91, 247);
            this.nm_Out.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nm_Out.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_Out.Name = "nm_Out";
            this.nm_Out.Size = new System.Drawing.Size(45, 21);
            this.nm_Out.TabIndex = 61;
            this.nm_Out.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chk_gemdump_enable
            // 
            this.chk_gemdump_enable.AutoSize = true;
            this.chk_gemdump_enable.Location = new System.Drawing.Point(7, 292);
            this.chk_gemdump_enable.Name = "chk_gemdump_enable";
            this.chk_gemdump_enable.Size = new System.Drawing.Size(144, 16);
            this.chk_gemdump_enable.TabIndex = 60;
            this.chk_gemdump_enable.Text = "宝石倾销点券兑换宝石";
            this.chk_gemdump_enable.UseVisualStyleBackColor = true;
            // 
            // num_troop_turntable_buygold
            // 
            this.num_troop_turntable_buygold.Location = new System.Drawing.Point(124, 226);
            this.num_troop_turntable_buygold.Name = "num_troop_turntable_buygold";
            this.num_troop_turntable_buygold.Size = new System.Drawing.Size(51, 21);
            this.num_troop_turntable_buygold.TabIndex = 59;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(23, 231);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(101, 12);
            this.label46.TabIndex = 58;
            this.label46.Text = "买次数若金币≤：";
            // 
            // combo_troop_turntable_type
            // 
            this.combo_troop_turntable_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_troop_turntable_type.FormattingEnabled = true;
            this.combo_troop_turntable_type.Items.AddRange(new object[] {
            "无敌将军炮",
            "五毒问心钉",
            "玄霆角",
            "七戮锋",
            "蟠龙华盖",
            "轩辕指南车",
            "落魂冥灯",
            "红衣将军炮",
            "化骨丛棘",
            "彤云角",
            "弑履岩",
            "玉麟旌麾",
            "化骨悬灯",
            "铜人计里车"});
            this.combo_troop_turntable_type.Location = new System.Drawing.Point(134, 204);
            this.combo_troop_turntable_type.Name = "combo_troop_turntable_type";
            this.combo_troop_turntable_type.Size = new System.Drawing.Size(100, 20);
            this.combo_troop_turntable_type.TabIndex = 57;
            // 
            // chk_troop_turntable_goldratio
            // 
            this.chk_troop_turntable_goldratio.AutoSize = true;
            this.chk_troop_turntable_goldratio.Location = new System.Drawing.Point(177, 228);
            this.chk_troop_turntable_goldratio.Name = "chk_troop_turntable_goldratio";
            this.chk_troop_turntable_goldratio.Size = new System.Drawing.Size(96, 16);
            this.chk_troop_turntable_goldratio.TabIndex = 56;
            this.chk_troop_turntable_goldratio.Text = "金币增加倍率";
            this.chk_troop_turntable_goldratio.UseVisualStyleBackColor = true;
            // 
            // chk_cake_event
            // 
            this.chk_cake_event.AutoSize = true;
            this.chk_cake_event.Location = new System.Drawing.Point(6, 270);
            this.chk_cake_event.Name = "chk_cake_event";
            this.chk_cake_event.Size = new System.Drawing.Size(96, 16);
            this.chk_cake_event.TabIndex = 55;
            this.chk_cake_event.Text = "自动月饼活动";
            this.chk_cake_event.UseVisualStyleBackColor = true;
            // 
            // chk_troop_turntable
            // 
            this.chk_troop_turntable.AutoSize = true;
            this.chk_troop_turntable.Location = new System.Drawing.Point(6, 207);
            this.chk_troop_turntable.Name = "chk_troop_turntable";
            this.chk_troop_turntable.Size = new System.Drawing.Size(126, 16);
            this.chk_troop_turntable.TabIndex = 54;
            this.chk_troop_turntable.Text = "自动兵器转盘,选择";
            this.chk_troop_turntable.UseVisualStyleBackColor = true;
            // 
            // chk_super_fanpai
            // 
            this.chk_super_fanpai.AutoSize = true;
            this.chk_super_fanpai.Location = new System.Drawing.Point(6, 66);
            this.chk_super_fanpai.Name = "chk_super_fanpai";
            this.chk_super_fanpai.Size = new System.Drawing.Size(96, 16);
            this.chk_super_fanpai.TabIndex = 53;
            this.chk_super_fanpai.Text = "自动超级翻牌";
            this.chk_super_fanpai.UseVisualStyleBackColor = true;
            // 
            // label39
            // 
            this.label39.ForeColor = System.Drawing.Color.Brown;
            this.label39.Location = new System.Drawing.Point(22, 89);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(238, 30);
            this.label39.TabIndex = 52;
            this.label39.Text = "注意：宝石翻牌是否升级和超级翻牌是否全开由全局设置的宝石性价比决定";
            // 
            // chk_troop_feedback_openbox
            // 
            this.chk_troop_feedback_openbox.AutoSize = true;
            this.chk_troop_feedback_openbox.Location = new System.Drawing.Point(24, 185);
            this.chk_troop_feedback_openbox.Name = "chk_troop_feedback_openbox";
            this.chk_troop_feedback_openbox.Size = new System.Drawing.Size(108, 16);
            this.chk_troop_feedback_openbox.TabIndex = 44;
            this.chk_troop_feedback_openbox.Text = "自动开奖励宝箱";
            this.chk_troop_feedback_openbox.UseVisualStyleBackColor = true;
            // 
            // combo_troop_feedback_opentype
            // 
            this.combo_troop_feedback_opentype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_troop_feedback_opentype.FormattingEnabled = true;
            this.combo_troop_feedback_opentype.Items.AddRange(new object[] {
            "免费开启",
            "四倍开启"});
            this.combo_troop_feedback_opentype.Location = new System.Drawing.Point(135, 181);
            this.combo_troop_feedback_opentype.Name = "combo_troop_feedback_opentype";
            this.combo_troop_feedback_opentype.Size = new System.Drawing.Size(87, 20);
            this.combo_troop_feedback_opentype.TabIndex = 43;
            // 
            // chk_troop_feedback_opentreasure
            // 
            this.chk_troop_feedback_opentreasure.AutoSize = true;
            this.chk_troop_feedback_opentreasure.Location = new System.Drawing.Point(24, 165);
            this.chk_troop_feedback_opentreasure.Name = "chk_troop_feedback_opentreasure";
            this.chk_troop_feedback_opentreasure.Size = new System.Drawing.Size(246, 16);
            this.chk_troop_feedback_opentreasure.TabIndex = 42;
            this.chk_troop_feedback_opentreasure.Text = "自动开完世界宝箱(军令>40后不会继续开)";
            this.chk_troop_feedback_opentreasure.UseVisualStyleBackColor = true;
            // 
            // chk_troop_feedback_doubleweapon
            // 
            this.chk_troop_feedback_doubleweapon.AutoSize = true;
            this.chk_troop_feedback_doubleweapon.Location = new System.Drawing.Point(150, 143);
            this.chk_troop_feedback_doubleweapon.Name = "chk_troop_feedback_doubleweapon";
            this.chk_troop_feedback_doubleweapon.Size = new System.Drawing.Size(120, 16);
            this.chk_troop_feedback_doubleweapon.TabIndex = 41;
            this.chk_troop_feedback_doubleweapon.Text = "自动双倍兵器碎片";
            this.chk_troop_feedback_doubleweapon.UseVisualStyleBackColor = true;
            // 
            // chk_troop_feedback_refine_notired
            // 
            this.chk_troop_feedback_refine_notired.AutoSize = true;
            this.chk_troop_feedback_refine_notired.Location = new System.Drawing.Point(24, 143);
            this.chk_troop_feedback_refine_notired.Name = "chk_troop_feedback_refine_notired";
            this.chk_troop_feedback_refine_notired.Size = new System.Drawing.Size(108, 16);
            this.chk_troop_feedback_refine_notired.TabIndex = 40;
            this.chk_troop_feedback_refine_notired.Text = "自动炼制无疲劳";
            this.chk_troop_feedback_refine_notired.UseVisualStyleBackColor = true;
            // 
            // chk_troop_feedback_enable
            // 
            this.chk_troop_feedback_enable.AutoSize = true;
            this.chk_troop_feedback_enable.Location = new System.Drawing.Point(6, 122);
            this.chk_troop_feedback_enable.Name = "chk_troop_feedback_enable";
            this.chk_troop_feedback_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_troop_feedback_enable.TabIndex = 39;
            this.chk_troop_feedback_enable.Text = "兵器回馈";
            this.chk_troop_feedback_enable.UseVisualStyleBackColor = true;
            // 
            // chk_gem_flop_enable
            // 
            this.chk_gem_flop_enable.AutoSize = true;
            this.chk_gem_flop_enable.Location = new System.Drawing.Point(6, 41);
            this.chk_gem_flop_enable.Name = "chk_gem_flop_enable";
            this.chk_gem_flop_enable.Size = new System.Drawing.Size(180, 16);
            this.chk_gem_flop_enable.TabIndex = 37;
            this.chk_gem_flop_enable.Text = "自动宝石翻牌, 最多升级次数";
            this.chk_gem_flop_enable.UseVisualStyleBackColor = true;
            // 
            // num_gem_flop_upgrade_count
            // 
            this.num_gem_flop_upgrade_count.Location = new System.Drawing.Point(192, 36);
            this.num_gem_flop_upgrade_count.Name = "num_gem_flop_upgrade_count";
            this.num_gem_flop_upgrade_count.Size = new System.Drawing.Size(51, 21);
            this.num_gem_flop_upgrade_count.TabIndex = 38;
            // 
            // chk_silver_flop_enable
            // 
            this.chk_silver_flop_enable.AutoSize = true;
            this.chk_silver_flop_enable.Location = new System.Drawing.Point(6, 19);
            this.chk_silver_flop_enable.Name = "chk_silver_flop_enable";
            this.chk_silver_flop_enable.Size = new System.Drawing.Size(96, 16);
            this.chk_silver_flop_enable.TabIndex = 36;
            this.chk_silver_flop_enable.Text = "自动银币翻牌";
            this.chk_silver_flop_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.txt_arch_upgrade);
            this.groupBox12.Controls.Add(this.label25);
            this.groupBox12.Controls.Add(this.chk_arch_create);
            this.groupBox12.Controls.Add(this.txt_giftevent_serial);
            this.groupBox12.Controls.Add(this.chk_giftevent_enable);
            this.groupBox12.Controls.Add(this.chk_arch_enable);
            this.groupBox12.Location = new System.Drawing.Point(293, 10);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(240, 136);
            this.groupBox12.TabIndex = 44;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "考古/犒赏";
            // 
            // txt_arch_upgrade
            // 
            this.txt_arch_upgrade.Location = new System.Drawing.Point(119, 58);
            this.txt_arch_upgrade.Name = "txt_arch_upgrade";
            this.txt_arch_upgrade.Size = new System.Drawing.Size(113, 21);
            this.txt_arch_upgrade.TabIndex = 48;
            this.tip_msg.SetToolTip(this.txt_arch_upgrade, "1=白色,2=蓝色,3=绿色,4=黄色,5=红色, 如果什么都不升级就不要填");
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(24, 64);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(89, 12);
            this.label25.TabIndex = 47;
            this.label25.Text = "升级紫色类型：";
            // 
            // chk_arch_create
            // 
            this.chk_arch_create.AutoSize = true;
            this.chk_arch_create.Location = new System.Drawing.Point(6, 42);
            this.chk_arch_create.Name = "chk_arch_create";
            this.chk_arch_create.Size = new System.Drawing.Size(132, 16);
            this.chk_arch_create.TabIndex = 38;
            this.chk_arch_create.Text = "我要自己建考古队伍";
            this.chk_arch_create.UseVisualStyleBackColor = true;
            // 
            // txt_giftevent_serial
            // 
            this.txt_giftevent_serial.Location = new System.Drawing.Point(189, 105);
            this.txt_giftevent_serial.Name = "txt_giftevent_serial";
            this.txt_giftevent_serial.Size = new System.Drawing.Size(43, 21);
            this.txt_giftevent_serial.TabIndex = 34;
            this.txt_giftevent_serial.Text = "231";
            this.tip_msg.SetToolTip(this.txt_giftevent_serial, "银币=1,兵器=2,宝石=3,如先宝石再兵器再银币,设置为321,如果某项不要,则不写,如只要兵器,那么设置为2");
            // 
            // chk_giftevent_enable
            // 
            this.chk_giftevent_enable.AutoSize = true;
            this.chk_giftevent_enable.Location = new System.Drawing.Point(6, 110);
            this.chk_giftevent_enable.Name = "chk_giftevent_enable";
            this.chk_giftevent_enable.Size = new System.Drawing.Size(180, 16);
            this.chk_giftevent_enable.TabIndex = 32;
            this.chk_giftevent_enable.Text = "自动犒赏活动,兑换物品顺序:";
            this.chk_giftevent_enable.UseVisualStyleBackColor = true;
            // 
            // chk_arch_enable
            // 
            this.chk_arch_enable.AutoSize = true;
            this.chk_arch_enable.Location = new System.Drawing.Point(6, 20);
            this.chk_arch_enable.Name = "chk_arch_enable";
            this.chk_arch_enable.Size = new System.Drawing.Size(72, 16);
            this.chk_arch_enable.TabIndex = 33;
            this.chk_arch_enable.Text = "自动考古";
            this.chk_arch_enable.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.chk_again);
            this.groupBox11.Controls.Add(this.combo_kfwd_openbox_type);
            this.groupBox11.Controls.Add(this.chk_kfwd_openbox);
            this.groupBox11.Controls.Add(this.combo_kfzb_market_weapon_pos_2);
            this.groupBox11.Controls.Add(this.chk_kfzb_market_weapon_2);
            this.groupBox11.Controls.Add(this.chk_kfzb_market_ignore);
            this.groupBox11.Controls.Add(this.combo_kfzb_market_weapon_pos_1);
            this.groupBox11.Controls.Add(this.combo_kfzb_market_gem_pos);
            this.groupBox11.Controls.Add(this.combo_kfzb_market_stone_pos);
            this.groupBox11.Controls.Add(this.combo_kfzb_market_silver_pos);
            this.groupBox11.Controls.Add(this.chk_kfzb_market_weapon_1);
            this.groupBox11.Controls.Add(this.chk_kfzb_market_gem);
            this.groupBox11.Controls.Add(this.chk_kfzb_market_stone);
            this.groupBox11.Controls.Add(this.chk_kfzb_market_silver);
            this.groupBox11.Controls.Add(this.chk_kfzb_market);
            this.groupBox11.Controls.Add(this.num_crossplatm_compete_gold);
            this.groupBox11.Controls.Add(this.num_kf_banquet_buygold);
            this.groupBox11.Controls.Add(this.chk_kf_banquet_nation);
            this.groupBox11.Controls.Add(this.chk_crossplatm_compete);
            this.groupBox11.Controls.Add(this.chk_kf_banquet_enabled);
            this.groupBox11.Controls.Add(this.chk_kfwd_enable);
            this.groupBox11.Controls.Add(this.chk_kfwd_buyreward);
            this.groupBox11.Location = new System.Drawing.Point(8, 10);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(279, 264);
            this.groupBox11.TabIndex = 43;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "武斗会/争霸赛/盛宴";
            // 
            // chk_again
            // 
            this.chk_again.AutoSize = true;
            this.chk_again.Location = new System.Drawing.Point(201, 244);
            this.chk_again.Name = "chk_again";
            this.chk_again.Size = new System.Drawing.Size(72, 16);
            this.chk_again.TabIndex = 63;
            this.chk_again.Text = "再喝一杯";
            this.chk_again.UseVisualStyleBackColor = true;
            // 
            // combo_kfwd_openbox_type
            // 
            this.combo_kfwd_openbox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfwd_openbox_type.FormattingEnabled = true;
            this.combo_kfwd_openbox_type.Items.AddRange(new object[] {
            "免费",
            "2倍",
            "4倍",
            "10倍"});
            this.combo_kfwd_openbox_type.Location = new System.Drawing.Point(138, 36);
            this.combo_kfwd_openbox_type.Name = "combo_kfwd_openbox_type";
            this.combo_kfwd_openbox_type.Size = new System.Drawing.Size(56, 20);
            this.combo_kfwd_openbox_type.TabIndex = 62;
            // 
            // chk_kfwd_openbox
            // 
            this.chk_kfwd_openbox.AutoSize = true;
            this.chk_kfwd_openbox.Location = new System.Drawing.Point(30, 40);
            this.chk_kfwd_openbox.Name = "chk_kfwd_openbox";
            this.chk_kfwd_openbox.Size = new System.Drawing.Size(108, 16);
            this.chk_kfwd_openbox.TabIndex = 61;
            this.chk_kfwd_openbox.Text = "开启武斗会宝箱";
            this.chk_kfwd_openbox.UseVisualStyleBackColor = true;
            // 
            // combo_kfzb_market_weapon_pos_2
            // 
            this.combo_kfzb_market_weapon_pos_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfzb_market_weapon_pos_2.FormattingEnabled = true;
            this.combo_kfzb_market_weapon_pos_2.Items.AddRange(new object[] {
            "第1列",
            "第2列以上",
            "第3列以上",
            "第4列以上",
            "第5列以上"});
            this.combo_kfzb_market_weapon_pos_2.Location = new System.Drawing.Point(138, 195);
            this.combo_kfzb_market_weapon_pos_2.Name = "combo_kfzb_market_weapon_pos_2";
            this.combo_kfzb_market_weapon_pos_2.Size = new System.Drawing.Size(82, 20);
            this.combo_kfzb_market_weapon_pos_2.TabIndex = 60;
            // 
            // chk_kfzb_market_weapon_2
            // 
            this.chk_kfzb_market_weapon_2.AutoSize = true;
            this.chk_kfzb_market_weapon_2.Location = new System.Drawing.Point(30, 199);
            this.chk_kfzb_market_weapon_2.Name = "chk_kfzb_market_weapon_2";
            this.chk_kfzb_market_weapon_2.Size = new System.Drawing.Size(108, 16);
            this.chk_kfzb_market_weapon_2.TabIndex = 59;
            this.chk_kfzb_market_weapon_2.Text = "兵器(钉锋车灯)";
            this.chk_kfzb_market_weapon_2.UseVisualStyleBackColor = true;
            // 
            // chk_kfzb_market_ignore
            // 
            this.chk_kfzb_market_ignore.AutoSize = true;
            this.chk_kfzb_market_ignore.Location = new System.Drawing.Point(113, 85);
            this.chk_kfzb_market_ignore.Name = "chk_kfzb_market_ignore";
            this.chk_kfzb_market_ignore.Size = new System.Drawing.Size(144, 16);
            this.chk_kfzb_market_ignore.TabIndex = 58;
            this.chk_kfzb_market_ignore.Text = "争霸商城忽略金币下限";
            this.tip_msg.SetToolTip(this.chk_kfzb_market_ignore, "级别高+6分, 攻方+2分, 每次复仇+1分, 支持分高的, 如果分一样则支持攻方");
            this.chk_kfzb_market_ignore.UseVisualStyleBackColor = true;
            // 
            // combo_kfzb_market_weapon_pos_1
            // 
            this.combo_kfzb_market_weapon_pos_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfzb_market_weapon_pos_1.FormattingEnabled = true;
            this.combo_kfzb_market_weapon_pos_1.Items.AddRange(new object[] {
            "第1列",
            "第2列以上",
            "第3列以上",
            "第4列以上",
            "第5列以上"});
            this.combo_kfzb_market_weapon_pos_1.Location = new System.Drawing.Point(138, 173);
            this.combo_kfzb_market_weapon_pos_1.Name = "combo_kfzb_market_weapon_pos_1";
            this.combo_kfzb_market_weapon_pos_1.Size = new System.Drawing.Size(82, 20);
            this.combo_kfzb_market_weapon_pos_1.TabIndex = 53;
            // 
            // combo_kfzb_market_gem_pos
            // 
            this.combo_kfzb_market_gem_pos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfzb_market_gem_pos.FormattingEnabled = true;
            this.combo_kfzb_market_gem_pos.Items.AddRange(new object[] {
            "第1列",
            "第2列以上",
            "第3列以上",
            "第4列以上",
            "第5列以上"});
            this.combo_kfzb_market_gem_pos.Location = new System.Drawing.Point(138, 150);
            this.combo_kfzb_market_gem_pos.Name = "combo_kfzb_market_gem_pos";
            this.combo_kfzb_market_gem_pos.Size = new System.Drawing.Size(82, 20);
            this.combo_kfzb_market_gem_pos.TabIndex = 52;
            // 
            // combo_kfzb_market_stone_pos
            // 
            this.combo_kfzb_market_stone_pos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfzb_market_stone_pos.FormattingEnabled = true;
            this.combo_kfzb_market_stone_pos.Items.AddRange(new object[] {
            "第1列",
            "第2列以上",
            "第3列以上",
            "第4列以上",
            "第5列以上"});
            this.combo_kfzb_market_stone_pos.Location = new System.Drawing.Point(138, 128);
            this.combo_kfzb_market_stone_pos.Name = "combo_kfzb_market_stone_pos";
            this.combo_kfzb_market_stone_pos.Size = new System.Drawing.Size(82, 20);
            this.combo_kfzb_market_stone_pos.TabIndex = 51;
            // 
            // combo_kfzb_market_silver_pos
            // 
            this.combo_kfzb_market_silver_pos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfzb_market_silver_pos.FormattingEnabled = true;
            this.combo_kfzb_market_silver_pos.Items.AddRange(new object[] {
            "第1列",
            "第2列以上",
            "第3列以上",
            "第4列以上",
            "第5列以上"});
            this.combo_kfzb_market_silver_pos.Location = new System.Drawing.Point(138, 104);
            this.combo_kfzb_market_silver_pos.Name = "combo_kfzb_market_silver_pos";
            this.combo_kfzb_market_silver_pos.Size = new System.Drawing.Size(82, 20);
            this.combo_kfzb_market_silver_pos.TabIndex = 50;
            // 
            // chk_kfzb_market_weapon_1
            // 
            this.chk_kfzb_market_weapon_1.AutoSize = true;
            this.chk_kfzb_market_weapon_1.Location = new System.Drawing.Point(30, 177);
            this.chk_kfzb_market_weapon_1.Name = "chk_kfzb_market_weapon_1";
            this.chk_kfzb_market_weapon_1.Size = new System.Drawing.Size(96, 16);
            this.chk_kfzb_market_weapon_1.TabIndex = 49;
            this.chk_kfzb_market_weapon_1.Text = "兵器(炮角旗)";
            this.chk_kfzb_market_weapon_1.UseVisualStyleBackColor = true;
            // 
            // chk_kfzb_market_gem
            // 
            this.chk_kfzb_market_gem.AutoSize = true;
            this.chk_kfzb_market_gem.Location = new System.Drawing.Point(30, 154);
            this.chk_kfzb_market_gem.Name = "chk_kfzb_market_gem";
            this.chk_kfzb_market_gem.Size = new System.Drawing.Size(48, 16);
            this.chk_kfzb_market_gem.TabIndex = 48;
            this.chk_kfzb_market_gem.Text = "宝石";
            this.chk_kfzb_market_gem.UseVisualStyleBackColor = true;
            // 
            // chk_kfzb_market_stone
            // 
            this.chk_kfzb_market_stone.AutoSize = true;
            this.chk_kfzb_market_stone.Location = new System.Drawing.Point(30, 132);
            this.chk_kfzb_market_stone.Name = "chk_kfzb_market_stone";
            this.chk_kfzb_market_stone.Size = new System.Drawing.Size(48, 16);
            this.chk_kfzb_market_stone.TabIndex = 47;
            this.chk_kfzb_market_stone.Text = "玉石";
            this.chk_kfzb_market_stone.UseVisualStyleBackColor = true;
            // 
            // chk_kfzb_market_silver
            // 
            this.chk_kfzb_market_silver.AutoSize = true;
            this.chk_kfzb_market_silver.Location = new System.Drawing.Point(30, 108);
            this.chk_kfzb_market_silver.Name = "chk_kfzb_market_silver";
            this.chk_kfzb_market_silver.Size = new System.Drawing.Size(48, 16);
            this.chk_kfzb_market_silver.TabIndex = 46;
            this.chk_kfzb_market_silver.Text = "银币";
            this.chk_kfzb_market_silver.UseVisualStyleBackColor = true;
            // 
            // chk_kfzb_market
            // 
            this.chk_kfzb_market.AutoSize = true;
            this.chk_kfzb_market.Location = new System.Drawing.Point(6, 86);
            this.chk_kfzb_market.Name = "chk_kfzb_market";
            this.chk_kfzb_market.Size = new System.Drawing.Size(72, 16);
            this.chk_kfzb_market.TabIndex = 45;
            this.chk_kfzb_market.Text = "争霸商城";
            this.tip_msg.SetToolTip(this.chk_kfzb_market, "级别高+6分, 攻方+2分, 每次复仇+1分, 支持分高的, 如果分一样则支持攻方");
            this.chk_kfzb_market.UseVisualStyleBackColor = true;
            // 
            // num_crossplatm_compete_gold
            // 
            this.num_crossplatm_compete_gold.Location = new System.Drawing.Point(204, 58);
            this.num_crossplatm_compete_gold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_crossplatm_compete_gold.Name = "num_crossplatm_compete_gold";
            this.num_crossplatm_compete_gold.Size = new System.Drawing.Size(44, 21);
            this.num_crossplatm_compete_gold.TabIndex = 44;
            // 
            // num_kf_banquet_buygold
            // 
            this.num_kf_banquet_buygold.Location = new System.Drawing.Point(205, 219);
            this.num_kf_banquet_buygold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_kf_banquet_buygold.Name = "num_kf_banquet_buygold";
            this.num_kf_banquet_buygold.Size = new System.Drawing.Size(44, 21);
            this.num_kf_banquet_buygold.TabIndex = 43;
            // 
            // chk_kf_banquet_nation
            // 
            this.chk_kf_banquet_nation.AutoSize = true;
            this.chk_kf_banquet_nation.Location = new System.Drawing.Point(24, 244);
            this.chk_kf_banquet_nation.Name = "chk_kf_banquet_nation";
            this.chk_kf_banquet_nation.Size = new System.Drawing.Size(180, 16);
            this.chk_kf_banquet_nation.TabIndex = 42;
            this.chk_kf_banquet_nation.Text = "只参加国宴(国王带队的盛宴)";
            this.chk_kf_banquet_nation.UseVisualStyleBackColor = true;
            // 
            // chk_crossplatm_compete
            // 
            this.chk_crossplatm_compete.AutoSize = true;
            this.chk_crossplatm_compete.Location = new System.Drawing.Point(6, 63);
            this.chk_crossplatm_compete.Name = "chk_crossplatm_compete";
            this.chk_crossplatm_compete.Size = new System.Drawing.Size(198, 16);
            this.chk_crossplatm_compete.TabIndex = 31;
            this.chk_crossplatm_compete.Text = "争霸赛献花,如果支持所需金币≤";
            this.tip_msg.SetToolTip(this.chk_crossplatm_compete, "级别高+6分, 攻方+2分, 每次复仇+1分, 支持分高的, 如果分一样则支持攻方");
            this.chk_crossplatm_compete.UseVisualStyleBackColor = true;
            // 
            // chk_kf_banquet_enabled
            // 
            this.chk_kf_banquet_enabled.AutoSize = true;
            this.chk_kf_banquet_enabled.Location = new System.Drawing.Point(7, 224);
            this.chk_kf_banquet_enabled.Name = "chk_kf_banquet_enabled";
            this.chk_kf_banquet_enabled.Size = new System.Drawing.Size(198, 16);
            this.chk_kf_banquet_enabled.TabIndex = 35;
            this.chk_kf_banquet_enabled.Text = "自动盛宴活动,购买邀请券若金≤";
            this.chk_kf_banquet_enabled.UseVisualStyleBackColor = true;
            // 
            // chk_kfwd_enable
            // 
            this.chk_kfwd_enable.AutoSize = true;
            this.chk_kfwd_enable.Location = new System.Drawing.Point(6, 19);
            this.chk_kfwd_enable.Name = "chk_kfwd_enable";
            this.chk_kfwd_enable.Size = new System.Drawing.Size(84, 16);
            this.chk_kfwd_enable.TabIndex = 40;
            this.chk_kfwd_enable.Text = "自动武斗会";
            this.chk_kfwd_enable.UseVisualStyleBackColor = true;
            // 
            // chk_kfwd_buyreward
            // 
            this.chk_kfwd_buyreward.AutoSize = true;
            this.chk_kfwd_buyreward.Location = new System.Drawing.Point(93, 18);
            this.chk_kfwd_buyreward.Name = "chk_kfwd_buyreward";
            this.chk_kfwd_buyreward.Size = new System.Drawing.Size(108, 16);
            this.chk_kfwd_buyreward.TabIndex = 41;
            this.chk_kfwd_buyreward.Text = "购买武斗会宝箱";
            this.chk_kfwd_buyreward.UseVisualStyleBackColor = true;
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.lbl_tempStatus);
            this.logPage.Controls.Add(this.btn_tempStop);
            this.logPage.Controls.Add(this.btn_tempAdd);
            this.logPage.Controls.Add(this.logTemp);
            this.logPage.Controls.Add(this.label19);
            this.logPage.Controls.Add(this.label18);
            this.logPage.Controls.Add(this.logSurpise);
            this.logPage.Controls.Add(this.txt_servers);
            this.logPage.Controls.Add(this.logText);
            this.logPage.Location = new System.Drawing.Point(4, 22);
            this.logPage.Name = "logPage";
            this.logPage.Padding = new System.Windows.Forms.Padding(3);
            this.logPage.Size = new System.Drawing.Size(1052, 607);
            this.logPage.TabIndex = 1;
            this.logPage.Text = "日志";
            this.logPage.UseVisualStyleBackColor = true;
            // 
            // lbl_tempStatus
            // 
            this.lbl_tempStatus.Location = new System.Drawing.Point(571, 526);
            this.lbl_tempStatus.Name = "lbl_tempStatus";
            this.lbl_tempStatus.Size = new System.Drawing.Size(181, 75);
            this.lbl_tempStatus.TabIndex = 8;
            this.lbl_tempStatus.Text = "这里显示正在执行的信息";
            // 
            // btn_tempStop
            // 
            this.btn_tempStop.Enabled = false;
            this.btn_tempStop.Location = new System.Drawing.Point(677, 491);
            this.btn_tempStop.Name = "btn_tempStop";
            this.btn_tempStop.Size = new System.Drawing.Size(75, 23);
            this.btn_tempStop.TabIndex = 7;
            this.btn_tempStop.Text = "停止";
            this.btn_tempStop.UseVisualStyleBackColor = true;
            this.btn_tempStop.Click += new System.EventHandler(this.btn_tempStop_Click);
            // 
            // btn_tempAdd
            // 
            this.btn_tempAdd.Location = new System.Drawing.Point(570, 491);
            this.btn_tempAdd.Name = "btn_tempAdd";
            this.btn_tempAdd.Size = new System.Drawing.Size(101, 23);
            this.btn_tempAdd.TabIndex = 6;
            this.btn_tempAdd.Text = "添加临时任务";
            this.btn_tempAdd.UseVisualStyleBackColor = true;
            this.btn_tempAdd.Click += new System.EventHandler(this.btn_tempAdd_Click);
            // 
            // logTemp
            // 
            this.logTemp.BackColor = System.Drawing.SystemColors.Control;
            this.logTemp.Location = new System.Drawing.Point(3, 491);
            this.logTemp.Name = "logTemp";
            this.logTemp.ReadOnly = true;
            this.logTemp.Size = new System.Drawing.Size(561, 110);
            this.logTemp.TabIndex = 5;
            this.logTemp.Text = "";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.DarkRed;
            this.label19.Location = new System.Drawing.Point(767, 488);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(179, 12);
            this.label19.TabIndex = 4;
            this.label19.Text = "惊喜日志(兵器升级 + 征收获金)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label18.Location = new System.Drawing.Point(765, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(263, 12);
            this.label18.TabIndex = 3;
            this.label18.Text = "各个服务下次工作时间 (按照时间从前到后排序)";
            // 
            // logSurpise
            // 
            this.logSurpise.BackColor = System.Drawing.SystemColors.Control;
            this.logSurpise.Location = new System.Drawing.Point(765, 506);
            this.logSurpise.Name = "logSurpise";
            this.logSurpise.ReadOnly = true;
            this.logSurpise.Size = new System.Drawing.Size(272, 95);
            this.logSurpise.TabIndex = 2;
            this.logSurpise.Text = "";
            // 
            // txt_servers
            // 
            this.txt_servers.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_servers.Location = new System.Drawing.Point(765, 23);
            this.txt_servers.Multiline = true;
            this.txt_servers.Name = "txt_servers";
            this.txt_servers.ReadOnly = true;
            this.txt_servers.Size = new System.Drawing.Size(272, 462);
            this.txt_servers.TabIndex = 1;
            // 
            // logText
            // 
            this.logText.Location = new System.Drawing.Point(0, 0);
            this.logText.Name = "logText";
            this.logText.ReadOnly = true;
            this.logText.Size = new System.Drawing.Size(758, 485);
            this.logText.TabIndex = 0;
            this.logText.Text = "";
            this.logText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.logText_LinkClicked);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_login,
            this.menu_init,
            this.menu_saveSettings,
            this.menu_relogin,
            this.menu_refresh,
            this.menu_startServer,
            this.menu_stopServer,
            this.menu_clearLog,
            this.menu_exit,
            this.menu_test,
            this.menu_more,
            this.menu_about});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1063, 25);
            this.mainMenu.TabIndex = 1;
            // 
            // menu_login
            // 
            this.menu_login.Name = "menu_login";
            this.menu_login.Size = new System.Drawing.Size(44, 21);
            this.menu_login.Text = "登录";
            this.menu_login.Click += new System.EventHandler(this.menu_login_Click);
            // 
            // menu_init
            // 
            this.menu_init.Name = "menu_init";
            this.menu_init.Size = new System.Drawing.Size(68, 21);
            this.menu_init.Text = "读取数据";
            this.menu_init.Click += new System.EventHandler(this.menu_init_Click);
            // 
            // menu_saveSettings
            // 
            this.menu_saveSettings.Name = "menu_saveSettings";
            this.menu_saveSettings.Size = new System.Drawing.Size(68, 21);
            this.menu_saveSettings.Text = "保存设置";
            this.menu_saveSettings.Click += new System.EventHandler(this.menu_saveSettings_Click);
            // 
            // menu_relogin
            // 
            this.menu_relogin.Name = "menu_relogin";
            this.menu_relogin.Size = new System.Drawing.Size(68, 21);
            this.menu_relogin.Text = "重新登录";
            this.menu_relogin.Click += new System.EventHandler(this.menu_relogin_Click);
            // 
            // menu_refresh
            // 
            this.menu_refresh.Name = "menu_refresh";
            this.menu_refresh.Size = new System.Drawing.Size(68, 21);
            this.menu_refresh.Text = "刷新界面";
            this.menu_refresh.Click += new System.EventHandler(this.menu_refresh_Click);
            // 
            // menu_startServer
            // 
            this.menu_startServer.Name = "menu_startServer";
            this.menu_startServer.Size = new System.Drawing.Size(68, 21);
            this.menu_startServer.Text = "开始挂机";
            this.menu_startServer.Click += new System.EventHandler(this.menu_startServer_Click);
            // 
            // menu_stopServer
            // 
            this.menu_stopServer.Name = "menu_stopServer";
            this.menu_stopServer.Size = new System.Drawing.Size(68, 21);
            this.menu_stopServer.Text = "停止挂机";
            this.menu_stopServer.Click += new System.EventHandler(this.menu_stopServer_Click);
            // 
            // menu_clearLog
            // 
            this.menu_clearLog.Name = "menu_clearLog";
            this.menu_clearLog.Size = new System.Drawing.Size(68, 21);
            this.menu_clearLog.Text = "清空日志";
            this.menu_clearLog.Click += new System.EventHandler(this.menu_clearLog_Click);
            // 
            // menu_exit
            // 
            this.menu_exit.Name = "menu_exit";
            this.menu_exit.Size = new System.Drawing.Size(44, 21);
            this.menu_exit.Text = "退出";
            this.menu_exit.Click += new System.EventHandler(this.menu_exit_Click);
            // 
            // menu_test
            // 
            this.menu_test.Name = "menu_test";
            this.menu_test.Size = new System.Drawing.Size(41, 21);
            this.menu_test.Text = "test";
            this.menu_test.Click += new System.EventHandler(this.menu_test_Click);
            // 
            // menu_more
            // 
            this.menu_more.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_default,
            this.menu_hideGame,
            this.menu_reportViewer,
            this.menu_protocol_viewer});
            this.menu_more.Name = "menu_more";
            this.menu_more.Size = new System.Drawing.Size(53, 21);
            this.menu_more.Text = "更多...";
            // 
            // menu_default
            // 
            this.menu_default.Name = "menu_default";
            this.menu_default.Size = new System.Drawing.Size(177, 22);
            this.menu_default.Text = "默认设置";
            this.menu_default.Click += new System.EventHandler(this.menu_default_Click);
            // 
            // menu_hideGame
            // 
            this.menu_hideGame.Name = "menu_hideGame";
            this.menu_hideGame.Size = new System.Drawing.Size(177, 22);
            this.menu_hideGame.Text = "隐藏/显示游戏界面";
            this.menu_hideGame.Click += new System.EventHandler(this.menu_hideGame_Click);
            // 
            // menu_reportViewer
            // 
            this.menu_reportViewer.Name = "menu_reportViewer";
            this.menu_reportViewer.Size = new System.Drawing.Size(177, 22);
            this.menu_reportViewer.Text = "战报分析器";
            this.menu_reportViewer.Click += new System.EventHandler(this.menu_reportViewer_Click);
            // 
            // menu_protocol_viewer
            // 
            this.menu_protocol_viewer.Name = "menu_protocol_viewer";
            this.menu_protocol_viewer.Size = new System.Drawing.Size(177, 22);
            this.menu_protocol_viewer.Text = "协议分析器";
            this.menu_protocol_viewer.Click += new System.EventHandler(this.menu_protocol_viewer_Click);
            // 
            // menu_about
            // 
            this.menu_about.Name = "menu_about";
            this.menu_about.Size = new System.Drawing.Size(44, 21);
            this.menu_about.Text = "关于";
            this.menu_about.Click += new System.EventHandler(this.menu_about_Click);
            // 
            // menuWeaponCalc
            // 
            this.menuWeaponCalc.Name = "menuWeaponCalc";
            this.menuWeaponCalc.Size = new System.Drawing.Size(32, 19);
            // 
            // menu_trainCalc
            // 
            this.menu_trainCalc.Name = "menu_trainCalc";
            this.menu_trainCalc.Size = new System.Drawing.Size(32, 19);
            // 
            // menu_translator
            // 
            this.menu_translator.Name = "menu_translator";
            this.menu_translator.Size = new System.Drawing.Size(32, 19);
            // 
            // menu_report
            // 
            this.menu_report.Name = "menu_report";
            this.menu_report.Size = new System.Drawing.Size(32, 19);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(5, 666);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(184, 12);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "这里即将显示的是实时调用信息";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_playerinfo
            // 
            this.lbl_playerinfo.Location = new System.Drawing.Point(195, 666);
            this.lbl_playerinfo.Name = "lbl_playerinfo";
            this.lbl_playerinfo.Size = new System.Drawing.Size(861, 12);
            this.lbl_playerinfo.TabIndex = 5;
            this.lbl_playerinfo.Text = "玩家信息";
            this.lbl_playerinfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_time
            // 
            this.lbl_time.AutoSize = true;
            this.lbl_time.Location = new System.Drawing.Point(935, 9);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(0, 12);
            this.lbl_time.TabIndex = 6;
            // 
            // tip_msg
            // 
            this.tip_msg.AutomaticDelay = 100;
            this.tip_msg.AutoPopDelay = 5000;
            this.tip_msg.InitialDelay = 20;
            this.tip_msg.ReshowDelay = 20;
            // 
            // notify_icon
            // 
            this.notify_icon.Icon = ((System.Drawing.Icon)(resources.GetObject("notify_icon.Icon")));
            this.notify_icon.Text = "烟花三月下扬州";
            this.notify_icon.Visible = true;
            this.notify_icon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notify_icon_MouseDoubleClick);
            // 
            // subBrowser
            // 
            this.subBrowser.Enabled = true;
            this.subBrowser.Location = new System.Drawing.Point(1062, 5);
            this.subBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("subBrowser.OcxState")));
            this.subBrowser.Size = new System.Drawing.Size(300, 150);
            this.subBrowser.TabIndex = 7;
            this.subBrowser.Visible = false;
            this.subBrowser.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.subBrowser_NavigateComplete2);
            // 
            // login_worker
            // 
            this.login_worker.WorkerReportsProgress = true;
            this.login_worker.WorkerSupportsCancellation = true;
            this.login_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.login_worker_DoWork);
            this.login_worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.login_worker_ProgressChanged);
            this.login_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.login_worker_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1063, 682);
            this.Controls.Add(this.subBrowser);
            this.Controls.Add(this.lbl_time);
            this.Controls.Add(this.lbl_playerinfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.mainTab);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "傲视天地小助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mainTab.ResumeLayout(false);
            this.gamePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameBrowser)).EndInit();
            this.setPage.ResumeLayout(false);
            this.setPage.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_toukui_ticket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_equip_qh_use_gold_limit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_equipQh)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_mh_failcount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_equipMh)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_yupei_attr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_upgrade_crystal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_melt_failcount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_item_reserve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_upgrade_gem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_goon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_polish_reserve)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_1)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_npc_reserve_order)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_army_first_interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_army_reserve_order)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_attack_army)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_reserved_num)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_cityevent_maxstar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_reserve_token)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_score)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_player_level_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_attack_player_level_min)).EndInit();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_heros)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_buildings)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_get_baoshi_stone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_secretary_open_treasure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_get_stone)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_gem_price)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_credit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_stone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_gold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_global_reserve_silver)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_impose_loyalty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_impose_reserve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_force_impose_gold)).EndInit();
            this.setPage2.ResumeLayout(false);
            this.setPage2.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_boat_up_coin)).EndInit();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_DianQuan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_CiShu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_BaoShi)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downUp_QiFuDQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downUp_QiFuCoin)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_reserve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_refine_factory_gold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_refine_factory_stone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_weave_count)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_visit_fail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_refine_reserve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_movable_weave_price)).EndInit();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_treasure_game_goldstep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_daily_treasure_game_goldstep)).EndInit();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_troop_turntable_buygold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_gem_flop_upgrade_count)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_crossplatm_compete_gold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_kf_banquet_buygold)).EndInit();
            this.logPage.ResumeLayout(false);
            this.logPage.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subBrowser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
