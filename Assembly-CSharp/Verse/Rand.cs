using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x0200015E RID: 350
	public static class Rand
	{
		// Token: 0x1700018D RID: 397
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0002FF44 File Offset: 0x0002E144
		public static int Seed
		{
			set
			{
				if (Rand.stateStack.Count == 0)
				{
					Log.ErrorOnce("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.", 825343540);
				}
				Rand.seed = (uint)value;
				Rand.iterations = 0U;
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002FF3C File Offset: 0x0002E13C
		public static RandBlock Block(int blockSeed)
		{
			return new RandBlock(blockSeed);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0002FF6D File Offset: 0x0002E16D
		public static float Value
		{
			get
			{
				return (float)(((double)MurmurHash.GetInt(Rand.seed, Rand.iterations++) - -2147483648.0) / 4294967295.0);
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0002FF9C File Offset: 0x0002E19C
		public static float ValueAsync(int seed)
		{
			return (float)(((double)MurmurHash.GetInt((uint)seed, 0U) - -2147483648.0) / 4294967295.0);
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0002FFBB File Offset: 0x0002E1BB
		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5f;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0002FFC9 File Offset: 0x0002E1C9
		public static int Sign
		{
			get
			{
				if (!Rand.Bool)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0002FFD5 File Offset: 0x0002E1D5
		public static int Int
		{
			get
			{
				return MurmurHash.GetInt(Rand.seed, Rand.iterations++);
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0002FFF0 File Offset: 0x0002E1F0
		public static Vector3 UnitVector3
		{
			get
			{
				return new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00030038 File Offset: 0x0002E238
		public static Vector2 UnitVector2
		{
			get
			{
				return new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00030070 File Offset: 0x0002E270
		public static Vector2 InsideUnitCircle
		{
			get
			{
				Vector2 result;
				do
				{
					result = new Vector2(Rand.Value - 0.5f, Rand.Value - 0.5f) * 2f;
				}
				while (result.sqrMagnitude > 1f);
				return result;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x000300B4 File Offset: 0x0002E2B4
		public static Vector3 InsideUnitCircleVec3
		{
			get
			{
				Vector2 insideUnitCircle = Rand.InsideUnitCircle;
				return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x000300DD File Offset: 0x0002E2DD
		// (set) Token: 0x06000A4E RID: 2638 RVA: 0x000300EF File Offset: 0x0002E2EF
		private static ulong StateCompressed
		{
			get
			{
				return (ulong)Rand.seed | (ulong)Rand.iterations << 32;
			}
			set
			{
				Rand.seed = (uint)(value & (ulong)-1);
				Rand.iterations = (uint)(value >> 32 & (ulong)-1);
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00030108 File Offset: 0x0002E308
		static Rand()
		{
			Rand.seed = (uint)DateTime.Now.GetHashCode();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0003013B File Offset: 0x0002E33B
		public static void EnsureStateStackEmpty()
		{
			if (Rand.stateStack.Count <= 0)
			{
				return;
			}
			Log.Warning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.");
			while (Rand.stateStack.Any<ulong>())
			{
				Rand.PopState();
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00030168 File Offset: 0x0002E368
		public static Vector2 InsideAnnulus(float innerRadius, float outerRadius)
		{
			float f = 6.2831855f * Rand.Value;
			Vector2 a = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			innerRadius *= innerRadius;
			outerRadius *= outerRadius;
			return Mathf.Sqrt(Mathf.Lerp(innerRadius, outerRadius, Rand.Value)) * a;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000301B8 File Offset: 0x0002E3B8
		public static Vector3 InsideAnnulusVector3(float innerRadius, float outerRadius)
		{
			Vector2 vector = Rand.InsideAnnulus(innerRadius, outerRadius);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000301E4 File Offset: 0x0002E3E4
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			return Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2) * widthFactor + centerX;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00030220 File Offset: 0x0002E420
		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2);
			if (num <= 0f)
			{
				return num * lowerWidthFactor + centerX;
			}
			return num * upperWidthFactor + centerX;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0003026C File Offset: 0x0002E46C
		public static T EnumValue<T>()
		{
			Array values = Enum.GetValues(typeof(T));
			return (T)((object)values.GetValue(Rand.Range(0, values.Length)));
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000302A0 File Offset: 0x0002E4A0
		public static int Range(int minInclusive, int maxExclusive)
		{
			if (maxExclusive <= minInclusive)
			{
				return minInclusive;
			}
			return minInclusive + Mathf.Abs(Rand.Int % (maxExclusive - minInclusive));
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000302B8 File Offset: 0x0002E4B8
		public static int RangeInclusive(int minInclusive, int maxInclusive)
		{
			if (maxInclusive <= minInclusive)
			{
				return minInclusive;
			}
			return Rand.Range(minInclusive, maxInclusive + 1);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x000302C9 File Offset: 0x0002E4C9
		public static float Range(float minInclusive, float maxInclusive)
		{
			if (maxInclusive <= minInclusive)
			{
				return minInclusive;
			}
			return Rand.Value * (maxInclusive - minInclusive) + minInclusive;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000302DC File Offset: 0x0002E4DC
		public static bool Chance(float chance)
		{
			return chance > 0f && (chance >= 1f || Rand.Value < chance);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000302FA File Offset: 0x0002E4FA
		public static bool DynamicChance(int amountPlaced, int amountToPlace, int positionsRemaining)
		{
			return amountPlaced < amountToPlace && positionsRemaining > 0 && Rand.Chance((float)(amountToPlace - amountPlaced) / (float)positionsRemaining);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00030312 File Offset: 0x0002E512
		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			Rand.PushState(specialSeed);
			bool result = Rand.Chance(chance);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00030325 File Offset: 0x0002E525
		public static float ValueSeeded(int specialSeed)
		{
			Rand.PushState(specialSeed);
			float value = Rand.Value;
			Rand.PopState();
			return value;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00030337 File Offset: 0x0002E537
		public static float RangeSeeded(float minInclusive, float maxInclusive, int specialSeed)
		{
			Rand.PushState(specialSeed);
			float result = Rand.Range(minInclusive, maxInclusive);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0003034B File Offset: 0x0002E54B
		public static int RangeSeeded(int minInclusive, int maxExclusive, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.Range(minInclusive, maxExclusive);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0003035F File Offset: 0x0002E55F
		public static int RangeInclusiveSeeded(int minInclusive, int maxInclusive, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.RangeInclusive(minInclusive, maxInclusive);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00030374 File Offset: 0x0002E574
		public static T Element<T>(params T[] list)
		{
			float value = Rand.Value;
			return list[Rand.Range(0, Mathf.RoundToInt(value * (float)list.Length))];
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0003039E File Offset: 0x0002E59E
		public static T Element<T>(T a, T b)
		{
			if (!Rand.Bool)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000303AC File Offset: 0x0002E5AC
		public static T Element<T>(T a, T b, T c)
		{
			float value = Rand.Value;
			if (value < 0.33333f)
			{
				return a;
			}
			if (value < 0.66666f)
			{
				return b;
			}
			return c;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000303D4 File Offset: 0x0002E5D4
		public static T Element<T>(T a, T b, T c, T d)
		{
			float value = Rand.Value;
			if (value < 0.25f)
			{
				return a;
			}
			if (value < 0.5f)
			{
				return b;
			}
			if (value < 0.75f)
			{
				return c;
			}
			return d;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00030408 File Offset: 0x0002E608
		public static T Element<T>(T a, T b, T c, T d, T e)
		{
			float value = Rand.Value;
			if (value < 0.2f)
			{
				return a;
			}
			if (value < 0.4f)
			{
				return b;
			}
			if (value < 0.6f)
			{
				return c;
			}
			if (value < 0.8f)
			{
				return d;
			}
			return e;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00030448 File Offset: 0x0002E648
		public static T Element<T>(T a, T b, T c, T d, T e, T f)
		{
			float value = Rand.Value;
			if (value < 0.16666f)
			{
				return a;
			}
			if (value < 0.33333f)
			{
				return b;
			}
			if (value < 0.5f)
			{
				return c;
			}
			if (value < 0.66666f)
			{
				return d;
			}
			if (value < 0.83333f)
			{
				return e;
			}
			return f;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00030490 File Offset: 0x0002E690
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB)
		{
			float num = weightA + weightB;
			if (Rand.Value < weightA / num)
			{
				return a;
			}
			return b;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000304B0 File Offset: 0x0002E6B0
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC)
		{
			float num = weightA + weightB + weightC;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			return c;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000304E0 File Offset: 0x0002E6E0
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD)
		{
			float num = weightA + weightB + weightC + weightD;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			return d;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00030520 File Offset: 0x0002E720
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE)
		{
			float num = weightA + weightB + weightC + weightD + weightE;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			return e;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00030574 File Offset: 0x0002E774
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE, T f, float weightF)
		{
			float num = weightA + weightB + weightC + weightD + weightE + weightF;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			if (value < (weightA + weightB + weightC + weightD + weightE) / num)
			{
				return e;
			}
			return f;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000305DE File Offset: 0x0002E7DE
		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000305EF File Offset: 0x0002E7EF
		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x000305FC File Offset: 0x0002E7FC
		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00030610 File Offset: 0x0002E810
		public static float ByCurve(SimpleCurve curve)
		{
			if (curve.PointsCount < 3)
			{
				throw new ArgumentException("curve has < 3 points");
			}
			if (curve[0].y != 0f || curve[curve.PointsCount - 1].y != 0f)
			{
				throw new ArgumentException("curve has start/end point with y != 0");
			}
			float num = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				if (curve[i].y < 0f)
				{
					throw new ArgumentException("curve has point with y < 0");
				}
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
			}
			float num2 = Rand.Range(0f, num);
			for (int j = 0; j < curve.PointsCount - 1; j++)
			{
				float num3 = (curve[j + 1].x - curve[j].x) * (curve[j].y + curve[j + 1].y);
				if (num3 >= num2)
				{
					float num4 = curve[j + 1].x - curve[j].x;
					float y = curve[j].y;
					float y2 = curve[j + 1].y;
					float num5 = num2 / (y + y2);
					if (Rand.Range(0f, (y + y2) / 2f) > Mathf.Lerp(y, y2, num5 / num4))
					{
						num5 = num4 - num5;
					}
					return num5 + curve[j].x;
				}
				num2 -= num3;
			}
			throw new Exception("Reached end of Rand.ByCurve without choosing a point.");
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00030810 File Offset: 0x0002EA10
		public static float ByCurveAverage(SimpleCurve curve)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
				num2 += (curve[i + 1].x - curve[i].x) * (curve[i].x * (2f * curve[i].y + curve[i + 1].y) + curve[i + 1].x * (curve[i].y + 2f * curve[i + 1].y));
			}
			return num2 / num / 3f;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00030928 File Offset: 0x0002EB28
		public static bool MTBEventOccurs(float mtb, float mtbUnit, float ticksSinceLastCheck)
		{
			if (float.IsPositiveInfinity(mtb))
			{
				return false;
			}
			if (mtb == 0f)
			{
				return true;
			}
			if (mtb < 0f)
			{
				Log.Error("MTBEventOccurs with nevative mtb=" + mtb.ToString());
				return true;
			}
			if (mtbUnit <= 0f)
			{
				Log.Error("MTBEventOccurs with mtbUnit=" + mtbUnit.ToString());
				return false;
			}
			if (ticksSinceLastCheck <= 0f)
			{
				Log.Error("MTBEventOccurs with checkDuration=" + ticksSinceLastCheck.ToString());
				return false;
			}
			double num = (double)ticksSinceLastCheck / ((double)mtb * (double)mtbUnit);
			if (num <= 0.0)
			{
				Log.Error(string.Concat(new string[]
				{
					"chancePerCheck is ",
					num.ToString(),
					". mtb=",
					mtb.ToString(),
					", mtbUnit=",
					mtbUnit.ToString(),
					", checkDuration=",
					ticksSinceLastCheck.ToString()
				}));
				return false;
			}
			double num2 = 1.0;
			if (num < 0.0001)
			{
				while (num < 0.0001)
				{
					num *= 8.0;
					num2 /= 8.0;
				}
				if ((double)Rand.Value > num2)
				{
					return false;
				}
			}
			return (double)Rand.Value < num;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00030A68 File Offset: 0x0002EC68
		public static float CombineMTBs(List<float> mtbs)
		{
			if (mtbs.Count == 0)
			{
				return 0f;
			}
			if (mtbs.Count == 1)
			{
				return mtbs[0];
			}
			foreach (float num in mtbs)
			{
				if (num < 0f || float.IsNaN(num))
				{
					Log.Error(string.Format("Unexpected MTB value found while combining MTBs: {0}", num));
					return 0f;
				}
			}
			return (float)(1.0 / mtbs.Aggregate(0.0, (double acc, float mtb) => acc + 1.0 / (double)mtb));
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00030B38 File Offset: 0x0002ED38
		public static float CombineMTBs(float mtb1, float mtb2)
		{
			return (float)(1.0 / (1.0 / (double)mtb1 + 1.0 / (double)mtb2));
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00030B60 File Offset: 0x0002ED60
		public static void SplitRandomly(float valueToSplit, int splitIntoCount, List<float> outValues, List<float> zeroIfFractionBelow = null, List<float> minFractions = null)
		{
			outValues.Clear();
			if (splitIntoCount > 0)
			{
				if (minFractions != null)
				{
					float num = 0f;
					for (int i = 0; i < minFractions.Count; i++)
					{
						if (minFractions[i] > 1f)
						{
							throw new ArgumentException("minFractions[i] > 1");
						}
						num += minFractions[i];
					}
					if (num > 1f)
					{
						throw new ArgumentException("minFractions sum > 1");
					}
				}
				float num2 = 0f;
				int num3 = 0;
				for (;;)
				{
					num3++;
					if (num3 > 10000)
					{
						break;
					}
					outValues.Clear();
					for (int j = 0; j < splitIntoCount; j++)
					{
						float num4 = Rand.Range(0f, 1f);
						num2 += num4;
						outValues.Add(num4);
					}
					bool flag = true;
					if (minFractions != null)
					{
						for (int k = 0; k < minFractions.Count; k++)
						{
							if (outValues[k] / num2 < minFractions[k])
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						goto IL_151;
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Could not meet SplitRandomly requirements. valueToSplit=",
					valueToSplit.ToString(),
					", splitIntoCount=",
					splitIntoCount.ToString(),
					", zeroIfFractionsBelow=",
					zeroIfFractionBelow.ToStringSafeEnumerable(),
					", minFractions=",
					minFractions.ToStringSafeEnumerable()
				}));
				IL_151:
				if (zeroIfFractionBelow != null)
				{
					for (int l = 0; l < zeroIfFractionBelow.Count; l++)
					{
						if (outValues[l] / num2 < zeroIfFractionBelow[l])
						{
							outValues[l] = 0f;
							num2 = 0f;
							for (int m = 0; m < outValues.Count; m++)
							{
								num2 += outValues[m];
							}
						}
					}
				}
				if (num2 != 0f)
				{
					for (int n = 0; n < outValues.Count; n++)
					{
						outValues[n] = outValues[n] / num2 * valueToSplit;
					}
				}
				return;
			}
			if (valueToSplit == 0f)
			{
				return;
			}
			throw new ArgumentException("splitIntoCount <= 0");
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00030D50 File Offset: 0x0002EF50
		[DebugOutput("System", false)]
		internal static void RandTests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Random generation tests");
			stringBuilder.Append("To see texture outputs, turn on 'draw recorded noise' and run this again.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Performance test with " + 10000000.ToString() + " values");
			Stopwatch stopwatch = new Stopwatch();
			float num = 0f;
			Rand.PushState();
			stopwatch.Start();
			for (int i = 0; i < 10000000; i++)
			{
				num += Rand.Value;
			}
			stopwatch.Stop();
			Rand.PopState();
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Time: ",
				stopwatch.ElapsedMilliseconds.ToString(),
				"ms (for sum ",
				num.ToString(),
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Distribution test with " + 100000.ToString() + " values");
			DebugHistogram debugHistogram = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			DebugHistogram debugHistogram2 = new DebugHistogram(new float[]
			{
				0f,
				0.01f,
				0.02f,
				0.03f,
				0.04f,
				0.05f,
				0.06f,
				0.07f,
				0.08f,
				0.09f,
				0.1f,
				1f
			});
			Rand.PushState();
			for (int j = 0; j < 100000; j++)
			{
				debugHistogram.Add(Rand.Value);
				debugHistogram2.Add(Rand.Value);
			}
			Rand.PopState();
			stringBuilder.AppendLine("Gross histogram:");
			debugHistogram.Display(stringBuilder);
			stringBuilder.AppendLine("Fine histogram:");
			debugHistogram2.Display(stringBuilder);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Long-term tests");
			for (int k = 0; k < 3; k++)
			{
				int num2 = 0;
				for (int l = 0; l < 5000000; l++)
				{
					if (Rand.MTBEventOccurs(250f, 60000f, 60f))
					{
						num2++;
					}
				}
				string value = string.Concat(new string[]
				{
					"MTB=",
					250.ToString(),
					" days, MTBUnit=",
					60000.ToString(),
					", check duration=",
					60.ToString(),
					" Simulated ",
					5000.ToString(),
					" days (",
					5000000.ToString(),
					" tests). Got ",
					num2.ToString(),
					" events."
				});
				stringBuilder.AppendLine(value);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Short-term tests");
			for (int m = 0; m < 5; m++)
			{
				int num3 = 0;
				for (int n = 0; n < 10000; n++)
				{
					if (Rand.MTBEventOccurs(1f, 24000f, 12000f))
					{
						num3++;
					}
				}
				string value2 = string.Concat(new string[]
				{
					"MTB=",
					1f.ToString(),
					" days, MTBUnit=",
					24000f.ToString(),
					", check duration=",
					12000f.ToString(),
					", ",
					10000.ToString(),
					" tests got ",
					num3.ToString(),
					" events."
				});
				stringBuilder.AppendLine(value2);
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				int num5 = 0;
				for (int num6 = 0; num6 < 10000; num6++)
				{
					if (Rand.MTBEventOccurs(2f, 24000f, 6000f))
					{
						num5++;
					}
				}
				string value3 = string.Concat(new string[]
				{
					"MTB=",
					2f.ToString(),
					" days, MTBUnit=",
					24000f.ToString(),
					", check duration=",
					6000f.ToString(),
					", ",
					10000.ToString(),
					" tests got ",
					num5.ToString(),
					" events."
				});
				stringBuilder.AppendLine(value3);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Near seed tests");
			DebugHistogram debugHistogram3 = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			Rand.PushState();
			for (int num7 = 0; num7 < 1000; num7++)
			{
				Rand.Seed = num7;
				debugHistogram3.Add(Rand.Value);
			}
			Rand.PopState();
			debugHistogram3.Display(stringBuilder);
			int @int = Rand.Int;
			stringBuilder.AppendLine("Repeating single ValueSeeded with seed " + @int.ToString() + ". This should give the same result:");
			for (int num8 = 0; num8 < 4; num8++)
			{
				stringBuilder.AppendLine("   " + Rand.ValueSeeded(@int).ToString());
			}
			Log.Message(stringBuilder.ToString());
			if (DebugViewSettings.drawRecordedNoise)
			{
				int[] array = new int[65536];
				Rand.PushState();
				for (int num9 = 0; num9 < 65536; num9++)
				{
					array[num9] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One rand output per pixel", array, 1000f, 256);
				int[] array2 = new int[65536];
				Rand.PushState();
				for (int num10 = 0; num10 < 65536; num10++)
				{
					Rand.Seed = num10;
					array2[num10] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One seed per pixel", array2, 1000f, 256);
				int[] array3 = new int[65536];
				Rand.PushState();
				for (int num11 = 0; num11 < 300000; num11++)
				{
					int num12 = (int)(Rand.Value * 65536f);
					array3[num12]++;
				}
				Rand.PopState();
				float whiteValue = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel index", array3, whiteValue, 256);
				int[] array4 = new int[65536];
				Rand.PushState();
				for (int num13 = 0; num13 < 300000; num13++)
				{
					int num14 = (int)(Rand.Value * 256f);
					int num15 = (int)(Rand.Value * 256f);
					int num16 = num14 + 256 * num15;
					array4[num16]++;
				}
				Rand.PopState();
				float whiteValue2 = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel coords", array4, whiteValue2, 256);
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0003140C File Offset: 0x0002F60C
		private static void DebugStoreBucketsAsTexture(string name, int[] buckets, float whiteValue, int texSize)
		{
			Texture2D texture2D = new Texture2D(texSize, texSize);
			for (int i = 0; i < texSize; i++)
			{
				for (int j = 0; j < texSize; j++)
				{
					int num = i + j * texSize;
					float num2 = (float)buckets[num] / whiteValue;
					num2 = Mathf.Clamp01(num2);
					texture2D.SetPixel(i, j, new Color(num2, num2, num2));
				}
			}
			texture2D.Apply();
			NoiseDebugUI.StoreTexture(texture2D, name);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00031470 File Offset: 0x0002F670
		public static int RandSeedForHour(this Thing t, int salt)
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(t.HashOffset(), Find.TickManager.TicksAbs / 2500), salt);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00031494 File Offset: 0x0002F694
		public static bool TryRangeInclusiveWhere(int from, int to, Predicate<int> predicate, out int value)
		{
			int num = to - from + 1;
			if (num <= 0)
			{
				value = 0;
				return false;
			}
			int num2 = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 5);
			for (int i = 0; i < num2; i++)
			{
				int num3 = Rand.RangeInclusive(from, to);
				if (predicate(num3))
				{
					value = num3;
					return true;
				}
			}
			Rand.tmpRange.Clear();
			for (int j = from; j <= to; j++)
			{
				Rand.tmpRange.Add(j);
			}
			Rand.tmpRange.Shuffle<int>();
			int k = 0;
			int count = Rand.tmpRange.Count;
			while (k < count)
			{
				if (predicate(Rand.tmpRange[k]))
				{
					value = Rand.tmpRange[k];
					return true;
				}
				k++;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0003155C File Offset: 0x0002F75C
		public static Vector3 PointOnSphereCap(Vector3 center, float angle)
		{
			if (angle <= 0f)
			{
				return center;
			}
			if (angle >= 180f)
			{
				return Rand.UnitVector3;
			}
			float num = Rand.Range(Mathf.Cos(angle * 0.017453292f), 1f);
			float f = Rand.Range(0f, 6.2831855f);
			Vector3 point = new Vector3(Mathf.Sqrt(1f - num * num) * Mathf.Cos(f), Mathf.Sqrt(1f - num * num) * Mathf.Sin(f), num);
			return Quaternion.FromToRotation(Vector3.forward, center) * point;
		}

		// Token: 0x0400053B RID: 1339
		private static uint seed;

		// Token: 0x0400053C RID: 1340
		private static uint iterations;

		// Token: 0x0400053D RID: 1341
		private static readonly Stack<ulong> stateStack = new Stack<ulong>();

		// Token: 0x0400053E RID: 1342
		private static List<int> tmpRange = new List<int>();
	}
}
