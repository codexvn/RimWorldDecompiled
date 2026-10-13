using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A1E RID: 14878
	public abstract class QuestPart_AddShipJob_Wait : QuestPart_AddShipJob
	{
		// Token: 0x06014612 RID: 83474 RVA: 0x0061714C File Offset: 0x0061534C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.showGizmos, "showGizmos", false, false);
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawnedDownedOrDead", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.sendAwayIfAllDespawned;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
				List<Thing> list2 = this.sendAwayIfAnyDespawnedDownedOrDead;
				if (list2 == null)
				{
					return;
				}
				list2.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400E116 RID: 57622
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x0400E117 RID: 57623
		public bool showGizmos = true;

		// Token: 0x0400E118 RID: 57624
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x0400E119 RID: 57625
		public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;
	}
}
