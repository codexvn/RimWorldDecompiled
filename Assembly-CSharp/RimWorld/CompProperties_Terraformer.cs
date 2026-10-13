using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E82 RID: 11906
	public class CompProperties_Terraformer : CompProperties, IPlantEffectRadius
	{
		// Token: 0x06010C34 RID: 68660 RVA: 0x004D47C4 File Offset: 0x004D29C4
		public CompProperties_Terraformer()
		{
			this.compClass = typeof(CompTerraformer);
		}

		// Token: 0x17002B08 RID: 11016
		// (get) Token: 0x06010C35 RID: 68661 RVA: 0x004D47F2 File Offset: 0x004D29F2
		public float PlantEffectRadius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x0400AEA2 RID: 44706
		public float radius = 6.9f;

		// Token: 0x0400AEA3 RID: 44707
		public float secondsPerConvert = 20f;

		// Token: 0x0400AEA4 RID: 44708
		public TerrainDef convertTerrainDef;
	}
}
