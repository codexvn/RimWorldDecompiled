using System;

namespace Verse.AI
{
	// Token: 0x02000FF3 RID: 4083
	public class ThinkNode_ConditionalDoingJob : ThinkNode_Conditional
	{
		// Token: 0x06006472 RID: 25714 RVA: 0x001FD5B4 File Offset: 0x001FB7B4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CurJobDef == this.jobDef;
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x001FD5C4 File Offset: 0x001FB7C4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalDoingJob thinkNode_ConditionalDoingJob = (ThinkNode_ConditionalDoingJob)base.DeepCopy(resolve);
			thinkNode_ConditionalDoingJob.jobDef = this.jobDef;
			return thinkNode_ConditionalDoingJob;
		}

		// Token: 0x040047D4 RID: 18388
		public JobDef jobDef;
	}
}
