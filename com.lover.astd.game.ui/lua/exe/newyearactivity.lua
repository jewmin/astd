-- 新年活动
NewYearActivityExe = {}

NewYearActivityExe.name = "newyearactivity"
NewYearActivityExe.readable = "新年活动"
NewYearActivityExe.id = LuaExeId.eNewYear
NewYearActivityExe.exe = LuaExeFactory.createLuaExe(NewYearActivityExe)

NewYearActivityExe.execute = function()
    local result = ActivityManager():getBombNianInfo(ProtocolMgr(), ILogger(), User(), 1, 5, 10)
    if result == 1 then
        return NewYearActivityExe.exe:next_hour()
    elseif result == 10 then
        return NewYearActivityExe.exe:next_hour()
    elseif result == 2 then
        return NewYearActivityExe.exe:next_day()
    else
        return NewYearActivityExe.exe:immediate()
    end
end

EventCallDispatcher.registerLuaExeExecute(NewYearActivityExe.id, NewYearActivityExe.execute)
