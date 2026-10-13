using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002276 RID: 8822
	public class CompAbilityEffect_ShowTargetIdeoCertainty : CompAbilityEffect
	{
		// Token: 0x17001D62 RID: 7522
		// (get) Token: 0x0600C049 RID: 49225 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C04A RID: 49226 RVA: 0x0037B2D0 File Offset: 0x003794D0
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Pawn pawn = target.Pawn;
				if (pawn.Ideo != null)
				{
					return "IdeoCertaintyTooltip".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"), pawn.ideo.Certainty.ToStringPercent().Named("PERCENTAGE"));
				}
			}
			return null;
		}
	}
}
