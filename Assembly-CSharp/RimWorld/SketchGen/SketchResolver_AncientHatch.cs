using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038A7 RID: 14503
	public class SketchResolver_AncientHatch : SketchResolver
	{
		// Token: 0x06014090 RID: 82064 RVA: 0x005F386F File Offset: 0x005F1A6F
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x06014091 RID: 82065 RVA: 0x005F387C File Offset: 0x005F1A7C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Ancient hatch"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			CellRect cellRect = new CellRect(0, 0, 7, 7);
			foreach (IntVec3 pos in cellRect)
			{
				sketch.AddTerrain(TerrainDefOf.AncientConcrete, pos, true);
			}
			foreach (IntVec3 pos2 in cellRect.ContractedBy(1))
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos2, true);
			}
			foreach (IntVec3 pos3 in cellRect.ContractedBy(1).Corners)
			{
				sketch.AddThing(ThingDefOf.AncientSecurityTurret, pos3, Rot4.North, null, 1, null, null, true, 2f);
			}
			sketch.AddThing(ThingDefOf.AncientHatch, cellRect.CenterCell, Rot4.North, null, 1, null, null, true, 2f);
			parms.sketch.Merge(sketch, true);
		}
	}
}
