using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023BD RID: 9149
	public class RecordWorker_TimeAsChildInColony : RecordWorker
	{
		// Token: 0x0600C699 RID: 50841 RVA: 0x0039E10C File Offset: 0x0039C30C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.Spawned && pawn.RaceProps.Humanlike && pawn.Map.IsPlayerHome && pawn.DevelopmentalStage.Juvenile();
		}
	}
}
