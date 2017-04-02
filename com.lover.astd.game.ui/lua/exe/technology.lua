-- 新科技
TechnologyExe = {}

TechnologyExe.name = "newtechnology"
TechnologyExe.readable = "新科技"
TechnologyExe.id = LuaExeId.eTech
TechnologyExe.exe = LuaExeFactory.createLuaExe(TechnologyExe)

TechnologyExe.execute = function()
    return EquipManager():handleNewTech(ProtocolMgr(), ILogger(), User(), equipConfig.tech_availablebintie, equipConfig.tech_consumebintie)
end

EventCallDispatcher.registerLuaExeExecute(TechnologyExe.id, TechnologyExe.execute)
