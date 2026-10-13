using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D23 RID: 11555
	public class CompLootSpawn : ThingComp
	{
		// Token: 0x1700293D RID: 10557
		// (get) Token: 0x060103BD RID: 66493 RVA: 0x004B19F5 File Offset: 0x004AFBF5
		private CompProperties_LootSpawn Props
		{
			get
			{
				return (CompProperties_LootSpawn)this.props;
			}
		}

		// Token: 0x060103BE RID: 66494 RVA: 0x004B1A04 File Offset: 0x004AFC04
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			Building_Crate building_Crate = (Building_Crate)this.parent;
			if (this.Props.contents == null)
			{
				building_Crate.Open();
				return;
			}
			List<Thing> list = this.Props.contents.root.Generate(default(ThingSetMakerParams));
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Thing thing = list[i];
				if (!building_Crate.TryAcceptThing(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}
	}
}
