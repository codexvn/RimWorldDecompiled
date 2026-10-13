using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027CF RID: 10191
	public class RitualStageAction_ReleaseLanterns : RitualStageTickActionMaker
	{
		// Token: 0x0600DF14 RID: 57108 RVA: 0x004191AE File Offset: 0x004173AE
		public override IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage)
		{
			StageEndTrigger_DurationPercentage stageEndTrigger_DurationPercentage = (StageEndTrigger_DurationPercentage)stage.endTriggers.FirstOrFallback((StageEndTrigger e) => e is StageEndTrigger_DurationPercentage, null);
			if (stageEndTrigger_DurationPercentage == null)
			{
				yield break;
			}
			int durationTicks = (int)(stageEndTrigger_DurationPercentage.percentage * (float)ritual.DurationTicks);
			foreach (Pawn pawn in ritual.assignments.Participants)
			{
				yield return new ActionOnTick_ReleaseLantern
				{
					tick = durationTicks - this.preliminaryTicks.RandomInRange,
					pawn = pawn,
					woodCost = this.woodCost
				};
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04009631 RID: 38449
		public int woodCost = 4;

		// Token: 0x04009632 RID: 38450
		public IntRange preliminaryTicks;
	}
}
