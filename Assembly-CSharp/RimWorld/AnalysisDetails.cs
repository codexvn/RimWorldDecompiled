using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3A RID: 6970
	public class AnalysisDetails : IExposable
	{
		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x06009BB2 RID: 39858 RVA: 0x002D2771 File Offset: 0x002D0971
		public bool Satisfied
		{
			get
			{
				return this.timesDone >= this.required;
			}
		}

		// Token: 0x06009BB3 RID: 39859 RVA: 0x002D2784 File Offset: 0x002D0984
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.required, "required", 0, false);
			Scribe_Values.Look<int>(ref this.timesDone, "timesDone", 0, false);
		}

		// Token: 0x04006C21 RID: 27681
		public int id;

		// Token: 0x04006C22 RID: 27682
		public int required;

		// Token: 0x04006C23 RID: 27683
		public int timesDone;
	}
}
