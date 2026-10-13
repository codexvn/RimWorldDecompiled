using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200149F RID: 5279
	public class JobGiver_GetNeuralSupercharge : ThinkNode_JobGiver
	{
		// Token: 0x06007EC7 RID: 32455 RVA: 0x00262474 File Offset: 0x00260674
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0f;
			}
			int lastReceivedNeuralSuperchargeTick = pawn.health.lastReceivedNeuralSuperchargeTick;
			if (lastReceivedNeuralSuperchargeTick != -1 && Find.TickManager.TicksGame - lastReceivedNeuralSuperchargeTick < 30000)
			{
				return 0f;
			}
			if (this.ClosestSupercharger(pawn) == null)
			{
				return 0f;
			}
			return 9.25f;
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x002624CC File Offset: 0x002606CC
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.ClosestSupercharger(pawn);
			if (thing == null || !pawn.CanReserve(thing, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GetNeuralSupercharge, thing);
		}

		// Token: 0x06007EC9 RID: 32457 RVA: 0x00262508 File Offset: 0x00260708
		private Thing ClosestSupercharger(Pawn pawn)
		{
			JobGiver_GetNeuralSupercharge.<>c__DisplayClass2_0 CS$<>8__locals1 = new JobGiver_GetNeuralSupercharge.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			return GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForDef(ThingDefOf.NeuralSupercharger), PathEndMode.InteractionCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<ClosestSupercharger>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}
	}
}
