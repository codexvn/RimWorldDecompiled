using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FE1 RID: 4065
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x06006449 RID: 25673 RVA: 0x001FD2B1 File Offset: 0x001FB4B1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x001FD2CB File Offset: 0x001FB4CB
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x040047CF RID: 18383
		public List<MentalStateDef> states;
	}
}
