using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200021F RID: 543
	public interface ICreepJoinerDef
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000E6A RID: 3690
		float Weight { get; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000E6B RID: 3691
		float MinCombatPoints { get; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000E6C RID: 3692
		bool CanOccurRandomly { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000E6D RID: 3693
		List<CreepJoinerBaseDef> Excludes { get; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000E6E RID: 3694
		List<CreepJoinerBaseDef> Requires { get; }
	}
}
