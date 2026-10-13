using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200196D RID: 6509
	public class CompProperties_FacilityQualityBased : CompProperties_Facility
	{
		// Token: 0x060093D2 RID: 37842 RVA: 0x002B786B File Offset: 0x002B5A6B
		public CompProperties_FacilityQualityBased()
		{
			this.compClass = typeof(CompFacilityQualityBased);
		}

		// Token: 0x04005F55 RID: 24405
		public Dictionary<StatDef, Dictionary<QualityCategory, float>> statOffsetsPerQuality;
	}
}
