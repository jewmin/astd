-- ´óÁ·±ø
TrainingExe = {}

TrainingExe.name = "training"
TrainingExe.readable = "´óÁ·±ø"
TrainingExe.id = LuaExeId.eTraining
TrainingExe.exe = LuaExeFactory.createLuaExe(TrainingExe)

TrainingExe.execute = function()
	-- ILogger():logInfo("TrainingExe.execute")
	local result, info = activity.training_getInfo()
	if result == activity.null then
		return 60000
	elseif result == activity.error then
		return TrainingExe.exe:next_halfhour()
	elseif result == activity.success then
		local ret = true
		if info.trainingstate == 0 then
		ret, info = activity.training_start()
		end

		while info.round > 0 do
		while TrainingExe.isUpArmy(info.uparmy, info.army) do
			ret, info = activity.training_upArmy(info.uparmy)
			if not ret then return TrainingExe.exe:next_halfhour() end
		end

		ret, info = activity.training_attackArmy(info.army, info.armyidx)
		if not ret then return TrainingExe.exe:next_halfhour() end

		if info.round == 0 and TrainingExe.isAddRound(info.buyroundcost) then
			ret, info = activity.training_buyRound(info.buyroundcost)
			if not ret then return TrainingExe.exe:next_halfhour() end
		end
		end

		local len = string.len(info.flags)
		for i = 1, len do
		local flag = string.sub(info.flags, i, i)
		if flag == "1" then
			ret, info = activity.training_recHongbao(i)
			if not ret then return TrainingExe.exe:next_halfhour() end
		end
		end

		local hongbao = 0
		while info.hongbao > 0 do
		if hongbao > 8 and TrainingExe.isReset(info.resettime, info.resetcost) then
			ret = activity.training_resetReward()
			if not ret then return TrainingExe.exe:next_halfhour() end
			info.resettime = info.resettime - 1
			hongbao = 0
		end

		ret = activity.training_getReward()
		if not ret then return TrainingExe.exe:next_halfhour() end
		info.hongbao = info.hongbao - 1
		hongbao = hongbao + 1
		end
	end
	return TrainingExe.exe:next_hour()
end

TrainingExe.isAddRound = function(buyroundcost)
	if buyroundcost <= trainingConfig.buyroundcost and global.getGoldAvailable() >= buyroundcost then
		return true
	end
	return false
end

TrainingExe.isReset = function(resettime, resetcost)
	if resettime > 0 then
		return true
	elseif resetcost <= trainingConfig.resetcost and global.getGoldAvailable() >= resetcost then
		return true
	end
	return false
end

TrainingExe.isUpArmy = function(uparmy, army)
	if army < trainingConfig.uparmy.army and uparmy <= trainingConfig.uparmy.gold and global.getGoldAvailable() >= uparmy then
		return true
	end
	return false
end

EventCallDispatcher.registerLuaExeExecute(TrainingExe.id, TrainingExe.execute)
