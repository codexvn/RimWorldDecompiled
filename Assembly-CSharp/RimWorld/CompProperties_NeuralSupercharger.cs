using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001946 RID: 6470
	public class CompProperties_NeuralSupercharger : CompProperties_Rechargeable
	{
		// Token: 0x06009329 RID: 37673 RVA: 0x002B4DB7 File Offset: 0x002B2FB7
		public CompProperties_NeuralSupercharger()
		{
			this.compClass = typeof(CompNeuralSupercharger);
		}

		// Token: 0x0600932A RID: 37674 RVA: 0x002B4DCF File Offset: 0x002B2FCF
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.effectCharged != null && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("CompProperties_NeuralSupercharger has effectCharged but parent {0} has tickerType!=Normal", parentDef);
			}
			yield break;
		}

		// Token: 0x04005E9F RID: 24223
		[MustTranslate]
		public string jobString;

		// Token: 0x04005EA0 RID: 24224
		public EffecterDef effectCharged;
	}
}
