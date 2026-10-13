using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A3A RID: 14906
	public class QuestNode_GetColonistCountFromColonyPercentage : QuestNode
	{
		// Token: 0x06014666 RID: 83558 RVA: 0x006183B0 File Offset: 0x006165B0
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014667 RID: 83559 RVA: 0x006183C0 File Offset: 0x006165C0
		private void SetVars(Slate slate)
		{
			string value = this.storeAs.GetValue(slate);
			float minAgeResolved = this.minAge.GetValue(slate).GetValueOrDefault();
			int num = PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => this.ColonistCounts(c, minAgeResolved));
			int var = Mathf.Clamp((int)((float)num * this.colonyPercentage.GetValue(slate)), 1, num - 1);
			slate.Set<int>(value, var, false);
		}

		// Token: 0x06014668 RID: 83560 RVA: 0x0061843C File Offset: 0x0061663C
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			float num = (float)this.mustHaveFreeColonistsAvailableCount.GetValue(slate);
			if (num > 0f)
			{
				float minAgeResolved = this.minAge.GetValue(slate).GetValueOrDefault();
				return (float)PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => this.ColonistCounts(c, minAgeResolved)) >= num;
			}
			return true;
		}

		// Token: 0x06014669 RID: 83561 RVA: 0x006184AC File Offset: 0x006166AC
		private bool ColonistCounts(Pawn pawn, float minAge)
		{
			return !pawn.IsQuestLodger() && pawn.ageTracker.AgeBiologicalYearsFloat >= minAge;
		}

		// Token: 0x0400E17B RID: 57723
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E17C RID: 57724
		public SlateRef<float> colonyPercentage;

		// Token: 0x0400E17D RID: 57725
		public SlateRef<int> mustHaveFreeColonistsAvailableCount;

		// Token: 0x0400E17E RID: 57726
		public SlateRef<float?> minAge;
	}
}
