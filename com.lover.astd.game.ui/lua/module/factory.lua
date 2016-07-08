-- luaExeBase生成工厂
LuaExeFactory = {}

LuaExeFactory.createLuaExe = function(exeTable)
  local exe = CreateLuaExe(exeTable.name, exeTable.readable, exeTable.id)
  if exe == nil then
    ILogger():logInfo(string.format("CreateLuaExe(%s) error", exeTable.readable))
    return nil
  else
    -- ILogger():logInfo(string.format("CreateLuaExe(%s) success", exeTable.readable))
    return exe
  end
end
