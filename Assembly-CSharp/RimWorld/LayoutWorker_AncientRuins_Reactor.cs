using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBF RID: 7871
	public class LayoutWorker_AncientRuins_Reactor : LayoutWorker_AncientRuins
	{
		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600AE91 RID: 44689 RVA: 0x0031CBE4 File Offset: 0x0031ADE4
		protected override float RoomToExteriorDoorRatio
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600AE92 RID: 44690 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool SpawnRandomBlastDoors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AE93 RID: 44691 RVA: 0x0031CBEB File Offset: 0x0031ADEB
		public LayoutWorker_AncientRuins_Reactor(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600AE94 RID: 44692 RVA: 0x0031CBF4 File Offset: 0x0031ADF4
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			int minRoomHeight = base.Def.minRoomHeight;
			int minRoomWidth = base.Def.minRoomWidth;
			int minRoomHeight2 = minRoomHeight;
			float areaPrunePercent = 0.1f;
			bool canRemoveRooms = true;
			bool generateDoors = false;
			IntRange? maxMergeRoomsRange = new IntRange?(new IntRange(2, 4));
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, minRoomWidth, minRoomHeight2, areaPrunePercent, canRemoveRooms, generateDoors, base.Def.corridorDef, 2, maxMergeRoomsRange, base.Def.corridorShapes, false, false);
		}
	}
}
