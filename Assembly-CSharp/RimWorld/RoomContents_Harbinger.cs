using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0B RID: 7947
	public class RoomContents_Harbinger : RoomContentsWorker
	{
		// Token: 0x0600AFE9 RID: 45033 RVA: 0x00322298 File Offset: 0x00320498
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 intVec;
			if (!room.TryGetRandomCellInRoom(map, out intVec, 2, 0, null, false))
			{
				return;
			}
			((Plant)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null), intVec, map, WipeMode.Vanish)).Growth = RoomContents_Harbinger.TreeGrowthRange.RandomInRange;
			foreach (IntVec3 c in GridShapeMaker.IrregularLump(intVec, map, RoomContents_Harbinger.DirtAmount.RandomInRange, (IntVec3 x) => room.Contains(x, 1)))
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.Soil);
			}
		}

		// Token: 0x04007892 RID: 30866
		private static readonly IntRange DirtAmount = new IntRange(10, 14);

		// Token: 0x04007893 RID: 30867
		private static readonly FloatRange TreeGrowthRange = new FloatRange(0.5f, 0.9f);
	}
}
