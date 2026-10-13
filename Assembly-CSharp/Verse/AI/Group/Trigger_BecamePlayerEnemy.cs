using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010DC RID: 4316
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x0600693A RID: 26938 RVA: 0x00212B9E File Offset: 0x00210D9E
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
