-- ����
SpecialEquipExe = {}

SpecialEquipExe.name = "specialequip"
SpecialEquipExe.readable = "����"
SpecialEquipExe.id = LuaExeId.eSpecialEquip
SpecialEquipExe.exe = LuaExeFactory.createLuaExe(SpecialEquipExe)

SpecialEquipExe.execute = function()
    return EquipManager():handleSpecialEquipInfo(ProtocolMgr(), ILogger(), User())
end

EventCallDispatcher.registerLuaExeExecute(SpecialEquipExe.id, SpecialEquipExe.execute)
