using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3E RID: 6974
	public sealed class CustomXenogermDatabase : IExposable
	{
		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x06009BC5 RID: 39877 RVA: 0x002D29D3 File Offset: 0x002D0BD3
		public List<CustomXenogerm> CustomXenogermsForReading
		{
			get
			{
				return this.customXenogerms;
			}
		}

		// Token: 0x06009BC6 RID: 39878 RVA: 0x002D29DC File Offset: 0x002D0BDC
		public void Add(CustomXenogerm customXenogerm)
		{
			if (!ModLister.CheckBiotech("Custom xenogerm database"))
			{
				return;
			}
			this.customXenogerms.RemoveAll((CustomXenogerm c) => c.name == customXenogerm.name);
			this.customXenogerms.Add(customXenogerm);
		}

		// Token: 0x06009BC7 RID: 39879 RVA: 0x002D2A2C File Offset: 0x002D0C2C
		public bool Remove(CustomXenogerm customXenogerm)
		{
			return this.customXenogerms.Remove(customXenogerm);
		}

		// Token: 0x06009BC8 RID: 39880 RVA: 0x002D2A3A File Offset: 0x002D0C3A
		public void ExposeData()
		{
			Scribe_Collections.Look<CustomXenogerm>(ref this.customXenogerms, "customXenogerms", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.customXenogerms == null)
			{
				this.customXenogerms = new List<CustomXenogerm>();
			}
		}

		// Token: 0x04006C2A RID: 27690
		private List<CustomXenogerm> customXenogerms = new List<CustomXenogerm>();
	}
}
