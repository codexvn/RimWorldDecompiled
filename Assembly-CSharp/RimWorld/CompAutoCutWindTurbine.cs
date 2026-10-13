using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BB3 RID: 11187
	public class CompAutoCutWindTurbine : CompAutoCut
	{
		// Token: 0x0600FA1E RID: 64030 RVA: 0x0048B7A0 File Offset: 0x004899A0
		public override IEnumerable<IntVec3> GetAutoCutCells()
		{
			return WindTurbineUtility.CalculateWindCells(this.parent.Position, this.parent.Rotation, this.parent.def.size);
		}

		// Token: 0x0600FA1F RID: 64031 RVA: 0x0048B7D0 File Offset: 0x004899D0
		public override ThingFilter GetDefaultAutoCutFilter()
		{
			if (this.defaultAutoCutFilter == null)
			{
				this.defaultAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.GetFixedAutoCutFilter().AllowedThingDefs)
				{
					if (thingDef.blockWind)
					{
						this.defaultAutoCutFilter.SetAllow(thingDef, true);
					}
				}
			}
			return this.defaultAutoCutFilter;
		}

		// Token: 0x0600FA20 RID: 64032 RVA: 0x0048B84C File Offset: 0x00489A4C
		public override ThingFilter GetFixedAutoCutFilter()
		{
			if (this.fixedAutoCutFilter == null)
			{
				this.fixedAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.parent.Map.wildPlantSpawner.AllWildPlants)
				{
					if (thingDef.plant.allowAutoCut)
					{
						this.fixedAutoCutFilter.SetAllow(thingDef, true);
					}
				}
				this.fixedAutoCutFilter.SetAllow(ThingCategoryDefOf.Stumps, true, null, null);
			}
			return this.fixedAutoCutFilter;
		}

		// Token: 0x0600FA21 RID: 64033 RVA: 0x0048B8F0 File Offset: 0x00489AF0
		public override void CompTick()
		{
			base.CompTick();
			if (this.autoCut && this.parent.IsHashIntervalTick(2000))
			{
				base.DesignatePlantsToCut();
			}
		}

		// Token: 0x0400A469 RID: 42089
		private ThingFilter fixedAutoCutFilter;

		// Token: 0x0400A46A RID: 42090
		public ThingFilter defaultAutoCutFilter;
	}
}
