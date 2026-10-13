using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F15 RID: 7957
	public class RoomContents_Trees : RoomContentsWorker
	{
		// Token: 0x0600B004 RID: 45060 RVA: 0x00322854 File Offset: 0x00320A54
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			if (RoomContents_Trees.treeKinds.Empty<ThingDef>())
			{
				RoomContents_Trees.treeKinds.AddRange(new ThingDef[]
				{
					ThingDefOf.Plant_TreeOak,
					ThingDefOf.Plant_TreePoplar,
					ThingDefOf.Plant_TreeBirch,
					ThingDefOf.Plant_TreePine
				});
			}
			ThingDef thingDef = RoomContents_Trees.treeKinds.RandomElement<ThingDef>();
			IntVec3 center;
			if (!room.TryGetRandomCellInRoom(map, out center, 2, 0, null, false))
			{
				return;
			}
			int randomInRange = RoomContents_Trees.DirtAmountRange.RandomInRange;
			int randomInRange2 = RoomContents_Trees.TressRange.RandomInRange;
			int num = 0;
			List<IntVec3> list = GridShapeMaker.IrregularLump(center, map, randomInRange, (IntVec3 x) => room.Contains(x, 1));
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec = list[i];
				map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Soil);
				if (Rand.DynamicChance(num, randomInRange2, list.Count - i))
				{
					num++;
					Plant plant = (Plant)GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
					plant.Growth = RoomContents_Trees.TreeGrowthRange.RandomInRange;
					if (!thingDef.plant.dieIfLeafless)
					{
						plant.MakeLeafless(Plant.LeaflessCause.Cold, true);
					}
				}
			}
		}

		// Token: 0x0400789E RID: 30878
		private static readonly IntRange DirtAmountRange = new IntRange(18, 24);

		// Token: 0x0400789F RID: 30879
		private static readonly IntRange TressRange = new IntRange(2, 4);

		// Token: 0x040078A0 RID: 30880
		private static readonly FloatRange TreeGrowthRange = new FloatRange(0.7f, 1f);

		// Token: 0x040078A1 RID: 30881
		private static readonly List<ThingDef> treeKinds = new List<ThingDef>();
	}
}
