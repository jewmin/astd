using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.logicexe.temp
{
    public class OpenAllTrainerExe : TempExeBase, ITempExe, IExecute
    {
        /// <summary>
        /// 要开启的次数
        /// </summary>
        private int _train_times;
        /// <summary>
        /// 已开启的次数
        /// </summary>
        private int _open_train_times;

        private bool _finished;

        public OpenAllTrainerExe()
        {
            _name = ConfigStrings.S_Temp;
            _readable = ConfigStrings.SR_Temp;
        }

        public override long execute()
        {
            if (this._open_train_times == this._train_times)
            {
                this.logInfo("已经开启完毕, 将关闭临时任务");
                this._finished = true;
                return base.an_hour_later();
            }
            if (!_factory.getHeroManager().openAllTrainer(_proto, _logger))
            {
                this.logInfo("开启失败, 将关闭临时任务");
                this._finished = true;
                return base.an_hour_later();
            }
            this._open_train_times++;
            return immediate();
        }

        public bool isFinished()
        {
            return _finished;
        }

        public void setTarget(Dictionary<string, string> conf)
        {
            if (conf == null || !conf.ContainsKey("open_trainer_times")) return;
            int.TryParse(conf["open_trainer_times"], out this._train_times);
            this._open_train_times = 0;
        }

        public string getStatus()
        {
            return "进行中...";
        }
    }
}
