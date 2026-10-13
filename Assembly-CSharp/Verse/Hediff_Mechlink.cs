using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000754 RID: 1876
	public class Hediff_Mechlink : HediffWithComps
	{
		// Token: 0x06002DDE RID: 11742 RVA: 0x000EBF7C File Offset: 0x000EA17C
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (!ModLister.CheckBiotech("Mechlink"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			if (this.pawn.Spawned)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelMechlinkInstalled".Translate() + ": " + this.pawn.LabelShortCap, "LetterMechlinkInstalled".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000EC025 File Offset: 0x000EA225
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
			if (mechanitor == null)
			{
				return;
			}
			mechanitor.Notify_MechlinkRemoved();
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000EC042 File Offset: 0x000EA242
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.pawn.Spawned && this.pawn.IsHashIntervalTick(300, delta))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.Mechanitors, OpportunityType.Important);
			}
		}

		// Token: 0x04002429 RID: 9257
		private const int LearningOpportunityCheckInterval = 300;
	}
}
