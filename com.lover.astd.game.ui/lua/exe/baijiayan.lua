-- 百家宴
BaijiayanExe = {}

BaijiayanExe.name = "baijiayan"
BaijiayanExe.readable = "百家宴"
BaijiayanExe.id = LuaExeId.eBaijiayan
BaijiayanExe.exe = LuaExeFactory.createLuaExe(BaijiayanExe)

BaijiayanExe.execute = function()
  ILogger():logInfo("BaijiayanExe.execute")
  return BaijiayanExe.exe:next_halfhour()
end

EventCallDispatcher.registerLuaExeExecute(BaijiayanExe.id, BaijiayanExe.execute)
