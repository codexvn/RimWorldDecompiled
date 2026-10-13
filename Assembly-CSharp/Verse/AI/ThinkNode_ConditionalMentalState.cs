using System;

namespace Verse.AI
{
	// Token: 0x02000FDF RID: 4063
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x06006443 RID: 25667 RVA: 0x001FD22D File Offset: 0x001FB42D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x001FD248 File Offset: 0x001FB448
		protected override bool Satisfied(Pawn pawn)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef != null && mentalStateDef == this.state;
		}

		// Token: 0x040047CD RID: 18381
		public MentalStateDef state;
	}
}
