-- ÉÍÔÂËÍÀñ
MoralExe = {}

MoralExe.name = "moral"
MoralExe.readable = "ÉÍÔÂËÍÀñ"
MoralExe.id = LuaExeId.eMoral
MoralExe.exe = LuaExeFactory.createLuaExe(MoralExe)

MoralExe.execute = function()
	-- ILogger():logInfo("MoralExe.execute")
	local result, info = activity.event_getMGEventInfo()
	if not result then
		return MoralExe.exe:next_halfhour()
	else
		local getAllReward = true
		for i, v in ipairs(info.moralinfolist) do
			if v.state == 0 then
				getAllReward = false
			elseif v.state == 1 then
				result = activity.event_recvMoralReward(i)
				if not result then return MoralExe.exe:next_halfhour() end
			end
		end

		if not getAllReward then
			if info.freecakenum > 0 then
				result = activity.event_eatMoonCake(0)
				if not result then return MoralExe.exe:next_halfhour() end
			elseif MoralExe.isGoldCake(info.moontype, info.cakecost) then
				result = activity.event_eatMoonCake(info.cakecost)
				if not result then return MoralExe.exe:next_halfhour() end
			else
				getAllReward = true
			end
		end

		if getAllReward then
			if info.havenextg == 1 then
				result = activity.event_nextGeneral(0)
				if not result then return MoralExe.exe:next_halfhour() end
			elseif MoralExe.isAddRound(info.buyroundcost) then
				result = activity.event_nextGeneral(info.buyroundcost)
				if not result then return MoralExe.exe:next_halfhour() end
			else
				return MoralExe.exe:next_day()
			end
		end
	end
	return MoralExe.exe:immediate()
end

MoralExe.isAddRound = function(buyroundcost)
	if buyroundcost <= moralConfig.buyroundcost and global.getGoldAvailable() >= buyroundcost then
		return true
	end
	return false
end

MoralExe.isGoldCake = function(moontype, cakecost)
	if moontype >= moralConfig.moral.moontype and cakecost <= moralConfig.moral.cakecost and global.getGoldAvailable() >= cakecost then
		return true
	end
	return false
end

EventCallDispatcher.registerLuaExeExecute(MoralExe.id, MoralExe.execute)
