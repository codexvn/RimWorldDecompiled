using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A12 RID: 6674
	public class ChemicalDef : Def
	{
		// Token: 0x06009680 RID: 38528 RVA: 0x002C09A0 File Offset: 0x002BEBA0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.canBeAddicted && this.addictionHediff == null)
			{
				yield return "addictionHediff is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x040062C3 RID: 25283
		public HediffDef addictionHediff;

		// Token: 0x040062C4 RID: 25284
		public HediffDef toleranceHediff;

		// Token: 0x040062C5 RID: 25285
		public bool canBinge = true;

		// Token: 0x040062C6 RID: 25286
		public bool canBeAddicted = true;

		// Token: 0x040062C7 RID: 25287
		public bool generateAddictionGenes = true;

		// Token: 0x040062C8 RID: 25288
		public float geneOverdoseChanceFactorResist = 1f;

		// Token: 0x040062C9 RID: 25289
		public float geneOverdoseChanceFactorImmune = 1f;

		// Token: 0x040062CA RID: 25290
		public float geneToleranceBuildupFactorResist = 1f;

		// Token: 0x040062CB RID: 25291
		public float geneToleranceBuildupFactorImmune = 1f;

		// Token: 0x040062CC RID: 25292
		public float onGeneratedAddictedToleranceChance;

		// Token: 0x040062CD RID: 25293
		public List<HediffGiver_Event> onGeneratedAddictedEvents;
	}
}
