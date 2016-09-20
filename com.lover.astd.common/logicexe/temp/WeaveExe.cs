using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.logicexe.temp
{
    public class WeaveExe : TempExeBase, ITempExe, IExecute
    {
        private bool _finished;

        public WeaveExe()
        {
            _name = ConfigStrings.S_Temp;
            _readable = ConfigStrings.SR_Temp;
        }
        public override long execute()
        {
            int weave_state = 0;
            int num = 10;
            int result = 0;
            do
            {
                result = _factory.getMiscManager().handleWeaveInfo(_proto, _logger, _user, 130, 10, out weave_state, false, false, false);
            }
            while (num > 0 && result != 2);
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
