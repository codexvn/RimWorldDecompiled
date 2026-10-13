using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001966 RID: 6502
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x060093AA RID: 37802 RVA: 0x002B6F18 File Offset: 0x002B5118
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x060093AB RID: 37803 RVA: 0x002B6F6A File Offset: 0x002B516A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.eggFertilizedDef == null)
			{
				yield return "eggFertilizedDef is null";
			}
			else if (this.eggUnfertilizedDef == null)
			{
				yield return "eggUnfertilizedDef is null";
			}
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04005F02 RID: 24322
		public float eggLayIntervalDays = 1f;

		// Token: 0x04005F03 RID: 24323
		public IntRange eggCountRange = IntRange.One;

		// Token: 0x04005F04 RID: 24324
		public ThingDef eggUnfertilizedDef;

		// Token: 0x04005F05 RID: 24325
		public ThingDef eggFertilizedDef;

		// Token: 0x04005F06 RID: 24326
		public int eggFertilizationCountMax = 1;

		// Token: 0x04005F07 RID: 24327
		public bool eggLayFemaleOnly = true;

		// Token: 0x04005F08 RID: 24328
		public float eggProgressUnfertilizedMax = 1f;
	}
}
