-- 百家宴
baijiayanConfig =
{
  -- 金币吃粽子
  gold_eat =
  {
    -- 是否开启
    open = true,
    -- 饥饿度>=hunger
    hunger = 12,
    -- 花费金币<=gold
    gold = 2,
  },

  -- 金币加圈数
  add_round =
  {
    -- 是否开启
    open = true,

    -- 花费金币<=gold
    gold = 20,
  },

  select_db =
  {
    "夏桀",
    "成吉思汗",
    "张良",
    "李白"
  }
}

-- 大练兵
trainingConfig =
{
  -- 购买轮数, 花费金币<=buyroundcost
  buyroundcost = 0,

  -- 重置奖励, 花费金币<=resetcost
  resetcost = 0,

  -- 升级部队
  uparmy =
  {
    -- 花费金币<=gold
    gold = 0,
    -- 部队类型 1:普通 2:精英 3:首领
    army = 3,
  }
}

-- 赏月送礼
moralConfig =
{
  -- 再来一轮, 花费金币<=buyroundcost
  buyroundcost = 0,

  -- 送礼
  moral =
  {
    -- 花费金币<=cakecost
    cakecost = 0,
    -- 月亮类型 1:月黑风高;士气低落 2:月色朦胧;士气规整 3:月满乾坤;士气高涨
    moontype = 3,
  }
}

-- 雪地通商
snowTradingConfig =
{
  -- 购买次数费用
  buyroundcost = 4,
  -- 加固雪橇
  reinforce = false,
  -- 加固雪橇费用
  reinforcecost = 20,
  -- 奖励类型 1:镔铁 2:点券
  choose = 2,
}

-- 草船借箭
borrowingArrowsConfig =
{
  -- 发船费用
  buyboatcostlimit = 0,
  -- 是否使用神机妙算
  calculatestream = false,
  -- 神机妙算费用
  calculatestreamcostlimit = 0,
  -- 邀功费用
  costlimit = 150000,
  -- 承重百分比
  percent = 0.8,
}

-- 抓捕
arrestEventConfig =
{
  -- 抓捕令费用
  arresttokencostlimit = 5,
  -- 密粽费用
  ricedumplingcostlimit = 5,
  -- 鞭子费用
  hishenlimit = 0,
}

-- 阅兵庆典
ParadeEventConfig =
{
    -- 阅兵费用
    costlimit = 0,
    -- 轮次购买费用
    roundcostlimit = 0,
}

RingEventConfig =
{
  -- 随机敲钟费用
  random_ring_cost = 5,
  -- 福禄寿敲钟费用
  other_ring_cost = 5,
  -- 进度奖励
  progress_choose = 1,
}