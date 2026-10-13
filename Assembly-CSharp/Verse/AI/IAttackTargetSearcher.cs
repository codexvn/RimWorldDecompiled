using System;

namespace Verse.AI
{
	// Token: 0x02001033 RID: 4147
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06006538 RID: 25912
		Thing Thing { get; }

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06006539 RID: 25913
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x0600653A RID: 25914
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x0600653B RID: 25915
		int LastAttackTargetTick { get; }
	}
}
