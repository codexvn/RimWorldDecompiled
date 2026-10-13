using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC1 RID: 7873
	public class LayoutWorker_Mechhive : LayoutWorker_Structure
	{
		// Token: 0x0600AE97 RID: 44695 RVA: 0x00002C2C File Offset: 0x00000E2C
		public LayoutWorker_Mechhive(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600AE98 RID: 44696 RVA: 0x0031CCB4 File Offset: 0x0031AEB4
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			int minRoomHeight = base.Def.minRoomHeight;
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0.1f, true, false, null, 2, new IntRange?(new IntRange(2, 4)), CorridorShape.All, true, false);
		}

		// Token: 0x0600AE99 RID: 44697 RVA: 0x0031CCFE File Offset: 0x0031AEFE
		protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
		{
			base.PostLayoutFlushedToSketch(parms);
			LayoutWorker_Mechhive.ReplaceDoors(parms.layoutSketch);
		}

		// Token: 0x0600AE9A RID: 44698 RVA: 0x0031CD14 File Offset: 0x0031AF14
		private static void ReplaceDoors(LayoutSketch sketch)
		{
			int num = Mathf.CeilToInt((float)sketch.Things.Count((SketchThing thing) => thing.def.IsDoor) * 0.2f);
			foreach (SketchThing sketchThing in sketch.Things.InRandomOrder(null))
			{
				if (sketchThing.def.IsDoor)
				{
					sketchThing.def = ThingDefOf.AncientBlastDoor;
					sketchThing.stuff = null;
					num--;
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x04007808 RID: 30728
		private const float BlastDoorRatio = 0.2f;
	}
}
