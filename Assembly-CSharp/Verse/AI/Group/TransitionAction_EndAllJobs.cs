using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010B2 RID: 4274
	public class TransitionAction_EndAllJobs : TransitionAction
	{
		// Token: 0x060068DD RID: 26845 RVA: 0x00211D1C File Offset: 0x0020FF1C
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
