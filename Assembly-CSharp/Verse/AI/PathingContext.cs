using System;

namespace Verse.AI
{
	// Token: 0x02000FB6 RID: 4022
	public class PathingContext : IDisposable
	{
		// Token: 0x060062CF RID: 25295 RVA: 0x001F2658 File Offset: 0x001F0858
		public PathingContext(Map map, PathGrid pathGrid)
		{
			this.map = map;
			this.pathGrid = pathGrid;
		}

		// Token: 0x060062D0 RID: 25296 RVA: 0x001F266E File Offset: 0x001F086E
		public void Dispose()
		{
			this.pathGrid.Dispose();
		}

		// Token: 0x0400469A RID: 18074
		public readonly Map map;

		// Token: 0x0400469B RID: 18075
		public readonly PathGrid pathGrid;
	}
}
