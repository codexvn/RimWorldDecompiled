using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001926 RID: 6438
	public abstract class CompProperties_Readable : CompProperties
	{
		// Token: 0x0600926C RID: 37484 RVA: 0x002B23E7 File Offset: 0x002B05E7
		public CompProperties_Readable()
		{
			this.compClass = typeof(CompReadable);
		}

		// Token: 0x04005D79 RID: 23929
		public MentalBreakIntensity mentalBreakIntensity = MentalBreakIntensity.Major;

		// Token: 0x04005D7A RID: 23930
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04005D7B RID: 23931
		public List<ReadingOutcomeProperties> doers;
	}
}
