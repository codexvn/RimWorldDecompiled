using System;

namespace Verse.AI
{
	// Token: 0x02000EDE RID: 3806
	public interface IJobEndable
	{
		// Token: 0x06005FAF RID: 24495
		Pawn GetActor();

		// Token: 0x06005FB0 RID: 24496
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
