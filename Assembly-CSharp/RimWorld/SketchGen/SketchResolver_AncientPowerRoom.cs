using System;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038AD RID: 14509
	public class SketchResolver_AncientPowerRoom : SketchResolver
	{
		// Token: 0x060140A8 RID: 82088 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x060140A9 RID: 82089 RVA: 0x005F3E20 File Offset: 0x005F2020
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient power room"))
			{
				return;
			}
			CellRect value = parms.rect.Value;
			ThingDef ancientGenerator = ThingDefOf.AncientGenerator;
			foreach (IntVec3 intVec in value.Cells)
			{
				if (!Rand.Chance(0.85f))
				{
					CellRect cellRect = GenAdj.OccupiedRect(intVec, ancientGenerator.defaultPlacingRot, ancientGenerator.size);
					bool flag = true;
					foreach (IntVec3 pos in cellRect.ExpandedBy(1))
					{
						if (parms.sketch.ThingsAt(pos).Any<SketchThing>())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						parms.sketch.AddThing(ancientGenerator, intVec, ancientGenerator.defaultPlacingRot, null, 1, null, null, true, 2f);
						ScatterDebrisUtility.ScatterAround(intVec, ancientGenerator.size, Rot4.North, parms.sketch, ThingDefOf.Filth_OilSmear, 0.45f, 1, int.MaxValue, null);
					}
				}
			}
			SketchResolveParams parms2 = parms;
			parms2.thingCentral = ancientGenerator;
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
		}

		// Token: 0x0400DD63 RID: 56675
		private const float SkipChance = 0.85f;

		// Token: 0x0400DD64 RID: 56676
		private const float OilSmearChance = 0.45f;
	}
}
