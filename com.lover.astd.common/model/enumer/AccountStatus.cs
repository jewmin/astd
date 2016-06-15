using System;

namespace com.lover.astd.common.model.enumer
{
	public enum AccountStatus
	{
		STA_not_start,
		STA_initial,
		STA_running,
		STA_relogin,
		STA_stopped,
		STA_stopped_login_verify,
		STA_login_failed,
		STA_login_failed_max,
		STA_stopped_game_verify,
		STA_due_time = 100
	}
}
