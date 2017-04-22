using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace com.lover.astd.common.logicexe.temp
{
    public class RefineExe : TempExeBase, ITempExe, IExecute
    {
        private bool _finished;

        public RefineExe()
        {
            _name = ConfigStrings.S_Temp;
            _readable = ConfigStrings.SR_Temp;
        }

        public override long execute()
        {
            int result = 0;
            do
            {
                result = _factory.getMiscManager().handleRefineInfo(_factory.getEquipManager(), _proto, _logger, _user, 0.1, 0, 0, false, true);
                if (result == 4)
                {
                    Thread.Sleep(11000);
                    result = 0;
                }
            }
            while (result == 0);

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
            return "进行中......";
        }
    }
}
