using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002161 RID: 8545
	public class RoomRoleWorker_ContainmentCell : RoomRoleWorker
	{
		// Token: 0x0600BAB2 RID: 47794 RVA: 0x0035E62C File Offset: 0x0035C82C
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (ThingRequestGroup.EntityHolder.Includes(thing.def))
				{
					num += 100f;
				}
				else if (thing.def == ThingDefOf.Electroharvester || thing.def == ThingDefOf.ElectricInhibitor || thing.def == ThingDefOf.BioferriteHarvester)
				{
					num += 50f;
				}
			}
			return num;
		}

		// Token: 0x0600BAB3 RID: 47795 RVA: 0x0035E6A8 File Offset: 0x0035C8A8
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (ThingRequestGroup.EntityHolder.Includes(buildingDef))
			{
				return 100f;
			}
			if (buildingDef == ThingDefOf.Electroharvester || buildingDef == ThingDefOf.ElectricInhibitor || buildingDef == ThingDefOf.BioferriteHarvester)
			{
				return 50f;
			}
			return 0f;
		}
	}
}
