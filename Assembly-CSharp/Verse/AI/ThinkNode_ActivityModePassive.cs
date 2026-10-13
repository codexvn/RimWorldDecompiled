using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FEE RID: 4078
	public class ThinkNode_ActivityModePassive : ThinkNode_Conditional
	{
		// Token: 0x06006468 RID: 25704 RVA: 0x001FD569 File Offset: 0x001FB769
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetComp<CompActivity>() != null && pawn.GetComp<CompActivity>().IsDormant;
		}
	}
}
