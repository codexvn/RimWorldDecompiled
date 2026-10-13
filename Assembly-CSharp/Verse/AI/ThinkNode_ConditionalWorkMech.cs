using System;

namespace Verse.AI
{
	// Token: 0x02000FF0 RID: 4080
	public class ThinkNode_ConditionalWorkMech : ThinkNode_Conditional
	{
		// Token: 0x0600646C RID: 25708 RVA: 0x001FD597 File Offset: 0x001FB797
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.IsWorkMech;
		}
	}
}
