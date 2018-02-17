-- 新春拜年
MemoryEventExe = {}

MemoryEventExe.name = "memoryevent"
MemoryEventExe.readable = "新春拜年"
MemoryEventExe.id = LuaExeId.eMemoryEvent
MemoryEventExe.exe = LuaExeFactory.createLuaExe(MemoryEventExe)

MemoryEventExe.execute = function()
	return ActivityManager():MemoryEventExecute(ProtocolMgr(), ILogger(), User(), MemoryEventConfig.wish_cost, MemoryEventConfig.hongbao_cost)
end

EventCallDispatcher.registerLuaExeExecute(MemoryEventExe.id, MemoryEventExe.execute)
