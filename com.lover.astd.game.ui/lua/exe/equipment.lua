-- ������װǿ��
EquipmentExe = {}

EquipmentExe.name = "monkeyequipment"
EquipmentExe.readable = "������װǿ��"
EquipmentExe.id = LuaExeId.eEquipment
EquipmentExe.exe = LuaExeFactory.createLuaExe(EquipmentExe)

EquipmentExe.execute = function()
	return CommonManager():equip_handleMonkeyTao(ProtocolMgr(), ILogger(), equipConfig.leave_tickets, equipConfig.use_tickets)
end

EventCallDispatcher.registerLuaExeExecute(EquipmentExe.id, EquipmentExe.execute)
