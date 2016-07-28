-- 全局方法
global = {}

-- 宝石性价比，返回1级宝石等价金币
global.getGemPrice = function()
	local conf = GameConfig(ConfigStrings.S_Global)
	local price = conf:ContainsKey(ConfigStrings.gem_price) and tonumber(conf.gem_price) or 100.0
	local price = price > 0.0 and 10.0 / price or 0.1
	return price
end

-- 根据key返回可用数量
global.getAvailable = function(key, value)
	local conf = GameConfig(ConfigStrings.S_Global)
	local val = conf:ContainsKey(key) and tonumber(conf[key]) or 0
	local reserve_value = value - val
	if reserve_value < 0 then reserve_value = 0 end
	return reserve_value
end

-- 可用金币
global.getGoldAvailable = function()
	return global.getAvailable(ConfigStrings.gold_reserve, User().Gold)
end

-- 可用玉石
global.getStoneAvailable = function()
	return global.getAvailable(ConfigStrings.stone_reserve, User().Stone)
end

-- 可用银币
global.getSilverAvailable = function()
	return global.getAvailable(ConfigStrings.silver_reserve, User().Silver)
end

global.split = function(str, separator)
	local t = {}
	local i = 0
	local j = 0
	while true do
		j = string.find(str, separator, i + 1)
		if j == nil then break end
		table.insert(t, string.sub(str, i + 1, j - 1))
		i = j
	end
	return t
end
