using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002899 RID: 10393
	public abstract class StartingPawnCount : IExposable
	{
		// Token: 0x1700229C RID: 8860
		// (get) Token: 0x0600E38A RID: 58250
		public abstract string Summary { get; }

		// Token: 0x0600E38B RID: 58251 RVA: 0x00427F5A File Offset: 0x0042615A
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
			Scribe_Values.Look<bool>(ref this.requiredAtStart, "requiredAtStart", false, false);
		}

		// Token: 0x04009861 RID: 39009
		public int count = 1;

		// Token: 0x04009862 RID: 39010
		public bool requiredAtStart;

		// Token: 0x04009863 RID: 39011
		[Unsaved(false)]
		public string countBuffer;
	}
}
