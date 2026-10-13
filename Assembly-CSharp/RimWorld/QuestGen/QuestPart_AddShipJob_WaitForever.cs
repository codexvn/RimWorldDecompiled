using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A21 RID: 14881
	public class QuestPart_AddShipJob_WaitForever : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0601461B RID: 83483 RVA: 0x006172B0 File Offset: 0x006154B0
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitForever shipJob_WaitForever = (ShipJob_WaitForever)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitForever.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitForever.showGizmos = this.showGizmos;
			shipJob_WaitForever.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitForever.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitForever;
		}
	}
}
