using System;
using System.Collections.Generic;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026DA RID: 9946
	public abstract class RitualCancellationTrigger
	{
		// Token: 0x0600DA00 RID: 55808
		public abstract IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments);
	}
}
