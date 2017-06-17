-- ����
ArrestEventExe = {}

ArrestEventExe.name = "arrestevent"
ArrestEventExe.readable = "����"
ArrestEventExe.id = LuaExeId.eArrestEvent
ArrestEventExe.exe = LuaExeFactory.createLuaExe(ArrestEventExe)

ArrestEventExe.execute = function()
    return ActivityManager():ArrestEventExecute(ProtocolMgr(), ILogger(), User(), global.getGoldAvailable(), arrestEventConfig.hishenlimit, arrestEventConfig.ricedumplingcostlimit, arrestEventConfig.arresttokencostlimit)
end

EventCallDispatcher.registerLuaExeExecute(ArrestEventExe.id, ArrestEventExe.execute)
