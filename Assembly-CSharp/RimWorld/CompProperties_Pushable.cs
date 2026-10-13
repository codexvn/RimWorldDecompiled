using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DCF RID: 11727
	public class CompProperties_Pushable : CompProperties
	{
		// Token: 0x060107DF RID: 67551 RVA: 0x004C1F23 File Offset: 0x004C0123
		public CompProperties_Pushable()
		{
			this.compClass = typeof(CompPushable);
		}

		// Token: 0x0400ABFE RID: 44030
		public float offsetDistance;

		// Token: 0x0400ABFF RID: 44031
		public float smoothTime = 0.3f;

		// Token: 0x0400AC00 RID: 44032
		public bool givePushOption;
	}
}
