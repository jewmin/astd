using com.lover.astd.common.manager;
using System;

namespace com.lover.astd.common.logic
{
	public class ServiceFactory
	{
		private TimeMgr _tmrMgr = new TimeMgr();

		private HeroMgr _heroMgr;

        private BigHeroMgr _bigHeroMgr;

		private CampaignMgr _campaignMgr;

		private EquipMgr _equipMgr;

		private TroopMgr _troopMgr;

		private BuildingMgr _buildMgr;

		private ActivityMgr _activityMgr;

		private BattleMgr _battleMgr;

		private MiscMgr _miscMgr;

		public TimeMgr TmrMgr
		{
			get
			{
				return this._tmrMgr;
			}
		}

		public HeroMgr getHeroManager()
		{
			bool flag = this._heroMgr == null;
			if (flag)
			{
				this._heroMgr = new HeroMgr(this._tmrMgr, this);
			}
			return this._heroMgr;
		}

        public BigHeroMgr getBigHeroManager()
        {
            if (_bigHeroMgr == null)
            {
                _bigHeroMgr = new BigHeroMgr(_tmrMgr, this);
            }
            return _bigHeroMgr;
        }

		public CampaignMgr getCampaignManager()
		{
			bool flag = this._campaignMgr == null;
			if (flag)
			{
				this._campaignMgr = new CampaignMgr(this._tmrMgr, this);
			}
			return this._campaignMgr;
		}

		public EquipMgr getEquipManager()
		{
			bool flag = this._equipMgr == null;
			if (flag)
			{
				this._equipMgr = new EquipMgr(this._tmrMgr, this);
			}
			return this._equipMgr;
		}

		public TroopMgr getTroopManager()
		{
			bool flag = this._troopMgr == null;
			if (flag)
			{
				this._troopMgr = new TroopMgr(this._tmrMgr, this);
			}
			return this._troopMgr;
		}

		public BuildingMgr getBuildingManager()
		{
			bool flag = this._buildMgr == null;
			if (flag)
			{
				this._buildMgr = new BuildingMgr(this._tmrMgr, this);
			}
			return this._buildMgr;
		}

		public ActivityMgr getActivityManager()
		{
			bool flag = this._activityMgr == null;
			if (flag)
			{
				this._activityMgr = new ActivityMgr(this._tmrMgr, this);
			}
			return this._activityMgr;
		}

		public BattleMgr getBattleManager()
		{
			bool flag = this._battleMgr == null;
			if (flag)
			{
				this._battleMgr = new BattleMgr(this._tmrMgr, this);
			}
			return this._battleMgr;
		}

		public MiscMgr getMiscManager()
		{
			bool flag = this._miscMgr == null;
			if (flag)
			{
				this._miscMgr = new MiscMgr(this._tmrMgr, this);
			}
			return this._miscMgr;
		}
	}
}
