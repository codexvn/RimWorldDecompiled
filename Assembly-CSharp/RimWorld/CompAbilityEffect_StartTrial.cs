using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200228C RID: 8844
	public class CompAbilityEffect_StartTrial : CompAbilityEffect_StartRitualOnPawn
	{
		// Token: 0x17001D6F RID: 7535
		// (get) Token: 0x0600C08B RID: 49291 RVA: 0x0037C352 File Offset: 0x0037A552
		public new CompProperties_AbilityStartTrial Props
		{
			get
			{
				return (CompProperties_AbilityStartTrial)this.props;
			}
		}

		// Token: 0x0600C08C RID: 49292 RVA: 0x0037C360 File Offset: 0x0037A560
		protected override Precept_Ritual RitualForTarget(Pawn pawn)
		{
			PreceptDef preceptDef = pawn.InMentalState ? this.Props.ritualDefForMentalState : (pawn.IsPrisonerOfColony ? this.Props.ritualDefForPrisoner : null);
			if (preceptDef != null)
			{
				for (int i = 0; i < this.parent.pawn.Ideo.PreceptsListForReading.Count; i++)
				{
					if (this.parent.pawn.Ideo.PreceptsListForReading[i].def == preceptDef)
					{
						return (Precept_Ritual)this.parent.pawn.Ideo.PreceptsListForReading[i];
					}
				}
			}
			return base.RitualForTarget(pawn);
		}

		// Token: 0x0600C08D RID: 49293 RVA: 0x0037C40C File Offset: 0x0037A60C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateCanWalk(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNotGuilty(pawn, throwMessages, this.parent) && base.Valid(target, throwMessages);
		}
	}
}
