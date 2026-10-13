using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F83 RID: 3971
	public class MentalState_InhumanRambling : MentalState_WanderSad
	{
		// Token: 0x06006210 RID: 25104 RVA: 0x001EFB6D File Offset: 0x001EDD6D
		public override void PostStart(string reason)
		{
			this.ticksToRamble = this.def.ticksBetweenRamblingRange.RandomInRange;
			base.PostStart(reason);
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x001EFB8C File Offset: 0x001EDD8C
		public override void MentalStateTick(int delta)
		{
			if (this.pawn.Awake() && !this.pawn.Suspended && this.pawn.Spawned)
			{
				this.ticksToRamble--;
				if (this.ticksToRamble <= 0)
				{
					this.ticksToRamble = this.def.ticksBetweenRamblingRange.RandomInRange;
					MentalState_InhumanRambling.DoInhumanRambling(this.pawn);
				}
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x06006212 RID: 25106 RVA: 0x001EFC00 File Offset: 0x001EDE00
		public static void DoInhumanRambling(Pawn pawn)
		{
			SocialInteractionUtility.ImitateInteractionWithNoPawn(pawn, InteractionDefOf.InhumanRambling);
			GenClamor.DoClamor(pawn, 9.9f, delegate(Thing source, Pawn hearer)
			{
				if (hearer != source && hearer.needs.mood != null)
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HeardInhumanRambling, pawn, null);
				}
			});
		}

		// Token: 0x06006213 RID: 25107 RVA: 0x001EFC46 File Offset: 0x001EDE46
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToRamble, "ticksToRamble", 0, false);
		}

		// Token: 0x04004652 RID: 18002
		private int ticksToRamble;

		// Token: 0x04004653 RID: 18003
		private const float RambleEffectRadius = 9.9f;
	}
}
