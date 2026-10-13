using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B4 RID: 6324
	public class ThoughtWorker_LookChangeDesired : ThoughtWorker
	{
		// Token: 0x0600907E RID: 36990 RVA: 0x002A96B1 File Offset: 0x002A78B1
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.style == null || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return p.style.LookChangeDesired;
		}
	}
}
