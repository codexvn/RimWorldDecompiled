using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B6F RID: 11119
	public class ThingSetMaker_Nutrition : ThingSetMaker
	{
		// Token: 0x0600F8A4 RID: 63652 RVA: 0x00484E17 File Offset: 0x00483017
		public ThingSetMaker_Nutrition()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x0600F8A5 RID: 63653 RVA: 0x00484E2C File Offset: 0x0048302C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				return false;
			}
			if (parms.countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			if (parms.totalNutritionRange == null || parms.totalNutritionRange.Value.max <= 0f)
			{
				return false;
			}
			float maxValue;
			if (parms.maxTotalMass != null)
			{
				float? maxTotalMass = parms.maxTotalMass;
				maxValue = float.MaxValue;
				if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null) && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1))
				{
					return false;
				}
			}
			return this.GeneratePossibleDefs(parms, out maxValue, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x0600F8A6 RID: 63654 RVA: 0x00484F28 File Offset: 0x00483128
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing(false));
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.GetStatValue(StatDefOf.Nutrition, true, -1), maxMass, false);
			this.nextSeed++;
		}

		// Token: 0x0600F8A7 RID: 63655 RVA: 0x0048465D File Offset: 0x0048285D
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x0600F8A8 RID: 63656 RVA: 0x00484FC7 File Offset: 0x004831C7
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalNutrition);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600F8A9 RID: 63657 RVA: 0x00484FDC File Offset: 0x004831DC
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition)
		{
			ThingSetMaker_Nutrition.<>c__DisplayClass6_0 CS$<>8__locals1 = new ThingSetMaker_Nutrition.<>c__DisplayClass6_0();
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalNutrition = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalNutritionRange ?? FloatRange.Zero;
			TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			QualityGenerator valueOrDefault2 = parms.qualityGenerator.GetValueOrDefault();
			totalNutrition = floatRange.RandomInRange;
			CS$<>8__locals1.numMeats = enumerable.Count((ThingDef x) => x.IsMeat);
			CS$<>8__locals1.numLeathers = enumerable.Count((ThingDef x) => x.IsLeather);
			CS$<>8__locals1.numEggs = enumerable.Count((ThingDef x) => x.IsEgg);
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalNutrition, enumerable, valueOrDefault, valueOrDefault2, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), new Func<ThingDef, float>(CS$<>8__locals1.<GeneratePossibleDefs>g__WeightSelector|3), 100, maxMass, true, 0f);
		}

		// Token: 0x0600F8AA RID: 63658 RVA: 0x00485195 File Offset: 0x00483395
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
				if (parms.totalNutritionRange == null || parms.totalNutritionRange.Value.max == 3.4028235E+38f || !thingDef.IsNutritionGivingIngestible || thingDef.ingestible.CachedNutrition <= parms.totalNutritionRange.Value.max)
				{
					yield return thingDef;
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400A383 RID: 41859
		private int nextSeed;
	}
}
