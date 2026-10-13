using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002205 RID: 8709
	public class CompProperties_AbilityConvert : CompProperties_AbilityEffect
	{
		// Token: 0x0600BEA5 RID: 48805 RVA: 0x003744C9 File Offset: 0x003726C9
		public CompProperties_AbilityConvert()
		{
			this.compClass = typeof(CompAbilityEffect_Convert);
		}

		// Token: 0x0600BEA6 RID: 48806 RVA: 0x003744EC File Offset: 0x003726EC
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			if (this.convertPowerFactor < 0f)
			{
				yield return "convertPowerFactor not set";
			}
			yield break;
		}

		// Token: 0x040082EA RID: 33514
		[MustTranslate]
		public string successMessage;

		// Token: 0x040082EB RID: 33515
		[MustTranslate]
		public string failMessage;

		// Token: 0x040082EC RID: 33516
		public ThoughtDef failedThoughtInitiator;

		// Token: 0x040082ED RID: 33517
		public ThoughtDef failedThoughtRecipient;

		// Token: 0x040082EE RID: 33518
		public float convertPowerFactor = -1f;
	}
}
