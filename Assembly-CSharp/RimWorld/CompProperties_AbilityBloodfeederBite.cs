using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F5 RID: 8693
	public class CompProperties_AbilityBloodfeederBite : CompProperties_AbilityEffect
	{
		// Token: 0x0600BE65 RID: 48741 RVA: 0x003734EC File Offset: 0x003716EC
		public CompProperties_AbilityBloodfeederBite()
		{
			this.compClass = typeof(CompAbilityEffect_BloodfeederBite);
		}

		// Token: 0x0600BE66 RID: 48742 RVA: 0x0037351A File Offset: 0x0037171A
		public override IEnumerable<string> ExtraStatSummary()
		{
			yield return "AbilityHemogenGain".Translate() + ": " + (this.hemogenGain * 100f).ToString("F0");
			yield break;
		}

		// Token: 0x040082CE RID: 33486
		public float hemogenGain;

		// Token: 0x040082CF RID: 33487
		public ThoughtDef thoughtDefToGiveTarget;

		// Token: 0x040082D0 RID: 33488
		public ThoughtDef opinionThoughtDefToGiveTarget;

		// Token: 0x040082D1 RID: 33489
		public float resistanceGain;

		// Token: 0x040082D2 RID: 33490
		public float nutritionGain = 0.1f;

		// Token: 0x040082D3 RID: 33491
		public float targetBloodLoss = 0.4499f;

		// Token: 0x040082D4 RID: 33492
		public IntRange bloodFilthToSpawnRange;
	}
}
