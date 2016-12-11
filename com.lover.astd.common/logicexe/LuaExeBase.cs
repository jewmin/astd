using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;

namespace com.lover.astd.common.logicexe
{
    public class LuaExeBase : ExeBase
    {
        protected int id_;
        protected LuaMgr lua_mgr_;

        public LuaExeBase(LuaMgr lua_mgr)
        {
            base._name = "LuaExeBase";
            base._readable = "LuaExe基类";
            this.id_ = 0;
            this.lua_mgr_ = lua_mgr;
        }

        public override long execute()
        {
            try
            {
                if (this.id_ == 0) throw new Exception("id cannot be zero");
                if (this.lua_mgr_ == null) throw new Exception("lua_mgr cannot be null");
                object[] retval = this.lua_mgr_.CallFunction("OnLuaExecute", this.id_);
                if (retval == null || retval[0] == null) throw new Exception("invalid return value");
                return Convert.ToInt64(retval[0]);
            }
            catch (Exception ex)
            {
                base.logInfo(string.Format("{0} execute error: {1}", this._readable, ex.Message));
                return base.next_halfhour();
            }
        }

        public void setLuaId(int id)
        {
            this.id_ = id;
        }
    }
}
