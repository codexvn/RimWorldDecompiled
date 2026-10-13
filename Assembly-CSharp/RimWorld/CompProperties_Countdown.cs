using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C47 RID: 11335
	public class CompProperties_Countdown : CompProperties
	{
		// Token: 0x0600FE39 RID: 65081 RVA: 0x0049B967 File Offset: 0x00499B67
		public CompProperties_Countdown()
		{
			this.compClass = typeof(CompCountdown);
		}

		// Token: 0x0400A69E RID: 42654
		[MustTranslate]
		public string label;
	}
}
