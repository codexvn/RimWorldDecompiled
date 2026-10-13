using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C54 RID: 11348
	public class CompProperties_DestroyAfterDelay : CompProperties
	{
		// Token: 0x0600FE7E RID: 65150 RVA: 0x0049C723 File Offset: 0x0049A923
		public CompProperties_DestroyAfterDelay()
		{
			this.compClass = typeof(CompDestroyAfterDelay);
		}

		// Token: 0x0400A6C1 RID: 42689
		public int delayTicks;

		// Token: 0x0400A6C2 RID: 42690
		public DestroyMode destroyMode;

		// Token: 0x0400A6C3 RID: 42691
		public bool displayCountdownOnLabel;

		// Token: 0x0400A6C4 RID: 42692
		[MustTranslate]
		public string countdownLabel;
	}
}
