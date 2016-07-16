-- 傲视天地
-- User() 玩家
-- ILogger() 日志
-- GameConfig(string name) 游戏配置
-- OtherConfig(string name) 黑名单配置
-- ProtocolMgr() 协议
-- MiscManager() 杂七杂八
-- BattleManager() 战斗
-- ActivityManager() 活动
-- BuildingManager() 建筑
-- TroopManager() 兵力
-- EquipManager() 装备
-- CampaignManager() 战役
-- BigHeroManager() 大将
-- HeroManager() 武将
-- CreateLuaExe(string name, string readable, int id) 创建exe

luanet.load_assembly("System.Xml")
luanet.load_assembly("mscorlib")
luanet.load_assembly("com.lover.astd.common")

ConfigStrings = luanet.import_type("com.lover.astd.common.ConfigStrings") -- 配置字符集
ActivityType = luanet.import_type("com.lover.astd.common.model.enumer.ActivityType") -- 活动类型
ExeCode = luanet.import_type("com.lover.astd.common.model.enumer.ExeCode") -- 结果码

InitFnTable = {}
FinaFnTable = {}

require("config.luaexeid") -- lua执行事件Id
require("config.activity") -- 活动配置
require("module.event") -- 事件模块
require("module.global") -- 全局模块
require("module.refine") -- 炼制模块
require("module.factory") -- 工厂模块
require("module.activity") -- 活动模块
require("exe.baijiayan") -- 百家宴
require("exe.movable") -- 百家宴

-- 执行execute
function OnLuaExecute(exeId)
	-- ILogger():logInfo(string.format("OnLuaExecute(%d)", exeId))
	local func = EventCallDispatcher.LuaExeExecute[exeId]
	if func then return func() end
end

-- 添加exe
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

-- 删除exe
function OnDelExe(exeMgr)
	ILogger():logInfo("OnDelExe")
	exeMgr:removeExe(BaijiayanExe.exe)
	exeMgr:removeExe(MovableExe.exe)
end

-- 初始化函数
function initialization(exeMgr)
	for i = 1, table.getn(InitFnTable) do
		InitFnTable[i](exeMgr)
	end
end

-- 析构化函数
function finalization(exeMgr)
	for i = 1, table.getn(FinaFnTable) do
		FinaFnTable[i](exeMgr)
	end
end

table.insert(InitFnTable, OnAddExe)
table.insert(FinaFnTable, OnDelExe)
