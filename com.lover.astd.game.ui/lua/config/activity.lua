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
  },

  select_db =
  {
    "����",
    "�Ƶ�",
    "���",
    "���"
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

-- ѩ��ͨ��
snowTradingConfig =
{
  -- �����������
  buyroundcost = 4,
  -- �ӹ�ѩ��
  reinforce = false,
  -- �ӹ�ѩ������
  reinforcecost = 20,
  -- �������� 1:���� 2:��ȯ
  choose = 2,
}

borrowingArrowsConfig =
{
  -- ��������
  buyboatcostlimit = 5,
  -- �Ƿ�ʹ���������
  calculatestream = false,
  -- ����������
  calculatestreamcostlimit = 20,
  -- ��������
  costlimit = 150000,
  -- ���ذٷֱ�
  percent = 0.8,
}

arrestEventConfig =
{
  -- ץ�������
  arresttokencostlimit = 5,
  -- ���շ���
  ricedumplingcostlimit = 5,
  -- ���ӷ���
  hishenlimit = 0,
}

ParadeEventConfig =
{
    -- �ı�����
    costlimit = 0,
    -- �ִι������
    roundcostlimit = 2,
}
