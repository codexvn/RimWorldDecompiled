using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036B8 RID: 14008
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x06013753 RID: 79699 RVA: 0x005C89E8 File Offset: 0x005C6BE8
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && (!ModsConfig.IdeologyActive || !corpse.InnerPawn.IsSlave) && (!ModsConfig.AnomalyActive || !(t is UnnaturalCorpse)) && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
