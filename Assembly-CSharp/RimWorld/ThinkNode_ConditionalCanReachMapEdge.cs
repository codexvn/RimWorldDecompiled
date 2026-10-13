using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A9 RID: 6057
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06008D9D RID: 36253 RVA: 0x002A0E06 File Offset: 0x0029F006
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCanReachMapEdge thinkNode_ConditionalCanReachMapEdge = (ThinkNode_ConditionalCanReachMapEdge)base.DeepCopy(resolve);
			thinkNode_ConditionalCanReachMapEdge.allowFlyOutInstead = this.allowFlyOutInstead;
			return thinkNode_ConditionalCanReachMapEdge;
		}

		// Token: 0x06008D9E RID: 36254 RVA: 0x002A0E20 File Offset: 0x0029F020
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge() || (this.allowFlyOutInstead && (pawn.RaceProps.canLeaveMapFlying && !pawn.Position.Roofed(pawn.Map) && pawn.Faction != Faction.OfPlayer && pawn.flight.CanEverFly) && !pawn.IsQuestLodger());
		}

		// Token: 0x04005BCE RID: 23502
		public bool allowFlyOutInstead;
	}
}
