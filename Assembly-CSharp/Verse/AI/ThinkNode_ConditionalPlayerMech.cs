using System;

namespace Verse.AI
{
	// Token: 0x02000FF1 RID: 4081
	public class ThinkNode_ConditionalPlayerMech : ThinkNode_Conditional
	{
		// Token: 0x0600646E RID: 25710 RVA: 0x001FD5A4 File Offset: 0x001FB7A4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonyMech;
		}
	}
}
