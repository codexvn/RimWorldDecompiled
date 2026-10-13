using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000149 RID: 329
	public static class MathEvaluatorCustomFunctions
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x0002EF50 File Offset: 0x0002D150
		private static object Min(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Min(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0002EF80 File Offset: 0x0002D180
		private static object Max(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Max(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002EFB0 File Offset: 0x0002D1B0
		private static object Round(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002EFD8 File Offset: 0x0002D1D8
		private static object RoundToDigits(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002F008 File Offset: 0x0002D208
		private static object Floor(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Floor(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002F030 File Offset: 0x0002D230
		private static object RoundRandom(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)GenMath.RoundRandom(Convert.ToSingle(args[0], invariantCulture));
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002F058 File Offset: 0x0002D258
		private static object RandInt(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.RangeInclusive(Convert.ToInt32(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002F088 File Offset: 0x0002D288
		private static object RandFloat(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.Range(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture));
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002F0B8 File Offset: 0x0002D2B8
		private static object RangeAverage(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)FloatRange.FromString(Convert.ToString(args[0], invariantCulture)).Average;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002F0E8 File Offset: 0x0002D2E8
		private static object RoundToTicksRough(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int num = Convert.ToInt32(args[0], invariantCulture);
			if (num <= 250)
			{
				return 250;
			}
			if (num < 5000)
			{
				return GenMath.RoundTo(num, 250);
			}
			if (num < 60000)
			{
				return GenMath.RoundTo(num, 2500);
			}
			if (num < 120000)
			{
				return GenMath.RoundTo(num, 6000);
			}
			return GenMath.RoundTo(num, 60000);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002F174 File Offset: 0x0002D374
		public static object Lerp(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Mathf.Lerp(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture), Convert.ToSingle(args[2], invariantCulture));
		}

		// Token: 0x0400050F RID: 1295
		public static readonly MathEvaluatorCustomFunctions.FunctionType[] FunctionTypes = new MathEvaluatorCustomFunctions.FunctionType[]
		{
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "min",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Min)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "max",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Max)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "round",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Round)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundToDigits",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundToDigits)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "floor",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Floor)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundRandom",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundRandom)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "randInt",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RandInt)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "randFloat",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RandFloat)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "rangeAverage",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RangeAverage)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundToTicksRough",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundToTicksRough)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "lerp",
				minArgs = 3,
				maxArgs = 3,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Lerp)
			}
		};

		// Token: 0x0200014A RID: 330
		public class FunctionType
		{
			// Token: 0x04000510 RID: 1296
			public string name;

			// Token: 0x04000511 RID: 1297
			public int minArgs;

			// Token: 0x04000512 RID: 1298
			public int maxArgs;

			// Token: 0x04000513 RID: 1299
			public Func<object[], object> func;
		}
	}
}
