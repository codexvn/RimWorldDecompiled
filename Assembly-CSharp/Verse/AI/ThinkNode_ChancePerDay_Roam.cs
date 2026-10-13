using System;

namespace Verse.AI
{
	// Token: 0x02000FD9 RID: 4057
	public class ThinkNode_ChancePerDay_Roam : ThinkNode_ChancePerHour
	{
		// Token: 0x06006435 RID: 25653 RVA: 0x001FD105 File Offset: 0x001FB305
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.roamMtbDays.GetValueOrDefault(0f) * 24f;
		}
	}
}
