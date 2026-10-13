using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DFA RID: 11770
	public class CompProperties_SendSignalOnCountdown : CompProperties
	{
		// Token: 0x06010905 RID: 67845 RVA: 0x004C65F7 File Offset: 0x004C47F7
		public CompProperties_SendSignalOnCountdown()
		{
			this.compClass = typeof(CompSendSignalOnCountdown);
		}

		// Token: 0x0400ACA5 RID: 44197
		public SimpleCurve countdownCurveTicks;

		// Token: 0x0400ACA6 RID: 44198
		[NoTranslate]
		public string signalTag;
	}
}
