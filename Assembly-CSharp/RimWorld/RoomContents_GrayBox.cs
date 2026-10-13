using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0A RID: 7946
	public class RoomContents_GrayBox : RoomContentsWorker
	{
		// Token: 0x0600AFE5 RID: 45029 RVA: 0x0032221C File Offset: 0x0032041C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			Building_Crate building_Crate;
			RoomContents_GrayBox.TrySpawnBoxInRoom(map, room, out building_Crate);
		}

		// Token: 0x0600AFE6 RID: 45030 RVA: 0x00322240 File Offset: 0x00320440
		public static bool TrySpawnBoxInRoom(Map map, LayoutRoom room, out Building_Crate spawned)
		{
			spawned = null;
			IntVec3 cell;
			if (!room.TryGetRandomCellInRoom(ThingDefOf.GrayBox, map, out cell, null, 2, 1, null, false))
			{
				return false;
			}
			spawned = RoomContents_GrayBox.SpawnBoxInRoom(cell, map, null, true);
			return true;
		}

		// Token: 0x0600AFE7 RID: 45031 RVA: 0x0032227B File Offset: 0x0032047B
		public static Building_Crate SpawnBoxInRoom(IntVec3 cell, Map map, ThingSetMakerDef rewardMaker = null, bool addRewards = true)
		{
			return RoomGenUtility.SpawnCrate(ThingDefOf.GrayBox, cell, map, rewardMaker ?? ThingSetMakerDefOf.Reward_GrayBox, addRewards, null);
		}
	}
}
