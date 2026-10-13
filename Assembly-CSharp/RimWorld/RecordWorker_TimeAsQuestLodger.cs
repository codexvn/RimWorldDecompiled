using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023BC RID: 9148
	public class RecordWorker_TimeAsQuestLodger : RecordWorker
	{
		// Token: 0x0600C697 RID: 50839 RVA: 0x0039E0F4 File Offset: 0x0039C2F4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.HasExtraHomeFaction(null);
		}
	}
}
