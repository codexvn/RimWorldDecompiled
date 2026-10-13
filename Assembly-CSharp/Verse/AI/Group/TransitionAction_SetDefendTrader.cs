using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010B8 RID: 4280
	public class TransitionAction_SetDefendTrader : TransitionAction
	{
		// Token: 0x060068EA RID: 26858 RVA: 0x00211F3C File Offset: 0x0021013C
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			Pawn pawn = TraderCaravanUtility.FindTrader(lordToil_DefendPoint.lord);
			if (pawn != null)
			{
				lordToil_DefendPoint.SetDefendPoint(pawn.Position);
				return;
			}
			IEnumerable<Pawn> source = from x in lordToil_DefendPoint.lord.ownedPawns
			where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
			select x;
			if (source.Any<Pawn>())
			{
				lordToil_DefendPoint.SetDefendPoint(source.RandomElement<Pawn>().Position);
				return;
			}
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
