using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FEC RID: 4076
	public class ThinkNode_ConditionalLowEnergy : ThinkNode_Conditional
	{
		// Token: 0x06006464 RID: 25700 RVA: 0x001FD539 File Offset: 0x001FB739
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			return ((needs != null) ? needs.energy : null) != null && pawn.needs.energy.IsLowEnergySelfShutdown;
		}
	}
}
