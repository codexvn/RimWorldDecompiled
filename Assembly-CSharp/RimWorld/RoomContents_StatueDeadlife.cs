using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F12 RID: 7954
	public class RoomContents_StatueDeadlife : RoomContents_DeadBodyLabyrinth
	{
		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x0600AFFB RID: 45051 RVA: 0x0031F50D File Offset: 0x0031D70D
		protected override IntRange CorpseRange
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x0600AFFC RID: 45052 RVA: 0x0031F51F File Offset: 0x0031D71F
		protected override IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(2, 4);
			}
		}

		// Token: 0x0600AFFD RID: 45053 RVA: 0x00322700 File Offset: 0x00320900
		protected override void SpawnCorpses(Map map, LayoutRoom room)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 4, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayStatueDeadlifeDust, null), centerCell, map, WipeMode.Vanish);
			int randomInRange = this.CorpseRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 near = centerCell;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.ValidCell(c, map)));
				}
				IntVec3 cell;
				if (RCellFinder.TryFindRandomCellNearWith(near, validator, map, out cell, 2, 6))
				{
					base.SpawnCorpse(cell, map).InnerPawn.inventory.DestroyAll(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600AFFE RID: 45054 RVA: 0x003227D4 File Offset: 0x003209D4
		private bool ValidCell(IntVec3 cell, Map map)
		{
			return cell.GetFirstThing(map) == null;
		}
	}
}
