using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F7F RID: 3967
	public class MentalState_Roaming : MentalState
	{
		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x060061FB RID: 25083 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x001EF7BC File Offset: 0x001ED9BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.exitDest, "exitDest", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.waitAtEdgeUntilTick, "waitAtEdgeUntilTick", 0, false);
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x001EF7FC File Offset: 0x001ED9FC
		public override void PreStart()
		{
			base.PreStart();
			Pawn_CallTracker caller = this.pawn.caller;
			if (caller != null)
			{
				caller.DoCall(false);
			}
			Messages.Message("MessageRoamerLeaving".Translate(this.pawn.Named("PAWN")).CapitalizeFirst(), this.pawn, MessageTypeDefOf.ThreatSmall, true);
		}

		// Token: 0x060061FE RID: 25086 RVA: 0x001EF864 File Offset: 0x001EDA64
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (!this.exitDest.IsValid || !this.pawn.CanReach(this.exitDest, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				this.waitAtEdgeUntilTick = -1;
				this.exitDest = IntVec3.Invalid;
				if (!RCellFinder.TryFindRandomExitSpot(this.pawn, out this.exitDest, TraverseMode.ByPawn))
				{
					base.RecoverFromState();
					return;
				}
			}
			if (this.pawn.roping.IsRoped)
			{
				base.RecoverFromState();
			}
			if (this.waitAtEdgeUntilTick < 0 && this.pawn.Position.InHorDistOf(this.exitDest, 12f))
			{
				this.waitAtEdgeUntilTick = Find.TickManager.TicksGame + MentalState_Roaming.WaitAtEdgeBeforeExitingTicks.RandomInRange;
			}
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x001EF93C File Offset: 0x001EDB3C
		public override void PostEnd()
		{
			base.PostEnd();
			this.pawn.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x001EF963 File Offset: 0x001EDB63
		public bool ShouldExitMapNow()
		{
			return (this.waitAtEdgeUntilTick > 0 && Find.TickManager.TicksGame > this.waitAtEdgeUntilTick) || base.Age > 60000;
		}

		// Token: 0x0400464A RID: 17994
		public const int WanderDistance = 12;

		// Token: 0x0400464B RID: 17995
		private const int MaxTicksToRoamBeforeExit = 60000;

		// Token: 0x0400464C RID: 17996
		private static readonly IntRange WaitAtEdgeBeforeExitingTicks = new IntRange(7000, 8000);

		// Token: 0x0400464D RID: 17997
		public IntVec3 exitDest = IntVec3.Invalid;

		// Token: 0x0400464E RID: 17998
		public int waitAtEdgeUntilTick = -1;
	}
}
