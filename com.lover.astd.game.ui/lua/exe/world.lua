-- Õ¿≥«ºŒΩ±
WorldExe = {}

WorldExe.name = "tucity"
WorldExe.readable = "Õ¿≥«ºŒΩ±"
WorldExe.id = LuaExeId.eTuCity
WorldExe.exe = LuaExeFactory.createLuaExe(WorldExe)

WorldExe.execute = function()
  -- ILogger():logInfo("WorldExe.execute")
  local result, data = world.getTuCityInfo()
  if result == world.success then
    for i, v in ipairs(data) do
      world.getTuCityReward(v.playerid, v.areaid)
    end
  end
  return WorldExe.exe:next_halfhour()
end

EventCallDispatcher.registerLuaExeExecute(WorldExe.id, WorldExe.execute)
