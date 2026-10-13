using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001893 RID: 6291
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x06009017 RID: 36887 RVA: 0x002A8085 File Offset: 0x002A6285
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
