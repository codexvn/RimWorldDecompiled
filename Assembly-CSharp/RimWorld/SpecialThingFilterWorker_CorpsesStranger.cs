using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036BA RID: 14010
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x06013757 RID: 79703 RVA: 0x005C8AB0 File Offset: 0x005C6CB0
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && (!ModsConfig.AnomalyActive || !(t is UnnaturalCorpse)) && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
