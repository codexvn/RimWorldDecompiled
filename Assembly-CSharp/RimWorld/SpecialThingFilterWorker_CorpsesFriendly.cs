using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036BC RID: 14012
	public class SpecialThingFilterWorker_CorpsesFriendly : SpecialThingFilterWorker
	{
		// Token: 0x0601375B RID: 79707 RVA: 0x005C8B14 File Offset: 0x005C6D14
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
