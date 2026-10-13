using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038BF RID: 14527
	public class SketchResolver_AddThingsCentral : SketchResolver
	{
		// Token: 0x060140EE RID: 82158 RVA: 0x005F5818 File Offset: 0x005F3A18
		protected override void ResolveInt(SketchResolveParams parms)
		{
			CellRect cellRect = parms.rect ?? parms.sketch.OccupiedRect;
			bool allowWood = parms.allowWood ?? true;
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.thingCentral, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.thingCentral));
			bool requireFloor = parms.requireFloor.GetValueOrDefault();
			this.processed.Clear();
			try
			{
				Predicate<IntVec3> <>9__1;
				foreach (IntVec3 c in cellRect.Cells.InRandomOrder(null))
				{
					CellRect outerRect = cellRect;
					Sketch sketch = parms.sketch;
					HashSet<IntVec3> hashSet = this.processed;
					Predicate<IntVec3> canTraverse;
					if ((canTraverse = <>9__1) == null)
					{
						canTraverse = (<>9__1 = ((IntVec3 x) => !parms.sketch.ThingsAt(x).Any<SketchThing>() && (!requireFloor || (parms.sketch.TerrainAt(x) != null && parms.sketch.TerrainAt(x).layerable))));
					}
					CellRect cellRect2 = SketchGenUtility.FindBiggestRectAt(c, outerRect, sketch, hashSet, canTraverse);
					if (cellRect2.Width >= parms.thingCentral.size.x + 2 && cellRect2.Height >= parms.thingCentral.size.z + 2)
					{
						IntVec3 intVec = new IntVec3(cellRect2.CenterCell.x - parms.thingCentral.size.x / 2, 0, cellRect2.CenterCell.z - parms.thingCentral.size.z / 2);
						if (Rand.Chance(0.4f) && this.CanPlaceAt(parms.thingCentral, intVec, Rot4.North, parms.sketch))
						{
							parms.sketch.AddThing(parms.thingCentral, intVec, Rot4.North, stuff, 1, null, null, false, 2f);
						}
					}
				}
			}
			finally
			{
				this.processed.Clear();
			}
		}

		// Token: 0x060140EF RID: 82159 RVA: 0x005F5A80 File Offset: 0x005F3C80
		private bool CanPlaceAt(ThingDef def, IntVec3 position, Rot4 rot, Sketch sketch)
		{
			foreach (IntVec3 position2 in GenAdj.OccupiedRect(position, Rot4.North, def.size).AdjacentCellsCardinal)
			{
				if (sketch.GetDoor(position2) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060140F0 RID: 82160 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0400DD88 RID: 56712
		private HashSet<IntVec3> processed = new HashSet<IntVec3>();

		// Token: 0x0400DD89 RID: 56713
		private const float Chance = 0.4f;
	}
}
