using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014E5 RID: 5349
	public class JoyGiver_BuildSnowman : JoyGiver
	{
		// Token: 0x06007FC0 RID: 32704 RVA: 0x00267100 File Offset: 0x00265300
		public override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction))
			{
				return null;
			}
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null))
			{
				return null;
			}
			if (pawn.Map.snowGrid.TotalDepth < 200f)
			{
				return null;
			}
			IntVec3 c = JoyGiver_BuildSnowman.TryFindSnowmanBuildCell(pawn);
			if (!c.IsValid)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x00267168 File Offset: 0x00265368
		private static IntVec3 TryFindSnowmanBuildCell(Pawn pawn)
		{
			Region rootReg;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (Region r) => r.Room.PsychologicallyOutdoors, 100, out rootReg, RegionType.Set_Passable))
			{
				return IntVec3.Invalid;
			}
			IntVec3 result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region region) => region.District == rootReg.District, delegate(Region region)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 randomCell = region.RandomCell;
					if (JoyGiver_BuildSnowman.IsGoodSnowmanCell(randomCell, pawn))
					{
						result = randomCell;
						return true;
					}
				}
				return false;
			}, 30, RegionType.Set_Passable);
			return result;
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x00267210 File Offset: 0x00265410
		private static bool IsGoodSnowmanCell(IntVec3 c, Pawn pawn)
		{
			if (pawn.Map.snowGrid.GetDepth(c) < 0.5f)
			{
				return false;
			}
			if (c.Fogged(pawn.Map))
			{
				return false;
			}
			if (c.GetEdifice(pawn.Map) != null)
			{
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (!c2.InBounds(pawn.Map))
				{
					return false;
				}
				if (!c2.Standable(pawn.Map))
				{
					return false;
				}
				if (pawn.Map.reservationManager.IsReservedAndRespected(c2, pawn))
				{
					return false;
				}
			}
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Snowman);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Position.InHorDistOf(c, 12f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400556E RID: 21870
		private const float MinSnowmanDepth = 0.5f;

		// Token: 0x0400556F RID: 21871
		private const float MinDistBetweenSnowmen = 12f;

		// Token: 0x04005570 RID: 21872
		private const float MinSnowDepth = 200f;
	}
}
