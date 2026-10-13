using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C3 RID: 6083
	public class ThinkNode_HarmedRecently : ThinkNode_Conditional
	{
		// Token: 0x06008DD7 RID: 36311 RVA: 0x002A1643 File Offset: 0x0029F843
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.lastHarmTick > 0 && Find.TickManager.TicksGame < pawn.mindState.lastHarmTick + this.thresholdTicks;
		}

		// Token: 0x04005BD8 RID: 23512
		public int thresholdTicks = 2500;
	}
}
