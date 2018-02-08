-- 世界模块
world = {}

world.null = 1
world.error = 10
world.finish = 2
world.success = 0

world.getTuCityInfo = function()
	local url = "/root/world!getTuCityInfo.action"
	local result = ProtocolMgr():getXml(url, "屠城嘉奖-信息")

	if not result then return world.null end
	if not result.CmdSucceed then return world.error end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local data = {}
	local cmdResult = result.CmdResult
	local xmlNodeList = cmdResult:SelectNodes("/results/info")
	if xmlNodeList ~= nil then
		for i = 1, xmlNodeList.Count do
			local childXmlNode = xmlNodeList:Item(i - 1)
			local childXmlNodeList = childXmlNode.ChildNodes
			local info = {}
			info.playerid = 0
			info.areaid = 0
			for j = 1, childXmlNodeList.Count do
				local childChildXmlNode = childXmlNodeList:Item(j - 1)
				if childChildXmlNode.Name == "playerid" then
					info.playerid = tonumber(childChildXmlNode.InnerText)
				elseif childChildXmlNode.Name == "areaid" then
					info.areaid = tonumber(childChildXmlNode.InnerText)
				end
			end
			table.insert(data, info)
		end
	end
	return world.success, data
end

world.getTuCityReward = function(playerId, areaId)
	local url = "/root/world!getTuCityReward.action"
	local data = string.format("playerId=%d&areaId=%d", playerId, areaId)
	local result = ProtocolMgr():postXml(url, data, "屠城嘉奖-搜刮")
	if not result or not result.CmdSucceed then return false end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/rewardinfo")
	local reward = global.handleXmlNode(xmlNode)
	ILogger():logInfo(string.format("屠城嘉奖，获得 %s", global.tostring(reward)))
	return true
end
