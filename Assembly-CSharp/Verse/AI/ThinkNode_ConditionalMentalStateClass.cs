using System;

namespace Verse.AI
{
	// Token: 0x02000FE0 RID: 4064
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06006446 RID: 25670 RVA: 0x001FD272 File Offset: 0x001FB472
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x001FD28C File Offset: 0x001FB48C
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsInstanceOfType(mentalState);
		}

		// Token: 0x040047CE RID: 18382
		public Type stateClass;
	}
}
