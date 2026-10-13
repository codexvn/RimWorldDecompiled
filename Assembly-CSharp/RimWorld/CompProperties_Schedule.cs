using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001991 RID: 6545
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x06009452 RID: 37970 RVA: 0x002B9158 File Offset: 0x002B7358
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		// Token: 0x04005FF1 RID: 24561
		public float startTime;

		// Token: 0x04005FF2 RID: 24562
		public float endTime = 1f;

		// Token: 0x04005FF3 RID: 24563
		[MustTranslate]
		public string offMessage;
	}
}
