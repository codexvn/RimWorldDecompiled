using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A6C RID: 14956
	public class QuestNode_GetPlantPlayerCanHarvest : QuestNode
	{
		// Token: 0x06014718 RID: 83736 RVA: 0x0061B76B File Offset: 0x0061996B
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x06014719 RID: 83737 RVA: 0x0061B774 File Offset: 0x00619974
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0601471A RID: 83738 RVA: 0x0061B784 File Offset: 0x00619984
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x = slate.Get<float>("points", 0f, false);
			float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(map.Tile);
			float lowestTemp = seasonalTemp;
			float highestTemp = seasonalTemp;
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 15; i++)
			{
				int absTick = ticksAbs + 60000 * i;
				float num = seasonalTemp + Find.World.tileTemperatures.OffsetFromDailyRandomVariation(map.Tile, absTick);
				if (num < lowestTemp)
				{
					lowestTemp = num;
				}
				if (num > highestTemp)
				{
					highestTemp = num;
				}
			}
			ThingDef thingDef;
			if (!(from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant && !def.plant.cavePlant && def.plant.Sowable && def.plant.harvestedThingDef != null && def.plant.growDays <= (float)this.maxPlantGrowDays.GetValue(slate) && def.plant.minGrowthTemperature <= lowestTemp + 5f && def.plant.maxGrowthTemperature >= highestTemp - 5f && Command_SetPlantToGrow.IsPlantAvailable(def, map)
			select def).TryRandomElement(out thingDef))
			{
				return false;
			}
			SimpleCurve value = this.pointsToRequiredWorkCurve.GetValue(slate);
			float randomInRange = (this.workRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			float num2 = value.Evaluate(x) * randomInRange;
			float num3 = (thingDef.plant.sowWork + thingDef.plant.harvestWork) / thingDef.plant.harvestYield;
			int num4 = GenMath.RoundRandom(num2 / num3);
			num4 = Mathf.Max(num4, 1);
			slate.Set<ThingDef>(this.storeHarvestItemDefAs.GetValue(slate), thingDef.plant.harvestedThingDef, false);
			slate.Set<int>(this.storeHarvestItemCountAs.GetValue(slate), num4, false);
			if (this.storeGrowDaysAs.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeGrowDaysAs.GetValue(slate), thingDef.plant.growDays, false);
			}
			return true;
		}

		// Token: 0x0400E231 RID: 57905
		[NoTranslate]
		public SlateRef<string> storeHarvestItemDefAs;

		// Token: 0x0400E232 RID: 57906
		[NoTranslate]
		public SlateRef<string> storeHarvestItemCountAs;

		// Token: 0x0400E233 RID: 57907
		[NoTranslate]
		public SlateRef<string> storeGrowDaysAs;

		// Token: 0x0400E234 RID: 57908
		public SlateRef<int> maxPlantGrowDays;

		// Token: 0x0400E235 RID: 57909
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x0400E236 RID: 57910
		public SlateRef<FloatRange?> workRandomFactorRange;

		// Token: 0x0400E237 RID: 57911
		private const float TemperatureBuffer = 5f;

		// Token: 0x0400E238 RID: 57912
		private const int TemperatureCheckDays = 15;
	}
}
