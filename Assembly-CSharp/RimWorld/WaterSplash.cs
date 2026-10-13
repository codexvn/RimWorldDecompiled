using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B43 RID: 11075
	public class WaterSplash : Projectile
	{
		// Token: 0x0600F79F RID: 63391 RVA: 0x0047FE04 File Offset: 0x0047E004
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			base.Impact(hitThing, blockedByShield);
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(base.Position))
			{
				if (thing.def == ThingDefOf.Fire)
				{
					list.Add(thing);
				}
			}
			foreach (Thing thing2 in list)
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
