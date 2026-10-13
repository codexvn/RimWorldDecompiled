using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD0 RID: 6864
	public class RecordDef : Def
	{
		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x060099A3 RID: 39331 RVA: 0x002CBDA7 File Offset: 0x002C9FA7
		public RecordWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecordWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0400691F RID: 26911
		public RecordType type;

		// Token: 0x04006920 RID: 26912
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x04006921 RID: 26913
		public List<JobDef> measuredTimeJobs;

		// Token: 0x04006922 RID: 26914
		public int displayOrder = 99999;

		// Token: 0x04006923 RID: 26915
		[Unsaved(false)]
		private RecordWorker workerInt;
	}
}
