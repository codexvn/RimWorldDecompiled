using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B40 RID: 6976
	public sealed class CustomXenotypeDatabase : IExposable
	{
		// Token: 0x06009BCC RID: 39884 RVA: 0x002D2A98 File Offset: 0x002D0C98
		public void ExposeData()
		{
			Scribe_Collections.Look<CustomXenotype>(ref this.customXenotypes, "customXenotypes", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.customXenotypes == null)
			{
				this.customXenotypes = new List<CustomXenotype>();
			}
		}

		// Token: 0x04006C2C RID: 27692
		public List<CustomXenotype> customXenotypes = new List<CustomXenotype>();
	}
}
