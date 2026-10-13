using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A17 RID: 14871
	public class QuestNode_AddShipJob_FlyAway : QuestNode_AddShipJob
	{
		// Token: 0x17003347 RID: 13127
		// (get) Token: 0x060145F3 RID: 83443 RVA: 0x00616B55 File Offset: 0x00614D55
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.FlyAway;
			}
		}

		// Token: 0x060145F4 RID: 83444 RVA: 0x00616B5C File Offset: 0x00614D5C
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_FlyAway shipJob_FlyAway = shipJob as ShipJob_FlyAway;
			if (shipJob_FlyAway != null)
			{
				MapParent value = this.destination.GetValue(slate);
				if (value != null)
				{
					shipJob_FlyAway.destinationTile = value.Tile;
				}
				shipJob_FlyAway.dropMode = (this.unsatisfiedDropMode.GetValue(slate) ?? TransportShipDropMode.NonRequired);
			}
		}

		// Token: 0x0400E100 RID: 57600
		public SlateRef<MapParent> destination;

		// Token: 0x0400E101 RID: 57601
		public SlateRef<TransportShipDropMode?> unsatisfiedDropMode;
	}
}
