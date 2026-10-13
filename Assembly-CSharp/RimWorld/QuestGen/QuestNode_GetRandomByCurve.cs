using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A70 RID: 14960
	public class QuestNode_GetRandomByCurve : QuestNode
	{
		// Token: 0x06014725 RID: 83749 RVA: 0x0061BACB File Offset: 0x00619CCB
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014726 RID: 83750 RVA: 0x0061BAD5 File Offset: 0x00619CD5
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014727 RID: 83751 RVA: 0x0061BAE4 File Offset: 0x00619CE4
		private void SetVars(Slate slate)
		{
			float num = Rand.ByCurve(this.curve.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			if (this.min.GetValue(slate) != null)
			{
				num = Mathf.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Mathf.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400E240 RID: 57920
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E241 RID: 57921
		public SlateRef<SimpleCurve> curve;

		// Token: 0x0400E242 RID: 57922
		public SlateRef<bool> roundRandom;

		// Token: 0x0400E243 RID: 57923
		public SlateRef<float?> min;

		// Token: 0x0400E244 RID: 57924
		public SlateRef<float?> max;
	}
}
