using System;

namespace RimWorld
{
	// Token: 0x0200174D RID: 5965
	public class RaidStrategyWorker_ImmediateAttackFriendly : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x06008C10 RID: 35856 RVA: 0x0029B331 File Offset: 0x00299531
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
