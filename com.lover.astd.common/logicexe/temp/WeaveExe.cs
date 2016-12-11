using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common;

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
            int like = DbHelper.GetIntVariable(_user._db_userid, ConfigStrings.weave_item);
            do
            {
                result = _factory.getMiscManager().handleWeaveInfo(_proto, _logger, _user, 130, 20, out weave_state, ref like, true, false, false);
            }
            while (num > 0 && result != 2);
            DbHelper.SetVariable(_user._db_userid, ConfigStrings.weave_item, like.ToString());
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
