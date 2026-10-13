using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DA8 RID: 11688
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x060106F5 RID: 67317 RVA: 0x004BE0C3 File Offset: 0x004BC2C3
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		// Token: 0x0400AB71 RID: 43889
		public float harmFrequencyPerArea = 0.011f;

		// Token: 0x0400AB72 RID: 43890
		public float leaflessPlantKillChance = 0.05f;

		// Token: 0x0400AB73 RID: 43891
		public bool ignoreSpecialTrees;

		// Token: 0x0400AB74 RID: 43892
		public bool messageOnCropDeath = true;

		// Token: 0x0400AB75 RID: 43893
		public SimpleCurve radiusPerDayCurve;
	}
}
