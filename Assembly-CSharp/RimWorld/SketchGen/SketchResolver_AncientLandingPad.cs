using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038A9 RID: 14505
	public class SketchResolver_AncientLandingPad : SketchResolver
	{
		// Token: 0x06014096 RID: 82070 RVA: 0x005F386F File Offset: 0x005F1A6F
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x06014097 RID: 82071 RVA: 0x005F3ABC File Offset: 0x005F1CBC
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient landing pad"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			IntVec2 intVec = parms.landingPadSize ?? new IntVec2(12, 12);
			CellRect cellRect = new CellRect(0, 0, intVec.x, intVec.z);
			foreach (IntVec3 pos in cellRect)
			{
				sketch.AddTerrain(TerrainDefOf.AncientConcrete, pos, true);
			}
			foreach (IntVec3 pos2 in cellRect.Corners)
			{
				sketch.AddThing(ThingDefOf.AncientShipBeacon, pos2, Rot4.North, null, 1, null, null, true, 2f);
			}
			parms.sketch.Merge(sketch, true);
			SketchResolveParams parms2 = parms;
			parms2.destroyChanceExp = new float?(5f);
			SketchResolverDefOf.DamageBuildings.Resolve(parms2);
		}
	}
}
