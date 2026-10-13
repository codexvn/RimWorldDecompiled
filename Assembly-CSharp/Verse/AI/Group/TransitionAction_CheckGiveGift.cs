using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010BB RID: 4283
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x060068F1 RID: 26865 RVA: 0x00212044 File Offset: 0x00210244
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.GiveRandomGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
