using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B39 RID: 6969
	public class GeneratedLocationDef : Def
	{
		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x06009BB0 RID: 39856 RVA: 0x002D2717 File Offset: 0x002D0917
		public List<PlanetLayerDef> LayerDefs
		{
			get
			{
				List<PlanetLayerDef> result;
				if ((result = this.layerDefs) == null && (result = GeneratedLocationDef.SurfaceList) == null)
				{
					List<PlanetLayerDef> list = new List<PlanetLayerDef>();
					list.Add(PlanetLayerDefOf.Surface);
					result = list;
					GeneratedLocationDef.SurfaceList = list;
				}
				return result;
			}
		}

		// Token: 0x04006C1A RID: 27674
		private List<PlanetLayerDef> layerDefs;

		// Token: 0x04006C1B RID: 27675
		public int layerMaximum = -1;

		// Token: 0x04006C1C RID: 27676
		public WorldObjectDef worldObjectDef;

		// Token: 0x04006C1D RID: 27677
		public List<ThingDef> preciousResources;

		// Token: 0x04006C1E RID: 27678
		public FloatRange TimeoutRangeDays = new FloatRange(20f, 40f);

		// Token: 0x04006C1F RID: 27679
		public float weight = 1f;

		// Token: 0x04006C20 RID: 27680
		private static List<PlanetLayerDef> SurfaceList;
	}
}
