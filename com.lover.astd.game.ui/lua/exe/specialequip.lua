-- ����
SpecialEquipExe = {}

SpecialEquipExe.name = "specialequip"
SpecialEquipExe.readable = "����"
SpecialEquipExe.id = LuaExeId.eSpecialEquip
SpecialEquipExe.exe = LuaExeFactory.createLuaExe(SpecialEquipExe)

SpecialEquipExe.execute = function()
	-- return EquipManager():handleSpecialEquipInfo(ProtocolMgr(), ILogger(), User(), equipConfig.special_equip_prop)
	return EquipManager():getSpecialEquipCastInfo(ProtocolMgr(), ILogger(), User(), equipConfig.firstcost_limit, equipConfig.secondcost_limit)
end

EventCallDispatcher.registerLuaExeExecute(SpecialEquipExe.id, SpecialEquipExe.execute)
