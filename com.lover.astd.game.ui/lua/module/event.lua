-- 事件分派器
EventCallDispatcher = {}
EventCallDispatcher.LuaExeExecute = {} -- luaExe执行事件

-- 注册一个事件回调函数
EventCallDispatcher.registerEventCall = function(eventId, proc)
  eventId = tonumber(eventId)
  local callTable = EventCallDispatcher[eventId]
  if not callTable or type(callTable) ~= "table" then
    EventCallDispatcher[eventId] = {}
  else
    for i = 1, #callTable do
      if callTable[i] == proc then return false end
    end
  end
  -- 这里不能用局部table插入，否则不会修改到EventCallDispatcher中
  table.insert(EventCallDispatcher[eventId], proc)
  return true
end

-- 移除一个已注册的事件回调函数
EventCallDispatcher.unregisterEventCall = function(eventId, proc)
  eventId = tonumber(eventId)
  local callTable = EventCallDispatcher[eventId]
  if callTable then
    for i = 1, #callTable do
      -- 这里不能用局部table插入，否则不会修改到EventCallDispatcher中
      if callTable[i] == proc then table.remove(EventCallDispatcher[eventId], i) return true end
    end
  end
  return false
end

-- 一个luaExeId只能注册一个函数
EventCallDispatcher.registerLuaExeExecute = function(exeId, func)
  if EventCallDispatcher.LuaExeExecute[exeId] ~= nil then
    ILogger():logInfo("luaExe has register: "..exeId)
    return
  end
  EventCallDispatcher.LuaExeExecute[exeId] = func
end
