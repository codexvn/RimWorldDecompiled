using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DEE RID: 15854
	public static class DistressCallUtility
	{
		// Token: 0x06015A1F RID: 88607 RVA: 0x00679A7C File Offset: 0x00677C7C
		public static void SpawnPawns(Map map, IEnumerable<Pawn> pawns, IntVec3 root, int radius)
		{
			Predicate<IntVec3> <>9__0;
			foreach (Pawn newThing in pawns)
			{
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 loc;
				if (!RCellFinder.TryFindRandomCellNearWith(root, validator, map, out loc, radius, 2147483647))
				{
					break;
				}
				GenSpawn.Spawn(newThing, loc, map, WipeMode.Vanish);
			}
		}

		// Token: 0x06015A20 RID: 88608 RVA: 0x00679B14 File Offset: 0x00677D14
		public static void SpawnCorpses(Map map, IEnumerable<Pawn> pawns, IEnumerable<Pawn> killers, IntVec3 root, int radius)
		{
			int num = Find.TickManager.TicksGame - map.Parent.creationGameTicks;
			Predicate<IntVec3> <>9__0;
			foreach (Pawn pawn in pawns)
			{
				HealthUtility.SimulateKilledByPawn(pawn, killers.RandomElement<Pawn>());
				Corpse corpse = pawn.Corpse;
				if (corpse != null)
				{
					corpse.timeOfDeath = map.Parent.creationGameTicks;
					CompRottable compRottable = corpse.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += (float)num;
					}
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => c.Standable(map) && c.GetEdifice(map) == null));
					}
					IntVec3 intVec;
					if (RCellFinder.TryFindRandomCellNearWith(root, validator, map, out intVec, radius, 2147483647))
					{
						if ((corpse.InnerPawn.kindDef.IsFleshBeast() && compRottable.Stage == RotStage.Dessicated) || corpse.InnerPawn.kindDef == PawnKindDefOf.Fingerspike)
						{
							FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
							break;
						}
						GenSpawn.Spawn(corpse, intVec, map, WipeMode.Vanish);
						corpse.SetForbidden(true, true);
						pawn.DropAndForbidEverything(false, false);
						if (num < 300000)
						{
							int randomInRange = DistressCallUtility.BloodFilthToSpawn.RandomInRange;
							for (int i = 0; i < randomInRange; i++)
							{
								IntVec3 intVec2 = CellFinder.RandomClosewalkCellNear(intVec, map, 3, null);
								if (intVec2.InBounds(map) && GenSight.LineOfSight(intVec2, intVec, map))
								{
									FilthMaker.TryMakeFilth(intVec2, map, pawn.RaceProps.BloodDef, pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400F064 RID: 61540
		private static readonly IntRange BloodFilthToSpawn = new IntRange(1, 5);
	}
}
