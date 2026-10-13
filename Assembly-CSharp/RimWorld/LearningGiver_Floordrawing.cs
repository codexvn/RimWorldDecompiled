using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001518 RID: 5400
	public class LearningGiver_Floordrawing : LearningGiver
	{
		// Token: 0x060080A6 RID: 32934 RVA: 0x0026C324 File Offset: 0x0026A524
		public static bool TryFindFloordrawingSpots(Pawn pawn, out IntVec3 drawFrom, out IntVec3 drawOn)
		{
			LearningGiver_Floordrawing.<>c__DisplayClass2_0 CS$<>8__locals1 = new LearningGiver_Floordrawing.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.traverseParams = TraverseParms.For(CS$<>8__locals1.pawn, Danger.None, TraverseMode.ByPawn, false, false, false, true);
			CS$<>8__locals1.innerDrawFrom = IntVec3.Invalid;
			CS$<>8__locals1.innerDrawOn = IntVec3.Invalid;
			CS$<>8__locals1.<TryFindFloordrawingSpots>g__FindDrawCell|0(false);
			if (CS$<>8__locals1.innerDrawFrom == IntVec3.Invalid)
			{
				CS$<>8__locals1.<TryFindFloordrawingSpots>g__FindDrawCell|0(true);
			}
			drawFrom = CS$<>8__locals1.innerDrawFrom;
			drawOn = CS$<>8__locals1.innerDrawOn;
			return drawFrom != IntVec3.Invalid && drawOn != IntVec3.Invalid;
		}

		// Token: 0x060080A7 RID: 32935 RVA: 0x0026C3CC File Offset: 0x0026A5CC
		public static bool CanFloorDrawFrom(IntVec3 spot, Pawn drawer)
		{
			Map map = drawer.Map;
			return spot.InBounds(map) && spot.Standable(map) && !spot.IsForbidden(drawer) && map.areaManager.Home[spot] && spot.GetDoor(map) == null && drawer.CanReserve(spot, 1, -1, null, false);
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x0026C429 File Offset: 0x0026A629
		private static bool CanDrawOn(IntVec3 cell, Map map, bool desperate)
		{
			return FilthMaker.CanMakeFilth(cell, map, ThingDefOf.Filth_Floordrawing, FilthSourceFlags.Pawn) && (desperate || (cell.GetFirstItem(map) == null && cell.GetFirstBuilding(map) == null && cell.GetFirstThing(map) == null));
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x0026C460 File Offset: 0x0026A660
		public static bool TryFloorDrawCellPair(IntVec3 companionCell, bool canDrawOnRoot, Pawn drawer, bool desperate, out bool shouldDrawOnRoot)
		{
			shouldDrawOnRoot = false;
			if (!LearningGiver_Floordrawing.CanFloorDrawFrom(companionCell, drawer))
			{
				return false;
			}
			bool flag = LearningGiver_Floordrawing.CanDrawOn(companionCell, drawer.Map, desperate);
			if (!canDrawOnRoot && !flag)
			{
				return false;
			}
			if (!canDrawOnRoot)
			{
				return true;
			}
			if (!flag)
			{
				shouldDrawOnRoot = true;
				return true;
			}
			shouldDrawOnRoot = Rand.Bool;
			return true;
		}

		// Token: 0x060080AA RID: 32938 RVA: 0x0026C4AC File Offset: 0x0026A6AC
		public override bool CanDo(Pawn pawn)
		{
			IntVec3 intVec;
			IntVec3 intVec2;
			return base.CanDo(pawn) && LearningGiver_Floordrawing.TryFindFloordrawingSpots(pawn, out intVec, out intVec2);
		}

		// Token: 0x060080AB RID: 32939 RVA: 0x0026C4D0 File Offset: 0x0026A6D0
		public override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			IntVec3 c2;
			if (!LearningGiver_Floordrawing.TryFindFloordrawingSpots(pawn, out c, out c2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c, c2);
		}

		// Token: 0x040055FE RID: 22014
		private static readonly IntVec3[] offsets = new IntVec3[]
		{
			IntVec3.East,
			IntVec3.South
		};
	}
}
