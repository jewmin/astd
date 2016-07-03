-- 傲视天地外挂脚本
-- User()
-- ILogger()
-- GameConfig(string name)
-- OtherConfig(string name)
-- ProtocolMgr()
-- MiscManager()
-- BattleManager()
-- ActivityManager()
-- BuildingManager()
-- TroopManager()
-- EquipManager()
-- CampaignManager()
-- BigHeroManager()
-- HeroManager()

luanet.load_assembly("System.Xml")
luanet.load_assembly("mscorlib")
luanet.load_assembly("com.lover.astd.common")

ConfigStrings = luanet.import_type("com.lover.astd.common.ConfigStrings") -- 配置字符集
ActivityType = luanet.import_type("com.lover.astd.common.model.enumer.ActivityType") -- 活动类型
ExeCode = luanet.import_type("com.lover.astd.common.model.enumer.ExeCode") -- 结果码

user = User() -- 玩家
logger = ILogger() -- 日志
proto_mgr = ProtocolMgr() -- 协议管理器
misc_mgr = MiscManager() -- 杂七杂八管理器
battle_mgr = BattleManager() -- 战斗管理器
activity_mgr = ActivityManager() -- 活动管理器
building_mgr = BuildingManager() -- 建筑管理器
troop_mgr = TroopManager() -- 兵力管理器
equip_mgr = EquipManager() -- 装备管理器
campaign_mgr = CampaignManager() -- 战役管理器
big_hero_mgr = BigHeroManager() -- 大将训练管理器
hero_mgr = HeroManager() -- 武将训练管理器

global = require("module.global") -- 全局模块
refine = require("module.refine") -- 炼制模块

movable_exe = require("exe.movable") -- 行动力

-- 行动力
function movable_execute()
	return movable_exe.execute()
end
