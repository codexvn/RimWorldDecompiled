using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200016D RID: 365
	public class PawnTags : IExposable
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x000321A8 File Offset: 0x000303A8
		public bool Contains(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000321B6 File Offset: 0x000303B6
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04000595 RID: 1429
		public List<string> tags;
	}
}
