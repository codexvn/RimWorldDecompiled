using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022D4 RID: 8916
	public class CreepJoinerWorker_MetalhorrorDownside : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1F5 RID: 49653 RVA: 0x0038309F File Offset: 0x0038129F
		public override void OnCreated()
		{
			MetalhorrorUtility.Infect(base.Pawn, null, "InsectImplant", null);
		}

		// Token: 0x0600C1F6 RID: 49654 RVA: 0x000026EA File Offset: 0x000008EA
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
		}
	}
}
