using System;

namespace Verse.AI
{
	// Token: 0x02000FE4 RID: 4068
	public class ThinkNode_ConditionalCapableOfWorkTag : ThinkNode_Conditional
	{
		// Token: 0x06006450 RID: 25680 RVA: 0x001FD2FE File Offset: 0x001FB4FE
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCapableOfWorkTag thinkNode_ConditionalCapableOfWorkTag = (ThinkNode_ConditionalCapableOfWorkTag)base.DeepCopy(resolve);
			thinkNode_ConditionalCapableOfWorkTag.workTags = this.workTags;
			return thinkNode_ConditionalCapableOfWorkTag;
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x001FD318 File Offset: 0x001FB518
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(this.workTags);
		}

		// Token: 0x040047D0 RID: 18384
		public WorkTags workTags;
	}
}
