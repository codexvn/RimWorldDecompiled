using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017CE RID: 6094
	public class ThinkNode_ConditionalAimingAtCount : ThinkNode_Conditional
	{
		// Token: 0x06008DF0 RID: 36336 RVA: 0x002A194C File Offset: 0x0029FB4C
		protected override bool Satisfied(Pawn pawn)
		{
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			int num = 0;
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (potentialTargetsFor[i].TargetCurrentlyAimingAt == pawn)
				{
					num++;
					if (num >= this.thresholdAiming)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005BE1 RID: 23521
		public int thresholdAiming = 1;
	}
}
