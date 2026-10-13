using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E2 RID: 6626
	public class AbilityGroupDef : Def
	{
		// Token: 0x04006176 RID: 24950
		public int cooldownTicks;

		// Token: 0x04006177 RID: 24951
		public bool sendMessageOnCooldownComplete;

		// Token: 0x04006178 RID: 24952
		public List<string> ritualRoleIds;
	}
}
