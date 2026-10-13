using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DFD RID: 11773
	public class CompProperties_SendSignalOnMotion : CompProperties
	{
		// Token: 0x0601091A RID: 67866 RVA: 0x004C6977 File Offset: 0x004C4B77
		public CompProperties_SendSignalOnMotion()
		{
			this.compClass = typeof(CompSendSignalOnMotion);
		}

		// Token: 0x0400ACAE RID: 44206
		public bool triggerOnPawnInRoom;

		// Token: 0x0400ACAF RID: 44207
		public float radius;

		// Token: 0x0400ACB0 RID: 44208
		public int enableAfterTicks;

		// Token: 0x0400ACB1 RID: 44209
		public bool onlyHumanlike;

		// Token: 0x0400ACB2 RID: 44210
		public bool triggeredBySkipPsycasts;

		// Token: 0x0400ACB3 RID: 44211
		[NoTranslate]
		public string signalTag;
	}
}
