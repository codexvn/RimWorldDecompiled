using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010DB RID: 4315
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x06006938 RID: 26936 RVA: 0x00212B4C File Offset: 0x00210D4C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.FactionRelationsChanged)
			{
				FactionRelationKind? previousRelationKind = signal.previousRelationKind;
				FactionRelationKind factionRelationKind = FactionRelationKind.Hostile;
				return (previousRelationKind.GetValueOrDefault() == factionRelationKind & previousRelationKind != null) && lord.faction != null && !lord.faction.HostileTo(Faction.OfPlayer);
			}
			return false;
		}
	}
}
