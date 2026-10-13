using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B6C RID: 11116
	public class ThingSetMaker_MarketValue : ThingSetMaker
	{
		// Token: 0x0600F88E RID: 63630 RVA: 0x0048488B File Offset: 0x00482A8B
		public ThingSetMaker_MarketValue()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x0600F88F RID: 63631 RVA: 0x004848A0 File Offset: 0x00482AA0
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			List<ThingDef> list = this.AllowedThingDefs(parms).ToList<ThingDef>();
			if (!list.Any<ThingDef>())
			{
				return false;
			}
			if (parms.countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			if (parms.totalMarketValueRange == null || parms.totalMarketValueRange.Value.max <= 0f)
			{
				return false;
			}
			float maxValue;
			if (parms.maxTotalMass != null)
			{
				float? maxTotalMass = parms.maxTotalMass;
				maxValue = float.MaxValue;
				if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null) && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(list, parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1))
				{
					return false;
				}
			}
			return this.GeneratePossibleDefs(parms, out maxValue, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x0600F890 RID: 63632 RVA: 0x0048499C File Offset: 0x00482B9C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing(false));
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.MarketValue, maxMass, true);
			this.nextSeed++;
		}

		// Token: 0x0600F891 RID: 63633 RVA: 0x0048465D File Offset: 0x0048285D
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x0600F892 RID: 63634 RVA: 0x00484A3B File Offset: 0x00482C3B
		private float GetSingleThingValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
		}

		// Token: 0x0600F893 RID: 63635 RVA: 0x00484A49 File Offset: 0x00482C49
		private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.minRewardCount;
		}

		// Token: 0x0600F894 RID: 63636 RVA: 0x00484A64 File Offset: 0x00482C64
		private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.stackLimit;
		}

		// Token: 0x0600F895 RID: 63637 RVA: 0x00484A7F File Offset: 0x00482C7F
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalMarketValue);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600F896 RID: 63638 RVA: 0x00484A94 File Offset: 0x00482C94
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalMarketValue = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalMarketValueRange ?? FloatRange.Zero;
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			QualityGenerator valueOrDefault2 = parms.qualityGenerator.GetValueOrDefault();
			totalMarketValue = floatRange.RandomInRange;
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalMarketValue, enumerable, valueOrDefault, valueOrDefault2, new Func<ThingStuffPairWithQuality, float>(this.GetMinValue), new Func<ThingStuffPairWithQuality, float>(this.GetMaxValue), new Func<ThingStuffPairWithQuality, float>(this.GetSingleThingValue), null, 100, maxMass, parms.allowNonStackableDuplicates.GetValueOrDefault(true), totalMarketValue * parms.minSingleItemMarketValuePct.GetValueOrDefault());
		}

		// Token: 0x0600F897 RID: 63639 RVA: 0x00484B96 File Offset: 0x00482D96
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel.GetValueOrDefault();
			foreach (ThingDef thingDef in this.AllowedThingDefs(parms))
			{
				if (parms.maxTotalMass != null)
				{
					float? maxTotalMass = parms.maxTotalMass;
					float maxValue = float.MaxValue;
					if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null))
					{
						float minMass = ThingSetMakerUtility.GetMinMass(thingDef, techLevel);
						maxTotalMass = parms.maxTotalMass;
						if (minMass > maxTotalMass.GetValueOrDefault() & maxTotalMass != null)
						{
							continue;
						}
					}
				}
				if (parms.totalMarketValueRange == null || parms.totalMarketValueRange.Value.max == 3.4028235E+38f || ThingSetMakerUtility.GetMinMarketValue(thingDef, techLevel) <= parms.totalMarketValueRange.Value.max)
				{
					yield return thingDef;
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400A378 RID: 41848
		private int nextSeed;
	}
}
