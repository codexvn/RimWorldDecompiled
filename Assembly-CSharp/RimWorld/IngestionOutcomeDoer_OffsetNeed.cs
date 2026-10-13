using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A7 RID: 6567
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x06009498 RID: 38040 RVA: 0x002BA13C File Offset: 0x002B833C
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			if (pawn.needs == null)
			{
				return;
			}
			Need need;
			if (!pawn.needs.TryGetNeed(this.need, out need))
			{
				return;
			}
			float num = this.offset * (float)ingestedCount;
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num, false, true);
			if (this.perIngested)
			{
				num *= (float)ingested.stackCount;
			}
			need.CurLevel += num;
		}

		// Token: 0x06009499 RID: 38041 RVA: 0x002BA1A2 File Offset: 0x002B83A2
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			string str = (this.offset >= 0f) ? "+" : string.Empty;
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, this.need.LabelCap, str + this.offset.ToStringPercent(), this.need.description, this.need.listPriority, null, null, false, false);
			yield break;
		}

		// Token: 0x04006084 RID: 24708
		public NeedDef need;

		// Token: 0x04006085 RID: 24709
		public float offset;

		// Token: 0x04006086 RID: 24710
		public ChemicalDef toleranceChemical;

		// Token: 0x04006087 RID: 24711
		public bool perIngested;
	}
}
