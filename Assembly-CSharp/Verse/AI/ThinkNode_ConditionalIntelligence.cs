using System;

namespace Verse.AI
{
	// Token: 0x02000FE7 RID: 4071
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x06006458 RID: 25688 RVA: 0x001FD3E6 File Offset: 0x001FB5E6
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x001FD400 File Offset: 0x001FB600
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}

		// Token: 0x040047D2 RID: 18386
		public Intelligence minIntelligence = Intelligence.ToolUser;
	}
}
