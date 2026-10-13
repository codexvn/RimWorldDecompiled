using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038D4 RID: 14548
	public class SketchResolver_MechhiveLaunchPad : SketchResolver
	{
		// Token: 0x0601413D RID: 82237 RVA: 0x005F87B4 File Offset: 0x005F69B4
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null && parms.rect != null;
		}

		// Token: 0x0601413E RID: 82238 RVA: 0x005F87CC File Offset: 0x005F69CC
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Mechhive guard outpost"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			CellRect value = parms.rect.Value;
			foreach (IntVec3 pos in value.Cells)
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(1, 0, 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(1, 0, value.maxZ - 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(value.maxX - 1, 0, 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(value.maxX - 1, 0, value.maxZ - 1), Rot4.North, null, 1, null, null, true, 2f);
			parms.sketch.Merge(sketch, true);
		}
	}
}
