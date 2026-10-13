using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AD2 RID: 6866
	public class RecordWorker
	{
		// Token: 0x060099A5 RID: 39333 RVA: 0x002CBDFC File Offset: 0x002C9FFC
		public virtual bool ShouldMeasureTimeNow(Pawn pawn)
		{
			if (this.def.measuredTimeJobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob == null)
			{
				return false;
			}
			for (int i = 0; i < this.def.measuredTimeJobs.Count; i++)
			{
				if (curJob.def == this.def.measuredTimeJobs[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04006928 RID: 26920
		public RecordDef def;
	}
}
