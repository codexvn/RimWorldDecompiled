using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C58 RID: 11352
	public class CompDestroyNearbyPlantsOnSpawn : ThingComp
	{
		// Token: 0x17002818 RID: 10264
		// (get) Token: 0x0600FE8F RID: 65167 RVA: 0x0049C979 File Offset: 0x0049AB79
		public CompProperties_DestroyNearbyPlantsOnSpawn Props
		{
			get
			{
				return (CompProperties_DestroyNearbyPlantsOnSpawn)this.props;
			}
		}

		// Token: 0x0600FE90 RID: 65168 RVA: 0x0049C988 File Offset: 0x0049AB88
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			foreach (IntVec3 c in GenRadial.RadialCellsAround(this.parent.Position, (float)this.Props.radius, true))
			{
				if (c.InBounds(this.parent.Map))
				{
					List<Thing> thingList = c.GetThingList(this.parent.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing is Plant && !thing.def.preventSkyfallersLandingOn)
						{
							thing.Kill(null, null);
						}
					}
				}
			}
		}
	}
}
