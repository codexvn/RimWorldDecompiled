using System;

namespace Verse.AI.Group
{
	// Token: 0x020010B7 RID: 4279
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x060068E8 RID: 26856 RVA: 0x00211F14 File Offset: 0x00210114
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
