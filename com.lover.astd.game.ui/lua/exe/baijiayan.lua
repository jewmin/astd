-- 百家宴
BaijiayanExe = {}

BaijiayanExe.name = "baijiayan"
BaijiayanExe.readable = "百家宴"
BaijiayanExe.id = LuaExeId.eBaijiayan
BaijiayanExe.exe = LuaExeFactory.createLuaExe(BaijiayanExe)

BaijiayanExe.execute = function()
  -- ILogger():logInfo("BaijiayanExe.execute")
  local result, info = activity.event_getDuanwuEventInfo()
  if result == activity.null then
    return 60000
  elseif result == activity.error then
    return BaijiayanExe.exe:next_halfhour()
  elseif result == activity.success then
    -- for i, v in ipairs(info.rewards) do
    --   if v.state == 1 then activity.event_getRewardById(v.id) end
    -- end
    --
    -- ILogger():logInfo(string.format("剩余轮数:%d，饥饿度:%d/%d，普通粽子饥饿度:%d，金币粽子饥饿度:%d，花费金币:%d"
    -- , info.remainround, info.currenthunger, info.maxhunger, info.hunger, info.goldhunger, info.goldcost))
    -- if info.remainround > 0 then
    --   if info.currenthunger > 0 then
    --     local gold = BaijiayanExe.isGoldEat(info.hunger, info.goldcost) and 1 or 0
    --     if activity.event_eatZongzi(gold, info.goldcost) then return BaijiayanExe.exe:immediate() end
    --   else
    --     if activity.event_nextRound() then return BaijiayanExe.exe:immediate() end
    --   end
    -- elseif BaijiayanExe.isAddRound(info.buyroundcost) then
    --   if activity.event_buyRound(info.buyroundcost) then return BaijiayanExe.exe:immediate() end
    -- else
    --   return BaijiayanExe.exe:next_day()
    -- end
    for i, v in ipairs(info.rewards) do
      if v.state[1] == 1 then activity.event_getRewardById(v.id[1], BaijiayanExe.getChoice(v.choice)) end
    end

    ILogger():logInfo(string.format("剩余轮数:%d，饥饿度:%d/%d，普通粽子饥饿度:%d，金币粽子饥饿度:%d，花费金币:%d"
    , info.remainround[1], info.hunger[1], info.maxhunger[1], info.zongziinfo[1].hunger[1], info.zongziinfo[1].goldhunger[1], info.zongziinfo[1].goldcost[1]))
    if info.remainround[1] > 0 then
      if info.hunger[1] > 0 then
        local gold = BaijiayanExe.isGoldEat(info.zongziinfo[1].hunger[1], info.zongziinfo[1].goldcost[1]) and 1 or 0
        if activity.event_eatZongzi(gold, info.zongziinfo[1].goldcost[1]) then return BaijiayanExe.exe:immediate() end
      else
        if activity.event_nextRound() then return BaijiayanExe.exe:immediate() end
      end
    elseif BaijiayanExe.isAddRound(info.buyroundcost[1]) then
      if activity.event_buyRound(info.buyroundcost[1]) then return BaijiayanExe.exe:immediate() end
    else
      return BaijiayanExe.exe:next_day()
    end
  end
  return BaijiayanExe.exe:next_hour()
end

BaijiayanExe.isAddRound = function(buyroundcost)
  if baijiayanConfig.add_round.open and buyroundcost <= baijiayanConfig.add_round.gold and global.getGoldAvailable() >= buyroundcost then
    return true
  end
  return false
end

BaijiayanExe.isGoldEat = function(hunger, goldcost)
  if baijiayanConfig.gold_eat.open and hunger >= baijiayanConfig.gold_eat.hunger and goldcost <= baijiayanConfig.gold_eat.gold and global.getGoldAvailable() >= goldcost then
    return true
  end
  return false
end

BaijiayanExe.getChoice = function(choices)
  for i, v in ipairs(choices) do
    for j = 1, #baijiayanConfig.select_db do
      if v.bigginfo[1].name[1] == baijiayanConfig.select_db[j] then return v.rewardid[1] end
    end
  end
  return nil
end

EventCallDispatcher.registerLuaExeExecute(BaijiayanExe.id, BaijiayanExe.execute)
