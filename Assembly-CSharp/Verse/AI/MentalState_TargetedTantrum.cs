using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F98 RID: 3992
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x0600626F RID: 25199 RVA: 0x001F0EB0 File Offset: 0x001EF0B0
		public override void MentalStateTick(int delta)
		{
			if (this.target == null || this.target.Destroyed)
			{
				base.RecoverFromState();
				return;
			}
			if (this.target.Spawned && this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				base.MentalStateTick(delta);
				return;
			}
			Thing target = this.target;
			if (!this.TryFindNewTarget())
			{
				base.RecoverFromState();
				return;
			}
			Messages.Message("MessageTargetedTantrumChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			base.MentalStateTick(delta);
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x001F0FBA File Offset: 0x001EF1BA
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x001F0FCC File Offset: 0x001EF1CC
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x001F1038 File Offset: 0x001EF238
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04004679 RID: 18041
		public const int MinMarketValue = 300;

		// Token: 0x0400467A RID: 18042
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
