using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022C2 RID: 8898
	public class LifeStageWorker
	{
		// Token: 0x0600C195 RID: 49557 RVA: 0x00380DFC File Offset: 0x0037EFFC
		public virtual void Notify_LifeStageStarted(Pawn pawn, LifeStageDef previousLifeStage)
		{
			pawn.health.capacities.Notify_CapacityLevelsDirty();
		}

		// Token: 0x04008444 RID: 33860
		public LifeStageDef def;
	}
}
