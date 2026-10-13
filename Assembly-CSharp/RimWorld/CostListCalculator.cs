using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003568 RID: 13672
	public static class CostListCalculator
	{
		// Token: 0x060132BD RID: 78525 RVA: 0x005B2E76 File Offset: 0x005B1076
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x060132BE RID: 78526 RVA: 0x005B2E82 File Offset: 0x005B1082
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x060132BF RID: 78527 RVA: 0x005B2E98 File Offset: 0x005B1098
		public static List<ThingDefCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			if (CostListCalculator.cachedDifficulty != Find.Storyteller.difficulty)
			{
				CostListCalculator.Reset();
				CostListCalculator.cachedDifficulty = Find.Storyteller.difficulty;
			}
			CostListCalculator.CostListPair key = new CostListCalculator.CostListPair(entDef, stuff);
			List<ThingDefCountClass> list;
			if (!CostListCalculator.cachedCosts.TryGetValue(key, out list))
			{
				list = new List<ThingDefCountClass>();
				int num = 0;
				if (entDef.MadeFromStuff)
				{
					if (errorOnNullStuff && stuff == null)
					{
						Log.Error("Cannot get AdjustedCostList for " + ((entDef != null) ? entDef.ToString() : null) + " with null Stuff.");
						if (GenStuff.DefaultStuffFor(entDef) == null)
						{
							return null;
						}
						return entDef.CostListAdjusted(GenStuff.DefaultStuffFor(entDef), true);
					}
					else if (stuff != null)
					{
						num = Mathf.RoundToInt((float)entDef.CostStuffCount / stuff.VolumePerUnit);
						if (num < 1)
						{
							num = 1;
						}
					}
					else
					{
						num = entDef.CostStuffCount;
					}
				}
				else if (stuff != null)
				{
					Log.Error(string.Concat(new string[]
					{
						"Got AdjustedCostList for ",
						(entDef != null) ? entDef.ToString() : null,
						" with stuff ",
						(stuff != null) ? stuff.ToString() : null,
						" but is not MadeFromStuff."
					}));
				}
				bool flag = false;
				if (entDef.CostList != null)
				{
					for (int i = 0; i < entDef.CostList.Count; i++)
					{
						ThingDefCountClass thingDefCountClass = entDef.CostList[i];
						if (thingDefCountClass.thingDef == stuff)
						{
							list.Add(new ThingDefCountClass(thingDefCountClass.thingDef, thingDefCountClass.count + num));
							flag = true;
						}
						else
						{
							list.Add(thingDefCountClass);
						}
					}
				}
				if (!flag && num > 0)
				{
					list.Add(new ThingDefCountClass(stuff, num));
				}
				CostListCalculator.cachedCosts.Add(key, list);
			}
			return list;
		}

		// Token: 0x0400C6F7 RID: 50935
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x0400C6F8 RID: 50936
		private static Difficulty cachedDifficulty = null;

		// Token: 0x02003569 RID: 13673
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x060132C1 RID: 78529 RVA: 0x005B3043 File Offset: 0x005B1243
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x060132C2 RID: 78530 RVA: 0x005B3053 File Offset: 0x005B1253
			public override int GetHashCode()
			{
				return Gen.HashCombine<ThingDef>(Gen.HashCombine<BuildableDef>(0, this.buildable), this.stuff);
			}

			// Token: 0x060132C3 RID: 78531 RVA: 0x005B306C File Offset: 0x005B126C
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x060132C4 RID: 78532 RVA: 0x005B3084 File Offset: 0x005B1284
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x060132C5 RID: 78533 RVA: 0x005B3092 File Offset: 0x005B1292
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x060132C6 RID: 78534 RVA: 0x005B30B2 File Offset: 0x005B12B2
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400C6F9 RID: 50937
			public BuildableDef buildable;

			// Token: 0x0400C6FA RID: 50938
			public ThingDef stuff;
		}

		// Token: 0x0200356A RID: 13674
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x060132C7 RID: 78535 RVA: 0x005B30BE File Offset: 0x005B12BE
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x060132C8 RID: 78536 RVA: 0x005B30C7 File Offset: 0x005B12C7
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x0400C6FB RID: 50939
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();
		}
	}
}
