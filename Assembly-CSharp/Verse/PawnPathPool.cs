using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C6 RID: 454
	public class PawnPathPool
	{
		// Token: 0x06000C7B RID: 3195 RVA: 0x0003ABD4 File Offset: 0x00038DD4
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0003ABF0 File Offset: 0x00038DF0
		public PawnPath GetPath()
		{
			PawnPath pawnPath;
			if (!this.paths.TryPop(out pawnPath))
			{
				pawnPath = new PawnPath();
				pawnPath.pool = this;
				int num = this.map.mapPawns.AllPawnsSpawnedCount * 2 + 1 + 4;
				int num2 = this.pathsCreated + 1;
				this.pathsCreated = num2;
				if (num2 > num)
				{
					Log.ErrorOnce(string.Format("Leak suspected in object pool for {0}s, created: {1}, expected less than {2}. Dispose them so they can be reused by the {3}.", new object[]
					{
						"PawnPath",
						this.pathsCreated,
						num,
						"PawnPathPool"
					}), 664788);
				}
			}
			return pawnPath;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0003AC86 File Offset: 0x00038E86
		public void ReturnPath(PawnPath path)
		{
			this.paths.Push(path);
		}

		// Token: 0x0400071C RID: 1820
		private readonly Map map;

		// Token: 0x0400071D RID: 1821
		private int pathsCreated;

		// Token: 0x0400071E RID: 1822
		private readonly Stack<PawnPath> paths = new Stack<PawnPath>(64);
	}
}
