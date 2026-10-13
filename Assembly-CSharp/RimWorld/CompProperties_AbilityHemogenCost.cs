using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002244 RID: 8772
	public class CompProperties_AbilityHemogenCost : CompProperties_AbilityEffect
	{
		// Token: 0x0600BF85 RID: 49029 RVA: 0x00378771 File Offset: 0x00376971
		public CompProperties_AbilityHemogenCost()
		{
			this.compClass = typeof(CompAbilityEffect_HemogenCost);
		}

		// Token: 0x0600BF86 RID: 49030 RVA: 0x00378789 File Offset: 0x00376989
		public override IEnumerable<string> ExtraStatSummary()
		{
			yield return "AbilityHemogenCost".Translate() + ": " + Mathf.RoundToInt(this.hemogenCost * 100f).ToString();
			yield break;
		}

		// Token: 0x04008368 RID: 33640
		public float hemogenCost;
	}
}
