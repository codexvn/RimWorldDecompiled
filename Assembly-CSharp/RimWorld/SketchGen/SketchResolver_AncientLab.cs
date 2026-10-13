using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038A8 RID: 14504
	public class SketchResolver_AncientLab : SketchResolver
	{
		// Token: 0x06014093 RID: 82067 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x06014094 RID: 82068 RVA: 0x005F39F8 File Offset: 0x005F1BF8
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient lab"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.thingCentral = ThingDefOf.AncientOperatingTable;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
			SketchResolveParams parms3 = parms;
			parms3.wallEdgeThing = ThingDefOf.AncientDisplayBank;
			parms3.requireFloor = new bool?(true);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms3);
			foreach (IntVec3 pos in parms.rect.Value.Cells)
			{
				parms.sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
		}
	}
}
