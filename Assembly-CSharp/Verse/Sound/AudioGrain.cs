using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DF9 RID: 3577
	public abstract class AudioGrain
	{
		// Token: 0x06005AF1 RID: 23281
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}
