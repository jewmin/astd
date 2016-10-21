-- ȫ�ַ���
global = {}
global.type2name = {}
global.type2name[1] = "����"
global.type2name[2] = "��ʯ"
global.type2name[5] = "%d����ʯ"
global.type2name[6] = "����[%s]"
global.type2name[7] = "������Ƭ[%s]"
global.type2name[8] = "���մ���"
global.type2name[9] = "��֯����"
global.type2name[10] = "ͨ�̴���"
global.type2name[11] = "��������"
global.type2name[12] = "��������"
global.type2name[13] = "�������ÿ�"
global.type2name[14] = "ս��˫����"
global.type2name[15] = "ǿ��������"
global.type2name[16] = "ǿ�����ۿ�"
global.type2name[17] = "����������"
global.type2name[18] = "����������"
global.type2name[27] = "������"
global.type2name[28] = "����"
global.type2name[29] = "����������"
global.type2name[30] = "���շ�����"
global.type2name[31] = "�����ٻ���"
global.type2name[32] = "��֯������"
global.type2name[34] = "�ж���"
global.type2name[35] = "ҡǮ��"
global.type2name[36] = "������Ʊ"
global.type2name[38] = "����[%s+%d]"
global.type2name[39] = "���"
global.type2name[42] = "��ȯ"
global.type2name[43] = "���ر���"
global.type2name[44] = "�Ҵ�����"
global.type2name[48] = "%d����������"
global.type2name[50] = "����"

-- ��������xml
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

-- ��������ַ���
global.tostring = function(data)
  local info = ""
  if data ~= nil then
    for i, v in ipairs(data) do
      info = info .. string.format("%s ", global.initstring(v))
    end
  end
  return info
end

-- ���ݽ������������������
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

-- ȡ��������
global.getArmy = function(army)
  local armyname = {"��ͨ", "��Ӣ", "����"}
  if army >= 1 and army <= 3 then
    return armyname[army]
  else
    return "δ֪"
  end
end

-- ȡ����
global.getMoon = function(type)
  local moon = {"�ºڷ��;ʿ������", "��ɫ����;ʿ������", "����Ǭ��;ʿ������"}
  if type >= 1 and type <= 3 then
    return moon[type]
  else
    return "δ֪"
  end
end
