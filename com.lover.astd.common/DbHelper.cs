using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Threading;

namespace com.lover.astd.common
{
    public class DbHelper
    {
        private static readonly object obj = new object();
        private static string dbname_ = "astd.db";
        private static string dirname_ = "";
        private static string path_ = "Data Source =" + Directory.GetCurrentDirectory() + "\\astd.db";

        public static string Path
        {
            get { return path_; }
            set { path_ = value; }
        }

        public static void SetVariable(int userid, string configname, string configvalue)
        {
            string value = GetVariable(userid, configname);
            if (!value.Equals(configvalue))
            {
                using (SQLiteConnection conn = new SQLiteConnection(path_))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = string.Format("INSERT INTO config (userid, configname, configvalue) VALUES ({0}, '{1}', '{2}');", userid, configname, configvalue);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static int GetIntVariable(int userid, string configname)
        {
            int nvalue = 0;
            string value = GetVariable(userid, configname);
            int.TryParse(value, out nvalue);
            return nvalue;
        }

        public static string GetVariable(int userid, string configname)
        {
            string value = "";
            using (SQLiteConnection conn = new SQLiteConnection(path_))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = string.Format("SELECT configvalue FROM config WHERE userid = {0} and configname = '{1}' LIMIT 1;", userid, configname);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            value = reader.GetString(0);
                        }
                    }
                }
            }
            return value;
        }

        public static void CreateTable(string partner, int serverid, string rolename)
        {
            string dirName = "accounts";
            dirname_ = string.Format("{0}/{1}_{2}_{3}", dirName, partner, serverid, rolename);
            path_ = string.Format("Data Source ={0}/{1}", dirname_, dbname_);

            using (SQLiteConnection conn = new SQLiteConnection(path_))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS users(userid integer PRIMARY KEY autoincrement, servertype integer, serverid integer, actorid integer, actorname varchar(32));");
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = string.Format("CREATE TABLE IF NOT EXISTS config(userid integer, configname varchar(32), configvalue varchar(255));");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int GetUserId(int servertype, int serverid, int actorid)
        {
            int userid = 0;
            using (SQLiteConnection conn = new SQLiteConnection(path_))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = string.Format("SELECT userid FROM users WHERE servertype = {0} and serverid = {1} and actorid = {2} LIMIT 1;", servertype, serverid, actorid);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            userid = reader.GetInt32(0);
                        }
                    }
                }
            }
            return userid;
        }

        public static void InsertUser(int servertype, int serverid, int actorid, string actorname)
        {
            int userid = GetUserId(servertype, serverid, actorid);
            if (userid == 0)
            {
                using (SQLiteConnection conn = new SQLiteConnection(path_))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = string.Format("INSERT INTO users (servertype, serverid, actorid, actorname) VALUES ({0}, {1}, {2}, '{3}');", servertype, serverid, actorid, actorname);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
