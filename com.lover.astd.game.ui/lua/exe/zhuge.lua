-- Ǳ�ܴ���
ZhugeExe = {}

ZhugeExe.name = "zhuge"
ZhugeExe.readable = "Ǳ�ܴ���"
ZhugeExe.id = LuaExeId.eZhuge
ZhugeExe.exe = LuaExeFactory.createLuaExe(ZhugeExe)

ZhugeExe.execute = function()
    return HeroManager():handleXiZhuge(ProtocolMgr(), ILogger(), User())
end

EventCallDispatcher.registerLuaExeExecute(ZhugeExe.id, ZhugeExe.execute)
