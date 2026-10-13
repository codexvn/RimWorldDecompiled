using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183D RID: 6205
	public class ThoughtWorker_Precept_AlwaysActive : ThoughtWorker_Precept
	{
		// Token: 0x06008F51 RID: 36689 RVA: 0x002A5833 File Offset: 0x002A3A33
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtState.ActiveDefault;
		}
	}
}
