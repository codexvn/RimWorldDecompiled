using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002055 RID: 8277
	public class GenStep_ChopTrees : GenStep
	{
		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x0600B5CE RID: 46542 RVA: 0x00343B09 File Offset: 0x00341D09
		public override int SeedPart
		{
			get
			{
				return 874575948;
			}
		}

		// Token: 0x0600B5CF RID: 46543 RVA: 0x00343B10 File Offset: 0x00341D10
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect cellRect;
			IntVec3 centerCell;
			if (MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				centerCell = cellRect.CenterCell;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				foreach (Thing thing in list)
				{
					num += thing.Position.x;
					num2 += thing.Position.z;
				}
				centerCell = new IntVec3(num / list.Count, 0, num2 / list.Count);
			}
			foreach (IntVec3 c in GenRadial.RadialCellsAround(centerCell, (float)this.radialRange, true))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Plant plant = thingList[i] as Plant;
						if (plant != null && plant.def.plant.IsTree)
						{
							plant.TrySpawnStump(PlantDestructionMode.Chop);
							plant.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x04007CBA RID: 31930
		public int radialRange = 35;
	}
}
