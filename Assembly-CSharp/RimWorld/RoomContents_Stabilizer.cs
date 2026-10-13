using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1D RID: 7965
	public class RoomContents_Stabilizer : RoomContentsWorker
	{
		// Token: 0x0600B01E RID: 45086 RVA: 0x00322E80 File Offset: 0x00321080
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			CellRect cellRect = CellRect.CenteredOn(room.Boundary.CenterCell, 5, 5);
			foreach (IntVec3 c in cellRect)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
			}
			foreach (IntVec3 loc in cellRect.Corners)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MechPylon, null), loc, map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.CerebrexStabilizer, null), cellRect.CenterCell, map, WipeMode.Vanish);
		}
	}
}
