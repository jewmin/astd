-- �ټ���
baijiayanConfig =
{
  -- ��ҳ�����
  gold_eat =
  {
    -- �Ƿ���
    open = true,
    -- ������>=hunger
    hunger = 12,
    -- ���ѽ��<=gold
    gold = 2,
  },

  -- ��Ҽ�Ȧ��
  add_round =
  {
    -- �Ƿ���
    open = true,

    -- ���ѽ��<=gold
    gold = 20,
  }
}

-- ������
trainingConfig =
{
  -- ��������, ���ѽ��<=buyroundcost
  buyroundcost = 0,

  -- ���ý���, ���ѽ��<=resetcost
  resetcost = 0,

  -- ��������
  uparmy =
  {
    -- ���ѽ��<=gold
    gold = 0,
    -- �������� 1:��ͨ 2:��Ӣ 3:����
    army = 3,
  }
}

-- ��������
moralConfig =
{
  -- ����һ��, ���ѽ��<=buyroundcost
  buyroundcost = 0,

  -- ����
  moral =
  {
    -- ���ѽ��<=cakecost
    cakecost = 0,
    -- �������� 1:�ºڷ��;ʿ������ 2:��ɫ����;ʿ������ 3:����Ǭ��;ʿ������
    moontype = 3,
  }
}
