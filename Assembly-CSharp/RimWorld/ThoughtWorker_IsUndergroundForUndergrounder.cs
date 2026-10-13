using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001890 RID: 6288
	public class ThoughtWorker_IsUndergroundForUndergrounder : ThoughtWorker
	{
		// Token: 0x06009011 RID: 36881 RVA: 0x002A8004 File Offset: 0x002A6204
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && flag;
		}
	}
}
