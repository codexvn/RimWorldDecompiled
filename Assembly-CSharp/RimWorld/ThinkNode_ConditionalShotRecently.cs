using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017CC RID: 6092
	public class ThinkNode_ConditionalShotRecently : ThinkNode_Conditional
	{
		// Token: 0x06008DEC RID: 36332 RVA: 0x002A1881 File Offset: 0x0029FA81
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.lastRangedHarmTick > 0 && Find.TickManager.TicksGame < pawn.mindState.lastRangedHarmTick + this.thresholdTicks;
		}

		// Token: 0x04005BDF RID: 23519
		public int thresholdTicks = 2500;
	}
}
