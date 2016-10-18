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