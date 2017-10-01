-- 新国庆阅兵
ParadeEventExe = {}

ParadeEventExe.name = "paradeevent"
ParadeEventExe.readable = "新国庆阅兵"
ParadeEventExe.id = LuaExeId.eParadeEvent
ParadeEventExe.exe = LuaExeFactory.createLuaExe(ParadeEventExe)

ParadeEventExe.execute = function()
    return ActivityManager():ParadeEventExecute(ProtocolMgr(), ILogger(), User(), global.getGoldAvailable(), ParadeEventConfig.costlimit, ParadeEventConfig.roundcostlimit)
end

EventCallDispatcher.registerLuaExeExecute(ParadeEventExe.id, ParadeEventExe.execute)
