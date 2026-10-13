using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036B9 RID: 14009
	public class SpecialThingFilterWorker_CorpsesSlave : SpecialThingFilterWorker
	{
		// Token: 0x06013755 RID: 79701 RVA: 0x005C8A5C File Offset: 0x005C6C5C
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.IsSlave;
		}
	}
}
