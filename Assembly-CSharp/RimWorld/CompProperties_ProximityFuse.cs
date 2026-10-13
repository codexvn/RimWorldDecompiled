using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197E RID: 6526
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x06009412 RID: 37906 RVA: 0x002B842E File Offset: 0x002B662E
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x06009413 RID: 37907 RVA: 0x002B8446 File Offset: 0x002B6646
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompProximityFuse needs tickerType " + TickerType.Rare.ToString() + " or faster, has " + parentDef.tickerType.ToString();
			}
			if (parentDef.CompDefFor<CompExplosive>() == null)
			{
				yield return "CompProximityFuse requires a CompExplosive";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005F9C RID: 24476
		public ThingDef target;

		// Token: 0x04005F9D RID: 24477
		public float radius;
	}
}
