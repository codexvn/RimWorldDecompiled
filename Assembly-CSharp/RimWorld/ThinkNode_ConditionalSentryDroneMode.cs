using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D6 RID: 6102
	public class ThinkNode_ConditionalSentryDroneMode : ThinkNode_Conditional
	{
		// Token: 0x06008E02 RID: 36354 RVA: 0x002A1AA4 File Offset: 0x0029FCA4
		protected override bool Satisfied(Pawn pawn)
		{
			CompSentryDrone compSentryDrone = pawn.TryGetComp<CompSentryDrone>();
			return compSentryDrone != null && compSentryDrone.Mode == this.mode;
		}

		// Token: 0x04005BE6 RID: 23526
		public CompSentryDrone.SentryDroneMode mode;
	}
}
