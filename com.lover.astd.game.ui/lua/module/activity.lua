-- �ģ��
activity = {}

activity.success = 0
activity.null = 1
activity.finish = 2
activity.error = 10

activity.event_getDuanwuEventInfo = function()
  local url = "/root/event!getDuanwuEventInfo.action"
	local result = ProtocolMgr():getXml(url, "����ټ���-��Ϣ")
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
  local result = ProtocolMgr():postXml(url, data, "����ټ���-������")
	if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/reward/baoji")
	local baoji = xmlNode and tonumber(xmlNode.InnerText) or 0
	xmlNode = cmdResult:SelectSingleNode("/results/reward/tickets")
	local tickets = xmlNode and tonumber(xmlNode.InnerText) or 0
	local tips = string.format("�����ӣ���õ�ȯ+%d", tickets)
  if baoji > 1 then tips = string.format("�����ӣ�%d����������õ�ȯ+%d", baoji, tickets) end
  if gold == 1 then tips = string.format("����%d��ң�%s", use_gold, tips) end
	ILogger():logInfo(tips)
  return true
end

activity.event_buyRound = function(use_gold)
  local url = "/root/event!buyRound.action"
  local result = ProtocolMgr():getXml(url, "����ټ���-��������")
	if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo(string.format("����%d��ҹ�������", use_gold))
  return true
end

activity.event_nextRound = function()
  local url = "/root/event!nextRound.action"
  local result = ProtocolMgr():getXml(url, "����ټ���-�ٳ�һ��")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo("�ٳ�һ��")
  return true
end

activity.event_getRewardById = function(rewardId)
  local url = "/root/event!getRewardById.action"
  local data = string.format("rewardId=%d", rewardId)
  local result = ProtocolMgr():postXml(url, data, "����ټ���-��ȡ����")
	if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  local cmdResult = result.CmdResult
	local xmlNode = cmdResult:SelectSingleNode("/results/reward/tickets")
	local tickets = xmlNode and tonumber(xmlNode.InnerText) or 0
  xmlNode = cmdResult:SelectSingleNode("/results/reward/bigginfo/name")
  local name = xmlNode and xmlNode.InnerText or ""
  xmlNode = cmdResult:SelectSingleNode("/results/reward/bigginfo/num")
  local num = xmlNode and tonumber(xmlNode.InnerText) or 0
  local tips = "��ȡ����"
  if tickets > 0 then tips = string.format("��ȡ��������õ�ȯ+%d", tickets) end
  if num > 0 then tips = string.format("%s������(%s)+%d", tips, name, num) end
	ILogger():logInfo(tips)
  return true
end

activity.training_parseXml = function(cmdResult)
  local data = {}
  -- �����������ѽ��
  local xmlNode = cmdResult:SelectSingleNode("/results/training/buyroundcost")
  data.buyroundcost = xmlNode and tonumber(xmlNode.InnerText) or 100
  -- ���ý������ѽ��
  xmlNode = cmdResult:SelectSingleNode("/results/training/resetcost")
  data.resetcost = xmlNode and tonumber(xmlNode.InnerText) or 20
  -- �������ӻ��ѽ��
  xmlNode = cmdResult:SelectSingleNode("/results/training/uparmy")
  data.uparmy = xmlNode and tonumber(xmlNode.InnerText) or 10
  -- �Ѿ��������
  xmlNode = cmdResult:SelectSingleNode("/results/training/buycount")
  data.buycount = xmlNode and tonumber(xmlNode.InnerText) or 0
  -- ����
  xmlNode = cmdResult:SelectSingleNode("/results/training/round")
  data.round = xmlNode and tonumber(xmlNode.InnerText) or 0
  -- �������
  xmlNode = cmdResult:SelectSingleNode("/results/training/hongbao")
  data.hongbao = xmlNode and tonumber(xmlNode.InnerText) or 0
  -- ������Ϣ 1:��ͨ 2:��Ӣ 3:����
  xmlNode = cmdResult:SelectSingleNode("/results/training/aramy")
  data.aramy = xmlNode and xmlNode.InnerText
  local myarmy = 1
  local myarmyidx = 1
  if data.aramy ~= "null" then
    local len = string.len(data.aramy)
    myarmy = tonumber(string.sub(data.aramy, myarmyidx, myarmyidx))
    for i = 2, len do
      local army = tonumber(string.sub(data.aramy, i, i))
      if myarmy < army then
        myarmy = army
        myarmyidx = i
      end
    end
  end
  data.army = myarmy
  data.armyidx = myarmyidx
  -- ս������
  xmlNode = cmdResult:SelectSingleNode("/results/training/flag")
  data.flag = xmlNode and tonumber(xmlNode.InnerText) or 0
  -- �״̬
  xmlNode = cmdResult:SelectSingleNode("/results/training/trainingstate")
  data.trainingstate = xmlNode and tonumber(xmlNode.InnerText) or 0
  -- ս�콱�����״̬ 0:δ�ﵽ 1:����ȡ 2:����ȡ
  xmlNode = cmdResult:SelectSingleNode("/results/training/flags")
  data.flags = xmlNode and xmlNode.InnerText or "0000"
  -- ����״̬
  xmlNode = cmdResult:SelectSingleNode("/results/training/reward")
  data.reward = xmlNode and xmlNode.InnerText or "00000000"
  --������ô���
  xmlNode = cmdResult:SelectSingleNode("/results/training/resettime")
  data.resettime = xmlNode and tonumber(xmlNode.InnerText) or 0
  -- ר��1
  xmlNode = cmdResult:SelectSingleNode("/results/training/special1")
  data.special1 = xmlNode and xmlNode.InnerText or ""
  -- ר��2
  xmlNode = cmdResult:SelectSingleNode("/results/training/special2")
  data.special2 = xmlNode and xmlNode.InnerText or ""
  return data
end

activity.training_getInfo = function()
  local url = "/root/training!getInfo.action"
  local result = ProtocolMgr():getXml(url, "������-�����Ϣ")
  if not result or not result.CmdSucceed then return activity.error end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  local data = activity.training_parseXml(result.CmdResult)
  return activity.success, data
end

activity.training_start = function()
  local url = "/root/training!start.action"
  local result = ProtocolMgr():getXml(url, "������-��ʼ")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo("������ ��ʼ")
  local data = activity.training_parseXml(result.CmdResult)
  return true, data
end

activity.training_attackArmy = function(army, armyidx)
  local url = "/root/training!attackArmy.action"
  local data = string.format("army=%d", armyidx)
  local result = ProtocolMgr():postXml(url, data, "������-��������")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo(string.format("������ ����%s����", global.getArmy(army)))
  local data = activity.training_parseXml(result.CmdResult)
  return true, data
end

activity.training_recHongbao = function(hongbao)
  local url = "/root/training!recHongbao.action"
  local data = string.format("hongbao=%d", hongbao)
  local result = ProtocolMgr():postXml(url, data, "������-��ȡս�콱�����")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo("������ ��ȡս�콱�����")
  local data = activity.training_parseXml(result.CmdResult)
  return true, data
end

activity.training_getReward = function()
  local url = "/root/training!getReward.action"
  local result = ProtocolMgr():getXml(url, "������-�򿪺��")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  local cmdResult = result.CmdResult
  local xmlNode = cmdResult:SelectSingleNode("/results/rewardinfo")
  local reward = global.handleXmlNode(xmlNode)
  ILogger():logInfo(string.format("������ �򿪺������� %s", global.tostring(reward)))
  return true
end

activity.training_resetReward = function()
  local url = "/root/training!resetReward.action"
  local result = ProtocolMgr():getXml(url, "������-���ý���")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo("������ ���ý���")
  return true
end

activity.training_buyRound = function(buyroundcost)
  local url = "/root/training!buyRound.action"
  local result = ProtocolMgr():getXml(url, "������-��������")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo(string.format("������ ���� %d ��ҹ�������", buyroundcost))
  local data = activity.training_parseXml(result.CmdResult)
  return true, data
end

activity.training_upArmy = function(uparmy)
  local url = "/root/training!upArmy.action"
  local result = ProtocolMgr():getXml(url, "������-��������")
  if not result or not result.CmdSucceed then return false end

  -- ILogger():logInfo(result.CmdResult.InnerXml)
  ILogger():logInfo(string.format("������ ���� %d �����������", uparmy))
  local data = activity.training_parseXml(result.CmdResult)
  return true, data
end
