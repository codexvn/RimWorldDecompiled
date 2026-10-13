using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BCA RID: 11210
	public class CompProperties_AmbientSound : CompProperties
	{
		// Token: 0x0600FAD0 RID: 64208 RVA: 0x0048E1B7 File Offset: 0x0048C3B7
		public CompProperties_AmbientSound()
		{
			this.compClass = typeof(CompAmbientSound);
		}

		// Token: 0x0400A4B0 RID: 42160
		public SoundDef sound;

		// Token: 0x0400A4B1 RID: 42161
		public bool disabledOnUnpowered;

		// Token: 0x0400A4B2 RID: 42162
		public bool disableOnHacked;

		// Token: 0x0400A4B3 RID: 42163
		public bool disableOnInteracted;
	}
}
