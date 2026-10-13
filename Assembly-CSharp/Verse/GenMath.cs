using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010F RID: 271
	public static class GenMath
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x00025B70 File Offset: 0x00023D70
		public static float RoundedHundredth(float f)
		{
			return Mathf.Round(f * 100f) / 100f;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00025B84 File Offset: 0x00023D84
		public static int RoundTo(int value, int roundToNearest)
		{
			return (int)Math.Round((double)((float)value / (float)roundToNearest)) * roundToNearest;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00025B94 File Offset: 0x00023D94
		public static float RoundTo(float value, float roundToNearest)
		{
			return (float)((int)Math.Round((double)(value / roundToNearest))) * roundToNearest;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00025BA4 File Offset: 0x00023DA4
		public static float ChanceEitherHappens(float chanceA, float chanceB)
		{
			return chanceA + (1f - chanceA) * chanceB;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00025BB1 File Offset: 0x00023DB1
		public static float SmootherStep(float edge0, float edge1, float x)
		{
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * x * (x * (x * 6f - 15f) + 10f);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00025BDC File Offset: 0x00023DDC
		public static int RoundRandom(float f)
		{
			return (int)f + ((Rand.Value < f % 1f) ? 1 : 0);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00025BF3 File Offset: 0x00023DF3
		public static float WeightedAverage(float A, float weightA, float B, float weightB)
		{
			return (A * weightA + B * weightB) / (weightA + weightB);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00025C00 File Offset: 0x00023E00
		public static float Median<T>(IList<T> list, Func<T, float> orderBy, float noneValue = 0f, float center = 0.5f)
		{
			if (list.NullOrEmpty<T>())
			{
				return noneValue;
			}
			GenMath.tmpElements.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				GenMath.tmpElements.Add(orderBy(list[i]));
			}
			GenMath.tmpElements.Sort();
			return GenMath.tmpElements[Mathf.Min(Mathf.FloorToInt((float)GenMath.tmpElements.Count * center), GenMath.tmpElements.Count - 1)];
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00025C80 File Offset: 0x00023E80
		public static float WeightedMedian(IList<Pair<float, float>> list, float noneValue = 0f, float center = 0.5f)
		{
			GenMath.tmpPairs.Clear();
			GenMath.tmpPairs.AddRange(list);
			float num = 0f;
			for (int i = 0; i < GenMath.tmpPairs.Count; i++)
			{
				float second = GenMath.tmpPairs[i].Second;
				if (second < 0f)
				{
					Log.ErrorOnce("Negative weight in WeightedMedian: " + second.ToString(), GenMath.tmpPairs.GetHashCode());
				}
				else
				{
					num += second;
				}
			}
			if (num <= 0f)
			{
				return noneValue;
			}
			GenMath.tmpPairs.SortBy((Pair<float, float> x) => x.First);
			float num2 = 0f;
			for (int j = 0; j < GenMath.tmpPairs.Count; j++)
			{
				float first = GenMath.tmpPairs[j].First;
				float second2 = GenMath.tmpPairs[j].Second;
				num2 += second2 / num;
				if (num2 >= center)
				{
					return first;
				}
			}
			return GenMath.tmpPairs.Last<Pair<float, float>>().First;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00025DA1 File Offset: 0x00023FA1
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00025DAC File Offset: 0x00023FAC
		public static float LerpDouble(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			float num = (x - inFrom) / (inTo - inFrom);
			return outFrom + (outTo - outFrom) * num;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00025DC9 File Offset: 0x00023FC9
		public static float LerpDoubleClamped(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			return GenMath.LerpDouble(inFrom, inTo, outFrom, outTo, Mathf.Clamp(x, Mathf.Min(inFrom, inTo), Mathf.Max(inFrom, inTo)));
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00025DE9 File Offset: 0x00023FE9
		public static float Reflection(float value, float mirror)
		{
			return mirror - (value - mirror);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00025DF0 File Offset: 0x00023FF0
		public static Quaternion ToQuat(this float ang)
		{
			return Quaternion.AngleAxis(ang, Vector3.up);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00025E00 File Offset: 0x00024000
		public static float GetFactorInInterval(float min, float mid, float max, float power, float x)
		{
			if (min > max)
			{
				return 0f;
			}
			if (x <= min || x >= max)
			{
				return 0f;
			}
			if (x == mid)
			{
				return 1f;
			}
			float f;
			if (x < mid)
			{
				f = 1f - (mid - x) / (mid - min);
			}
			else
			{
				f = 1f - (x - mid) / (max - mid);
			}
			return Mathf.Pow(f, power);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00025E64 File Offset: 0x00024064
		public static float FlatHill(float min, float lower, float upper, float max, float x)
		{
			if (x < min)
			{
				return 0f;
			}
			if (x < lower)
			{
				return Mathf.InverseLerp(min, lower, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return Mathf.InverseLerp(max, upper, x);
			}
			return 0f;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00025EA0 File Offset: 0x000240A0
		public static float FlatHill(float minY, float min, float lower, float upper, float max, float maxY, float x)
		{
			if (x < min)
			{
				return minY;
			}
			if (x < lower)
			{
				return GenMath.LerpDouble(min, lower, minY, 1f, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return GenMath.LerpDouble(upper, max, 1f, maxY, x);
			}
			return maxY;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00025EEE File Offset: 0x000240EE
		public static int OctileDistance(int dx, int dz, int cardinal, int diagonal)
		{
			return cardinal * (dx + dz) + (diagonal - 2 * cardinal) * Mathf.Min(dx, dz);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00025F03 File Offset: 0x00024103
		public static float UnboundedValueToFactor(float val)
		{
			if (val > 0f)
			{
				return 1f + val;
			}
			return 1f / (1f - val);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00025F24 File Offset: 0x00024124
		[DebugOutput("System", false)]
		public static void TestMathPerf()
		{
			IntVec3 intVec = new IntVec3(72, 0, 65);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Math perf tests (" + 10000000f.ToString() + " tests each)");
			float num = 0f;
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num2 = 0;
			while ((float)num2 < 10000000f)
			{
				num += (float)Math.Sqrt(101.20999908447266);
				num2++;
			}
			stringBuilder.AppendLine("(float)System.Math.Sqrt(" + 101.21f.ToString() + "): " + stopwatch.ElapsedTicks.ToString());
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			int num3 = 0;
			while ((float)num3 < 10000000f)
			{
				num += Mathf.Sqrt(101.21f);
				num3++;
			}
			stringBuilder.AppendLine("UnityEngine.Mathf.Sqrt(" + 101.21f.ToString() + "): " + stopwatch2.ElapsedTicks.ToString());
			Stopwatch stopwatch3 = Stopwatch.StartNew();
			int num4 = 0;
			while ((float)num4 < 10000000f)
			{
				num += GenMath.Sqrt(101.21f);
				num4++;
			}
			stringBuilder.AppendLine("Verse.GenMath.Sqrt(" + 101.21f.ToString() + "): " + stopwatch3.ElapsedTicks.ToString());
			Stopwatch stopwatch4 = Stopwatch.StartNew();
			int num5 = 0;
			while ((float)num5 < 10000000f)
			{
				num += (float)intVec.LengthManhattan;
				num5++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthManhattan: " + stopwatch4.ElapsedTicks.ToString());
			Stopwatch stopwatch5 = Stopwatch.StartNew();
			int num6 = 0;
			while ((float)num6 < 10000000f)
			{
				num += intVec.LengthHorizontal;
				num6++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontal: " + stopwatch5.ElapsedTicks.ToString());
			Stopwatch stopwatch6 = Stopwatch.StartNew();
			int num7 = 0;
			while ((float)num7 < 10000000f)
			{
				num += (float)intVec.LengthHorizontalSquared;
				num7++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontalSquared: " + stopwatch6.ElapsedTicks.ToString());
			stringBuilder.AppendLine("total: " + num.ToString());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002617F File Offset: 0x0002437F
		public static float Min(float a, float b, float c)
		{
			if (a < b)
			{
				if (a < c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b < c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00026194 File Offset: 0x00024394
		public static int Max(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b > c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000261A9 File Offset: 0x000243A9
		public static float SphericalDistance(Vector3 normalizedA, Vector3 normalizedB)
		{
			if (normalizedA == normalizedB)
			{
				return 0f;
			}
			return Mathf.Acos(Vector3.Dot(normalizedA, normalizedB));
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000261C8 File Offset: 0x000243C8
		public static void DHondtDistribution(List<int> candidates, Func<int, float> scoreGetter, int numToDistribute)
		{
			GenMath.tmpScores.Clear();
			GenMath.tmpCalcList.Clear();
			for (int i = 0; i < candidates.Count; i++)
			{
				float item = scoreGetter(i);
				candidates[i] = 0;
				GenMath.tmpScores.Add(item);
				GenMath.tmpCalcList.Add(item);
			}
			for (int j = 0; j < numToDistribute; j++)
			{
				int num = GenMath.tmpCalcList.IndexOf(GenMath.tmpCalcList.Max());
				int index = num;
				int num2 = candidates[index];
				candidates[index] = num2 + 1;
				GenMath.tmpCalcList[num] = GenMath.tmpScores[num] / ((float)candidates[num] + 1f);
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002627F File Offset: 0x0002447F
		public static int PositiveMod(int x, int m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0002627F File Offset: 0x0002447F
		public static long PositiveMod(long x, long m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002627F File Offset: 0x0002447F
		public static float PositiveMod(float x, float m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00026288 File Offset: 0x00024488
		public static int PositiveModRemap(long x, int d, int m)
		{
			if (x < 0L)
			{
				x -= (long)(d - 1);
			}
			return (int)((x / (long)d % (long)m + (long)m) % (long)m);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x000262A5 File Offset: 0x000244A5
		public static Vector3 BezierCubicEvaluate(float t, GenMath.BezierCubicControls bcc)
		{
			return GenMath.BezierCubicEvaluate(t, bcc.w0, bcc.w1, bcc.w2, bcc.w3);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000262C8 File Offset: 0x000244C8
		public static Vector3 BezierCubicEvaluate(float t, Vector3 w0, Vector3 w1, Vector3 w2, Vector3 w3)
		{
			float d = t * t;
			float num = 1f - t;
			float d2 = num * num;
			return w0 * d2 * num + 3f * w1 * d2 * t + 3f * w2 * num * d + w3 * d * t;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00026340 File Offset: 0x00024540
		public static float CirclesOverlapArea(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			float num = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
			float num2 = Mathf.Sqrt(num);
			float num3 = r1 * r1;
			float num4 = r2 * r2;
			float num5 = Mathf.Abs(r1 - r2);
			if (num2 >= r1 + r2)
			{
				return 0f;
			}
			if (num2 <= num5 && r1 >= r2)
			{
				return 3.1415927f * num4;
			}
			if (num2 <= num5 && r2 >= r1)
			{
				return 3.1415927f * num3;
			}
			float num6 = Mathf.Acos((num3 - num4 + num) / (2f * r1 * num2)) * 2f;
			float num7 = Mathf.Acos((num4 - num3 + num) / (2f * r2 * num2)) * 2f;
			float num8 = (num7 * num4 - num4 * Mathf.Sin(num7)) * 0.5f;
			float num9 = (num6 * num3 - num3 * Mathf.Sin(num6)) * 0.5f;
			return num8 + num9;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00026412 File Offset: 0x00024612
		public static bool AnyIntegerInRange(float min, float max)
		{
			return Mathf.Ceil(min) <= max;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00026420 File Offset: 0x00024620
		public static void NormalizeToSum1(ref float a, ref float b, ref float c)
		{
			float num = a + b + c;
			if (num == 0f)
			{
				a = 1f;
				b = 0f;
				c = 0f;
				return;
			}
			a /= num;
			b /= num;
			c /= num;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00026466 File Offset: 0x00024666
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
			{
				return Mathf.InverseLerp(a, b, value);
			}
			if (value >= a)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00026484 File Offset: 0x00024684
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			if (by1 >= by2 && by1 >= by3)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3)
			{
				return elem2;
			}
			return elem3;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0002649E File Offset: 0x0002469E
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4)
			{
				return elem3;
			}
			return elem4;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000264D8 File Offset: 0x000246D8
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5)
			{
				return elem4;
			}
			return elem5;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00026544 File Offset: 0x00024744
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6)
			{
				return elem5;
			}
			return elem6;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000265E4 File Offset: 0x000247E4
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7)
			{
				return elem6;
			}
			return elem7;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000266C8 File Offset: 0x000248C8
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7 && by1 >= by8)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7 && by2 >= by8)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7 && by3 >= by8)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7 && by4 >= by8)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7 && by5 >= by8)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7 && by6 >= by8)
			{
				return elem6;
			}
			if (by7 >= by1 && by7 >= by2 && by7 >= by3 && by7 >= by4 && by7 >= by5 && by7 >= by6 && by7 >= by8)
			{
				return elem7;
			}
			return elem8;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000267F6 File Offset: 0x000249F6
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00026808 File Offset: 0x00024A08
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00026820 File Offset: 0x00024A20
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00026848 File Offset: 0x00024A48
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00026874 File Offset: 0x00024A74
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000268A8 File Offset: 0x00024AA8
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7, elem8, -by8);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000268E0 File Offset: 0x00024AE0
		public static T MaxByRandomIfEqual<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8, float eps = 0.0001f)
		{
			return GenMath.MaxBy<T>(elem1, by1 + Rand.Range(0f, eps), elem2, by2 + Rand.Range(0f, eps), elem3, by3 + Rand.Range(0f, eps), elem4, by4 + Rand.Range(0f, eps), elem5, by5 + Rand.Range(0f, eps), elem6, by6 + Rand.Range(0f, eps), elem7, by7 + Rand.Range(0f, eps), elem8, by8 + Rand.Range(0f, eps));
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00026978 File Offset: 0x00024B78
		public static float Stddev(IEnumerable<float> data)
		{
			int num = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (float num4 in data)
			{
				num++;
				num2 += (double)num4;
				num3 += (double)(num4 * num4);
			}
			double num5 = num2 / (double)num;
			return Mathf.Sqrt((float)(num3 / (double)num - num5 * num5));
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000269FC File Offset: 0x00024BFC
		public static float InverseParabola(float x)
		{
			x = Mathf.Clamp01(x);
			return -4f * x * (x - 1f);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00026A18 File Offset: 0x00024C18
		public static float ExponentialWarpInterpolation(float min, float max, float fraction, Vector2 setPoint)
		{
			float p = Mathf.Log(Mathf.InverseLerp(min, max, setPoint.y), setPoint.x);
			float t = Mathf.Pow(fraction, p);
			return Mathf.Lerp(min, max, t);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00026A50 File Offset: 0x00024C50
		public static float InverseExponentialWarpInterpolation(float min, float max, float value, Vector2 setPoint)
		{
			float num = Mathf.Log(Mathf.InverseLerp(min, max, setPoint.y), setPoint.x);
			return Mathf.Exp(Mathf.Log(Mathf.InverseLerp(min, max, value)) / num);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00026A8A File Offset: 0x00024C8A
		public static float Sign(float val)
		{
			if (val >= 0f)
			{
				return 1f;
			}
			return -1f;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00026A9F File Offset: 0x00024C9F
		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00026AAF File Offset: 0x00024CAF
		public static float RemapClamped(this float value, float from1, float to1, float from2, float to2)
		{
			return (Mathf.Clamp(value, from1, to1) - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00026AC6 File Offset: 0x00024CC6
		public static Vector3 Position(this Matrix4x4 matrix)
		{
			return matrix.GetColumn(3);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00026AD8 File Offset: 0x00024CD8
		public static float MeanAngle(List<float> angles)
		{
			float x = angles.Sum((float a) => Mathf.Cos(a * 3.1415927f / 180f)) / (float)angles.Count;
			return Mathf.Atan2(angles.Sum((float a) => Mathf.Sin(a * 3.1415927f / 180f)) / (float)angles.Count, x) * 180f / 3.1415927f;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00026B54 File Offset: 0x00024D54
		public static float SmoothMin(float a, float b, float k)
		{
			if (k == 0f)
			{
				return Mathf.Min(a, b);
			}
			float num = Mathf.Max(0f, Mathf.Min(1f, (b - a) / k + 0.5f));
			return a * num + b * (1f - num) - k * num * (1f - num);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00026BAC File Offset: 0x00024DAC
		public static double SmoothMin(double a, double b, double k)
		{
			if (k == 0.0)
			{
				return Math.Min(a, b);
			}
			double num = Math.Max(0.0, Math.Min(1.0, (b - a) / k + 0.5));
			return a * num + b * (1.0 - num) - k * num * (1.0 - num);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00026C1A File Offset: 0x00024E1A
		public static Vector3 RotateScale(this Vector3 vec, RotationDirection rotation)
		{
			if (rotation == RotationDirection.Clockwise || rotation == RotationDirection.Counterclockwise)
			{
				return new Vector3(vec.z, vec.y, vec.x);
			}
			return vec;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00026C3D File Offset: 0x00024E3D
		public static Vector3 RotateScale(this Vector3 vec, Rot4 orientation)
		{
			return vec.RotateScale((RotationDirection)orientation.AsInt);
		}

		// Token: 0x0400040A RID: 1034
		public const float BigEpsilon = 1E-07f;

		// Token: 0x0400040B RID: 1035
		public const float Sqrt2 = 1.4142135f;

		// Token: 0x0400040C RID: 1036
		private static List<float> tmpElements = new List<float>();

		// Token: 0x0400040D RID: 1037
		private static List<Pair<float, float>> tmpPairs = new List<Pair<float, float>>();

		// Token: 0x0400040E RID: 1038
		private static List<float> tmpScores = new List<float>();

		// Token: 0x0400040F RID: 1039
		private static List<float> tmpCalcList = new List<float>();

		// Token: 0x02000110 RID: 272
		public struct BezierCubicControls
		{
			// Token: 0x04000410 RID: 1040
			public Vector3 w0;

			// Token: 0x04000411 RID: 1041
			public Vector3 w1;

			// Token: 0x04000412 RID: 1042
			public Vector3 w2;

			// Token: 0x04000413 RID: 1043
			public Vector3 w3;
		}
	}
}
