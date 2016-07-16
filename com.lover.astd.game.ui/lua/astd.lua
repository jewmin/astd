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

InitFnTable = {}
FinaFnTable = {}

require("config.luaexeid") -- luaִ���¼�Id
require("config.activity") -- �����
require("module.event") -- �¼�ģ��
require("module.global") -- ȫ��ģ��
require("module.refine") -- ����ģ��
require("module.factory") -- ����ģ��
require("module.activity") -- �ģ��
require("exe.baijiayan") -- �ټ���
require("exe.movable") -- �ټ���

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
end

-- ɾ��exe
function OnDelExe(exeMgr)
	ILogger():logInfo("OnDelExe")
	exeMgr:removeExe(BaijiayanExe.exe)
	exeMgr:removeExe(MovableExe.exe)
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
