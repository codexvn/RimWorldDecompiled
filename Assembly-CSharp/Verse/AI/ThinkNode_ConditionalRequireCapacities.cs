using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FE5 RID: 4069
	public class ThinkNode_ConditionalRequireCapacities : ThinkNode_Conditional
	{
		// Token: 0x06006453 RID: 25683 RVA: 0x001FD329 File Offset: 0x001FB529
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRequireCapacities thinkNode_ConditionalRequireCapacities = (ThinkNode_ConditionalRequireCapacities)base.DeepCopy(resolve);
			thinkNode_ConditionalRequireCapacities.requiredCapacities = this.requiredCapacities;
			return thinkNode_ConditionalRequireCapacities;
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x001FD344 File Offset: 0x001FB544
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.health != null && pawn.health.capacities != null)
			{
				foreach (PawnCapacityDef capacity in this.requiredCapacities)
				{
					if (!pawn.health.capacities.CapableOf(capacity))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x040047D1 RID: 18385
		public List<PawnCapacityDef> requiredCapacities;
	}
}
