-- Ç±ÄÜ´ãÁ¶
ZhugeExe = {}

ZhugeExe.name = "zhuge"
ZhugeExe.readable = "Ç±ÄÜ´ãÁ¶"
ZhugeExe.id = LuaExeId.eZhuge
ZhugeExe.exe = LuaExeFactory.createLuaExe(ZhugeExe)

ZhugeExe.execute = function()
    return HeroManager():handleXiZhuge(ProtocolMgr(), ILogger(), User())
end

EventCallDispatcher.registerLuaExeExecute(ZhugeExe.id, ZhugeExe.execute)
