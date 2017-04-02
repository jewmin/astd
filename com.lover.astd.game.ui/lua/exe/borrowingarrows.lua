-- ²Ý´¬½è¼ý
BorrowingArrowsExe = {}

BorrowingArrowsExe.name = "borrowingarrows"
BorrowingArrowsExe.readable = "²Ý´¬½è¼ý"
BorrowingArrowsExe.id = LuaExeId.eBorrowingArrows
BorrowingArrowsExe.exe = LuaExeFactory.createLuaExe(BorrowingArrowsExe)

BorrowingArrowsExe.execute = function()
	return ActivityManager():borrowingArrowsExecute(ProtocolMgr(), ILogger(), User(),
		global.getGoldAvailable(), borrowingArrowsConfig.buyboatcostlimit, borrowingArrowsConfig.calculatestream,
		borrowingArrowsConfig.calculatestreamcostlimit, borrowingArrowsConfig.costlimit, borrowingArrowsConfig.percent)
end

EventCallDispatcher.registerLuaExeExecute(BorrowingArrowsExe.id, BorrowingArrowsExe.execute)
