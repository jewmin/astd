-- 新年敲钟
RingEventExe = {}

RingEventExe.name = "ringevent"
RingEventExe.readable = "新年敲钟"
RingEventExe.id = LuaExeId.eRingEvent
RingEventExe.exe = LuaExeFactory.createLuaExe(RingEventExe)

RingEventExe.execute = function()
	return ActivityManager():RingEventExecute(ProtocolMgr(), ILogger(), User(), RingEventConfig.random_ring_cost, RingEventConfig.other_ring_cost, RingEventConfig.progress_choose)
end

EventCallDispatcher.registerLuaExeExecute(RingEventExe.id, RingEventExe.execute)
