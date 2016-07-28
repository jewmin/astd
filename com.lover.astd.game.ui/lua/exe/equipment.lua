-- 百家宴
EquipmentExe = {}

EquipmentExe.name = "monkeyequipment"
EquipmentExe.readable = "猴子套装强化"
EquipmentExe.id = LuaExeId.eEquipment
EquipmentExe.exe = LuaExeFactory.createLuaExe(EquipmentExe)

EquipmentExe.execute = function()
  -- ILogger():logInfo("EquipmentExe.execute")
  local result, info = equip.getUpgradeInfo()
  if result == equip.null then
    return 60000
  elseif result == equip.error then
    return EquipmentExe.exe:next_halfhour()
  elseif result == equip.success then
    if info.playerequipdtos == nil then
      ILogger():logInfo("还没有猴子套装")
      return EquipmentExe.exe:next_hour()
    end

    if info.ticketnumber < equipConfig.leave_tickets then
      ILogger():logInfo(string.format("当前拥有点券%d < %d", info.ticketnumber, equipConfig.leave_tickets))
      return EquipmentExe.exe:next_hour()
    end

    local upgrade = false
    for i, v in ipairs(info.playerequipdtos) do
      local monkeyname = string.format("%s(%s)", v.equipname, v.generalname)
      if v.xuli >= v.maxxuli then
        equip.useXuli(monkeyname, v.composite)
      end

      if v.tickets <= equipConfig.use_tickets then
        upgrade = true
        equip.upgradeMonkeyTao(monkeyname, v.composite, 0)
      end
    end

    if upgrade then
      return EquipmentExe.exe:immediate()
    else
      return EquipmentExe.exe:next_day()
    end
  end
  return EquipmentExe.exe:next_hour()
end

EventCallDispatcher.registerLuaExeExecute(EquipmentExe.id, EquipmentExe.execute)
