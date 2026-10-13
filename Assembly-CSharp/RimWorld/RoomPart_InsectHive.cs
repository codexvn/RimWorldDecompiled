using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F60 RID: 8032
	public class RoomPart_InsectHive : RoomPartWorker
	{
		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x0600B16C RID: 45420 RVA: 0x0031F50D File Offset: 0x0031D70D
		protected virtual IntRange HivesCountRange
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x0600B16D RID: 45421 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_InsectHive(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B16E RID: 45422 RVA: 0x0032B17C File Offset: 0x0032937C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 3, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			int randomInRange = this.HivesCountRange.RandomInRange;
			HiveUtility.SpawnHives(centerCell, map, randomInRange, 5f, WipeMode.VanishOrMoveAside, false, false, true, true, true, true);
		}
	}
}
