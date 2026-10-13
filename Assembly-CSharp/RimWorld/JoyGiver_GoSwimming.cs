using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014EC RID: 5356
	public class JoyGiver_GoSwimming : JoyGiver
	{
		// Token: 0x06007FD3 RID: 32723 RVA: 0x0026787E File Offset: 0x00265A7E
		public static bool HappyToSwimOutsideOnMap(Map map)
		{
			return map.mapTemperature.OutdoorTemp >= 10f && !map.Biome.inVacuum && map.gameConditionManager.AllowEnjoyableOutsideNow(map);
		}

		// Token: 0x06007FD4 RID: 32724 RVA: 0x002678B4 File Offset: 0x00265AB4
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_GoSwimming.<>c__DisplayClass2_0 CS$<>8__locals1 = new JoyGiver_GoSwimming.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			if (!ModLister.CheckOdyssey("Swimming"))
			{
				return null;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(CS$<>8__locals1.pawn, 0.05f))
			{
				return null;
			}
			Region reg;
			if (!CellFinder.TryFindClosestRegionWith(CS$<>8__locals1.pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), new Predicate<Region>(CS$<>8__locals1.<TryGiveJob>g__RegionValidator|2), 100, out reg, RegionType.Set_Passable))
			{
				return null;
			}
			IntVec3 root;
			if (!reg.TryFindRandomCellInRegionUnforbidden(CS$<>8__locals1.pawn, new Predicate<IntVec3>(CS$<>8__locals1.<TryGiveJob>g__CellValidator|1), out root))
			{
				return null;
			}
			List<IntVec3> list;
			if (!SwimPathFinder.TryFindSwimPath(CS$<>8__locals1.pawn, root, out list))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, list[0]);
			job.targetQueueA = new List<LocalTargetInfo>();
			for (int i = 1; i < list.Count; i++)
			{
				job.targetQueueA.Add(list[i]);
			}
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}

		// Token: 0x06007FD6 RID: 32726 RVA: 0x002679BB File Offset: 0x00265BBB
		[CompilerGenerated]
		internal static bool <TryGiveJob>g__EnjoyableInsideNow|2_0(Room room)
		{
			return room.Temperature > 10f;
		}

		// Token: 0x0400557E RID: 21886
		private const float MinSwimmingTemperature = 10f;
	}
}
