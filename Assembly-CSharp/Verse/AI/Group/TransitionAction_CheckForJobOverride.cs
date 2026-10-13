using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010BA RID: 4282
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x060068EF RID: 26863 RVA: 0x00211FF0 File Offset: 0x002101F0
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i].CurJob != null)
				{
					ownedPawns[i].jobs.CheckForJobOverride(0f, true);
				}
			}
		}
	}
}
