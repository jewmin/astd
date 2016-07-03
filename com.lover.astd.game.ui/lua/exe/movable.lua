-- 行动力
local movable_exe = {}

movable_exe.execute = function()
  local conf = GameConfig(ConfigStrings.S_Movable)
	if not conf.enabled or string.lower(conf.enabled) ~= "true" then
		return ExeCode.next_hour
	end

	-- 纺织
	local weave_price = conf:ContainsKey(ConfigStrings.trade_visit) and tonumber(conf.weave_price) or 130 -- 纺织布价
	local weave_count = conf:ContainsKey(ConfigStrings.weave_count) and tonumber(conf.weave_count) or 0 -- 纺织次数
	-- 精炼
	local refine_reserve = conf:ContainsKey(ConfigStrings.refine_reserve) and tonumber(conf.refine_reserve) or 0 -- 保留精炼次数
	-- 炼制
	local refine_item = conf:ContainsKey(ConfigStrings.refine_item) and conf.refine_item ~= "" and conf.refine_item or "无敌将军炮" -- 炼制物品
	local refine_gold_limit = conf:ContainsKey(ConfigStrings.gold_refine_limit) and tonumber(conf.gold_refine_limit) or 0 -- 金币炼制上限
	local refine_stone_limit = conf:ContainsKey(ConfigStrings.stone_refine_limit) and tonumber(conf.stone_refine_limit) or 0 -- 玉石炼制上限
	-- 通商
	local winter_trade = conf:ContainsKey(ConfigStrings.winter_trade) and string.lower(conf.winter_trade) == "true" or false -- 冬季优先通商
	local arch_trade = false -- 考古优先通商
	local arch_trade_max = false -- 考古自动升级驿站并极限通商
	local arch_refill = false -- 考古自动补充行动力
	if user:isActivityRunning(ActivityType.ArchEvent) then
		arch_trade = conf:ContainsKey(ConfigStrings.arch_trade) and string.lower(conf.arch_trade) == "true" or false
		arch_trade_max = conf:ContainsKey(ConfigStrings.arch_trade_max) and string.lower(conf.arch_trade_max) == "true" or false
		arch_refill = conf:ContainsKey(ConfigStrings.arch_refill) and string.lower(conf.arch_refill) == "true" or false
	end
	local visit_merchants = conf:ContainsKey(ConfigStrings.trade_visit) and conf.trade_visit or "" -- 拜访商盟
	local max_visit_fail_count = conf:ContainsKey(ConfigStrings.trade_visit_fail) and tonumber(conf.trade_visit_fail) or 0 -- 允许失败次数

	local movable_order = conf:ContainsKey(ConfigStrings.order) and conf.order or "2341" -- 行动力顺序
	local reserve = conf:ContainsKey(ConfigStrings.reserve) and tonumber(conf.reserve) or 0 -- 保留行动力

	local is_new_trade = user._is_new_trade -- 新通商
	local is_refine_bintie = user._is_refine_bintie -- 炼制镔铁

	if user.CurMovable <= reserve then
		logger:logInfo("已经达到保留行动力下限, 下个小时再检测")
		return ExeCode.next_hour
	end

	for i = 1, #movable_order do
		local order = string.sub(movable_order, i, i)
		if order == "1" then -- 通商
			if is_new_trade then
				local result, _, boxnum = misc_mgr:handleNewTradeInfo(proto_mgr, logger, user, visit_merchants, max_visit_fail_count, global.getSilverAvailable(), global.getGoldAvailable())
				while boxnum > 0 do
					_, boxnum = misc_mgr:openNewTradeBox(proto_mgr, logger)
				end
				if result == 0 then
					return ExeCode.immediate
				elseif result == 1 then
					return ExeCode.one_minute
				elseif result == 2 then
					return ExeCode.next_hour
				elseif result == 10 then
					return ExeCode.next_halfhour
				end
			else
				local result = misc_mgr:handleTradeInfo(proto_mgr, logger, user, false, false)
				if result == 0 then
					return ExeCode.immediate
				elseif result == 1 then
					return ExeCode.one_minute
				elseif result == 3 then
					return ExeCode.next_halfhour
				elseif result == 10 then
					return ExeCode.next_halfhour
				end
			end
		elseif order == "2" then -- 纺织
			local result = misc_mgr:handleWeaveInfo(proto_mgr, logger, user, weave_price, weave_count, false, false)
			if result == 0 then
				return ExeCode.immediate
			elseif result == 1 then
				return ExeCode.one_minute
			elseif result == 3 then
				return ExeCode.next_halfhour
			elseif result == 4 then
				return ExeCode.next_halfhour
			elseif result == 10 then
				return ExeCode.next_halfhour
			end
		elseif order == "3" then -- 炼制
			if is_refine_bintie then
				local result = refine.getRefineBintieFactory()
				if result == 0 then
					return ExeCode.immediate
				elseif result == 1 then
					return ExeCode.one_minute
				elseif result == 10 then
					return ExeCode.next_halfhour
				end
			else
				local result = misc_mgr:handleRefineFactoryInfo(proto_mgr, logger, user, refine_gold_limit, refine_stone_limit, global.getGoldAvailable(), global.getStoneAvailable(), refine_item, global.getSilverAvailable(), false)
				if result == 0 then
					return ExeCode.immediate
				elseif result == 1 then
					return ExeCode.one_minute
				elseif result == 3 then
					return ExeCode.next_halfhour
				elseif result == 4 then
					return ExeCode.next_halfhour
				elseif result == 10 then
					return ExeCode.next_halfhour
				end
			end
		elseif order == "4" then -- 精炼
			local result = misc_mgr:handleRefineInfo(equip_mgr, proto_mgr, logger, user, global.getGemPrice(), global.getGoldAvailable(), refine_reserve, false)
			if result == 0 then
				return ExeCode.immediate
			elseif result == 1 then
				return ExeCode.one_minute
			elseif result == 3 then
				return ExeCode.next_halfhour
			elseif result == 4 then
				return ExeCode.one_minute
			elseif result == 10 then
				return ExeCode.next_halfhour
			end
		elseif order == "5" then -- 璞玉
			local result = misc_mgr:gamblingStone(proto_mgr, logger, user)
			if result == 0 then
				return ExeCode.immediate
			elseif result == 1 then
				return ExeCode.one_minute
			elseif result == 3 then
				return ExeCode.next_halfhour
			elseif result == 4 then
				return ExeCode.one_minute
			elseif result == 10 then
				return ExeCode.next_halfhour
			end
		end
	end
	return ExeCode.next_hour
end

return movable_exe
