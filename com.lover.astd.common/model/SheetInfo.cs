﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    /// <summary>
    /// 专属
    /// </summary>
    public class SheetInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public int id;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 品质 白1,蓝2,绿3,黄4,红5,紫6
        /// </summary>
        public int quality;
        /// <summary>
        /// 类型 剑1,甲2,马3,披4,书5,符6
        /// </summary>
        public int type;
        /// <summary>
        /// 等级
        /// </summary>
        public int lv;
        /// <summary>
        /// 材料
        /// </summary>
        public int material1num;
        /// <summary>
        /// 镔铁
        /// </summary>
        public int material2num;
        /// <summary>
        /// 增加经验
        /// </summary>
        public int addexp;
        /// <summary>
        /// 增加属性
        /// </summary>
        public string effect;
        /// <summary>
        /// 成功率
        /// </summary>
        public double succprob;
        /// <summary>
        /// 拥有数量
        /// </summary>
        public int equipnum;
        /// <summary>
        /// 材料名称
        /// </summary>
        public string goodsname;
        /// <summary>
        /// 材料数量
        /// </summary>
        public int goodsnum;
        /// <summary>
        /// 材料类型 item道具 special专属
        /// </summary>
        public string src;
        /// <summary>
        /// 专属材料id
        /// </summary>
        public int preid;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SheetInfo()
        {
            id = 0;
        }
        /// <summary>
        /// 处理xml
        /// </summary>
        /// <param name="node"></param>
        public void handle(XmlNode node)
        {
            if (node == null) return;
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(node);
            id = lua.GetIntValue("sheetinfo.id");
            name = lua.GetStringValue("sheetinfo.name");
            quality = lua.GetIntValue("sheetinfo.quality");
            type = lua.GetIntValue("sheetinfo.type");
            lv = lua.GetIntValue("sheetinfo.lv");
            material1num = lua.GetIntValue("sheetinfo.material1num");
            material2num = lua.GetIntValue("sheetinfo.material2num");
            addexp = lua.GetIntValue("sheetinfo.addexp");
            effect = lua.GetStringValue("sheetinfo.effect");
            succprob = lua.GetDoubleValue("sheetinfo.succprob");
            equipnum = lua.GetIntValue("sheetinfo.equipnum");
            goodsname = lua.GetStringValue("sheetinfo.goodsname");
            goodsnum = lua.GetIntValue("sheetinfo.goodsnum");
            src = lua.GetStringValue("sheetinfo.src");
            preid = lua.GetIntValue("sheetinfo.preid");
        }
    }

    /// <summary>
    /// 铸造
    /// </summary>
    public class SkillInfo
    {
        /// <summary>
        /// 镔铁
        /// </summary>
        public int material2num;
        /// <summary>
        /// 铸造次数
        /// </summary>
        public int makenum;
        /// <summary>
        /// 铸造cd
        /// </summary>
        public int makecd;
        /// <summary>
        /// 当前铸造专属
        /// </summary>
        public int sheetid;
        /// <summary>
        /// 结束cd
        /// </summary>
        public int endcd;
        /// <summary>
        /// 铸造状态 未完成0,完成1
        /// </summary>
        public int makestate;
        /// <summary>
        /// 可铸造品质
        /// </summary>
        public int canmakequality;
        /// <summary>
        /// 增加成功率
        /// </summary>
        public double addprob;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillInfo()
        {
            material2num = 0;
            makenum = 0;
            sheetid = 0;
        }
        /// <summary>
        /// 处理xml
        /// </summary>
        /// <param name="node"></param>
        public void handle(XmlNode node)
        {
            if (node == null) return;
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(node);
            material2num = lua.GetIntValue("skillinfo.material2num");
            makenum = lua.GetIntValue("skillinfo.makenum");
            makecd = lua.GetIntValue("skillinfo.makecd");
            sheetid = lua.GetIntValue("skillinfo.making.sheetid");
            endcd = lua.GetIntValue("skillinfo.making.endcd");
            makestate = lua.GetIntValue("skillinfo.making.makestate");
            canmakequality = lua.GetIntValue("skillinfo.canmakequality");
            addprob = lua.GetDoubleValue("skillinfo.addprob");
        }
        /// <summary>
        /// 处理xml2
        /// </summary>
        /// <param name="node"></param>
        public void handle2(XmlNode node)
        {
            if (node == null) return;
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(node);
            material2num = lua.GetIntValue("results.material2num");
            makenum = lua.GetIntValue("results.makenum");
            makecd = lua.GetIntValue("results.makecd");
            sheetid = lua.GetIntValue("results.making.sheetid");
            endcd = lua.GetIntValue("results.making.endcd");
            makestate = lua.GetIntValue("results.making.makestate");
        }
    }
}
