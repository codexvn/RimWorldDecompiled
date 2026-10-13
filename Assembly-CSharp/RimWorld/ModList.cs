using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200329A RID: 12954
	public class ModList : IExposable
	{
		// Token: 0x0601239D RID: 74653 RVA: 0x0055B9AB File Offset: 0x00559BAB
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.ids, "ids", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.names, "names", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0400BDB0 RID: 48560
		public string fileName;

		// Token: 0x0400BDB1 RID: 48561
		public List<string> ids;

		// Token: 0x0400BDB2 RID: 48562
		public List<string> names;
	}
}
