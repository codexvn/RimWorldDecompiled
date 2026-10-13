using System;

namespace Verse
{
	// Token: 0x0200012B RID: 299
	public struct ProfilerBlock : IDisposable
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x000026EA File Offset: 0x000008EA
		public ProfilerBlock(string blockName)
		{
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x000026EA File Offset: 0x000008EA
		public void Dispose()
		{
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002BFFB File Offset: 0x0002A1FB
		public static ProfilerBlock Scope(string blockName)
		{
			return new ProfilerBlock(blockName);
		}
	}
}
