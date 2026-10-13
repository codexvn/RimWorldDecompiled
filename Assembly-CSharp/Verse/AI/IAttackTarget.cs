using System;

namespace Verse.AI
{
	// Token: 0x02001050 RID: 4176
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060065CE RID: 26062
		Thing Thing { get; }

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060065CF RID: 26063
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060065D0 RID: 26064
		float TargetPriorityFactor { get; }

		// Token: 0x060065D1 RID: 26065
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
