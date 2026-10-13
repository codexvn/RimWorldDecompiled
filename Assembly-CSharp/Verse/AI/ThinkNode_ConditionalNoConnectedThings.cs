using System;

namespace Verse.AI
{
	// Token: 0x02000FE9 RID: 4073
	public class ThinkNode_ConditionalNoConnectedThings : ThinkNode_Conditional
	{
		// Token: 0x0600645D RID: 25693 RVA: 0x001FD448 File Offset: 0x001FB648
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.connections == null || !pawn.connections.ConnectedThings.Any<Thing>();
		}
	}
}
