using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001982 RID: 6530
	public class CompProperties_SelfhealHitpoints : CompProperties
	{
		// Token: 0x0600942D RID: 37933 RVA: 0x002B894F File Offset: 0x002B6B4F
		public CompProperties_SelfhealHitpoints()
		{
			this.compClass = typeof(CompSelfhealHitpoints);
		}

		// Token: 0x0600942E RID: 37934 RVA: 0x002B8967 File Offset: 0x002B6B67
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType == TickerType.Rare && this.ticksPerHeal % 250 != 0)
			{
				yield return "TickerType is set to Rare, but ticksPerHeal value is not multiple of " + 250.ToString();
			}
			if (parentDef.tickerType == TickerType.Long && this.ticksPerHeal % 2000 != 0)
			{
				yield return "TickerType is set to Long, but ticksPerHeal value is not multiple of " + 2000.ToString();
			}
			if (parentDef.tickerType == TickerType.Never)
			{
				yield return "has CompSelfhealHitpoints, but its TickerType is set to Never";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005FB2 RID: 24498
		public int ticksPerHeal;
	}
}
