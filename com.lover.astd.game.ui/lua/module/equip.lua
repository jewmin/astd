-- 装备模块
equip = {}

equip.null = 1
equip.error = 10
equip.finish = 2
equip.success = 0

equip.getUpgradeInfo = function()
  local url = "/root/equip!getUpgradeInfo.action"
	local result = ProtocolMgr():getXml(url, "装备-信息")

	if not result then return equip.null end
	if not result.CmdSucceed then return equip.error end

	ILogger():logInfo(result.CmdResult.InnerXml)
  local data = {}
	local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/ticketnumber")
	data.ticketnumber = xmlNode and tonumber(xmlNode.InnerText) or 0

  local xmlNodeList = cmdResult:SelectNodes("/results/playerequipdto")
  data.playerequipdtos = {}
  if xmlNodeList ~= nil then
    for i = 1, xmlNodeList.Count do
      local childXmlNode = xmlNodeList:Item(i - 1)
      local childXmlNodeList = childXmlNode.ChildNodes
      local playerequipdto = {}
      for j = 1, childXmlNodeList.Count do
        local childChildXmlNode = childXmlNodeList:Item(j - 1)
        if childChildXmlNode.Name == "composite" then
          playerequipdto.composite = tonumber(childChildXmlNode.InnerText)
        elseif childChildXmlNode.Name == "equipname" then
          playerequipdto.equipname = childChildXmlNode.InnerText
        elseif childChildXmlNode.Name == "generalname" then
          playerequipdto.generalname = childChildXmlNode.InnerText
        elseif childChildXmlNode.Name == "monkeylv" then
          playerequipdto.monkeylv = tonumber(childChildXmlNode.InnerText)
        elseif childChildXmlNode.Name == "tickets" then
          playerequipdto.tickets = tonumber(childChildXmlNode.InnerText)
        elseif childChildXmlNode.Name == "xuli" then
          playerequipdto.xuli = tonumber(childChildXmlNode.InnerText)
        elseif childChildXmlNode.Name == "maxxuli" then
          playerequipdto.maxxuli = tonumber(childChildXmlNode.InnerText)
        end
      end
      table.insert(data.playerequipdtos, playerequipdto)
    end
  end

	return equip.success, data
end

equip.upgradeMonkeyTao = function(name, composite, num)
  local url = "/root/equip!upgradeMonkeyTao.action"
  local data = string.format("composite=%d&num=%d", composite, num)
  local result = ProtocolMgr():postXml(url, data, "装备-强化猴子套装")
	if not result or not result.CmdSucceed then return false end

  ILogger():logInfo(result.CmdResult.InnerXml)
  local tips = string.format("强化%s成功", name)
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/baoji")
	local baoji = xmlNode and tonumber(xmlNode.InnerText) or 0
  if baoji > 1 then
    tips = tips .. string.format("，%d倍暴击", baoji)
  end

  local xmlNodeList = cmdResult:SelectNodes("/results/addinfo")
  if xmlNodeList ~= nil then
    for i = 1, xmlNodeList.Count do
      local childXmlNode = xmlNodeList:Item(i - 1)
      local childXmlNodeList = childXmlNode.ChildNodes
      local addinfo = {}
      for j = 1, childXmlNodeList.Count do
        local childChildXmlNode = childXmlNodeList:Item(j - 1)
        if childChildXmlNode.Name == "name" then
          addinfo.name = childChildXmlNode.InnerText
        elseif childChildXmlNode.Name == "val" then
          addinfo.val = childChildXmlNode.InnerText
        end
      end
      tips = tips .. "，" .. string.format("%s+%s", equip.getCarAttrName(addinfo.name), addinfo.val or "未知数值")
    end
  end

	ILogger():logInfo(tips)
  return true
end

equip.useXuli = function(name, composite)
  local url = "/root/equip!useXuli.action"
  local data = string.format("composite=%d", composite)
  local result = ProtocolMgr():postXml(url, data, "装备-使用蓄力")
	if not result or not result.CmdSucceed then return false end

  ILogger():logInfo(result.CmdResult.InnerXml)
  local tips = string.format("%s使用蓄力", name)
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/getevent")
  local getevent = xmlNode and tonumber(xmlNode.InnerText) or 0
  if getevent == 1 then
    xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/gethighnum")
    local gethighnum = xmlNode and tonumber(xmlNode.InnerText) or 0
    if gethighnum > 0 then tips = tips .. string.format("，普通强化次数+%d", gethighnum) end
  elseif getevent == 2 then
    xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/addinfo/val")
    local val = xmlNode and tonumber(xmlNode.InnerText) or 0
    xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/addinfo/name")
    if val > 0 then tips = tips .. string.format("，%s+%d", equip.getCarAttrName(xmlNode.InnerText), val) end
  elseif getevent == 3 then
    xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/newattr")
    local newattr = global.split(xmlNode.InnerText, ",")
    for i, v in ipairs(newattr) do
      if tonumber(v) > 0 then tips = tips .. string.format("，%s+%s", equip.getAttrName(i), v) end
    end
  end
  ILogger():logInfo(tips)
  return true
end

equip.getCarAttrName = function(name)
  if name == "att" then
    return "普攻"
  elseif name == "def" then
    return "普防"
  elseif name == "satt" then
    return "战攻"
  elseif name == "sdef" then
    return "战防"
  elseif name == "stgatt" then
    return "策攻"
  elseif name == "stgdef" then
    return "策防"
  elseif name == "hp" then
    return "兵力"
  end
end

equip.getAttrName = function(num)
  if num == 1 then
    return "统"
  elseif num == 2 then
    return "勇"
  elseif num == 3 then
    return "智"
  end
end
