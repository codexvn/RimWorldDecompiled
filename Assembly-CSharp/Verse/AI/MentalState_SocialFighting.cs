using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F7E RID: 3966
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x060061F4 RID: 25076 RVA: 0x001EF5B1 File Offset: 0x001ED7B1
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x060061F5 RID: 25077 RVA: 0x001EF5E8 File Offset: 0x001ED7E8
		private bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				if (!this.otherPawn.InMentalState)
				{
					return false;
				}
				MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
				return mentalState_SocialFighting != null && mentalState_SocialFighting.otherPawn == this.pawn;
			}
		}

		// Token: 0x060061F6 RID: 25078 RVA: 0x001EF62B File Offset: 0x001ED82B
		public override void MentalStateTick(int delta)
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
				return;
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x001EF644 File Offset: 0x001ED844
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.pawn.jobs != null)
			{
				this.pawn.jobs.StopAll(false, true);
			}
			this.pawn.mindState.meleeThreat = null;
			if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && this.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("MessageNoLongerSocialFighting".Translate(this.pawn.LabelShort, this.otherPawn.LabelShort, this.pawn.Named("PAWN1"), this.otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
			if (!this.pawn.Dead && this.pawn.needs.mood != null && !this.otherPawn.Dead)
			{
				ThoughtDef def;
				if (Rand.Value < 0.5f)
				{
					def = ThoughtDefOf.HadAngeringFight;
				}
				else
				{
					def = ThoughtDefOf.HadCatharticFight;
				}
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn, null);
			}
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x001EF7A3 File Offset: 0x001ED9A3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04004649 RID: 17993
		public Pawn otherPawn;
	}
}
