using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D7 RID: 6359
	public class ThoughtWorker_MyChildrenHappy : ThoughtWorker_MyChildHappy
	{
		// Token: 0x060090E3 RID: 37091 RVA: 0x002AA7F2 File Offset: 0x002A89F2
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) > 1;
		}
	}
}
