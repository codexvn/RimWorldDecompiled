using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038D3 RID: 14547
	public class SketchResolver_MechhiveGuardOutpost : SketchResolver
	{
		// Token: 0x0601413A RID: 82234 RVA: 0x005F386F File Offset: 0x005F1A6F
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x0601413B RID: 82235 RVA: 0x005F8678 File Offset: 0x005F6878
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Mechhive guard outpost"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			CellRect cellRect = CellRect.CenteredOn(IntVec3.Zero, 5, 5);
			foreach (IntVec3 intVec in cellRect.EdgeCells)
			{
				if (intVec.x != 0 && intVec.z != 0)
				{
					sketch.AddThing(ThingDefOf.Barricade, intVec, Rot4.North, ThingDefOf.Steel, 1, null, null, true, 2f);
				}
			}
			foreach (IntVec3 pos in cellRect.ContractedBy(1).Cells)
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
			sketch.AddThing(ThingDefOf.Turret_AutoMiniTurret, cellRect.CenterCell, Rot4.North, null, 1, null, null, true, 2f);
			parms.sketch.Merge(sketch, true);
		}
	}
}
