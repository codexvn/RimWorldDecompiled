using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AB RID: 6571
	public class IngestionOutcomeDoer_OffsetHemogen : IngestionOutcomeDoer
	{
		// Token: 0x060094AE RID: 38062 RVA: 0x002BA417 File Offset: 0x002B8617
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			GeneUtility.OffsetHemogen(pawn, this.offset * (float)ingestedCount, true);
		}

		// Token: 0x060094AF RID: 38063 RVA: 0x002BA429 File Offset: 0x002B8629
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (ModsConfig.BiotechActive)
			{
				string str = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Hemogen".Translate().CapitalizeFirst(), str + Mathf.RoundToInt(this.offset * 100f).ToString(), "HemogenDesc".Translate(), 1000, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x04006091 RID: 24721
		public float offset;
	}
}
