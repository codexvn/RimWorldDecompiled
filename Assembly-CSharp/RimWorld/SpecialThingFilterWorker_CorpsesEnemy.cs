using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036BD RID: 14013
	public class SpecialThingFilterWorker_CorpsesEnemy : SpecialThingFilterWorker
	{
		// Token: 0x0601375D RID: 79709 RVA: 0x005C8B40 File Offset: 0x005C6D40
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
