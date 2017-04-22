using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.model;

namespace com.lover.astd.common.logicexe.temp
{
    public class TechnologyExe : TempExeBase, ITempExe, IExecute
    {
        private int left_bintie_;
        private int cost_bintie_;
        private bool finish_;

        public TechnologyExe()
        {
            this._name = ConfigStrings.S_Temp;
            this._readable = ConfigStrings.SR_Temp;
            this.left_bintie_ = 100000;
            this.cost_bintie_ = 0;
            this.finish_ = false;
        }

        public override long execute()
        {
            int bintie = _user._specialEquipSkillInfo.material2num - this.left_bintie_;
            if (bintie <= 0)
            {
                this.finish_ = true;
                return immediate();
            }

            List<Technology> list = _factory.getEquipManager().getNewTech(_proto, _logger);
            foreach (Technology item in list)
            {
                while (bintie > 0 && item.progress_ < item.requireprogress_ && item.consumebintie_ <= this.cost_bintie_)
                {
                    _factory.getEquipManager().researchNewTech(_proto, _logger, item);
                    bintie -= item.consumebintie_;
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
            if (conf == null || !conf.ContainsKey("left_bintie") || !conf.ContainsKey("cost_bintie")) return;
            int.TryParse(conf["left_bintie"], out this.left_bintie_);
            int.TryParse(conf["cost_bintie"], out this.cost_bintie_);
        }

        public string getStatus()
        {
            return "进行中...";
        }
    }
}
