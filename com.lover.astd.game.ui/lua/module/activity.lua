-- 活动模块
activity = {}

activity.success = 0
activity.null = 1
activity.finish = 2
activity.error = 10

activity.event_getDuanwuEventInfo = function()
  local url = "/root/event!getDuanwuEventInfo.action"
	local result = ProtocolMgr():getXml(url, "端午百家宴-信息")
	if not result then return activity.null end
	if not result.CmdSucceed then return activity.error end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local data = {}
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/hunger")
	data.currenthunger = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/maxhunger")
	data.maxhunger = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/eatnum")
	data.eatnum = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/buyroundcost")
	data.buyroundcost = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/remainround")
	data.remainround = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/zongziinfo/hunger")
	data.hunger = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/zongziinfo/goldcost")
	data.goldcost = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/zongziinfo/goldhunger")
	data.goldhunger = xmlNode and tonumber(xmlNode.InnerText) or 0
  local xmlNodeList = cmdResult:SelectNodes("/results/rewards")
  data.rewards = {}
  if xmlNodeList ~= nil then
    for i = 1, xmlNodeList.Count do
      local childXmlNode = xmlNodeList:Item(i - 1)
      local childXmlNodeList = childXmlNode.ChildNodes
      local reward = {}
      reward.id = 0
      reward.state = 0
      for j = 1, childXmlNodeList.Count do
        local childChildXmlNode = childXmlNodeList:Item(j - 1)
        if childChildXmlNode.Name == "id" then
          reward.id = tonumber(childChildXmlNode.InnerText)
        elseif childChildXmlNode.Name == "state" then
          reward.state = tonumber(childChildXmlNode.InnerText)
        end
      end
      table.insert(data.rewards, reward)
    end
  end
  return activity.success, data
end

activity.event_eatZongzi = function(gold, use_gold)
  local url = "/root/event!eatZongzi.action"
  local data = string.format("gold=%d", gold)
  local result = ProtocolMgr():postXml(url, data, "端午百家宴-吃粽子")
	if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/reward/baoji")
	local baoji = xmlNode and tonumber(xmlNode.InnerText) or 0
	xmlNode = cmdResult:SelectSingleNode("/results/reward/tickets")
	local tickets = xmlNode and tonumber(xmlNode.InnerText) or 0
	local tips = string.format("吃粽子，获得点券+%d", tickets)
  if baoji > 1 then tips = string.format("吃粽子，%d倍暴击，获得点券+%d", baoji, tickets) end
  if gold == 1 then tips = string.format("花费%d金币，%s", use_gold, tips) end
	ILogger():logInfo(tips)
  return true
end

activity.event_buyRound = function(use_gold)
  local url = "/root/event!buyRound.action"
  local result = ProtocolMgr():getXml(url, "端午百家宴-购买轮数")
	if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo(string.format("花费%d金币购买轮数", use_gold))
  return true
end

activity.event_nextRound = function()
  local url = "/root/event!nextRound.action"
  local result = ProtocolMgr():getXml(url, "端午百家宴-再吃一轮")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo("再吃一轮")
  return true
end

activity.event_getRewardById = function(rewardId)
  local url = "/root/event!getRewardById.action"
  local data = string.format("rewardId=%d", rewardId)
  local result = ProtocolMgr():postXml(url, data, "端午百家宴-领取奖励")
	if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/reward/tickets")
	local tickets = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/reward/bigginfo/name")
  local name = xmlNode and xmlNode.InnerText or ""
  xmlNode = cmdResult:SelectSingleNode("/results/reward/bigginfo/num")
  local num = xmlNode and tonumber(xmlNode.InnerText) or 0
  local tips = "领取奖励"
  if tickets > 0 then tips = string.format("领取奖励，获得点券+%d", tickets) end
  if num > 0 then tips = string.format("%s，大将令(%s)+%d", tips, name, num) end
	ILogger():logInfo(tips)
  return true
end
