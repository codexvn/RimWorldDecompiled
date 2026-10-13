using System;

namespace Verse
{
	// Token: 0x0200015D RID: 349
	public struct RandBlock : IDisposable
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x0002FF2D File Offset: 0x0002E12D
		public RandBlock(int seed)
		{
			Rand.PushState(seed);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0002FF35 File Offset: 0x0002E135
		public void Dispose()
		{
			Rand.PopState();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0002FF3C File Offset: 0x0002E13C
		public static RandBlock Seed(int seed)
		{
			return new RandBlock(seed);
		}
	}
}
