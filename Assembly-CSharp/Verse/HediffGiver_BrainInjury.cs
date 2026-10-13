using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000778 RID: 1912
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x06002EE2 RID: 12002 RVA: 0x000F0EAC File Offset: 0x000EF0AC
		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			if (!(hediff is Hediff_Injury))
			{
				return false;
			}
			if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				return false;
			}
			if (pawn.IsSubhuman)
			{
				return false;
			}
			float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
			if (Rand.Value < num * this.chancePerDamagePct && base.TryApply(pawn, null))
			{
				if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040024A7 RID: 9383
		public float chancePerDamagePct;

		// Token: 0x040024A8 RID: 9384
		public string letterLabel;

		// Token: 0x040024A9 RID: 9385
		public string letter;
	}
}
