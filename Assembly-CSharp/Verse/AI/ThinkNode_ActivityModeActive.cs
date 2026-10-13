using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FEF RID: 4079
	public class ThinkNode_ActivityModeActive : ThinkNode_Conditional
	{
		// Token: 0x0600646A RID: 25706 RVA: 0x001FD580 File Offset: 0x001FB780
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetComp<CompActivity>() != null && pawn.GetComp<CompActivity>().IsActive;
		}
	}
}
