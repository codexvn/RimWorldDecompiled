using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200223B RID: 8763
	public class CompAbilityEffect_GiveHediffPsychic : CompAbilityEffect_GiveHediff
	{
		// Token: 0x17001D3A RID: 7482
		// (get) Token: 0x0600BF6C RID: 49004 RVA: 0x0037811E File Offset: 0x0037631E
		public new CompProperties_AbilityGiveHediffPsychic Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediffPsychic)this.props;
			}
		}

		// Token: 0x0600BF6D RID: 49005 RVA: 0x0037812B File Offset: 0x0037632B
		protected override bool TryResist(Pawn pawn)
		{
			return Rand.Chance(1f - pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1));
		}
	}
}
