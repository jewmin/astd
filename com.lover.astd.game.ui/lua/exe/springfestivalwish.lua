-- ´Ç¾ÉÓ­ÐÂ
SpringFestivalWishExe = {}

SpringFestivalWishExe.name = "springfestivalwish"
SpringFestivalWishExe.readable = "´Ç¾ÉÓ­ÐÂ"
SpringFestivalWishExe.id = LuaExeId.eSpringFestivalWish
SpringFestivalWishExe.exe = LuaExeFactory.createLuaExe(SpringFestivalWishExe)

SpringFestivalWishExe.execute = function()
    local result = ActivityManager():getSpringFestivalWishInfo(ProtocolMgr(), ILogger(), User())
    if result == 10 then
        return SpringFestivalWishExe.exe:next_halfhour()
    elseif result == 2 then
        return SpringFestivalWishExe.exe:next_day()
    else
        return SpringFestivalWishExe.exe:immediate()
    end
end

EventCallDispatcher.registerLuaExeExecute(SpringFestivalWishExe.id, SpringFestivalWishExe.execute)
