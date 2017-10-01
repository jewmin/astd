-- �¹����ı�
ParadeEventExe = {}

ParadeEventExe.name = "paradeevent"
ParadeEventExe.readable = "�¹����ı�"
ParadeEventExe.id = LuaExeId.eParadeEvent
ParadeEventExe.exe = LuaExeFactory.createLuaExe(ParadeEventExe)

ParadeEventExe.execute = function()
    return ActivityManager():ParadeEventExecute(ProtocolMgr(), ILogger(), User(), global.getGoldAvailable(), ParadeEventConfig.costlimit, ParadeEventConfig.roundcostlimit)
end

EventCallDispatcher.registerLuaExeExecute(ParadeEventExe.id, ParadeEventExe.execute)
