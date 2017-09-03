using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.model;
using System.Threading;

namespace com.lover.astd.common.logicexe.temp
{
    public class TechnologyExe : TempExeBase, ITempExe, IExecute
    {
        private int left_bintie_;
        private int cost_bintie_;
        private int get_bintie_;
        private bool finish_;

        public TechnologyExe()
        {
            this._name = ConfigStrings.S_Temp;
            this._readable = ConfigStrings.SR_Temp;
            this.left_bintie_ = 100000;
            this.cost_bintie_ = 0;
            this.get_bintie_ = 0;
            this.finish_ = false;
        }

        public override long execute()
        {
            while (this.get_bintie_ > 0)
            {
                _factory.getBattleManager().getJailInfo(_proto, _logger, _user, false, getGoldAvailable(), 1);
                Thread.Sleep(60000);
                this.get_bintie_--;
            }

            _factory.getEquipManager().getSpecialEquipInfo(_proto, _logger, _user);
            int bintie = _user._specialEquipSkillInfo.material2num - this.left_bintie_;
            if (bintie <= 0)
            {
                this.finish_ = true;
                return immediate();
            }

            List<Technology> list = _factory.getEquipManager().getNewTech(_proto, _logger);
            foreach (Technology item in list)
            {
                if (item.consumerestype_ == "bintie")
                {
                    while (bintie > 0 && item.progress_ < item.requireprogress_ && item.consumenum_ <= this.cost_bintie_)
                    {
                        _factory.getEquipManager().researchNewTech(_proto, _logger, item);
                        bintie -= item.consumenum_;
                    }
                }
            }

            this.finish_ = true;
            return immediate();
        }

        public bool isFinished()
        {
            return this.finish_;
        }

        public void setTarget(Dictionary<string, string> conf)
        {
            if (conf == null || !conf.ContainsKey("left_bintie") || !conf.ContainsKey("cost_bintie") || !conf.ContainsKey("get_bintie")) return;
            int.TryParse(conf["left_bintie"], out this.left_bintie_);
            int.TryParse(conf["cost_bintie"], out this.cost_bintie_);
            int.TryParse(conf["get_bintie"], out this.get_bintie_);
        }

        public string getStatus()
        {
            return "进行中...";
        }
    }
}
