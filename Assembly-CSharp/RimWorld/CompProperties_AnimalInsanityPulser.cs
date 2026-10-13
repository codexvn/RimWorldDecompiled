using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001951 RID: 6481
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x06009369 RID: 37737 RVA: 0x002B5ABB File Offset: 0x002B3CBB
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x04005ED0 RID: 24272
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04005ED1 RID: 24273
		public int radius = 25;
	}
}
