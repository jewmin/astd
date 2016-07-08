-- ����ģ��
refine = {}

refine.null = 1
refine.error = 10
refine.finish = 2
refine.success = 0

refine.getRefineBintieFactory = function()
	local url = "/root/refine!getRefineBintieFactory.action"
	local result = ProtocolMgr():getXml(url, "�߼����ƹ���-��Ϣ")

	if not result then return refine.null end
	if not result.CmdSucceed then return refine.error end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/remainhigh")
	local remainhigh = xmlNode and tonumber(xmlNode.InnerText) or 0
	if remainhigh == 0 then return refine.finish end

	if refine.doRefineBintieFactory(0) then return refine.success end
	return refine.error
end

refine.doRefineBintieFactory = function(mode)
	local url = "/root/refine!doRefineBintieFactory.action"
	local data = string.format("mode=%d", mode)
	local result = ProtocolMgr():postXml(url, data, "�߼����ƹ���-����")
	if not result or not result.CmdSucceed then return false end

	-- ILogger():logInfo(result.CmdResult.InnerXml)
	local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/cri")
	local cri = xmlNode and tonumber(xmlNode.InnerText) or 0
	xmlNode = cmdResult:SelectSingleNode("/results/basebintie")
	local basebintie = xmlNode and tonumber(xmlNode.InnerText) or 0
	local tips = string.format("�߼����ƣ��������+%d", basebintie * cri)
	if cri > 1 then tips = string.format("�߼����ƣ�%d���������������+%d", cri, basebintie * cri) end
	ILogger():logInfo(tips)
	return true
end
