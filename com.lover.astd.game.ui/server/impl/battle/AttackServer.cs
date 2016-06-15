using com.lover.astd.common.config;
using System;
using System.Collections.Generic;
using com.lover.astd.common;

namespace com.lover.astd.game.ui.server.impl.battle
{
	internal class AttackServer : LogicServer
	{
        public AttackServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = ConfigStrings.S_Attack;
			this.ServerReadableName = ConfigStrings.SR_Attack;
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
            _mainForm.chk_attack_player.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
            _mainForm.chk_attack_player_cityevent.Checked = (config.ContainsKey("city_event") && config["city_event"].ToLower().Equals("true"));
            _mainForm.chk_Nation.Checked = (config.ContainsKey("nation_event") && config["nation_event"].ToLower().Equals("true"));
			int city_event_maxstar = 3;
			if (config.ContainsKey("city_event_maxstar"))
			{
                int.TryParse(config["city_event_maxstar"], out city_event_maxstar);
			}
            int city_event_reserved_num = 0;
			if (config.ContainsKey("city_event_reserved_num"))
			{
                int.TryParse(config["city_event_reserved_num"], out city_event_reserved_num);
			}
            _mainForm.num_attack_cityevent_maxstar.Value = city_event_maxstar;
            _mainForm.nUD_reserved_num.Value = city_event_reserved_num;
            _mainForm.chk_attack_player_gongjian.Checked = (config.ContainsKey("gongjian") && config["gongjian"].ToLower().Equals("true"));
            _mainForm.chk_attack_enable_attack.Checked = (config.ContainsKey("enable_attack") && config["enable_attack"].ToLower().Equals("true"));
            _mainForm.chk_attack_get_extra_order.Checked = (config.ContainsKey("get_extra_order") && config["get_extra_order"].ToLower().Equals("true"));
            _mainForm.chk_attack_npc_enemy.Checked = (config.ContainsKey("attack_npc") && config["attack_npc"].ToLower().Equals("true"));
            _mainForm.chk_attack_player_use_token.Checked = (config.ContainsKey("use_token") && config["use_token"].ToLower().Equals("true"));
            _mainForm.chk_attack_player_move_tokenfull.Checked = (config.ContainsKey("auto_move") && config["auto_move"].ToLower().Equals("true"));
            _mainForm.chk_attack_not_injail.Checked = (config.ContainsKey("not_injail") && config["not_injail"].ToLower().Equals("true"));
            _mainForm.chk_attack_jail_tech.Checked = (config.ContainsKey("jail_tech") && config["jail_tech"].ToLower().Equals("true"));
            int level_min = 0;
            int level_max = 0;
			int min_score = 5000;
			if (config.ContainsKey("level_min"))
			{
                int.TryParse(config["level_min"], out level_min);
			}
			if (config.ContainsKey("level_max"))
			{
                int.TryParse(config["level_max"], out level_max);
			}
			if (config.ContainsKey("min_score"))
			{
				int.TryParse(config["min_score"], out min_score);
			}
            _mainForm.num_attack_player_level_min.Value = level_min;
            _mainForm.num_attack_player_level_max.Value = level_max;
            _mainForm.num_attack_score.Value = min_score;
			if (config.ContainsKey("move_target"))
			{
                _mainForm.lbl_attack_target.Text = config["move_target"].Trim();
			}
            int attack_reserve_token = 0;
			if (config.ContainsKey("attack_reserve_token"))
			{
                int.TryParse(config["attack_reserve_token"], out attack_reserve_token);
			}
            _mainForm.num_attack_reserve_token.Value = attack_reserve_token;
            _mainForm.chk_attack_reserve_token.Checked = (config.ContainsKey("attack_reserve_token_enable") && config["attack_reserve_token_enable"].ToLower().Equals("true"));
			if (config.ContainsKey("attack_filter_type"))
			{
                int attack_filter_type = 0;
                int.TryParse(config["attack_filter_type"], out attack_filter_type);
                if (attack_filter_type < 0 || attack_filter_type > _mainForm.combo_attack_filter_type.Items.Count - 1)
				{
                    attack_filter_type = 0;
				}
                _mainForm.combo_attack_filter_type.SelectedIndex = attack_filter_type;
				if (config.ContainsKey("attack_filter_content"))
				{
                    _mainForm.txt_attack_filter_content.Text = config["attack_filter_content"];
				}
			}
            _mainForm.chk_juedou.Checked = (config.ContainsKey(ConfigStrings.juedou) && config[ConfigStrings.juedou].ToLower().Equals("true"));
		}

		public override void saveSettings()
		{
			GameConfig expr_0D = base.getConfig();
            expr_0D.setConfig(this.ServerName, "enabled", _mainForm.chk_attack_player.Checked.ToString());//开启
            expr_0D.setConfig(this.ServerName, "city_event", _mainForm.chk_attack_player_cityevent.Checked.ToString());//自动悬赏
            expr_0D.setConfig(this.ServerName, "nation_event", _mainForm.chk_Nation.Checked.ToString());//自动天降奇兵
            expr_0D.setConfig(this.ServerName, "city_event_maxstar", _mainForm.num_attack_cityevent_maxstar.Value.ToString());//最高星级
            expr_0D.setConfig(this.ServerName, "city_event_reserved_num", _mainForm.nUD_reserved_num.Value.ToString());//悬赏保留次数
            expr_0D.setConfig(this.ServerName, "gongjian", _mainForm.chk_attack_player_gongjian.Checked.ToString());//自动攻坚战
            expr_0D.setConfig(this.ServerName, "enable_attack", _mainForm.chk_attack_enable_attack.Checked.ToString());//自动攻击玩家
            expr_0D.setConfig(this.ServerName, "get_extra_order", _mainForm.chk_attack_get_extra_order.Checked.ToString());//领取马车攻击令
            expr_0D.setConfig(this.ServerName, "level_min", _mainForm.num_attack_player_level_min.Value.ToString());//攻击玩家最小等级
            expr_0D.setConfig(this.ServerName, "level_max", _mainForm.num_attack_player_level_max.Value.ToString());//攻击玩家最大等级
            expr_0D.setConfig(this.ServerName, "attack_npc", _mainForm.chk_attack_npc_enemy.Checked.ToString());//攻击守备军
            expr_0D.setConfig(this.ServerName, "use_token", _mainForm.chk_attack_player_use_token.Checked.ToString());//使用令牌
            expr_0D.setConfig(this.ServerName, "auto_move", _mainForm.chk_attack_player_move_tokenfull.Checked.ToString());//自动移动
            expr_0D.setConfig(this.ServerName, "not_injail", _mainForm.chk_attack_not_injail.Checked.ToString());//不打被抓的
            expr_0D.setConfig(this.ServerName, "min_score", _mainForm.num_attack_score.Value.ToString());//战绩
            expr_0D.setConfig(this.ServerName, "move_target", _mainForm.lbl_attack_target.Text);//移动目标
            expr_0D.setConfig(this.ServerName, "jail_tech", _mainForm.chk_attack_jail_tech.Checked.ToString());//劳作技术研究
            expr_0D.setConfig(this.ServerName, "attack_reserve_token_enable", _mainForm.chk_attack_reserve_token.Checked.ToString());//保留军令
            expr_0D.setConfig(this.ServerName, "attack_reserve_token", _mainForm.num_attack_reserve_token.Value.ToString());//军令数
            expr_0D.setConfig(this.ServerName, "attack_filter_type", _mainForm.combo_attack_filter_type.SelectedIndex.ToString());//定向攻击类型
            expr_0D.setConfig(this.ServerName, "attack_filter_content", _mainForm.txt_attack_filter_content.Text);//攻击目标
            expr_0D.setConfig(this.ServerName, ConfigStrings.juedou, _mainForm.chk_juedou.Checked.ToString());//决斗
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "city_event", "true");
			expr_06.setConfig(this.ServerName, "nation_event", "true");
			expr_06.setConfig(this.ServerName, "city_event_maxstar", "3");
			expr_06.setConfig(this.ServerName, "city_event_reserved_num", "0");
			expr_06.setConfig(this.ServerName, "gongjian", "true");
			expr_06.setConfig(this.ServerName, "enable_attack", "true");
			expr_06.setConfig(this.ServerName, "get_extra_order", "true");
			expr_06.setConfig(this.ServerName, "level_min", "0");
			expr_06.setConfig(this.ServerName, "level_max", base.getUser().Level.ToString());
			expr_06.setConfig(this.ServerName, "attack_npc", "true");
			expr_06.setConfig(this.ServerName, "use_token", "true");
			expr_06.setConfig(this.ServerName, "auto_move", "true");
			expr_06.setConfig(this.ServerName, "not_injail", "true");
			expr_06.setConfig(this.ServerName, "min_score", "5000");
			expr_06.setConfig(this.ServerName, "move_target", "");
			expr_06.setConfig(this.ServerName, "jail_tech", "false");
			expr_06.setConfig(this.ServerName, "attack_reserve_token_enable", "false");
			expr_06.setConfig(this.ServerName, "attack_reserve_token", "0");
			expr_06.setConfig(this.ServerName, "attack_filter_type", "0");
			expr_06.setConfig(this.ServerName, "attack_filter_content", "");
            expr_06.setConfig(this.ServerName, ConfigStrings.juedou, "false");
			this.renderSettings();
		}
	}
}
