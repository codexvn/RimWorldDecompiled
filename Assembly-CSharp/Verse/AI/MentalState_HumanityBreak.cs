using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F82 RID: 3970
	public class MentalState_HumanityBreak : MentalState_WanderOwnRoom
	{
		// Token: 0x0600620B RID: 25099 RVA: 0x001EFA40 File Offset: 0x001EDC40
		public override void PostStart(string reason)
		{
			this.forceRecoverAfterTicks = MentalState_HumanityBreak.RecoveryDurationTicksRange.RandomInRange;
			this.ticksToRamble = MentalState_HumanityBreak.RambleTicksRange.RandomInRange;
			base.PostStart(reason);
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x001EFA7C File Offset: 0x001EDC7C
		public override void MentalStateTick(int delta)
		{
			if (this.pawn.Awake() && !this.pawn.Suspended)
			{
				if (this.pawn.IsHashIntervalTick(30, delta))
				{
					this.age += 30;
					if (this.age >= this.forceRecoverAfterTicks)
					{
						base.RecoverFromState();
						return;
					}
				}
				if (this.pawn.Spawned)
				{
					this.ticksToRamble--;
					if (this.ticksToRamble <= 0)
					{
						this.ticksToRamble = MentalState_HumanityBreak.RambleTicksRange.RandomInRange;
						SocialInteractionUtility.ImitateInteractionWithNoPawn(this.pawn, InteractionDefOf.InhumanRambling);
					}
				}
			}
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x001EFB21 File Offset: 0x001EDD21
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToRamble, "ticksToRamble", 0, false);
		}

		// Token: 0x0400464F RID: 17999
		private int ticksToRamble;

		// Token: 0x04004650 RID: 18000
		private static readonly IntRange RecoveryDurationTicksRange = new IntRange(10000, 20000);

		// Token: 0x04004651 RID: 18001
		private static readonly IntRange RambleTicksRange = new IntRange(1200, 6000);
	}
}
