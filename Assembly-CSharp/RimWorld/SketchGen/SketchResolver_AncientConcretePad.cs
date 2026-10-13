using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038C4 RID: 14532
	public class SketchResolver_AncientConcretePad : SketchResolver
	{
		// Token: 0x060140FF RID: 82175 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x06014100 RID: 82176 RVA: 0x005F62B8 File Offset: 0x005F44B8
		protected override void ResolveInt(SketchResolveParams parms)
		{
			Sketch sketch = new Sketch();
			foreach (IntVec3 pos in parms.rect.Value.EdgeCells)
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
			foreach (IntVec3 pos2 in parms.rect.Value.ContractedBy(1))
			{
				sketch.AddTerrain(TerrainDefOf.AncientConcrete, pos2, true);
			}
			foreach (IntVec3 pos3 in parms.rect.Value.Corners)
			{
				sketch.AddThing(ThingDefOf.AncientShipBeacon, pos3, Rot4.North, null, 1, null, null, true, 2f);
			}
			parms.sketch.Merge(sketch, true);
		}
	}
}
