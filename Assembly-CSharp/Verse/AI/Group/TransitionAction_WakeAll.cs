using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010B4 RID: 4276
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x060068E1 RID: 26849 RVA: 0x00211E10 File Offset: 0x00210010
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				RestUtility.WakeUp(ownedPawns[i], true);
			}
			List<Building> ownedBuildings = trans.target.lord.ownedBuildings;
			for (int j = 0; j < ownedBuildings.Count; j++)
			{
				CompCanBeDormant compCanBeDormant = ownedBuildings[j].TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.WakeUpWithDelay();
				}
			}
		}
	}
}
