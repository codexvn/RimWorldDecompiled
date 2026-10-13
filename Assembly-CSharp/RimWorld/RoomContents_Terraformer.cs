using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F26 RID: 7974
	public class RoomContents_Terraformer : RoomContentsWorker
	{
		// Token: 0x0600B037 RID: 45111 RVA: 0x003235A4 File Offset: 0x003217A4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomGenUtility.TryPlaceInRandomCorner(map, room, ThingDefOf.HunterDroneTrap, faction, null);
			if (Rand.Bool)
			{
				RoomGenUtility.TryPlaceInRandomCorner(map, room, ThingDefOf.HunterDroneTrap, faction, null);
			}
			CellRect largest = (from r in room.rects
			where r.Width >= 6 && r.Height >= 6
			orderby r.Area descending
			select r).FirstOrDefault<CellRect>();
			this.SpawnTerraformer(map, largest);
			this.RemoveRoof(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600B038 RID: 45112 RVA: 0x00323643 File Offset: 0x00321843
		private void SpawnTerraformer(Map map, CellRect largest)
		{
			GenSpawn.Spawn(ThingDefOf.AncientTerraformer, largest.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B039 RID: 45113 RVA: 0x0032365C File Offset: 0x0032185C
		private void RemoveRoof(Map map, LayoutRoom room)
		{
			foreach (IntVec3 c in room.Cells)
			{
				map.roofGrid.SetRoof(c, null);
			}
		}
	}
}
