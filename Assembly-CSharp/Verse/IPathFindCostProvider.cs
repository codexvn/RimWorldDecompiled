using System;

namespace Verse
{
	// Token: 0x020005C7 RID: 1479
	public interface IPathFindCostProvider
	{
		// Token: 0x06002602 RID: 9730
		ushort PathFindCostFor(Pawn pawn);

		// Token: 0x06002603 RID: 9731
		CellRect GetOccupiedRect();
	}
}
