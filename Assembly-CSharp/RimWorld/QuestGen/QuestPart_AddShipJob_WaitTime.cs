using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A20 RID: 14880
	public class QuestPart_AddShipJob_WaitTime : QuestPart_AddShipJob_Wait
	{
		// Token: 0x06014618 RID: 83480 RVA: 0x00617234 File Offset: 0x00615434
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitTime shipJob_WaitTime = (ShipJob_WaitTime)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitTime.duration = this.duration;
			shipJob_WaitTime.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitTime.showGizmos = this.showGizmos;
			shipJob_WaitTime.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitTime.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitTime;
		}

		// Token: 0x06014619 RID: 83481 RVA: 0x0061728D File Offset: 0x0061548D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x0400E11D RID: 57629
		public int duration;
	}
}
