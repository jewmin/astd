-- �¼�������
EventCallDispatcher = {}
EventCallDispatcher.LuaExeExecute = {} -- luaExeִ���¼�

-- ע��һ���¼��ص�����
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
  -- ���ﲻ���þֲ�table���룬���򲻻��޸ĵ�EventCallDispatcher��
  table.insert(EventCallDispatcher[eventId], proc)
  return true
end

-- �Ƴ�һ����ע����¼��ص�����
EventCallDispatcher.unregisterEventCall = function(eventId, proc)
  eventId = tonumber(eventId)
  local callTable = EventCallDispatcher[eventId]
  if callTable then
    for i = 1, #callTable do
      -- ���ﲻ���þֲ�table���룬���򲻻��޸ĵ�EventCallDispatcher��
      if callTable[i] == proc then table.remove(EventCallDispatcher[eventId], i) return true end
    end
  end
  return false
end

-- һ��luaExeIdֻ��ע��һ������
EventCallDispatcher.registerLuaExeExecute = function(exeId, func)
  if EventCallDispatcher.LuaExeExecute[exeId] ~= nil then
    ILogger():logInfo("luaExe has register: "..exeId)
    return
  end
  EventCallDispatcher.LuaExeExecute[exeId] = func
end
