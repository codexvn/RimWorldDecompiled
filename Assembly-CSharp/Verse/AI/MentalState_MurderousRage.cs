using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F8E RID: 3982
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x0600623A RID: 25146 RVA: 0x001F074C File Offset: 0x001EE94C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x001F0765 File Offset: 0x001EE965
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x001F0774 File Offset: 0x001EE974
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.target != null && this.target.Dead)
			{
				base.RecoverFromState();
			}
			if (this.pawn.IsHashIntervalTick(120, delta) && !this.IsTargetStillValidAndReachable())
			{
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageMurderousRageChangedTarget".Translate(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).Resolve().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick(delta);
			}
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x001F0850 File Offset: 0x001EEA50
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x001F08F0 File Offset: 0x001EEAF0
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x06006240 RID: 25152 RVA: 0x001F090C File Offset: 0x001EEB0C
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, false, TraverseMode.ByPawn);
		}

		// Token: 0x0400466A RID: 18026
		public Pawn target;

		// Token: 0x0400466B RID: 18027
		private const int NoLongerValidTargetCheckInterval = 120;
	}
}
