-- 雪地通商
SnowTradingExe = {}

SnowTradingExe.name = "snowtrading"
SnowTradingExe.readable = "雪地通商"
SnowTradingExe.id = LuaExeId.eSnowTrading
SnowTradingExe.exe = LuaExeFactory.createLuaExe(SnowTradingExe)

SnowTradingExe.execute = function()
    local result = ActivityManager():snowTradingGetSnowTradingInfo(ProtocolMgr(), ILogger(), User(), snowTradingConfig.buyroundcost, global.getGoldAvailable(), snowTradingConfig.reinforce, snowTradingConfig.reinforcecost, snowTradingConfig.choose)
    if result == 1 then
        return SnowTradingExe.exe:next_halfhour()
    elseif result == 10 then
        return SnowTradingExe.exe:next_halfhour()
    elseif result == 2 then
        return SnowTradingExe.exe:next_halfhour()
    elseif result == 3 then
        return SnowTradingExe.exe:next_day()
    else
        return SnowTradingExe.exe:immediate()
    end
end

EventCallDispatcher.registerLuaExeExecute(SnowTradingExe.id, SnowTradingExe.execute)
