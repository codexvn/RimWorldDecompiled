using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001810 RID: 6160
	public class ThoughtWorker_Precept_IsApostate_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008EC9 RID: 36553 RVA: 0x002A3FDF File Offset: 0x002A21DF
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return otherPawn.ideo != null && otherPawn.Ideo != p.Ideo && otherPawn.ideo.PreviousIdeos.Contains(p.Ideo);
		}
	}
}
