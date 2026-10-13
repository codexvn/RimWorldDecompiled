using System;

namespace Verse.AI
{
	// Token: 0x02000FDE RID: 4062
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x0600643F RID: 25663 RVA: 0x001FD1F1 File Offset: 0x001FB3F1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x06006440 RID: 25664 RVA: 0x001FD20B File Offset: 0x001FB40B
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.Satisfied(pawn) == !this.invert)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06006441 RID: 25665
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x040047CC RID: 18380
		public bool invert;
	}
}
