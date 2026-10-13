using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001892 RID: 6290
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06009015 RID: 36885 RVA: 0x002A805C File Offset: 0x002A625C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
