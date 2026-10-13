using System;
using RimWorld;
using Verse;

// Token: 0x0200002D RID: 45
public class LayoutWorker_SimpleRuin : LayoutWorker_Structure
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000BE RID: 190 RVA: 0x00006AAC File Offset: 0x00004CAC
	protected override float RoomToExteriorDoorRatio
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00002C2C File Offset: 0x00000E2C
	public LayoutWorker_SimpleRuin(LayoutDef def) : base(def)
	{
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00006AB4 File Offset: 0x00004CB4
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		LayoutStructureSketch sketch = parms.sketch;
		int minRoomHeight = base.Def.minRoomHeight;
		return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0f, false, false, null, 2, new IntRange?(IntRange.Zero), CorridorShape.All, true, false);
	}
}
