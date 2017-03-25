using System.Drawing;
namespace com.lover.astd.game.ui
{
    partial class NewMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewMainForm));
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
            this.练将计算器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_about = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_lua = new System.Windows.Forms.ToolStripMenuItem();
            this.login_worker = new System.ComponentModel.BackgroundWorker();
            this.tip_msg = new System.Windows.Forms.ToolTip(this.components);
            this.num_fete_6 = new System.Windows.Forms.NumericUpDown();
            this.txt_attack_filter_content = new System.Windows.Forms.TextBox();
            this.chk_attack_player_gongjian = new System.Windows.Forms.CheckBox();
            this.txt_boat_upgrade = new System.Windows.Forms.TextBox();
            this.txt_movable_visit = new System.Windows.Forms.TextBox();
            this.txt_movable_order = new System.Windows.Forms.TextBox();
            this.txt_new_day_treasure_game_boxtype = new System.Windows.Forms.TextBox();
            this.txt_treasure_game_dice_type = new System.Windows.Forms.TextBox();
            this.txt_treasure_game_goldmove_boxtype = new System.Windows.Forms.TextBox();
            this.txt_treasure_game_boxtype = new System.Windows.Forms.TextBox();
            this.txt_daily_treasure_game_goldmove_boxtype = new System.Windows.Forms.TextBox();
            this.txt_daily_treasure_game_boxtype = new System.Windows.Forms.TextBox();
            this.txt_arch_upgrade = new System.Windows.Forms.TextBox();
            this.txt_giftevent_serial = new System.Windows.Forms.TextBox();
            this.chk_kfzb_market_ignore = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market = new System.Windows.Forms.CheckBox();
            this.chk_crossplatm_compete = new System.Windows.Forms.CheckBox();
            this.num_ticket_bighero = new System.Windows.Forms.NumericUpDown();
            this.notify_icon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.chk_market_usetokenafter5 = new System.Windows.Forms.CheckBox();
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
            this.combo_jailwork_type = new System.Windows.Forms.ComboBox();
            this.chk_juedou = new System.Windows.Forms.CheckBox();
            this.label67 = new System.Windows.Forms.Label();
            this.nUD_reserved_num = new System.Windows.Forms.NumericUpDown();
            this.label66 = new System.Windows.Forms.Label();
            this.chk_Nation = new System.Windows.Forms.CheckBox();
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
            this.chk_big_hero_tufei_enable = new System.Windows.Forms.CheckBox();
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
            this.label76 = new System.Windows.Forms.Label();
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
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.label74 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.combo_kfrank_def_formation = new System.Windows.Forms.ComboBox();
            this.combo_kfrank_ack_formation = new System.Windows.Forms.ComboBox();
            this.nUD_kfrank_point = new System.Windows.Forms.NumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.chk_kfrank = new System.Windows.Forms.CheckBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.nUD_qm_drink_coin = new System.Windows.Forms.NumericUpDown();
            this.label72 = new System.Windows.Forms.Label();
            this.nUD_qm_round_coin = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.chk_qingming = new System.Windows.Forms.CheckBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.nUD_BGActivity_coin = new System.Windows.Forms.NumericUpDown();
            this.label70 = new System.Windows.Forms.Label();
            this.chk_BGActivity = new System.Windows.Forms.CheckBox();
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
            this.label75 = new System.Windows.Forms.Label();
            this.combo_movable_weave_like = new System.Windows.Forms.ComboBox();
            this.num_movable_reserve = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.num_movable_refine_factory_gold = new System.Windows.Forms.NumericUpDown();
            this.num_movable_refine_factory_stone = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.num_movable_weave_count = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.num_movable_visit_fail = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.num_movable_refine_reserve = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.combo_movable_refine_item = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.num_movable_weave_price = new System.Windows.Forms.NumericUpDown();
            this.chk_movable_enable = new System.Windows.Forms.CheckBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.chk_new_day_treasure_game_shake_tree = new System.Windows.Forms.CheckBox();
            this.label69 = new System.Windows.Forms.Label();
            this.chk_new_day_treasure_game = new System.Windows.Forms.CheckBox();
            this.label68 = new System.Windows.Forms.Label();
            this.lbl_day_treasure_game_sel = new System.Windows.Forms.Label();
            this.btn_day_treasure_game_select = new System.Windows.Forms.Button();
            this.chk_daily_treasure_game_super_gold_end = new System.Windows.Forms.CheckBox();
            this.chk_treasure_game_ticket_gold_end = new System.Windows.Forms.CheckBox();
            this.combo_treasure_game_use_ticket_type = new System.Windows.Forms.ComboBox();
            this.chk_treasure_game_use_ticket = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
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
            this.num_super_fanpai_buy_gold = new System.Windows.Forms.NumericUpDown();
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
            this.label25 = new System.Windows.Forms.Label();
            this.chk_arch_create = new System.Windows.Forms.CheckBox();
            this.chk_giftevent_enable = new System.Windows.Forms.CheckBox();
            this.chk_arch_enable = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.chk_again = new System.Windows.Forms.CheckBox();
            this.combo_kfwd_openbox_type = new System.Windows.Forms.ComboBox();
            this.chk_kfwd_openbox = new System.Windows.Forms.CheckBox();
            this.combo_kfzb_market_weapon_pos_2 = new System.Windows.Forms.ComboBox();
            this.chk_kfzb_market_weapon_2 = new System.Windows.Forms.CheckBox();
            this.combo_kfzb_market_weapon_pos_1 = new System.Windows.Forms.ComboBox();
            this.combo_kfzb_market_gem_pos = new System.Windows.Forms.ComboBox();
            this.combo_kfzb_market_stone_pos = new System.Windows.Forms.ComboBox();
            this.combo_kfzb_market_silver_pos = new System.Windows.Forms.ComboBox();
            this.chk_kfzb_market_weapon_1 = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_gem = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_stone = new System.Windows.Forms.CheckBox();
            this.chk_kfzb_market_silver = new System.Windows.Forms.CheckBox();
            this.num_crossplatm_compete_gold = new System.Windows.Forms.NumericUpDown();
            this.num_kf_banquet_buygold = new System.Windows.Forms.NumericUpDown();
            this.chk_kf_banquet_nation = new System.Windows.Forms.CheckBox();
            this.chk_kf_banquet_enabled = new System.Windows.Forms.CheckBox();
            this.chk_kfwd_enable = new System.Windows.Forms.CheckBox();
            this.chk_kfwd_buyreward = new System.Windows.Forms.CheckBox();
            this.logPage = new System.Windows.Forms.TabPage();
            this.subBrowser = new AxSHDocVw.AxWebBrowser();
            this.lbl_tempStatus = new System.Windows.Forms.Label();
            this.btn_tempStop = new System.Windows.Forms.Button();
            this.btn_tempAdd = new System.Windows.Forms.Button();
            this.logTemp = new System.Windows.Forms.RichTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.logSurpise = new System.Windows.Forms.RichTextBox();
            this.txt_servers = new System.Windows.Forms.TextBox();
            this.logText = new System.Windows.Forms.RichTextBox();
            this.lbl_playerinfo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbl_time = new System.Windows.Forms.Label();
            this.chk_attack_before_22 = new System.Windows.Forms.CheckBox();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ticket_bighero)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_kfrank_point)).BeginInit();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_qm_drink_coin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_qm_round_coin)).BeginInit();
            this.groupBox21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_BGActivity_coin)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.num_super_fanpai_buy_gold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_troop_turntable_buygold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_gem_flop_upgrade_count)).BeginInit();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_crossplatm_compete_gold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_kf_banquet_buygold)).BeginInit();
            this.logPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subBrowser)).BeginInit();
            this.SuspendLayout();
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
            this.menu_about,
            this.menu_lua});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1064, 24);
            this.mainMenu.TabIndex = 2;
            // 
            // menu_login
            // 
            this.menu_login.Name = "menu_login";
            this.menu_login.Size = new System.Drawing.Size(45, 20);
            this.menu_login.Text = "登录";
            this.menu_login.Click += new System.EventHandler(this.menu_login_Click);
            // 
            // menu_init
            // 
            this.menu_init.Name = "menu_init";
            this.menu_init.Size = new System.Drawing.Size(71, 20);
            this.menu_init.Text = "读取数据";
            this.menu_init.Click += new System.EventHandler(this.menu_init_Click);
            // 
            // menu_saveSettings
            // 
            this.menu_saveSettings.Name = "menu_saveSettings";
            this.menu_saveSettings.Size = new System.Drawing.Size(71, 20);
            this.menu_saveSettings.Text = "保存设置";
            this.menu_saveSettings.Click += new System.EventHandler(this.menu_saveSettings_Click);
            // 
            // menu_relogin
            // 
            this.menu_relogin.Name = "menu_relogin";
            this.menu_relogin.Size = new System.Drawing.Size(71, 20);
            this.menu_relogin.Text = "重新登录";
            this.menu_relogin.Click += new System.EventHandler(this.menu_relogin_Click);
            // 
            // menu_refresh
            // 
            this.menu_refresh.Name = "menu_refresh";
            this.menu_refresh.Size = new System.Drawing.Size(71, 20);
            this.menu_refresh.Text = "刷新界面";
            this.menu_refresh.Click += new System.EventHandler(this.menu_refresh_Click);
            // 
            // menu_startServer
            // 
            this.menu_startServer.Name = "menu_startServer";
            this.menu_startServer.Size = new System.Drawing.Size(71, 20);
            this.menu_startServer.Text = "开始挂机";
            this.menu_startServer.Click += new System.EventHandler(this.menu_startServer_Click);
            // 
            // menu_stopServer
            // 
            this.menu_stopServer.Name = "menu_stopServer";
            this.menu_stopServer.Size = new System.Drawing.Size(71, 20);
            this.menu_stopServer.Text = "停止挂机";
            this.menu_stopServer.Click += new System.EventHandler(this.menu_stopServer_Click);
            // 
            // menu_clearLog
            // 
            this.menu_clearLog.Name = "menu_clearLog";
            this.menu_clearLog.Size = new System.Drawing.Size(71, 20);
            this.menu_clearLog.Text = "清空日志";
            this.menu_clearLog.Click += new System.EventHandler(this.menu_clearLog_Click);
            // 
            // menu_exit
            // 
            this.menu_exit.Name = "menu_exit";
            this.menu_exit.Size = new System.Drawing.Size(45, 20);
            this.menu_exit.Text = "退出";
            this.menu_exit.Click += new System.EventHandler(this.menu_exit_Click);
            // 
            // menu_test
            // 
            this.menu_test.Name = "menu_test";
            this.menu_test.Size = new System.Drawing.Size(38, 20);
            this.menu_test.Text = "test";
            this.menu_test.Click += new System.EventHandler(this.menu_test_Click);
            // 
            // menu_more
            // 
            this.menu_more.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_default,
            this.menu_hideGame,
            this.menu_reportViewer,
            this.menu_protocol_viewer,
            this.练将计算器ToolStripMenuItem});
            this.menu_more.Name = "menu_more";
            this.menu_more.Size = new System.Drawing.Size(54, 20);
            this.menu_more.Text = "更多...";
            // 
            // menu_default
            // 
            this.menu_default.Name = "menu_default";
            this.menu_default.Size = new System.Drawing.Size(183, 22);
            this.menu_default.Text = "默认设置";
            this.menu_default.Click += new System.EventHandler(this.menu_default_Click);
            // 
            // menu_hideGame
            // 
            this.menu_hideGame.Name = "menu_hideGame";
            this.menu_hideGame.Size = new System.Drawing.Size(183, 22);
            this.menu_hideGame.Text = "隐藏/显示游戏界面";
            this.menu_hideGame.Click += new System.EventHandler(this.menu_hideGame_Click);
            // 
            // menu_reportViewer
            // 
            this.menu_reportViewer.Name = "menu_reportViewer";
            this.menu_reportViewer.Size = new System.Drawing.Size(183, 22);
            this.menu_reportViewer.Text = "战报分析器";
            this.menu_reportViewer.Click += new System.EventHandler(this.menu_reportViewer_Click);
            // 
            // menu_protocol_viewer
            // 
            this.menu_protocol_viewer.Name = "menu_protocol_viewer";
            this.menu_protocol_viewer.Size = new System.Drawing.Size(183, 22);
            this.menu_protocol_viewer.Text = "协议分析器";
            this.menu_protocol_viewer.Click += new System.EventHandler(this.menu_protocol_viewer_Click);
            // 
            // 练将计算器ToolStripMenuItem
            // 
            this.练将计算器ToolStripMenuItem.Name = "练将计算器ToolStripMenuItem";
            this.练将计算器ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.练将计算器ToolStripMenuItem.Text = "练将计算器";
            this.练将计算器ToolStripMenuItem.Click += new System.EventHandler(this.menu_train_hero_Click);
            // 
            // menu_about
            // 
            this.menu_about.Name = "menu_about";
            this.menu_about.Size = new System.Drawing.Size(45, 20);
            this.menu_about.Text = "关于";
            this.menu_about.Click += new System.EventHandler(this.menu_about_Click);
            // 
            // menu_lua
            // 
            this.menu_lua.Name = "menu_lua";
            this.menu_lua.Size = new System.Drawing.Size(61, 20);
            this.menu_lua.Text = "刷新lua";
            this.menu_lua.Click += new System.EventHandler(this.menu_lua_Click);
            // 
            // login_worker
            // 
            this.login_worker.WorkerReportsProgress = true;
            this.login_worker.WorkerSupportsCancellation = true;
            this.login_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.login_worker_DoWork);
            this.login_worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.login_worker_ProgressChanged);
            this.login_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.login_worker_RunWorkerCompleted);
            // 
            // tip_msg
            // 
            this.tip_msg.AutomaticDelay = 100;
            this.tip_msg.AutoPopDelay = 5000;
            this.tip_msg.InitialDelay = 20;
            this.tip_msg.ReshowDelay = 20;
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
            // txt_attack_filter_content
            // 
            this.txt_attack_filter_content.Location = new System.Drawing.Point(219, 135);
            this.txt_attack_filter_content.Name = "txt_attack_filter_content";
            this.txt_attack_filter_content.Size = new System.Drawing.Size(75, 21);
            this.txt_attack_filter_content.TabIndex = 56;
            this.tip_msg.SetToolTip(this.txt_attack_filter_content, "2=2星,3=3星,哪项不要就不要填, 兵器和1星不要钱,自动开");
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
            // txt_boat_upgrade
            // 
            this.txt_boat_upgrade.Location = new System.Drawing.Point(98, 66);
            this.txt_boat_upgrade.Name = "txt_boat_upgrade";
            this.txt_boat_upgrade.Size = new System.Drawing.Size(100, 21);
            this.txt_boat_upgrade.TabIndex = 3;
            this.tip_msg.SetToolTip(this.txt_boat_upgrade, "1=蓝色,2=绿色,3=黄色,4=红色, 如果什么都不升级就不要填");
            // 
            // txt_movable_visit
            // 
            this.txt_movable_visit.Location = new System.Drawing.Point(83, 119);
            this.txt_movable_visit.Name = "txt_movable_visit";
            this.txt_movable_visit.Size = new System.Drawing.Size(61, 21);
            this.txt_movable_visit.TabIndex = 45;
            this.tip_msg.SetToolTip(this.txt_movable_visit, "1=楼兰, 2=西域, 3=巴蜀, 4=大理, 5=闽南, 6=辽东, 7=关东, 8=淮南, 9=荆楚, 10=南越, 11=浔阳, 12=岭南");
            // 
            // txt_movable_order
            // 
            this.txt_movable_order.Location = new System.Drawing.Point(96, 15);
            this.txt_movable_order.Name = "txt_movable_order";
            this.txt_movable_order.Size = new System.Drawing.Size(46, 21);
            this.txt_movable_order.TabIndex = 35;
            this.txt_movable_order.Text = "2341";
            this.tip_msg.SetToolTip(this.txt_movable_order, "1=通商,2=纺织,3=炼制,4=精炼,5=璞玉 如果哪项不要就不要填");
            // 
            // txt_new_day_treasure_game_boxtype
            // 
            this.txt_new_day_treasure_game_boxtype.Location = new System.Drawing.Point(118, 382);
            this.txt_new_day_treasure_game_boxtype.Name = "txt_new_day_treasure_game_boxtype";
            this.txt_new_day_treasure_game_boxtype.Size = new System.Drawing.Size(104, 21);
            this.txt_new_day_treasure_game_boxtype.TabIndex = 67;
            this.tip_msg.SetToolTip(this.txt_new_day_treasure_game_boxtype, "4=中级铁锤,10=高级铁锤,哪项不要就不要填");
            // 
            // txt_treasure_game_dice_type
            // 
            this.txt_treasure_game_dice_type.Location = new System.Drawing.Point(147, 274);
            this.txt_treasure_game_dice_type.Name = "txt_treasure_game_dice_type";
            this.txt_treasure_game_dice_type.Size = new System.Drawing.Size(75, 21);
            this.txt_treasure_game_dice_type.TabIndex = 65;
            this.tip_msg.SetToolTip(this.txt_treasure_game_dice_type, "0:1,1:2,2:0,位置:类型,0表示开始位置,类型:1(1点),2(2点),0(免费)");
            // 
            // txt_treasure_game_goldmove_boxtype
            // 
            this.txt_treasure_game_goldmove_boxtype.Location = new System.Drawing.Point(147, 218);
            this.txt_treasure_game_goldmove_boxtype.Name = "txt_treasure_game_goldmove_boxtype";
            this.txt_treasure_game_goldmove_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_treasure_game_goldmove_boxtype.TabIndex = 56;
            this.tip_msg.SetToolTip(this.txt_treasure_game_goldmove_boxtype, "0=兵器宝箱, 1=1星,2=2星,3=3星,4=摇钱树,5=超级门票,6=200级驿站后的行动力, 哪项不要就不要填");
            // 
            // txt_treasure_game_boxtype
            // 
            this.txt_treasure_game_boxtype.Location = new System.Drawing.Point(147, 191);
            this.txt_treasure_game_boxtype.Name = "txt_treasure_game_boxtype";
            this.txt_treasure_game_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_treasure_game_boxtype.TabIndex = 54;
            this.tip_msg.SetToolTip(this.txt_treasure_game_boxtype, "2=2星,3=3星,哪项不要就不要填, 兵器和1星不要钱,自动开");
            // 
            // txt_daily_treasure_game_goldmove_boxtype
            // 
            this.txt_daily_treasure_game_goldmove_boxtype.Location = new System.Drawing.Point(147, 88);
            this.txt_daily_treasure_game_goldmove_boxtype.Name = "txt_daily_treasure_game_goldmove_boxtype";
            this.txt_daily_treasure_game_goldmove_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_daily_treasure_game_goldmove_boxtype.TabIndex = 52;
            this.tip_msg.SetToolTip(this.txt_daily_treasure_game_goldmove_boxtype, "0=兵器宝箱, 1=1星,2=2星,3=3星,4=摇钱树,6=200级驿站后的行动力,哪项不要就不要填");
            // 
            // txt_daily_treasure_game_boxtype
            // 
            this.txt_daily_treasure_game_boxtype.Location = new System.Drawing.Point(147, 62);
            this.txt_daily_treasure_game_boxtype.Name = "txt_daily_treasure_game_boxtype";
            this.txt_daily_treasure_game_boxtype.Size = new System.Drawing.Size(75, 21);
            this.txt_daily_treasure_game_boxtype.TabIndex = 50;
            this.tip_msg.SetToolTip(this.txt_daily_treasure_game_boxtype, "2=2星,3=3星,哪项不要就不要填, 兵器和1星不要钱,自动开");
            // 
            // txt_arch_upgrade
            // 
            this.txt_arch_upgrade.Location = new System.Drawing.Point(119, 58);
            this.txt_arch_upgrade.Name = "txt_arch_upgrade";
            this.txt_arch_upgrade.Size = new System.Drawing.Size(113, 21);
            this.txt_arch_upgrade.TabIndex = 48;
            this.tip_msg.SetToolTip(this.txt_arch_upgrade, "1=白色,2=蓝色,3=绿色,4=黄色,5=红色, 如果什么都不升级就不要填");
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
            // num_ticket_bighero
            // 
            this.num_ticket_bighero.Location = new System.Drawing.Point(151, 223);
            this.num_ticket_bighero.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.num_ticket_bighero.Name = "num_ticket_bighero";
            this.num_ticket_bighero.Size = new System.Drawing.Size(46, 21);
            this.num_ticket_bighero.TabIndex = 30;
            this.tip_msg.SetToolTip(this.num_ticket_bighero, "0为不大祭祀");
            // 
            // notify_icon
            // 
            this.notify_icon.ContextMenuStrip = this.contextMenuStrip1;
            this.notify_icon.Icon = ((System.Drawing.Icon)(resources.GetObject("notify_icon.Icon")));
            this.notify_icon.Text = "烟花三月下扬州";
            this.notify_icon.Visible = true;
            this.notify_icon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notify_icon_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItem.Text = "退出";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.gamePage);
            this.mainTab.Controls.Add(this.setPage);
            this.mainTab.Controls.Add(this.setPage2);
            this.mainTab.Controls.Add(this.logPage);
            this.mainTab.Location = new System.Drawing.Point(2, 25);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(1060, 633);
            this.mainTab.TabIndex = 3;
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
            this.groupBox9.Controls.Add(this.chk_market_usetokenafter5);
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
            // chk_market_usetokenafter5
            // 
            this.chk_market_usetokenafter5.AutoSize = true;
            this.chk_market_usetokenafter5.Location = new System.Drawing.Point(113, 223);
            this.chk_market_usetokenafter5.Name = "chk_market_usetokenafter5";
            this.chk_market_usetokenafter5.Size = new System.Drawing.Size(90, 16);
            this.chk_market_usetokenafter5.TabIndex = 39;
            this.chk_market_usetokenafter5.Text = "5点前不进货";
            this.chk_market_usetokenafter5.UseVisualStyleBackColor = true;
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
            this.tabPage7.Controls.Add(this.chk_attack_before_22);
            this.tabPage7.Controls.Add(this.combo_jailwork_type);
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
            // combo_jailwork_type
            // 
            this.combo_jailwork_type.FormattingEnabled = true;
            this.combo_jailwork_type.Items.AddRange(new object[] {
            "宝石",
            "镔铁"});
            this.combo_jailwork_type.Location = new System.Drawing.Point(6, 55);
            this.combo_jailwork_type.Name = "combo_jailwork_type";
            this.combo_jailwork_type.Size = new System.Drawing.Size(89, 20);
            this.combo_jailwork_type.TabIndex = 62;
            this.combo_jailwork_type.Text = "劳作类型";
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
            this.label31.Location = new System.Drawing.Point(101, 53);
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
            this.chk_attack_player_move_tokenfull.Location = new System.Drawing.Point(96, 31);
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
            this.groupBox6.Controls.Add(this.chk_big_hero_tufei_enable);
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
            this.groupBox6.Location = new System.Drawing.Point(214, 312);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 288);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "武将管理";
            // 
            // chk_big_hero_tufei_enable
            // 
            this.chk_big_hero_tufei_enable.AutoSize = true;
            this.chk_big_hero_tufei_enable.Location = new System.Drawing.Point(77, 39);
            this.chk_big_hero_tufei_enable.Name = "chk_big_hero_tufei_enable";
            this.chk_big_hero_tufei_enable.Size = new System.Drawing.Size(96, 16);
            this.chk_big_hero_tufei_enable.TabIndex = 20;
            this.chk_big_hero_tufei_enable.Text = "开启大将突飞";
            this.chk_big_hero_tufei_enable.UseVisualStyleBackColor = true;
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
            this.dg_heros.Size = new System.Drawing.Size(190, 147);
            this.dg_heros.TabIndex = 17;
            this.dg_heros.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_heros.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_heros.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_heros.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dg_rowPrePaint);
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
            this.label3.Location = new System.Drawing.Point(7, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "洗什么：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "洗哪位：";
            // 
            // chk_hero_wash_zhi
            // 
            this.chk_hero_wash_zhi.AutoSize = true;
            this.chk_hero_wash_zhi.Location = new System.Drawing.Point(155, 264);
            this.chk_hero_wash_zhi.Name = "chk_hero_wash_zhi";
            this.chk_hero_wash_zhi.Size = new System.Drawing.Size(36, 16);
            this.chk_hero_wash_zhi.TabIndex = 14;
            this.chk_hero_wash_zhi.Text = "智";
            this.chk_hero_wash_zhi.UseVisualStyleBackColor = true;
            // 
            // chk_hero_wash_yong
            // 
            this.chk_hero_wash_yong.AutoSize = true;
            this.chk_hero_wash_yong.Location = new System.Drawing.Point(113, 264);
            this.chk_hero_wash_yong.Name = "chk_hero_wash_yong";
            this.chk_hero_wash_yong.Size = new System.Drawing.Size(36, 16);
            this.chk_hero_wash_yong.TabIndex = 13;
            this.chk_hero_wash_yong.Text = "勇";
            this.chk_hero_wash_yong.UseVisualStyleBackColor = true;
            // 
            // chk_hero_wash_tong
            // 
            this.chk_hero_wash_tong.AutoSize = true;
            this.chk_hero_wash_tong.Location = new System.Drawing.Point(71, 264);
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
            this.combo_hero_wash.Location = new System.Drawing.Point(70, 238);
            this.combo_hero_wash.Name = "combo_hero_wash";
            this.combo_hero_wash.Size = new System.Drawing.Size(121, 20);
            this.combo_hero_wash.TabIndex = 6;
            this.combo_hero_wash.ValueMember = "Id";
            // 
            // chk_hero_wash_enable
            // 
            this.chk_hero_wash_enable.AutoSize = true;
            this.chk_hero_wash_enable.Location = new System.Drawing.Point(7, 219);
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
            this.groupBox5.Location = new System.Drawing.Point(8, 312);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 289);
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
            this.dg_buildings.Size = new System.Drawing.Size(190, 221);
            this.dg_buildings.TabIndex = 3;
            this.dg_buildings.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseDown);
            this.dg_buildings.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CellMouseMove);
            this.dg_buildings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_DataError);
            this.dg_buildings.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dg_rowPrePaint);
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
            this.groupBox4.Controls.Add(this.label76);
            this.groupBox4.Controls.Add(this.num_get_baoshi_stone);
            this.groupBox4.Controls.Add(this.num_ticket_bighero);
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
            this.groupBox4.Size = new System.Drawing.Size(200, 249);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "小秘书";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(47, 228);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(101, 12);
            this.label76.TabIndex = 31;
            this.label76.Text = "大将令兑换点券≤";
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
            this.setPage2.Controls.Add(this.groupBox23);
            this.setPage2.Controls.Add(this.groupBox22);
            this.setPage2.Controls.Add(this.groupBox21);
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
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.label74);
            this.groupBox23.Controls.Add(this.label73);
            this.groupBox23.Controls.Add(this.combo_kfrank_def_formation);
            this.groupBox23.Controls.Add(this.combo_kfrank_ack_formation);
            this.groupBox23.Controls.Add(this.nUD_kfrank_point);
            this.groupBox23.Controls.Add(this.label40);
            this.groupBox23.Controls.Add(this.chk_kfrank);
            this.groupBox23.Location = new System.Drawing.Point(545, 404);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(285, 122);
            this.groupBox23.TabIndex = 59;
            this.groupBox23.TabStop = false;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(124, 47);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(65, 12);
            this.label74.TabIndex = 32;
            this.label74.Text = "防守阵型：";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(124, 21);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(65, 12);
            this.label73.TabIndex = 31;
            this.label73.Text = "攻击阵型：";
            // 
            // combo_kfrank_def_formation
            // 
            this.combo_kfrank_def_formation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfrank_def_formation.FormattingEnabled = true;
            this.combo_kfrank_def_formation.Items.AddRange(new object[] {
            "不变阵",
            "格挡阵",
            "长蛇阵",
            "锋矢阵",
            "偃月阵",
            "锥形阵",
            "八卦阵",
            "七星阵",
            "雁行阵"});
            this.combo_kfrank_def_formation.Location = new System.Drawing.Point(194, 43);
            this.combo_kfrank_def_formation.Name = "combo_kfrank_def_formation";
            this.combo_kfrank_def_formation.Size = new System.Drawing.Size(81, 20);
            this.combo_kfrank_def_formation.TabIndex = 30;
            // 
            // combo_kfrank_ack_formation
            // 
            this.combo_kfrank_ack_formation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_kfrank_ack_formation.FormattingEnabled = true;
            this.combo_kfrank_ack_formation.Items.AddRange(new object[] {
            "不变阵",
            "格挡阵",
            "长蛇阵",
            "锋矢阵",
            "偃月阵",
            "锥形阵",
            "八卦阵",
            "七星阵",
            "雁行阵"});
            this.combo_kfrank_ack_formation.Location = new System.Drawing.Point(193, 17);
            this.combo_kfrank_ack_formation.Name = "combo_kfrank_ack_formation";
            this.combo_kfrank_ack_formation.Size = new System.Drawing.Size(81, 20);
            this.combo_kfrank_ack_formation.TabIndex = 29;
            // 
            // nUD_kfrank_point
            // 
            this.nUD_kfrank_point.Location = new System.Drawing.Point(217, 70);
            this.nUD_kfrank_point.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nUD_kfrank_point.Name = "nUD_kfrank_point";
            this.nUD_kfrank_point.Size = new System.Drawing.Size(56, 21);
            this.nUD_kfrank_point.TabIndex = 13;
            this.nUD_kfrank_point.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(22, 74);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(191, 12);
            this.label40.TabIndex = 13;
            this.label40.Text = "胜利5场后，使用防守阵型，积分≥";
            // 
            // chk_kfrank
            // 
            this.chk_kfrank.AutoSize = true;
            this.chk_kfrank.Location = new System.Drawing.Point(6, 20);
            this.chk_kfrank.Name = "chk_kfrank";
            this.chk_kfrank.Size = new System.Drawing.Size(72, 16);
            this.chk_kfrank.TabIndex = 9;
            this.chk_kfrank.Text = "自动对战";
            this.chk_kfrank.UseVisualStyleBackColor = true;
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.nUD_qm_drink_coin);
            this.groupBox22.Controls.Add(this.label72);
            this.groupBox22.Controls.Add(this.nUD_qm_round_coin);
            this.groupBox22.Controls.Add(this.label71);
            this.groupBox22.Controls.Add(this.chk_qingming);
            this.groupBox22.Location = new System.Drawing.Point(545, 333);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(285, 65);
            this.groupBox22.TabIndex = 58;
            this.groupBox22.TabStop = false;
            // 
            // nUD_qm_drink_coin
            // 
            this.nUD_qm_drink_coin.Location = new System.Drawing.Point(218, 38);
            this.nUD_qm_drink_coin.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nUD_qm_drink_coin.Name = "nUD_qm_drink_coin";
            this.nUD_qm_drink_coin.Size = new System.Drawing.Size(56, 21);
            this.nUD_qm_drink_coin.TabIndex = 12;
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(112, 43);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(101, 12);
            this.label72.TabIndex = 11;
            this.label72.Text = "酒仙附体到金币≤";
            // 
            // nUD_qm_round_coin
            // 
            this.nUD_qm_round_coin.Location = new System.Drawing.Point(218, 13);
            this.nUD_qm_round_coin.Name = "nUD_qm_round_coin";
            this.nUD_qm_round_coin.Size = new System.Drawing.Size(56, 21);
            this.nUD_qm_round_coin.TabIndex = 10;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(111, 18);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(101, 12);
            this.label71.TabIndex = 9;
            this.label71.Text = "购买圈数到金币≤";
            // 
            // chk_qingming
            // 
            this.chk_qingming.AutoSize = true;
            this.chk_qingming.Location = new System.Drawing.Point(7, 17);
            this.chk_qingming.Name = "chk_qingming";
            this.chk_qingming.Size = new System.Drawing.Size(72, 16);
            this.chk_qingming.TabIndex = 10;
            this.chk_qingming.Text = "清明煮酒";
            this.chk_qingming.UseVisualStyleBackColor = true;
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.nUD_BGActivity_coin);
            this.groupBox21.Controls.Add(this.label70);
            this.groupBox21.Controls.Add(this.chk_BGActivity);
            this.groupBox21.Location = new System.Drawing.Point(843, 532);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(204, 69);
            this.groupBox21.TabIndex = 57;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "大宴活动";
            // 
            // nUD_BGActivity_coin
            // 
            this.nUD_BGActivity_coin.Location = new System.Drawing.Point(92, 39);
            this.nUD_BGActivity_coin.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUD_BGActivity_coin.Name = "nUD_BGActivity_coin";
            this.nUD_BGActivity_coin.Size = new System.Drawing.Size(56, 21);
            this.nUD_BGActivity_coin.TabIndex = 8;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(9, 44);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(77, 12);
            this.label70.TabIndex = 1;
            this.label70.Text = "自动到金币≤";
            // 
            // chk_BGActivity
            // 
            this.chk_BGActivity.AutoSize = true;
            this.chk_BGActivity.Location = new System.Drawing.Point(9, 20);
            this.chk_BGActivity.Name = "chk_BGActivity";
            this.chk_BGActivity.Size = new System.Drawing.Size(96, 16);
            this.chk_BGActivity.TabIndex = 0;
            this.chk_BGActivity.Text = "自动大宴活动";
            this.chk_BGActivity.UseVisualStyleBackColor = true;
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.cb_OpenYuebingHongbaoType);
            this.groupBox20.Controls.Add(this.chk_AutoYuebingHongbao);
            this.groupBox20.Controls.Add(this.chk_GoldTrain);
            this.groupBox20.Controls.Add(this.chk_AutoYuebing);
            this.groupBox20.Location = new System.Drawing.Point(843, 426);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(204, 100);
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
            this.cb_OpenYuebingHongbaoType.Location = new System.Drawing.Point(123, 71);
            this.cb_OpenYuebingHongbaoType.Name = "cb_OpenYuebingHongbaoType";
            this.cb_OpenYuebingHongbaoType.Size = new System.Drawing.Size(61, 20);
            this.cb_OpenYuebingHongbaoType.TabIndex = 3;
            // 
            // chk_AutoYuebingHongbao
            // 
            this.chk_AutoYuebingHongbao.AutoSize = true;
            this.chk_AutoYuebingHongbao.Location = new System.Drawing.Point(10, 74);
            this.chk_AutoYuebingHongbao.Name = "chk_AutoYuebingHongbao";
            this.chk_AutoYuebingHongbao.Size = new System.Drawing.Size(108, 16);
            this.chk_AutoYuebingHongbao.TabIndex = 2;
            this.chk_AutoYuebingHongbao.Text = "自动开阅兵红包";
            this.chk_AutoYuebingHongbao.UseVisualStyleBackColor = true;
            // 
            // chk_GoldTrain
            // 
            this.chk_GoldTrain.AutoSize = true;
            this.chk_GoldTrain.Location = new System.Drawing.Point(10, 48);
            this.chk_GoldTrain.Name = "chk_GoldTrain";
            this.chk_GoldTrain.Size = new System.Drawing.Size(72, 16);
            this.chk_GoldTrain.TabIndex = 1;
            this.chk_GoldTrain.Text = "金币训练";
            this.chk_GoldTrain.UseVisualStyleBackColor = true;
            // 
            // chk_AutoYuebing
            // 
            this.chk_AutoYuebing.AutoSize = true;
            this.chk_AutoYuebing.Location = new System.Drawing.Point(10, 22);
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
            this.groupBox19.Location = new System.Drawing.Point(843, 281);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(204, 142);
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
            this.chk_ShenHuo.Location = new System.Drawing.Point(851, 261);
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
            this.groupBox17.Location = new System.Drawing.Point(843, 115);
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
            900000,
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
            90000,
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
            this.groupBox13.Location = new System.Drawing.Point(545, 535);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(285, 66);
            this.groupBox13.TabIndex = 51;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "杂项";
            // 
            // chk_auto_hide_flash
            // 
            this.chk_auto_hide_flash.AutoSize = true;
            this.chk_auto_hide_flash.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_auto_hide_flash.Location = new System.Drawing.Point(6, 42);
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
            this.combo_global_boss_key_key.Location = new System.Drawing.Point(231, 18);
            this.combo_global_boss_key_key.Name = "combo_global_boss_key_key";
            this.combo_global_boss_key_key.Size = new System.Drawing.Size(47, 20);
            this.combo_global_boss_key_key.TabIndex = 38;
            // 
            // chk_global_boss_key_shift
            // 
            this.chk_global_boss_key_shift.AutoSize = true;
            this.chk_global_boss_key_shift.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chk_global_boss_key_shift.Location = new System.Drawing.Point(171, 20);
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
            this.chk_global_boss_key_alt.Location = new System.Drawing.Point(123, 20);
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
            this.chk_global_boss_key_ctrl.Location = new System.Drawing.Point(66, 20);
            this.chk_global_boss_key_ctrl.Name = "chk_global_boss_key_ctrl";
            this.chk_global_boss_key_ctrl.Size = new System.Drawing.Size(48, 16);
            this.chk_global_boss_key_ctrl.TabIndex = 1;
            this.chk_global_boss_key_ctrl.Text = "Ctrl";
            this.chk_global_boss_key_ctrl.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label75);
            this.groupBox2.Controls.Add(this.combo_movable_weave_like);
            this.groupBox2.Controls.Add(this.num_movable_reserve);
            this.groupBox2.Controls.Add(this.label45);
            this.groupBox2.Controls.Add(this.num_movable_refine_factory_gold);
            this.groupBox2.Controls.Add(this.num_movable_refine_factory_stone);
            this.groupBox2.Controls.Add(this.label44);
            this.groupBox2.Controls.Add(this.label43);
            this.groupBox2.Controls.Add(this.num_movable_weave_count);
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Controls.Add(this.label38);
            this.groupBox2.Controls.Add(this.num_movable_visit_fail);
            this.groupBox2.Controls.Add(this.label37);
            this.groupBox2.Controls.Add(this.txt_movable_visit);
            this.groupBox2.Controls.Add(this.label32);
            this.groupBox2.Controls.Add(this.num_movable_refine_reserve);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.combo_movable_refine_item);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.txt_movable_order);
            this.groupBox2.Controls.Add(this.num_movable_weave_price);
            this.groupBox2.Controls.Add(this.chk_movable_enable);
            this.groupBox2.Location = new System.Drawing.Point(545, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 173);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "行动力";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(25, 149);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(53, 12);
            this.label75.TabIndex = 63;
            this.label75.Text = "纺织布匹";
            // 
            // combo_movable_weave_like
            // 
            this.combo_movable_weave_like.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_movable_weave_like.FormattingEnabled = true;
            this.combo_movable_weave_like.Items.AddRange(new object[] {
            "孔羽缭绫",
            "虎纹亚麻",
            "流云壮锦"});
            this.combo_movable_weave_like.Location = new System.Drawing.Point(83, 145);
            this.combo_movable_weave_like.Name = "combo_movable_weave_like";
            this.combo_movable_weave_like.Size = new System.Drawing.Size(61, 20);
            this.combo_movable_weave_like.TabIndex = 62;
            // 
            // num_movable_reserve
            // 
            this.num_movable_reserve.Location = new System.Drawing.Point(228, 15);
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
            this.label45.Location = new System.Drawing.Point(160, 20);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(65, 12);
            this.label45.TabIndex = 60;
            this.label45.Text = "保留行动力";
            // 
            // num_movable_refine_factory_gold
            // 
            this.num_movable_refine_factory_gold.Location = new System.Drawing.Point(228, 93);
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
            this.num_movable_refine_factory_stone.Location = new System.Drawing.Point(228, 67);
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
            this.label44.Location = new System.Drawing.Point(88, 98);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(137, 12);
            this.label44.TabIndex = 57;
            this.label44.Text = "使用金币炼制如果金币≤";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(88, 72);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(137, 12);
            this.label43.TabIndex = 56;
            this.label43.Text = "使用玉石炼制如果玉石≤";
            // 
            // num_movable_weave_count
            // 
            this.num_movable_weave_count.Location = new System.Drawing.Point(228, 41);
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
            this.label42.Location = new System.Drawing.Point(160, 46);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(65, 12);
            this.label42.TabIndex = 54;
            this.label42.Text = "纺织多少次";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(26, 124);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(53, 12);
            this.label38.TabIndex = 48;
            this.label38.Text = "拜访商盟";
            // 
            // num_movable_visit_fail
            // 
            this.num_movable_visit_fail.Location = new System.Drawing.Point(228, 119);
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
            this.num_movable_visit_fail.Size = new System.Drawing.Size(46, 21);
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
            this.label37.Location = new System.Drawing.Point(148, 124);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(77, 12);
            this.label37.TabIndex = 46;
            this.label37.Text = "允许失败次数";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(148, 150);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(77, 12);
            this.label32.TabIndex = 42;
            this.label32.Text = "保留精炼次数";
            // 
            // num_movable_refine_reserve
            // 
            this.num_movable_refine_reserve.Location = new System.Drawing.Point(228, 145);
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
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(9, 72);
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
            this.combo_movable_refine_item.Location = new System.Drawing.Point(6, 94);
            this.combo_movable_refine_item.Name = "combo_movable_refine_item";
            this.combo_movable_refine_item.Size = new System.Drawing.Size(79, 20);
            this.combo_movable_refine_item.TabIndex = 37;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(22, 45);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(65, 12);
            this.label29.TabIndex = 36;
            this.label29.Text = "纺织布价≥";
            // 
            // num_movable_weave_price
            // 
            this.num_movable_weave_price.Location = new System.Drawing.Point(96, 41);
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
            this.chk_movable_enable.Location = new System.Drawing.Point(6, 17);
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
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(28, 196);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(89, 12);
            this.label36.TabIndex = 55;
            this.label36.Text = "打开宝箱类型：";
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
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(28, 72);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(89, 12);
            this.label33.TabIndex = 51;
            this.label33.Text = "打开宝箱类型：";
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
            this.groupBox15.Controls.Add(this.num_super_fanpai_buy_gold);
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
            // num_super_fanpai_buy_gold
            // 
            this.num_super_fanpai_buy_gold.Location = new System.Drawing.Point(192, 62);
            this.num_super_fanpai_buy_gold.Name = "num_super_fanpai_buy_gold";
            this.num_super_fanpai_buy_gold.Size = new System.Drawing.Size(51, 21);
            this.num_super_fanpai_buy_gold.TabIndex = 69;
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
            this.chk_super_fanpai.Size = new System.Drawing.Size(180, 16);
            this.chk_super_fanpai.TabIndex = 53;
            this.chk_super_fanpai.Text = "自动超级翻牌，自动到金币≤";
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
            this.num_gem_flop_upgrade_count.Location = new System.Drawing.Point(192, 37);
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
            this.logPage.Controls.Add(this.subBrowser);
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
            // subBrowser
            // 
            this.subBrowser.Enabled = true;
            this.subBrowser.Location = new System.Drawing.Point(416, -19);
            this.subBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("subBrowser.OcxState")));
            this.subBrowser.Size = new System.Drawing.Size(300, 150);
            this.subBrowser.TabIndex = 9;
            this.subBrowser.Visible = false;
            this.subBrowser.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.subBrowser_NavigateComplete2);
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
            // 
            // lbl_playerinfo
            // 
            this.lbl_playerinfo.Location = new System.Drawing.Point(200, 664);
            this.lbl_playerinfo.Name = "lbl_playerinfo";
            this.lbl_playerinfo.Size = new System.Drawing.Size(860, 12);
            this.lbl_playerinfo.TabIndex = 7;
            this.lbl_playerinfo.Text = "玩家信息";
            this.lbl_playerinfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(5, 664);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(190, 12);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "这里即将显示的是实时调用信息";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_time
            // 
            this.lbl_time.AutoSize = true;
            this.lbl_time.Location = new System.Drawing.Point(930, 7);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(0, 12);
            this.lbl_time.TabIndex = 8;
            // 
            // chk_attack_before_22
            // 
            this.chk_attack_before_22.AutoSize = true;
            this.chk_attack_before_22.Location = new System.Drawing.Point(6, 31);
            this.chk_attack_before_22.Name = "chk_attack_before_22";
            this.chk_attack_before_22.Size = new System.Drawing.Size(84, 16);
            this.chk_attack_before_22.TabIndex = 63;
            this.chk_attack_before_22.Text = "10点后消息";
            this.chk_attack_before_22.UseVisualStyleBackColor = true;
            // 
            // NewMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 682);
            this.Controls.Add(this.lbl_time);
            this.Controls.Add(this.lbl_playerinfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.mainTab);
            this.Controls.Add(this.mainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewMainForm";
            this.Text = "傲视天地小助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewMainForm_FormClosing);
            this.Load += new System.EventHandler(this.NewMainForm_Load);
            this.Resize += new System.EventHandler(this.NewMainForm_Resize);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_fete_6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ticket_bighero)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
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
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_kfrank_point)).EndInit();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_qm_drink_coin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_qm_round_coin)).EndInit();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_BGActivity_coin)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.num_super_fanpai_buy_gold)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.subBrowser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menu_login;
        public System.Windows.Forms.ToolStripMenuItem menu_init;
        public System.Windows.Forms.ToolStripMenuItem menu_saveSettings;
        private System.Windows.Forms.ToolStripMenuItem menu_relogin;
        public System.Windows.Forms.ToolStripMenuItem menu_refresh;
        public System.Windows.Forms.ToolStripMenuItem menu_startServer;
        public System.Windows.Forms.ToolStripMenuItem menu_stopServer;
        private System.Windows.Forms.ToolStripMenuItem menu_clearLog;
        private System.Windows.Forms.ToolStripMenuItem menu_exit;
        public System.Windows.Forms.ToolStripMenuItem menu_test;
        private System.Windows.Forms.ToolStripMenuItem menu_more;
        private System.Windows.Forms.ToolStripMenuItem menu_default;
        private System.Windows.Forms.ToolStripMenuItem menu_hideGame;
        private System.Windows.Forms.ToolStripMenuItem menu_reportViewer;
        private System.Windows.Forms.ToolStripMenuItem menu_protocol_viewer;
        private System.Windows.Forms.ToolStripMenuItem menu_about;
        private System.ComponentModel.BackgroundWorker login_worker;
        private System.Windows.Forms.ToolTip tip_msg;
        private System.Windows.Forms.NotifyIcon notify_icon;
        public System.Windows.Forms.TabControl mainTab;
        public System.Windows.Forms.TabPage gamePage;
        public AxSHDocVw.AxWebBrowser gameBrowser;
        public System.Windows.Forms.TabPage setPage;
        public System.Windows.Forms.CheckBox chk_AutoBuyCredit;
        public System.Windows.Forms.CheckBox chk_Cai;
        public System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.ComboBox combo_daily_weapon_type;
        public System.Windows.Forms.CheckBox chk_daily_weapon;
        private System.Windows.Forms.Button btn_weaponCalc;
        public System.Windows.Forms.RichTextBox txt_weaponinfo;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.CheckBox chk_upgrade_weapone_enable;
        public System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        public System.Windows.Forms.NumericUpDown nUD_toukui_ticket;
        public System.Windows.Forms.CheckBox chk_upgrade_toukui;
        public System.Windows.Forms.NumericUpDown num_equip_qh_use_gold_limit;
        public System.Windows.Forms.CheckBox chk_equip_qh_use_gold;
        public System.Windows.Forms.DataGridView dg_equipQh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_chk_equipEnchant;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name_equipEnchant;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_level_equipEnchant;
        public System.Windows.Forms.CheckBox chk_equip_qh_enable;
        public System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.NumericUpDown num_mh_failcount;
        public System.Windows.Forms.ComboBox combo_mh_type;
        public System.Windows.Forms.DataGridView dg_equipMh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_chk_equipMagic;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name_equipMagic;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_level_equipMagic;
        public System.Windows.Forms.CheckBox chk_equip_mh_enable;
        public System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.NumericUpDown num_yupei_attr;
        public System.Windows.Forms.CheckBox chk_auto_melt_yupei;
        public System.Windows.Forms.NumericUpDown num_upgrade_crystal;
        public System.Windows.Forms.CheckBox chk_upgrade_war_chariot;
        public System.Windows.Forms.CheckBox chk_upgrade_crystal;
        public System.Windows.Forms.CheckBox chk_openbox_for_yupei;
        private System.Windows.Forms.Label label22;
        public System.Windows.Forms.NumericUpDown num_polish_melt_failcount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.NumericUpDown num_polish_item_reserve;
        public System.Windows.Forms.NumericUpDown num_upgrade_gem;
        public System.Windows.Forms.NumericUpDown num_polish_goon;
        public System.Windows.Forms.NumericUpDown num_polish_reserve;
        public System.Windows.Forms.CheckBox chk_upgrade_gem;
        public System.Windows.Forms.CheckBox chk_polish_enable;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.NumericUpDown num_fete_6;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.NumericUpDown num_fete_5;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.NumericUpDown num_fete_4;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.NumericUpDown num_fete_3;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.NumericUpDown num_fete_2;
        private System.Windows.Forms.Label lbl_fete_silver;
        public System.Windows.Forms.NumericUpDown num_fete_1;
        public System.Windows.Forms.CheckBox chk_fete_enable;
        public System.Windows.Forms.GroupBox groupBox9;
        public System.Windows.Forms.CheckBox chk_market_usetoken;
        public System.Windows.Forms.CheckBox chk_market_super;
        public System.Windows.Forms.CheckBox chk_market_drop_notbuy;
        public System.Windows.Forms.CheckBox chk_market_notbuy_iffail;
        public System.Windows.Forms.CheckBox chk_market_force_attack;
        public System.Windows.Forms.CheckBox chk_market_purpleweapon_gold;
        public System.Windows.Forms.CheckBox chk_market_redweapon_gold;
        public System.Windows.Forms.CheckBox chk_market_stone_gold;
        public System.Windows.Forms.CheckBox chk_market_silver_gold;
        public System.Windows.Forms.CheckBox chk_market_gem_gold;
        public System.Windows.Forms.ComboBox combo_market_purpleweapon_type;
        public System.Windows.Forms.ComboBox combo_market_redweapon_type;
        public System.Windows.Forms.ComboBox combo_market_stone_type;
        public System.Windows.Forms.ComboBox combo_market_silver_type;
        public System.Windows.Forms.ComboBox combo_market_gem_type;
        public System.Windows.Forms.CheckBox chk_market_purpleweapon;
        public System.Windows.Forms.CheckBox chk_market_redweapon;
        public System.Windows.Forms.CheckBox chk_market_stone;
        public System.Windows.Forms.CheckBox chk_market_silver;
        public System.Windows.Forms.CheckBox chk_market_gem;
        public System.Windows.Forms.CheckBox chk_market_enable;
        public System.Windows.Forms.GroupBox groupBox8;
        public System.Windows.Forms.ComboBox combo_defense_formation;
        public System.Windows.Forms.CheckBox chk_defense_format;
        public System.Windows.Forms.TabControl tabControl2;
        public System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label47;
        public System.Windows.Forms.CheckBox chk_battle_event_stone_gold;
        public System.Windows.Forms.CheckBox chk_battle_event_weapon_gold;
        public System.Windows.Forms.CheckBox chk_battle_event_gem_gold;
        public System.Windows.Forms.CheckBox chk_battle_event_weapon;
        public System.Windows.Forms.CheckBox chk_battle_event_stone;
        public System.Windows.Forms.CheckBox chk_battle_event_gem;
        public System.Windows.Forms.CheckBox chk_battle_event_enabled;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button btn_selForceNpc;
        public System.Windows.Forms.Label lbl_attackForceNpc;
        private System.Windows.Forms.Button btn_selNpc;
        public System.Windows.Forms.Label lbl_attackNpc;
        public System.Windows.Forms.NumericUpDown num_attack_npc_reserve_order;
        public System.Windows.Forms.CheckBox chk_attack_npc_enable;
        public System.Windows.Forms.TabPage tabPage6;
        public System.Windows.Forms.CheckBox chk_attack_army_first;
        public System.Windows.Forms.CheckBox chk_attack_army_only_jingyingtime;
        public System.Windows.Forms.NumericUpDown num_attack_army_first_interval;
        public System.Windows.Forms.NumericUpDown num_attack_army_reserve_order;
        public System.Windows.Forms.DataGridView dg_attack_army;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn column_firstbattle;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        public System.Windows.Forms.CheckBox chk_attack_army;
        public System.Windows.Forms.TabPage tabPage7;
        public System.Windows.Forms.CheckBox chk_juedou;
        private System.Windows.Forms.Label label67;
        public System.Windows.Forms.NumericUpDown nUD_reserved_num;
        private System.Windows.Forms.Label label66;
        public System.Windows.Forms.CheckBox chk_Nation;
        public System.Windows.Forms.TextBox txt_attack_filter_content;
        public System.Windows.Forms.ComboBox combo_attack_filter_type;
        public System.Windows.Forms.Label label48;
        public System.Windows.Forms.NumericUpDown num_attack_cityevent_maxstar;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label28;
        public System.Windows.Forms.NumericUpDown num_attack_reserve_token;
        public System.Windows.Forms.CheckBox chk_attack_reserve_token;
        public System.Windows.Forms.CheckBox chk_attack_enable_attack;
        public System.Windows.Forms.CheckBox chk_attack_get_extra_order;
        public System.Windows.Forms.CheckBox chk_attack_jail_tech;
        public System.Windows.Forms.CheckBox chk_attack_player_gongjian;
        public System.Windows.Forms.CheckBox chk_attack_player_cityevent;
        public System.Windows.Forms.Label lbl_attack_target;
        private System.Windows.Forms.Button btn_attack_target;
        public System.Windows.Forms.Label label24;
        public System.Windows.Forms.NumericUpDown num_attack_score;
        public System.Windows.Forms.CheckBox chk_attack_not_injail;
        public System.Windows.Forms.CheckBox chk_attack_npc_enemy;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown num_attack_player_level_max;
        public System.Windows.Forms.NumericUpDown num_attack_player_level_min;
        public System.Windows.Forms.CheckBox chk_attack_player_move_tokenfull;
        public System.Windows.Forms.CheckBox chk_attack_player_use_token;
        public System.Windows.Forms.CheckBox chk_attack_player;
        public System.Windows.Forms.TabPage tabPage8;
        public System.Windows.Forms.Label lbl_attack_resCampaign;
        private System.Windows.Forms.Button btn_sel_resCampaign;
        public System.Windows.Forms.CheckBox chk_res_campaign_enable;
        public System.Windows.Forms.GroupBox groupBox6;
        public System.Windows.Forms.CheckBox chk_hero_giftevent;
        public System.Windows.Forms.CheckBox chk_hero_nocd;
        public System.Windows.Forms.DataGridView dg_heros;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_chk_hero;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name_hero;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_level_hero;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox chk_hero_wash_zhi;
        public System.Windows.Forms.CheckBox chk_hero_wash_yong;
        public System.Windows.Forms.CheckBox chk_hero_wash_tong;
        public System.Windows.Forms.ComboBox combo_hero_wash;
        public System.Windows.Forms.CheckBox chk_hero_wash_enable;
        public System.Windows.Forms.CheckBox chk_hero_tufei_enable;
        public System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.CheckBox chk_building_protect;
        public System.Windows.Forms.CheckBox chk_selAll_building;
        public System.Windows.Forms.DataGridView dg_buildings;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_chk_building;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name_building;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_level_building;
        public System.Windows.Forms.CheckBox chk_building_enable;
        public System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.NumericUpDown num_get_baoshi_stone;
        public System.Windows.Forms.CheckBox chk_get_baoshi_stone;
        public System.Windows.Forms.CheckBox chk_secretary_giftbox;
        public System.Windows.Forms.NumericUpDown num_secretary_open_treasure;
        public System.Windows.Forms.CheckBox chk_secretary_open_treasure;
        public System.Windows.Forms.CheckBox chk_dailytask_enable;
        public System.Windows.Forms.CheckBox chk_stock_7;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.CheckBox chk_stock_6;
        public System.Windows.Forms.CheckBox chk_merchant_onlyfree;
        public System.Windows.Forms.CheckBox chk_stock_5;
        public System.Windows.Forms.ComboBox combo_merchant_sell_level;
        public System.Windows.Forms.CheckBox chk_stock_4;
        public System.Windows.Forms.ComboBox combo_merchant_type;
        public System.Windows.Forms.CheckBox chk_stock_3;
        public System.Windows.Forms.CheckBox chk_merchant;
        public System.Windows.Forms.CheckBox chk_stock_2;
        public System.Windows.Forms.CheckBox chk_stock_1;
        public System.Windows.Forms.NumericUpDown num_get_stone;
        public System.Windows.Forms.CheckBox chk_stock_enable;
        public System.Windows.Forms.CheckBox chk_get_stone;
        public System.Windows.Forms.CheckBox chk_dinner;
        public System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label23;
        public System.Windows.Forms.NumericUpDown num_global_gem_price;
        public System.Windows.Forms.NumericUpDown num_global_reserve_credit;
        public System.Windows.Forms.CheckBox chk_global_reserve_credit;
        public System.Windows.Forms.NumericUpDown num_global_reserve_stone;
        public System.Windows.Forms.CheckBox chk_global_reserve_stone;
        public System.Windows.Forms.NumericUpDown num_global_reserve_gold;
        public System.Windows.Forms.CheckBox chk_global_reserve_gold;
        public System.Windows.Forms.NumericUpDown num_global_reserve_silver;
        public System.Windows.Forms.CheckBox chk_global_reserve_silver;
        public System.Windows.Forms.CheckBox chk_global_logonAward;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.NumericUpDown num_impose_loyalty;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.NumericUpDown num_impose_reserve;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown num_force_impose_gold;
        public System.Windows.Forms.CheckBox chk_impose_enable;
        public System.Windows.Forms.TabPage setPage2;
        private System.Windows.Forms.GroupBox groupBox20;
        public System.Windows.Forms.ComboBox cb_OpenYuebingHongbaoType;
        public System.Windows.Forms.CheckBox chk_AutoYuebingHongbao;
        public System.Windows.Forms.CheckBox chk_GoldTrain;
        public System.Windows.Forms.CheckBox chk_AutoYuebing;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Label label63;
        public System.Windows.Forms.NumericUpDown nUD_boat_up_coin;
        private System.Windows.Forms.Label label62;
        public System.Windows.Forms.CheckBox chk_AutoSelectTime;
        public System.Windows.Forms.TextBox txt_boat_upgrade;
        private System.Windows.Forms.Label label54;
        public System.Windows.Forms.CheckBox chk_boat_creat;
        public System.Windows.Forms.CheckBox chk_AutoBoat;
        public System.Windows.Forms.CheckBox chk_ShenHuo;
        private System.Windows.Forms.GroupBox groupBox17;
        public System.Windows.Forms.CheckBox chk_SuperUseFree;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label61;
        public System.Windows.Forms.NumericUpDown nUD_DianQuan;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label57;
        public System.Windows.Forms.NumericUpDown nUD_CiShu;
        private System.Windows.Forms.Label label55;
        public System.Windows.Forms.NumericUpDown nUD_BaoShi;
        public System.Windows.Forms.CheckBox chk_AutoBaiShen;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label51;
        public System.Windows.Forms.NumericUpDown downUp_QiFuDQ;
        private System.Windows.Forms.Label label50;
        public System.Windows.Forms.NumericUpDown downUp_QiFuCoin;
        private System.Windows.Forms.Label label49;
        public System.Windows.Forms.CheckBox checkBox_QiFu;
        private System.Windows.Forms.GroupBox groupBox13;
        public System.Windows.Forms.CheckBox chk_auto_hide_flash;
        public System.Windows.Forms.CheckBox chk_global_boss_key_enable;
        public System.Windows.Forms.ComboBox combo_global_boss_key_key;
        public System.Windows.Forms.CheckBox chk_global_boss_key_shift;
        public System.Windows.Forms.CheckBox chk_global_boss_key_alt;
        public System.Windows.Forms.CheckBox chk_global_boss_key_ctrl;
        public System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.NumericUpDown num_movable_reserve;
        private System.Windows.Forms.Label label45;
        public System.Windows.Forms.NumericUpDown num_movable_refine_factory_gold;
        public System.Windows.Forms.NumericUpDown num_movable_refine_factory_stone;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        public System.Windows.Forms.NumericUpDown num_movable_weave_count;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label38;
        public System.Windows.Forms.NumericUpDown num_movable_visit_fail;
        private System.Windows.Forms.Label label37;
        public System.Windows.Forms.TextBox txt_movable_visit;
        private System.Windows.Forms.Label label32;
        public System.Windows.Forms.NumericUpDown num_movable_refine_reserve;
        private System.Windows.Forms.Label label30;
        public System.Windows.Forms.ComboBox combo_movable_refine_item;
        private System.Windows.Forms.Label label29;
        public System.Windows.Forms.TextBox txt_movable_order;
        public System.Windows.Forms.NumericUpDown num_movable_weave_price;
        public System.Windows.Forms.CheckBox chk_movable_enable;
        private System.Windows.Forms.GroupBox groupBox18;
        public System.Windows.Forms.CheckBox chk_new_day_treasure_game_shake_tree;
        private System.Windows.Forms.Label label69;
        public System.Windows.Forms.TextBox txt_new_day_treasure_game_boxtype;
        public System.Windows.Forms.CheckBox chk_new_day_treasure_game;
        public System.Windows.Forms.TextBox txt_treasure_game_dice_type;
        private System.Windows.Forms.Label label68;
        public System.Windows.Forms.Label lbl_day_treasure_game_sel;
        private System.Windows.Forms.Button btn_day_treasure_game_select;
        public System.Windows.Forms.CheckBox chk_daily_treasure_game_super_gold_end;
        public System.Windows.Forms.CheckBox chk_treasure_game_ticket_gold_end;
        public System.Windows.Forms.ComboBox combo_treasure_game_use_ticket_type;
        public System.Windows.Forms.CheckBox chk_treasure_game_use_ticket;
        private System.Windows.Forms.Label label35;
        public System.Windows.Forms.TextBox txt_treasure_game_goldmove_boxtype;
        private System.Windows.Forms.Label label36;
        public System.Windows.Forms.TextBox txt_treasure_game_boxtype;
        private System.Windows.Forms.Label label34;
        public System.Windows.Forms.TextBox txt_daily_treasure_game_goldmove_boxtype;
        private System.Windows.Forms.Label label33;
        public System.Windows.Forms.TextBox txt_daily_treasure_game_boxtype;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        public System.Windows.Forms.NumericUpDown num_treasure_game_goldstep;
        public System.Windows.Forms.NumericUpDown num_daily_treasure_game_goldstep;
        public System.Windows.Forms.CheckBox chk_treasure_game_enable;
        public System.Windows.Forms.CheckBox chk_daily_treasure_game;
        private System.Windows.Forms.GroupBox groupBox16;
        public System.Windows.Forms.ComboBox combo_tower_type;
        public System.Windows.Forms.CheckBox chk_tower_enable;
        public System.Windows.Forms.CheckBox chk_world_army_openbox;
        public System.Windows.Forms.CheckBox chk_festival_event;
        public System.Windows.Forms.ComboBox combo_world_army_refresh_type;
        public System.Windows.Forms.CheckBox chk_world_army_buybox2;
        public System.Windows.Forms.CheckBox chk_world_army_buybox1;
        public System.Windows.Forms.CheckBox chk_world_army_enable;
        public System.Windows.Forms.ComboBox combo_world_army_boxtype;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label64;
        public System.Windows.Forms.NumericUpDown nm_Out;
        public System.Windows.Forms.CheckBox chk_gemdump_enable;
        public System.Windows.Forms.NumericUpDown num_troop_turntable_buygold;
        private System.Windows.Forms.Label label46;
        public System.Windows.Forms.ComboBox combo_troop_turntable_type;
        public System.Windows.Forms.CheckBox chk_troop_turntable_goldratio;
        public System.Windows.Forms.CheckBox chk_cake_event;
        public System.Windows.Forms.CheckBox chk_troop_turntable;
        public System.Windows.Forms.CheckBox chk_super_fanpai;
        private System.Windows.Forms.Label label39;
        public System.Windows.Forms.CheckBox chk_troop_feedback_openbox;
        public System.Windows.Forms.ComboBox combo_troop_feedback_opentype;
        public System.Windows.Forms.CheckBox chk_troop_feedback_opentreasure;
        public System.Windows.Forms.CheckBox chk_troop_feedback_doubleweapon;
        public System.Windows.Forms.CheckBox chk_troop_feedback_refine_notired;
        public System.Windows.Forms.CheckBox chk_troop_feedback_enable;
        public System.Windows.Forms.CheckBox chk_gem_flop_enable;
        public System.Windows.Forms.NumericUpDown num_gem_flop_upgrade_count;
        public System.Windows.Forms.CheckBox chk_silver_flop_enable;
        private System.Windows.Forms.GroupBox groupBox12;
        public System.Windows.Forms.TextBox txt_arch_upgrade;
        private System.Windows.Forms.Label label25;
        public System.Windows.Forms.CheckBox chk_arch_create;
        public System.Windows.Forms.TextBox txt_giftevent_serial;
        public System.Windows.Forms.CheckBox chk_giftevent_enable;
        public System.Windows.Forms.CheckBox chk_arch_enable;
        private System.Windows.Forms.GroupBox groupBox11;
        public System.Windows.Forms.CheckBox chk_again;
        public System.Windows.Forms.ComboBox combo_kfwd_openbox_type;
        public System.Windows.Forms.CheckBox chk_kfwd_openbox;
        public System.Windows.Forms.ComboBox combo_kfzb_market_weapon_pos_2;
        public System.Windows.Forms.CheckBox chk_kfzb_market_weapon_2;
        public System.Windows.Forms.CheckBox chk_kfzb_market_ignore;
        public System.Windows.Forms.ComboBox combo_kfzb_market_weapon_pos_1;
        public System.Windows.Forms.ComboBox combo_kfzb_market_gem_pos;
        public System.Windows.Forms.ComboBox combo_kfzb_market_stone_pos;
        public System.Windows.Forms.ComboBox combo_kfzb_market_silver_pos;
        public System.Windows.Forms.CheckBox chk_kfzb_market_weapon_1;
        public System.Windows.Forms.CheckBox chk_kfzb_market_gem;
        public System.Windows.Forms.CheckBox chk_kfzb_market_stone;
        public System.Windows.Forms.CheckBox chk_kfzb_market_silver;
        public System.Windows.Forms.CheckBox chk_kfzb_market;
        public System.Windows.Forms.NumericUpDown num_crossplatm_compete_gold;
        public System.Windows.Forms.NumericUpDown num_kf_banquet_buygold;
        public System.Windows.Forms.CheckBox chk_kf_banquet_nation;
        public System.Windows.Forms.CheckBox chk_crossplatm_compete;
        public System.Windows.Forms.CheckBox chk_kf_banquet_enabled;
        public System.Windows.Forms.CheckBox chk_kfwd_enable;
        public System.Windows.Forms.CheckBox chk_kfwd_buyreward;
        public System.Windows.Forms.TabPage logPage;
        private System.Windows.Forms.Label lbl_tempStatus;
        private System.Windows.Forms.Button btn_tempStop;
        private System.Windows.Forms.Button btn_tempAdd;
        public System.Windows.Forms.RichTextBox logTemp;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.RichTextBox logSurpise;
        public System.Windows.Forms.TextBox txt_servers;
        public System.Windows.Forms.RichTextBox logText;
        public System.Windows.Forms.Label lbl_playerinfo;
        public System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.Label lbl_time;
        public AxSHDocVw.AxWebBrowser subBrowser;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Label label70;
        public System.Windows.Forms.NumericUpDown nUD_BGActivity_coin;
        public System.Windows.Forms.CheckBox chk_BGActivity;
        private System.Windows.Forms.GroupBox groupBox22;
        public System.Windows.Forms.CheckBox chk_kfrank;
        public System.Windows.Forms.CheckBox chk_qingming;
        public System.Windows.Forms.NumericUpDown nUD_qm_round_coin;
        private System.Windows.Forms.Label label71;
        public System.Windows.Forms.NumericUpDown nUD_qm_drink_coin;
        private System.Windows.Forms.Label label72;
        public System.Windows.Forms.CheckBox chk_big_hero_tufei_enable;
        private System.Windows.Forms.ToolStripMenuItem menu_lua;
        private System.Windows.Forms.GroupBox groupBox23;
        public System.Windows.Forms.NumericUpDown nUD_kfrank_point;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label73;
        public System.Windows.Forms.ComboBox combo_kfrank_def_formation;
        public System.Windows.Forms.ComboBox combo_kfrank_ack_formation;
        private System.Windows.Forms.ToolStripMenuItem 练将计算器ToolStripMenuItem;
        public System.Windows.Forms.CheckBox chk_market_usetokenafter5;
        public System.Windows.Forms.NumericUpDown num_super_fanpai_buy_gold;
        public System.Windows.Forms.ComboBox combo_jailwork_type;
        private System.Windows.Forms.Label label75;
        public System.Windows.Forms.ComboBox combo_movable_weave_like;
        private System.Windows.Forms.Label label76;
        public System.Windows.Forms.NumericUpDown num_ticket_bighero;
        public System.Windows.Forms.CheckBox chk_attack_before_22;

    }
}