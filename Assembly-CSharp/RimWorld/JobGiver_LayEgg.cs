using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013D0 RID: 5072
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		// Token: 0x06007C21 RID: 31777 RVA: 0x00256170 File Offset: 0x00254370
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				return null;
			}
			ThingDef singleDef = compEggLayer.NextEggType();
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true);
			if (pawn.Faction == Faction.OfPlayer)
			{
				Thing bestEggBox = this.GetBestEggBox(pawn, peMode, traverseParms);
				if (bestEggBox != null)
				{
					return JobMaker.MakeJob(JobDefOf.LayEgg, bestEggBox);
				}
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(singleDef), peMode, traverseParms, 30f, (Thing x) => pawn.GetRoom(RegionType.Set_All) == null || x.GetRoom(RegionType.Set_All) == pawn.GetRoom(RegionType.Set_All), null, 0, -1, false, RegionType.Set_Passable, false, false);
			return JobMaker.MakeJob(JobDefOf.LayEgg, (thing != null) ? thing.Position : RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some, false));
		}

		// Token: 0x06007C22 RID: 31778 RVA: 0x00256274 File Offset: 0x00254474
		private Thing GetBestEggBox(Pawn pawn, PathEndMode peMode, TraverseParms tp)
		{
			JobGiver_LayEgg.<>c__DisplayClass4_0 CS$<>8__locals1 = new JobGiver_LayEgg.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.eggDef = CS$<>8__locals1.pawn.TryGetComp<CompEggLayer>().NextEggType();
			return GenClosest.ClosestThing_Regionwise_ReachablePrioritized(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForDef(ThingDefOf.EggBox), peMode, tp, 30f, new Predicate<Thing>(CS$<>8__locals1.<GetBestEggBox>g__IsUsableBox|0), new Func<Thing, float>(JobGiver_LayEgg.<GetBestEggBox>g__GetScore|4_1), 10, 30, false);
		}

		// Token: 0x06007C24 RID: 31780 RVA: 0x002562F0 File Offset: 0x002544F0
		[CompilerGenerated]
		internal static float <GetBestEggBox>g__GetScore|4_1(Thing thing)
		{
			CompEggContainer compEggContainer = thing.TryGetComp<CompEggContainer>();
			if (compEggContainer == null || compEggContainer.Full)
			{
				return 0f;
			}
			Thing containedThing = compEggContainer.ContainedThing;
			int? num = (containedThing != null) ? new int?(containedThing.stackCount) : null;
			float? num2 = (num != null) ? new float?((float)num.GetValueOrDefault() * 5f) : null;
			if (num2 == null)
			{
				return 0.5f;
			}
			return num2.GetValueOrDefault();
		}

		// Token: 0x040053C9 RID: 21449
		private const float LayRadius = 5f;

		// Token: 0x040053CA RID: 21450
		private const float MaxSearchRadius = 30f;

		// Token: 0x040053CB RID: 21451
		private const int MinSearchRegions = 10;
	}
}
