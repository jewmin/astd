-- »€¡∂◊® Ù
SmeltSpecialEquipExe = {}

SmeltSpecialEquipExe.name = "smeltspecialequip"
SmeltSpecialEquipExe.readable = "»€¡∂◊® Ù"
SmeltSpecialEquipExe.id = LuaExeId.eSmeltSpecialEquip
SmeltSpecialEquipExe.exe = LuaExeFactory.createLuaExe(SmeltSpecialEquipExe)

SmeltSpecialEquipExe.execute = function()
	return EquipManager():getAllSpecialEquip(ProtocolMgr(), ILogger(), User(), equipConfig.equiplevel_limit, equipConfig.quality_limit)
end

EventCallDispatcher.registerLuaExeExecute(SmeltSpecialEquipExe.id, SmeltSpecialEquipExe.execute)
