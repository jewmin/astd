-- װ��ģ��
equip = {}

equip.null = 1
equip.error = 10
equip.finish = 2
equip.success = 0

equip.getUpgradeInfo = function()
	local url = "/root/equip!getUpgradeInfo.action"
	local result = ProtocolMgr():getXml(url, "װ��-��Ϣ")

	if not result then return equip.null end
	if not result.CmdSucceed then return equip.error end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
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
	local result = ProtocolMgr():postXml(url, data, "װ��-ǿ��������װ")
	if not result or not result.CmdSucceed then return false end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local tips = string.format("ǿ��%s�ɹ�", name)
	local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/baoji")
	local baoji = xmlNode and tonumber(xmlNode.InnerText) or 0
	if baoji > 1 then
		tips = tips .. string.format("��%d������", baoji)
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
		tips = tips .. "��" .. string.format("%s+%s", equip.getCarAttrName(addinfo.name), addinfo.val or "δ֪��ֵ")
		end
	end

	ILogger():logInfo(tips)
	return true
end

equip.useXuli = function(name, composite)
	local url = "/root/equip!useXuli.action"
	local data = string.format("composite=%d", composite)
	local result = ProtocolMgr():postXml(url, data, "װ��-ʹ������")
	if not result or not result.CmdSucceed then return false end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local tips = string.format("%sʹ������", name)
	local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/getevent")
	local getevent = xmlNode and tonumber(xmlNode.InnerText) or 0
	if getevent == 1 then
		xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/gethighnum")
		local gethighnum = xmlNode and tonumber(xmlNode.InnerText) or 0
		if gethighnum > 0 then tips = tips .. string.format("����ͨǿ������+%d", gethighnum) end
	elseif getevent == 2 then
		xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/addinfo/val")
		local val = xmlNode and tonumber(xmlNode.InnerText) or 0
		xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/addinfo/name")
		if val > 0 then tips = tips .. string.format("��%s+%d", equip.getCarAttrName(xmlNode.InnerText), val) end
	elseif getevent == 3 then
		xmlNode = cmdResult:SelectSingleNode("/results/xuliinfo/newattr")
		local newattr = global.split(xmlNode.InnerText, ",")
		for i, v in ipairs(newattr) do
		if tonumber(v) > 0 then tips = tips .. string.format("��%s+%s", equip.getAttrName(i), v) end
		end
	end
	ILogger():logInfo(tips)
	return true
end

equip.getCarAttrName = function(name)
	if name == "att" then
		return "�չ�"
	elseif name == "def" then
		return "�շ�"
	elseif name == "satt" then
		return "ս��"
	elseif name == "sdef" then
		return "ս��"
	elseif name == "stgatt" then
		return "�߹�"
	elseif name == "stgdef" then
		return "�߷�"
	elseif name == "hp" then
		return "����"
	end
end

equip.getAttrName = function(num)
	if num == 1 then
		return "ͳ"
	elseif num == 2 then
		return "��"
	elseif num == 3 then
		return "��"
	end
end
