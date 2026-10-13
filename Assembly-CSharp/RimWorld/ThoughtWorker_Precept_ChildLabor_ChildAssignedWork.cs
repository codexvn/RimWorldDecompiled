using System;

namespace RimWorld
{
	// Token: 0x0200182F RID: 6191
	public class ThoughtWorker_Precept_ChildLabor_ChildAssignedWork : ThoughtWorker_Precept_ChildLabor
	{
		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06008F27 RID: 36647 RVA: 0x002A50D7 File Offset: 0x002A32D7
		protected override TimeAssignmentDef AssignmentDef
		{
			get
			{
				return TimeAssignmentDefOf.Work;
			}
		}
	}
}
