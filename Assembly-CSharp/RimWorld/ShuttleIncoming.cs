using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B49 RID: 11081
	public class ShuttleIncoming : Skyfaller, IActiveTransporter, IThingHolder
	{
		// Token: 0x170026E0 RID: 9952
		// (get) Token: 0x0600F7B7 RID: 63415 RVA: 0x00480442 File Offset: 0x0047E642
		// (set) Token: 0x0600F7B8 RID: 63416 RVA: 0x0048045A File Offset: 0x0047E65A
		public ActiveTransporterInfo Contents
		{
			get
			{
				return ((ActiveTransporter)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveTransporter)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600F7B9 RID: 63417 RVA: 0x00480564 File Offset: 0x0047E764
		protected override void SpawnThings()
		{
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.innerContainer[i];
				foreach (IntVec3 c in GenAdj.CellsOccupiedBy(base.Position, thing.Rotation, thing.def.Size))
				{
					foreach (Thing thing2 in base.Map.thingGrid.ThingsAt(c).ToList<Thing>())
					{
						if (thing2.def.IsBlueprint)
						{
							thing2.Destroy(DestroyMode.Cancel);
						}
					}
				}
				foreach (Thing thing3 in base.Map.thingGrid.ThingsAt(ThingUtility.InteractionCellWhenAt(thing.def, base.Position, thing.Rotation, base.Map)).ToList<Thing>())
				{
					if (thing3.def.IsBlueprint)
					{
						thing3.Destroy(DestroyMode.Cancel);
					}
				}
			}
			base.SpawnThings();
		}
	}
}
