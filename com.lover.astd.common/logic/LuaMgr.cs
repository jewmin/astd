using System;
using System.Collections.Generic;
using System.Text;
using LuaInterface;
using System.IO;
using com.lover.astd.common.config;
using com.lover.astd.common.model;
using System.Drawing;

namespace com.lover.astd.common.logic
{
    public class LuaMgr
    {
        private readonly string lua_file_ = "\\lua\\astd.lua";
        private Lua lua_;
        private ProtocolMgr proto_;
        private ServiceFactory factory_;
        private ILogger logger_;
        private GameConfig conf_;
        private OtherConfig other_conf_;
        private User user_;

        public LuaMgr(ProtocolMgr proto, ILogger logger, ServiceFactory factory, GameConfig conf, OtherConfig other_conf, User user)
        {
            proto_ = proto;
            logger_ = logger;
            factory_ = factory;
            conf_ = conf;
            other_conf_ = other_conf;
            user_ = user;
            CreateVM();
        }

        public void CreateVM()
        {
            lua_ = new Lua();
            lua_.RegisterFunction("User", this, this.GetType().GetMethod("GetUser"));
            lua_.RegisterFunction("ILogger", this, this.GetType().GetMethod("GetLogger"));
            lua_.RegisterFunction("GameConfig", conf_, conf_.GetType().GetMethod("getConfig"));
            lua_.RegisterFunction("OtherConfig", other_conf_, other_conf_.GetType().GetMethod("getConfig"));
            lua_.RegisterFunction("ProtocolMgr", this, this.GetType().GetMethod("GetProtocolMgr"));
            lua_.RegisterFunction("MiscManager", factory_, factory_.GetType().GetMethod("getMiscManager"));
            lua_.RegisterFunction("BattleManager", factory_, factory_.GetType().GetMethod("getBattleManager"));
            lua_.RegisterFunction("ActivityManager", factory_, factory_.GetType().GetMethod("getActivityManager"));
            lua_.RegisterFunction("BuildingManager", factory_, factory_.GetType().GetMethod("getBuildingManager"));
            lua_.RegisterFunction("TroopManager", factory_, factory_.GetType().GetMethod("getTroopManager"));
            lua_.RegisterFunction("EquipManager", factory_, factory_.GetType().GetMethod("getEquipManager"));
            lua_.RegisterFunction("CampaignManager", factory_, factory_.GetType().GetMethod("getCampaignManager"));
            lua_.RegisterFunction("BigHeroManager", factory_, factory_.GetType().GetMethod("getBigHeroManager"));
            lua_.RegisterFunction("HeroManager", factory_, factory_.GetType().GetMethod("getHeroManager"));
            lua_.RegisterFunction("L", this, this.GetType().GetMethod("UTF8toUnicode"));
            lua_.DoFile(Directory.GetCurrentDirectory() + lua_file_);
            logger_.log("加载lua虚拟机", Color.Violet);
        }

        public User GetUser()
        {
            return user_;
        }

        public ProtocolMgr GetProtocolMgr()
        {
            return proto_;
        }

        public ILogger GetLogger()
        {
            return logger_;
        }

        public object[] CallFunction(string func, params object[] args)
        {
            if (lua_ == null)
            {
                logger_.logError(string.Format("call lua function ({0}) error: Lua is null", func));
                return null;
            }

            LuaFunction lf = lua_.GetFunction(func);
            if (lf == null)
            {
                logger_.logError(string.Format("call lua function ({0}) error: LuaFunction is null", func));
                return null;
            }

            try
            {
                return lf.Call(args);
            }
            catch (Exception ex)
            {
                logger_.logError(string.Format("call lua function ({0}) error: {1}", func, ex.Message));
                return null;
            }
        }

        public string UTF8toUnicode(string utf8_string)
        {
            byte[] utf8_bytes = Encoding.Default.GetBytes(utf8_string);
            byte[] unicode_bytes = Encoding.Convert(Encoding.UTF8, Encoding.Default, utf8_bytes, 0, utf8_bytes.Length);
            string unicode_string = Encoding.Default.GetString(unicode_bytes, 0, unicode_bytes.Length);
            return unicode_string;
        }
    }
}
