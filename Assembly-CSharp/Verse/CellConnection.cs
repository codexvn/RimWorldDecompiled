using System;

namespace Verse
{
	// Token: 0x020005AD RID: 1453
	[Flags]
	public enum CellConnection : byte
	{
		// Token: 0x04001ED1 RID: 7889
		Self = 0,
		// Token: 0x04001ED2 RID: 7890
		North = 1,
		// Token: 0x04001ED3 RID: 7891
		South = 2,
		// Token: 0x04001ED4 RID: 7892
		East = 4,
		// Token: 0x04001ED5 RID: 7893
		West = 8,
		// Token: 0x04001ED6 RID: 7894
		NorthEast = 16,
		// Token: 0x04001ED7 RID: 7895
		SouthEast = 32,
		// Token: 0x04001ED8 RID: 7896
		SouthWest = 64,
		// Token: 0x04001ED9 RID: 7897
		NorthWest = 128,
		// Token: 0x04001EDA RID: 7898
		CardinalNeighbors = 15,
		// Token: 0x04001EDB RID: 7899
		DiagonalNeighbors = 240,
		// Token: 0x04001EDC RID: 7900
		NorthNeighbours = 145,
		// Token: 0x04001EDD RID: 7901
		EastNeighbours = 52,
		// Token: 0x04001EDE RID: 7902
		SouthNeighbours = 98,
		// Token: 0x04001EDF RID: 7903
		WestNeighbours = 200,
		// Token: 0x04001EE0 RID: 7904
		AllNeighbours = 255
	}
}
