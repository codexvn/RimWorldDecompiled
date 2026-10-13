using System;

namespace Verse
{
	// Token: 0x020006C7 RID: 1735
	public class HediffComp_GiveNeurocharge : HediffComp
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x000E1F8F File Offset: 0x000E018F
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.pawn.health.lastReceivedNeuralSuperchargeTick = Find.TickManager.TicksGame;
		}
	}
}
