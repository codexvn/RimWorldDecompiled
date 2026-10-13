using System;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x02000026 RID: 38
public class LayoutWorker_AncientStockpile : LayoutWorker_Structure
{
	// Token: 0x0600008C RID: 140 RVA: 0x00002C2C File Offset: 0x00000E2C
	public LayoutWorker_AncientStockpile(LayoutDef def) : base(def)
	{
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00005178 File Offset: 0x00003378
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		LayoutStructureSketch sketch = parms.sketch;
		int minRoomHeight = base.Def.minRoomHeight;
		return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0.25f, true, false, null, 2, new IntRange?(new IntRange(2, 4)), CorridorShape.All, false, false);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000051C4 File Offset: 0x000033C4
	protected override void PostGraphsGenerated(StructureLayout layout, StructureGenParams parms)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			layoutRoom.noExteriorDoors = (base.Def.exteriorDoorDef == null);
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00005224 File Offset: 0x00003424
	protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
	{
		base.PostLayoutFlushedToSketch(parms);
		LayoutWorker_AncientStockpile.ReplaceDoors(parms.layoutSketch);
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00005238 File Offset: 0x00003438
	private static void ReplaceDoors(LayoutSketch sketch)
	{
		int num = Mathf.CeilToInt((float)sketch.Things.Count((SketchThing thing) => thing.def.IsDoor) * 0.5f);
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

	// Token: 0x04000040 RID: 64
	private const float BlastDoorRatio = 0.5f;
}
