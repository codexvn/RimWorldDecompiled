using System;

namespace Verse
{
	// Token: 0x020006E5 RID: 1765
	public class HediffCompProperties_MessageOnRemoval : HediffCompProperties_MessageBase
	{
		// Token: 0x06002BA3 RID: 11171 RVA: 0x000E3C19 File Offset: 0x000E1E19
		public HediffCompProperties_MessageOnRemoval()
		{
			this.compClass = typeof(HediffComp_MessageOnRemoval);
		}

		// Token: 0x0400231D RID: 8989
		public bool messageOnZeroSeverity = true;

		// Token: 0x0400231E RID: 8990
		public bool messageOnNonZeroSeverity = true;
	}
}
