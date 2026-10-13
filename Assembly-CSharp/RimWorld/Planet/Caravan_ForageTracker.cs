using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D92 RID: 15762
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x17003564 RID: 13668
		// (get) Token: 0x06015788 RID: 87944 RVA: 0x0066EFB5 File Offset: 0x0066D1B5
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public ValueTuple<ThingDef, float> ForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x17003565 RID: 13669
		// (get) Token: 0x06015789 RID: 87945 RVA: 0x0066EFC4 File Offset: 0x0066D1C4
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0601578A RID: 87946 RVA: 0x0066EFEA File Offset: 0x0066D1EA
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0601578B RID: 87947 RVA: 0x0066EFF9 File Offset: 0x0066D1F9
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x0601578C RID: 87948 RVA: 0x0066F011 File Offset: 0x0066D211
		public void ForageTrackerTickInterval(int delta)
		{
			if (this.caravan.IsHashIntervalTick(15, delta))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x0601578D RID: 87949 RVA: 0x0066F029 File Offset: 0x0066D229
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Forage",
					action = new Action(this.Forage)
				};
			}
			yield break;
		}

		// Token: 0x0601578E RID: 87950 RVA: 0x0066F03C File Offset: 0x0066D23C
		private void UpdateProgressInterval()
		{
			float num = 15f * ForagedFoodPerDayCalculator.GetProgressPerTick(this.caravan, null);
			this.progress += num;
			if (this.progress >= 1f)
			{
				this.Forage();
				this.progress = 0f;
			}
		}

		// Token: 0x0601578F RID: 87951 RVA: 0x0066F088 File Offset: 0x0066D288
		private void Forage()
		{
			ThingDef foragedFood = this.caravan.Biome.foragedFood;
			if (foragedFood == null)
			{
				return;
			}
			int i = GenMath.RoundRandom(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(this.caravan, null));
			int b = Mathf.FloorToInt((this.caravan.MassCapacity - this.caravan.MassUsage) / foragedFood.GetStatValueAbstract(StatDefOf.Mass, null));
			i = Mathf.Min(i, b);
			while (i > 0)
			{
				Thing thing = ThingMaker.MakeThing(foragedFood, null);
				thing.stackCount = Mathf.Min(i, foragedFood.stackLimit);
				i -= thing.stackCount;
				CaravanInventoryUtility.GiveThing(this.caravan, thing);
			}
		}

		// Token: 0x0400EEF9 RID: 61177
		private Caravan caravan;

		// Token: 0x0400EEFA RID: 61178
		private float progress;

		// Token: 0x0400EEFB RID: 61179
		private const int UpdateProgressIntervalTicks = 15;
	}
}
