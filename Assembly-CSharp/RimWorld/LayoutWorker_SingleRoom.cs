using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC3 RID: 7875
	public class LayoutWorker_SingleRoom : LayoutWorker_Structure
	{
		// Token: 0x0600AE9E RID: 44702 RVA: 0x00002C2C File Offset: 0x00000E2C
		public LayoutWorker_SingleRoom(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600AE9F RID: 44703 RVA: 0x0031CDCC File Offset: 0x0031AFCC
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			int minRoomHeight = base.Def.minRoomHeight;
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0.2f, false, false, null, 2, null, CorridorShape.All, true, true);
		}
	}
}
