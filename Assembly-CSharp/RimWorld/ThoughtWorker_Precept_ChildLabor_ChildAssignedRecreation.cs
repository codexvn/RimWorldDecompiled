using System;

namespace RimWorld
{
	// Token: 0x0200182E RID: 6190
	public class ThoughtWorker_Precept_ChildLabor_ChildAssignedRecreation : ThoughtWorker_Precept_ChildLabor
	{
		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06008F25 RID: 36645 RVA: 0x002A50C8 File Offset: 0x002A32C8
		protected override TimeAssignmentDef AssignmentDef
		{
			get
			{
				return TimeAssignmentDefOf.Joy;
			}
		}
	}
}
