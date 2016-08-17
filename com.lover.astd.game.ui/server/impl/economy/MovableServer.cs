using com.lover.astd.common.config;
using System;
using System.Collections.Generic;

namespace com.lover.astd.game.ui.server.impl.economy
{
	public class MovableServer : LogicServer
	{
        public MovableServer(NewMainForm frm)
		{
			this._mainForm = frm;
			this.ServerName = "movable";
			this.ServerReadableName = "行动力";
		}

		public override void renderSettings()
		{
			Dictionary<string, string> config = base.getConfig(this.ServerName);
			if (config.Count == 0)
			{
				return;
			}
            //行动力开启
            _mainForm.chk_movable_enable.Checked = (config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true"));
			//行动力顺序
            if (config.ContainsKey("order"))
			{
                _mainForm.txt_movable_order.Text = config["order"];
			}
            //炼制物品
			if (config.ContainsKey("refine_item") && config["refine_item"] != "")
			{
                _mainForm.combo_movable_refine_item.SelectedItem = config["refine_item"];
			}
			int value = 130;
			if (config.ContainsKey("weave_price"))
			{
				int.TryParse(config["weave_price"], out value);
			}
            //纺织布价
            _mainForm.num_movable_weave_price.Value = value;
			int num = 10;
			if (config.ContainsKey("refine_reserve"))
			{
				int.TryParse(config["refine_reserve"], out num);
			}
			if (num < 3)
			{
				num = 3;
			}
            //保留精炼次数
            _mainForm.num_movable_refine_reserve.Value = num;
            //_mainForm.chk_movable_winter_trade.Checked = (config.ContainsKey("winter_trade") && config["winter_trade"].ToLower().Equals("true"));
            //_mainForm.chk_movable_arch_trade.Checked = (config.ContainsKey("arch_trade") && config["arch_trade"].ToLower().Equals("true"));
            //_mainForm.chk_movable_arch_refill.Checked = (config.ContainsKey("arch_refill") && config["arch_refill"].ToLower().Equals("true"));
            //_mainForm.chk_movable_arch_trade_max.Checked = (config.ContainsKey("arch_trade_max") && config["arch_trade_max"].ToLower().Equals("true"));
			//拜访商盟
            if (config.ContainsKey("trade_visit"))
			{
                _mainForm.txt_movable_visit.Text = config["trade_visit"];
			}
			int value2 = 1;
			if (config.ContainsKey("trade_visit_fail"))
			{
				int.TryParse(config["trade_visit_fail"], out value2);
			}
            //失败次数
            _mainForm.num_movable_visit_fail.Value = value2;
			int value3 = 0;
			if (config.ContainsKey("reserve"))
			{
				int.TryParse(config["reserve"], out value3);
			}
            //保留行动力
            _mainForm.num_movable_reserve.Value = value3;
			int value4 = 0;
			if (config.ContainsKey("weave_count"))
			{
				int.TryParse(config["weave_count"], out value4);
			}
            //纺织次数
            _mainForm.num_movable_weave_count.Value = value4;
			int value5 = 0;
			if (config.ContainsKey("gold_refine_limit"))
			{
				int.TryParse(config["gold_refine_limit"], out value5);
			}
            //金币炼制上限
            _mainForm.num_movable_refine_factory_gold.Value = value5;
			int value6 = 0;
			if (config.ContainsKey("stone_refine_limit"))
			{
				int.TryParse(config["stone_refine_limit"], out value6);
			}
            //玉石炼制上限
            _mainForm.num_movable_refine_factory_stone.Value = value6;
		}

		public override void saveSettings()
		{
			GameConfig config = base.getConfig();
            config.setConfig(this.ServerName, "enabled", _mainForm.chk_movable_enable.Checked.ToString());
            config.setConfig(this.ServerName, "order", _mainForm.txt_movable_order.Text);
            if (_mainForm.combo_movable_refine_item.SelectedIndex >= 0)
			{
                config.setConfig(this.ServerName, "refine_item", _mainForm.combo_movable_refine_item.SelectedItem.ToString());
			}
			else
			{
				config.setConfig(this.ServerName, "refine_item", "");
			}
            config.setConfig(this.ServerName, "weave_price", _mainForm.num_movable_weave_price.Value.ToString());
            config.setConfig(this.ServerName, "refine_reserve", _mainForm.num_movable_refine_reserve.Value.ToString());
            //config.setConfig(this.ServerName, "winter_trade", _mainForm.chk_movable_winter_trade.Checked.ToString());
            //config.setConfig(this.ServerName, "arch_trade", _mainForm.chk_movable_arch_trade.Checked.ToString());
            //config.setConfig(this.ServerName, "arch_refill", _mainForm.chk_movable_arch_refill.Checked.ToString());
            //config.setConfig(this.ServerName, "arch_trade_max", _mainForm.chk_movable_arch_trade_max.Checked.ToString());
            config.setConfig(this.ServerName, "winter_trade", "false");
            config.setConfig(this.ServerName, "arch_trade", "false");
            config.setConfig(this.ServerName, "arch_refill", "false");
            config.setConfig(this.ServerName, "arch_trade_max", "false");
            config.setConfig(this.ServerName, "trade_visit", _mainForm.txt_movable_visit.Text);
            config.setConfig(this.ServerName, "trade_visit_fail", _mainForm.num_movable_visit_fail.Value.ToString());
            config.setConfig(this.ServerName, "reserve", _mainForm.num_movable_reserve.Value.ToString());
            config.setConfig(this.ServerName, "weave_count", _mainForm.num_movable_weave_count.Value.ToString());
            config.setConfig(this.ServerName, "gold_refine_limit", _mainForm.num_movable_refine_factory_gold.Value.ToString());
            config.setConfig(this.ServerName, "stone_refine_limit", _mainForm.num_movable_refine_factory_stone.Value.ToString());
		}

		public override void loadDefaultSettings()
		{
			GameConfig expr_06 = base.getConfig();
			expr_06.setConfig(this.ServerName, "enabled", "true");
			expr_06.setConfig(this.ServerName, "order", "2341");
			expr_06.setConfig(this.ServerName, "reserve", "0");
			expr_06.setConfig(this.ServerName, "refine_item", "无敌将军炮");
			expr_06.setConfig(this.ServerName, "gold_refine_limit", "0");
			expr_06.setConfig(this.ServerName, "stone_refine_limit", "0");
			expr_06.setConfig(this.ServerName, "weave_count", "20");
			expr_06.setConfig(this.ServerName, "weave_price", "130");
			expr_06.setConfig(this.ServerName, "refine_reserve", "10");
			expr_06.setConfig(this.ServerName, "winter_trade", "false");
			expr_06.setConfig(this.ServerName, "arch_trade", "false");
			expr_06.setConfig(this.ServerName, "arch_refill", "false");
			expr_06.setConfig(this.ServerName, "arch_trade_max", "false");
			expr_06.setConfig(this.ServerName, "trade_visit", "");
			expr_06.setConfig(this.ServerName, "trade_visit_fail", "1");
			this.renderSettings();
		}
	}
}
