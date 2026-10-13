using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026D1 RID: 9937
	public class RitualBehaviorWorker_Trial : RitualBehaviorWorker
	{
		// Token: 0x0600D9D5 RID: 55765 RVA: 0x0040768C File Offset: 0x0040588C
		public RitualBehaviorWorker_Trial()
		{
		}

		// Token: 0x0600D9D6 RID: 55766 RVA: 0x0040769B File Offset: 0x0040589B
		public RitualBehaviorWorker_Trial(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9D7 RID: 55767 RVA: 0x004076AC File Offset: 0x004058AC
		public override void Cleanup(LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.PawnWithRole("convict");
			if (pawn.IsPrisonerOfColony)
			{
				pawn.guest.WaitInsteadOfEscapingFor(2500);
			}
		}

		// Token: 0x0600D9D8 RID: 55768 RVA: 0x004076E0 File Offset: 0x004058E0
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("leader");
			Pawn pawn = ritual.PawnWithRole("convict");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}

		// Token: 0x0600D9D9 RID: 55769 RVA: 0x00407724 File Offset: 0x00405924
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 0)
			{
				return;
			}
			if (this.ticksSinceLastInteraction != -1 && this.ticksSinceLastInteraction <= 700)
			{
				this.ticksSinceLastInteraction++;
				return;
			}
			this.ticksSinceLastInteraction = 0;
			Pawn pawn = ritual.PawnWithRole("leader");
			Pawn pawn2 = ritual.PawnWithRole("convict");
			if (Rand.Bool)
			{
				pawn.interactions.TryInteractWith(pawn2, InteractionDefOf.Trial_Accuse);
				return;
			}
			pawn2.interactions.TryInteractWith(pawn, InteractionDefOf.Trial_Defend);
		}

		// Token: 0x0600D9DA RID: 55770 RVA: 0x004077B1 File Offset: 0x004059B1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastInteraction, "ticksSinceLastInteraction", -1, false);
		}

		// Token: 0x0400938D RID: 37773
		private int ticksSinceLastInteraction = -1;

		// Token: 0x0400938E RID: 37774
		public const int SocialInteractionIntervalTicks = 700;
	}
}
