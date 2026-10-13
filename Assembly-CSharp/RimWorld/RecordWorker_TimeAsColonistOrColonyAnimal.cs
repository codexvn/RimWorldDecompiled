using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023BB RID: 9147
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x0600C695 RID: 50837 RVA: 0x0039E0C1 File Offset: 0x0039C2C1
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && !pawn.HasExtraHomeFaction(null) && !pawn.IsSlave && !pawn.IsPrisoner;
		}
	}
}
