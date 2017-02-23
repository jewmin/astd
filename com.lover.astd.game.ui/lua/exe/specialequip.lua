-- ж§дь
SpecialEquipExe = {}

SpecialEquipExe.name = "specialequip"
SpecialEquipExe.readable = "ж§дь"
SpecialEquipExe.id = LuaExeId.eSpecialEquip
SpecialEquipExe.exe = LuaExeFactory.createLuaExe(SpecialEquipExe)

SpecialEquipExe.execute = function()
    return EquipManager():handleSpecialEquipInfo(ProtocolMgr(), ILogger(), User(), equipConfig.special_equip_prop)
end

EventCallDispatcher.registerLuaExeExecute(SpecialEquipExe.id, SpecialEquipExe.execute)
