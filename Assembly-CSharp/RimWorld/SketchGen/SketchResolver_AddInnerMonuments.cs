using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038BC RID: 14524
	public class SketchResolver_AddInnerMonuments : SketchResolver
	{
		// Token: 0x060140E8 RID: 82152 RVA: 0x005F5648 File Offset: 0x005F3848
		protected override void ResolveInt(SketchResolveParams parms)
		{
			CellRect outerRect = parms.rect ?? parms.sketch.OccupiedRect;
			HashSet<IntVec3> processed = new HashSet<IntVec3>();
			using (IEnumerator<IntVec3> enumerator = outerRect.Cells.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					CellRect cellRect = SketchGenUtility.FindBiggestRectAt(c, outerRect, parms.sketch, processed, (IntVec3 x) => !parms.sketch.ThingsAt(x).Any<SketchThing>() && parms.sketch.AnyTerrainAt(c));
					if (cellRect.Width >= 7 && cellRect.Height >= 7)
					{
						int newX = Rand.RangeInclusive(5, cellRect.Width - 2);
						int newZ = Rand.RangeInclusive(5, cellRect.Height - 2);
						Sketch sketch = new Sketch();
						SketchResolveParams parms2 = parms;
						parms2.sketch = sketch;
						parms2.monumentSize = new IntVec2?(new IntVec2(newX, newZ));
						parms2.rect = null;
						SketchResolverDefOf.Monument.Resolve(parms2);
						parms.sketch.MergeAt(sketch, cellRect.CenterCell, Sketch.SpawnPosType.OccupiedCenter, false);
					}
				}
			}
		}

		// Token: 0x060140E9 RID: 82153 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0400DD83 RID: 56707
		private const int MinRectWidth = 7;

		// Token: 0x0400DD84 RID: 56708
		private const int MinRectHeight = 7;
	}
}
