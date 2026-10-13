using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C0 RID: 6080
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		// Token: 0x06008DD1 RID: 36305 RVA: 0x002A13A0 File Offset: 0x0029F5A0
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			Map map = pawn.Map;
			return !map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map, true, false);
		}
	}
}
