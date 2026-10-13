using System;
using RimWorld;
using Verse;

// Token: 0x0200002C RID: 44
public class LayoutWorker_OrbitalPlatform : LayoutWorker_Structure
{
	// Token: 0x060000BC RID: 188 RVA: 0x00002C2C File Offset: 0x00000E2C
	public LayoutWorker_OrbitalPlatform(LayoutDef def) : base(def)
	{
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00006A64 File Offset: 0x00004C64
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		int minRoomWidth = 10;
		int minRoomHeight = 10;
		float areaPrunePercent = 0.1f;
		bool canRemoveRooms = true;
		bool generateDoors = false;
		IntRange? maxMergeRoomsRange = new IntRange?(new IntRange(2, 4));
		CorridorShape corridorShapes = base.Def.corridorShapes;
		return RoomLayoutGenerator.GenerateRandomLayout(rect, minRoomWidth, minRoomHeight, areaPrunePercent, canRemoveRooms, generateDoors, base.Def.corridorDef, 2, maxMergeRoomsRange, corridorShapes, false, false);
	}
}
