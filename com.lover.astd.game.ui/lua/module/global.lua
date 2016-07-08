-- ȫ�ַ���
global = {}

-- ��ʯ�Լ۱ȣ�����1����ʯ�ȼ۽��
global.getGemPrice = function()
	local conf = GameConfig(ConfigStrings.S_Global)
	local price = conf:ContainsKey(ConfigStrings.gem_price) and tonumber(conf.gem_price) or 100.0
	local price = price > 0.0 and 10.0 / price or 0.1
	return price
end

-- ����key���ؿ�������
global.getAvailable = function(key, value)
	local conf = GameConfig(ConfigStrings.S_Global)
	local val = conf:ContainsKey(key) and tonumber(conf[key]) or 0
	local reserve_value = value - val
	if reserve_value < 0 then reserve_value = 0 end
	return reserve_value
end

-- ���ý��
global.getGoldAvailable = function()
	return global.getAvailable(ConfigStrings.gold_reserve, User().Gold)
end

-- ������ʯ
global.getStoneAvailable = function()
	return global.getAvailable(ConfigStrings.stone_reserve, User().Stone)
end

-- ��������
global.getSilverAvailable = function()
	return global.getAvailable(ConfigStrings.silver_reserve, User().Silver)
end
