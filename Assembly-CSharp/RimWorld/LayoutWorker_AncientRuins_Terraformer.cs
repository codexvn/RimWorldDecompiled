using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC0 RID: 7872
	public class LayoutWorker_AncientRuins_Terraformer : LayoutWorker_AncientRuins
	{
		// Token: 0x0600AE95 RID: 44693 RVA: 0x0031CBEB File Offset: 0x0031ADEB
		public LayoutWorker_AncientRuins_Terraformer(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600AE96 RID: 44694 RVA: 0x0031CC54 File Offset: 0x0031AE54
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
