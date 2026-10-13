using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A15 RID: 14869
	public class QuestNode_AddShipJob_Wait : QuestNode_AddShipJob
	{
		// Token: 0x060145EE RID: 83438 RVA: 0x00616AB8 File Offset: 0x00614CB8
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Wait shipJob_Wait = shipJob as ShipJob_Wait;
			if (shipJob_Wait != null)
			{
				shipJob_Wait.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied.GetValue(slate);
				shipJob_Wait.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned.GetValue(slate);
			}
			ShipJob_WaitTime shipJob_WaitTime = shipJob as ShipJob_WaitTime;
			if (shipJob_WaitTime != null)
			{
				shipJob_WaitTime.duration = this.ticks.GetValue(slate);
			}
		}

		// Token: 0x0400E0FC RID: 57596
		public SlateRef<int> ticks;

		// Token: 0x0400E0FD RID: 57597
		public SlateRef<bool> leaveImmediatelyWhenSatisfied;

		// Token: 0x0400E0FE RID: 57598
		public SlateRef<List<Thing>> sendAwayIfAllDespawned;
	}
}
