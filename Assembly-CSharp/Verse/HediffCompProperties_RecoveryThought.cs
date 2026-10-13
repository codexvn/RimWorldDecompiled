using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006F5 RID: 1781
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x06002BCE RID: 11214 RVA: 0x000E446A File Offset: 0x000E266A
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x0400233D RID: 9021
		public ThoughtDef thought;
	}
}
