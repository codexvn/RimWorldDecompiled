using System;

namespace Verse.AI
{
	// Token: 0x02000FE8 RID: 4072
	public class ThinkNode_ConditionalCanPickupOpportunisticWeapon : ThinkNode_Conditional
	{
		// Token: 0x0600645B RID: 25691 RVA: 0x001FD427 File Offset: 0x001FB627
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.pickupOpportunisticWeapon;
		}
	}
}
