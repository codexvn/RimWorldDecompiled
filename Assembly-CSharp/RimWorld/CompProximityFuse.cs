using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002DC1 RID: 11713
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17002A09 RID: 10761
		// (get) Token: 0x06010793 RID: 67475 RVA: 0x004C0BC5 File Offset: 0x004BEDC5
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x06010794 RID: 67476 RVA: 0x004C0BD2 File Offset: 0x004BEDD2
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06010795 RID: 67477 RVA: 0x004C0BEC File Offset: 0x004BEDEC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Trigger",
				action = delegate()
				{
					this.parent.GetComp<CompExplosive>().StartWick(null);
				}
			};
			yield break;
		}

		// Token: 0x06010796 RID: 67478 RVA: 0x004C0BFC File Offset: 0x004BEDFC
		public override void CompTickRare()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForDef(this.Props.target), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), this.Props.radius, null, null, 0, -1, false, RegionType.Set_Passable, false, false) != null)
			{
				this.parent.GetComp<CompExplosive>().StartWick(null);
			}
		}
	}
}
