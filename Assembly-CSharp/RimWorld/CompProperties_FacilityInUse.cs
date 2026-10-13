using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C81 RID: 11393
	public class CompProperties_FacilityInUse : CompProperties
	{
		// Token: 0x0600FFBE RID: 65470 RVA: 0x004A19F5 File Offset: 0x0049FBF5
		public CompProperties_FacilityInUse()
		{
			this.compClass = typeof(CompFacilityInUse);
		}

		// Token: 0x0600FFBF RID: 65471 RVA: 0x004A1A0D File Offset: 0x0049FC0D
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType == TickerType.Never)
			{
				yield return string.Format("CompProperties_FacilityInUse has parent {0} with tickerType=Never", parentDef);
			}
			if (this.effectInUse != null && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("CompProperties_FacilityInUse has effectInUse but parent {0} has tickerType!=Normal", parentDef);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A743 RID: 42819
		public float? inUsePowerConsumption;

		// Token: 0x0400A744 RID: 42820
		public EffecterDef effectInUse;
	}
}
