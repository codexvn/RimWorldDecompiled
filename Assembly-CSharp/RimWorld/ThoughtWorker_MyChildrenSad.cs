using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D9 RID: 6361
	public class ThoughtWorker_MyChildrenSad : ThoughtWorker_MyChildSad
	{
		// Token: 0x060090EA RID: 37098 RVA: 0x002AA7F2 File Offset: 0x002A89F2
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) > 1;
		}
	}
}
