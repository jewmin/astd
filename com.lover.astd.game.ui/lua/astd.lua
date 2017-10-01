-- �������
-- User() ���
-- ILogger() ��־
-- GameConfig(string name) ��Ϸ����
-- OtherConfig(string name) ����������
-- ProtocolMgr() Э��
-- MiscManager() �����Ӱ�
-- BattleManager() ս��
-- ActivityManager() �
-- BuildingManager() ����
-- TroopManager() ����
-- EquipManager() װ��
-- CampaignManager() ս��
-- BigHeroManager() ��
-- HeroManager() �佫
-- CommonManager() ͨ��
-- CreateLuaExe(string name, string readable, int id) ����exe

luanet.load_assembly("System.Xml")
luanet.load_assembly("mscorlib")
luanet.load_assembly("com.lover.astd.common")

ConfigStrings = luanet.import_type("com.lover.astd.common.ConfigStrings") -- �����ַ���
ActivityType = luanet.import_type("com.lover.astd.common.model.enumer.ActivityType") -- �����
ExeCode = luanet.import_type("com.lover.astd.common.model.enumer.ExeCode") -- �����
XmlNodeType = luanet.import_type("System.Xml.XmlNodeType") -- xml�ڵ�����

InitFnTable = {}
FinaFnTable = {}
ExeTable = {}

require("config.luaexeid") -- luaִ���¼�Id
require("config.activity") -- �����
require("config.equip") -- װ������
require("config.execonfig") -- ��������
require("module.event") -- �¼�ģ��
require("module.global") -- ȫ��ģ��
require("module.refine") -- ����ģ��
require("module.factory") -- ����ģ��
require("module.activity") -- �ģ��
require("module.equip") -- װ��ģ��
require("module.world") -- ����ģ��
require("exe.baijiayan") -- �ټ���
require("exe.movable") -- �ټ���
require("exe.equipment") -- ������װǿ��
require("exe.world") -- ���Ǽν�
require("exe.training") -- ������
require("exe.moral") -- ��������
require("exe.snowtrading") -- ѩ��ͨ��
require("exe.springfestivalwish") -- �Ǿ�ӭ��
require("exe.newyearactivity") -- ����
require("exe.specialequip") -- ����
require("exe.zhuge") -- Ǳ�ܴ���
require("exe.technology") -- �¿Ƽ�
require("exe.borrowingarrows") -- �ݴ����
require("exe.arrestevent") -- ����
require("exe.paradeevent") -- �¹����ı�

-- ִ��execute
function OnLuaExecute(exeId)
	-- ILogger():logInfo(string.format("OnLuaExecute(%d)", exeId))
	local func = EventCallDispatcher.LuaExeExecute[exeId]
	if func then return func() end
end

-- ���exe
function OnAddExe(exeMgr)
	ILogger():logInfo("OnAddExe")
	for i = 1, table.getn(ExeTable) do
		ExeTable[i]:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
		ExeTable[i]:setOtherConf(exeMgr._otherConf)
		ExeTable[i]:init_data()
		exeMgr:addExe(ExeTable[i])
	end
end

-- ɾ��exe
function OnDelExe(exeMgr)
	ILogger():logInfo("OnDelExe")
	for i = 1, table.getn(ExeTable) do
		exeMgr:removeExe(ExeTable[i])
	end
end

-- ��ʼ������
function initialization(exeMgr)
	for i = 1, table.getn(InitFnTable) do
		InitFnTable[i](exeMgr)
	end
end

-- ����������
function finalization(exeMgr)
	for i = 1, table.getn(FinaFnTable) do
		FinaFnTable[i](exeMgr)
	end
end

table.insert(InitFnTable, OnAddExe)
table.insert(FinaFnTable, OnDelExe)

table.insert(ExeTable, BaijiayanExe.exe)
table.insert(ExeTable, MovableExe.exe)
table.insert(ExeTable, EquipmentExe.exe)
table.insert(ExeTable, WorldExe.exe)
table.insert(ExeTable, TrainingExe.exe)
table.insert(ExeTable, MoralExe.exe)
table.insert(ExeTable, SnowTradingExe.exe)
table.insert(ExeTable, SpringFestivalWishExe.exe)
table.insert(ExeTable, NewYearActivityExe.exe)
-- table.insert(ExeTable, SpecialEquipExe.exe)
table.insert(ExeTable, ZhugeExe.exe)
table.insert(ExeTable, TechnologyExe.exe)
table.insert(ExeTable, BorrowingArrowsExe.exe)
table.insert(ExeTable, ArrestEventExe.exe)
table.insert(ExeTable, ParadeEventExe.exe)
