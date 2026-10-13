using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198C RID: 6540
	public class CompProperties_Forbiddable : CompProperties
	{
		// Token: 0x0600944D RID: 37965 RVA: 0x002B90D2 File Offset: 0x002B72D2
		public CompProperties_Forbiddable()
		{
			this.compClass = typeof(CompForbiddable);
		}

		// Token: 0x04005FEC RID: 24556
		public bool allowNonPlayer;

		// Token: 0x04005FED RID: 24557
		public bool forbidOnMake;
	}
}
