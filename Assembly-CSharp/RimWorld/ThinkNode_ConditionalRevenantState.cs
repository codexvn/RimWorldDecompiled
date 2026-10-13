using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D4 RID: 6100
	public class ThinkNode_ConditionalRevenantState : ThinkNode_Conditional
	{
		// Token: 0x06008DFE RID: 36350 RVA: 0x002A1A5C File Offset: 0x0029FC5C
		protected override bool Satisfied(Pawn pawn)
		{
			CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
			return compRevenant != null && compRevenant.revenantState == this.state;
		}

		// Token: 0x04005BE5 RID: 23525
		public RevenantState state;
	}
}
