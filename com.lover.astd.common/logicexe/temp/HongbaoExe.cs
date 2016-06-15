using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.logicexe.temp
{
    public class HongbaoExe : TempExeBase, ITempExe, IExecute
    {
        private bool _finished;

        public HongbaoExe()
        {
            _name = ConfigStrings.S_Temp;
            _readable = ConfigStrings.SR_Temp;
        }
        public override long execute()
        {
            _factory.getActivityManager().getPayHongbaoEventInfo(_proto, _logger);
            _finished = true;
            return immediate();
        }

        public bool isFinished()
        {
            return _finished;
        }

        public void setTarget(Dictionary<string, string> conf)
        {
            
        }

        public string getStatus()
        {
            return "进行中...";
        }
    }
}
