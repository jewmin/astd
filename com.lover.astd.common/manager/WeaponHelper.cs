using com.lover.astd.common.model;
using System;

namespace com.lover.astd.common.manager
{
	public class WeaponHelper
	{
		private static int[] _redStandards = new int[]
		{
			300,
			200,
			500,
			300,
			180,
			500,
			100
		};

		private static double[] _redFactors = new double[]
		{
			1.0,
			1.5,
			2.0,
			2.5,
			3.0,
			4.0,
			6.0
		};

		private static double[] _purpleStandards = new double[]
		{
			2040.0,
			1360.0,
			3400.0,
			2040.0,
			1224.0,
			3400.0,
			680.0
		};

		private static double[] _purpleInc_20 = new double[]
		{
			102.0,
			68.0,
			170.0,
			102.0,
			61.2,
			170.0,
			34.0
		};

		private static double[] _purpleInc_30 = new double[]
		{
			126.0,
			84.0,
			210.0,
			126.0,
			75.6,
			210.0,
			42.0
		};

		private static double[] _purpleInc_40plus = new double[]
		{
			42.0,
			28.0,
			70.0,
			42.0,
			25.2,
			70.0,
			14.0
		};

        public static int getRedWeaponBaseAmount(Equipment equip)
        {
            if (equip.goodstype != GoodsType.Weapon)
            {
                return 0;
            }
            return WeaponHelper._redStandards[equip.Type - EquipmentType.Sword];
        }

        public static int getRedWeaponMaxAmount(Equipment equip)
        {
            if (equip.goodstype != GoodsType.Weapon)
            {
                return 0;
            }
            return (int)((double)WeaponHelper._redStandards[equip.Type - EquipmentType.Sword] * WeaponHelper._redFactors[6]);
        }

		public static int getPurpleWeaponBaseAmount(int type, int star)
		{
			bool flag = type < 0 || type > 7 || star < 20;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				double num = WeaponHelper._purpleStandards[type];
				int num2 = (star > 30) ? 10 : (star - 20);
				int num3 = (star > 40) ? 10 : (star - 30);
				bool flag2 = num2 < 0;
				if (flag2)
				{
					num2 = 0;
				}
				bool flag3 = num3 < 0;
				if (flag3)
				{
					num3 = 0;
				}
				double num4 = num + WeaponHelper._purpleInc_20[type] * (double)num2 + WeaponHelper._purpleInc_30[type] * (double)num3;
				bool flag4 = star > 40;
				if (flag4)
				{
					int i = 40;
					int num5 = 1;
					while (i < star)
					{
						int num6 = star - i;
						int num7 = (num6 > 10) ? 10 : num6;
						bool flag5 = i < 60;
						if (flag5)
						{
							num4 += (WeaponHelper._purpleInc_30[type] + WeaponHelper._purpleInc_40plus[type] * (double)num5) * (double)num7;
						}
						else
						{
							num4 += (WeaponHelper._purpleInc_30[type] + WeaponHelper._purpleInc_40plus[type] * 3.0) * (double)num7;
						}
						i += 10;
						int num8 = num5;
						num5 = num8 + 1;
					}
				}
				result = (int)num4;
			}
			return result;
		}

		public static int calcRedWeaponStar(Equipment equip)
		{
			bool flag = equip.goodstype != GoodsType.Weapon;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int num = equip.Type - EquipmentType.Sword;
				int num2 = WeaponHelper._redStandards[num];
				double num3 = WeaponHelper._redFactors[6];
				int valueNow = equip.ValueNow;
				int num4 = 0;
				int num5;
				for (int i = 0; i < 7; i = num5 + 1)
				{
					bool flag2 = (int)((double)num2 * WeaponHelper._redFactors[i]) > valueNow;
					if (flag2)
					{
						num4 = i - 1;
						break;
					}
					num5 = i;
				}
				result = num4;
			}
			return result;
		}

		public static double getPurpleWeaponFullToughness(int star)
		{
			double[] array = new double[]
			{
				37.6,
				38.7,
				39.8,
				40.9,
				42.0,
				43.1,
				44.25,
				45.4,
				46.55,
				47.7,
				48.85,
				50.0,
				51.2,
				52.4,
				53.6,
				54.8,
				56.1,
				57.4,
				58.7,
				60.0,
				61.3,
				62.7,
				64.1,
				65.5,
				66.9,
				68.3,
				69.9,
				71.5,
				73.1,
				74.7,
				76.3,
				78.1,
				79.9,
				81.7,
				83.5,
				85.3,
				87.4,
				89.5,
				91.6,
				93.8,
				95.9,
				98.1,
				100.3,
				102.5,
				104.7,
				107.0,
				109.3,
				111.6,
				113.9,
				116.2,
				118.5,
				120.8,
				123.1,
				125.4,
				127.7,
				130.0,
				132.3,
				134.6,
				136.9,
				139.1,
				141.4,
				143.7,
				146.0,
				148.3,
				150.7,
				153.8,
				156.9,
				160.0,
				163.1,
				166.8,
				170.5,
				174.9,
				179.3,
				183.7,
				188.1,
				192.5
			};
			bool flag = star < 45;
			double num;
			if (flag)
			{
				num = array[0] - (double)(45 - star) * 0.6;
			}
			else
			{
				bool flag2 = star >= 45 && star <= 120;
				if (flag2)
				{
					num = array[star - 45];
				}
				else
				{
					num = array[array.Length - 1] + (double)(star - 120) * 4.4;
				}
			}
			return num * 0.01;
		}

		public static void getWeaponInfo(Equipment equip, int star_expect, out int amount_expect, out double toughness, out double full_toughness, out int failcount, out int needcount)
		{
			toughness = 0.0;
			full_toughness = 0.0;
			failcount = 0;
			needcount = 0;
			amount_expect = 0;
			bool flag = equip.goodstype != GoodsType.Weapon;
			if (!flag)
			{
				int num = equip.Type - EquipmentType.Sword;
				bool flag2 = equip.Quality == EquipmentQuality.Red;
				if (flag2)
				{
					int num2 = WeaponHelper._redStandards[num];
					double num3 = WeaponHelper._redFactors[6];
					int valueNow = equip.ValueNow;
					int num4 = WeaponHelper.calcRedWeaponStar(equip);
					int num5 = (int)((double)num2 * WeaponHelper._redFactors[num4]);
					toughness = (double)(valueNow - num5) * 1.0 / (double)num5;
					amount_expect = (int)((double)num2 * WeaponHelper._redFactors[num4 + 1] * (1.0 + toughness));
					failcount = (int)(toughness * 255.0 / 0.24);
					needcount = (int)Math.Pow(2.0, (double)(num4 + 3)) - 1 - failcount;
				}
				else
				{
					bool flag3 = equip.Quality == EquipmentQuality.Purple;
					if (flag3)
					{
						int valueNow2 = equip.ValueNow;
						int level = equip.Level;
						int purpleWeaponBaseAmount = WeaponHelper.getPurpleWeaponBaseAmount(num, level);
						toughness = (double)(valueNow2 - purpleWeaponBaseAmount) * 1.0 / (double)purpleWeaponBaseAmount;
						full_toughness = WeaponHelper.getPurpleWeaponFullToughness(level);
						int purpleWeaponBaseAmount2 = WeaponHelper.getPurpleWeaponBaseAmount(num, star_expect);
						amount_expect = (int)((double)purpleWeaponBaseAmount2 * (1.0 + toughness));
						failcount = (int)(toughness * 100.0 / 0.02);
					}
				}
			}
		}
	}
}
