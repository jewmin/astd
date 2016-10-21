-- 全局方法
global = {}
global.type2name = {}
global.type2name[1] = "银币"
global.type2name[2] = "玉石"
global.type2name[5] = "%d级宝石"
global.type2name[6] = "兵器[%s]"
global.type2name[7] = "兵器碎片[%s]"
global.type2name[8] = "征收次数"
global.type2name[9] = "纺织次数"
global.type2name[10] = "通商次数"
global.type2name[11] = "炼化次数"
global.type2name[12] = "兵力减少"
global.type2name[13] = "副本重置卡"
global.type2name[14] = "战役双倍卡"
global.type2name[15] = "强化暴击卡"
global.type2name[16] = "强化打折卡"
global.type2name[17] = "兵器提升卡"
global.type2name[18] = "兵器暴击卡"
global.type2name[27] = "进货令"
global.type2name[28] = "军令"
global.type2name[29] = "政绩翻倍卡"
global.type2name[30] = "征收翻倍卡"
global.type2name[31] = "商人召唤卡"
global.type2name[32] = "纺织翻倍卡"
global.type2name[34] = "行动力"
global.type2name[35] = "摇钱树"
global.type2name[36] = "超级门票"
global.type2name[38] = "宝物[%s+%d]"
global.type2name[39] = "金币"
global.type2name[42] = "点券"
global.type2name[43] = "神秘宝箱"
global.type2name[44] = "家传玉佩"
global.type2name[48] = "%d倍暴击铁锤"
global.type2name[50] = "镔铁"

-- 解析奖励xml
global.handleXmlNode = function(xmlNode)
  local list = xmlNode:SelectNodes("reward");
  if list == nil then return nil end
  local data = {}
  for i = 1, list.Count do
    local reward = {}
    reward.type = 0
    reward.name = ""
    reward.quality = 1
    reward.level = 1
    reward.num = 0
    local node = list:Item(i - 1)
    local attr = node:SelectSingleNode("type")
    if attr ~= nil then reward.type = tonumber(attr.InnerText) end
    attr = node:SelectSingleNode("lv")
    if attr ~= nil then reward.level = tonumber(attr.InnerText) end
    attr = node:SelectSingleNode("num")
    if attr ~= nil then reward.num = tonumber(attr.InnerText) end
    attr = node:SelectSingleNode("itemname")
    if attr ~= nil then reward.name = attr.InnerText end
    attr = node:SelectSingleNode("quality")
    if attr ~= nil then reward.quality = tonumber(attr.InnerText) end
    table.insert(data, reward)
  end
  return data
end

-- 输出奖励字符串
global.tostring = function(data)
  local info = ""
  if data ~= nil then
    for i, v in ipairs(data) do
      info = info .. string.format("%s ", global.initstring(v))
    end
  end
  return info
end

-- 根据奖励类型输出奖励内容
global.initstring = function(reward)
  local name = global.type2name[reward.type] or ""
  if reward.type == 5 or reward.type == 48 then
    name = string.format(global.type2name[reward.type], reward.level)
  elseif reward.type == 6 or reward.type == 7 then
    name = string.format(global.type2name[reward.type], reward.name)
  elseif reward.type == 38 then
    name = string.format(global.type2name[reward.type], reward.name, reward.level)
  end
  return string.format("%s+%d", name, reward.num)
end

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

-- 取部队名称
global.getArmy = function(army)
  local armyname = {"普通", "精英", "首领"}
  if army >= 1 and army <= 3 then
    return armyname[army]
  else
    return "未知"
  end
end

-- 取月亮
global.getMoon = function(type)
  local moon = {"月黑风高;士气低落", "月色朦胧;士气规整", "月满乾坤;士气高涨"}
  if type >= 1 and type <= 3 then
    return moon[type]
  else
    return "未知"
  end
end
