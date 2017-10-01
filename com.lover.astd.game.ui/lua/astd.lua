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
-- CommonManager() 通用
-- CreateLuaExe(string name, string readable, int id) 创建exe

luanet.load_assembly("System.Xml")
luanet.load_assembly("mscorlib")
luanet.load_assembly("com.lover.astd.common")

ConfigStrings = luanet.import_type("com.lover.astd.common.ConfigStrings") -- 配置字符集
ActivityType = luanet.import_type("com.lover.astd.common.model.enumer.ActivityType") -- 活动类型
ExeCode = luanet.import_type("com.lover.astd.common.model.enumer.ExeCode") -- 结果码
XmlNodeType = luanet.import_type("System.Xml.XmlNodeType") -- xml节点类型

InitFnTable = {}
FinaFnTable = {}
ExeTable = {}

require("config.luaexeid") -- lua执行事件Id
require("config.activity") -- 活动配置
require("config.equip") -- 装备配置
require("config.execonfig") -- 功能配置
require("module.event") -- 事件模块
require("module.global") -- 全局模块
require("module.refine") -- 炼制模块
require("module.factory") -- 工厂模块
require("module.activity") -- 活动模块
require("module.equip") -- 装备模块
require("module.world") -- 世界模块
require("exe.baijiayan") -- 百家宴
require("exe.movable") -- 百家宴
require("exe.equipment") -- 猴子套装强化
require("exe.world") -- 屠城嘉奖
require("exe.training") -- 大练兵
require("exe.moral") -- 赏月送礼
require("exe.snowtrading") -- 雪地通商
require("exe.springfestivalwish") -- 辞旧迎新
require("exe.newyearactivity") -- 新年活动
require("exe.specialequip") -- 铸造
require("exe.zhuge") -- 潜能淬炼
require("exe.technology") -- 新科技
require("exe.borrowingarrows") -- 草船借箭
require("exe.arrestevent") -- 端午
require("exe.paradeevent") -- 新国庆阅兵

-- 执行execute
function OnLuaExecute(exeId)
	-- ILogger():logInfo(string.format("OnLuaExecute(%d)", exeId))
	local func = EventCallDispatcher.LuaExeExecute[exeId]
	if func then return func() end
end

-- 添加exe
function OnAddExe(exeMgr)
	ILogger():logInfo("OnAddExe")
	for i = 1, table.getn(ExeTable) do
		ExeTable[i]:setVariables(exeMgr._proto, exeMgr._logger, exeMgr._server, exeMgr._user, exeMgr._conf, exeMgr._factory)
		ExeTable[i]:setOtherConf(exeMgr._otherConf)
		ExeTable[i]:init_data()
		exeMgr:addExe(ExeTable[i])
	end
end

-- 删除exe
function OnDelExe(exeMgr)
	ILogger():logInfo("OnDelExe")
	for i = 1, table.getn(ExeTable) do
		exeMgr:removeExe(ExeTable[i])
	end
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
