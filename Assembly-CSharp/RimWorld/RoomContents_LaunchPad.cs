using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EE2 RID: 7906
	public class RoomContents_LaunchPad : RoomContentsWorker
	{
		// Token: 0x0600AF20 RID: 44832 RVA: 0x0031F74C File Offset: 0x0031D94C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			CellRect cellRect = room.rects[0];
			CellRect cellRect2 = cellRect.ContractedBy(2);
			for (int i = cellRect2.Width - 1; i >= 0; i--)
			{
				for (int j = cellRect2.Height - 1; j >= 0; j--)
				{
					IntVec3 intVec = new IntVec3(i, 0, j);
					IntVec3 intVec2 = cellRect2.Min + intVec;
					map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.AncientTile);
					if (intVec.z % 2 == 0 && intVec.x % 2 == 0 && Rand.Chance(0.3f))
					{
						GenSpawn.Spawn(ThingDefOf.MechanoidDropPod, intVec2, map, WipeMode.Vanish);
					}
				}
			}
			foreach (IntVec3 loc in cellRect.ContractedBy(1).Corners)
			{
				GenSpawn.Spawn(ThingDefOf.AncientShipBeacon, loc, map, WipeMode.Vanish);
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0400784A RID: 30794
		private const float PodSpawnChance = 0.3f;
	}
}
