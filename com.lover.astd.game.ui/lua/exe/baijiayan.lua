-- �ټ���
BaijiayanExe = {}

BaijiayanExe.name = "baijiayan"
BaijiayanExe.readable = "�ټ���"
BaijiayanExe.id = LuaExeId.eBaijiayan
BaijiayanExe.exe = LuaExeFactory.createLuaExe(BaijiayanExe)

BaijiayanExe.execute = function()
  ILogger():logInfo("BaijiayanExe.execute")
  return BaijiayanExe.exe:next_halfhour()
end

EventCallDispatcher.registerLuaExeExecute(BaijiayanExe.id, BaijiayanExe.execute)
