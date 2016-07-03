-- ���������ҽű�
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

ConfigStrings = luanet.import_type("com.lover.astd.common.ConfigStrings") -- �����ַ���
ActivityType = luanet.import_type("com.lover.astd.common.model.enumer.ActivityType") -- �����
ExeCode = luanet.import_type("com.lover.astd.common.model.enumer.ExeCode") -- �����

user = User() -- ���
logger = ILogger() -- ��־
proto_mgr = ProtocolMgr() -- Э�������
misc_mgr = MiscManager() -- �����Ӱ˹�����
battle_mgr = BattleManager() -- ս��������
activity_mgr = ActivityManager() -- �������
building_mgr = BuildingManager() -- ����������
troop_mgr = TroopManager() -- ����������
equip_mgr = EquipManager() -- װ��������
campaign_mgr = CampaignManager() -- ս�۹�����
big_hero_mgr = BigHeroManager() -- ��ѵ��������
hero_mgr = HeroManager() -- �佫ѵ��������

global = require("module.global") -- ȫ��ģ��
refine = require("module.refine") -- ����ģ��

movable_exe = require("exe.movable") -- �ж���

-- �ж���
function movable_execute()
	return movable_exe.execute()
end
