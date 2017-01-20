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

require("config.luaexeid") -- luaִ���¼�Id
require("config.activity") -- �����
require("config.equip") -- װ������
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

-- ִ��execute
function OnLuaExecute(exeId)
	-- ILogger():logInfo(string.format("OnLuaExecute(%d)", exeId))
	local func = EventCallDispatcher.LuaExeExecute[exeId]
	if func then return func() end
end

-- ���exe
function OnAddExe(exeMgr)
	ILogger():logInfo("OnAddExe")
	BaijiayanExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	BaijiayanExe.exe:setOtherConf(exeMgr._otherConf);
	BaijiayanExe.exe:init_data()
	exeMgr:addExe(BaijiayanExe.exe)

	MovableExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	MovableExe.exe:setOtherConf(exeMgr._otherConf);
	MovableExe.exe:init_data()
	exeMgr:addExe(MovableExe.exe)

	EquipmentExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	EquipmentExe.exe:setOtherConf(exeMgr._otherConf);
	EquipmentExe.exe:init_data()
	exeMgr:addExe(EquipmentExe.exe)

	WorldExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	WorldExe.exe:setOtherConf(exeMgr._otherConf);
	WorldExe.exe:init_data()
	exeMgr:addExe(WorldExe.exe)

	TrainingExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	TrainingExe.exe:setOtherConf(exeMgr._otherConf);
	TrainingExe.exe:init_data()
	exeMgr:addExe(TrainingExe.exe)

	MoralExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	MoralExe.exe:setOtherConf(exeMgr._otherConf);
	MoralExe.exe:init_data()
	exeMgr:addExe(MoralExe.exe)

	SnowTradingExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	SnowTradingExe.exe:setOtherConf(exeMgr._otherConf);
	SnowTradingExe.exe:init_data()
	exeMgr:addExe(SnowTradingExe.exe)

	SpringFestivalWishExe.exe:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
	SpringFestivalWishExe.exe:setOtherConf(exeMgr._otherConf);
	SpringFestivalWishExe.exe:init_data()
	exeMgr:addExe(SpringFestivalWishExe.exe)
end

-- ɾ��exe
function OnDelExe(exeMgr)
	ILogger():logInfo("OnDelExe")
	exeMgr:removeExe(BaijiayanExe.exe)
	exeMgr:removeExe(MovableExe.exe)
	exeMgr:removeExe(EquipmentExe.exe)
	exeMgr:removeExe(WorldExe.exe)
	exeMgr:removeExe(TrainingExe.exe)
	exeMgr:removeExe(MoralExe.exe)
	exeMgr:removeExe(SnowTradingExe.exe)
	exeMgr:removeExe(SpringFestivalWishExe.exe)
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
