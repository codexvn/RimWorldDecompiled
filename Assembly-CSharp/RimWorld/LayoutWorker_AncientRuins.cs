using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBD RID: 7869
	public class LayoutWorker_AncientRuins : LayoutWorker_Structure
	{
		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600AE86 RID: 44678 RVA: 0x0031CA4F File Offset: 0x0031AC4F
		protected override float RoomToExteriorDoorRatio
		{
			get
			{
				return 0.33f;
			}
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600AE87 RID: 44679 RVA: 0x0031CA56 File Offset: 0x0031AC56
		protected virtual float RandomBlastDoorRatio
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600AE88 RID: 44680 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool SpawnRandomBlastDoors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AE89 RID: 44681 RVA: 0x0031CA60 File Offset: 0x0031AC60
		protected override ThingDef GetWallDoorStuff(StructureGenParams parms)
		{
			ThingDef wallDoorStuff = base.GetWallDoorStuff(parms);
			if (wallDoorStuff != ThingDefOf.WoodLog)
			{
				return wallDoorStuff;
			}
			return ThingDefOf.Steel;
		}

		// Token: 0x0600AE8A RID: 44682 RVA: 0x00002C2C File Offset: 0x00000E2C
		public LayoutWorker_AncientRuins(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600AE8B RID: 44683 RVA: 0x0031CA84 File Offset: 0x0031AC84
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			float areaPrunePercent = base.Def.areaPrunePercent;
			int minRoomHeight = base.Def.minRoomHeight;
			int minRoomWidth = base.Def.minRoomWidth;
			int minRoomHeight2 = minRoomHeight;
			float areaPrunePercent2 = areaPrunePercent;
			bool canRemoveRooms = true;
			bool generateDoors = false;
			IntRange? maxMergeRoomsRange = new IntRange?(IntRange.One);
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, minRoomWidth, minRoomHeight2, areaPrunePercent2, canRemoveRooms, generateDoors, base.Def.corridorDef ?? LayoutRoomDefOf.AncientRuinsCorridor, 2, maxMergeRoomsRange, base.Def.corridorShapes, base.Def.canDisconnectRooms, false);
		}

		// Token: 0x0600AE8C RID: 44684 RVA: 0x0031CAFD File Offset: 0x0031ACFD
		protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
		{
			base.PostLayoutFlushedToSketch(parms);
			if (this.SpawnRandomBlastDoors)
			{
				this.ReplaceRandomBlastDoors(parms.layoutSketch);
			}
		}

		// Token: 0x0600AE8D RID: 44685 RVA: 0x0031CB1C File Offset: 0x0031AD1C
		protected virtual void ReplaceRandomBlastDoors(LayoutSketch sketch)
		{
			int num = Mathf.CeilToInt((float)sketch.Things.Count((SketchThing thing) => thing.def.IsDoor) * this.RandomBlastDoorRatio);
			foreach (SketchThing sketchThing in sketch.Things.InRandomOrder(null))
			{
				if (sketchThing.def.IsDoor && sketchThing.def != ThingDefOf.AncientBlastDoor)
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
	}
}
