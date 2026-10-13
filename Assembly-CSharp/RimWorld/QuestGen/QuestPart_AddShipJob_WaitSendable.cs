using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A22 RID: 14882
	public class QuestPart_AddShipJob_WaitSendable : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0601461D RID: 83485 RVA: 0x00617300 File Offset: 0x00615500
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitSendable shipJob_WaitSendable = (ShipJob_WaitSendable)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitSendable.destination = this.destination;
			shipJob_WaitSendable.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitSendable.showGizmos = this.showGizmos;
			shipJob_WaitSendable.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitSendable.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			shipJob_WaitSendable.targetPlayerSettlement = this.targetPlayerSettlement;
			return shipJob_WaitSendable;
		}

		// Token: 0x0601461E RID: 83486 RVA: 0x00617365 File Offset: 0x00615565
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.destination, "destination", false);
			Scribe_Values.Look<bool>(ref this.targetPlayerSettlement, "targetPlayerSettlement", false, false);
		}

		// Token: 0x0400E11E RID: 57630
		public MapParent destination;

		// Token: 0x0400E11F RID: 57631
		public bool targetPlayerSettlement;
	}
}
