using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001797 RID: 6039
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x06008D77 RID: 36215 RVA: 0x002A0B54 File Offset: 0x0029ED54
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x06008D78 RID: 36216 RVA: 0x002A0B6E File Offset: 0x0029ED6E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}

		// Token: 0x04005BCD RID: 23501
		private TrainableDef trainable;
	}
}
