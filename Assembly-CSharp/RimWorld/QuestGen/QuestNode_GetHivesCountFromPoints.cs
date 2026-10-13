using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A4A RID: 14922
	public class QuestNode_GetHivesCountFromPoints : QuestNode
	{
		// Token: 0x0601469B RID: 83611 RVA: 0x00618DE1 File Offset: 0x00616FE1
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0601469C RID: 83612 RVA: 0x00618DEB File Offset: 0x00616FEB
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0601469D RID: 83613 RVA: 0x00618DF8 File Offset: 0x00616FF8
		private void SetVars(Slate slate)
		{
			float num = slate.Get<float>("points", 0f, false);
			num *= IncidentWorker_Infestation.PointsFactorCurve.Evaluate(num);
			int num2 = Mathf.RoundToInt(num / 220f);
			if (num2 < 1)
			{
				num2 = 1;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), num2, false);
		}

		// Token: 0x0400E1B1 RID: 57777
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
